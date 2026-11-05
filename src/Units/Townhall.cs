// Translated from Townhall.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.CombatStructure;

namespace Aow.Units.Townhall
{
    public partial class TTownhall : TCombatStructure
    {
        public TTownhall(TECustomNode owner) : base(owner)
        {}

        // Townhall.pas:36
        public new const int ClassID = 2229520;

    }

    public partial class TTownhallResource : TCombatStructureResource
    {
        public TTownhallResource(TECustomNode owner) : base(owner)
        {}

        // Townhall.pas:39
        public new const int ClassID = 2229521;

        // Townhall.pas:45
        public override int ResourceUserClassID() => 2229520;

        // Townhall.pas:42
        public override string ResourceEditName() => "Combat Townhall";

    }

    public static partial class Townhall
    {
        // Townhall.pas:48
        public static void RegisterTownhall(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TTownhallResource>(), EClass.Of<TTownhall>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
