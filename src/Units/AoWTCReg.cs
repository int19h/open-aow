// Translated from AoWTCReg.pas.
using Aow.Game.Engine;
using static Aow.Game.Combat.AoWTCUnit;
using static Aow.Game.Commands.CombatTEUnit;
using static Aow.Game.Draw.CombatFireUnit;
using static Aow.Game.Draw.CombatObstacleUnit;
using static Aow.Game.World.CenterViewLocUnit;
using static Aow.Game.World.FleeLocUnit;
using static Aow.Game.World.Hexes.CombatHexUnit;
using static Aow.Game.World.Hexes.CombatRoadUnit;
using static Aow.Game.World.Hexes.CombatTerrainUnit;
using static Aow.Game.World.Hexes.IsoCombatHexUnit;
using static Aow.Game.World.Hexes.SingleCombatHexUnit;
using static Aow.Game.World.Terrain.CityWallUnit;
using static Aow.Game.World.Terrain.CombatILTerUnit;
using static Aow.Game.World.Terrain.HouseUnit;
using static Aow.Game.World.Terrain.MainStructureUnit;
using static Aow.Game.World.Terrain.TownhallUnit;
using static Aow.Game.World.Terrain.WorkShopUnit;

namespace Aow.Game.Combat;

public static partial class AoWTCRegUnit
{
    // AoWTCReg.pas:22
    // { AoWTCReg, AoWTCPCK.dpl: register the tactical-combat classes in original order. }
    public static void RegisterTacticalCombat(TEngine AEngine)
    {
        RegisterAoWTCClasses(AEngine);
        RegisterCombatILTerrain(AEngine);
        RegisterCombatHexagon(AEngine);
        RegisterCombatRoad(AEngine);
        RegisterCombatTerrain(AEngine);
        RegisterCityWall(AEngine);
        RegisterWorkshop(AEngine);
        RegisterTownhall(AEngine);
        RegisterHouse(AEngine);
        RegisterFleeLocation(AEngine);
        RegisterSingleCombatHexagon(AEngine);
        RegisterCombatObstacle(AEngine);
        RegisterCombatTE(AEngine);
        RegisterIsometricCombatHexagon(AEngine);
        RegisterMainStructure(AEngine);
        RegisterCenterViewLocation(AEngine);
        RegisterCombatFire(AEngine);
    }

    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
