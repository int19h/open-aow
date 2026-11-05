// Translated from AoWTCReg.pas.
using Aow.Units.Engine;
using static Aow.Units.WorkShop.WorkShopUnit;
using static Aow.Units.Townhall.TownhallUnit;
using static Aow.Units.House.HouseUnit;
using static Aow.Units.SingleCombatHex.SingleCombatHexUnit;
using static Aow.Units.IsoCombatHex.IsoCombatHexUnit;
using static Aow.Units.MainStructure.MainStructureUnit;
using static Aow.Units.CenterViewLoc.CenterViewLocUnit;
using static Aow.Units.FleeLoc.FleeLocUnit;
using static Aow.Units.CombatHex.CombatHexUnit;
using static Aow.Units.CombatTE.CombatTEUnit;
using static Aow.Units.CombatTerrain.CombatTerrainUnit;
using static Aow.Units.CombatRoad.CombatRoadUnit;
using static Aow.Units.CombatObstacle.CombatObstacleUnit;
using static Aow.Units.CombatILTer.CombatILTerUnit;
using static Aow.Units.CityWall.CityWallUnit;
using static Aow.Units.CombatFire.CombatFireUnit;
using static Aow.Units.AoWTC.AoWTCUnit;

namespace Aow.Units.AoWTCReg;

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
