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
        public override TMagicSphere GetMagicSphere() => TMagicSphere.msWater;

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
        public override int ResourceUserClassID() => 132275;

        // WaterNode.pas:27


        // WaterNode.pas:52
        public override AnsiString ResourceEditName() => TranslateRStr(WaterNodeRStr);

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
