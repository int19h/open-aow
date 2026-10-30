// Translated from Merchant.pas.
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

namespace Aow.Units.Merchant
{
    // { TMerchant (instance size 48, VMT 557C9C54) }
    public partial class TMerchant : Aow.Units.AoWE.TStructure
    {
        // Merchant.pas:38
        public new const int ClassID = 131338;

        // Merchant.pas:35
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
        }

        // Merchant.pas:18
        public static new Aow.Units.Merchant.TMerchant Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Merchant.TMerchant o = new Aow.Units.Merchant.TMerchant();
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

        public new partial class Meta : Aow.Units.AoWE.TStructure.Meta
        {
            // Merchant.pas:38
            public override int ClassID() => Aow.Units.Merchant.TMerchant.ClassID;

            // Merchant.pas:18
            public new Aow.Units.Merchant.TMerchant Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Merchant.TMerchant)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Merchant.TMerchant => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TMerchantResource (instance size 100, VMT 557C9EB4) }
    public partial class TMerchantResource : Aow.Units.AoWE.TStructureResource
    {
        // Merchant.pas:47
        public new const int ClassID = 131339;

        // Merchant.pas:41
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FTerrainBound = false;
        }

        // Merchant.pas:53
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131338;
            return Result;
        }

        // Merchant.pas:25
        public static new Aow.Units.Merchant.TMerchantResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.Merchant.TMerchantResource o = new Aow.Units.Merchant.TMerchantResource();
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

        public new partial class Meta : Aow.Units.AoWE.TStructureResource.Meta
        {
            // Merchant.pas:47
            public override int ClassID() => Aow.Units.Merchant.TMerchantResource.ClassID;

            // Merchant.pas:50
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__1 = Aow.Units.AoWE.AoWE.MerchantRStr;
                var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(call__1);
                Result = call__2;
                return Result;
            }

            // Merchant.pas:25
            public new Aow.Units.Merchant.TMerchantResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Merchant.TMerchantResource)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Merchant.TMerchantResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public static partial class Merchant
    {
        // Merchant.pas:56
        public static void RegisterMerchant(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Merchant.TMerchant>(), Aow.Units.Engine.EClass.Of<Aow.Units.Merchant.TMerchantResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
