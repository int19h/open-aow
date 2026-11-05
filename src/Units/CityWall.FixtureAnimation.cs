#nullable disable
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CityWall
{
    public partial class TCityWall
    {
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence;
            int ImageIndex;
            int Xhp = default;
            int Yhp = default;
            int TerrainOffset;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            var yhx = GetYhx();
            var xhx = GetXhx();
            switch (AoWCombatMap.MapContainer.GetMapLevel(0).GetField(xhx, yhx).TerrainType)
            {
                case 3:
                    TerrainOffset = 10;
                    break;
                case 5:
                case 7:
                case 12:
                    TerrainOffset = 20;
                    break;
                default:
                    TerrainOffset = 0;
                    break;
            }

            Sequence = FResource.ImageSequenceList.GetImageSequence(WallISIndex[FWallType - 1] + (TerrainOffset + FDamageImage));
            if (Sequence != null)
            {
                do
                {
                    switch (Sequence.Layer)
                    {
                        case 7:
                            ImageIndex = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            if (ImageIndex != -1)
                            {
                                if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
                                {
                                    var xhx2 = GetXhx();
                                    var yhx2 = GetYhx();
                                    AoWHSSet.FFlagControl.ShowFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, xhx2 + AoWHSMap.Frame + yhx2 * 100, (byte)FFlagID, SurfaceDesc, false);
                                }
                            }

                            break;
                        default:
                        {
                            ImageIndex = Sequence.GetUnloopedFrameIndex(FImageIndex);
                            if (ImageIndex != -1)
                            {
                                Sequence.ImageLibrary[ImageIndex].Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                            break;
                    }

                    Sequence = Sequence.Next;
                }
                while (!(Sequence == null));
            }
        }
    }
}
