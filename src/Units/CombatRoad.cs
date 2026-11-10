// Translated from CombatRoad.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.World.Hexes;

public partial class CombatRoad : AbstractRoad
{
    public CombatRoad(ECustomNode owner) : base(owner)
    { }

    // CombatRoad.pas:51
    public new const int ClassID = 2229506;

    public FixedArray_byte_7 FImages;

    // main image, neighbour images
    // CombatRoad.pas:108
    public override void NeighbourTerrainChanged(int HN, in HSTerrainInfo Msg)
    {
        if (HN == 0)
        {
            UpdateTransition((byte)Msg.HN);
        }
    }

    // CombatRoad.pas:113
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        CustomImageLibrary Images;
        int X = default;
        int Y = default;
        Aow.Game.World.Map.HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref X, ref Y);
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
    public override Aow.Game.World.MapObjectResourceList GetResourceList(global::Aow.Game.Engine.Engine AEngine) => Aow.Game.Engine.AoWHSSet.Current.FCombatResources;

    // CombatRoad.pas:57
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
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
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        HexagonSprite OldRoad;
        MapField MapField;
        MapField = MapContainer.GetField(Xhx, Yhx, Lhx);
        OldRoad = MapField.FindHS(typeof(AbstractRoad));
        if (OldRoad != null)
        {
            OldRoad.Dispose();
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
        MapLevel MapLevel;
        sbyte Overlay;
        int X = default;
        int Y = default;
        MapLevel = (global::Aow.Game.World.Map.MapLevel)Owner.Owner;
        Aow.Game.World.Map.HexGeometry.CenterHNtoHX(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, HN, ref X, ref Y);
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

    public new partial class Resource : AbstractRoadResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CombatRoad.pas:133
        public new const int ClassID = 2229507;

        // CombatRoad.pas:144
        public override void MsgProc(ref EMsg Msg, EObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 65621)
            {
                Msg.Result = 5;
            }
        }

        // CombatRoad.pas:153
        public override Aow.Game.Draw.Images.LibraryImage GetCellImage()
        {
            Aow.Game.Draw.Images.LibraryImage Result = null;
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
        public override RoadControl GetRoadControl() => ((AoWHSSet)Engine.HSSet).FCombatRoads;

        // CombatRoad.pas:136
        public override string ResourceEditName() => "Combat Road";

    }

}
