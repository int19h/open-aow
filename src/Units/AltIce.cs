// Translated from AltIce.pas.
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

namespace Aow.Units.AltIce
{
    public partial class TAltarOfIce : Aow.Units.Altar.TAltar
    {
        public TAltarOfIce(TECustomNode owner) : base(owner)
        {}

        // AltIce.pas:36
        public new const int ClassID = 131659;

        // AltIce.pas:39
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TIceStorm(null);
            return Result;
        }

    }

    public partial class TAltarOfIceResource : Aow.Units.Altar.TAltarResource
    {
        public TAltarOfIceResource(TECustomNode owner) : base(owner)
        {
        }

        // AltIce.pas:52
        public new const int ClassID = 131660;

        // AltIce.pas:44


        // AltIce.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131659;
            return Result;
        }

        // AltIce.pas:26


        // AltIce.pas:49
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfIceRStr);
            return Result;
        }

    }

    public static partial class AltIce
    {
        // AltIce.pas:58
        public static void RegisterAltarOfIce(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AltIce.TAltarOfIce>(), Aow.Units.Engine.EClass.Of<Aow.Units.AltIce.TAltarOfIceResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
