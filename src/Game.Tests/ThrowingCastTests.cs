#nullable disable warnings
using Aow.Game.AI.Groups;
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;

namespace Aow.Game.Tests;

// A hero list owns a hero, a combat structure shares the door id, prisoners are an army, and a
// move sprite has the other kind. The original cast kept those values. Each case used to throw.
// The path treats that value as absent, and a value the cast already accepted still runs.
[Collection("Engine")]
[EngineState]
public sealed class ThrowingCastTests
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

    // A hero that always wishes to join the seated player.
    private sealed class JoiningHero : Hero
    {
        public JoiningHero() : base(null)
        {
        }

        public override bool CanJoin(PlayerId? player) => true;

        public override bool OfferToJoin(PlayerId? player) => true;
    }

    private sealed class ItemSite : Structure
    {
        public ItemSite() : base(null)
        {
        }

        public override int GetShowPriority() => 0;

        public override FootprintCell<TerrainType> GetTerrain(int index) => World.Map.TerrainType.Water;

        // The tag query reads the resource. This site has none and leaves the query unanswered.
        public override void MapFieldMessageProc(ref MapFieldMessage message)
        {
        }

        public override string CombatMapName(CombatMapSite site)
        {
            site.TakesCombatItems = true;
            return "Blank000";
        }
    }

    [Fact]
    public void HeroListOwnerIsAbsentAndAnArmyStillReportsItsPlace()
    {
        Player player = new(null) { Holder = EngineFixture.Engine };
        Leader leader = new(null) { Holder = EngineFixture.Engine };
        leader.SetOwnerWithoutNotify(player.Heroes);
        player.Leader = leader;
        player.GetLeaderLocation(out int x, out int y, L: out int level);
        Assert.Equal((sbyte)-1, (sbyte)x);
        Assert.Equal((sbyte)-1, (sbyte)y);
        Assert.Equal((sbyte)-1, (sbyte)level);

        Army army = new(null)
        {
            Holder = EngineFixture.Engine,
            Status = ArmyStatuses.Activated,
            Location = new MapCoordinate(4, 6, 1)
        };
        leader.SetOwnerWithoutNotify(army);
        player.GetLeaderLocation(out x, out y, L: out level);
        Assert.Equal((sbyte)4, (sbyte)x);
        Assert.Equal((sbyte)6, (sbyte)y);
        Assert.Equal((sbyte)1, (sbyte)level);
    }

    [Fact]
    public void HealingLeavesAHeroListOutOfTheSelector()
    {
        using MapScope scope = new();
        HeroList heroList = new(null) { Holder = EngineFixture.Engine };
        Troop troop = new(null) { Holder = EngineFixture.Engine, UnitId = 9 };
        troop.SetOwnerWithoutNotify(heroList);
        int source = -1;
        int count = -1;
        scope.Map.AbilityActivated += sender =>
        {
            UnitSelectionActivation info = (UnitSelectionActivation)sender;
            source = info.UnitSelector.SourceUnitId;
            count = info.UnitSelector.Units.GetCount();
        };
        HealingAbility ability = new(null, holder: EngineFixture.Engine);
        string reason = "";
        Assert.True(ability.Activate(troop, ref reason));
        Assert.Equal(9, source);
        Assert.Equal(0, count);

        UnitUserList list = new(null) { Holder = EngineFixture.Engine };
        Troop listed = new(null) { Holder = EngineFixture.Engine, UnitId = 8 };
        listed.SetOwnerWithoutNotify(list);
        source = -1;
        Assert.True(ability.Activate(listed, ref reason));
        Assert.Equal(8, source);
    }

    [Fact]
    public void NonHeroFindIsAbsentAndAHeroStillResolves()
    {
        using MapScope scope = new();
        scope.Map.SeatedPlayer = PlayerId.Independent;
        Troop troop = new(null) { Holder = EngineFixture.Engine, UnitId = 42, Player = PlayerId.Independent };
        Hero hero = new(null) { Holder = EngineFixture.Engine, UnitId = 43, Player = PlayerId.Player4 };
        scope.Map.UnitControl.RegisterUnit(troop);
        scope.Map.UnitControl.RegisterUnit(hero);

        HeroUpgradeEventLog skipped = new(null, holder: EngineFixture.Engine) { UnitId = 42 };
        Assert.False(skipped.Execute(false, trigger: ExecuteTrigger.User, done: null));

        HeroUpgradeEventLog resolved = new(null, holder: EngineFixture.Engine) { UnitId = 43 };
        Assert.False(resolved.Execute(false, trigger: ExecuteTrigger.User, done: null));

        SpellTokenEvent spell = new(null) { Holder = EngineFixture.Engine, EventSourceId = 42 };
        TokenExecutor executor = new(null);
        Assert.Equal(ExecuteResult.DoneHalt, spell.Execute(executor));

        executor.ExecuteCount = 1;
        ExecuteResult? later = null;
        Exception error = Record.Exception(() => later = spell.Execute(executor));
        Assert.IsNotType<InvalidCastException>(error);
        if (error is null)
        {
            Assert.NotEqual(ExecuteResult.DoneHalt, later);
        }
    }

    [Fact]
    public void SharedWallIdSkipsAStructureAndKeepsADoor()
    {
        IDisposable previousScope = EngineFixture.UseCombatMap(CombatMap.Current);
        CombatMap map = new(null, holder: EngineFixture.Engine);
        try
        {
            MapContainer container = new(null) { Holder = EngineFixture.Engine };
            MapLevel level = new(container);
            level.CreateMapFieldList(1, 1);
            container.Map = map;
            map.MapContainer = container;
            TacticalBattle combat = new(null) { Holder = EngineFixture.Engine };
            map.SetCombat(combat);
            combat.CombatData.WallType = 1;
            MapField field = container.MapLevels[0].Field[0, 0];
            CombatStructure structure = new(null) { Holder = EngineFixture.Engine };
            structure.SetOwnerWithoutNotify(field);
            field.HexSpriteList[0] = structure;
            field.StoredCount = 1;
            BattleAI ai = new(EngineFixture.Engine);
            int value = 7;
            ai.EvalHex(null, ability: 0, spell: 0, 0, 0, ref value, hitWalls: true, friendly: false, U: null, ignoreWall: null);
            Assert.Equal(7, value);

            CityWallDoor door = new(null) { Holder = EngineFixture.Engine };
            door.SetOwnerWithoutNotify(field);
            field.HexSpriteList[0] = door;
            value = 7;
            ai.EvalHex(null, ability: 0, spell: 0, 0, 0, ref value, hitWalls: true, friendly: false, U: null, ignoreWall: null);
            Assert.Equal(7, value);
        }
        finally
        {
            previousScope.Dispose();
        }
    }

    [Fact]
    public void PrisonersStayOffTheItemList()
    {
        IDisposable previousScope = EngineFixture.UseCombatMap(CombatMap.Current);
        using MapScope scope = new();
        try
        {
            MapContainer container = new(null) { Holder = EngineFixture.Engine };
            MapLevel level = new(container);
            level.CreateMapFieldList(7, 7);
            // The generator reads the terrain of the strategic fields, which a played map always has.
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    level.GetField(x, y).SetTerrain(TerrainType.Grass);
                }
            }
            container.Map = scope.Map;
            scope.Map.MapContainer = container;
            MapField field = container.MapLevels[0].Field[3, 3];
            _ = field.AddHexSprite(new ItemSite(), hexNumber: 0);

            TacticalBattle combat = new(null)
            {
                Holder = EngineFixture.Engine,
                ExplorationSite = 1,
                Items = new PrisonersArmy(null) { Holder = EngineFixture.Engine }
            };
            RandomBattleMapGenerator prisoners = new(EngineFixture.Engine);
            Exception error = Record.Exception(() => prisoners.RandomMap(combat, 3, 3, L: 0));
            Assert.Null(prisoners.Items);
            // The generator keeps going after the item field and can fail on the map file.
            // The prisoners cast is the one whose message names ItemList.
            if (error is InvalidCastException cast)
            {
                Assert.DoesNotContain("ItemList", cast.Message);
            }

            ItemList items = new(null) { Holder = EngineFixture.Engine };
            combat.Items = items;
            RandomBattleMapGenerator laidOut = new(EngineFixture.Engine);
            _ = Record.Exception(() => laidOut.RandomMap(combat, 3, 3, L: 0));
            Assert.Same(items, laidOut.Items);
        }
        finally
        {
            previousScope.Dispose();
        }
    }

    [Fact]
    public void MovementSpriteOfTheOtherKindIsAbsent()
    {
        using MapScope scope = new();
        MapContainer container = new(null) { Holder = EngineFixture.Engine };
        MapLevel level = new(container);
        level.CreateMapFieldList(1, 1);
        container.Map = scope.Map;
        container.Width = 1;
        container.Height = 1;
        container.LevelCount = 1;
        scope.Map.MapContainer = container;

        MoveControlSettings tactical = new(null)
        {
            Holder = EngineFixture.Engine,
            ArmyHexSprite = new TacticalBattleUnitHexSprite(null) { Holder = EngineFixture.Engine }
        };
        tactical.SetSourcePosition(0, 0, 0);
        tactical.SetDestinationPosition(1, 0, 0);
        Exception error = Record.Exception(() => scope.Map.MoveControl.Activate(tactical));
        Assert.Null(scope.Map.MoveControl.ArmyHexSprite);
        Assert.Null(error);

        scope.Map.MoveControl.Deactivate();
        ArmyHexSprite army = new(null) { Holder = EngineFixture.Engine };
        MoveControlSettings world = new(null)
        {
            Holder = EngineFixture.Engine,
            ArmyHexSprite = army
        };
        world.SetSourcePosition(0, 0, 0);
        world.SetDestinationPosition(1, 0, 0);
        Assert.True(scope.Map.MoveControl.Activate(world));
        Assert.Same(army, scope.Map.MoveControl.ArmyHexSprite);

        IDisposable previousScope = EngineFixture.UseCombatMap(CombatMap.Current);
        CombatMap combatMap = new(null, holder: EngineFixture.Engine);
        try
        {
            CombatMoveControlSettings settings = new(null)
            {
                Holder = EngineFixture.Engine,
                ArmyHexSprite = new ArmyHexSprite(null) { Holder = EngineFixture.Engine }
            };
            settings.SetSourcePosition(-1, -1, -1);
            settings.SetDestinationPosition(-1, -1, -1);
            CombatMoveControl control = combatMap.MoveControl;
            control.ProcessMove(settings);
            Assert.Null(control.UnitHexSprite);

            TacticalBattleUnitHexSprite unit = new(null) { Holder = EngineFixture.Engine };
            settings.ArmyHexSprite = unit;
            control.ProcessMove(settings);
            Assert.Same(unit, control.UnitHexSprite);
        }
        finally
        {
            previousScope.Dispose();
        }
    }

    [Fact]
    public void ArmyOwnerIsNullForEveryNonArmyOwner()
    {
        Army army = new(null) { Holder = EngineFixture.Engine };
        UnitList units = new(null) { Holder = EngineFixture.Engine };
        UnitUserList users = new(null) { Holder = EngineFixture.Engine };
        HeroList heroes = new(null) { Holder = EngineFixture.Engine };
        HeroLibrary library = new(null) { Holder = EngineFixture.Engine };
        Troop troop = new(null) { Holder = EngineFixture.Engine };
        troop.SetOwnerWithoutNotify(army);
        Assert.Same(army, troop.ArmyOwner);
        troop.SetOwnerWithoutNotify(units);
        Assert.Null(troop.ArmyOwner);
        troop.SetOwnerWithoutNotify(users);
        Assert.Null(troop.ArmyOwner);
        troop.SetOwnerWithoutNotify(null);
        Assert.Null(troop.ArmyOwner);

        // A hero admits the same owners. A unit list and a unit user list hold generated
        // troops and campaign transfers, a hero list and a library the heroes outside armies.
        Hero hero = new(null) { Holder = EngineFixture.Engine };
        hero.SetOwnerWithoutNotify(army);
        Assert.Same(army, hero.ArmyOwner);
        hero.SetOwnerWithoutNotify(units);
        Assert.Null(hero.ArmyOwner);
        hero.SetOwnerWithoutNotify(users);
        Assert.Null(hero.ArmyOwner);
        hero.SetOwnerWithoutNotify(heroes);
        Assert.Null(hero.ArmyOwner);
        hero.SetOwnerWithoutNotify(library);
        Assert.Null(hero.ArmyOwner);
    }

    [Fact]
    public void JoiningHeroOutsideAnArmyMakesNoJoinLog()
    {
        using MapScope scope = new();
        UnitList units = new(null) { Holder = EngineFixture.Engine };
        JoiningHero hero = new() { Holder = EngineFixture.Engine, UnitId = 42, Player = PlayerId.Independent };
        hero.SetOwnerWithoutNotify(units);
        scope.Map.UnitControl.RegisterUnit(hero);
        HeroEmergeEventLog emerge = new(null, holder: EngineFixture.Engine) { UnitId = 42, Player = PlayerId.Player1 };
        HeroJoinEventLog? log = null;
        Exception error = Record.Exception(() => log = emerge.CreateHeroJoinEventLog());
        Assert.Null(error);
        Assert.Null(log);

        HeroList heroes = new(null) { Holder = EngineFixture.Engine };
        hero.SetOwnerWithoutNotify(heroes);
        error = Record.Exception(() => log = emerge.CreateHeroJoinEventLog());
        Assert.Null(error);
        Assert.Null(log);
    }

    [Fact]
    public void WarpPartyWithoutArmyReturnsFalse()
    {
        using MapScope scope = new();
        scope.Map.TokenManager.CreatePlayer(PlayerId.Player1);
        UnitList units = new(null) { Holder = EngineFixture.Engine };
        Hero hero = new(null) { Holder = EngineFixture.Engine, UnitId = 7, Player = PlayerId.Player1 };
        hero.SetOwnerWithoutNotify(units);
        WarpParty spell = new(null, holder: EngineFixture.Engine);
        bool result = true;
        Exception error = Record.Exception(() => result = spell.Activate(hero));
        Assert.Null(error);
        Assert.False(result);
    }

    [Fact]
    public void TransporterWithoutArmyLeavesNoTransporterArmy()
    {
        using MapScope scope = new();
        UnitList units = new(null) { Holder = EngineFixture.Engine };
        Troop transporter = new(null) { Holder = EngineFixture.Engine, UnitId = 5 };
        transporter.SetOwnerWithoutNotify(units);
        AIMoveControlSettings settings = new(null)
        {
            Holder = EngineFixture.Engine,
            Transporter = transporter
        };
        AIMoveControl control = new(null) { Holder = EngineFixture.Engine };
        bool result = false;
        Exception error = Record.Exception(() => result = control.CanMove(settings));
        Assert.Null(error);
        Assert.True(result);
        control.Dispose();
    }

    [Fact]
    public void UnitSelectionWithoutAnArmySkipsConcealment()
    {
        using MapScope scope = new();
        UnitList units = new(null) { Holder = EngineFixture.Engine };
        Troop source = new(null) { Holder = EngineFixture.Engine, UnitId = 6, Player = PlayerId.Player1 };
        Troop target = new(null) { Holder = EngineFixture.Engine, UnitId = 9, Player = PlayerId.Player1 };
        _ = units.Units.AddUnit(source);
        _ = units.Units.AddUnit(target);
        UnitSpell spell = new(null, holder: EngineFixture.Engine);
        UnitSpellCaster caster = new(null) { Holder = EngineFixture.Engine };
        Exception error = Record.Exception(() => spell.SetupUnitSelectionControl(caster, source, targets: source.OwnerUnits));
        Assert.Null(error);
        Assert.Equal(2, caster.TargetUnits.GetCount());

        Army army = new(null)
        {
            Holder = EngineFixture.Engine,
            Player = PlayerId.Player1
        };
        _ = army.Units.AddUnit(source);
        _ = army.Units.AddUnit(target);
        UnitSpellCaster listed = new(null) { Holder = EngineFixture.Engine };
        error = Record.Exception(() => spell.SetupUnitSelectionControl(listed, source, targets: source.OwnerUnits));
        Assert.Null(error);
        Assert.Equal(2, listed.TargetUnits.GetCount());
    }
}
