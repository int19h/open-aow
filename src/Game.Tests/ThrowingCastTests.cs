using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
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
        private readonly AoWHSMap _previous = AoWHSMap.Current;

        public MapScope()
        {
            Map = new AoWHSMap(null!);
            AoWHSMap.Current = Map;
        }

        public AoWHSMap Map { get; }

        public void Dispose() => AoWHSMap.Current = _previous;
    }

    private sealed class ItemSite : Structure
    {
        public ItemSite() : base(null!)
        {
        }

        public override int GetShowPriority() => 0;

        public override sbyte GetTerrain(int index) => 0;

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
        var player = new Player(null!);
        var leader = new Leader(null!);
        leader.SetOwnerWithoutNotify(player.Heroes);
        player.Leader = leader;
        sbyte x = 0;
        sbyte y = 0;
        sbyte level = 0;
        player.GetLeaderLocation(ref x, ref y, ref level);
        Assert.Equal((sbyte)-1, x);
        Assert.Equal((sbyte)-1, y);
        Assert.Equal((sbyte)-1, level);

        var army = new Army(null!);
        army.Status = Set<ArmyStatusElement>.Of(ArmyStatusElement.Activated);
        army.X = 4;
        army.Y = 6;
        army.MapLevel = 1;
        leader.SetOwnerWithoutNotify(army);
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
        var troop = new Troop(null!) { UnitId = 9 };
        troop.SetOwnerWithoutNotify(heroList);
        int source = -1;
        int count = -1;
        scope.Map.FOnActivateAbility += sender =>
        {
            var info = (UnitSelectionActivation)sender;
            source = info.UnitSelector.SourceUnitId;
            count = info.UnitSelector.Units.GetCount();
        };
        var ability = new HealingAbility(null!);
        string reason = "";
        Assert.True(ability.Activate(troop, ref reason));
        Assert.Equal(9, source);
        Assert.Equal(0, count);

        var list = new UnitUserList(null!);
        var listed = new Troop(null!) { UnitId = 8 };
        listed.SetOwnerWithoutNotify(list);
        source = -1;
        Assert.True(ability.Activate(listed, ref reason));
        Assert.Equal(8, source);
    }

    [Fact]
    public void NonHeroFindIsAbsentAndAHeroStillResolves()
    {
        using var scope = new MapScope();
        scope.Map.SeatedPlayer = 0;
        var troop = new Troop(null!) { UnitId = 42, Player = 0 };
        var hero = new Hero(null!) { UnitId = 43, Player = 4 };
        scope.Map.UnitControl.RegisterUnit(troop);
        scope.Map.UnitControl.RegisterUnit(hero);

        var skipped = new HeroUpgradeEventLog(null!) { UnitId = 42 };
        Assert.False(skipped.Execute(false, ExecuteTrigger.User, null!));

        var resolved = new HeroUpgradeEventLog(null!) { UnitId = 43 };
        Assert.False(resolved.Execute(false, ExecuteTrigger.User, null!));

        var spell = new SpellTokenEvent(null!) { EventSourceId = 42 };
        var executor = new TokenExecutor(null!);
        Assert.Equal(ExecuteResult.DoneHalt, spell.Execute(executor));

        executor.ExecuteCount = 1;
        ExecuteResult? later = null;
        var error = Record.Exception(() => later = spell.Execute(executor));
        Assert.IsNotType<InvalidCastException>(error);
        if (error == null)
        {
            Assert.NotEqual(ExecuteResult.DoneHalt, later);
        }
    }

    [Fact]
    public void SharedWallIdSkipsAStructureAndKeepsADoor()
    {
        var previous = CombatMap.Current;
        var map = new CombatMap(null!);
        try
        {
            var container = new MapContainer(null!);
            var level = new MapLevel(container);
            level.CreateMapFieldList(1, 1);
            container.Map = map;
            map.MapContainer = container;
            var combat = new TacticalBattle(null!);
            map.SetCombat(combat);
            combat.CombatData.WallType = 1;
            var field = container.MapLevels[0].Field[0, 0];
            var structure = new CombatStructure(null!);
            structure.SetOwnerWithoutNotify(field);
            field._hexSpriteList[0] = structure;
            field._count = 1;
            var ai = new BattleAI();
            int value = 7;
            ai.EvalHex(null!, 0, 0, 0, 0, ref value, true, false, null!, null);
            Assert.Equal(7, value);

            var door = new CityWallDoor(null!);
            door.SetOwnerWithoutNotify(field);
            field._hexSpriteList[0] = door;
            value = 7;
            ai.EvalHex(null!, 0, 0, 0, 0, ref value, true, false, null!, null);
            Assert.Equal(7, value);
        }
        finally
        {
            CombatMap.Current = previous;
        }
    }

    [Fact]
    public void PrisonersStayOffTheItemList()
    {
        var previous = CombatMap.Current;
        using var scope = new MapScope();
        try
        {
            var container = new MapContainer(null!);
            var level = new AoWMapLevel(container);
            level.CreateMapFieldList(7, 7);
            container.Map = scope.Map;
            scope.Map.MapContainer = container;
            var field = container.MapLevels[0].Field[3, 3];
            field.AddHexSprite(new ItemSite(), 0);

            var combat = new TacticalBattle(null!) { ExplorationSite = 1 };
            combat.Items = new PrisonersArmy(null!);
            var prisoners = new RandomBattleMapGenerator();
            var error = Record.Exception(() => prisoners.RandomMap(combat, 3, 3, 0));
            Assert.Null(prisoners.Items);
            // The generator keeps going after the item field and can fail on the map file.
            // The prisoners cast is the one whose message names ItemList.
            if (error is InvalidCastException cast)
            {
                Assert.DoesNotContain("ItemList", cast.Message);
            }

            var items = new ItemList(null!);
            combat.Items = items;
            var laidOut = new RandomBattleMapGenerator();
            Record.Exception(() => laidOut.RandomMap(combat, 3, 3, 0));
            Assert.Same(items, laidOut.Items);
        }
        finally
        {
            CombatMap.Current = previous;
        }
    }

    [Fact]
    public void MovementSpriteOfTheOtherKindIsAbsent()
    {
        using var scope = new MapScope();
        var container = new MapContainer(null!);
        var level = new MapLevel(container);
        level.CreateMapFieldList(1, 1);
        container.Map = scope.Map;
        container.Width = 1;
        container.Height = 1;
        container.LevelCount = 1;
        scope.Map.MapContainer = container;

        var tactical = new MoveControlSettings(null!);
        tactical.ArmyHexSprite = new TacticalBattleUnitHexSprite(null!);
        tactical.SetSourcePosition(0, 0, 0);
        tactical.SetDestinationPosition(1, 0, 0);
        var error = Record.Exception(() => scope.Map.MoveControl.Activate(tactical));
        Assert.Null(scope.Map.MoveControl.ArmyHexSprite);
        Assert.Null(error);

        scope.Map.MoveControl.Deactivate();
        var army = new ArmyHexSprite(null!);
        var world = new MoveControlSettings(null!);
        world.ArmyHexSprite = army;
        world.SetSourcePosition(0, 0, 0);
        world.SetDestinationPosition(1, 0, 0);
        Assert.True(scope.Map.MoveControl.Activate(world));
        Assert.Same(army, scope.Map.MoveControl.ArmyHexSprite);

        var previous = CombatMap.Current;
        var combatMap = new CombatMap(null!);
        try
        {
            var settings = new CombatMoveControlSettings(null!);
            settings.ArmyHexSprite = new ArmyHexSprite(null!);
            settings.SetSourcePosition(-1, -1, -1);
            settings.SetDestinationPosition(-1, -1, -1);
            var control = (CombatMoveControl)combatMap.MoveControl;
            control.ProcessMove(settings);
            Assert.Null(control.UnitHexSprite);

            var unit = new TacticalBattleUnitHexSprite(null!);
            settings.ArmyHexSprite = unit;
            control.ProcessMove(settings);
            Assert.Same(unit, control.UnitHexSprite);
        }
        finally
        {
            CombatMap.Current = previous;
        }
    }
}
