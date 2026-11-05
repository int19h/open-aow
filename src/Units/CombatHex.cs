// Translated from CombatHex.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWHex;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;

namespace Aow.Units.CombatHex
{
    public partial class TCombatHexagon : TAbstractHexagon
    {
        public TCombatHexagon(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 2229504;

        public byte FImageVariant;
        public FixedArray_byte_3 FTransitions;
        // CombatHex.pas:174
        public override void TerrainChanged(in THSTerrainInfo Msg)
        {
            if (TSet<byte>.WithBounds(0, 255).With(0).With(6).With(14).Contains((byte)Msg.TerrainType))
            {
                if (Msg.TerrainType != 14)
                {
                    var call__1 = GetLhx();
                    var call__2 = GetYhx();
                    var call__3 = GetXhx();
                    ((TAoWHSSet)FResource.Engine.HSSet).FCombatHexagons.Place(AoWCombatMap.MapContainer, call__3, call__2, call__1, Msg.TerrainType);
                }
                else
                {
                    var call__4 = GetLhx();
                    var call__5 = GetYhx();
                    var call__6 = GetXhx();
                    ((TAoWHSSet)FResource.Engine.HSSet).FCombatHexagons.Place(AoWCombatMap.MapContainer, call__6, call__5, call__4, 0);
                }
            }
        }

        // CombatHex.pas:138
        public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Info)
        {
            if (HN == 0)
            {
                switch (Info.HN)
                {
                    case 6:
                        UpdateTransition(1);
                        break;
                    case 1:
                        UpdateTransition(2);
                        break;
                    case 2:
                        UpdateTransition(3);
                        break;
                }
            }
        }

