// Translated from WorkShop.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.CombatStructure;

namespace Aow.Units.WorkShop
{
    public partial class TWorkshop : TCombatStructure
    {
        public TWorkshop(TECustomNode owner) : base(owner)
        {}

        // WorkShop.pas:36
        public new const int ClassID = 2229512;

    }

    public partial class TWorkshopResource : TCombatStructureResource
    {
        public TWorkshopResource(TECustomNode owner) : base(owner)
        {}

        // WorkShop.pas:39
        public new const int ClassID = 2229513;

        // WorkShop.pas:45
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229512;
            return Result;
        }

        // WorkShop.pas:42
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Workshop";
            return Result;
        }

    }

    public static partial class WorkShop
    {
        // WorkShop.pas:48
        public static void RegisterWorkshop(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TWorkshopResource>(), EClass.Of<TWorkshop>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
