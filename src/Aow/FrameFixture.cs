using System.Diagnostics;
using System.Globalization;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using Aow.Formats;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
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

    internal static DateTime FileStampUtc(DateTime localTime, TimeSpan offset)
        => new DateTimeOffset(DateTime.SpecifyKind(localTime, kind: DateTimeKind.Unspecified), offset).UtcDateTime;

    private readonly Dictionary<string, string> _defaults = [with(StringComparer.Ordinal)];
    private readonly Dictionary<string, string> _values = [with(StringComparer.Ordinal)];
    private readonly Dictionary<string, object> _settings = [with(StringComparer.OrdinalIgnoreCase)];
    private static readonly HashSet<string> s_booleans = ["Skip Intro", "Debug Mode", "Allow Resolution Switches", "Music Enabled", "Sound Effects Enabled", "Shadows Enabled", "Surface In VMem"];

    public static int Run(string path, string name, string gameRoot, string output, int? frameLimit = null)
    {
        gameRoot = Path.GetFullPath(gameRoot);
        output = Path.GetFullPath(output);
        FrameFixture fixture = Load(path, name);
        string scene = fixture.Get("scene");
        bool presentationClock = fixture._defaults.GetValueOrDefault("frame_clock") == "presentation" || fixture._values.GetValueOrDefault("frame_clock") == "presentation";
        bool worldCapture = fixture._values.GetValueOrDefault("capture_clock") == "world";
        bool dayCapture = fixture._values.GetValueOrDefault("capture_clock") == "day";
        bool titleCapture = fixture._values.GetValueOrDefault("capture_clock") == "title";
        bool combatCapture = fixture._values.GetValueOrDefault("capture_clock") == "combat";
        bool afterCombatCapture = fixture._values.GetValueOrDefault("capture_clock") == "after-combat";
        bool tactical = fixture._values.GetValueOrDefault("action") == "tactical";
        List<Click> clicks = fixture.Clicks();
        int frame = frameLimit ?? int.Parse(fixture.Get("frame"), provider: CultureInfo.InvariantCulture);
        if (frame < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(frameLimit));
        }

        int[] checkpoints = fixture._values.TryGetValue("checkpoints", out string? checkpointText) ? JsonSerializer.Deserialize<int[]>(checkpointText)! : [];
        if (checkpoints.Length > 30 || checkpoints.Any(f => f < 1 || f >= frame) ||
            !checkpoints.SequenceEqual(checkpoints.Distinct().Order()) || (checkpoints.Length > 0 && !afterCombatCapture))
        {
            throw new InvalidDataException("Checkpoints require sorted earlier frames on the after-combat clock");
        }

        HashSet<int> capturedCheckpoints = [];
        Snapshot[] snapshots = fixture._values.TryGetValue("snapshots", out string? snapshotText)
            ? JsonSerializer.Deserialize<Snapshot[]>(Regex.Replace(snapshotText, pattern: @"(\w+)\s*=", replacement: "\"$1\":"), options: new JsonSerializerOptions { PropertyNameCaseInsensitive = true })! : [];
        if (snapshots.Length > 8 || snapshots.Any(s => !Regex.IsMatch(s.Name, pattern: "^[a-z0-9-]+$") || s.Day < 1 || s.Frame < 1 ||
            !clicks.Any(c => c.Clock == "day" && c.Day == s.Day && c.At == s.Frame)) ||
            snapshots.Select(s => s.Name).Distinct().Count() != snapshots.Length || (snapshots.Length > 0 && checkpoints.Length > 0))
        {
            throw new InvalidDataException("Snapshots require distinct safe names and matching day-clock inputs");
        }

        HashSet<string> capturedSnapshots = [];
        Snapshot? pendingSnapshot = null;
        (int width, int height) = fixture.Pair("size");
        (int X, int Y) mouse = fixture.Pair("mouse");
        DateTime now = DateTime.Parse(fixture.Get("local_time"), provider: CultureInfo.InvariantCulture);
        DateTime? fileStampUtc = fixture._fileLocalTime is null ? null :
            FileStampUtc(DateTime.Parse(fixture._fileLocalTime, provider: CultureInfo.InvariantCulture), DateTimeOffset.Now.Offset);
        HeadlessDisplayHost display = new(output, width, height, captureFrame: frame) { Mouse = mouse, FixtureFrame = frame };
        _ = Directory.CreateDirectory(output);
        using FrameDiagnostics? diagnostics = FrameDiagnostics.Start(output);
        using StreamWriter trace = new(Path.Combine(output, "exceptions.log")) { NewLine = "\n", AutoFlush = true };
        void Record(object? sender, FirstChanceExceptionEventArgs error) => trace.WriteLine($"{error.Exception}\n{new StackTrace(1, fNeedFileInfo: true)}");
        AppDomain.CurrentDomain.FirstChanceException += Record;
        string? saveDirectory = fixture._values.GetValueOrDefault("save_directory");
        if (saveDirectory is not null)
        {
            string directory = Path.Combine(output, "user", saveDirectory);
            if (Directory.Exists(directory))
            {
                foreach (string saved in Directory.EnumerateFiles(directory))
                {
                    if (Path.GetFileName(saved) != "roundtrip.asg")
                    {
                        throw new InvalidDataException($"Unexpected fixture save product {saved}");
                    }

                    File.Delete(saved);
                }
            }
        }
        FixtureHost host = new(gameRoot, user: Path.Combine(output, "user"), fixture._settings, clock: new FixtureClock(now), saveDirectory);
        RtlHost.InstallHost(host);
        DelphiRandom.SuppressRandomize = true;
        // Native startup resource sequences load with seed zero, before the fixed Randomize seed.
        DelphiRandom.RandSeed = 0;
        DelphiRandom.RandomizeSeed = int.Parse(fixture.Get("seed"), provider: CultureInfo.InvariantCulture);
        FrameFixtureInputs.AddressAnimationPhase = int.Parse(fixture.Get("address_animation_phase"), provider: CultureInfo.InvariantCulture);
        FrameFixtureInputs.WaterVariantPhase = fixture._values.TryGetValue("water_variant_phase", out string? waterPhase) ? int.Parse(waterPhase, provider: CultureInfo.InvariantCulture) : null;
        FrameFixtureInputs.Tactical = tactical;
        // The native capture uses positive FPS below 15 at its six strategic moves.
        // Reproduce that movement tier without making animation depend on host speed.
        FrameFixtureInputs.MeasuredFrameRate = fixture._defaults.TryGetValue("measured_frame_rate", out string? measuredRate) ? int.Parse(measuredRate, provider: CultureInfo.InvariantCulture) : tactical ? 9 : null;
        string argument = "";
        string? input = null;
        if (scene != "title")
        {
            input = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(path))!, "..", "..", "tools", "oracle", "obj", "frames", "inputs", $"{scene}.asg");
            if (!File.Exists(input))
            {
                throw new FileNotFoundException("Prepared fixture input missing", fileName: input);
            }

            File.Copy(input, destFileName: Path.Combine(output, "user", $"{scene}.asg"), overwrite: true);
            argument = Path.Combine(gameRoot, $"{scene}.asg");
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
            if (StrategicMap.Current is not null)
            {
                titleOrigin = display.Frame;
            }

            if (StrategicMap.Current?.CombatMap is not null)
            {
                sawCombat = true;
                lastCombatWorldFrame = StrategicMap.Current.Frame;
            }
            if (StrategicMap.Current is { } map && observedDay != map.Day)
            {
                observedDay = map.Day;
                dayOrigin = map.Frame;
            }
        }
        int DayFrame() => StrategicMap.Current is { } map ? map.Frame - dayOrigin + 1 : -1;
        int AfterCombatFrame() => sawCombat && StrategicMap.Current is { CombatMap: null } map ? map.Frame - lastCombatWorldFrame : -1;
        long ScriptFrame(Click click)
        {
            string clock = click.Clock ?? fixture._defaults.GetValueOrDefault("frame_clock", "world");
            StrategicMap? map = StrategicMap.Current;
            return clock switch
            {
                "presentation" => display.Frame,
                "after-combat" => AfterCombatFrame(),
                "combat" => map?.CombatMap?.Frame ?? -1,
                "title" => map is null ? display.Frame - titleOrigin : -1,
                _ => map?.Day == (click.Day ?? int.Parse(fixture.Get("day"), provider: CultureInfo.InvariantCulture))
                    ? clock == "day" ? DayFrame() : map.Frame : -1
            };
        }
        using StreamWriter script = new(Path.Combine(output, "script.jsonl")) { NewLine = "\n", AutoFlush = true };
        try
        {
            using MainForm form = MainForm.Create(display, argument);
            if (saveDirectory is not null)
            {
                form.UI.FileDialogWindow.Directory = $"{Path.Combine(gameRoot, saveDirectory).Replace('/', '\\')}\\";
            }

            FrameFixtureInputs.TacticalInitialized = () =>
            {
                if (StrategicMap.Current?.CombatMap is not CombatMap combat)
                {
                    return;
                }

                combat.PlaySounds = true;
                combat.SetSeatedCombatPlayer(0);
                form.UI.ChatWindow.MinClick();
                tacticalReady = true;
            };
            bool AtTarget()
            {
                ObserveDay();
                return applied == clicks.Count && (afterCombatCapture ? AfterCombatFrame() == frame :
                combatCapture ? StrategicMap.Current?.CombatMap?.Frame == frame :
                titleCapture ? StrategicMap.Current is null && display.Frame - titleOrigin == frame :
                dayCapture ? StrategicMap.Current?.Day == int.Parse(fixture.Get("day"), provider: CultureInfo.InvariantCulture) && DayFrame() == frame :
                !worldCapture && (scene == "title" || presentationClock) ? display.Frame == frame :
                tactical ? tacticalReady && StrategicMap.Current.CombatMap.Frame == frame :
                StrategicMap.Current is { } map && map.Day == int.Parse(fixture.Get("day"), provider: CultureInfo.InvariantCulture) && map.Frame == frame);
            }
            display.CaptureWhen = () =>
            {
                ObserveDay();
                pendingSnapshot = snapshots.FirstOrDefault(s => StrategicMap.Current?.Day == s.Day && DayFrame() == s.Frame && !capturedSnapshots.Contains(s.Name));
                display.CaptureName = pendingSnapshot is null ? null : $"snapshot-{pendingSnapshot.Name}";
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
                    diagnostics?.Snapshot(snapshot.Name, ui: form.UI);
                    StrategicMap.Current.PlayerControl.AutoSave();
                    File.Copy(Path.Combine(output, "user", "Save", "autosave.asg"), destFileName: Path.Combine(output, $"snapshot-{snapshot.Name}.asg"), overwrite: true);
                    _ = capturedSnapshots.Add(snapshot.Name);
                    pendingSnapshot = null;
                }
                if (display.Frame % 100 == 0)
                {
                    File.WriteAllText(Path.Combine(output, "progress.json"), contents: JsonSerializer.Serialize(new
                    {
                        presentation = display.Frame,
                        applied,
                        day = StrategicMap.Current?.Day,
                        map_frame = StrategicMap.Current?.Frame,
                        seat = StrategicMap.Current?.SeatedPlayer,
                        combat_frame = StrategicMap.Current?.CombatMap?.Frame
                    }) + "\n");
                }

                if (AtTarget())
                {
                    complete = true;
                    form.Display.StopUpdateFrameLoop();
                    return;
                }
                if (applied < clicks.Count)
                {
                    Click click = clicks[applied];
                    long scriptFrame = ScriptFrame(click);
                    if (scriptFrame > click.At)
                    {
                        throw new InvalidOperationException($"Missed scripted frame {click.At}");
                    }

                    if (scriptFrame == click.At)
                    {
                        if (saveDirectory is not null)
                        {
                            foreach (string saved in Directory.EnumerateFiles(Path.Combine(output, "user", saveDirectory)))
                            {
                                if (fileStampUtc is DateTime stamp)
                                {
                                    File.SetLastWriteTimeUtc(saved, lastWriteTimeUtc: stamp);
                                }
                                else
                                {
                                    File.SetLastWriteTime(saved, lastWriteTime: now);
                                }
                            }
                        }

                        WindowManager manager = form.UI.General.Manager;
                        int inputX = click.X, inputY = click.Y;
                        if (click.Hex is int hex && click.Hy is int hy)
                        {
                            MapWindow mapWindow = form.UI.MapWindow;
                            HexGeometry.HexToPixel(hex, hy, out inputX, out inputY);
                            inputX += 28 + mapWindow.MainWindow.WinLeft - mapWindow.MapViewer.SceneX;
                            inputY += 28 + mapWindow.MainWindow.WinTop - mapWindow.MapViewer.SceneY;
                            if (!mapWindow.InsideMap(inputX, inputY))
                            {
                                throw new InvalidOperationException($"Script hex {hex},{hy} is outside the map view at {inputX},{inputY}");
                            }
                        }
                        if (click.KeyDown is int keyDown)
                        {
                            for (int i = 0; i < click.Repeat; i++)
                            {
                                form.KeyDown((ushort)keyDown, shift: default);
                            }
                        }
                        else if (click.Key is int key)
                        {
                            byte character = (byte)key;
                            manager.CheckKeyPress(ref character);
                        }
                        else
                        {
                            manager.CheckMouseMove(default, inputX, inputY);
                            if (!click.MoveOnly)
                            {
                                manager.CheckMouseDown(shift: click.DoubleUI ? ShiftStates.Double : default, button: (MouseButton)click.Button, x: inputX, y: inputY);
                                manager.CheckMouseUp((MouseButton)click.Button, shift: default, inputX, inputY);
                            }
                        }
                        if (click.Double || click.DoubleUI)
                        {
                            form.DoubleClick();
                        }

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
                            selected_combat_unit = form.UI.General.CombatUnit?.Unit.Id
                        }));
                        applied++;
                    }
                }
                if (display.Frame >= 10000)
                {
                    throw new TimeoutException("Fixture target not reached");
                }
            };
            form.Run(scene == "title" && !worldCapture && !dayCapture ? frame : 0);
            if (scene == "title" && !worldCapture && !dayCapture)
            {
                complete = display.Frame == frame;
            }

            if (!complete || applied != clicks.Count)
            {
                throw new InvalidOperationException("Fixture ended before all inputs and the target frame");
            }

            if (!capturedCheckpoints.SetEquals(checkpoints))
            {
                throw new InvalidOperationException("Fixture missed an intermediate checkpoint");
            }

            if (!capturedSnapshots.SetEquals(snapshots.Select(s => s.Name)))
            {
                throw new InvalidOperationException("Fixture missed a state snapshot");
            }

            if (fixture._values.GetValueOrDefault("language_checks") == "true")
            {
                LocalizationFrameChecks.Run(gameRoot, output, form.Layouts, ui: form.UI);
            }

            TaskBar taskbar = form.UI.TaskBar;
            ControlWindow controls = form.UI.ControlWindow;
            File.WriteAllText(Path.Combine(output, "controls.json"), contents: $"{JsonSerializer.Serialize(new[] { controls.Unit1CheckBox.Text, controls.Unit2CheckBox.Text, controls.MovesLabel.Text })}\n");
            ArmyHexSprite? selected = StrategicMap.Current?.SelectedArmy?.ArmyHexSprite;
            if (StrategicMap.Current is { } world)
            {
                List<object> heroes = [];
                for (int i = 0; i < world.HeroControl.GetCount(); i++)
                {
                    Hero hero = world.HeroControl[i];
                    MapCoordinate coordinate1 = hero.GetLocation();
                    sbyte x = coordinate1.X;
                    sbyte y = coordinate1.Y;
                    sbyte level = coordinate1.MapLevel;
                    object[] Items(ItemCollection list) => [.. Enumerable.Range(0, list.Count)
                        .Select(index => list.GetItem(index))
                        .OfType<Item>()
                        .Select(item => (object)new { FItemID = item.ItemId, name = item.Name, type = item.ItemType.ToString() })];
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
                        equipped = Items(hero.HeroItems.Items),
                        inventory = Items(hero.Inventory.Items)
                    });
                }
                File.WriteAllText(Path.Combine(output, "heroes.json"), contents: JsonSerializer.Serialize(heroes,
                    options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
                if (name.StartsWith("paths-", comparisonType: StringComparison.Ordinal))
                {
                    List<object> players = [];
                    List<object> cities = [];
                    for (int playerIndex = 0; playerIndex < world.Players.GetCount(); playerIndex++)
                    {
                        Player player = world.Players.GetPlayers(playerIndex);
                        players.Add(new { index = playerIndex, units = player.GetUnitCount(), gold = player.Gems });
                        for (int index = 0; index < player.Structures.GetCount(); index++)
                        {
                            if (player.Structures.GetStructure(index) is City city)
                            {
                                cities.Add(new { name = city.Name, player = playerIndex, wall = city.WallType, upgrade = city.UpgradeLevel });
                            }
                        }
                    }
                    File.WriteAllText(Path.Combine(output, "world.json"), contents: JsonSerializer.Serialize(new
                    {
                        day = world.Day,
                        seat = world.SeatedPlayer,
                        game_type = world.GameType.ToString(),
                        players,
                        cities
                    }, options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
                }
            }
            if (StrategicMap.Current?.CombatMap is CombatMap combatMap)
            {
                HexSpriteMapViewer viewer = form.UI.MapWindow.MapViewer;
                List<object> units = [];
                CombatData data = combatMap.Combat.CombatData;
                for (int i = 0; i < data.GetCount(); i++)
                {
                    if (data.GetObjects(i) is TacticalBattleUnit unit && unit.UnitHexSprite is { } sprite)
                    {
                        HexGeometry.HexToPixel(sprite.HexX, sprite.HexY, out int x, out int y);
                        units.Add(new
                        {
                            FID = unit.Id,
                            FCombatPlayer = unit.CombatPlayer,
                            FMovePoints = unit.MovePoints,
                            status = (ulong)unit.Status,
                            hits = unit.Troop.GetHits(),
                            hit_points = unit.Troop.GetHitPoints(),
                            hx = sprite.HexX,
                            hy = sprite.HexY,
                            screen_x = x - viewer.SceneX + 12,
                            screen_y = y - viewer.SceneY + 12
                        });
                    }
                }

                File.WriteAllText(Path.Combine(output, "combat.json"), contents: JsonSerializer.Serialize(new
                {
                    FRound = combatMap.Round,
                    FCurrentCombatPlayer = combatMap.CurrentCombatPlayer,
                    FSeatedCombatPlayer = combatMap.SeatedCombatPlayer,
                    units
                }, options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
            }
            File.WriteAllText(Path.Combine(output, "selection.json"), contents: JsonSerializer.Serialize(new
            {
                count = StrategicMap.Current?.SelectedArmy?.MoveSettingsList?.Count,
                x = selected?.HexX,
                y = selected?.HexY,
                level = selected?.HexLevel,
                units = selected?.Army?.Units.Count
            }) + "\n");
            if (fixture._values.GetValueOrDefault("save_state") == "true")
            {
                StrategicMap map = StrategicMap.Current ?? throw new InvalidOperationException("State capture requires a map");
                string saveName = "autosave.asg";
                if (fixture._values.GetValueOrDefault("save_world") == "true")
                {
                    saveName = "paths-world.asg";
                    form.UI.Engine.SaveHsm($"{form.UI.Engine.StartupDirectory}Save\\{saveName}", progress: null!, profile: GameProfile.WorldMap);
                }
                else
                {
                    map.PlayerControl.AutoSave();
                }

                File.Copy(Path.Combine(output, "user", "Save", saveName), destFileName: Path.Combine(output, "state.asg"), overwrite: true);
            }
            if (fixture._values.TryGetValue("saved_files", out string? savedFiles))
            {
                foreach (string saved in JsonSerializer.Deserialize<string[]>(savedFiles)!)
                {
                    File.Copy(Path.Combine(output, "user", saved), destFileName: Path.Combine(output, $"saved-{Path.GetFileName(saved)}"), overwrite: true);
                }
            }

            File.WriteAllText(Path.Combine(output, "taskbar.json"), contents: JsonSerializer.Serialize(
                new[] { taskbar.WorldGoldTotalLabel, taskbar.WorldGoldIncomeLabel, taskbar.WorldManaTotalLabel, taskbar.WorldManaIncomeLabel, taskbar.CombatGoldTotalLabel, taskbar.CombatGoldIncomeLabel, taskbar.CombatManaTotalLabel, taskbar.CombatManaIncomeLabel }
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
                options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
            File.WriteAllText(Path.Combine(output, "fixture.json"), contents: JsonSerializer.Serialize(new
            {
                fixture = name,
                complete,
                display_presentations = display.Frame,
                map_frame = StrategicMap.Current?.Frame,
                day = StrategicMap.Current?.Day,
                combat_frame = StrategicMap.Current?.CombatMap?.Frame,
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
            }, options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
            Console.WriteLine($"fixture {name}: map frame {StrategicMap.Current?.Frame}, combat frame {StrategicMap.Current?.CombatMap?.Frame}, {display.Frame} presentations, {applied} clicks, {width}x{height} RGB555LE, no window, silent");
        }
        finally
        {
            FrameFixtureInputs.AddressAnimationPhase = null;
            FrameFixtureInputs.WaterVariantPhase = null;
            FrameFixtureInputs.PinnedAddress = null;
            FrameFixtureInputs.Tactical = false;
            FrameFixtureInputs.MeasuredFrameRate = null;
            FrameFixtureInputs.TacticalInitialized = null;
            DelphiRandom.RandomizeSeed = null;
            AppDomain.CurrentDomain.FirstChanceException -= Record;
        }
        return 0;
    }

    private string Get(string key) => _values.TryGetValue(key, out string? value) ? value : _defaults.TryGetValue(key, out value) ? value : throw new InvalidDataException($"Missing fixture field {key}");

    private sealed record Snapshot(string Name, int Day, int Frame);

    private (int X, int Y) Pair(string key)
    {
        using JsonDocument json = JsonDocument.Parse(Get(key));
        return (json.RootElement[0].GetInt32(), json.RootElement[1].GetInt32());
    }

    private static FrameFixture Load(string path, string name)
    {
        FrameFixture result = new();
        string section = "";
        Dictionary<string, string>? current = null;
        List<Dictionary<string, string>> fixtures = [];
        foreach (string raw in File.ReadLines(path))
        {
            string line = raw.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            if (line == "[[fixture]]")
            {
                section = "fixture";
                current = [with(StringComparer.Ordinal)];
                fixtures.Add(current);
                continue;
            }
            if (line.StartsWith('['))
            {
                section = line.Trim('[', ']');
                continue;
            }
            int equals = line.IndexOf('=');
            if (equals < 0)
            {
                throw new InvalidDataException($"Invalid fixture line: {line}");
            }

            string key = line[..equals].Trim().Trim('"');
            string value = line[(equals + 1)..].Trim();
            if (section == "defaults")
            {
                result._defaults.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            }
            else if (section == "file_clock")
            {
                if (key != "local_time")
                {
                    throw new InvalidDataException($"Unknown file clock field {key}");
                }

                result._fileLocalTime = JsonSerializer.Deserialize<string>(value)!;
            }
            else if (section.StartsWith("registry.", comparisonType: StringComparison.Ordinal))
            {
                int integer = int.Parse(value, provider: CultureInfo.InvariantCulture);
                result._settings.Add($@"\Software\Triumph Studios\Age of Wonders\{section[9..]}\{key}", s_booleans.Contains(key) ? integer != 0 : integer);
            }
            else if (section == "fixture")
            {
                current!.Add(key, value.StartsWith('"') ? JsonSerializer.Deserialize<string>(value)! : value);
            }
            else
            {
                throw new InvalidDataException($"Unknown fixture section {section}");
            }
        }
        Dictionary<string, string> match = fixtures.SingleOrDefault(f => f.GetValueOrDefault("name") == name) ?? throw new InvalidDataException($"Unknown fixture {name}");
        foreach (KeyValuePair<string, string> item in match)
        {
            result._values.Add(item.Key, item.Value);
        }

        return result;
    }

    private sealed record Click(int At, int X, int Y, int Button, bool Double, int? Key, string? Clock, int? Day, int? KeyDown, int Repeat, int? Hex, int? Hy, bool DoubleUI, bool MoveOnly);

    private List<Click> Clicks()
    {
        if (!_values.TryGetValue("clicks", out string? text))
        {
            return [];
        }

        using JsonDocument document = JsonDocument.Parse(Regex.Replace(text, pattern: @"(\w+)\s*=", replacement: "\"$1\":"));
        return [.. document.RootElement.EnumerateArray().Select(c => new Click(c.GetProperty("at").GetInt32(),
            c.TryGetProperty("x", out JsonElement x) ? x.GetInt32() : 0, c.TryGetProperty("y", out JsonElement y) ? y.GetInt32() : 0,
            Button: c.TryGetProperty("button", out JsonElement button) ? button.GetInt32() : 0,
            Double: c.TryGetProperty("double", out JsonElement twice) && twice.GetBoolean(),
            Key: c.TryGetProperty("key", out JsonElement key) ? key.GetInt32() : null,
            Clock: c.TryGetProperty("clock", out JsonElement clock) ? clock.GetString() : null,
            Day: c.TryGetProperty("day", out JsonElement day) ? day.GetInt32() : null,
            KeyDown: c.TryGetProperty("key_down", out JsonElement keyDown) ? keyDown.GetInt32() : null,
            Repeat: c.TryGetProperty("repeat", out JsonElement repeat) ? repeat.GetInt32() : 1,
            Hex: c.TryGetProperty("hx", out JsonElement hex) ? hex.GetInt32() : null,
            Hy: c.TryGetProperty("hy", out JsonElement hy) ? hy.GetInt32() : null,
            DoubleUI: c.TryGetProperty("double_ui", out JsonElement doubleUI) && doubleUI.GetBoolean(),
            MoveOnly: c.TryGetProperty("move_only", out JsonElement moveOnly) && moveOnly.GetBoolean()))];
    }

    internal sealed class FixtureHost : IRtlHost, IDialogService
    {
        public FixtureHost(string root, string user, Dictionary<string, object> settings, IClock clock, string? saveDirectory)
        {
            GameRoot = Path.GetFullPath(root);
            if (saveDirectory is not null)
            {
                _ = Directory.CreateDirectory(Path.Combine(user, saveDirectory));
            }

            Files = new LayeredFileSystem(GameRoot, userDirectory: user, userOnlyDirectories: saveDirectory is null ? [] : [saveDirectory]);
            Clock = clock;
            foreach (KeyValuePair<string, object> item in settings)
            {
                Settings.Write(item.Key, item.Value);
            }
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

        public int Show(DialogRequest request) => throw new InvalidOperationException($"Fixture dialog: {request.Text}");
    }
}
