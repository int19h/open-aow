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
            TImageSequence Sequence = default;
            int ImageIndex = default;
            int Xhp = default;
            int Yhp = default;
            int TerrainOffset = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            var call__1 = GetYhx();
            var call__2 = GetXhx();
            var call__3 = AoWCombatMap.MapContainer.GetMapLevel(0);
            var call__4 = call__3.GetField(call__2, call__1);
            switch (call__4.TerrainType)
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

            var call__5 = FResource.ImageSequenceList.GetImageSequence(WallISIndex[FWallType - 1] + (TerrainOffset + FDamageImage));
            Sequence = call__5;
            if (Sequence != null)
            {
                do
                {
                    switch (Sequence.Layer)
                    {
                        case 7:
                            var call__6 = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                            ImageIndex = call__6;
                            if (ImageIndex != -1)
                            {
                                if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
                                {
                                    var call__7 = GetXhx();
                                    var call__8 = GetYhx();
                                    AoWHSSet.FFlagControl.ShowFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, call__7 + AoWHSMap.Frame + call__8 * 100, (byte)FFlagID, SurfaceDesc, false);
                                }
                            }

                            break;
                        default:
                        {
                            var call__9 = Sequence.GetUnloopedFrameIndex(FImageIndex);
                            ImageIndex = call__9;
                            if (ImageIndex != -1)
                            {
                                var call__10 = Sequence.ImageLibrary[ImageIndex];
                                call__10.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
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
