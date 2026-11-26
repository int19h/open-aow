#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Tree;
using Aow.Game;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Platform.Logging;
using Aow.Rtl;
using Aow.Rtl.Host;
using Microsoft.Extensions.Logging;

namespace Aow.Headless;

internal static class Program
{
    private static int Main(string[] arguments)
    {
        ProcessSafety.Initialize();
        using ILoggerFactory loggerFactory = HostLogging.Create(LogDirectory(arguments), honorEnvironmentOverrides: false);
        GlobalExceptionHandlers.Install(loggerFactory);
        RtlHost.LoggerFactory = loggerFactory;
        ILogger logger = loggerFactory.CreateLogger("OpenAoW.Headless");
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
            return PersistenceMeasurement.Run(Option("--game-root") ?? Environment.GetEnvironmentVariable("AOW_GAME_ROOT") ?? @"D:\Games\GOG\Age of Wonders", kind: arguments[1], file: arguments[2], map: Option("--map"), output: Option("--out") ?? throw new ArgumentException("--measure-persistence needs --out"), loggerFactory: loggerFactory);
        }
        string? output = null;
        List<object> saves = [];
        SortedSet<string> todos = [with(StringComparer.Ordinal)];
        HashSet<string> exceptions = [];
        SortedSet<string> runtimeErrors = [with(StringComparer.Ordinal)];
        SortedSet<string> loadWindowExceptions = [with(StringComparer.Ordinal)];
        List<DroppedObject> dropped = [];
        bool gameplay = false;
        bool writeManifest = false;
        int turns = 0, seed = 123456789, firstDay = 0, day = 0;
        long frames = 0;
        bool tactical = false, ended = false, complete = false;
        bool exerciseItems = false;
        int tacticalBattles = 0, globalSpell = 0, dispel = 0;
        string endEnchantment = null;
        LeaderAttackTarget? leaderAttack = null;
        PlayerType? campaignSeatType = null;
        int campaignSeat = -1;
        DirectedInputs? directed = null;
        int viewportWidth = 1600, viewportHeight = 937, animationFps = 30;
        string? error = null, input = null;
        string movementSpeed = "normal";
        bool cpuHumans = false;
        string nextScenario = null;
        string outcome = null;
        bool betweenMaps = false;
        string[] offeredScenarios = [];
        StrategicMap playedMap = null;
        RunEnd end = null;
        StreamWriter? fileLog = null;
        RandomTrace? randomTrace = null;
        try
        {
            HostRequest request = HostRequest.Parse(arguments);
            input = request.Input;
            output = request.Output;
            seed = request.Seed;
            turns = request.Turns;
            string root = request.Root;
            string settings = request.Settings;
            movementSpeed = request.MovementSpeed;
            tactical = request.Tactical;
            exerciseItems = request.ExerciseItems;
            tacticalBattles = request.TacticalBattles;
            globalSpell = request.GlobalSpell;
            dispel = request.Dispel;
            endEnchantment = request.EndEnchantment;
            leaderAttack = request.LeaderAttack;
            campaignSeatType = request.CampaignSeatType;
            string randomTracePath = request.RandomTracePath;
            string fileLogPath = request.FileLogPath;
            bool traceMovement = request.TraceMovement;
            animationFps = request.AnimationFps;
            viewportWidth = request.ViewportWidth;
            viewportHeight = request.ViewportHeight;
            cpuHumans = request.CpuHumans;
            nextScenario = request.NextScenario;
            HeadlessHooks.DriveHumanSeats = cpuHumans;

            _ = Directory.CreateDirectory(output);
            if (Directory.EnumerateFiles(output, searchPattern: "day-*.asg").Any())
            {
                throw new IOException("output already contains day saves");
            }

            writeManifest = true;
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
                    HostLog.FirstChanceException(logger, detail: detail);
                }
            };
            HeadlessHost host = new(root, output, settings, movementSpeed, input, loggerFactory: loggerFactory);
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
            GameEngine engine = Startup.CreateEngine(new HeadlessPlayerInput());
            NullSoundSystem.Install();
            CombatClasses.Register(engine);
            // FormCreate loads the release set before any campaign or map load. The transfer list
            // resolves unit indices against that set, and a later map load of the same file keeps it.
            engine.InitializeHexSpriteSet($"{engine.StartupDirectory}RELEASE\\RELEASE.HSS", progress: null);
            gameplay = true;
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

            // The dropped list is the partial-load evidence. Each entry is one child the loader
            // dropped, with its class, path and message. AcceptLoad copies the first-chance
            // exceptions from the load window into the diagnostic set and clears them, so a
            // handled load exception does not fail the turn loop. That diagnostic set does not
            // decide the outcome. An exception after the load still fails the loop.
            void AcceptLoad()
            {
                foreach (string detail in runtimeErrors)
                {
                    _ = loadWindowExceptions.Add(detail);
                }

                runtimeErrors.Clear();
            }

            SaveKind kind = SaveDocument.Kind(input);
            if (kind == SaveKind.WorldMap && nextScenario is not null)
            {
                throw new ArgumentException("--next-scenario applies to a campaign save.");
            }

            if (campaignSeatType is not null && nextScenario is null)
            {
                throw new ArgumentException("--campaign-seat-type applies to a campaign transfer (--next-scenario).");
            }

            bool play = true;
            if (kind == SaveKind.Campaign)
            {
                CampaignPlay.Register(engine);
                engine.LoadCampaign(input, dropped: dropped);
                AcceptLoad();
                if (engine.Campaign is not Campaign campaign)
                {
                    throw new InvalidDataException($"engine load failed{(FileBoundary.LastFailure is { } refused ? $": {refused}" : "")}");
                }

                switch (CampaignPlay.Choose(campaign.ScenarioStarted, nextScenario: nextScenario))
                {
                    case CampaignStep.PlayEmbeddedMap:
                        engine.LoadCampaignMap(null, dropped: dropped);
                        AcceptLoad();
                        break;
                    case CampaignStep.BetweenMaps:
                        play = false;
                        CheckTodos();
                        betweenMaps = true;
                        offeredScenarios = [.. ScenarioOffers.From(campaign).Select(static offer => ScenarioOffers.Label(offer))];
                        complete = true;
                        HostLog.Information(logger, text: $"campaign is between maps. Offered: {string.Join(separator: ", ", values: offeredScenarios)}");
                        break;
                    case CampaignStep.StartNext:
                        _ = CampaignPlay.StartNext(engine, name: nextScenario, dropped: dropped);
                        AcceptLoad();
                        if (campaignSeatType is { } seatType)
                        {
                            campaignSeat = CampaignPlay.SeatAsCpu(engine, seatType: seatType);
                        }

                        break;
                    default:
                        throw new InvalidOperationException("Unknown campaign step.");
                }
            }
            else
            {
                engine.LoadHsm(input, edit: false, progress: null, profile: GameProfile.WorldMap, dropped: dropped);
                AcceptLoad();
            }

            if (play)
            {
                if (engine.HexSpriteMap is not StrategicMap map)
                {
                    throw new InvalidDataException($"engine load failed{(FileBoundary.LastFailure is { } refused ? $": {refused}" : "")}");
                }

                playedMap = map;

                _ = map.SetViewPort(map.ViewPixelX, map.ViewPixelY, lHitPoints: map.ViewMapLevel, 0, 0);
                DelphiRandom.RandSeed = seed;
                DelphiRandom.SuppressRandomize = true;
                HeadlessHooks.Tactical = tactical;
                HeadlessHooks.TacticalBattles = tacticalBattles;
                directed = new(map, items: exerciseItems, spell: globalSpell, dispel: dispel, endEnchantment: endEnchantment, leaderAttack: leaderAttack);
                if (directed.HasTurnInputs)
                {
                    HeadlessHooks.BeforeAIPlayerProcess = directed.BeforeFrame;
                }

                map.GameEnded += _ =>
                {
                    // The native harness stops at TriggerGameTerminatedEvent and saves once more as
                    // final-day-NN (playtest.py). The port saves at the same event.
                    if (!ended && saves.Count != 0)
                    {
                        FinalSave(map.PlayerControl);
                    }

                    ended = true;
                };
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
                void Save(PlayerControl control)
                {
                    CheckTodos();
                    day = map.Day;
                    if (firstDay == 0)
                    {
                        firstDay = day;
                        directed.Initial();
                    }
                    string name = $"day-{day:D2}{DayExtension(map)}";
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
                    HostLog.Information(logger, text: $"day {day}: {name}, frame {frames}, viewport {map.ViewWidth}x{map.ViewHeight}");
                }
                void FinalSave(PlayerControl control)
                {
                    string name = $"final-day-{map.Day:D2}{DayExtension(map)}";
                    string file = Path.Combine(output, name);
                    host.FilesForRun.AutoSaveTarget = file;
                    control.AutoSave();
                    host.FilesForRun.AutoSaveTarget = null;
                    if (!File.Exists(file))
                    {
                        throw new IOException($"AutoSave did not write {name}");
                    }

                    saves.Add(new { day = map.Day, file = name, sha256 = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(file))) });
                    HostLog.Information(logger, text: $"game ended on day {map.Day}: {name}, frame {frames}");
                }
                HeadlessHooks.AfterNewDay = Save;
                HostLog.Information(logger, text: "activating map");
                map.Activate();
                if (cpuHumans)
                {
                    CpuSeats.Apply(map);
                }

                HostLog.Information(logger, text: "map activated");
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
                        HostLog.Information(logger, text: $"frame {frames}, map {(ulong)map.Status:X}, player {PlayerIdExtensions.ToOriginal(map.CurrentPlayer)}, busy {player.GetBusy()}, ready {map.TokenManager.PlayerReady(map.CurrentPlayer)}, AI locks {map.AILockCount}/{map.AIExecutor.LockCount}, AI {(ulong)map.AIExecutor.Status:X}, phase {player.AIControl.Phase}, actions {player.AIControl.Actions.Count}");
                    }
                    if (randomTrace is not null && frames % 100000 == 0 && map.CombatMap is CombatMap tacticalMap)
                    {
                        HostLog.Information(logger, text: $"combat frame {tacticalMap.Frame}, status {(ulong)tacticalMap.Status:X}, player {tacticalMap.CurrentCombatPlayer}, end pending {tacticalMap.EndTurnPending}, AI {tacticalMap.AI.Status}, effects {tacticalMap.Effects.Count}, result {tacticalMap.Combat.CombatResult}");
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
                directed?.AssertKnownResults(turns);
                complete = ended || saves.Count >= turns + 1;

                HostLog.Information(logger, text: $"complete: {complete}, day {day}, turns {day - firstDay}, frames {frames}, TODOs reached {todos.Count}, dialogs {host.DialogCount}, game ended {ended}");
            }
        }
        catch (Exception e)
        {
            error = e.ToString();
            HostLog.HostError(logger, error: e, location: "Headless.Program.Main");
        }
        finally
        {
            outcome = RunOutcome.Name(complete, droppedChildren: dropped.Count != 0, betweenMaps: betweenMaps);

            if (playedMap is not null)
            {
                end = RunEnd.Capture(playedMap, ended);
            }

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
                    dispel_spell = dispel,
                    end_enchantment_mode = endEnchantment,
                    leader_attack_target = leaderAttack is { } attack ? $"{attack.Player}:{attack.UnitId}" : null,
                    campaign_seat_type = campaignSeatType is { } seat ? (int?)(byte)seat : null,
                    campaign_seat = campaignSeat,
                    cpu_humans = cpuHumans,
                    next_scenario = nextScenario,
                    outcome,
                    between_maps = betweenMaps,
                    game_result = end?.GameResult,
                    active_players = end?.ActivePlayers,
                    players = end?.Players.Select(static player => new
                    {
                        id = player.Id,
                        type = player.Type,
                        status = player.Status,
                        game_over = player.GameOver,
                    }),
                    end_how = end?.How,
                    offered_scenarios = offeredScenarios,
                    item_exercise = directed?.ItemActions,
                    global_requests = directed?.GlobalRequests,
                    dispel = directed?.DispelRecord,
                    end_enchantment = directed?.EndRecord,
                    leader_attack = directed?.AttackRecord,
                    tactical_selections = HeadlessHooks.TacticalSelections,
                    tactical_executions = HeadlessHooks.TacticalExecutions,
                    complete,
                    game_ended = ended,
                    first_day = firstDay,
                    last_day = day,
                    frames,
                    saves,
                    reached_todos = todos,
                    dropped_objects = DroppedObjects.Record(dropped),
                    load_window_exceptions = loadWindowExceptions,
                    gameplay_exceptions = runtimeErrors,
                    error
                }, options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
            }

            FileBoundary.Log = null;
            fileLog?.Dispose();
            DelphiRandom.RandomTrace = null;
            DelphiRounding.TraceRound = null;
            HeadlessHooks.DriveHumanSeats = false;
            HeadlessHooks.BeforeMove = null;
            HeadlessHooks.BeforeAIPlayerProcess = null;
            DelphiList<EngineObject>.TraceSort = null;
            HeadlessHooks.ProcessCost = null;
            HeadlessHooks.MoveLine = null;
            randomTrace?.Dispose();
        }
        return RunOutcome.Succeeds(complete, failed: error is not null, transfer: nextScenario is not null, droppedChildren: dropped.Count != 0) ? 0 : 1;
    }

    // A campaign map's AutoSave writes the campaign (PlayerC.PAS:133), and the native harness
    // names those days .acg.
    private static string DayExtension(StrategicMap map) => map.GameType == GameType.Campaign ? ".acg" : ".asg";

    private static string LogDirectory(string[] arguments)
    {
        try
        {
            int output = Array.IndexOf(arguments, value: "--out");
            return output >= 0 && output + 1 < arguments.Length
                ? Path.GetFullPath(arguments[output + 1])
                : Path.Combine(Environment.CurrentDirectory, "logs");
        }
        catch
        {
            return Path.Combine(Environment.CurrentDirectory, "logs");
        }
    }
}
