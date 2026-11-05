// Translated from AltLife.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.Altar;
using Aow.Units.Storms;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AltLife
{
    public partial class TAltarOfLife : TAltar
    {
        public TAltarOfLife(TECustomNode owner) : base(owner)
        {}

        // AltLife.pas:36
        public new const int ClassID = 131635;

        // AltLife.pas:39
        public override TStorm CreateStorm()
        {
            TStorm Result = default;
            Result = new TDivineStorm(null);
            return Result;
        }

    }

    public partial class TAltarOfLifeResource : TAltarResource
    {
        public TAltarOfLifeResource(TECustomNode owner) : base(owner)
        {
        }

        // AltLife.pas:52
        public new const int ClassID = 131636;

        // AltLife.pas:44


        // AltLife.pas:55
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131635;
            return Result;
        }

        // AltLife.pas:26


        // AltLife.pas:49
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(AltarOfLifeRStr);
            return Result;
        }

    }

    public static partial class AltLife
    {
        // AltLife.pas:58
        public static void RegisterAltarOfLife(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TAltarOfLife>(), EClass.Of<TAltarOfLifeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
