using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json;
using Aow.Rtl;
using Aow.Units;
using Aow.Units.AoWE;
using Aow.Units.AoWTCReg;
using Game = Aow.Units.AoWE.AoWEUnit;
using Shim = Aow.Rtl.RtlUnit;

namespace Aow.Headless;

internal static class Program
{
    private static int Main(string[] args)
    {
        Aow.Rtl.ProcessSafety.Initialize();
        if (GateSwitches.CultureError("Headless") is { } culture)
        {
            Console.Error.WriteLine(culture);
            return 1;
        }
        GateSwitches.Enter("Headless", args);
        string? output = null;
        var saves = new List<object>();
        var todos = new SortedSet<string>(StringComparer.Ordinal);
        var exceptions = new HashSet<string>();
        var runtimeErrors = new SortedSet<string>(StringComparer.Ordinal);
        bool gameplay = false;
        bool writeManifest = false;
        int turns = 0, seed = 123456789, firstDay = 0, day = 0;
        long frames = 0;
        bool tactical = false, ended = false, complete = false;
        bool traceMovement = false, exerciseItems = false;
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
            for (int i = 0; i < args.Length; i++)
            {
                string Value() => ++i < args.Length ? args[i] : throw new ArgumentException("missing option value");
                switch (args[i])
                {
                    case "--load": input = Path.GetFullPath(Value()); break;
                    case "--out": output = Path.GetFullPath(Value()); break;
                    case "--seed":
                        string seedText = Value();
                        long seedValue = seedText.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                            ? long.Parse(seedText.AsSpan(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture)
                            : long.Parse(seedText, System.Globalization.CultureInfo.InvariantCulture);
                        if (seedValue < int.MinValue || seedValue > uint.MaxValue) throw new ArgumentOutOfRangeException("seed");
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
                        var dimensions = Value().Split('x');
                        viewportWidth = int.Parse(dimensions[0]);
                        viewportHeight = int.Parse(dimensions[1]);
                        break;
                    default: throw new ArgumentException("unknown option: " + args[i]);
                }
            }
            if (input is null || output is null || turns < 0) throw new ArgumentException("--load <save> --seed <n> --turns <n> --out <dir> [--tactical] [--file-log <path>]");
            Directory.CreateDirectory(output);
            if (Directory.EnumerateFiles(output, "day-*.asg").Any()) throw new IOException("output already contains day saves");
            writeManifest = true;
            trace = new StreamWriter(Path.Combine(output, "exceptions.log")) { NewLine = "\n" };
            AppDomain.CurrentDomain.FirstChanceException += (_, e) =>
            {
                string detail = (e.Exception.ToString() + (gameplay ? "\n" + new StackTrace(1, true) : "")).Replace("\r\n", "\n");
                if (e.Exception is NotImplementedException) todos.Add(detail);
                if (gameplay) runtimeErrors.Add(detail);
                if (exceptions.Add(detail)) { trace.WriteLine(detail); trace.Flush(); }
            };
            var host = new HeadlessHost(root, output, settings, movementSpeed, input);
            Shim.InstallHost(host);
            if (fileLogPath is not null)
            {
                // own-formats.md 4.2: every open and write at the file boundary, refusals included.
                var log = fileLog = new StreamWriter(fileLogPath, false, new System.Text.UTF8Encoding(false)) { NewLine = "\n" };
                FileBoundary.Log = record => { log.WriteLine(record.ToJson()); log.Flush(); };
            }
            if (randomTracePath is not null)
            {
                randomTrace = new(randomTracePath, () => frames);
                Shim.RandomTrace = randomTrace.Record;
                Shim.TraceRound = randomTrace.Rounding;
                HeadlessHooks.BeforeMove = randomTrace.Path;
                TList.TraceSort = randomTrace.Sort;
                HeadlessHooks.ProcessCost = randomTrace.Cost;
                if (traceMovement) HeadlessHooks.MoveLine = randomTrace.Line;
            }
            Startup.InitializeAll();
            Aow.Units.Sound.NullSoundSystem.Install();
            AoWTCRegUnit.RegisterTacticalCombat(Game.AoWEngine);
            gameplay = true;
            Game.AoWEngine.LoadHSM(input, false, null!, Aow.Formats.GameProfile.WorldMap);
            if (Game.AoWEngine.HSMap is not TAoWHSMap map)
                throw new InvalidDataException("engine load failed" + (FileBoundary.LastFailure is { } refused ? ": " + refused : ""));
            map.SetViewPort(map.ViewXhp, map.ViewYhp, map.ViewL, 0, 0);
            Shim.RandSeed = seed;
            Shim.SuppressRandomize = true;
            HeadlessHooks.Tactical = tactical;
            HeadlessHooks.TacticalBattles = tacticalBattles;
            directed = new(map, exerciseItems, globalSpell);
            if (globalSpell != 0) HeadlessHooks.BeforeAIPlayerProcess = directed.BeforeFrame;
            map.FGameOverEvents.Add(_ => ended = true);
            map.FExecuteEventLogEvents.Add(sender =>
            {
                var info = ((Aow.Units.AoWE.ExecuteEventLogNotification)sender).Info;
                if (info.Log is Aow.Units.EventLog.TMessageEventLog message)
                {
                    bool yes = message.FButtons is Aow.Units.EventLog.TMessageEventLogButtons.mebYesNo or Aow.Units.EventLog.TMessageEventLogButtons.mebYesNoCancel;
                    host.Show(new(message.FMessage.Text, message.FTitle, (int)message.FButtons, yes ? 6 : 1));
                    if (yes) message.Yes(); else message.OK();
                }
                info.Done?.Invoke(info.Log);
            });
            void CheckTodos()
            {
                if (todos.Count != 0) throw new InvalidOperationException("reached TODO: " + string.Join("\n", todos));
                if (runtimeErrors.Count != 0) throw new InvalidOperationException("gameplay exception: " + string.Join("\n", runtimeErrors));
            }
            void Save(TPlayerControl control)
            {
                CheckTodos();
                day = map.FDay;
                if (firstDay == 0) { firstDay = day; directed.Initial(); }
                string name = $"day-{day:D2}.asg";
                host.FilesForRun.AutoSaveTarget = Path.Combine(output, name);
                control.AutoSave();
                randomTrace?.Save(map.FDay);
                directed.DaySaved();
                host.FilesForRun.AutoSaveTarget = null;
                CheckTodos();
                string file = Path.Combine(output, name);
                if (!File.Exists(file)) throw new IOException("AutoSave did not write " + name);
                saves.Add(new { day, file = name, sha256 = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(file))) });
                Console.WriteLine($"day {day}: {name}, frame {frames}, viewport {map.ViewWidth}x{map.ViewHeight}");
                Console.Out.Flush();
            }
            HeadlessHooks.AfterNewDay = Save;
            Console.WriteLine("activating map");
            map.Activate();
            Console.WriteLine("map activated");
            CheckTodos();
            if (saves.Count == 0) Save(map.FPlayerControl);
            map.UpdateFPS(animationFps);
            var viewer = new Aow.Units.HSEngine.THSMapViewer(null!);
            viewer.HSMap = map;
            viewer.SceneWidth = viewportWidth;
            viewer.SceneHeight = viewportHeight;
            viewer.ChangeScene(map.ViewXhp, map.ViewYhp, map.ViewL);
            TacticalInput.Install(map, viewer);
            var watchdog = Stopwatch.StartNew();
            int previous = day;
            while (!ended && map.FDay < firstDay + turns)
            {
                frames++;
                if (frames <= 3 || frames % 1000000 == 0)
                {
                    var player = map.FPlayers.GetPlayers(map.FCurrentPlayer);
                    Console.WriteLine($"frame {frames}, map {map.Status.Image:X}, player {map.FCurrentPlayer}, busy {player.GetBusy()}, ready {map.FTokenManager.PlayerReady(map.FCurrentPlayer)}, AI locks {map.FAILockCount}/{map.FAIExecuter.FLockCount}, AI {map.FAIExecuter.FStatus.Image:X}, phase {player.FAIControl.FPhase}, actions {player.FAIControl.FActions.Count}");
                }
                if (randomTrace is not null && frames % 100000 == 0 && map.FCombatMap is Aow.Units.AoWTC.TAoWCombatMap tacticalMap)
                    Console.WriteLine($"combat frame {tacticalMap.Frame}, status {tacticalMap.Status.Image:X}, player {tacticalMap.FCurrentCombatPlayer}, end pending {tacticalMap.FEndTurnPending}, AI {tacticalMap.FAI.Status}, effects {tacticalMap.FEffects.Count}, result {tacticalMap.FCombat.FCombatResult}");
                map.NewFrame();
                CheckTodos();
                if (day != previous) { previous = day; watchdog.Restart(); }
                if (watchdog.Elapsed > TimeSpan.FromMinutes(5)) throw new TimeoutException($"no day transition for five minutes at day {map.FDay}, frame {frames}");
            }
            complete = ended || saves.Count >= turns + 1;
            Console.WriteLine($"complete: {complete}, day {day}, turns {day - firstDay}, frames {frames}, TODOs reached {todos.Count}, dialogs {host.DialogCount}, game ended {ended}");
        }
        catch (System.Exception e)
        {
            error = e.ToString();
            Console.Error.WriteLine(error);
        }
        finally
        {
            if (writeManifest && output is not null && Directory.Exists(output))
                File.WriteAllText(Path.Combine(output, "run.json"), JsonSerializer.Serialize(new
                {
                    input, input_sha256 = input is not null && File.Exists(input) ? Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(input))) : null,
                    seed, requested_turns = turns, movement_speed = movementSpeed, animation_fps = animationFps, viewport_width = viewportWidth, viewport_height = viewportHeight, tactical, tactical_battles = tacticalBattles, exercise_items = exerciseItems, global_spell = globalSpell,
                    item_exercise = directed?.ItemActions, global_requests = directed?.GlobalRequests, tactical_selections = HeadlessHooks.TacticalSelections,
                    tactical_executions = HeadlessHooks.TacticalExecutions, complete, game_ended = ended,
                    first_day = firstDay, last_day = day, frames, saves, reached_todos = todos, gameplay_exceptions = runtimeErrors, error
                }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
            trace?.Flush();
            FileBoundary.Log = null;
            fileLog?.Dispose();
            Shim.RandomTrace = null;
            Shim.TraceRound = null;
            HeadlessHooks.BeforeMove = null;
            HeadlessHooks.BeforeAIPlayerProcess = null;
            TList.TraceSort = null;
            HeadlessHooks.ProcessCost = null;
            HeadlessHooks.MoveLine = null;
            randomTrace?.Dispose();
        }
        return complete && error is null ? 0 : 1;
    }
}
