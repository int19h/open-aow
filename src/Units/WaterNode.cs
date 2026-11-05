// Translated from WaterNode.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.WaterNode
{
    public partial class TWaterNode : TProductionPlace
    {
        public TWaterNode(TECustomNode owner) : base(owner)
        {}

        // WaterNode.pas:37
        public new const int ClassID = 132275;

        // WaterNode.pas:40
        public override TMagicSphere GetMagicSphere()
        {
            TMagicSphere Result = default;
            Result = TMagicSphere.msWater;
            return Result;
        }

    }

    public partial class TWaterNodeResource : TProductionPlaceResource
    {
        public TWaterNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FTerrainBound = false;
            FRebuildGold = 50;
            FRebuildDays = 2;
        }

        // WaterNode.pas:55
        public new const int ClassID = 132276;

        // WaterNode.pas:43


        // WaterNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132275;
            return Result;
        }

        // WaterNode.pas:27


        // WaterNode.pas:52
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(WaterNodeRStr);
            return Result;
        }

    }

    public static partial class WaterNode
    {
        // WaterNode.pas:61
        public static void RegisterWaterNode(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TWaterNode>(), EClass.Of<TWaterNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
