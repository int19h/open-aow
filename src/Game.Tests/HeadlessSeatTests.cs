using System.Reflection;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Audio;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Headless;

namespace Aow.Game.Tests;

/// <summary>
/// Map-level seat handoff and a campaign document loaded through the host's registration.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class HeadlessSeatTests
{
    [Fact]
    public void ATurnGameHandsTheOpenHumanAndLeavesTheSeatHuman()
    {
        using SeatMap seats = new();
        seats.Map.SetupPlayerControl(PlayMode.Turn, connection: PlayerConnectionType.Local);
        seats.Map.CurrentPlayer = PlayerId.Player1;
        seats.Map.RequestCombatType = true;
        seats.Map.ShowIndependentMoves = false;
        seats.PlayingHumans();

        CpuSeats.Apply(seats.Map);

        Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
        Assert.Equal(PlayerType.Human, seats.Player(2).PlayerType);
        Assert.Equal(2, seats.Map.Players.LocalHumanPlayers.Count);
        Assert.Equal((byte)1, seats.Map.Players.LocalHumanPlayers[0]);
        Assert.Equal((byte)2, seats.Map.Players.LocalHumanPlayers[1]);
        Assert.True(seats.Map.RequestCombatType);
        Assert.False(seats.Map.ShowIndependentMoves);
        Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player1));
        Assert.False(seats.Map.AIExecutor.Drives(PlayerId.Player2));
    }

    [Fact]
    public void ASimultaneousGameHandsEveryPlayingHuman()
    {
        using SeatMap seats = new();
        seats.Map.SetupPlayerControl(PlayMode.Simultaneous, connection: PlayerConnectionType.Local);
        seats.Map.CurrentPlayer = PlayerId.Independent;
        seats.Map.RequestCombatType = false;
        seats.Map.ShowIndependentMoves = false;
        seats.PlayingHumans();

        CpuSeats.Apply(seats.Map);

        Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
        Assert.Equal(PlayerType.Human, seats.Player(2).PlayerType);
        Assert.Equal(2, seats.Map.Players.LocalHumanPlayers.Count);
        Assert.False(seats.Map.RequestCombatType);
        Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player1));
        Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player2));
        Assert.False(seats.Map.AIExecutor.Drives(PlayerId.Independent));
    }

    [Fact]
    public void TheLoadedMapHookRunsBeforeThePlayerControlAndANullHookIsSkipped()
    {
        // A bare map has no token executor for the turn control to activate, so the player control
        // fails after it starts. The hook has to run before that, and a null hook has to reach it too.
        using SeatMap seats = new();
        seats.Map.SetupPlayerControl(PlayMode.Turn, connection: PlayerConnectionType.Local);
        seats.Map.CurrentPlayer = PlayerId.Player1;
        seats.PlayingHumans();
        List<string> calls = [];
        HeadlessHooks.BeforeStartLoadedMap = map =>
        {
            Assert.Same(seats.Map, map);
            calls.Add("hook");
        };
        Exception? hooked;
        try
        {
            hooked = Record.Exception(seats.Map.StartLoadedMap);
        }
        finally
        {
            HeadlessHooks.BeforeStartLoadedMap = null;
        }

        Assert.Equal(["hook"], calls);
        AssertPlayerControlRan(hooked);
        AssertPlayerControlRan(Record.Exception(seats.Map.StartLoadedMap));
        Assert.Equal(["hook"], calls);
    }

    private static void AssertPlayerControlRan(Exception? error)
    {
        if (error is not null)
        {
            Assert.Contains($"{nameof(TurnPlayerControl)}.{nameof(TurnPlayerControl.StartLoadedMap)}", error.StackTrace);
            Assert.DoesNotContain(nameof(HeadlessHooks), error.StackTrace);
        }
    }

    [Fact]
    public void TheVictorySeatPlaysAsCpuAndTurnsHumanAtItsWinOnly()
    {
        using SeatMap seats = new();
        seats.PlayingHumans();
        seats.Player(2).PlayerType = PlayerType.Cpu1;
        for (int i = 3; i < seats.Map.Players.Count; i++)
        {
            if (seats.Map.Players.GetPlayers(i) is { } other)
            {
                other.PlayerType = PlayerType.Cpu1;
            }
        }

        seats.Map.Players.Update();
        CampaignVictory victory = new(EngineFixture.Engine, seatType: PlayerType.Cpu5);

        victory.BeforeStartLoadedMap(seats.Map);

        Assert.Equal(1, victory.Seat);
        Assert.Equal(PlayerType.Cpu5, seats.Player(1).PlayerType);
        // Only the type changes. The lists keep the seat from activation, so it is seated as the human.
        Assert.Equal((byte)1, Assert.Single(seats.Map.Players.LocalHumanPlayers));

        victory.BeforeSetGameOverStatus(seats.Player(1), status: GameOverStatus.Lost);
        victory.BeforeSetGameOverStatus(seats.Player(2), status: GameOverStatus.Won);
        Assert.Null(victory.Day);
        Assert.Equal(PlayerType.Cpu5, seats.Player(1).PlayerType);

        seats.Map.Players.Update();
        Assert.Empty(seats.Map.Players.LocalHumanPlayers);
        victory.BeforeSetGameOverStatus(seats.Player(1), status: GameOverStatus.Won);

        Assert.Equal(seats.Map.Day, victory.Day);
        Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
        Assert.Equal((byte)1, Assert.Single(seats.Map.Players.LocalHumanPlayers));
        // No campaign is loaded, so no finished save is recorded.
        victory.Finish();
        Assert.Null(victory.FinishedName);
        Assert.Null(victory.Finished);
    }

    [Fact]
    public void TheVictoryRefusesASeatThatIsNotHuman()
    {
        using SeatMap seats = new();
        seats.PlayingHumans();
        for (int i = 1; i < seats.Map.Players.Count; i++)
        {
            if (seats.Map.Players.GetPlayers(i) is { } player)
            {
                player.PlayerType = PlayerType.Cpu2;
            }
        }

        CampaignVictory victory = new(EngineFixture.Engine, seatType: PlayerType.Cpu5);

        InvalidDataException error = Assert.Throws<InvalidDataException>(() => victory.BeforeStartLoadedMap(seats.Map));
        Assert.Contains("not the human campaign seat", error.Message);
        Assert.Equal(-1, victory.Seat);
        Assert.Equal(PlayerType.Cpu2, seats.Player(1).PlayerType);
    }

    [Fact]
    public void AStartedAndAFinishedCampaignChooseFromTheDocument()
    {
        GameEngine engine = EngineFixture.Engine;
        EngineClass[] hostClasses =
        [
            EngineClass.Of<CampaignScenarioPage>(),
            EngineClass.Of<WorldMapCampaignScenario>(),
            EngineClass.Of<WorldMapCampaign>(),
        ];
        List<EngineClass> already = [];
        foreach (EngineClass hostClass in hostClasses)
        {
            if (engine.ClassTable.ContainsKey(hostClass.ClassId))
            {
                already.Add(hostClass);
            }
        }

        string directory = Path.Combine(EngineFixture.Root, "Save");
        _ = Directory.CreateDirectory(directory);
        string startedPath = Path.Combine(directory, "headless-started.oacg");
        string finishedPath = Path.Combine(directory, "headless-finished.oacg");
        IFileSystem recorded = RtlHost.Files;
        PhysicalFileSystem physical = new();
        _ = physical.SetCurrentDirectory(EngineFixture.Root);
        RtlHost.Files = physical;
        try
        {
            // An earlier test on this engine can leave the world map classes installed. The host
            // registers its own copy, so this test removes that copy and lets CampaignPlay.Register
            // install the classes the load uses.
            if (already.Count != 0)
            {
                engine.UnregisterEngineClasses([.. already]);
            }

            CampaignPlay.Register(engine);
            Assert.True(engine.ClassTable.ContainsKey(WorldMapCampaign.ClassId));
            WriteCampaign(engine, path: startedPath, started: true, scenarioId: 2);
            engine.LoadCampaign(startedPath);
            WorldMapCampaign started = Loaded(engine);
            Assert.True(started.ScenarioStarted);
            Assert.Equal(2, started.CurrentScenarioId);
            Assert.Equal(PlayMode.Turn, started.PlayMode);
            Assert.Equal(CampaignStep.PlayEmbeddedMap, CampaignPlay.Choose(started.ScenarioStarted, nextScenario: null));

            engine.CloseCampaign();
            WriteCampaign(engine, path: finishedPath, started: false, scenarioId: 7);
            engine.LoadCampaign(finishedPath);
            WorldMapCampaign finished = Loaded(engine);
            Assert.False(finished.ScenarioStarted);
            Assert.Equal(7, finished.CurrentScenarioId);
            Assert.Equal(7, Assert.Single(finished.CompletedScenarios));
            Assert.Equal(CampaignStep.BetweenMaps, CampaignPlay.Choose(finished.ScenarioStarted, nextScenario: null));
            Assert.Equal(CampaignStep.StartNext, CampaignPlay.Choose(finished.ScenarioStarted, nextScenario: "7"));
        }
        finally
        {
            engine.CloseCampaign();
            RtlHost.Files = recorded;
            engine.UnregisterEngineClasses(hostClasses);
            if (already.Count != 0)
            {
                engine.RegisterEngineClasses([.. already]);
            }
            if (File.Exists(startedPath))
            {
                File.Delete(startedPath);
            }

            if (File.Exists(finishedPath))
            {
                File.Delete(finishedPath);
            }
        }
    }

    [Fact]
    public void ADrivenHumanReadsTheCpuAttackTableAndARealHumanReadsDiplomacy()
    {
        Assert.False(HeadlessHooks.DriveHumanSeats);
        using SeatMap seats = new();
        Player human = seats.Player(1);
        human.DiplomaticRelations.SetDiplomaticRelation(PlayerId.Player2, value: DiplomaticRelation.Peace);
        BitList attacks = human.AIValidAttackPlayers ?? throw new InvalidOperationException("The attack table is missing.");
        attacks[PlayerIdExtensions.ToOriginal(PlayerId.Player2)] = true;

        Assert.False(human.ValidAttackRelation(PlayerId.Player2, independentRelation: 0));
        Assert.Equal(PlayerType.Human, human.PlayerType);

        HeadlessHooks.DriveHumanSeats = true;
        try
        {
            Assert.True(human.ValidAttackRelation(PlayerId.Player2, independentRelation: 0));
            Assert.Equal(PlayerType.Human, human.PlayerType);
        }
        finally
        {
            HeadlessHooks.DriveHumanSeats = false;
        }
    }

    [Fact]
    public void ADrivenHumanPathsInAiModeAndARealHumanDoesNot()
    {
        Assert.False(HeadlessHooks.DriveHumanSeats);
        using SeatMap seats = new();
        seats.InstallGrid(1, 1);
        ArmyHexSprite sprite = new(seats.Field(0, 0));
        try
        {
            Army army = sprite.Army ?? throw new InvalidOperationException("The army is missing.");
            army.Player = PlayerId.Player1;
            Assert.False(AiPath(sprite));
            Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);

            HeadlessHooks.DriveHumanSeats = true;
            try
            {
                Assert.True(AiPath(sprite));
                Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
            }
            finally
            {
                HeadlessHooks.DriveHumanSeats = false;
            }
        }
        finally
        {
            sprite.Dispose();
        }
    }

    [Fact]
    public void ADrivenHumanSkipsExploredTerrainAndInvalidatesARejectedMove()
    {
        Assert.False(HeadlessHooks.DriveHumanSeats);
        using SeatMap seats = new();
        seats.InstallGrid(2, 1);
        ArmyHexSprite sprite = new(seats.Field(0, 0));
        MoveArmyTokenEvent token = new(null) { Holder = EngineFixture.Engine };
        RecordingMoveControl recording = new(seats.Map);
        MoveControl? previous = null;
        try
        {
            previous = InstallMoveControl(seats.Map, replacement: recording);
            Army army = sprite.Army ?? throw new InvalidOperationException("The army is missing.");
            army.Player = PlayerId.Player1;
            seats.Map.ExplorationEnabled = true;
            AIMoveExecutor executor = seats.Player(1).AIControl.MoveExecutor ?? throw new InvalidOperationException("The move executor is missing.");
            executor.Status = AIMoveExecutorStatus.Busy;
            MovePath path = [];
            _ = path.AddMapCoordinate(0, 0, mapLevel: 0);
            _ = path.AddMapCoordinate(1, 0, mapLevel: 0);
            _ = path.AddMapCoordinate(0, 0, mapLevel: 0);
            Assert.True(token.Setup(sprite, selection: 0, path: path, mode: ArmyMoveModes.None));

            Assert.False(token.Validate());
            Assert.True(recording.Seen.Contains(MoveModes.AI));
            Assert.True(recording.Seen.Contains(MoveModes.ExploredTerrainOnly));
            Assert.Equal(AIMoveExecutorStatus.Busy, executor.Status);
            Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);

            HeadlessHooks.DriveHumanSeats = true;
            try
            {
                Assert.False(token.Validate());
                Assert.True(recording.Seen.Contains(MoveModes.AI));
                Assert.False(recording.Seen.Contains(MoveModes.ExploredTerrainOnly));
                Assert.Equal(AIMoveExecutorStatus.Invalidated, executor.Status);
                Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
            }
            finally
            {
                HeadlessHooks.DriveHumanSeats = false;
            }
        }
        finally
        {
            if (previous is not null)
            {
                _ = InstallMoveControl(seats.Map, replacement: previous);
            }

            recording.Dispose();
            _ = token.Release();
            sprite.Dispose();
        }
    }

    [Fact]
    public void ADrivenHumanKeepsAFogAttackAndARealHumanWalks()
    {
        Assert.False(HeadlessHooks.DriveHumanSeats);
        using SeatMap seats = new();
        seats.InstallGrid(2, 1);
        seats.Tokens().Initialize(seats.Map.PlayerCommunicator);
        ArmyHexSprite enemy = new(seats.Field(0, 0));
        ArmyHexSprite mover = new(seats.Field(1, 0));
        try
        {
            Army moverArmy = mover.Army ?? throw new InvalidOperationException("The army is missing.");
            moverArmy.Player = PlayerId.Player1;
            MovePath path = [];
            _ = path.AddMapCoordinate(0, 0, mapLevel: 0);
            _ = path.AddMapCoordinate(1, 0, mapLevel: 0);

            Assert.True(ArmyHexSprite.MoveArmy(mover, selection: 0, path: path, mode: ArmyMoveModes.None, force: true));
            _ = Assert.IsType<ArmyDefaultMoveTokenEvent>(LastToken(seats, player: PlayerId.Player1));
            Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);

            HeadlessHooks.DriveHumanSeats = true;
            try
            {
                Assert.True(ArmyHexSprite.MoveArmy(mover, selection: 0, path: path, mode: ArmyMoveModes.None, force: true));
                _ = Assert.IsType<ArmyCombatMoveTokenEvent>(LastToken(seats, player: PlayerId.Player1));
                Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
            }
            finally
            {
                HeadlessHooks.DriveHumanSeats = false;
            }
        }
        finally
        {
            mover.Dispose();
            enemy.Dispose();
        }
    }

    [Fact]
    public void ADrivenHumanTurnActivatesTheSeatAndTheEndDeactivatesIt()
    {
        Assert.False(HeadlessHooks.DriveHumanSeats);
        using SeatMap seats = new();
        using QuietTurn turn = new();
        seats.InstallEmptyContainer();
        seats.Tokens().Initialize(seats.Map.PlayerCommunicator);
        seats.Map.Day = 2;
        seats.Map.DayLimit = 0;
        seats.Map.RequestCombatType = false;
        seats.PlayingHumans();
        seats.Player(2).PlayerType = PlayerType.Cpu1;
        _ = seats.Map.TurnOrder.Add(1);
        _ = seats.Map.TurnOrder.Add(2);
        seats.Map.CurrentPlayer = PlayerId.Player1;
        seats.Map.SynchronizeBegin();
        seats.Map.SetupPlayerControl(PlayMode.Turn, connection: PlayerConnectionType.Local);
        PlayerControl control = seats.Map.PlayerControl;

        control.NewTurn(PlayerId.Player1);
        Assert.False(seats.Map.AIExecutor.Drives(PlayerId.Player1));
        control.EndTurn(PlayerId.Player1);
        Assert.False(seats.Map.AIExecutor.Drives(PlayerId.Player1));
        Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player2));
        Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);

        HeadlessHooks.DriveHumanSeats = true;
        try
        {
            control.NewTurn(PlayerId.Player1);
            Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player1));
            control.EndTurn(PlayerId.Player1);
            Assert.False(seats.Map.AIExecutor.Drives(PlayerId.Player1));
            Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player2));
            Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
        }
        finally
        {
            HeadlessHooks.DriveHumanSeats = false;
        }
    }

    [Fact]
    public void ADrivenHumanDayActivatesTheSeatAndTheDayEndDeactivatesIt()
    {
        Assert.False(HeadlessHooks.DriveHumanSeats);
        using SeatMap seats = new();
        using QuietTurn turn = new();
        seats.InstallEmptyContainer();
        seats.Tokens().Initialize(seats.Map.PlayerCommunicator);
        seats.Map.Day = 2;
        seats.Map.DayLimit = 0;
        seats.Player(0).Status = PlayerStatus.Playing;
        seats.Player(1).Status = PlayerStatus.Playing;
        seats.Player(2).Status = PlayerStatus.Idle;
        seats.Map.Players.Update();
        seats.Map.SynchronizeBegin();
        seats.Map.SetupPlayerControl(PlayMode.Simultaneous, connection: PlayerConnectionType.Local);
        PlayerControl control = seats.Map.PlayerControl;

        control.NewTurn(PlayerId.Player1);
        Assert.False(seats.Map.AIExecutor.Drives(PlayerId.Player1));
        Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);

        HeadlessHooks.DriveHumanSeats = true;
        try
        {
            control.NewTurn(PlayerId.Player1);
            Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player1));
            Assert.Equal(PlayerId.Player1, seats.Map.AIExecutor.Player);
            control.EndTurn(PlayerId.Player1);
            Assert.True(seats.Map.AIExecutor.Drives(PlayerId.Player1));
            Assert.Equal(PlayerId.Independent, seats.Map.AIExecutor.Player);
            Assert.Equal(PlayerType.Human, seats.Player(1).PlayerType);
        }
        finally
        {
            HeadlessHooks.DriveHumanSeats = false;
        }
    }

    private static bool AiPath(ArmyHexSprite sprite)
    {
        MoveControlSettings settings = new(null) { Holder = EngineFixture.Engine };
        try
        {
            settings.SetupArmy(sprite, selection: 0);
            return settings.Mode.Contains(MoveModes.AI);
        }
        finally
        {
            _ = settings.Release();
        }
    }

    private static TokenEvent LastToken(SeatMap seats, PlayerId? player)
    {
        TokenControl control = seats.Tokens().GetTokenControl(player) ?? throw new InvalidOperationException("The token control is missing.");
        TokenEventList events = control.Events;
        return events.GetEvent(events.GetCount() - 1);
    }

    private static MoveControl InstallMoveControl(StrategicMap map, MoveControl replacement)
    {
        const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        FieldInfo? field = null;
        foreach (FieldInfo candidate in map.GetType().GetFields(flags))
        {
            if (candidate.FieldType == typeof(MoveControl))
            {
                field = candidate;
                break;
            }
        }

        if (field is null)
        {
            throw new InvalidOperationException("The move control field is missing.");
        }

        MoveControl previous = (MoveControl)(field.GetValue(map) ?? throw new InvalidOperationException("The move control is missing."));
        field.SetValue(map, value: replacement);
        return previous;
    }

    private static void WriteCampaign(GameEngine engine, string path, bool started, int scenarioId)
    {
        engine.CloseCampaign();
        engine.NewCampaign(WorldMapCampaign.ClassId);
        WorldMapCampaign campaign = Loaded(engine);
        campaign.ScenarioStarted = started;
        campaign.CurrentScenarioId = scenarioId;
        campaign.PlayMode = started ? PlayMode.Turn : PlayMode.Simultaneous;
        if (!started)
        {
            _ = campaign.CompletedScenarios.Add(scenarioId);
        }

        engine.SaveCampaign(path);
        engine.CloseCampaign();
    }

    private static WorldMapCampaign Loaded(GameEngine engine)
        => engine.Campaign ?? throw new InvalidOperationException("The campaign document did not load.");

    private sealed class SeatMap : IDisposable
    {
        private readonly IDisposable _restore;

        internal StrategicMap Map { get; }

        public SeatMap()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            _restore = EngineFixture.UseMap(Map);
            LocalPlayerCommunicationControl communication = new(EngineFixture.Engine);
            communication.Setup(Map.Players.Count);
            for (int i = 0; i < Map.Players.Count; i++)
            {
                Player? seated = Map.Players.GetPlayers(i);
                communication.SetConnectionMode(
                    mode: seated?.PlayerType == PlayerType.Human ? ConnectionMode.Local : ConnectionMode.Global,
                    index: PlayerId.FromInt32(i));
            }

            _ = Map.PlayerCommunicator.ActivatePlayerCommunicationControl(communication);
            communication.Release();
        }

        internal Player Player(int index) => Map.Players.GetPlayers(index) ?? throw new InvalidOperationException("The seat is missing.");

        internal void PlayingHumans()
        {
            Player(1).Status = PlayerStatus.Playing;
            Player(2).Status = PlayerStatus.Playing;
            Map.Players.Update();
        }

        internal void InstallEmptyContainer() => Map.MapContainer = new MapContainer(null) { Holder = EngineFixture.Engine };

        internal void InstallGrid(int width, int height)
        {
            MapContainer container = new(null) { Holder = EngineFixture.Engine, Map = Map };
            MapLevel level = new(container);
            level.CreateMapFieldList(width, height);
            container.Width = width;
            container.Height = height;
            container.LevelCount = 1;
            Map.MapContainer = container;
            // The level learns its map from the same activation message a real load sends.
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 0x2000_0008, sender: Map);
            message.Payload = EngineFixture.Engine;
            level.MessageProc(ref message, sender: Map);
        }

        internal TokenManager Tokens() => Map.TokenManager ?? throw new InvalidOperationException("The token manager is missing.");

        internal MapField Field(int x, int y)
        {
            MapContainer container = Map.MapContainer ?? throw new InvalidOperationException("The map has no container.");
            return container.GetField(x, y, 0);
        }

        public void Dispose()
        {
            _restore.Dispose();
            Map.Dispose();
        }
    }

    /// <summary>Turns autosave off and clears the sound system for one turn or day.</summary>
    private sealed class QuietTurn : IDisposable
    {
        private readonly bool _previousAutoSave;
        private readonly SoundSystem? _previousSound;

        public QuietTurn()
        {
            _previousAutoSave = EngineFixture.Engine.Registry.GetAutoSave();
            _previousSound = AudioHost.SoundSystem;
            AudioHost.SoundSystem = null;
            EngineFixture.Engine.Registry.SetAutoSave(false);
        }

        public void Dispose()
        {
            AudioHost.SoundSystem = _previousSound;
            EngineFixture.Engine.Registry.SetAutoSave(_previousAutoSave);
        }
    }

    private sealed class RecordingMoveControl(GameMap owner) : MoveControl(owner)
    {
        internal MoveModes Seen { get; private set; }

        public override void ProcessMove(MoveControlSettings settings)
        {
            Seen = settings.Mode;
            settings.SetupMovePointData(4, 4, levelCount: 1);
        }
    }
}
