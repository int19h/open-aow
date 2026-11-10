using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Units.Tests;

// A city attack stores the combat wall in the tactical object list. The tactical AI passes that wall
// into the terrain path, and the hero item search reads every object as a tactical unit. The original
// cast does not check. The path compares the reference, and the search skips a non-matching object.
[Collection("Engine")]
[EngineState]
public sealed class TacticalCombatWallCastTests
{
    private sealed class PlacedUnitHS(ECustomNode owner) : TacticalCombatUnitHS(owner)
    {
        public override sbyte GetXhx() => 4;

        public override sbyte GetYhx() => 9;
    }

    [Fact]
    public void TerrainPathAcceptsTheCombatWallAsIgnore()
    {
        var ai = new CAI();
        var settings = new CombatMoveControlSettings(null!);
        XYLList path = new Movepath();
        int value = 7;
        var combat = new TacticalCombat(null!);
        var objects = combat.FCombatData.FCombatObjects;
        var unit = new TacticalCombatUnit(objects) { FCombat = combat };
        var wall = new CombatWall(objects) { FCombat = combat };
        wall.Setup(1, 1, 0);
        Assert.Equal(-1, ai.MakeTerrainPath(settings, ref path, ref value, unit, wall, true, true, 1));
        Assert.Equal(0, value);
        value = 7;
        Assert.Equal(-1, ai.MakeTerrainPath(settings, ref path, ref value, unit, unit, true, false, 1));
        Assert.Equal(0, value);
        value = 7;
        Assert.Equal(-1, ai.MakeTerrainPath(settings, ref path, ref value, unit, null, false, false, 1));
        Assert.Equal(0, value);
    }

    [Fact]
    public void UnitLocationSkipsTheCombatWallAndFindsTheUnit()
    {
        var previous = Aow.Game.Combat.AoWCombatMap.Current;
        var map = new AoWCombatMap(null!);
        try
        {
            var combat = new TacticalCombat(null!);
            map.SetCombat(combat);
            var objects = combat.FCombatData.FCombatObjects;
            var hero = new AbstractUnit(null!);
            var wall = new CombatWall(objects) { FCombat = combat };
            wall.Setup(1, 1, 0);
            var unit = new TacticalCombatUnit(objects) { FCombat = combat, FID = 2, FPosition = 1, FUnit = hero };
            unit.FUnitHS = new PlacedUnitHS(null!) { FUnit = unit };
            sbyte x = 0;
            sbyte y = 0;
            sbyte l = 0;
            map.GetUnitLocation(hero, ref x, ref y, ref l);
            Assert.Equal((sbyte)4, x);
            Assert.Equal((sbyte)9, y);
            Assert.Equal((sbyte)0, l);
            var other = new AbstractUnit(null!);
            map.GetUnitLocation(other, ref x, ref y, ref l);
            Assert.Equal((sbyte)-1, x);
        }
        finally
        {
            map.SetOwner(null!);
            map.Dispose();
            Aow.Game.Combat.AoWCombatMap.Current = previous;
        }
    }
}
