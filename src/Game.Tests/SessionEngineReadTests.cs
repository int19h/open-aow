using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

// Issue 335 G5. The session guards that tested the global map test the map of the engine the
// object reaches, which is the map their bodies read. An object that reaches no engine takes the
// branch the original took for a missing map, even while another map is current. A diplomatic
// action reaches the engine of the player that created it, of the copy that made it and of the
// load that read it, so the relation helpers always receive an engine.
[Collection("Engine")]
[EngineState]
public sealed class SessionEngineReadTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly IDisposable _previousScope = EngineFixture.UseMap(StrategicMap.Current);

        public MapScope()
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            EngineFixture.SetMap(Map);
        }

        public StrategicMap Map { get; }

        public void Dispose() => _previousScope.Dispose();
    }

    // Spells.PAS:1927 reads the map only to report a power source left on a destroying map. A
    // control that reaches no map has none to report and still empties its source list.
    [Fact]
    public void ADetachedMagicControlDisposesItsPowerSourcesWhileAMapIsCurrent()
    {
        using MapScope scope = new();
        PlayerMagicControl control = new(null);
        _ = control.PowerSources.AddChild(new PowerSource(null!));
        Assert.Null(control.GetEngine());

        control.Dispose();

        Assert.Equal(0, control.PowerSources.GetCount());
    }

    [Fact]
    public void ADetachedPlayerKeepsItsLeaderUnvalidatedWhileAMapIsCurrent()
    {
        using MapScope scope = new();
        Player player = new(null) { PlayerIndex = PlayerId.Player1 };
        Assert.Null(player.GetEngine());

        player.ValidateLeader();

        Assert.Null(player.Leader);
    }

    [Fact]
    public void DetachedTransferSettingsRetrieveNothingWhileAMapIsCurrent()
    {
        using MapScope scope = new();
        CampaignTransferSettings settings = new(null);
        Assert.Null(settings.GetEngine());

        settings.Retrieve();

        Assert.Null(settings.Leader);
        Assert.Equal(0, settings.Heroes.GetCount());
    }

    // Player.PAS:1188 holds a new action through CreationHolder, a load takes the engine of its
    // factory, and a copy loads through the engine it is given.
    [Fact]
    public void ActionsOfEveryRouteReachTheEngineAndAnswerTheirRelationReads()
    {
        using MapScope scope = new();
        StrategicMap map = scope.Map;
        Player source = map.Players.GetPlayers(PlayerId.Player1)!;
        Player target = map.Players.GetPlayers(PlayerId.Player2)!;
        target.PlayerRace = source.PlayerRace;
        map.Players[1].DiplomaticRelations.SetDiplomaticRelation(PlayerId.Player2, value: DiplomaticRelation.Alliance);
        map.Players[2].DiplomaticRelations.SetDiplomaticRelation(PlayerId.Player1, value: DiplomaticRelation.Alliance);
        DiplomaticAction created = new(null)
        {
            Holder = source.CreationHolder(),
            Source = PlayerId.Player1,
            Target = PlayerId.Player2,
            Proposal = new BreakAlliance(),
        };

        using MemoryStream memory = new();
        EngineFixture.Engine.StoreEngineObject(memory, objectValue: created, progress: null);
        memory.Position = 0;
        DiplomaticAction loaded = Assert.IsType<DiplomaticAction>(EngineFixture.Engine.RetrieveEngineObject(memory, progress: null));
        DiplomaticAction copied = created.Copy<DiplomaticAction>(created.GetEngine());

        foreach (DiplomaticAction action in new[] { created, loaded, copied })
        {
            Assert.Same(EngineFixture.Engine, action.GetEngine());
            Assert.Equal(-40, new BreakAlliance().RelationModifier(action, player: PlayerId.Player1, race: source.PlayerRace));
            Assert.True(new OfferPeace().LapsesUnanswered(action));
            Assert.Equal(DiplomaticActionReaction.Reject, new OfferPeace().RejectReaction(action));
        }
    }
}
