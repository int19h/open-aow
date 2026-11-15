#nullable disable
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Movement;

namespace Aow.Game.Tests;

// A city attack stores the combat wall in the tactical object list. The tactical AI passes that wall
// into the terrain path, and the hero item search reads every object as a tactical unit. The original
// cast does not check. The path compares the reference, and the search skips a non-matching object.
[Collection("Engine")]
[EngineState]
public sealed class TacticalBattleWallCastTests
{
    private sealed class PlacedUnitHexSprite(CustomNode owner) : TacticalBattleUnitHexSprite(owner)
    {
        public override sbyte GetHexX() => 4;

        public override sbyte GetHexY() => 9;
    }

    [Fact]
    public void TerrainPathAcceptsTheCombatWallAsIgnore()
    {
        BattleAI ai = new(EngineFixture.Engine);
        CombatMoveControlSettings settings = new(null!) { Holder = EngineFixture.Engine };
        MovePath path = new();
        int value = 7;
        TacticalBattle combat = new(null!) { Holder = EngineFixture.Engine };
        OwningNode objects = combat.CombatData.CombatObjects;
        TacticalBattleUnit unit = new(objects) { Combat = combat };
        CombatWall wall = new(objects) { Combat = combat };
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
        CombatMap previous = CombatMap.Current;
        CombatMap map = new(null!) { Holder = EngineFixture.Engine };
        try
        {
            TacticalBattle combat = new(null!) { Holder = EngineFixture.Engine };
            map.SetCombat(combat);
            OwningNode objects = combat.CombatData.CombatObjects;
            Troop hero = new(null!) { Holder = EngineFixture.Engine };
            CombatWall wall = new(objects) { Combat = combat };
            wall.Setup(1, 1, 0);
            TacticalBattleUnit unit = new(objects) { Combat = combat, Id = 2, Position = 1, Troop = hero };
            unit.UnitHexSprite = new PlacedUnitHexSprite(null!) { Holder = EngineFixture.Engine, Unit = unit };
            sbyte x = 0;
            sbyte y = 0;
            sbyte l = 0;
            map.GetUnitLocation(hero, out x, ref y, ref l);
            Assert.Equal((sbyte)4, x);
            Assert.Equal((sbyte)9, y);
            Assert.Equal((sbyte)0, l);
            map.GetUnitLocation(new Troop(null!) { Holder = EngineFixture.Engine }, x: out x, y: ref y, level: ref l);
            Assert.Equal((sbyte)-1, x);
        }
        finally
        {
            map.SetOwner(null!);
            map.Dispose();
            CombatMap.Current = previous;
        }
    }
}
