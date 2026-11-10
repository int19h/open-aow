using System.Globalization;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Platform;
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow;

internal sealed class FrameFixture
{
    private string? fileLocalTime;

    internal static DateTime FileStampUtc(DateTime localTime, TimeSpan offset) =>
        new DateTimeOffset(DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified), offset).UtcDateTime;

    private readonly Dictionary<string, string> defaults = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> values = new(StringComparer.Ordinal);
    private readonly Dictionary<string, object> settings = new(StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> booleans = ["Skip Intro", "Debug Mode", "Allow Resolution Switches", "Music Enabled", "Sound Effects Enabled", "Shadows Enabled", "Surface In VMem"];

    public static int Run(string path, string name, string gameRoot, string output, int? frameLimit = null)
    {
        gameRoot = Path.GetFullPath(gameRoot);
        output = Path.GetFullPath(output);
        var fixture = Load(path, name);
        string scene = fixture.Get("scene");
        bool presentationClock = fixture.defaults.GetValueOrDefault("frame_clock") == "presentation" || fixture.values.GetValueOrDefault("frame_clock") == "presentation";
        bool worldCapture = fixture.values.GetValueOrDefault("capture_clock") == "world";
        bool dayCapture = fixture.values.GetValueOrDefault("capture_clock") == "day";
        bool titleCapture = fixture.values.GetValueOrDefault("capture_clock") == "title";
        bool combatCapture = fixture.values.GetValueOrDefault("capture_clock") == "combat";
        bool afterCombatCapture = fixture.values.GetValueOrDefault("capture_clock") == "after-combat";
        bool tactical = fixture.values.GetValueOrDefault("action") == "tactical";
        var clicks = fixture.Clicks();
        int frame = frameLimit ?? int.Parse(fixture.Get("frame"), CultureInfo.InvariantCulture);
        if (frame < 1) throw new ArgumentOutOfRangeException(nameof(frameLimit));
        int[] checkpoints = fixture.values.TryGetValue("checkpoints", out string? checkpointText) ? JsonSerializer.Deserialize<int[]>(checkpointText)! : [];
        if (checkpoints.Length > 30 || checkpoints.Any(f => f < 1 || f >= frame) ||
            !checkpoints.SequenceEqual(checkpoints.Distinct().Order()) || checkpoints.Length > 0 && !afterCombatCapture)
            throw new InvalidDataException("Checkpoints require sorted earlier frames on the after-combat clock");
        var capturedCheckpoints = new HashSet<int>();
        Snapshot[] snapshots = fixture.values.TryGetValue("snapshots", out string? snapshotText)
            ? JsonSerializer.Deserialize<Snapshot[]>(Regex.Replace(snapshotText, @"(\w+)\s*=", "\"$1\":"), new JsonSerializerOptions { PropertyNameCaseInsensitive = true })! : [];
        if (snapshots.Length > 8 || snapshots.Any(s => !Regex.IsMatch(s.Name, "^[a-z0-9-]+$") || s.Day < 1 || s.Frame < 1 ||
            !clicks.Any(c => c.Clock == "day" && c.Day == s.Day && c.At == s.Frame)) ||
            snapshots.Select(s => s.Name).Distinct().Count() != snapshots.Length || snapshots.Length > 0 && checkpoints.Length > 0)
            throw new InvalidDataException("Snapshots require distinct safe names and matching day-clock inputs");
        var capturedSnapshots = new HashSet<string>();
        Snapshot? pendingSnapshot = null;
        var size = fixture.Pair("size");
        var mouse = fixture.Pair("mouse");
        var now = DateTime.Parse(fixture.Get("local_time"), CultureInfo.InvariantCulture);
        DateTime? fileStampUtc = fixture.fileLocalTime is null ? null :
            FileStampUtc(DateTime.Parse(fixture.fileLocalTime, CultureInfo.InvariantCulture), DateTimeOffset.Now.Offset);
        var display = new HeadlessDisplayHost(output, size.X, size.Y, frame) { Mouse = mouse, FixtureFrame = frame };
        Directory.CreateDirectory(output);
        using var diagnostics = FrameDiagnostics.Start(output);
        using var trace = new StreamWriter(Path.Combine(output, "exceptions.log")) { NewLine = "\n", AutoFlush = true };
        void Record(object? sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs error) => trace.WriteLine(error.Exception + "\n" + new System.Diagnostics.StackTrace(1, true));
        AppDomain.CurrentDomain.FirstChanceException += Record;
        string? saveDirectory = fixture.values.GetValueOrDefault("save_directory");
        if (saveDirectory is not null)
        {
            string directory = Path.Combine(output, "user", saveDirectory);
            if (Directory.Exists(directory))
                foreach (string saved in Directory.EnumerateFiles(directory))
                {
                    if (Path.GetFileName(saved) != "roundtrip.asg") throw new InvalidDataException("Unexpected fixture save product " + saved);
                    File.Delete(saved);
                }
        }
        var host = new FixtureHost(gameRoot, Path.Combine(output, "user"), fixture.settings, new FixtureClock(now), saveDirectory);
        RtlUnit.InstallHost(host);
        RtlUnit.SuppressRandomize = true;
        // Native startup resource sequences load with seed zero, before the fixed Randomize seed.
        RtlUnit.RandSeed = 0;
        RtlUnit.RandomizeSeed = int.Parse(fixture.Get("seed"), CultureInfo.InvariantCulture);
        FrameFixtureInputs.AddressAnimationPhase = int.Parse(fixture.Get("address_animation_phase"), CultureInfo.InvariantCulture);
        FrameFixtureInputs.WaterVariantPhase = fixture.values.TryGetValue("water_variant_phase", out string? waterPhase) ? int.Parse(waterPhase, CultureInfo.InvariantCulture) : null;
        FrameFixtureInputs.Tactical = tactical;
        // The native capture uses positive FPS below 15 at its six strategic moves.
        // Reproduce that movement tier without making animation depend on host speed.
        FrameFixtureInputs.MeasuredFrameRate = fixture.defaults.TryGetValue("measured_frame_rate", out var measuredRate) ? int.Parse(measuredRate, CultureInfo.InvariantCulture) : tactical ? 9 : null;
        string argument = "";
        string? input = null;
        if (scene != "title")
        {
            input = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path))!, "..", "..", "tools", "oracle", "obj", "frames", "inputs", scene + ".asg");
            if (!File.Exists(input)) throw new FileNotFoundException("Prepared fixture input missing", input);
            File.Copy(input, Path.Combine(output, "user", scene + ".asg"), true);
            argument = Path.Combine(gameRoot, scene + ".asg");
        }
        int applied = 0;
        bool complete = false;
        bool tacticalReady = false;
        int observedDay = -1, dayOrigin = 0;
        long titleOrigin = 0;
        bool sawCombat = false;
        int lastCombatWorldFrame = 0;
        void ObserveDay()
        {
            if (Aow.Game.World.Map.AoWHSMap.Current is not null) titleOrigin = display.Frame;
            if (Aow.Game.World.Map.AoWHSMap.Current?.FCombatMap is not null)
            {
                sawCombat = true;
                lastCombatWorldFrame = Aow.Game.World.Map.AoWHSMap.Current.Frame;
            }
            if (Aow.Game.World.Map.AoWHSMap.Current is { } map && observedDay != map.FDay)
            {
                observedDay = map.FDay;
                dayOrigin = map.Frame;
            }
        }
        int DayFrame() => Aow.Game.World.Map.AoWHSMap.Current is { } map ? map.Frame - dayOrigin + 1 : -1;
        int AfterCombatFrame() => sawCombat && Aow.Game.World.Map.AoWHSMap.Current is { FCombatMap: null } map ? map.Frame - lastCombatWorldFrame : -1;
        long ScriptFrame(Click click)
        {
            string clock = click.Clock ?? fixture.defaults.GetValueOrDefault("frame_clock", "world");
            var map = Aow.Game.World.Map.AoWHSMap.Current;
            return clock switch
            {
                "presentation" => display.Frame,
                "after-combat" => AfterCombatFrame(),
                "combat" => map?.FCombatMap?.Frame ?? -1,
                "title" => map is null ? display.Frame - titleOrigin : -1,
                _ => map?.FDay == (click.Day ?? int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture))
                    ? clock == "day" ? DayFrame() : map.Frame : -1
            };
        }
        using var script = new StreamWriter(Path.Combine(output, "script.jsonl")) { NewLine = "\n", AutoFlush = true };
        try
        {
            using var form = Aow.Platform.TMainForm.Create(display, argument);
            if (saveDirectory is not null)
                Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FDirectory = Path.Combine(gameRoot, saveDirectory).Replace('/', '\\') + "\\";
            FrameFixtureInputs.TacticalInitialized = () =>
            {
                if (Aow.Game.World.Map.AoWHSMap.Current?.FCombatMap is not Aow.Game.Combat.AoWCombatMap combat) return;
                combat.FPlaySounds = true;
                combat.SetSeatedCombatPlayer(0);
                Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.MinClick();
                tacticalReady = true;
            };
            bool AtTarget()
            {
                ObserveDay();
                if (applied != clicks.Count) return false;
                return afterCombatCapture ? AfterCombatFrame() == frame :
                combatCapture ? Aow.Game.World.Map.AoWHSMap.Current?.FCombatMap?.Frame == frame :
                titleCapture ? Aow.Game.World.Map.AoWHSMap.Current is null && display.Frame - titleOrigin == frame :
                dayCapture ? Aow.Game.World.Map.AoWHSMap.Current?.FDay == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) && DayFrame() == frame :
                !worldCapture && (scene == "title" || presentationClock) ? display.Frame == frame :
                tactical ? tacticalReady && Aow.Game.World.Map.AoWHSMap.Current.FCombatMap.Frame == frame :
                Aow.Game.World.Map.AoWHSMap.Current is { } map && map.FDay == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) && map.Frame == frame;
            }
            display.CaptureWhen = () =>
            {
                ObserveDay();
                pendingSnapshot = snapshots.FirstOrDefault(s => Aow.Game.World.Map.AoWHSMap.Current?.FDay == s.Day && DayFrame() == s.Frame && !capturedSnapshots.Contains(s.Name));
                display.CaptureName = pendingSnapshot is null ? null : "snapshot-" + pendingSnapshot.Name;
                if (pendingSnapshot is not null)
                {
                    display.FixtureFrame = pendingSnapshot.Frame;
                    return true;
                }
                int current = AfterCombatFrame();
                if (checkpoints.Contains(current) && capturedCheckpoints.Add(current))
                {
                    display.FixtureFrame = current;
                    return true;
                }
                display.FixtureFrame = frame;
                if (fixture.values.GetValueOrDefault("diagnostics") == "true" && display.Frame % 100 == 0 && !AtTarget())
                {
                    display.CaptureName = "progress";
                    return true;
                }
                return AtTarget();
            };
            display.AfterPresent = () =>
            {
                if (pendingSnapshot is { } snapshot)
                {
                    diagnostics?.Snapshot(snapshot.Name);
                    Aow.Game.World.Map.AoWHSMap.Current.FPlayerControl.AutoSave();
                    File.Copy(Path.Combine(output, "user", "Save", "autosave.asg"), Path.Combine(output, "snapshot-" + snapshot.Name + ".asg"), true);
                    capturedSnapshots.Add(snapshot.Name);
                    pendingSnapshot = null;
                }
                if (display.Frame % 100 == 0)
                    File.WriteAllText(Path.Combine(output, "progress.json"), JsonSerializer.Serialize(new
                    {
                        presentation = display.Frame,
                        applied,
                        day = Aow.Game.World.Map.AoWHSMap.Current?.FDay,
                        map_frame = Aow.Game.World.Map.AoWHSMap.Current?.Frame,
                        seat = Aow.Game.World.Map.AoWHSMap.Current?.FSeatedPlayer,
                        combat_frame = Aow.Game.World.Map.AoWHSMap.Current?.FCombatMap?.Frame
                    }) + "\n");
                if (AtTarget()) { complete = true; form.Display.StopUpdateFrameLoop(); return; }
                if (applied < clicks.Count)
                {
                    var click = clicks[applied];
                    long scriptFrame = ScriptFrame(click);
                    if (scriptFrame > click.At) throw new InvalidOperationException("Missed scripted frame " + click.At);
                    if (scriptFrame == click.At)
                    {
                        if (saveDirectory is not null)
                            foreach (string saved in Directory.EnumerateFiles(Path.Combine(output, "user", saveDirectory)))
                                if (fileStampUtc is DateTime stamp) File.SetLastWriteTimeUtc(saved, stamp);
                                else File.SetLastWriteTime(saved, now);
                        var manager = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager;
                        int inputX = click.X, inputY = click.Y;
                        if (click.Hx is int hx && click.Hy is int hy)
                        {
                            var mapWindow = Aow.Game.Ui.Dialogs.DialogHosts.MWindow;
                            Aow.Game.World.Map.HexGeometry.HXtoHP(hx, hy, ref inputX, ref inputY);
                            inputX += 28 + mapWindow.MapWindow.WinLeft - mapWindow.MapViewer.SceneX;
                            inputY += 28 + mapWindow.MapWindow.WinTop - mapWindow.MapViewer.SceneY;
                            if (!mapWindow.InsideMap(inputX, inputY)) throw new InvalidOperationException($"Script hex {hx},{hy} is outside the map view at {inputX},{inputY}");
                        }
                        if (click.KeyDown is int keyDown)
                            for (int i = 0; i < click.Repeat; i++) form.KeyDown((ushort)keyDown, default);
                        else if (click.Key is int key) { byte character = (byte)key; manager.CheckKeyPress(ref character); }
                        else
                        {
                            manager.CheckMouseMove(default, inputX, inputY);
                            if (!click.MoveOnly)
                            {
                                var shift = click.DoubleUi ? Set<ShiftState>.Of(ShiftState.Double) : default;
                                manager.CheckMouseDown((MouseButton)click.Button, shift, inputX, inputY);
                                manager.CheckMouseUp((MouseButton)click.Button, default, inputX, inputY);
                            }
                        }
                        if (click.Double || click.DoubleUi) form.DoubleClick();
                        script.WriteLine(JsonSerializer.Serialize(new
                        {
                            script_frame = scriptFrame,
                            X = inputX,
                            Y = inputY,
                            click.Hx,
                            click.Hy,
                            click.Button,
                            click.Double,
                            click.DoubleUi,
                            click.Key,
                            click.KeyDown,
                            click.Repeat,
                            click.Clock,
                            click.Day,
                            selected_combat_unit = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit?.FUnit.FID
                        }));
                        applied++;
                    }
                }
                if (display.Frame >= 10000) throw new TimeoutException("Fixture target not reached");
            };
            form.Run(scene == "title" && !worldCapture && !dayCapture ? frame : 0);
            if (scene == "title" && !worldCapture && !dayCapture) complete = display.Frame == frame;
            if (!complete || applied != clicks.Count) throw new InvalidOperationException("Fixture ended before all inputs and the target frame");
            if (!capturedCheckpoints.SetEquals(checkpoints)) throw new InvalidOperationException("Fixture missed an intermediate checkpoint");
            if (!capturedSnapshots.SetEquals(snapshots.Select(s => s.Name))) throw new InvalidOperationException("Fixture missed a state snapshot");
            if (fixture.values.GetValueOrDefault("language_checks") == "true") LocalizationFrameChecks.Run(gameRoot, output, form.Layouts);
            var taskbar = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow;
            var controls = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin;
            File.WriteAllText(Path.Combine(output, "controls.json"), JsonSerializer.Serialize(new[] { controls.Unit1Chk.Text, controls.Unit2Chk.Text, controls.MovesLbl.Text }) + "\n");
            var selected = Aow.Game.World.Map.AoWHSMap.Current?.FSelectedArmy?.GetArmyHS();
            if (Aow.Game.World.Map.AoWHSMap.Current is { } world)
            {
                var heroes = new List<object>();
                for (int i = 0; i < world.FHeroControl.GetCount(); i++)
                {
                    var hero = world.FHeroControl.GetHero(i);
                    sbyte x = -1, y = -1, level = -1;
                    hero.GetLocation(ref x, ref y, ref level);
                    object[] Items(ItemList list) => Enumerable.Range(0, list.GetCount())
                        .Select(index => list.GetItem(index))
                        .OfType<Item>()
                        .Select(item => (object)new { item.FItemID, name = item.FName, type = item.FItemType.ToString() }).ToArray();
                    heroes.Add(new
                    {
                        name = hero.FHeroName,
                        hero.FPlayer,
                        x,
                        y,
                        level,
                        attack = hero.GetAttack(),
                        defense = hero.GetDefense(),
                        damage = hero.GetDamage(),
                        equipped = Items(hero.FHeroItems),
                        inventory = Items(hero.FInventory)
                    });
                }
                File.WriteAllText(Path.Combine(output, "heroes.json"), JsonSerializer.Serialize(heroes,
                    new JsonSerializerOptions { WriteIndented = true }) + "\n");
                if (name.StartsWith("paths-", StringComparison.Ordinal))
                {
                    var players = new List<object>();
                    var cities = new List<object>();
                    for (int playerIndex = 0; playerIndex < world.FPlayers.GetCount(); playerIndex++)
                    {
                        var player = world.FPlayers.GetPlayers(playerIndex);
                        players.Add(new { index = playerIndex, units = player.GetUnitCount(), gold = player.FGems });
                        for (int index = 0; index < player.FStructures.GetCount(); index++)
                            if (player.FStructures.GetStructure(index) is Aow.Game.World.Structures.City city)
                                cities.Add(new { name = city.FName, player = playerIndex, wall = city.FWallType, upgrade = city.FUpgradeLevel });
                    }
                    File.WriteAllText(Path.Combine(output, "world.json"), JsonSerializer.Serialize(new
                    {
                        day = world.FDay,
                        seat = world.FSeatedPlayer,
                        game_type = world.FGameType.ToString(),
                        players,
                        cities
                    }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
                }
            }
            if (Aow.Game.World.Map.AoWHSMap.Current?.FCombatMap is Aow.Game.Combat.AoWCombatMap combatMap)
            {
                var viewer = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer;
                var units = new List<object>();
                var data = combatMap.FCombat.FCombatData;
                for (int i = 0; i < data.GetCount(); i++)
                    if (data.GetObjects(i) is Aow.Game.Combat.TacticalCombatUnit unit && unit.FUnitHS is { } sprite)
                    {
                        int x = 0, y = 0;
                        Aow.Game.World.Map.HexGeometry.HXtoHP(sprite.GetXhx(), sprite.GetYhx(), ref x, ref y);
                        units.Add(new
                        {
                            unit.FID,
                            unit.FCombatPlayer,
                            unit.FMovePoints,
                            status = unit.FStatus.Image,
                            hits = unit.FUnit.GetHits(),
                            hit_points = unit.FUnit.GetHitPoints(),
                            hx = sprite.GetXhx(),
                            hy = sprite.GetYhx(),
                            screen_x = x - viewer.SceneX + 12,
                            screen_y = y - viewer.SceneY + 12
                        });
                    }
                File.WriteAllText(Path.Combine(output, "combat.json"), JsonSerializer.Serialize(new
                {
                    combatMap.FRound,
                    combatMap.FCurrentCombatPlayer,
                    combatMap.FSeatedCombatPlayer,
                    units
                }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            }
            File.WriteAllText(Path.Combine(output, "selection.json"), JsonSerializer.Serialize(new
            {
                count = Aow.Game.World.Map.AoWHSMap.Current?.FSelectedArmy?.FMoveSettingsList?.Count,
                x = selected?.GetXhx(),
                y = selected?.GetYhx(),
                level = selected?.GetLhx(),
                units = selected?.FArmy?.Count
            }) + "\n");
            if (fixture.values.GetValueOrDefault("save_state") == "true")
            {
                var map = Aow.Game.World.Map.AoWHSMap.Current ?? throw new InvalidOperationException("State capture requires a map");
                string saveName = "autosave.asg";
                if (fixture.values.GetValueOrDefault("save_world") == "true")
                {
                    saveName = "paths-world.asg";
                    Aow.Game.Magic.Abilities.GameTables.AoWEngine.SaveHSM(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Save\\" + saveName, null!, Aow.Formats.GameProfile.WorldMap);
                }
                else map.FPlayerControl.AutoSave();
                File.Copy(Path.Combine(output, "user", "Save", saveName), Path.Combine(output, "state.asg"), true);
            }
            if (fixture.values.TryGetValue("saved_files", out string? savedFiles))
                foreach (string saved in JsonSerializer.Deserialize<string[]>(savedFiles)!)
                    File.Copy(Path.Combine(output, "user", saved), Path.Combine(output, "saved-" + Path.GetFileName(saved)), true);
            File.WriteAllText(Path.Combine(output, "taskbar.json"), JsonSerializer.Serialize(
                new[] { taskbar.WGTLbl, taskbar.WGILbl, taskbar.WMTLbl, taskbar.WMILbl, taskbar.TGTLbl, taskbar.TGILbl, taskbar.TMTLbl, taskbar.TMILbl }
                    .Select(c => new
                    {
                        name = c.Name,
                        text = c.Text,
                        c.WinLeft,
                        c.WinTop,
                        c.WinWidth,
                        c.WinHeight,
                        c.FVisible,
                        owner = c.FAOWOwner?.Name,
                        window = c.FAOWWindow?.Name
                    }),
                new JsonSerializerOptions { WriteIndented = true }) + "\n");
            File.WriteAllText(Path.Combine(output, "fixture.json"), JsonSerializer.Serialize(new
            {
                fixture = name,
                complete,
                display_presentations = display.Frame,
                map_frame = Aow.Game.World.Map.AoWHSMap.Current?.Frame,
                day = Aow.Game.World.Map.AoWHSMap.Current?.FDay,
                combat_frame = Aow.Game.World.Map.AoWHSMap.Current?.FCombatMap?.Frame,
                clicks_applied = applied,
                input_sha256 = input is null ? null : Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(input))),
                local_time = now,
                file_local_time = fixture.fileLocalTime,
                file_stamp_utc = fileStampUtc,
                address_animation_phase = FrameFixtureInputs.AddressAnimationPhase,
                water_variant_phase = FrameFixtureInputs.WaterVariantPhase,
                measured_frame_rate = FrameFixtureInputs.MeasuredFrameRate,
                tactical_ready = tacticalReady,
                no_window = true,
                silent = true,
                checkpoint_frames = checkpoints,
                snapshots = capturedSnapshots
            }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine($"fixture {name}: map frame {Aow.Game.World.Map.AoWHSMap.Current?.Frame}, combat frame {Aow.Game.World.Map.AoWHSMap.Current?.FCombatMap?.Frame}, {display.Frame} presentations, {applied} clicks, {size.X}x{size.Y} RGB555LE, no window, silent");
        }
        finally
        {
            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.WaterVariantPhase = null;
            FrameFixtureInputs.PinnedAddress = null;
            FrameFixtureInputs.Tactical = false;
            FrameFixtureInputs.MeasuredFrameRate = null;
            FrameFixtureInputs.TacticalInitialized = null;
            RtlUnit.RandomizeSeed = null;
            AppDomain.CurrentDomain.FirstChanceException -= Record;
        }
        return 0;
    }

    private string Get(string key) => values.TryGetValue(key, out string? value) ? value : defaults.TryGetValue(key, out value) ? value : throw new InvalidDataException("Missing fixture field " + key);

    private sealed record Snapshot(string Name, int Day, int Frame);

    private (int X, int Y) Pair(string key)
    {
        using var json = JsonDocument.Parse(Get(key));
        return (json.RootElement[0].GetInt32(), json.RootElement[1].GetInt32());
    }

    private static FrameFixture Load(string path, string name)
    {
        var result = new FrameFixture();
        string section = "";
        Dictionary<string, string>? current = null;
        var fixtures = new List<Dictionary<string, string>>();
        foreach (string raw in File.ReadLines(path))
        {
            string line = raw.Trim();
            if (line.Length == 0 || line.StartsWith('#')) continue;
            if (line == "[[fixture]]") { section = "fixture"; current = new(StringComparer.Ordinal); fixtures.Add(current); continue; }
            if (line.StartsWith('[')) { section = line.Trim('[', ']'); continue; }
            int equals = line.IndexOf('=');
            if (equals < 0) throw new InvalidDataException("Invalid fixture line: " + line);
            string key = line[..equals].Trim().Trim('"');
            string value = line[(equals + 1)..].Trim();
            if (section == "defaults") result.defaults.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            else if (section == "file_clock")
            {
                if (key != "local_time") throw new InvalidDataException("Unknown file clock field " + key);
                result.fileLocalTime = JsonSerializer.Deserialize<string>(value)!;
            }
            else if (section.StartsWith("registry.", StringComparison.Ordinal))
            {
                int integer = int.Parse(value, CultureInfo.InvariantCulture);
                result.settings.Add(@"\Software\Triumph Studios\Age of Wonders\" + section[9..] + "\\" + key, booleans.Contains(key) ? integer != 0 : integer);
            }
            else if (section == "fixture") current!.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            else throw new InvalidDataException("Unknown fixture section " + section);
        }
        var match = fixtures.SingleOrDefault(f => f.GetValueOrDefault("name") == name) ?? throw new InvalidDataException("Unknown fixture " + name);
        foreach (var item in match) result.values.Add(item.Key, item.Value);

        return result;
    }

    private sealed record Click(int At, int X, int Y, int Button, bool Double, int? Key, string? Clock, int? Day, int? KeyDown, int Repeat, int? Hx, int? Hy, bool DoubleUi, bool MoveOnly);

    private List<Click> Clicks()
    {
        if (!values.TryGetValue("clicks", out string? text)) return [];
        string json = Regex.Replace(text, @"(\w+)\s*=", "\"$1\":");
        using var document = JsonDocument.Parse(json);
        return document.RootElement.EnumerateArray().Select(c => new Click(c.GetProperty("at").GetInt32(),
            c.TryGetProperty("x", out var x) ? x.GetInt32() : 0, c.TryGetProperty("y", out var y) ? y.GetInt32() : 0,
            c.TryGetProperty("button", out var button) ? button.GetInt32() : 0,
            c.TryGetProperty("double", out var twice) && twice.GetBoolean(),
            c.TryGetProperty("key", out var key) ? key.GetInt32() : null,
            c.TryGetProperty("clock", out var clock) ? clock.GetString() : null,
            c.TryGetProperty("day", out var day) ? day.GetInt32() : null,
            c.TryGetProperty("key_down", out var keyDown) ? keyDown.GetInt32() : null,
            c.TryGetProperty("repeat", out var repeat) ? repeat.GetInt32() : 1,
            c.TryGetProperty("hx", out var hx) ? hx.GetInt32() : null,
            c.TryGetProperty("hy", out var hy) ? hy.GetInt32() : null,
            c.TryGetProperty("double_ui", out var doubleUi) && doubleUi.GetBoolean(),
            c.TryGetProperty("move_only", out var moveOnly) && moveOnly.GetBoolean())).ToList();
    }

    internal sealed class FixtureHost : IRtlHost, IDialogService
    {
        public FixtureHost(string root, string user, Dictionary<string, object> settings, IClock clock, string? saveDirectory)
        {
            GameRoot = Path.GetFullPath(root);
            if (saveDirectory is not null) Directory.CreateDirectory(Path.Combine(user, saveDirectory));
            Files = new LayeredFileSystem(GameRoot, user, saveDirectory is null ? [] : [saveDirectory]);
            Clock = clock;
            foreach (var item in settings) Settings.Write(item.Key, item.Value);
        }

        public IFileSystem Files { get; }
        public IDialogService Dialogs => this;
        // A fixture records the address and opens nothing.
        public RecordingShell Urls { get; } = new();
        public IShellService Shell => Urls;
        public IClock Clock { get; }
        public ISettingsStore Settings { get; } = new MemorySettingsStore();
        public string ExeName => Files.ExpandPath(Path.Combine(GameRoot, "AoW.exe"));
        public string GameRoot { get; }
        // The fixtures replay native scripts and compare frames exactly with the original game, so
        // the game writes the original extensions here (own-formats.md 4.2).
        public bool WritesOwnExtensions => false;

        public int Show(DialogRequest request) => throw new InvalidOperationException("Fixture dialog: " + request.Text);
    }
}
