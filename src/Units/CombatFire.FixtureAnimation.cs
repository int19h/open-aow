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
            TImageSequence Sequence;
            int ImageIndex;
            int Xhp = default;
            int Yhp = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            Sequence = AoWHSSet.FImageSequences.GetImageSequence(60);
            if (Sequence != null)
            {
                do
                {
                    ImageIndex = Sequence.GetFrameIndex(AoWHSMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                    if (ImageIndex != -1)
                    {
                        Sequence.ImageLibrary.Get(ImageIndex).Show(XhpSurf + Sequence.dXshift + FXShift, YhpSurf + Sequence.dYshift + FYShift, SurfaceDesc);
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
        }
    }
}
