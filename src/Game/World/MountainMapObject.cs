// Translated from Mountain.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Hexes;
using Aow.Game.World.Map;

namespace Aow.Game.World;

public partial class MountainMapObject : Hexes.Terrain
{
    public MountainMapObject(ECustomNode owner) : base(owner)
    { }

    // Mountain.pas:101
    public new const int ClassID = 131432;

    // Mountain.pas:104
    public override sbyte GetOverlay(int Index)
    {
        sbyte Result = FResource.TerrainList.Overlay[Index];
        if (Result >= 0)
        {
            Result = 0;
        }

        return Result;
    }

    // Mountain.pas:110
    public override bool CanChangeTerrain(in HexSpriteSenderInfo Info) => true;

    // Mountain.pas:136
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        int I;
        bool Valid;
        TerrainList List;
        MapField MapField;
        int X = default;
        int Y = default;
        HexSprite HS;
        I = 0;
        Valid = true;
        List = FResource.TerrainList;
        while (Valid && I < List.Count)
        {
            if (List.Terrain[I] >= 0 || List.Overlay[I] >= 0)
            {
                HexGeometry.CenterHNtoHX(Xhx, Yhx, I, ref X, ref Y);
                if (X >= 0 && Y >= 0 && X < MapContainer.Width && Y < MapContainer.Height)
                {
                    MapField = MapContainer.GetField(X, Y, Lhx);
                    HS = MapField.GetMaxLevelNoneTransparentHS();
                    if (HS != null && HS.EClass.ClassID == 131432)
                    {
                        Valid = ValidMountainOverlap(MapField, List.Overlay[I]);
                    }
                    else
                    {
                        Valid = HS == null || HS.GetLevel() < 10;
                    }
                }
            }

            I += 1;
        }

        return Valid && MapContainer.Map.CanChangeTerrain(this, Xhx, Yhx, Lhx, FResource.TerrainList);
    }

    // Mountain.pas:113
    public bool ValidMountainOverlap(MapField MapField, sbyte Overlay)
    {
        bool Valid;
        int I;
        MapObject HS;
        sbyte OtherOverlay;
        Valid = true;
        I = 0;
        while (I != MapField.Count && Valid)
        {
            HS = (MapObject)MapField.HSList[I];
            if (HS.EClass.ClassID == 131432)
            {
                var hNofHS = MapField.GetHNofHS(HS);
                OtherOverlay = HS.Resource.TerrainList.Overlay[hNofHS];
                Valid = !((OtherOverlay == 0 && Overlay == 0) || (OtherOverlay == 1 && Overlay == 1));
            }

            I += 1;
        }

        return Valid;
    }

}
