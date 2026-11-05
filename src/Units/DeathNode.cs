// Translated from DeathNode.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.DeathNode
{
    public partial class TDeathNode : TProductionPlace
    {
        public TDeathNode(TECustomNode owner) : base(owner)
        {}

        // DeathNode.pas:37
        public new const int ClassID = 132270;

        // DeathNode.pas:40
        public override TMagicSphere GetMagicSphere() => TMagicSphere.msDeath;

    }

    public partial class TDeathNodeResource : TProductionPlaceResource
    {
        public TDeathNodeResource(TECustomNode owner) : base(owner)
        {
            FRazeDefenderBudget = 200;
            FRebuildGold = 25;
            FRebuildDays = 2;
            FTerrainBound = false;
        }

        // DeathNode.pas:55
        public new const int ClassID = 132271;

        // DeathNode.pas:43


        // DeathNode.pas:58
        public override int ResourceUserClassID() => 132270;

        // DeathNode.pas:27


        // DeathNode.pas:52
        public override AnsiString ResourceEditName() => TranslateRStr(DeathNodeRStr);

    }

    public static partial class DeathNode
    {
        // DeathNode.pas:61
        public static void RegisterDeathNode(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TDeathNode>(), EClass.Of<TDeathNodeResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
