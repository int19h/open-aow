// Translated from AltDeath.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.Altar;
using Aow.Units.Storms;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AltDeath
{
    public partial class TAltarOfDeath : TAltar
    {
        public TAltarOfDeath(TECustomNode owner) : base(owner)
        {}

        // AltDeath.pas:36
        public new const int ClassID = 131637;

        // AltDeath.pas:39
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TDeathStorm(null);
            return Result;
        }

    }

    public partial class TAltarOfDeathResource : TAltarResource
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
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(AltarOfDeathRStr);
            return Result;
        }

    }

    public static partial class AltDeath
    {
        // AltDeath.pas:58
        public static void RegisterAltarOfDeath(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TAltarOfDeath>(), EClass.Of<TAltarOfDeathResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
