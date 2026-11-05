// Translated from MainStructure.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.CombatStructure;

namespace Aow.Units.MainStructure
{
    public partial class TMainStructure : TCombatStructure
    {
        public TMainStructure(TECustomNode owner) : base(owner)
        {}

        // MainStructure.pas:37
        public new const int ClassID = 2229532;

        // MainStructure.pas:40
        public override int GetTerrainTypeImage(int TerrainType)
        {
            int Result = default;
            Result = TerrainType;
            return Result;
        }

    }

    public partial class TMainStructureResource : TCombatStructureResource
    {
        public TMainStructureResource(TECustomNode owner) : base(owner)
        {}

        // MainStructure.pas:43
        public new const int ClassID = 2229533;

        // MainStructure.pas:49
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229532;
            return Result;
        }

        // MainStructure.pas:46
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Main Structure";
            return Result;
        }

    }

    public static partial class MainStructure
    {
        // MainStructure.pas:52
        public static void RegisterMainStructure(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TMainStructureResource>(), EClass.Of<TMainStructure>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
