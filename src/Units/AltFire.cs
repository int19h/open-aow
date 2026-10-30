// Translated from AltFire.pas.
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

namespace Aow.Units.AltFire
{
    // { TAltarOfFire (instance size 68, no added fields, VMT 557CF5F8) }
    public partial class TAltarOfFire : Aow.Units.Altar.TAltar
    {
        public TAltarOfFire(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // AltFire.pas:42
        public new const int ClassID = 131642;

        // AltFire.pas:37


        // AltFire.pas:45
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = new Aow.Units.Storms.TFireStorm(null);
            return Result;
        }

        // AltFire.pas:19


        public new partial class Meta : Aow.Units.Altar.TAltar.Meta
        {
            // AltFire.pas:42
            public override int ClassID() => Aow.Units.AltFire.TAltarOfFire.ClassID;

            // AltFire.pas:19
            public new Aow.Units.AltFire.TAltarOfFire Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.AltFire.TAltarOfFire)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.AltFire.TAltarOfFire => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TAltarOfFireResource (instance size 116, no added fields, VMT 557CF8B0) }
    public partial class TAltarOfFireResource : Aow.Units.Altar.TAltarResource
    {
        public TAltarOfFireResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
        }

        // AltFire.pas:58
        public new const int ClassID = 131643;

        // AltFire.pas:50


        // AltFire.pas:61
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131642;
            return Result;
        }

        // AltFire.pas:27


        // AltFire.pas:55
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfFireRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.Altar.TAltarResource.Meta
        {
            // AltFire.pas:58
            public override int ClassID() => Aow.Units.AltFire.TAltarOfFireResource.ClassID;

            // AltFire.pas:27
            public new Aow.Units.AltFire.TAltarOfFireResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.AltFire.TAltarOfFireResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.AltFire.TAltarOfFireResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class AltFire
    {
        // AltFire.pas:64
        public static void RegisterAltarOfFire(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.AltFire.TAltarOfFire>(), Aow.Units.Engine.EClass.Of<Aow.Units.AltFire.TAltarOfFireResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
