using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;

namespace Aow.Units.Tests;

// A hero list owns a hero, a combat structure shares the door id, prisoners are an army, and a
// move sprite has the other kind. The original cast kept those values. Each case used to throw.
// The path treats that value as absent, and a value the cast already accepted still runs.
[Collection("Engine")]
[EngineState]
public sealed class ThrowingCastTests
{
    private sealed class MapScope : IDisposable
    {
        private readonly AoWHSMap previous = AoWHSMap.Current;

        public MapScope()
        {
            Map = new AoWHSMap(null!);
            AoWHSMap.Current = Map;
        }

        public AoWHSMap Map { get; }

        public void Dispose() => AoWHSMap.Current = previous;
    }

    private sealed class ItemSite : Structure
    {
        public ItemSite() : base(null!)
        {
        }

        public override int GetShowPriority() => 0;

        public override sbyte GetTerrain(int index) => 0;

        // The tag query reads the resource. This site has none and leaves the query unanswered.
        public override void MapFieldMsgProc(ref MapFieldMsg Msg)
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
        var player = new Player(null!);
        var leader = new Leader(null!);
        leader.FOwner = player.FHeroes;
        player.FLeader = leader;
        sbyte x = 0;
        sbyte y = 0;
        sbyte level = 0;
        player.GetLeaderLocation(ref x, ref y, ref level);
        Assert.Equal((sbyte)-1, x);
        Assert.Equal((sbyte)-1, y);
        Assert.Equal((sbyte)-1, level);

        var army = new Army(null!);
        army.FStatus = Set<ArmyStatusElement>.Of(ArmyStatusElement.Activated);
        army.FX = 4;
        army.FY = 6;
        army.FL = 1;
        leader.FOwner = army;
        player.GetLeaderLocation(ref x, ref y, ref level);
        Assert.Equal((sbyte)4, x);
        Assert.Equal((sbyte)6, y);
        Assert.Equal((sbyte)1, level);
    }

    [Fact]
    public void HealingLeavesAHeroListOutOfTheSelector()
    {
        using var scope = new MapScope();
        var heroList = new HeroList(null!);
        var unit = new AbstractUnit(null!) { FUnitID = 9 };
        unit.FOwner = heroList;
        int source = -1;
        int count = -1;
        scope.Map.FOnActivateAbility += sender =>
        {
            var info = (UnitSelectorAAI)sender;
            source = info.FUnitSelector.FSourceUnitID;
            count = info.FUnitSelector.FUnits.GetCount();
        };
        var ability = new HealingAbility(null!);
        string reason = "";
        Assert.True(ability.Activate(unit, ref reason));
        Assert.Equal(9, source);
        Assert.Equal(0, count);

        var list = new UnitUserList(null!);
        var listed = new AbstractUnit(null!) { FUnitID = 8 };
        listed.FOwner = list;
        source = -1;
        Assert.True(ability.Activate(listed, ref reason));
        Assert.Equal(8, source);
    }

    [Fact]
    public void NonHeroFindIsAbsentAndAHeroStillResolves()
    {
        using var scope = new MapScope();
        scope.Map.FSeatedPlayer = 0;
        var unit = new AbstractUnit(null!) { FUnitID = 42, FPlayer = 0 };
        var hero = new Hero(null!) { FUnitID = 43, FPlayer = 4 };
        scope.Map.FUnitControl.RegisterUnit(unit);
        scope.Map.FUnitControl.RegisterUnit(hero);

        var skipped = new HeroUpgradeEventLog(null!) { FUnitID = 42 };
        Assert.False(skipped.Execute(false, ExecuteTrigger.User, null!));

        var resolved = new HeroUpgradeEventLog(null!) { FUnitID = 43 };
        Assert.False(resolved.Execute(false, ExecuteTrigger.User, null!));

        var spell = new SpellTE(null!) { FEventSourceID = 42 };
        var executer = new TokenExecuter(null!);
        Assert.Equal(ExecuteResult.DoneHalt, spell.Execute(executer));

        executer.FExecuteCount = 1;
        ExecuteResult? later = null;
        var error = Record.Exception(() => later = spell.Execute(executer));
        Assert.IsNotType<InvalidCastException>(error);
        if (error == null)
        {
            Assert.NotEqual(ExecuteResult.DoneHalt, later);
        }
    }

