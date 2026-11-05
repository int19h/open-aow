#nullable disable
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.House
{
    public partial class THouse
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
            if (FHitPoints == 0)
            {
                var call__1 = AoWHSSet.FImageSequences.GetImageSequence(50);
                Sequence = call__1;
                var call__2 = GetYhx();
                var call__3 = GetXhx();
                var call__4 = AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__5 = call__4.GetField(call__3, call__2);
                switch (call__5.TerrainType)
                {
                    case 3:
                        TerrainOffset = 1;
                        break;
                    case 5:
                    case 7:
                    case 12:
                        TerrainOffset = 2;
                        break;
                    default:
                        TerrainOffset = 0;
                        break;
                }

                if (Sequence != null)
                {
                    do
                    {
                        ImageIndex = Sequence.GetUnloopedFrameIndex(ObjectToInt(this) % 3 + TerrainOffset * 3);
                        if (ImageIndex != -1)
                        {
                            var call__6 = Sequence.ImageLibrary[ImageIndex];
                            call__6.Show(XhpSurf + Sequence.dXshift + 16, YhpSurf + Sequence.dYshift + 16, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }
                    while (!(Sequence == null));
                }
            }
            else
            {
                var call__7 = GetYhx();
                var call__8 = GetXhx();
                var call__9 = AoWCombatMap.MapContainer.GetMapLevel(0);
                var call__10 = call__9.GetField(call__8, call__7);
                switch (call__10.TerrainType)
                {
                    case 3:
                        TerrainOffset = 20;
                        break;
                    case 5:
                    case 7:
                    case 12:
                        TerrainOffset = 40;
                        break;
                    default:
                        TerrainOffset = 0;
                        break;
                }

                var call__11 = FResource.ImageSequenceList.GetImageSequence(TerrainOffset + FRace);
                Sequence = call__11;
                if (Sequence != null)
                {
                    do
                    {
                        switch (Sequence.Layer)
                        {
                            case 7:
                                var call__12 = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                                ImageIndex = call__12;
                                if (ImageIndex != -1)
                                {
                                    if (!AoWCombatMap.Status.Contains(THSMapStatusElement.msEditting))
                                    {
                                        var call__13 = GetXhx();
                                        var call__14 = GetYhx();
                                        AoWHSSet.FFlagControl.ShowStructureFlagEx(XhpSurf + 21 + Sequence.dXshift, YhpSurf + Sequence.dYshift + 16, call__13 + AoWHSMap.Frame + call__14 * 100, (byte)FFlagID, SurfaceDesc, false);
                                    }
                                }

                                break;
                            default:
                            {
                                var call__15 = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                                ImageIndex = call__15;
                                if (ImageIndex != -1)
                                {
                                    var call__16 = Sequence.ImageLibrary[ImageIndex];
                                    call__16.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
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

        public override void ShowShadow(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TImageSequence Sequence = default;
            int ImageIndex = default;
            int Xhp = default;
            int Yhp = default;
            if (FHitPoints > 0)
            {
                HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
                XhpSurf = Xhp - XhpSurf;
                YhpSurf = Yhp - YhpSurf;
                var call__21 = FResource.ImageSequenceList.GetImageSequence(FRace + 60);
                Sequence = call__21;
                if (Sequence != null)
                {
                    do
                    {
                        var call__22 = Sequence.GetFrameIndex(AoWCombatMap.Frame, FrameFixtureInputs.AnimationOffset(this));
                        ImageIndex = call__22;
                        if (ImageIndex != -1)
                        {
                            var call__23 = Sequence.ImageLibrary[ImageIndex];
                            call__23.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                        }

                        Sequence = Sequence.Next;
                    }
                    while (!(Sequence == null));
                }
            }
        }
    }
}
