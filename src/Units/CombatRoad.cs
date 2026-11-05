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
            TCustomImageLibrary Images;
            int X = default;
            int Y = default;
            HXtoHP(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, ref X, ref Y);
            X = X - XhpSurf - 8;
            Y = Y - YhpSurf;
            Images = FResource.ImageSequenceList.GetImageSequence(0).ImageLibrary;
            Images.Get(FImages[0]).Show(X, Y, SurfaceDesc);
            for (int I = 1; I <= 6; I++)
            {
                if (FImages[I] != 255)
                {
                    Images.Get(FImages[I]).Show(X - 8, Y - 16, SurfaceDesc);
                }
            }
        }

        // CombatRoad.pas:127
        public override void Activate()
        {
            base.Activate();
            var random = Random(12);
            FImages[0] = (byte)(random + 10);
        }

        // CombatRoad.pas:54
        public override TMapObjectResourceList GetResourceList(TEngine AEngine) => AoWHSSet.FCombatResources;

        // CombatRoad.pas:57
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result;
            if (Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.Width && Yhx < MapContainer.Height && Lhx < MapContainer.LevelCount)
            {
                Result = !(MapContainer.GetField(Xhx, Yhx, Lhx).OverlayType == 4 || MapContainer.GetField(Xhx, Yhx, Lhx).TerrainType == 15);
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
            THexagonSprite OldRoad;
            TMapField MapField;
            MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
            OldRoad = MapField.FindHS(typeof(TAbstractRoad));
            if (OldRoad != null)
            {
                OldRoad.Destroy();
            }

            return base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        }

        // CombatRoad.pas:101
        public override void UpdateTransitions()
        {
            for (int I = 1; I <= 6; I++)
            {
                UpdateTransition((byte)I);
            }
        }

        // CombatRoad.pas:77
        public void UpdateTransition(byte HN)
        {
            TMapLevel MapLevel;
            sbyte Overlay;
            int X = default;
            int Y = default;
            MapLevel = (TMapLevel)Owner.Owner;
            CenterHNtoHX(((TMapField)Owner).Xhx, ((TMapField)Owner).Yhx, HN, ref X, ref Y);
            if (X >= 0 && Y >= 0 && X < MapLevel.Width && Y < MapLevel.Height)
            {
                Overlay = MapLevel.GetField(X, Y).OverlayType;
                if (Overlay != 4)
                {
                    switch (HN)
                    {
                        case 1:
                            var random = Random(2);
                            FImages[HN] = (byte)(random * 3 + 1);
                            break;
                        case 2:
                            var random2 = Random(2);
                            FImages[HN] = (byte)(random2 * 3 + 2);
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
                            var random3 = Random(2);
                            FImages[HN] = (byte)(random3 * 3);
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
            ImageLib.TLibraryImage Result = null;
            if (ImageSequenceList != null && ImageSequenceList.Count > 0 && ImageSequenceList.GetImageSequence(0) != null)
            {
                Result = ImageSequenceList.GetImageSequence(0).ImageLibrary.Get(10);
            }

            if (Result == null)
            {
                Result = base.GetCellImage();
            }

            return Result;
        }

        // CombatRoad.pas:150
        public override int ResourceUserClassID() => 2229506;

        // CombatRoad.pas:139
        public override TRoadControl GetRoadControl() => ((TAoWHSSet)Engine.HSSet).FCombatRoads;

        // CombatRoad.pas:136
        public override string ResourceEditName() => "Combat Road";

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
