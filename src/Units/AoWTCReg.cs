// Translated from AoWTCReg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.WorkShop;
using static Aow.Units.WorkShop.WorkShop;
using Aow.Units.Townhall;
using static Aow.Units.Townhall.Townhall;
using Aow.Units.House;
using static Aow.Units.House.House;
using Aow.Units.SingleCombatHex;
using static Aow.Units.SingleCombatHex.SingleCombatHex;
using Aow.Units.IsoCombatHex;
using static Aow.Units.IsoCombatHex.IsoCombatHex;
using Aow.Units.MainStructure;
using static Aow.Units.MainStructure.MainStructure;
using Aow.Units.CenterViewLoc;
using static Aow.Units.CenterViewLoc.CenterViewLoc;
using Aow.Units.FleeLoc;
using static Aow.Units.FleeLoc.FleeLoc;
using Aow.Units.CombatHex;
using static Aow.Units.CombatHex.CombatHex;
using Aow.Units.CombatTE;
using static Aow.Units.CombatTE.CombatTE;
using Aow.Units.CombatTerrain;
using static Aow.Units.CombatTerrain.CombatTerrain;
using Aow.Units.CombatRoad;
using static Aow.Units.CombatRoad.CombatRoad;
using Aow.Units.CombatObstacle;
using static Aow.Units.CombatObstacle.CombatObstacle;
using Aow.Units.CombatILTer;
using static Aow.Units.CombatILTer.CombatILTer;
using Aow.Units.CityWall;
using static Aow.Units.CityWall.CityWall;
using Aow.Units.CombatFire;
using static Aow.Units.CombatFire.CombatFire;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.AoWTCReg
{
    public static partial class AoWTCReg
    {
        // AoWTCReg.pas:22
        // { AoWTCReg, AoWTCPCK.dpl: register the tactical-combat classes in original order. }
        public static void RegisterTacticalCombat(Aow.Units.Engine.TEngine AEngine)
        {
            Aow.Units.AoWTC.AoWTC.RegisterAoWTCClasses(AEngine);
            Aow.Units.CombatILTer.CombatILTer.RegisterCombatILTerrain(AEngine);
            Aow.Units.CombatHex.CombatHex.RegisterCombatHexagon(AEngine);
            Aow.Units.CombatRoad.CombatRoad.RegisterCombatRoad(AEngine);
            Aow.Units.CombatTerrain.CombatTerrain.RegisterCombatTerrain(AEngine);
            Aow.Units.CityWall.CityWall.RegisterCityWall(AEngine);
            Aow.Units.WorkShop.WorkShop.RegisterWorkshop(AEngine);
            Aow.Units.Townhall.Townhall.RegisterTownhall(AEngine);
            Aow.Units.House.House.RegisterHouse(AEngine);
            Aow.Units.FleeLoc.FleeLoc.RegisterFleeLocation(AEngine);
            Aow.Units.SingleCombatHex.SingleCombatHex.RegisterSingleCombatHexagon(AEngine);
            Aow.Units.CombatObstacle.CombatObstacle.RegisterCombatObstacle(AEngine);
            Aow.Units.CombatTE.CombatTE.RegisterCombatTE(AEngine);
            Aow.Units.IsoCombatHex.IsoCombatHex.RegisterIsometricCombatHexagon(AEngine);
            Aow.Units.MainStructure.MainStructure.RegisterMainStructure(AEngine);
            Aow.Units.CenterViewLoc.CenterViewLoc.RegisterCenterViewLocation(AEngine);
            Aow.Units.CombatFire.CombatFire.RegisterCombatFire(AEngine);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
