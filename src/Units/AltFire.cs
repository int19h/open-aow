// Translated from AltFire.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.Altar;
using Aow.Units.Storms;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AltFire
{
    public partial class TAltarOfFire : TAltar
    {
        public TAltarOfFire(TECustomNode owner) : base(owner)
        {
        }

        // AltFire.pas:42
        public new const int ClassID = 131642;

        // AltFire.pas:37


        // AltFire.pas:45
        public override TStorm CreateStorm() => new TFireStorm(null);

        // AltFire.pas:19


    }

    public partial class TAltarOfFireResource : TAltarResource
    {
        public TAltarOfFireResource(TECustomNode owner) : base(owner)
        {
        }

        // AltFire.pas:58
        public new const int ClassID = 131643;

        // AltFire.pas:50


        // AltFire.pas:61
        public override int ResourceUserClassID() => 131642;

        // AltFire.pas:27


        // AltFire.pas:55
        public override string ResourceEditName() => TranslateRStr(AltarOfFireRStr);

    }

    public static partial class AltFire
    {
        // AltFire.pas:64
        public static void RegisterAltarOfFire(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TAltarOfFire>(), EClass.Of<TAltarOfFireResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
