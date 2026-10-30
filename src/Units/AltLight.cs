// Translated from AltLight.pas.
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

namespace Aow.Units.AltLight
{
    // { TAltarOfLightning (instance size 68, no added fields, VMT 557D16F4) }
    public partial class TAltarOfLightning : Aow.Units.Altar.TAltar
    {
        public TAltarOfLightning(TECustomNode owner) : base(owner)
        {}

        // AltLight.pas:36
        public new const int ClassID = 131645;

        // AltLight.pas:39
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TLightningStorm(null);
            return Result;
        }

    }

    // { TAltarOfLightningResource (instance size 116, no added fields, VMT 557D19B4) }
    public partial class TAltarOfLightningResource : Aow.Units.Altar.TAltarResource
    {
        public TAltarOfLightningResource(TECustomNode owner) : base(owner)
        {
        }

        // AltLight.pas:52
        public new const int ClassID = 131646;

        // AltLight.pas:44


        // AltLight.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131645;
            return Result;
        }

        // AltLight.pas:26


        // AltLight.pas:49
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfLightningRStr);
            return Result;
        }

    }

    public static partial class AltLight
    {
        // AltLight.pas:58
        public static void RegisterAltarOfLightning(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AltLight.TAltarOfLightning>(), Aow.Units.Engine.EClass.Of<Aow.Units.AltLight.TAltarOfLightningResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
