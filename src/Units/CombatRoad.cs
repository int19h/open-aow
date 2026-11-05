// Translated from CombatRoad.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.CombatRoad
{
    public partial class TCombatRoad : TAbstractRoad
    {
        public TCombatRoad(TECustomNode owner) : base(owner)
        {}

        // CombatRoad.pas:51
        public new const int ClassID = 2229506;

        public FixedArray_byte_7 FImages;
        // main image, neighbour images
        // CombatRoad.pas:108
        public override void NeighbourTerrainChanged(int HN, in THSTerrainInfo Msg)
        {
            if (HN == 0)
            {
                UpdateTransition((byte)Msg.HN);
            }
        }

        // CombatRoad.pas:113
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            TCustomImageLibrary Images = default;
            int X = default;
            int Y = default;
            int I = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
            X = X - XhpSurf - 8;
            Y = Y - YhpSurf;
            var call__1 = FResource.ImageSequenceList.GetImageSequence(0);
            Images = call__1.ImageLibrary;
            var call__2 = Images.Get(FImages[0]);
            call__2.Show(X, Y, SurfaceDesc);
            {
                I = 1;
                byte end__3 = 6;
                for (; I <= end__3; I += 1)
                {
                    if (FImages[I] != 255)
                    {
                        var call__4 = Images.Get(FImages[I]);
                        call__4.Show(X - 8, Y - 16, SurfaceDesc);
                    }
                }
            }
        }

        // CombatRoad.pas:127
        public override void Activate()
        {
            base.Activate();
            var call__5 = Random(12);
            FImages[0] = (byte)(call__5 + 10);
        }

        // CombatRoad.pas:54
        public override TMapObjectResourceList GetResourceList(TEngine AEngine)
        {
            TMapObjectResourceList Result = default;
            Result = AoWHSSet.FCombatResources;
            return Result;
        }

        // CombatRoad.pas:57
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.Width && Yhx < MapContainer.Height && Lhx < MapContainer.LevelCount)
            {
                var call__6 = MapContainer.GetField(Xhx, Yhx, Lhx);
                bool bool__7 = call__6.OverlayType == 4;
                if (!bool__7)
                {
                    var call__8 = MapContainer.GetField(Xhx, Yhx, Lhx);
                    bool__7 = call__8.TerrainType == 15;
                }

                Result = !bool__7;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // CombatRoad.pas:66
        public override bool PlaceHX(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            THexagonSprite OldRoad = default;
            TMapField MapField = default;
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            OldRoad = MapField.FindHS(typeof(TAbstractRoad));
            if (OldRoad != null)
            {
                OldRoad.Destroy();
            }

            Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
            return Result;
        }

        // CombatRoad.pas:101
        public override void UpdateTransitions()
        {
            int I = default;
            {
                I = 1;
                byte end__9 = 6;
                for (; I <= end__9; I += 1)
                {
                    UpdateTransition((byte)I);
                }
            }
        }

        // CombatRoad.pas:77
        public void UpdateTransition(byte HN)
        {
            TMapLevel MapLevel = default;
            sbyte Overlay = default;
            int X = default;
            int Y = default;
            MapLevel = (TMapLevel)Owner.Owner;
            CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, HN, ref X, ref Y);
            if (X >= 0 && Y >= 0 && X < MapLevel.Width && Y < MapLevel.Height)
            {
                var call__10 = MapLevel.GetField(X, Y);
                Overlay = call__10.OverlayType;
                if (Overlay != 4)
                {
                    switch (HN)
                    {
                        case 1:
                            var call__11 = Random(2);
                            FImages[HN] = (byte)(call__11 * 3 + 1);
                            break;
                        case 2:
                            var call__12 = Random(2);
                            FImages[HN] = (byte)(call__12 * 3 + 2);
                            break;
                        case 3:
                            FImages[HN] = 6;
                            break;
                        case 4:
                            FImages[HN] = 7;
                            break;
                        case 5:
                            FImages[HN] = 8;
                            break;
                        case 6:
                            var call__13 = Random(2);
                            FImages[HN] = (byte)(call__13 * 3);
                            break;
                    }
                }
                else
                {
                    FImages[HN] = 255;
                }
            }
        }

    }

    public partial class TCombatRoadResource : TAbstractRoadResource
    {
        public TCombatRoadResource(TECustomNode owner) : base(owner)
        {}

        // CombatRoad.pas:133
        public new const int ClassID = 2229507;

        // CombatRoad.pas:144
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // CombatRoad.pas:153
        public override ImageLib.TLibraryImage GetCellImage()
        {
            ImageLib.TLibraryImage Result = default;
            Result = null;
            bool bool__14 = ImageSequenceList != null && ImageSequenceList.Count > 0;
            if (bool__14)
            {
                var call__15 = ImageSequenceList.GetImageSequence(0);
                bool__14 = call__15 != null;
            }

            if (bool__14)
            {
                var call__16 = ImageSequenceList.GetImageSequence(0);
                var call__17 = call__16.ImageLibrary.Get(10);
                Result = call__17;
            }

            if (Result == null)
            {
                Result = base.GetCellImage();
            }

            return Result;
        }

        // CombatRoad.pas:150
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 2229506;
            return Result;
        }

        // CombatRoad.pas:139
        public override TRoadControl GetRoadControl()
        {
            TRoadControl Result = default;
            Result = ((TAoWHSSet)Engine.HSSet).FCombatRoads;
            return Result;
        }

        // CombatRoad.pas:136
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Combat Road";
            return Result;
        }

    }

    public static partial class CombatRoad
    {
        // CombatRoad.pas:162
        public static void RegisterCombatRoad(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TCombatRoadResource>(), EClass.Of<TCombatRoad>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
