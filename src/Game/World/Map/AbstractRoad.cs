// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.World.Map;

// the base of roads and bridges
public partial class AbstractRoad : MapObject
{
    public new const int ClassID = 65906;

    public AbstractRoad(ECustomNode owner) : base(owner)
    { }

    // MAPOBJ.PAS:803
    public override byte GetLevel() => 10;

    // MAPOBJ.PAS:808
    public override int GetShowPriority()
    {
        int Xhp;
        int Yhp;
        Xhp = ((MapField)Owner).FXhx * 32 + 8;
        Yhp = (((MapField)Owner).FXhx & 1) * 16 + ((MapField)Owner).FYhx * 32;
        return Xhp + ((Yhp << 13) + 671088640);
    }

    // MAPOBJ.PAS:848
    public override void ChangeTerrain(in HSTerrainInfo Msg)
    {
        MapObjectResource R;
        if (Msg.HN == 0 && GetTerrain(0) != Msg.TerrainType)
        {
            var classID = FResource.EClass.ClassID;
            R = ((AbstractRoadResource)FResource).GetRoadControl().GetRndResource(Msg.TerrainType, classID);
            if (R != null)
            {
                SetResource(R);
                UpdateTransitions();
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                FResource.FEngine.FHSMap.FMapContainer.FMap.InvalidateStaticScene(xhx, yhx, lhx);
            }
            else
            {
                var lhx = GetLhx();
                var yhx = GetYhx();
                var xhx = GetXhx();
                if (((AbstractRoadResource)FResource).GetRoadControl().Place((MapContainer)Owner.Owner.Owner, xhx, yhx, lhx, Msg.TerrainType) == null)
                {
                    Dispose();
                }
            }
        }
    }

    // MAPOBJ.PAS:843
    public override void NeighbourTerrainChanged(int HN, in HSTerrainInfo Msg)
    {
        UpdateTransitions();
    }

    // MAPOBJ.PAS:823
    public override void Activate()
    {
        UpdateTransitions();
    }

    // MAPOBJ.PAS:828
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        bool Result;
        HexagonSprite HS;
        bool condition = Xhx >= 0 && Yhx >= 0 && Lhx >= 0 && Xhx < MapContainer.FWidth && Yhx < MapContainer.FHeight && Lhx < MapContainer.FLevelCount;
        if (condition)
        {
            var fieldValue = MapContainer.GetField(Xhx, Yhx, Lhx);
            var terrain = FResource.FTerrainList.GetTerrain(0);
            condition = fieldValue.FTerrainType == terrain;
        }

        if (condition)
        {
            HS = MapContainer.GetField(Xhx, Yhx, Lhx).GetMaxLevelNoneTransparentHS();
            Result = HS == null || HS.GetLevel() < 10 || HS is AbstractRoad;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // MAPOBJ.PAS:819
    public virtual void UpdateTransitions()
    {
    }

}