    [Fact]
    public void SharedWallIdSkipsAStructureAndKeepsADoor()
    {
        var previous = AoWCombatMap.Current;
        var map = new AoWCombatMap(null!);
        try
        {
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(1, 1);
            container.FMap = map;
            map.FMapContainer = container;
            var combat = new TacticalBattle(null!);
            map.SetCombat(combat);
            combat.FCombatData.FWallType = 1;
            var field = container.MapLevels[0].Field[0, 0];
            var structure = new CombatStructure(null!);
            structure.FOwner = field;
            field.FHSList[0] = structure;
            field.FCount = 1;
            var ai = new CAI();
            int value = 7;
            ai.EvalHex(null!, 0, 0, 0, 0, ref value, true, false, null!, null);
            Assert.Equal(7, value);

            var door = new CityWallDoor(null!);
            door.FOwner = field;
            field.FHSList[0] = door;
            value = 7;
            ai.EvalHex(null!, 0, 0, 0, 0, ref value, true, false, null!, null);
            Assert.Equal(7, value);
        }
        finally
        {
            AoWCombatMap.Current = previous;
        }
    }

    [Fact]
    public void PrisonersStayOffTheItemList()
    {
        var previous = AoWCombatMap.Current;
        using var scope = new MapScope();
        try
        {
            var container = new MapContainer(null!);
            var level = new AoWMapLevel(container);
            level.CreateMapFieldList(7, 7);
            container.FMap = scope.Map;
            scope.Map.FMapContainer = container;
            var field = container.MapLevels[0].Field[3, 3];
            field.AddHS(new ItemSite(), 0);

            var combat = new TacticalBattle(null!) { FExplorationSite = 1 };
            combat.FItems = new PrisonersArmy(null!);
            var prisoners = new RndTacHsm();
            var error = Record.Exception(() => prisoners.RandomMap(combat, 3, 3, 0));
            Assert.Null(prisoners.FItems);
            // The generator keeps going after the item field and can fail on the map file.
            // The prisoners cast is the one whose message names ItemList.
            if (error is InvalidCastException cast)
            {
                Assert.DoesNotContain("ItemList", cast.Message);
            }

            var items = new ItemList(null!);
            combat.FItems = items;
            var laidOut = new RndTacHsm();
            Record.Exception(() => laidOut.RandomMap(combat, 3, 3, 0));
            Assert.Same(items, laidOut.FItems);
        }
        finally
        {
            AoWCombatMap.Current = previous;
        }
    }

    [Fact]
    public void MovementSpriteOfTheOtherKindIsAbsent()
    {
        using var scope = new MapScope();
        var container = new MapContainer(null!);
        var level = new MapLevel(container);
        level.CreateMapFieldList(1, 1);
        container.FMap = scope.Map;
        container.FWidth = 1;
        container.FHeight = 1;
        container.FLevelCount = 1;
        scope.Map.FMapContainer = container;

        var tactical = new MoveControlSettings(null!);
        tactical.FArmyHS = new TacticalCombatUnitHS(null!);
        tactical.SetSourcePos(0, 0, 0);
        tactical.SetDestinationPos(1, 0, 0);
        var error = Record.Exception(() => scope.Map.FMoveControl.Activate(tactical));
        Assert.Null(scope.Map.FMoveControl.FArmyHS);
        Assert.Null(error);

        scope.Map.FMoveControl.Deactivate();
        var army = new ArmyHS(null!);
        var world = new MoveControlSettings(null!);
        world.FArmyHS = army;
        world.SetSourcePos(0, 0, 0);
        world.SetDestinationPos(1, 0, 0);
        Assert.True(scope.Map.FMoveControl.Activate(world));
        Assert.Same(army, scope.Map.FMoveControl.FArmyHS);

        var previous = AoWCombatMap.Current;
        var combatMap = new AoWCombatMap(null!);
        try
        {
            var settings = new CombatMoveControlSettings(null!);
            settings.FArmyHS = new ArmyHS(null!);
            settings.SetSourcePos(-1, -1, -1);
            settings.SetDestinationPos(-1, -1, -1);
            var control = (CombatMoveControl)combatMap.FMoveControl;
            control.ProcessMove(settings);
            Assert.Null(control.FUnitHS);

            var unit = new TacticalCombatUnitHS(null!);
            settings.FArmyHS = unit;
            control.ProcessMove(settings);
            Assert.Same(unit, control.FUnitHS);
        }
        finally
        {
            AoWCombatMap.Current = previous;
        }
    }
}
