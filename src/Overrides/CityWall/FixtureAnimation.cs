#nullable disable
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.CombatILTer;
using static Aow.Units.CombatILTer.CombatILTer;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.Tower;
using static Aow.Units.Tower.Tower;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CityWall
{
    public partial class TCityWall
    {
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int ImageIndex = default;
            int Xhp = default;
            int Yhp = default;
            int TerrainOffset = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            var call__1 = GetYhx();
            var call__2 = GetXhx();
            var call__3 = Aow.Units.AoWTC.AoWTC.AoWCombatMap.MapContainer.GetMapLevel(0);
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

            var call__5 = FResource.ImageSequenceList.GetImageSequence(Aow.Units.AoWTC.AoWTC.WallISIndex[FWallType - 1] + (TerrainOffset + FDamageImage));
            Sequence = call__5;
            if (Sequence != null)
            {
                do
                {
                    switch (Sequence.Layer)
                    {
                        case 7:
                            var call__6 = Sequence.GetFrameIndex(Aow.Units.AoWTC.AoWTC.AoWCombatMap.Frame, Aow.Units.AoWE.FrameFixtureInputs.AnimationOffset(this));
                            ImageIndex = call__6;
                            if (ImageIndex != -1)
                            {
                                if (!Aow.Units.AoWTC.AoWTC.AoWCombatMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                                {
                                    var call__7 = GetXhx();
                                    var call__8 = GetYhx();
                                    Aow.Units.AoWE.AoWE.AoWHSSet.FFlagControl.ShowFlagEx((XhpSurf + 21) + Sequence.dXshift, (YhpSurf + Sequence.dYshift) + 16, (call__7 + Aow.Units.AoWE.AoWE.AoWHSMap.Frame) + (call__8 * 100), unchecked((byte)FFlagID), SurfaceDesc, false);
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
