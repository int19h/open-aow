using System.Diagnostics;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Game;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow.Headless;

internal static class Program
{
    private static int Main(string[] arguments)
    {
        ProcessSafety.Initialize();
        if (GateSwitches.CultureError("Headless") is { } culture)
        {
            Console.Error.WriteLine(culture);
            return 1;
        }
        GateSwitches.Enter("Headless", arguments);
        // --measure-persistence <map|set|campaign> <file> [--map <file>] --out <dir> (own-formats.md 9.3).
        if (arguments.Length >= 3 && arguments[0] == "--measure-persistence")
        {
            string? Option(string name) => Array.IndexOf(arguments, value: name) is int at and >= 0 && at + 1 < arguments.Length ? arguments[at + 1] : null;
            return PersistenceMeasurement.Run(Option("--game-root") ?? Environment.GetEnvironmentVariable("AOW_GAME_ROOT") ?? @"D:\Games\GOG\Age of Wonders", kind: arguments[1], file: arguments[2], map: Option("--map"), output: Option("--out") ?? throw new ArgumentException("--measure-persistence needs --out"));
        }
        string? output = null;
        List<object> saves = [];
        SortedSet<string> todos = [with(StringComparer.Ordinal)];
        HashSet<string> exceptions = [];
        SortedSet<string> runtimeErrors = [with(StringComparer.Ordinal)];
        bool gameplay = false;
        bool writeManifest = false;
        int turns = 0, seed = 123456789, firstDay = 0, day = 0;
        long frames = 0;
        bool tactical = false, ended = false, complete = false;
        bool exerciseItems = false;
        int tacticalBattles = 0, globalSpell = 0;
        DirectedInputs? directed = null;
        int viewportWidth = 1600, viewportHeight = 937, animationFps = 30;
        string? error = null, input = null;
        string movementSpeed = "normal";
        StreamWriter? trace = null;
        StreamWriter? fileLog = null;
        RandomTrace? randomTrace = null;
        try
        {
            string root = Environment.GetEnvironmentVariable("AOW_GAME_ROOT") ?? @"D:\Games\GOG\Age of Wonders";
            string? settings = null;
            string? randomTracePath = null;
            string? fileLogPath = null;
            bool traceMovement = false;
            for (int i = 0; i < arguments.Length; i++)
            {
                string Value() => ++i < arguments.Length ? arguments[i] : throw new ArgumentException("missing option value");
                switch (arguments[i])
                {
                    case "--load": input = Path.GetFullPath(Value()); break;
                    case "--out": output = Path.GetFullPath(Value()); break;
                    case "--seed":
                        string seedText = Value();
                        long seedValue = seedText.StartsWith("0x", comparisonType: StringComparison.OrdinalIgnoreCase)
                            ? long.Parse(seedText.AsSpan(2), style: NumberStyles.HexNumber, provider: CultureInfo.InvariantCulture)
                            : long.Parse(seedText, provider: CultureInfo.InvariantCulture);
                        if (seedValue is < int.MinValue or > uint.MaxValue)
                        {
                            throw new ArgumentOutOfRangeException("seed");
                        }

                        seed = unchecked((int)seedValue);
                        break;
                    case "--turns": turns = int.Parse(Value()); break;
                    case "--game-root": root = Path.GetFullPath(Value()); break;
                    case "--settings": settings = Value(); break;
                    case "--movement-speed": movementSpeed = Value(); break;
                    case "--tactical": tactical = true; break;
                    case "--exercise-items": exerciseItems = true; break;
                    case "--tactical-battles": tacticalBattles = int.Parse(Value()); break;
                    case "--global-spell": globalSpell = int.Parse(Value()); break;
                    case "--trace-random": randomTracePath = Path.GetFullPath(Value()); break;
                    case "--trace-movement": traceMovement = true; break;
                    case "--file-log": fileLogPath = Path.GetFullPath(Value()); break;
                    case "--animation-fps": animationFps = int.Parse(Value()); break;
                    case "--viewport":
                        string[] dimensions = Value().Split('x');
                        viewportWidth = int.Parse(dimensions[0]);
                        viewportHeight = int.Parse(dimensions[1]);
                        break;
                    default: throw new ArgumentException($"unknown option: {arguments[i]}");
                }
            }
            if (input is null || output is null || turns < 0)
            {
                throw new ArgumentException("--load <save> --seed <n> --turns <n> --out <dir> [--tactical] [--file-log <path>]");
            }

            _ = Directory.CreateDirectory(output);
            if (Directory.EnumerateFiles(output, searchPattern: "day-*.asg").Any())
            {
                throw new IOException("output already contains day saves");
            }

            writeManifest = true;
            trace = new StreamWriter(Path.Combine(output, "exceptions.log")) { NewLine = "\n" };
            AppDomain.CurrentDomain.FirstChanceException += (sender, e) =>
            {
                string detail = (e.Exception.ToString() + (gameplay ? $"\n{new StackTrace(1, fNeedFileInfo: true)}" : "")).Replace("\r\n", "\n");
                if (e.Exception is NotImplementedException)
                {
                    _ = todos.Add(detail);
                }

                if (gameplay)
                {
                    _ = runtimeErrors.Add(detail);
                }

                if (exceptions.Add(detail))
                {
                    trace.WriteLine(detail);
                    trace.Flush();
                }
            };
            HeadlessHost host = new(root, output, settings, movementSpeed, input);
            RtlHost.InstallHost(host);
            if (fileLogPath is not null)
            {
                // own-formats.md 4.2: every open and write at the file boundary, refusals included.
                StreamWriter log = fileLog = new StreamWriter(fileLogPath, append: false, encoding: new UTF8Encoding(false)) { NewLine = "\n" };
                FileBoundary.Log = record =>
                {
                    log.WriteLine(record.ToJson());
                    log.Flush();
                };
            }
            if (randomTracePath is not null)
            {
                randomTrace = new(randomTracePath, () => frames);
                DelphiRandom.RandomTrace = randomTrace.Record;
                DelphiRounding.TraceRound = randomTrace.Rounding;
                HeadlessHooks.BeforeMove = randomTrace.Path;
                DelphiList<EngineObject>.TraceSort = randomTrace.Sort;
                HeadlessHooks.ProcessCost = randomTrace.Cost;
                if (traceMovement)
                {
                    HeadlessHooks.MoveLine = randomTrace.Line;
                }
            }
            Startup.InitializeAll();
            GameEngine engine = Startup.CreateEngine();
            NullSoundSystem.Install();
            CombatClasses.Register(engine);
            gameplay = true;
            engine.LoadHsm(input, edit: false, progress: null, profile: GameProfile.WorldMap);
            if (engine.HexSpriteMap is not StrategicMap map)
            {
                throw new InvalidDataException($"engine load failed{(FileBoundary.LastFailure is { } refused ? $": {refused}" : "")}");
            }

            _ = map.SetViewPort(map.ViewPixelX, map.ViewPixelY, lHitPoints: map.ViewMapLevel, 0, 0);
            DelphiRandom.RandSeed = seed;
            DelphiRandom.SuppressRandomize = true;
            HeadlessHooks.Tactical = tactical;
            HeadlessHooks.TacticalBattles = tacticalBattles;
            directed = new(map, items: exerciseItems, spell: globalSpell);
            if (globalSpell != 0)
            {
                HeadlessHooks.BeforeAIPlayerProcess = directed.BeforeFrame;
            }

            map.GameEnded += _ => ended = true;
            map.ExecutingEventLog += notification =>
            {
                ExecuteEventLogInfo info = notification.Info;
                if (info.Log is MessageEventLog message)
                {
                    bool yes = message.Buttons is MessageEventLogButtons.YesNo or MessageEventLogButtons.YesNoCancel;
                    _ = host.Show(new(message.Message.Text, Caption: message.Title, Flags: (int)message.Buttons, DefaultResult: yes ? 6 : 1));
                    _ = yes ? message.Yes() : message.Ok();
                }
                info.Done?.Invoke(info.Log);
            };
            void CheckTodos()
            {
                if (todos.Count != 0)
                {
                    throw new InvalidOperationException($"reached TODO: {string.Join("\n", values: todos)}");
                }

                if (runtimeErrors.Count != 0)
                {
                    throw new InvalidOperationException($"gameplay exception: {string.Join("\n", values: runtimeErrors)}");
                }
            }
            void Save(PlayerControl control)
            {
                CheckTodos();
                day = map.Day;
                if (firstDay == 0)
                {
                    firstDay = day;
                    directed.Initial();
                }
                string name = $"day-{day:D2}.asg";
                host.FilesForRun.AutoSaveTarget = Path.Combine(output, name);
                control.AutoSave();
                randomTrace?.Save(map.Day);
                directed.DaySaved();
                host.FilesForRun.AutoSaveTarget = null;
                CheckTodos();
                string file = Path.Combine(output, name);
                if (!File.Exists(file))
                {
                    throw new IOException($"AutoSave did not write {name}");
                }

                saves.Add(new { day, file = name, sha256 = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(file))) });
                Console.WriteLine($"day {day}: {name}, frame {frames}, viewport {map.ViewWidth}x{map.ViewHeight}");
                Console.Out.Flush();
            }
            HeadlessHooks.AfterNewDay = Save;
            Console.WriteLine("activating map");
            map.Activate();
            Console.WriteLine("map activated");
            CheckTodos();
            if (saves.Count == 0)
            {
                Save(map.PlayerControl);
            }

