// Generated from AoWHex.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;

namespace Aow.Units.AoWHex
{
    // { TAoWHexagon (instance size 20, VMT 5579A1D0): inherited +$10 transition
//     images [0..2], +$13 side flags. }
    public partial class TAoWHexagon : Aow.Units.Hexagon.THexagon
    {
        // AoWHex.pas:119
        public override void ChangeTerrain(in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            Aow.Units.HSEngine.TMapObjectResource R = default;
            bool bool__1 = (Msg.HN == 0);
            if (bool__1)
            {
                var call__2 = GetTerrain(0);
                bool__1 = (call__2 != Msg.TerrainType);
            }

            if (bool__1)
            {
                var call__3 = FResource.ClassType.ClassID();
                var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.HexagonControl.GetRndResource(Msg.TerrainType, call__3);
                R = call__4;
                if (R != null)
                {
                    SetResource(R);
                    UpdateTransitions();
                    Aow.Units.AoWE.AoWE.AoWHSMap.InvalidateStaticScene(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ((Aow.Units.HSEngine.TMapField)Owner).Lhx);
                }
                else
                {
                    var call__5 = GetLhx();
                    var call__6 = GetYhx();
                    var call__7 = GetXhx();
                    FResource.Engine.HSSet.HexagonControl.Place(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)call__7), unchecked((sbyte)call__6), unchecked((sbyte)call__5), unchecked((sbyte)Msg.TerrainType));
                }
            }
        }

        // AoWHex.pas:139
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Msg)
        {
            if (TSet<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)Msg.TerrainType))
            {
                if (unchecked((short)Msg.TerrainType) != 14)
                {
                    var call__8 = GetLhx();
                    var call__9 = GetYhx();
                    var call__10 = GetXhx();
                    FResource.Engine.HSSet.HexagonControl.Place(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)call__10), unchecked((sbyte)call__9), unchecked((sbyte)call__8), unchecked((sbyte)Msg.TerrainType));
                }
                else
                {
                    var call__11 = GetLhx();
                    var call__12 = GetYhx();
                    var call__13 = GetXhx();
                    FResource.Engine.HSSet.HexagonControl.Place(Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer, unchecked((sbyte)call__13), unchecked((sbyte)call__12), unchecked((sbyte)call__11), 0);
                }
            }
        }

        // AoWHex.pas:109
        public override void NeighbourTerrainChanged(int HN, in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            if (HN == 0)
            {
                switch (Info.HN)
                {
                    case 6:
                        UpdateTransition(0);
                        break;
                    case 1:
                        UpdateTransition(1);
                        break;
                    case 2:
                        UpdateTransition(2);
                        break;
                }
            }
        }

        // AoWHex.pas:150
        public override void UpdateTransition(byte Side)
        {
            bool Shifted = default;
            int NXhx = default;
            int NYhx = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int NeighbourTerrain = default;
            int OwnTerrain = default;
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Image = default;
            Shifted = false;
            FTransitions[Side] = 255;
            FTransitionSides = unchecked((byte)(FTransitionSides & Aow.Units.AoWHex.AoWHex.SideClear[Side]));
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.HNtoHX(Aow.Units.AoWHex.AoWHex.SideHN[Side], ref NXhx, ref NYhx);
            Aow.Units.HSEngine.HSEngine.TranslateHX(0, 0, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, NXhx, NYhx, ref NXhx, ref NYhx);
            if ((((NXhx >= 0) && (NYhx >= 0)) && (MapLevel.Width > NXhx)) && (MapLevel.Height > NYhx))
            {
                var call__14 = MapLevel.GetField(NXhx, NYhx);
                NeighbourTerrain = call__14.TerrainType;
                OwnTerrain = GetTerrain(0);
                if (((((((OwnTerrain != NeighbourTerrain) && (OwnTerrain >= 0)) && (NeighbourTerrain >= 0)) && (NeighbourTerrain != 8)) && (NeighbourTerrain != 7)) && (NeighbourTerrain != 15)) && (OwnTerrain != 15))
                {
                    if (OwnTerrain == 9)
                    {
                        if ((NeighbourTerrain == 10) || (NeighbourTerrain == 13))
                        {
                            var call__15 = Aow.Rtl.Rtl.Random(3);
                            FTransitions[Side] = unchecked((byte)((call__15 * 3) + (Side + 182)));
                        }
                        else
                        {
                            if ((NeighbourTerrain != 8) && (NeighbourTerrain != 7))
                            {
                                FTransitions[Side] = unchecked((byte)(Side + 147));
                                Shifted = true;
                            }
                        }
                    }
                    else
                    {
                        if (NeighbourTerrain == 9)
                        {
                            if ((OwnTerrain == 10) || (OwnTerrain == 13))
                            {
                                var call__16 = Aow.Rtl.Rtl.Random(3);
                                FTransitions[Side] = unchecked((byte)((call__16 * 3) + (Side + 182)));
                            }
                            else
                            {
                                if ((OwnTerrain != 8) && (OwnTerrain != 7))
                                {
                                    FTransitions[Side] = unchecked((byte)(Side + 144));
                                }
                            }
                        }
                        else
                        {
                            if (OwnTerrain == 13)
                            {
                                if (NeighbourTerrain == 10)
                                {
                                    FTransitions[Side] = unchecked((byte)(Side + 208));
                                }
                                else
                                {
                                    FTransitions[Side] = unchecked((byte)(Side + 217));
                                    Shifted = true;
                                }
                            }
                            else
                            {
                                if (NeighbourTerrain == 13)
                                {
                                    if (OwnTerrain == 10)
                                    {
                                        FTransitions[Side] = unchecked((byte)(Side + 211));
                                        Shifted = true;
                                    }
                                    else
                                    {
                                        FTransitions[Side] = unchecked((byte)(Side + 214));
                                    }
                                }
                                else
                                {
                                    if (OwnTerrain == 10)
                                    {
                                        FTransitions[Side] = unchecked((byte)(Side + 163));
                                        Shifted = true;
                                    }
                                    else
                                    {
                                        if (NeighbourTerrain == 10)
                                        {
                                            FTransitions[Side] = unchecked((byte)(Side + 160));
                                        }
                                        else
                                        {
                                            if ((NeighbourTerrain != 0) && (NeighbourTerrain != 6))
                                            {
                                                if (OwnTerrain == 6)
                                                {
                                                    // { Original unreachable branch retained: the outer condition excludes terrain 0. }
                                                    if (NeighbourTerrain == 0)
                                                    {
                                                        FTransitions[Side] = unchecked((byte)(Side + 96));
                                                    }
                                                    else
                                                    {
                                                        FTransitions[Side] = unchecked((byte)((unchecked((byte)(NeighbourTerrain << 4)) + 3) + Side));
                                                        Shifted = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (OwnTerrain == 0)
                                                    {
                                                        FTransitions[Side] = unchecked((byte)(Side + 3));
                                                        Shifted = true;
                                                    }
                                                    else
                                                    {
                                                        var call__17 = Aow.Rtl.Rtl.Random(3);
                                                        FTransitions[Side] = unchecked((byte)((call__17 * 3) + ((unchecked((byte)(OwnTerrain << 4)) + Side) + 6)));
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (FTransitions[Side] != 255)
            {
                IL = ((Aow.Units.AoWHex.TAoWHexagonResource)FResource).FTransitionLibrary;
                Image = FTransitions[Side];
                if (Image < IL.Count)
                {
                    var call__18 = IL[Image];
                    if (call__18 != null)
                    {
                        FTransitionSides = (byte)(((int)FTransitionSides) + (1 << Side));
                        if (Shifted)
                        {
                            FTransitionSides = (byte)(((int)FTransitionSides) + (1 << (Side + 3)));
                        }
                    }
                }
            }
        }
    }

    // { TAoWHexagonResource (instance size 56, VMT 5579A364) }
    public partial class TAoWHexagonResource : Aow.Units.Hexagon.THexagonResource
    {
        // AoWHex.pas:254
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            if (Msg.MsgID == 65556)
            {
                var call__19 = FTerrainList.Terrain[0];
                if (TSet<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)call__19))
                {
                    return;
                }
            }

            base.MsgProc(ref Msg, Sender);
        }
    }

    // { TAoWBorderHexagon (instance size 16, VMT 5579A458) }
    public partial class TAoWBorderHexagon : Aow.Units.BorderHx.TBorderHexagon
    {
        // AoWHex.pas:261
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            if (((Info.HN == 0) && (Aow.Units.AoWE.AoWE.AoWHSMap != null)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 0))
            {
                ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)((System.Object)Info.MapField)).FFieldFlags & -4096;
            }
        }
    }

    // { TAoWWaterHexagon (instance size 32, VMT 5579A5F8): +$18 FAnimationFrame,
//     +$19 FAnimationIndex, +$1A..$1F FShoreTerrain[1..6]. }
    public partial class TAoWWaterHexagon : Aow.Units.IsoHex.TLowerIsometricHexagon
    {
        public sbyte FAnimationFrame;
        public byte FAnimationIndex;
        public Aow.Units.FixedArray_byte_6 FShoreTerrain;
        // AoWHex.pas:268
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__20 = Aow.Rtl.Rtl.Random(20);
            FAnimationFrame = unchecked((sbyte)(-1 - call__20));
            var call__21 = Aow.Rtl.Rtl.Random(4);
            this.FSides = unchecked((byte)(this.FSides | (call__21 << 6)));
        }

        // AoWHex.pas:315
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            var call__22 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__22.ImageLibrary;
            var call__23 = IL[((((((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(Owner)) >> 3)) + ((Aow.Units.HSEngine.TMapField)Owner).Xhx) + (((Aow.Units.HSEngine.TMapField)Owner).Yhx * 2)) + ((int)(((uint)FSides) >> 6))) & 15) + 140];
            call__23.Show(Xhp, Yhp, SurfaceDesc);
            Xhp -= 6;
            Yhp -= 9;
            if ((FSides & 1) != 0)
            {
                var call__24 = IL[36];
                call__24.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 2) != 0)
            {
                var call__25 = IL[37];
                call__25.Show(Xhp, Yhp, SurfaceDesc);
            }

            if ((FSides & 32) != 0)
            {
                var call__26 = IL[35];
                call__26.Show(Xhp, Yhp, SurfaceDesc);
            }
        }

        // AoWHex.pas:278
        public override void UpdateTransitions()
        {
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.TMapField MapField = default;
            Aow.Units.HSEngine.THexagonSprite HS = default;
            FOpenSides = 0;
            FSides = unchecked((byte)(FSides & 192));
            Level = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, ((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, 1);
            while (Walk.HN <= 6)
            {
                if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__27 = Level.GetField(Walk.X, Walk.Y);
                    MapField = call__27;
                    HS = MapField.FindOwnedHS(typeof(Aow.Units.IsoHex.TLowerIsometricHexagon));
                    if (HS == null)
                    {
                        HS = MapField.FindOwnedHS(typeof(Aow.Units.BorderHx.TBorderHexagon));
                        if (HS == null)
                        {
                            FOpenSides = unchecked((byte)(FOpenSides | (1 << (Walk.HN - 1))));
                            if (MapField.TerrainType == 3)
                            {
                                FShoreTerrain[Walk.HN - 1] = 40;
                            }
                            else
                            {
                                if (MapField.TerrainType == 5)
                                {
                                    FShoreTerrain[Walk.HN - 1] = 80;
                                }
                                else
                                {
                                    FShoreTerrain[Walk.HN - 1] = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        var call__28 = GetTerrain(0);
                        if (call__28 != MapField.TerrainType)
                        {
                            FSides = unchecked((byte)(FSides | (1 << (Walk.HN - 1))));
                        }
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            if (FDynamic == null)
            {
                var call__29 = ClassType.DynamicIsometricHexagonClass();
                var call__30 = call__29.Create((Aow.Units.Engine.TECustomNode)((System.Object)this));
                FDynamic = call__30;
            }
        }

        // AoWHex.pas:333
        public override void ShowDynamic(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int X = default;
            int Y = default;
            Aow.Units.ImageLib.TCustomImageLibrary IL = default;
            int Variant = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            int Image = default;
            bool Showing = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(((Aow.Units.HSEngine.TMapField)Owner).Xhx, ((Aow.Units.HSEngine.TMapField)Owner).Yhx, ref Xhp, ref Yhp);
            Xhp = (Xhp - XhpSurf) - 8;
            Yhp = Yhp - YhpSurf;
            // { Original uses the shared engine settings and the Flood enchantment. }
            if ((Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0) && ((Ptr<byte>.Unbox(Owner)[26] != 0) || !((Aow.Units.AoWE.TAbstractAoWHSMap)((Aow.Units.HSEngine.TMapLevel)Owner.Owner).Map).FFogEnabled))
            {
                FAnimationFrame = (sbyte)(((int)FAnimationFrame) + 1);
                if (unchecked((short)FAnimationFrame) >= 0)
                {
                    Showing = false;
                    if (unchecked((short)FAnimationFrame) == 0)
                    {
                        // { Original tests the Flood enchantment. }
                        if (Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.FActiveEnchantments.Contains(Aow.Units.AoWE.TGlobalEnchantmentType.getFlood))
                        {
                            bool bool__31 = (FOpenSides == 0);
                            if (bool__31)
                            {
                                var call__32 = Aow.Rtl.Rtl.Random(300);
                                bool__31 = (call__32 == 0);
                            }

                            if (bool__31 && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                            {
                                var call__33 = Aow.Rtl.Rtl.Random(10);
                                FAnimationIndex = unchecked((byte)(call__33 + 30));
                            }
                            else
                            {
                                if (Aow.Rtl.Rtl.Random(10) < 3)
                                {
                                    var call__34 = Aow.Rtl.Rtl.Random(9);
                                    FAnimationIndex = unchecked((byte)(call__34 + 50));
                                }
                                else
                                {
                                    var call__35 = Aow.Rtl.Rtl.Random(10);
                                    FAnimationIndex = unchecked((byte)(call__35 + 20));
                                }
                            }
                        }
                        else
                        {
                            bool bool__36 = (FOpenSides == 0);
                            if (bool__36)
                            {
                                var call__37 = Aow.Rtl.Rtl.Random(700);
                                bool__36 = (call__37 == 0);
                            }

                            if (bool__36 && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                            {
                                var call__38 = Aow.Rtl.Rtl.Random(10);
                                FAnimationIndex = unchecked((byte)(call__38 + 30));
                            }
                            else
                            {
                                var call__39 = Aow.Rtl.Rtl.Random(10);
                                FAnimationIndex = unchecked((byte)(call__39 + 20));
                            }
                        }
                    }

                    if (FAnimationIndex < FResource.ImageSequenceList.Count)
                    {
                        Sequence = (Aow.Units.ImageLib.TImageSequence)FResource.ImageSequenceList.List[FAnimationIndex];
                        while (Sequence != null)
                        {
                            var call__40 = Sequence.GetUnloopedFrameIndex(FAnimationFrame);
                            Image = call__40;
                            if (Image != -1)
                            {
                                Showing = true;
                                var call__41 = Sequence.ImageLibrary.ImageExist(Image);
                                if (call__41)
                                {
                                    var call__42 = Sequence.ImageLibrary[Image];
                                    call__42.Show(Sequence.dXshift + Xhp, Sequence.dYshift + Yhp, SurfaceDesc);
                                }
                            }

                            Sequence = Sequence.Next;
                        }
                    }

                    if (!Showing)
                    {
                        if (Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail == 1)
                        {
                            var call__43 = Aow.Rtl.Rtl.Random(25);
                            FAnimationFrame = unchecked((sbyte)(-20 - call__43));
                        }
                        else
                        {
                            var call__44 = Aow.Rtl.Rtl.Random(25);
                            FAnimationFrame = unchecked((sbyte)(-10 - call__44));
                        }
                    }
                }
            }

            base.ShowDynamic(XhpSurf, YhpSurf, SurfaceDesc);
            var call__45 = FResource.ImageSequenceList.ImageSequences[0];
            IL = call__45.ImageLibrary;
            Variant = ((int)(((uint)Aow.Rtl.Rtl.ObjectToInt(this)) >> 8)) & 3;
            if (Variant == 3)
            {
                Variant = 0;
            }

            if (FOpenSides != 0)
            {
                X = Xhp - 19;
                Y = Yhp - 32;
                if ((FOpenSides & 1) != 0)
                {
                    if ((FOpenSides & 35) == 35)
                    {
                        var call__46 = IL[FShoreTerrain[1 - 1] + 6];
                        call__46.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 35) == 1)
                        {
                            var call__47 = IL[FShoreTerrain[1 - 1] + 13];
                            call__47.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 7) == 7)
                    {
                        var call__48 = IL[FShoreTerrain[2 - 1] + 25];
                        call__48.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 3) == 3)
                        {
                            var call__49 = IL[(Variant + 3) + FShoreTerrain[2 - 1]];
                            call__49.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 49) == 49)
                    {
                        var call__50 = IL[FShoreTerrain[6 - 1] + 24];
                        call__50.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 33) == 33)
                        {
                            var call__51 = IL[Variant + FShoreTerrain[6 - 1]];
                            call__51.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
                else
                {
                    if ((FOpenSides & 6) == 6)
                    {
                        var call__52 = IL[(Variant + 10) + FShoreTerrain[3 - 1]];
                        call__52.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 2) == 2)
                        {
                            var call__53 = IL[FShoreTerrain[2 - 1] + 17];
                            call__53.Show(X, Y, SurfaceDesc);
                        }
                    }

                    if ((FOpenSides & 48) == 48)
                    {
                        var call__54 = IL[(Variant + 7) + FShoreTerrain[5 - 1]];
                        call__54.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 32) == 32)
                        {
                            var call__55 = IL[FShoreTerrain[6 - 1] + 16];
                            call__55.Show(X, Y, SurfaceDesc);
                        }
                    }
                }

                if ((FOpenSides & 8) != 0)
                {
                    if ((FOpenSides & 14) == 14)
                    {
                        var call__56 = IL[FShoreTerrain[3 - 1] + 22];
                        call__56.Show(X, Y + 1, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 12) == 12)
                        {
                            if (Variant == 2)
                            {
                                var call__57 = IL[FShoreTerrain[3 - 1] + 19];
                                call__57.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__58 = IL[(Variant + 30) + FShoreTerrain[3 - 1]];
                                call__58.Show(X, Y + 1, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 56) == 56)
                    {
                        var call__59 = IL[FShoreTerrain[5 - 1] + 21];
                        call__59.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 24) == 24)
                        {
                            if (Variant == 2)
                            {
                                var call__60 = IL[FShoreTerrain[5 - 1] + 18];
                                call__60.Show(X, Y + 1, SurfaceDesc);
                            }
                            else
                            {
                                var call__61 = IL[(Variant + 28) + FShoreTerrain[5 - 1]];
                                call__61.Show(X, Y, SurfaceDesc);
                            }
                        }
                    }

                    if ((FOpenSides & 28) == 8)
                    {
                        var call__62 = IL[FShoreTerrain[4 - 1] + 23];
                        call__62.Show(X, Y, SurfaceDesc);
                    }
                }
                else
                {
                    if ((FOpenSides & 14) == 4)
                    {
                        var call__63 = IL[FShoreTerrain[3 - 1] + 15];
                        call__63.Show(X, Y, SurfaceDesc);
                    }
                    else
                    {
                        if ((FOpenSides & 56) == 16)
                        {
                            var call__64 = IL[FShoreTerrain[5 - 1] + 14];
                            call__64.Show(X, Y, SurfaceDesc);
                        }
                    }
                }
            }
        }

        // AoWHex.pas:48
        public static new Aow.Units.AoWHex.TAoWWaterHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWHex.TAoWWaterHexagon o = new Aow.Units.AoWHex.TAoWWaterHexagon();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.IsoHex.TLowerIsometricHexagon.Meta
        {
            // AoWHex.pas:275
            public override int ClassID()
            {
                int Result = default;
                Result = 132925;
                return Result;
            }

            // AoWHex.pas:48
            public new Aow.Units.AoWHex.TAoWWaterHexagon Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWHex.TAoWWaterHexagon o = (Aow.Units.AoWHex.TAoWWaterHexagon)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWHex.TAoWWaterHexagon => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TAoWWaterHexagonResource (instance size 56, VMT 5579A7A0) }
    public partial class TAoWWaterHexagonResource : Aow.Units.IsoHex.TLowerIsometricHexagonResource
    {
        // AoWHex.pas:446
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            this.FDynamicLevel = 23;
        }

        // AoWHex.pas:458
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132925;
            return Result;
        }

        // AoWHex.pas:58
        public static new Aow.Units.AoWHex.TAoWWaterHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.AoWHex.TAoWWaterHexagonResource o = new Aow.Units.AoWHex.TAoWWaterHexagonResource();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }

        public new partial class Meta : Aow.Units.IsoHex.TLowerIsometricHexagonResource.Meta
        {
            // AoWHex.pas:452
            public override int ClassID()
            {
                int Result = default;
                Result = 132926;
                return Result;
            }

            // AoWHex.pas:455
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                /* TODO(T-4.3) Address needs a cell or array backing */
                throw new System.NotImplementedException();
                return Result;
            }

            // AoWHex.pas:58
            public new Aow.Units.AoWHex.TAoWWaterHexagonResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.AoWHex.TAoWWaterHexagonResource o = (Aow.Units.AoWHex.TAoWWaterHexagonResource)NewInstance();
                try
                {
                    o.Create__init(AOwner);
                }
                catch
                {
                    o.Destroy();
                    throw;
                }

                return o;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.AoWHex.TAoWWaterHexagonResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    [StructLayout(LayoutKind.Explicit, Size = 28)]
    public struct TAoWFieldFlags
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_24 Reserved;
        [FieldOffset(24)]
        public int Flags;
    }

    // { needs: IsoHex private FDynamic +$10, FOpenSides +$14, FSides +$15,
