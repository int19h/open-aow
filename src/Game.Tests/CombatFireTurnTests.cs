using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;

namespace Aow.Game.Tests;

/// <summary>
/// CombatFire.NewTurn sends message 200711 to its hex (CombatFire.pas:98, CityWall.pas:317).
/// Index 1 of the tactical table is one die at attack 10. A wooden wall's defense is -2.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class CombatFireTurnTests
{
    [Fact]
    public void WoodenWallFireRollsTheTableDieAtSeed1()
    {
        StrategicMap map = new(null, holder: EngineFixture.Engine);
        IDisposable restoreMap = EngineFixture.UseMap(map);
        IDisposable restoreCombat = EngineFixture.UseCombatMap(CombatMap.Current);
        CombatMap combat = new(null, holder: EngineFixture.Engine);
        try
        {
            EngineFixture.SetCombatMap(combat);
            map.SynchronizeBegin();
            map.DefendersSeed = 1;

            MapContainer container = new(null) { Holder = EngineFixture.Engine, Map = combat };
            MapLevel level = new(container);
            const int side = 5;
            level.CreateMapFieldList(side, side);
            container.Width = side;
            container.Height = side;
            container.LevelCount = 1;
            combat.MapContainer = container;
            for (int x = 0; x < side; x++)
            {
                for (int y = 0; y < side; y++)
                {
                    bool centre = x == 2 && y == 2;
                    level.GetField(x, y).SetTerrain(centre ? TerrainType.Grass : TerrainType.Border);
                }
            }

            MapField field = level.GetField(2, 2);
            CityWall wall = new(null) { Holder = EngineFixture.Engine };
            wall.SetResource(new CityWall.Resource(null!) { Holder = EngineFixture.Engine });
            MemberNames.Field(typeof(MultiHexMapObject), name: "StoredHexX")!.SetValue(wall, value: 2);
            MemberNames.Field(typeof(MultiHexMapObject), name: "StoredHexY")!.SetValue(wall, value: 2);
            MemberNames.Field(typeof(CityWall), name: "WallType")!.SetValue(wall, value: 1);
            wall.SetHitPoints(TacticalTables.WallMaxHitPoints[0]);
            wall.SetOwnerWithoutNotify(field);
            _ = field.AddHexSprite(wall, hexNumber: 0);

            // A fire stands on its field. The constructor admits only that owner.
            CombatFire fire = new(field) { Holder = EngineFixture.Engine };
            fire.Activate();
            fire.NewTurn();
            Assert.Equal(7, wall.HitPoints);

            // Seed 1 makes the map random return 526467. The next d20 is 10, and one die deals 1.
            // A zero die count deals 0 and the wall stays at 7.
            fire.NewTurn();

            Assert.Equal(1, TacticalTables.TacticalBattleDamage[1]);
            Assert.Equal(10, TacticalTables.TacticalBattleAttack[1]);
            Assert.Equal(6, wall.HitPoints);
        }
        finally
        {
            combat.SetOwner(null);
            combat.Dispose();
            map.Dispose();
            restoreCombat.Dispose();
            restoreMap.Dispose();
        }
    }
}
