// Translated from AltLife.pas.
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

namespace Aow.Units.AltLife
{
    // { TAltarOfLife (instance size 68, no added fields, VMT 557D1BCC) }
    public partial class TAltarOfLife : Aow.Units.Altar.TAltar
    {
        // AltLife.pas:39
        public override Aow.Units.Storms.TStorm CreateStorm()
        {
            Aow.Units.Storms.TStorm Result = default;
            Result = Aow.Units.Storms.TDivineStorm.Create(null);
            return Result;
        }

        public new partial class Meta : Aow.Units.Altar.TAltar.Meta
        {
            // AltLife.pas:36
            public override int ClassID()
            {
                int Result = default;
                Result = 131635;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AltLife.TAltarOfLife => (Meta)MetaRegistry.Of(typeof(T));
            // intf_P1_09.inc:249
            public new Aow.Units.AltLife.TAltarOfLife Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltLife.TAltarOfLife o = (Aow.Units.AltLife.TAltarOfLife)NewInstance();
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

    // { TAltarOfLifeResource (instance size 116, no added fields, VMT 557D1E84) }
    public partial class TAltarOfLifeResource : Aow.Units.Altar.TAltarResource
    {
        // AltLife.pas:44
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // AltLife.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131635;
            return Result;
        }

        // AltLife.pas:26
        public static new Aow.Units.AltLife.TAltarOfLifeResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AltLife.TAltarOfLifeResource o = new Aow.Units.AltLife.TAltarOfLifeResource();
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
            // AltLife.pas:52
            public override int ClassID()
            {
                int Result = default;
                Result = 131636;
                return Result;
            }

            // AltLife.pas:49
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AltarOfLifeRStr);
                return Result;
            }

            // AltLife.pas:26
            public new Aow.Units.AltLife.TAltarOfLifeResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AltLife.TAltarOfLifeResource o = (Aow.Units.AltLife.TAltarOfLifeResource)NewInstance();
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
                where T : Aow.Units.AltLife.TAltarOfLifeResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class AltLife
    {
        // AltLife.pas:58
        public static void RegisterAltarOfLife(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltLife.TAltarOfLife>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AltLife.TAltarOfLifeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
