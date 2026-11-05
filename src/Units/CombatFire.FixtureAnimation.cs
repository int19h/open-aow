#nullable disable
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.CombatFire
{
    public partial class TCombatFire
    {
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            int ImageIndex = default;
            int Xhp = default;
            int Yhp = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            var call__1 = AoWHSSet.FImageSequences.GetImageSequence(60);
            Sequence = call__1;
            if (Sequence != null)
            {
                do
                {
                    var call__2 = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                    ImageIndex = call__2;
                    if (ImageIndex != -1)
                    {
                        var call__3 = Sequence.ImageLibrary.Get(ImageIndex);
                        call__3.Show(XhpSurf + Sequence.dXshift + FXShift, YhpSurf + Sequence.dYshift + FYShift, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
        }
    }
}