        // CombatHex.pas:148
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            TCustomImageLibrary Images = default;
            int Side = default;
            if (((TMapField)Owner).OverlayType != 4)
            {
                HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref Xhp, ref Yhp);
                Xhp = Xhp - XhpSurf - 8;
                Yhp = Yhp - YhpSurf;
                var call__7 = FResource.ImageSequenceList.GetImageSequence(0);
                Images = call__7.ImageLibrary;
                var call__8 = Images[FImageVariant];
                call__8.Show(Xhp, Yhp, SurfaceDesc);
                Images = ((TCombatHexagonResource)FResource).FTransitionLibrary;
                {
                    Side = 1;
                    byte end__9 = 3;
                    for (; Side <= end__9; Side += 1)
                    {
                        if (FTransitions[Side - 1] != 255)
                        {
                            var call__10 = Images[FTransitions[Side - 1]];
                            call__10.Show(Xhp - 8, Yhp - 16, SurfaceDesc);
                        }
                    }
                }
            }
        }

        // CombatHex.pas:168
        public override void Activate()
        {
            base.Activate();
            var call__11 = Random(12);
            FImageVariant = (byte)call__11;
        }

        // CombatHex.pas:73
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatHex.pas:134
        public override void UpdateTransitions()
        {
            int Side = default;
            {
                Side = 1;
                byte end__12 = 3;
                for (; Side <= end__12; Side += 1)
                {
                    UpdateTransition((byte)Side);
                }
            }
        }

        // CombatHex.pas:76
        public void UpdateTransition(byte Side)
        {
            TMapLevel MapLevel = default;
            int ImageIndex = default;
            sbyte NeighbourTerrain = default;
            sbyte TerrainType = default;
            int X = default;
            int Y = default;
            MapLevel = (TMapLevel)Owner.Owner;
            CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, CombatHex.TransitionNeighbours[Side - 1], ref X, ref Y);
            if (X >= 0 && Y >= 0 && X < MapLevel.Width && Y < MapLevel.Height)
            {
                var call__13 = MapLevel.GetField(X, Y);
                NeighbourTerrain = call__13.TerrainType;
                var call__14 = FResource.TerrainList.GetTerrain(0);
                TerrainType = call__14;
                ImageIndex = -1;
                if (NeighbourTerrain != TerrainType && NeighbourTerrain != 7)
                {
                    if (TerrainType == 0)
                    {
                        if (NeighbourTerrain == 6)
                        {
                            ImageIndex = 10;
                        }
                        else
                        {
                            ImageIndex = NeighbourTerrain + 4;
                        }
                    }
                    else
                    {
                        if (TerrainType == 6)
                        {
                            if (NeighbourTerrain == 0)
                            {
                                ImageIndex = 16;
                            }
                            else
                            {
                                ImageIndex = NeighbourTerrain + 10;
                            }
                        }
                        else
                        {
                            if (TerrainType == 12)
                            {
                                ImageIndex = 17;
                            }
                            else
                            {
                                if (TerrainType == 10)
                                {
                                    if (NeighbourTerrain == 12)
                                    {
                                        ImageIndex = 18;
                                    }
                                    else
                                    {
                                        if (NeighbourTerrain == 13)
                                        {
                                            ImageIndex = 19;
                                        }
                                        else
                                        {
                                            if (NeighbourTerrain == 9)
                                            {
                                                ImageIndex = 20;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (TerrainType == 13)
                                    {
                                        if (NeighbourTerrain == 12)
                                        {
                                            ImageIndex = 21;
                                        }
                                        else
                                        {
                                            if (NeighbourTerrain == 10)
                                            {
                                                ImageIndex = 22;
                                            }
                                            else
                                            {
                                                if (NeighbourTerrain == 9)
                                                {
                                                    ImageIndex = 23;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (TerrainType == 9)
                                        {
                                            if (NeighbourTerrain == 12)
                                            {
                                                ImageIndex = 24;
                                            }
                                            else
                                            {
                                                ImageIndex = 25;
                                            }
                                        }
                                        else
                                        {
                                            ImageIndex = TerrainType - 1;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ImageIndex != -1)
                {
                    ImageIndex = Random(2) * 3 + ImageIndex * 6 + (Side - 1);
                    var call__15 = ((TCombatHexagonResource)FResource).FTransitionLibrary.GetSafe(ImageIndex);
                    if (call__15 != null)
                    {
                        FTransitions[Side - 1] = (byte)ImageIndex;
                    }
                    else
                    {
                        FTransitions[Side - 1] = 255;
                    }
                }
                else
                {
                    FTransitions[Side - 1] = 255;
                }
            }
        }

    }

    public partial class TCombatHexagonResource : TAbstractHexagonResource
    {
        public TCombatHexagonResource(TECustomNode owner) : base(owner)
        {}

        public new const int ClassID = 2229505;

        public TCustomImageLibrary FTransitionLibrary;
        // CombatHex.pas:194
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            bool bool__16 = Msg.MsgID != 65556;
            if (!bool__16)
            {
                var call__17 = TerrainList.GetTerrain(0);
                bool__16 = !TSet<byte>.WithBounds(0, 255).With(0).With(6).Contains((byte)call__17);
            }

            if (bool__16)
            {
                base.MsgProc(ref Msg, Sender);
            }

            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }

            if (Msg.MsgID == 536870920)
            {
                var call__18 = AoWHSSet.ImageLibraryManager.LinkToIL(TSet<TILManagerCategoryElement>.Of(TILManagerCategoryElement.icDisk), (AnsiString)"TCombat\\Base_Hex\\CTransHx.ILB");
                FTransitionLibrary = call__18;
            }
        }

        // CombatHex.pas:207
        public override ImageLib.TLibraryImage GetCellImage()
        {
            ImageLib.TLibraryImage Result = default;
            Result = null;
            bool bool__19 = ImageSequenceList != null && ImageSequenceList.Count > 0;
            if (bool__19)
            {
                var call__20 = ImageSequenceList.GetImageSequence(0);
                bool__19 = call__20 != null;
            }

            if (bool__19)
            {
                var call__21 = ImageSequenceList.GetImageSequence(0);
                var call__22 = call__21.ImageLibrary[10];
                Result = call__22;
            }

            if (Result == null)
            {
                Result = base.GetCellImage();
            }

            return Result;
        }

        // CombatHex.pas:204
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229504;
            return Result;
        }

        // CombatHex.pas:191
        public override THexagonControl GetHexagonControl()
        {
            THexagonControl Result = default;
            Result = ((TAoWHSSet)Engine.HSSet).FCombatHexagons;
            return Result;
        }

        // CombatHex.pas:188
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Hexagon";
            return Result;
        }

    }

    public partial class TWaterCombatHexagon : TAoWWaterHexagon
    {
        public TWaterCombatHexagon(TECustomNode owner) : base(owner)
        {}

        // CombatHex.pas:219
        public new const int ClassID = 2229655;

        // CombatHex.pas:216
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

    }

    public partial class TWaterCombatHexagonResource : TAoWWaterHexagonResource
    {
        public TWaterCombatHexagonResource(TECustomNode owner) : base(owner)
        {}

        // CombatHex.pas:222
        public new const int ClassID = 2229656;

        // CombatHex.pas:228
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229655;
            return Result;
        }

        // CombatHex.pas:231
        public override THexagonControl GetHexagonControl()
        {
            THexagonControl Result = default;
            Result = ((TAoWHSSet)Engine.HSSet).FCombatHexagons;
            return Result;
        }

        // CombatHex.pas:225
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Water/Ice hex";
            return Result;
        }

    }

    public static partial class CombatHex
    {
        // CombatHex.pas:234
        public static void RegisterCombatHexagon(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCombatHexagonResource>(), EClass.Of<TCombatHexagon>(), EClass.Of<TWaterCombatHexagon>(), EClass.Of<TWaterCombatHexagonResource>()]);
        }

        public static FixedArray_byte_3 TransitionNeighbours = FixedArray_byte_3.From([6, 1, 2]);
        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
