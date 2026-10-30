// Translated from AltDeath.pas.
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
using Aow.Units.Altar;
using static Aow.Units.Altar.Altar;
using Aow.Units.Storms;
using static Aow.Units.Storms.Storms;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AltDeath
{
    // { TAltarOfDeath (instance size 68, no added fields, VMT 557D2094) }
    public partial class TAltarOfDeath : Aow.Units.Altar.TAltar
    {
        public TAltarOfDeath(TECustomNode owner) : base(owner)
        {}

        // AltDeath.pas:36
        public new const int ClassID = 131637;

        // AltDeath.pas:39
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TDeathStorm(null);
            return Result;
        }

    }

    // { TAltarOfDeathResource (instance size 116, no added fields, VMT 557D234C) }
    public partial class TAltarOfDeathResource : Aow.Units.Altar.TAltarResource
    {
        public TAltarOfDeathResource(TECustomNode owner) : base(owner)
        {
        }

        // AltDeath.pas:52
        public new const int ClassID = 131638;

        // AltDeath.pas:44


        // AltDeath.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131637;
            return Result;
        }

        // AltDeath.pas:26


        // AltDeath.pas:49
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfDeathRStr);
            return Result;
        }

    }

    public static partial class AltDeath
    {
        // AltDeath.pas:58
        public static void RegisterAltarOfDeath(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AltDeath.TAltarOfDeath>(), Aow.Units.Engine.EClass.Of<Aow.Units.AltDeath.TAltarOfDeathResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
