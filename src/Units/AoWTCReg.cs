// Translated from AoWTCReg.pas.
using Aow.Units.Engine;
using static Aow.Units.WorkShop.WorkShop;
using static Aow.Units.Townhall.Townhall;
using static Aow.Units.House.House;
using static Aow.Units.SingleCombatHex.SingleCombatHex;
using static Aow.Units.IsoCombatHex.IsoCombatHex;
using static Aow.Units.MainStructure.MainStructure;
using static Aow.Units.CenterViewLoc.CenterViewLoc;
using static Aow.Units.FleeLoc.FleeLoc;
using static Aow.Units.CombatHex.CombatHex;
using static Aow.Units.CombatTE.CombatTE;
using static Aow.Units.CombatTerrain.CombatTerrain;
using static Aow.Units.CombatRoad.CombatRoad;
using static Aow.Units.CombatObstacle.CombatObstacle;
using static Aow.Units.CombatILTer.CombatILTer;
using static Aow.Units.CityWall.CityWall;
using static Aow.Units.CombatFire.CombatFire;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.AoWTCReg;

public static partial class AoWTCReg
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
