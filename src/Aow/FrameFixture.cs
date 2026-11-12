using System.Diagnostics;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Aow.Formats;
using Aow.Game.Combat;

using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Platform;
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow;

internal sealed class FrameFixture
{
    private string? _fileLocalTime;

    internal static DateTime FileStampUtc(DateTime localTime, TimeSpan offset) =>
        new DateTimeOffset(DateTime.SpecifyKind(localTime, DateTimeKind.Unspecified), offset).UtcDateTime;

    private readonly Dictionary<string, string> _defaults = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> _values = new(StringComparer.Ordinal);
    private readonly Dictionary<string, object> _settings = new(StringComparer.OrdinalIgnoreCase);
    private static readonly HashSet<string> s_booleans = ["Skip Intro", "Debug Mode", "Allow Resolution Switches", "Music Enabled", "Sound Effects Enabled", "Shadows Enabled", "Surface In VMem"];

    public static int Run(string path, string name, string gameRoot, string output, int? frameLimit = null)
    {
        gameRoot = Path.GetFullPath(gameRoot);
        output = Path.GetFullPath(output);
        var fixture = Load(path, name);
        string scene = fixture.Get("scene");
        bool presentationClock = fixture._defaults.GetValueOrDefault("frame_clock") == "presentation" || fixture._values.GetValueOrDefault("frame_clock") == "presentation";
        bool worldCapture = fixture._values.GetValueOrDefault("capture_clock") == "world";
        bool dayCapture = fixture._values.GetValueOrDefault("capture_clock") == "day";
        bool titleCapture = fixture._values.GetValueOrDefault("capture_clock") == "title";
        bool combatCapture = fixture._values.GetValueOrDefault("capture_clock") == "combat";
        bool afterCombatCapture = fixture._values.GetValueOrDefault("capture_clock") == "after-combat";
        bool tactical = fixture._values.GetValueOrDefault("action") == "tactical";
        var clicks = fixture.Clicks();
        int frame = frameLimit ?? int.Parse(fixture.Get("frame"), CultureInfo.InvariantCulture);
        if (frame < 1) throw new ArgumentOutOfRangeException(nameof(frameLimit));
        int[] checkpoints = fixture._values.TryGetValue("checkpoints", out string? checkpointText) ? JsonSerializer.Deserialize<int[]>(checkpointText)! : [];
        if (checkpoints.Length > 30 || checkpoints.Any(f => f < 1 || f >= frame) ||
            !checkpoints.SequenceEqual(checkpoints.Distinct().Order()) || checkpoints.Length > 0 && !afterCombatCapture)
            throw new InvalidDataException("Checkpoints require sorted earlier frames on the after-combat clock");
        var capturedCheckpoints = new HashSet<int>();
        Snapshot[] snapshots = fixture._values.TryGetValue("snapshots", out string? snapshotText)
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
        DateTime? fileStampUtc = fixture._fileLocalTime is null ? null :
            FileStampUtc(DateTime.Parse(fixture._fileLocalTime, CultureInfo.InvariantCulture), DateTimeOffset.Now.Offset);
        var display = new HeadlessDisplayHost(output, size.X, size.Y, frame) { Mouse = mouse, FixtureFrame = frame };
        Directory.CreateDirectory(output);
        using var diagnostics = FrameDiagnostics.Start(output);
        using var trace = new StreamWriter(Path.Combine(output, "exceptions.log")) { NewLine = "\n", AutoFlush = true };
        void Record(object? sender, FirstChanceExceptionEventArgs error) => trace.WriteLine(error.Exception + "\n" + new StackTrace(1, true));
        AppDomain.CurrentDomain.FirstChanceException += Record;
        string? saveDirectory = fixture._values.GetValueOrDefault("save_directory");
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
        var host = new FixtureHost(gameRoot, Path.Combine(output, "user"), fixture._settings, new FixtureClock(now), saveDirectory);
        RtlUnit.InstallHost(host);
        RtlUnit.SuppressRandomize = true;
        // Native startup resource sequences load with seed zero, before the fixed Randomize seed.
        RtlUnit.RandSeed = 0;
        RtlUnit.RandomizeSeed = int.Parse(fixture.Get("seed"), CultureInfo.InvariantCulture);
        FrameFixtureInputs.AddressAnimationPhase = int.Parse(fixture.Get("address_animation_phase"), CultureInfo.InvariantCulture);
        FrameFixtureInputs.WaterVariantPhase = fixture._values.TryGetValue("water_variant_phase", out string? waterPhase) ? int.Parse(waterPhase, CultureInfo.InvariantCulture) : null;
        FrameFixtureInputs.Tactical = tactical;
        // The native capture uses positive FPS below 15 at its six strategic moves.
        // Reproduce that movement tier without making animation depend on host speed.
        FrameFixtureInputs.MeasuredFrameRate = fixture._defaults.TryGetValue("measured_frame_rate", out var measuredRate) ? int.Parse(measuredRate, CultureInfo.InvariantCulture) : tactical ? 9 : null;
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
            if (AoWHSMap.Current is not null) titleOrigin = display.Frame;
            if (AoWHSMap.Current?.CombatMap is not null)
            {
                sawCombat = true;
                lastCombatWorldFrame = AoWHSMap.Current.Frame;
            }
            if (AoWHSMap.Current is { } map && observedDay != map.Day)
            {
                observedDay = map.Day;
                dayOrigin = map.Frame;
            }
        }
        int DayFrame() => AoWHSMap.Current is { } map ? map.Frame - dayOrigin + 1 : -1;
        int AfterCombatFrame() => sawCombat && AoWHSMap.Current is { CombatMap: null } map ? map.Frame - lastCombatWorldFrame : -1;
        long ScriptFrame(Click click)
        {
            string clock = click.Clock ?? fixture._defaults.GetValueOrDefault("frame_clock", "world");
            var map = AoWHSMap.Current;
            return clock switch
            {
                "presentation" => display.Frame,
                "after-combat" => AfterCombatFrame(),
                "combat" => map?.CombatMap?.Frame ?? -1,
                "title" => map is null ? display.Frame - titleOrigin : -1,
                _ => map?.Day == (click.Day ?? int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture))
                    ? clock == "day" ? DayFrame() : map.Frame : -1
            };
        }
        using var script = new StreamWriter(Path.Combine(output, "script.jsonl")) { NewLine = "\n", AutoFlush = true };
        try
        {
            using MainForm form = MainForm.Create(display, argument);
            if (saveDirectory is not null)
                DialogHosts.FileDialogWindow.Directory = Path.Combine(gameRoot, saveDirectory).Replace('/', '\\') + "\\";
            FrameFixtureInputs.TacticalInitialized = () =>
            {
                if (AoWHSMap.Current?.CombatMap is not CombatMap combat) return;
                combat.PlaySounds = true;
                combat.SetSeatedCombatPlayer(0);
                DialogHosts.ChatWindow.MinClick();
                tacticalReady = true;
            };
            bool AtTarget()
            {
                ObserveDay();
                if (applied != clicks.Count) return false;
                return afterCombatCapture ? AfterCombatFrame() == frame :
                combatCapture ? AoWHSMap.Current?.CombatMap?.Frame == frame :
                titleCapture ? AoWHSMap.Current is null && display.Frame - titleOrigin == frame :
                dayCapture ? AoWHSMap.Current?.Day == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) && DayFrame() == frame :
                !worldCapture && (scene == "title" || presentationClock) ? display.Frame == frame :
                tactical ? tacticalReady && AoWHSMap.Current.CombatMap.Frame == frame :
                AoWHSMap.Current is { } map && map.Day == int.Parse(fixture.Get("day"), CultureInfo.InvariantCulture) && map.Frame == frame;
            }
            display.CaptureWhen = () =>
            {
                ObserveDay();
                pendingSnapshot = snapshots.FirstOrDefault(s => AoWHSMap.Current?.Day == s.Day && DayFrame() == s.Frame && !capturedSnapshots.Contains(s.Name));
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
                if (fixture._values.GetValueOrDefault("diagnostics") == "true" && display.Frame % 100 == 0 && !AtTarget())
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
                    AoWHSMap.Current.PlayerControl.AutoSave();
                    File.Copy(Path.Combine(output, "user", "Save", "autosave.asg"), Path.Combine(output, "snapshot-" + snapshot.Name + ".asg"), true);
                    capturedSnapshots.Add(snapshot.Name);
                    pendingSnapshot = null;
                }
                if (display.Frame % 100 == 0)
                    File.WriteAllText(Path.Combine(output, "progress.json"), JsonSerializer.Serialize(new
                    {
                        presentation = display.Frame,
                        applied,
                        day = AoWHSMap.Current?.Day,
                        map_frame = AoWHSMap.Current?.Frame,
                        seat = AoWHSMap.Current?.SeatedPlayer,
                        combat_frame = AoWHSMap.Current?.CombatMap?.Frame
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
                        var manager = DialogHosts.General.Manager;
                        int inputX = click.X, inputY = click.Y;
                        if (click.Hex is int hex && click.Hy is int hy)
                        {
                            var mapWindow = DialogHosts.MapWindow;
                            HexGeometry.HexToPixel(hex, hy, ref inputX, ref inputY);
                            inputX += 28 + mapWindow.MainWindow.WinLeft - mapWindow.MapViewer.SceneX;
                            inputY += 28 + mapWindow.MainWindow.WinTop - mapWindow.MapViewer.SceneY;
                            if (!mapWindow.InsideMap(inputX, inputY)) throw new InvalidOperationException($"Script hex {hex},{hy} is outside the map view at {inputX},{inputY}");
                        }
                        if (click.KeyDown is int keyDown)
                            for (int i = 0; i < click.Repeat; i++) form.KeyDown((ushort)keyDown, default);
                        else if (click.Key is int key) { byte character = (byte)key; manager.CheckKeyPress(ref character); }
                        else
                        {
                            manager.CheckMouseMove(default, inputX, inputY);
                            if (!click.MoveOnly)
                            {
                                var shift = click.DoubleUI ? Set<ShiftState>.Of(ShiftState.Double) : default;
                                manager.CheckMouseDown((MouseButton)click.Button, shift, inputX, inputY);
                                manager.CheckMouseUp((MouseButton)click.Button, default, inputX, inputY);
                            }
                        }
                        if (click.Double || click.DoubleUI) form.DoubleClick();
                        script.WriteLine(JsonSerializer.Serialize(new
                        {
                            script_frame = scriptFrame,
                            X = inputX,
                            Y = inputY,
                            Hx = click.Hex,
                            click.Hy,
                            click.Button,
                            click.Double,
                            DoubleUi = click.DoubleUI,
                            click.Key,
                            click.KeyDown,
                            click.Repeat,
                            click.Clock,
                            click.Day,
                            selected_combat_unit = DialogHosts.General.CombatUnit?.Unit.Id
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
            if (fixture._values.GetValueOrDefault("language_checks") == "true") LocalizationFrameChecks.Run(gameRoot, output, form.Layouts);
            TaskBar taskbar = DialogHosts.TaskBar;
            var controls = DialogHosts.ControlWindow;
            File.WriteAllText(Path.Combine(output, "controls.json"), JsonSerializer.Serialize(new[] { controls.Unit1Chk.Text, controls.Unit2Chk.Text, controls.MovesLbl.Text }) + "\n");
            var selected = AoWHSMap.Current?.SelectedArmy?.GetArmyHexSprite();
            if (AoWHSMap.Current is { } world)
            {
                var heroes = new List<object>();
                for (int i = 0; i < world.HeroControl.GetCount(); i++)
                {
                    var hero = world.HeroControl.GetHero(i);
                    sbyte x = -1, y = -1, level = -1;
                    hero.GetLocation(ref x, ref y, ref level);
                    object[] Items(ItemList list) => Enumerable.Range(0, list.GetCount())
                        .Select(index => list.GetItem(index))
                        .OfType<Item>()
                        .Select(item => (object)new { FItemID = item.ItemId, name = item.Name, type = item.ItemType.ToString() }).ToArray();
                    heroes.Add(new
                    {
                        name = hero.HeroName,
                        FPlayer = hero.Player,
                        x,
                        y,
                        level,
                        attack = hero.GetAttack(),
                        defense = hero.GetDefense(),
                        damage = hero.GetDamage(),
                        equipped = Items(hero.HeroItems),
                        inventory = Items(hero.Inventory)
                    });
                }
                File.WriteAllText(Path.Combine(output, "heroes.json"), JsonSerializer.Serialize(heroes,
                    new JsonSerializerOptions { WriteIndented = true }) + "\n");
                if (name.StartsWith("paths-", StringComparison.Ordinal))
                {
                    var players = new List<object>();
                    var cities = new List<object>();
                    for (int playerIndex = 0; playerIndex < world.Players.GetCount(); playerIndex++)
                    {
                        var player = world.Players.GetPlayers(playerIndex);
                        players.Add(new { index = playerIndex, units = player.GetUnitCount(), gold = player.Gems });
                        for (int index = 0; index < player.Structures.GetCount(); index++)
                            if (player.Structures.GetStructure(index) is City city)
                                cities.Add(new { name = city.Name, player = playerIndex, wall = city.WallType, upgrade = city.UpgradeLevel });
                    }
                    File.WriteAllText(Path.Combine(output, "world.json"), JsonSerializer.Serialize(new
                    {
                        day = world.Day,
                        seat = world.SeatedPlayer,
                        game_type = world.GameType.ToString(),
                        players,
                        cities
                    }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
                }
            }
            if (AoWHSMap.Current?.CombatMap is CombatMap combatMap)
            {
                var viewer = DialogHosts.MapWindow.MapViewer;
                var units = new List<object>();
                var data = combatMap.Combat.CombatData;
                for (int i = 0; i < data.GetCount(); i++)
                    if (data.GetObjects(i) is TacticalBattleUnit unit && unit.UnitHexSprite is { } sprite)
                    {
                        int x = 0, y = 0;
                        HexGeometry.HexToPixel(sprite.GetHexX(), sprite.GetHexY(), ref x, ref y);
                        units.Add(new
                        {
                            FID = unit.Id,
                            FCombatPlayer = unit.CombatPlayer,
                            FMovePoints = unit.MovePoints,
                            status = unit.Status.Image,
                            hits = unit.Troop.GetHits(),
                            hit_points = unit.Troop.GetHitPoints(),
                            hx = sprite.GetHexX(),
                            hy = sprite.GetHexY(),
                            screen_x = x - viewer.SceneX + 12,
                            screen_y = y - viewer.SceneY + 12
                        });
                    }
                File.WriteAllText(Path.Combine(output, "combat.json"), JsonSerializer.Serialize(new
                {
                    FRound = combatMap.Round,
                    FCurrentCombatPlayer = combatMap.CurrentCombatPlayer,
                    FSeatedCombatPlayer = combatMap.SeatedCombatPlayer,
                    units
                }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            }
            File.WriteAllText(Path.Combine(output, "selection.json"), JsonSerializer.Serialize(new
            {
                count = AoWHSMap.Current?.SelectedArmy?.MoveSettingsList?.Count,
                x = selected?.GetHexX(),
                y = selected?.GetHexY(),
                level = selected?.GetHexLevel(),
                units = selected?.Army?.Count
            }) + "\n");
            if (fixture._values.GetValueOrDefault("save_state") == "true")
            {
                var map = AoWHSMap.Current ?? throw new InvalidOperationException("State capture requires a map");
                string saveName = "autosave.asg";
                if (fixture._values.GetValueOrDefault("save_world") == "true")
                {
                    saveName = "paths-world.asg";
                    GameTables.GameEngine.SaveHsm(GameTables.GameEngine.StartupDirectory + "Save\\" + saveName, null!, GameProfile.WorldMap);
                }
                else map.PlayerControl.AutoSave();
                File.Copy(Path.Combine(output, "user", "Save", saveName), Path.Combine(output, "state.asg"), true);
            }
            if (fixture._values.TryGetValue("saved_files", out string? savedFiles))
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
                        FVisible = c.Visible,
                        owner = c.OwnerComponent?.Name,
                        window = c.Window?.Name
                    }),
                new JsonSerializerOptions { WriteIndented = true }) + "\n");
            File.WriteAllText(Path.Combine(output, "fixture.json"), JsonSerializer.Serialize(new
            {
                fixture = name,
                complete,
                display_presentations = display.Frame,
                map_frame = AoWHSMap.Current?.Frame,
                day = AoWHSMap.Current?.Day,
                combat_frame = AoWHSMap.Current?.CombatMap?.Frame,
                clicks_applied = applied,
                input_sha256 = input is null ? null : Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(input))),
                local_time = now,
                file_local_time = fixture._fileLocalTime,
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
            Console.WriteLine($"fixture {name}: map frame {AoWHSMap.Current?.Frame}, combat frame {AoWHSMap.Current?.CombatMap?.Frame}, {display.Frame} presentations, {applied} clicks, {size.X}x{size.Y} RGB555LE, no window, silent");
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

    private string Get(string key) => _values.TryGetValue(key, out string? value) ? value : _defaults.TryGetValue(key, out value) ? value : throw new InvalidDataException("Missing fixture field " + key);

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
            if (section == "defaults") result._defaults.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            else if (section == "file_clock")
            {
                if (key != "local_time") throw new InvalidDataException("Unknown file clock field " + key);
                result._fileLocalTime = JsonSerializer.Deserialize<string>(value)!;
            }
            else if (section.StartsWith("registry.", StringComparison.Ordinal))
            {
                int integer = int.Parse(value, CultureInfo.InvariantCulture);
                result._settings.Add(@"\Software\Triumph Studios\Age of Wonders\" + section[9..] + "\\" + key, s_booleans.Contains(key) ? integer != 0 : integer);
            }
            else if (section == "fixture") current!.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            else throw new InvalidDataException("Unknown fixture section " + section);
        }
        var match = fixtures.SingleOrDefault(f => f.GetValueOrDefault("name") == name) ?? throw new InvalidDataException("Unknown fixture " + name);
        foreach (var item in match) result._values.Add(item.Key, item.Value);

        return result;
    }

    private sealed record Click(int At, int X, int Y, int Button, bool Double, int? Key, string? Clock, int? Day, int? KeyDown, int Repeat, int? Hex, int? Hy, bool DoubleUI, bool MoveOnly);

    private List<Click> Clicks()
    {
        if (!_values.TryGetValue("clicks", out string? text)) return [];
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
            c.TryGetProperty("hx", out var hex) ? hex.GetInt32() : null,
            c.TryGetProperty("hy", out var hy) ? hy.GetInt32() : null,
            c.TryGetProperty("double_ui", out var doubleUI) && doubleUI.GetBoolean(),
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
        public string ExecutableName => Files.ExpandPath(Path.Combine(GameRoot, "AoW.exe"));
        public string GameRoot { get; }
        // The fixtures replay native scripts and compare frames exactly with the original game, so
        // the game writes the original extensions here (own-formats.md 4.2).
        public bool WritesOwnExtensions => false;

        public int Show(DialogRequest request) => throw new InvalidOperationException("Fixture dialog: " + request.Text);
    }
}
