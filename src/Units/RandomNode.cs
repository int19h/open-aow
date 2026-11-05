// Translated from RandomNode.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.RandomNode
{
    public partial class TRandomNode : TProductionPlace
    {
        public TRandomNode(TECustomNode owner) : base(owner)
        {}

        // RandomNode.pas:35
        public new const int ClassID = 133154;

    }

    public partial class TRandomNodeResource : TProductionPlaceResource
    {
        public TRandomNodeResource(TECustomNode owner) : base(owner)
        {
        }

        // RandomNode.pas:46
        public new const int ClassID = 133155;

        // RandomNode.pas:38


        // RandomNode.pas:49
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 133154;
            return Result;
        }

        // RandomNode.pas:25


        // RandomNode.pas:43
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = TranslateRStr(RandomNodeRStr);
            return Result;
        }

    }

    public static partial class RandomNode
    {
        // RandomNode.pas:52
        public static void RegisterRandomNode(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TRandomNode>(), EClass.Of<TRandomNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