            map.UpdateFPS(animationFps);
            HexSpriteMapViewer viewer = new(null)
            {
                HexSpriteMap = map,
                SceneWidth = viewportWidth,
                SceneHeight = viewportHeight
            };
            viewer.ChangeScene(map.ViewPixelX, map.ViewPixelY, lHitPoints: map.ViewMapLevel);
            TacticalInput.Install(map, viewer);
            Stopwatch watchdog = Stopwatch.StartNew();
            int previous = day;
            while (!ended && map.Day < firstDay + turns)
            {
                frames++;
                if (frames <= 3 || frames % 1000000 == 0)
                {
                    Player player = map.Players.GetPlayers(map.CurrentPlayer);
                    Console.WriteLine($"frame {frames}, map {map.Status.Image:X}, player {map.CurrentPlayer}, busy {player.GetBusy()}, ready {map.TokenManager.PlayerReady(map.CurrentPlayer)}, AI locks {map.AILockCount}/{map.AIExecutor.LockCount}, AI {map.AIExecutor.Status.Image:X}, phase {player.AIControl.Phase}, actions {player.AIControl.Actions.Count}");
                }
                if (randomTrace is not null && frames % 100000 == 0 && map.CombatMap is CombatMap tacticalMap)
                {
                    Console.WriteLine($"combat frame {tacticalMap.Frame}, status {tacticalMap.Status.Image:X}, player {tacticalMap.CurrentCombatPlayer}, end pending {tacticalMap.EndTurnPending}, AI {tacticalMap.AI.Status}, effects {tacticalMap.Effects.Count}, result {tacticalMap.Combat.CombatResult}");
                }

                map.NewFrame();
                CheckTodos();
                if (day != previous)
                {
                    previous = day;
                    watchdog.Restart();
                }
                if (watchdog.Elapsed > TimeSpan.FromMinutes(5))
                {
                    throw new TimeoutException($"no day transition for five minutes at day {map.Day}, frame {frames}");
                }
            }
            complete = ended || saves.Count >= turns + 1;
            Console.WriteLine($"complete: {complete}, day {day}, turns {day - firstDay}, frames {frames}, TODOs reached {todos.Count}, dialogs {host.DialogCount}, game ended {ended}");
        }
        catch (Exception e)
        {
            error = e.ToString();
            Console.Error.WriteLine(error);
        }
        finally
        {
            if (writeManifest && output is not null && Directory.Exists(output))
            {
                File.WriteAllText(Path.Combine(output, "run.json"), contents: JsonSerializer.Serialize(new
                {
                    input,
                    input_sha256 = input is not null && File.Exists(input) ? Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(input))) : null,
                    seed,
                    requested_turns = turns,
                    movement_speed = movementSpeed,
                    animation_fps = animationFps,
                    viewport_width = viewportWidth,
                    viewport_height = viewportHeight,
                    tactical,
                    tactical_battles = tacticalBattles,
                    exercise_items = exerciseItems,
                    global_spell = globalSpell,
                    item_exercise = directed?.ItemActions,
                    global_requests = directed?.GlobalRequests,
                    tactical_selections = HeadlessHooks.TacticalSelections,
                    tactical_executions = HeadlessHooks.TacticalExecutions,
                    complete,
                    game_ended = ended,
                    first_day = firstDay,
                    last_day = day,
                    frames,
                    saves,
                    reached_todos = todos,
                    gameplay_exceptions = runtimeErrors,
                    error
                }, options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
            }

            trace?.Flush();
            FileBoundary.Log = null;
            fileLog?.Dispose();
            DelphiRandom.RandomTrace = null;
            DelphiRounding.TraceRound = null;
            HeadlessHooks.BeforeMove = null;
            HeadlessHooks.BeforeAIPlayerProcess = null;
            DelphiList<EngineObject>.TraceSort = null;
            HeadlessHooks.ProcessCost = null;
            HeadlessHooks.MoveLine = null;
            randomTrace?.Dispose();
        }
        return complete && error is null ? 0 : 1;
    }
}
