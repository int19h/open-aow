using Aow.Engine;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;
using Aow.Game.World.Map;
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

        public void Dispose()
        {
            _restore.Dispose();
            Map.Dispose();
        }
    }
}
