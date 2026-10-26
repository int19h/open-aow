// Generated from AltLight.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
        // AltLight.pas:39
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = Aow.Units.Storms.TLightningStorm.Create(null);
            return Result;
        }

        public new partial class Meta : Aow.Units.Altar.TAltar.Meta
        {
            // AltLight.pas:36
            public override int ClassID()
            {
                int Result = default;
                Result = 131645;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AltLight.TAltarOfLightning => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:249
            public new Aow.Units.AltLight.TAltarOfLightning Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltLight.TAltarOfLightning o = (Aow.Units.AltLight.TAltarOfLightning)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }
        }
    }

    // { TAltarOfLightningResource (instance size 116, no added fields, VMT 557D19B4) }
    public partial class TAltarOfLightningResource : Aow.Units.Altar.TAltarResource
    {
        // AltLight.pas:44
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // AltLight.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131645;
            return Result;
        }

        // AltLight.pas:26
        public static new Aow.Units.AltLight.TAltarOfLightningResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AltLight.TAltarOfLightningResource o = new Aow.Units.AltLight.TAltarOfLightningResource();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.Altar.TAltarResource.Meta
        {
            // AltLight.pas:52
            public override int ClassID()
            {
                int Result = default;
                Result = 131646;
                return Result;
            }

            // AltLight.pas:49
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfLightningRStr);
                return Result;
            }

            // AltLight.pas:26
            public new Aow.Units.AltLight.TAltarOfLightningResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltLight.TAltarOfLightningResource o = (Aow.Units.AltLight.TAltarOfLightningResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AltLight.TAltarOfLightningResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class AltLight
    {
        // AltLight.pas:58
        public static void RegisterAltarOfLightning(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltLight.TAltarOfLightning>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltLight.TAltarOfLightningResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
