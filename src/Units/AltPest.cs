// Translated from AltPest.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.Altar;
using Aow.Units.Storms;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AltPest
{
    public partial class TAltarOfPestilence : TAltar
    {
        public TAltarOfPestilence(TECustomNode owner) : base(owner)
        {}

        // AltPest.pas:36
        public new const int ClassID = 131852;

        // AltPest.pas:39
        public override TStorm CreateStorm() => new TPestilenceStorm(null);

    }

    public partial class TAltarOfPestilenceResource : TAltarResource
    {
        public TAltarOfPestilenceResource(TECustomNode owner) : base(owner)
        {
        }

        // AltPest.pas:52
        public new const int ClassID = 131853;

        // AltPest.pas:44


        // AltPest.pas:55
        public override int ResourceUserClassID() => 131852;

        // AltPest.pas:26


        // AltPest.pas:49
        public override string ResourceEditName() => TranslateRStr(AltarOfPestilenceRStr);

    }

    public static partial class AltPest
    {
        // AltPest.pas:58
        public static void RegisterAltarOfPestilence(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TAltarOfPestilence>(), EClass.Of<TAltarOfPestilenceResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
