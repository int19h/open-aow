// Translated from LifeNode.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.LifeNode
{
    public partial class TLifeNode : TProductionPlace
    {
        public TLifeNode(TECustomNode owner) : base(owner)
        {}

        // LifeNode.pas:37
        public new const int ClassID = 131953;

        // LifeNode.pas:40
        public override TMagicSphere GetMagicSphere()
        {
            TMagicSphere Result = default;
            Result = TMagicSphere.msLife;
            return Result;
        }

    }

    public partial class TLifeNodeResource : TProductionPlaceResource
    {
        public TLifeNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FTerrainBound = false;
            FRebuildGold = 25;
            FRebuildDays = 2;
        }

        // LifeNode.pas:55
        public new const int ClassID = 131954;

        // LifeNode.pas:43


        // LifeNode.pas:58
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131953;
            return Result;
        }

        // LifeNode.pas:27


        // LifeNode.pas:52
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(LifeNodeRStr);
            return Result;
        }

    }

    public static partial class LifeNode
    {
        // LifeNode.pas:61
        public static void RegisterLifeNode(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TLifeNode>(), EClass.Of<TLifeNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
