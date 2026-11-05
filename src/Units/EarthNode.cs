// Translated from EarthNode.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.EarthNode
{
    public partial class TEarthNode : TProductionPlace
    {
        public TEarthNode(TECustomNode owner) : base(owner)
        {}

        // EarthNode.pas:36
        public new const int ClassID = 132302;

        // EarthNode.pas:39
        public override TMagicSphere GetMagicSphere()
        {
            TMagicSphere Result = default;
            Result = TMagicSphere.msEarth;
            return Result;
        }

    }

    public partial class TEarthNodeResource : TProductionPlaceResource
    {
        public TEarthNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 35;
            FRebuildDays = 2;
        }

        // EarthNode.pas:53
        public new const int ClassID = 132303;

        // EarthNode.pas:42


        // EarthNode.pas:56
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132302;
            return Result;
        }

        // EarthNode.pas:26


        // EarthNode.pas:50
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(EarthNodeRStr);
            return Result;
        }

    }

    public static partial class EarthNode
    {
        // EarthNode.pas:59
        public static void RegisterEarthNode(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TEarthNode>(), EClass.Of<TEarthNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