//     and resource FDynamicLevel +$34. Preserve the original storage locally. }
    public struct TIsoFields
    {
        public Aow.Units.FixedArray_byte_16 Reserved;
        public Aow.Units.IsoHex.TDynamicIsometricHexagon Dynamic;
        public byte OpenSides;
        public byte Sides;
    }

    [StructLayout(LayoutKind.Explicit, Size = 53)]
    public struct TIsoResourceFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_52 Reserved;
        [FieldOffset(52)]
        public byte DynamicLevel;
    }

    [StructLayout(LayoutKind.Explicit, Size = 17)]
    public struct TWaterViewFlags
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_16 Reserved;
        [FieldOffset(16)]
        public byte Flags;
    }

    // { needs: Hexagon.THexagon transition fields (+$10..$13) and
//     THexagonResource.FTransitionLibrary (+$34) are private in the recovered unit. }
    [StructLayout(LayoutKind.Explicit, Size = 20)]
    public struct THexagonFields
    {
        [FieldOffset(0)]
        public Aow.Units.FixedArray_byte_16 Reserved;
        [FieldOffset(16)]
        public Aow.Units.FixedArray_byte_3 Transitions;
        [FieldOffset(19)]
        public byte Sides;
    }

    public struct THexagonResourceFields
    {
        public Aow.Units.FixedArray_byte_52 Reserved;
        public Aow.Units.ImageLib.TCustomImageLibrary TransitionLibrary;
    }

    public static partial class AoWHex
    {
        // AoWHex.pas:461
        public static void RegisterAoWHexagon(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWHex.TAoWHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWHex.TAoWHexagonResource>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWHex.TAoWBorderHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.BorderHx.TBorderHexagonResource>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWHex.TAoWWaterHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.AoWHex.TAoWWaterHexagonResource>()]);
            AEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TUpperIsometricHexagon>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.IsoHex.TUpperIsometricHexagonResource>()]);
        }

        public static Aow.Units.FixedArray_byte_3 SideHN = Aow.Units.FixedArray_byte_3.From([6, 1, 2]);
        public static Aow.Units.FixedArray_byte_3 SideClear = Aow.Units.FixedArray_byte_3.From([246, 237, 219]);
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
