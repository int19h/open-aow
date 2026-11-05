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
            TImageSequence Sequence = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
            X = X - XhpSurf - 8;
            Y = Y - YhpSurf;
            var call__1 = FResource.ImageSequenceList.GetImageSequence(0);
            Sequence = call__1;
            var call__2 = Sequence.ImageLibrary.Get(Sequence.ImageIndex);
            call__2.Show(X, Y, SurfaceDesc);
        }

        // SingleCombatHex.pas:44
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

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
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229534;
            return Result;
        }

        // SingleCombatHex.pas:65
        public override void SetLevel(byte Value)
        {
            base.SetLevel(0);
        }

        // SingleCombatHex.pas:70
        public override THexagonControl GetHexagonControl()
        {
            THexagonControl Result = default;
            Result = null;
            return Result;
        }

        // SingleCombatHex.pas:62
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Single Hexagon";
            return Result;
        }

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
