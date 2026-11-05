// Translated from SingleCombatHex.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.SingleCombatHex
{
    public partial class TSingleCombatHexagon : TAbstractHexagon
    {
        public TSingleCombatHexagon(TECustomNode owner) : base(owner)
        {}

        // SingleCombatHex.pas:41
        public new const int ClassID = 2229534;

        // SingleCombatHex.pas:47
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            TImageSequence Sequence;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
            X = X - XhpSurf - 8;
            Y = Y - YhpSurf;
            Sequence = FResource.ImageSequenceList.GetImageSequence(0);
            Sequence.ImageLibrary.Get(Sequence.ImageIndex).Show(X, Y, SurfaceDesc);
        }

        // SingleCombatHex.pas:44
        public override TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

    }

    public partial class TSingleCombatHexagonResource : TAbstractHexagonResource
    {
        public TSingleCombatHexagonResource(TECustomNode owner) : base(owner)
        {}

        // SingleCombatHex.pas:59
        public new const int ClassID = 2229535;

        // SingleCombatHex.pas:73
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // SingleCombatHex.pas:79
        public override int ResourceUserClassID() => 2229534;

        // SingleCombatHex.pas:65
        public override void SetLevel(byte Value)
        {
            base.SetLevel(0);
        }

        // SingleCombatHex.pas:70
        public override THexagonControl GetHexagonControl() => null;

        // SingleCombatHex.pas:62
        public override string ResourceEditName() => "Combat Single Hexagon";

    }

    public static partial class SingleCombatHex
    {
        // SingleCombatHex.pas:82
        public static void RegisterSingleCombatHexagon(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TSingleCombatHexagonResource>(), EClass.Of<TSingleCombatHexagon>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
