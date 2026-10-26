// Generated from AltPest.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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

namespace Aow.Units.AltPest
{
    // { TAltarOfPestilence (instance size 68, no added fields, VMT 557D255C) }
    public partial class TAltarOfPestilence : Aow.Units.Altar.TAltar
    {
        // AltPest.pas:39
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = Aow.Units.Storms.TPestilenceStorm.Create(null);
            return Result;
        }

        public new partial class Meta : Aow.Units.Altar.TAltar.Meta
        {
            // AltPest.pas:36
            public override int ClassID()
            {
                int Result = default;
                Result = 131852;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AltPest.TAltarOfPestilence => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:249
            public new Aow.Units.AltPest.TAltarOfPestilence Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltPest.TAltarOfPestilence o = (Aow.Units.AltPest.TAltarOfPestilence)NewInstance();
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

    // { TAltarOfPestilenceResource (instance size 116, no added fields, VMT 557D281C) }
    public partial class TAltarOfPestilenceResource : Aow.Units.Altar.TAltarResource
    {
        // AltPest.pas:44
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // AltPest.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131852;
            return Result;
        }

        // AltPest.pas:26
        public static new Aow.Units.AltPest.TAltarOfPestilenceResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AltPest.TAltarOfPestilenceResource o = new Aow.Units.AltPest.TAltarOfPestilenceResource();
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
            // AltPest.pas:52
            public override int ClassID()
            {
                int Result = default;
                Result = 131853;
                return Result;
            }

            // AltPest.pas:49
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfPestilenceRStr);
                return Result;
            }

            // AltPest.pas:26
            public new Aow.Units.AltPest.TAltarOfPestilenceResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltPest.TAltarOfPestilenceResource o = (Aow.Units.AltPest.TAltarOfPestilenceResource)NewInstance();
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
                where T : Aow.Units.AltPest.TAltarOfPestilenceResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class AltPest
    {
        // AltPest.pas:58
        public static void RegisterAltarOfPestilence(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltPest.TAltarOfPestilence>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltPest.TAltarOfPestilenceResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
