// Translated from Hill.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Hill
{
    public partial class THillMO : Aow.Units.Terrain.TTerrainMO
    {
        public THillMO(TECustomNode owner) : base(owner)
        {}

        // Hill.pas:39
        public new const int ClassID = 131432;

        // Terrain.pas:19


    }

    public partial class THillMOR : Aow.Units.Terrain.TTerrainMOR
    {
        public THillMOR(TECustomNode owner) : base(owner)
        {}

        // Hill.pas:42
        public new const int ClassID = 131433;

        // Hill.pas:48
        public override Aow.Units.Engine.TEObject CreateResourceUser()
        {
            Aow.Units.Engine.TEObject Result = default;
            Aow.Units.Hill.THillMO MO = default;
            MO = new Aow.Units.Hill.THillMO(null);
            MO.SetResource(this);
            MO.SetResourceLinked(true);
            Result = MO;
            return Result;
        }

        // Hill.pas:45
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__1 = Aow.Units.AoWE.AoWE.HillRStr;
            var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(call__1);
            Result = call__2;
            return Result;
        }

    }

    public static partial class Hill
    {
        // Hill.pas:58
        public static void RegisterHill(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Hill.THillMO>()]);
        }

        public static readonly Aow.Rtl.AnsiString HillNameRStr = (AnsiString)"Hill";
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
