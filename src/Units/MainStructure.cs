// Translated from MainStructure.pas.
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
using Aow.Units.CombatStructure;
using static Aow.Units.CombatStructure.CombatStructure;

namespace Aow.Units.MainStructure
{
    public partial class TMainStructure : Aow.Units.CombatStructure.TCombatStructure
    {
        public TMainStructure(TECustomNode owner) : base(owner)
        {}

        // MainStructure.pas:37
        public new const int ClassID = 2229532;

        // MainStructure.pas:40
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            Result = TerrainType;
            return Result;
        }

    }

    public partial class TMainStructureResource : Aow.Units.CombatStructure.TCombatStructureResource
    {
        public TMainStructureResource(TECustomNode owner) : base(owner)
        {}

        // MainStructure.pas:43
        public new const int ClassID = 2229533;

        // MainStructure.pas:49
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229532;
            return Result;
        }

        // MainStructure.pas:46
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Combat Main Structure";
            return Result;
        }

    }

    public static partial class MainStructure
    {
        // MainStructure.pas:52
        public static void RegisterMainStructure(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.MainStructure.TMainStructureResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.MainStructure.TMainStructure>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
