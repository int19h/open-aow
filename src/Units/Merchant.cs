// Translated from Merchant.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.Merchant
{
    public partial class TMerchant : TStructure
    {
        public TMerchant(TECustomNode owner) : base(owner)
        {
        }

        // Merchant.pas:38
        public new const int ClassID = 131338;

        // Merchant.pas:35


        // Merchant.pas:18


    }

    public partial class TMerchantResource : TStructureResource
    {
        public TMerchantResource(TECustomNode owner) : base(owner)
        {
            FTerrainBound = false;
        }

        // Merchant.pas:47
        public new const int ClassID = 131339;

        // Merchant.pas:41


        // Merchant.pas:53
        public override int ResourceUserClassID() => 131338;

        // Merchant.pas:25


        // Merchant.pas:50
        public override string ResourceEditName() => TranslateRStr(MerchantRStr);

    }

    public static partial class Merchant
    {
        // Merchant.pas:56
        public static void RegisterMerchant(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TMerchant>(), EClass.Of<TMerchantResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
