// Generated from AltIce.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
    // { TAltarOfIce (instance size 68, no added fields, VMT 557CFAF0) }
    public partial class TAltarOfIce : Aow.Units.Altar.TAltar
    {
        // AltIce.pas:39
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = Aow.Units.Storms.TIceStorm.Create(null);
            return Result;
        }

        public new partial class Meta : Aow.Units.Altar.TAltar.Meta
        {
            // AltIce.pas:36
            public override int ClassID()
            {
                int Result = default;
                Result = 131659;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AltIce.TAltarOfIce => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:249
            public new Aow.Units.AltIce.TAltarOfIce Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltIce.TAltarOfIce o = (Aow.Units.AltIce.TAltarOfIce)NewInstance();
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

    // { TAltarOfIceResource (instance size 116, no added fields, VMT 557CFDA0) }
    public partial class TAltarOfIceResource : Aow.Units.Altar.TAltarResource
    {
        // AltIce.pas:44
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // AltIce.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131659;
            return Result;
        }

        // AltIce.pas:26
        public static new Aow.Units.AltIce.TAltarOfIceResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AltIce.TAltarOfIceResource o = new Aow.Units.AltIce.TAltarOfIceResource();
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
            // AltIce.pas:52
            public override int ClassID()
            {
                int Result = default;
                Result = 131660;
                return Result;
            }

            // AltIce.pas:49
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfIceRStr);
                return Result;
            }

            // AltIce.pas:26
            public new Aow.Units.AltIce.TAltarOfIceResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltIce.TAltarOfIceResource o = (Aow.Units.AltIce.TAltarOfIceResource)NewInstance();
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
                where T : Aow.Units.AltIce.TAltarOfIceResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class AltIce
    {
        // AltIce.pas:58
        public static void RegisterAltarOfIce(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltIce.TAltarOfIce>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltIce.TAltarOfIceResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
