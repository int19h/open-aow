using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;

namespace Aow.Game.Tests;

/// <summary>
/// A test mode asks for a tactical battle only when the probe finds a defender.
/// An empty walled city stays the fast capture the game already chooses (MoveT.PAS:2710).
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class ForcedTacticalCityTests
{
    private sealed class Breacher(CustomNode? owner) : Troop(owner)
    {
        public override int Moves => 40;

        public override int GetMovePoints() => 40;

        public override int GetHitPoints() => 16;

        public override int GetHits() => 16;

        // Ability 0 is unset, so the move-type scan never reads an empty ability slot. Walking keeps the unit on grass.
        public override MoveTypes GetMoveTypes() => MoveTypes.Walking;

        // The sight check reads ability 64. That slot is empty on this unit, and the range falls back to 4.
        public override int GetAbilityLevel(int abilityId) => 0;

        // The party army asks for morale while it adopts the unit. This unit has no race record.
        public override int GetUnitMoraleValue(PlayerId? player, int independentRelation) => 0;
    }

    private sealed class Board : IDisposable
    {
        private readonly IDisposable _restore;
        private readonly SoundSystem? _sound;
        private readonly bool _tactical;
        private readonly int _tacticalBattles;
        private readonly int _tacticalSelections;
        private readonly int _tacticalExecutions;
        private readonly bool _fixtureTactical;
        private readonly bool _addedTacticalClass;

        internal StrategicMap Map { get; }

        internal City City { get; }

        internal MapField CityField { get; }

        internal ArmyHexSprite Attacker { get; }

        private Board(bool defender, bool tactical, bool fixtureTactical, int selections)
        {
            Map = new StrategicMap(null, holder: EngineFixture.Engine);
            _restore = EngineFixture.UseMap(Map);
            _sound = AudioHost.SoundSystem;
            AudioHost.SoundSystem = null;
            _tactical = HeadlessHooks.Tactical;
            _tacticalBattles = HeadlessHooks.TacticalBattles;
            _tacticalSelections = HeadlessHooks.TacticalSelections;
            _tacticalExecutions = HeadlessHooks.TacticalExecutions;
            _fixtureTactical = FrameFixtureInputs.Tactical;
            HeadlessHooks.Tactical = tactical;
            HeadlessHooks.TacticalBattles = 1;
            HeadlessHooks.TacticalSelections = selections;
            HeadlessHooks.TacticalExecutions = 0;
            FrameFixtureInputs.Tactical = fixtureTactical;
            EngineClass tacticalClass = EngineClass.Of<TacticalBattle>();
            _addedTacticalClass = !EngineFixture.Engine.ClassTable.ContainsKey(tacticalClass.ClassId);
            if (_addedTacticalClass)
            {
                EngineFixture.Engine.RegisterEngineClasses([tacticalClass]);
            }

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
            Map.Players.GetPlayers(PlayerId.Player1)!.Status = PlayerStatus.Playing;
            Map.Players.GetPlayers(PlayerId.Player2)!.Status = PlayerStatus.Playing;
            Map.Players.GetPlayers(PlayerId.Player1)!.DiplomaticRelations.SetDiplomaticRelation(PlayerId.Player2, value: DiplomaticRelation.War);
            Map.Players.GetPlayers(PlayerId.Player2)!.DiplomaticRelations.SetDiplomaticRelation(PlayerId.Player1, value: DiplomaticRelation.War);

            MapContainer container = new(null) { Holder = EngineFixture.Engine, Map = Map };
            MapLevel level = new(container);
            level.CreateMapFieldList(3, 3);
            container.Width = 3;
            container.Height = 3;
            container.LevelCount = 1;
            Map.MapContainer = container;
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 0x2000_0008, sender: Map);
            message.Payload = EngineFixture.Engine;
            level.MessageProc(ref message, sender: Map);
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    container.GetField(x, y, level: 0).SetTerrain(TerrainType.Grass);
                }
            }

            Resource resource = new(null!) { Holder = EngineFixture.Engine };
            resource.TerrainList.SetTerrain(0, value: 1);
            EngineMessage.InitializeMessage(out EngineMessage resourceMessage, messageId: 536_870_920, sender: resource);
            resourceMessage.Payload = EngineFixture.Engine;
            resource.MessageProc(ref resourceMessage, sender: resource);
            City = new City(null!, holder: EngineFixture.Engine);
            City.SetResource(resource);
            City.WallType = (int)WallType.Wood;
            MemberNames.Field(typeof(MultiHexMapObject), name: "StoredHexX")!.SetValue(City, value: 1);
            MemberNames.Field(typeof(MultiHexMapObject), name: "StoredHexY")!.SetValue(City, value: 1);
            CityField = container.GetField(1, 1, level: 0);
            City.SetOwnerWithoutNotify(CityField);
            _ = CityField.AddHexSprite(City, hexNumber: 0);
            CityField.UpdateMapField();
            City.SetPlayer(PlayerId.Player2);

            // A played turn is synchronized. The combat log refuses an event on a map that is not.
            Map.SynchronizeBegin();
            Attacker = Place(PlayerId.Player1, x: 2, y: 1, breacher: true);
            if (defender)
            {
                _ = Place(player: PlayerId.Player2, x: 1, y: 1, breacher: false);
            }
        }

        internal static Board Open(bool defender, bool tactical, bool fixtureTactical, int selections = 0) => new(defender, tactical, fixtureTactical, selections);

        internal ArmyCombatMoveTokenEvent Attack()
        {
            MovePath path = [];
            _ = path.AddMapCoordinate(1, 1, mapLevel: 0);
            _ = path.AddMapCoordinate(2, 1, mapLevel: 0);
            ArmyCombatMoveTokenEvent move = new(null) { Holder = EngineFixture.Engine };
            Assert.True(move.Setup(Attacker, selection: 1, path: path, mode: ArmyMoveModes.None));
            MemberNames.Field(typeof(ArmyCombatMoveTokenEvent), name: "_combatType")!.SetValue(move, value: CombatType.Fast);
            return move;
        }

        public void Dispose()
        {
            try
            {
                // A battle left open has no tactical-battle list entry. Clearing it keeps map disposal off ActivateTacticalBattle.
                Battle? combat = Map.Combat;
                MemberNames.Field(typeof(StrategicMap), name: "Combat")!.SetValue(Map, value: null);
                // An open tactical battle stays executing. Its finalize walks a move off this three-hex map.
                if (combat is not null && !combat.Status.Contains(CombatStatuses.Executing))
                {
                    combat.Dispose();
                }

                Map.Dispose();
            }
            finally
            {
                HeadlessHooks.Tactical = _tactical;
                HeadlessHooks.TacticalBattles = _tacticalBattles;
                HeadlessHooks.TacticalSelections = _tacticalSelections;
                HeadlessHooks.TacticalExecutions = _tacticalExecutions;
                FrameFixtureInputs.Tactical = _fixtureTactical;
                AudioHost.SoundSystem = _sound;
                if (_addedTacticalClass)
                {
                    EngineFixture.Engine.UnregisterEngineClasses([EngineClass.Of<TacticalBattle>()]);
                }

                _restore.Dispose();
            }
        }

        private static ArmyHexSprite Place(PlayerId player, int x, int y, bool breacher)
        {
            ArmyHexSprite sprite = new(null) { Holder = EngineFixture.Engine };
            Breacher troop = new(null) { Holder = EngineFixture.Engine };
            if (breacher)
            {
                troop.SetAbilitySet(24, value: 1);
            }

            Army army = sprite.Army ?? throw new InvalidOperationException("The army is missing.");
            army.SetPlayer(player);
            Assert.True(army.AddUnit(troop));
            MapContainer container = StrategicMap.Current?.MapContainer ?? throw new InvalidOperationException("The map has no container.");
            MapField field = container.GetField(x, y, level: 0);
            sprite.SetOwnerWithoutNotify(field);
            _ = field.AddHexSprite(sprite, hexNumber: 0);
            return sprite;
        }
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void AnEmptyWalledCityIsCapturedWithoutATacticalBattle(bool tactical, bool fixtureTactical)
    {
        using Board board = Board.Open(defender: false, tactical: tactical, fixtureTactical: fixtureTactical);
        Assert.Equal(PlayerId.Player2, board.CityField.GetPlayer());
        ArmyCombatMoveTokenEvent move = board.Attack();

        move.ExecuteCombat();

        Assert.Equal(PlayerId.Player1, board.City.Player);
        Assert.Equal(PlayerId.Player1, board.CityField.GetPlayer());
        Assert.Equal(0, HeadlessHooks.TacticalSelections);
        Assert.Equal(0, HeadlessHooks.TacticalExecutions);
        Assert.Equal(CombatType.Fast, (CombatType)MemberNames.Field(typeof(ArmyCombatMoveTokenEvent), name: "_combatType")!.GetValue(move)!);
    }

    [Theory]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public void AWalledCityWithADefenderStillRunsTheForcedTacticalBattle(bool tactical, bool fixtureTactical)
    {
        using Board board = Board.Open(defender: true, tactical: tactical, fixtureTactical: fixtureTactical);
        Assert.Equal(PlayerId.Player2, board.CityField.GetPlayer());
        ArmyCombatMoveTokenEvent move = board.Attack();

        // The generator's combat-map load finds no tactical map in this fixture and leaves CombatMap null.
        Exception error = Assert.Throws<NullReferenceException>(move.ExecuteCombat);

        Assert.Contains("RandomBattleMapGenerator", error.ToString());
        Assert.Equal(1, HeadlessHooks.TacticalSelections);
        Assert.Equal(1, HeadlessHooks.TacticalExecutions);
        Assert.Equal(PlayerId.Player2, board.City.Player);
        TokenManager tokens = board.Map.TokenManager ?? throw new InvalidOperationException("The token manager is missing.");
        Assert.True(tokens.TokenExecutor.HoldTacticalBattle);
    }

    [Fact]
    public void ASpentBattleCapLeavesTheDefendedCityOnTheFastPath()
    {
        using Board board = Board.Open(defender: true, tactical: true, fixtureTactical: false, selections: 1);
        ArmyCombatMoveTokenEvent move = board.Attack();

        // The cap leaves the fast battle in place. This board's fast round stops in ExecuteCombatRound.
        Exception error = Assert.Throws<ArgumentOutOfRangeException>(move.ExecuteCombat);

        Assert.Contains("FastBattle.ExecuteCombatRound", error.ToString());
        Assert.Equal(1, HeadlessHooks.TacticalSelections);
        Assert.Equal(0, HeadlessHooks.TacticalExecutions);
        Assert.Equal(CombatType.Fast, (CombatType)MemberNames.Field(typeof(ArmyCombatMoveTokenEvent), name: "_combatType")!.GetValue(move)!);
    }
}
