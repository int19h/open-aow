// Translated from ILTer.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.World.Terrain;

// { The ILTer unit of HSEPack.dpl: image library terrain, multi-hexagon objects that show one image
//   per terrain type (TILTerrainMO, ClassID $20186; resource TILTerrainMOR, $20187), and the fixed
//   variant that keeps its terrain type and updates the map terrain instead
//   (TFixedILTerrainMO, $20188; TFixedILTerrainMOR, $20189). }
// FImageIndex, FTerrainType
public partial class ImageLibraryTerrainMapObject : MultiHexMapObject
{

    public ImageLibraryTerrainMapObject(ECustomNode owner) : base(owner)
    {
        FTerrainType = -1;
    }

    // ILTer.pas:79
    public new const int ClassID = 131462;

    protected internal int FImageIndex;
    protected internal sbyte FTerrainType;

    // ILTer.pas:164
    public override sbyte GetTerrain(int Index)
    {
        sbyte Result = FResource.TerrainList.GetTerrain(Index);
        if (Result >= 0)
        {
            Result = FTerrainType;
        }

        return Result;
    }

    // ILTer.pas:131
    public override bool CanChangeTerrain(in HexSpriteSenderInfo Info)
    {
        bool Result;
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            Result = GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, FXhx, FYhx, FTerrainType) == Info.MapField.TerrainType;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // ILTer.pas:152
    public override void ChangeTerrain(in HexSpriteTerrainInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
            }
            else
            {
                Dispose();
            }
        }
    }

    // ILTer.pas:140
    public override void TerrainChanged(in HexSpriteTerrainInfo Info)
    {
        if (FResource.TerrainList.GetTerrain(Info.HN) >= 0)
        {
            if (ValidTerrainType(Info.TerrainType))
            {
                FTerrainType = Info.TerrainType;
                FImageIndex = GetTerrainTypeImage(Info.TerrainType);
            }
            else
            {
                Dispose();
            }
        }
    }

    // ILTer.pas:170
    public override void Show(int XhpSurf, int YhpSurf, in LockedSurface SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        HexGeometry.HXtoHP(FXhx, FYhx, ref Xhp, ref Yhp);
        if (this.FResource.ImageSequenceList == null)
        {
            this.FResource.ImageLibrary[FImageIndex].Show(Xhp - XhpSurf + ((MultiHexMapObjectResource)this.FResource).dXImage, Yhp - YhpSurf + ((MultiHexMapObjectResource)this.FResource).dYImage, SurfaceDesc);
        }
        else
        {
            this.FResource.ShowComplexImage(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc, FImageIndex, (uint)(Xhp + Yhp));
        }
    }

    // ILTer.pas:184
    public override bool PlaceOnMap()
    {
        bool Result = base.PlaceOnMap();
        if (Result)
        {
            FTerrainType = GetValidTerrainType((MapLevel)Owner.Owner, FXhx, FYhx);
        }

        return Result;
    }

    // ILTer.pas:191
    public override bool Loaded() => base.PlaceOnMap();

    // ILTer.pas:255
    public override void Activate()
    {
        base.Activate();
        FTerrainType = GetValidTerrainType((MapLevel)Owner.Owner, FXhx, FYhx);
        FImageIndex = GetTerrainTypeImage(FTerrainType);
    }

    // ILTer.pas:196
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool CanPlaceHere;
        int Count;
        sbyte TerrainType;
        HexNumberWalkData Walk = default;
        sbyte Overlay;
        HexSprite HS;
        TerrainList TerrainList;
        CanPlaceHere = true;
        Count = FResource.TerrainList.Count;
        FTerrainType = GetValidTerrainType(MapContainer.GetMapLevel(Lhx), Xhx, Yhx);
        HexNumberWalkData.InitHNWalk(ref Walk, Xhx, Yhx, 0);
        while (CanPlaceHere && Walk.HN < Count)
        {
            TerrainType = GetTerrain(Walk.HN);
            Overlay = GetOverlay(Walk.HN);
            if ((TerrainType >= 0 || Overlay >= 0) && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapContainer.Width && Walk.Y < MapContainer.Height)
            {
                HS = MapContainer.GetField(Walk.X, Walk.Y, Lhx).GetMaxLevelNoneTransparentHS();
                CanPlaceHere = HS == null || CanPlaceOnHS(HS);
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        if (CanPlaceHere)
        {
            TerrainList = new TerrainList();
            TerrainList.Assign(FResource.TerrainList);
            for (int I = 0, last = TerrainList.Count - 1; I <= last; I++)
            {
                if (TerrainList.GetTerrain(I) >= 0)
                {
                    TerrainList.SetTerrain(I, FTerrainType);
                }
            }

            CanPlaceHere = CanPlaceHere && MapContainer.Map.CanChangeTerrain(this, Xhx, Yhx, Lhx, TerrainList);
            TerrainList?.Dispose();
        }

        return CanPlaceHere;
    }

    // ILTer.pas:236
    public override bool PlaceHX(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result = base.PlaceHX(MapContainer, Xhx, Yhx, Lhx);
        if (Result)
        {
            Result = false;
            FTerrainType = GetValidTerrainType(MapContainer.GetMapLevel(Lhx), Xhx, Yhx);
            if (FTerrainType >= 0)
            {
                FImageIndex = GetTerrainTypeImage(FTerrainType);
                if (FImageIndex != -1)
                {
                    if (!GetEditMode())
                    {
                        OverrideTerrain();
                    }

                    Result = true;
                }
            }
        }

        return Result;
    }

    // ILTer.pas:89
    public virtual bool ValidTerrainType(int TerrainType)
    {
        bool Result;
        int Index;
        ImageSequenceList Sequences;
        if (TerrainType >= 0)
        {
            Index = GetTerrainTypeImage(TerrainType);
            Sequences = FResource.ImageSequenceList;
            if (Sequences == null)
            {
                Result = FResource.ImageLibrary.ImageExist(Index);
            }
            else
            {
                Result = Sequences.GetImageSequence(Index) != null;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // ILTer.pas:126
    public virtual int GetTerrainTypeImage(int TerrainType) => TerrainType;

    // ILTer.pas:84
    public virtual sbyte ForceTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx) => -2;

    // ILTer.pas:107
    public virtual sbyte GetValidTerrainType(MapLevel MapLevel, sbyte Xhx, sbyte Yhx)
    {
        int Count;
        sbyte Result = GetMostFrequentMapFieldTerrainType(MapLevel, Xhx, Yhx, -1);
        if (!ValidTerrainType(Result))
        {
            Result = ForceTerrainType(MapLevel, Xhx, Yhx);
            if (GetEditMode() || Result != -2)
            {
                if (!ValidTerrainType(Result))
                {
                    Result = 0;
                    Count = FResource.Engine.HSSet.TerrainTypes.Count;
                    while (Result < Count && !ValidTerrainType(Result))
                    {
                        Result = (sbyte)(Result + 1);
                    }

                    if (Result == Count)
                    {
                        Result = -1;
                    }
                }
            }
        }

        return Result;
    }

    public int ImageIndex { get => FImageIndex; set => FImageIndex = value; }
    public sbyte TerrainType { get => FTerrainType; }

    // ILTer.pas:20

}
