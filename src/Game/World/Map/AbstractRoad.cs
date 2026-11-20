// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.World.Map;

// the base of roads and bridges
public partial class AbstractRoad(CustomNode owner) : MapObject(owner)
{
    public new const int ClassId = 65906;

    // MAPOBJ.PAS:803
    public override byte GetLevel() => 10;

    // MAPOBJ.PAS:808
    public override int GetShowPriority()
    {
        int pixelX = (Owner.HexX * 32) + 8;
        int pixelY = ((Owner.HexX & 1) * 16) + (Owner.HexY * 32);
        return pixelX + (pixelY << 13) + 671088640;
    }

    // MAPOBJ.PAS:848
    public override void ChangeTerrain(in HexSpriteTerrainInfo message)
    {
        if (message.HexNumber == 0 && GetTerrain(0) != message.TerrainType)
        {
            int classId = Resource.EngineClass.ClassId;
            MapObjectResource r = Resource.GetTerrainControl().GetRandomResource(message.TerrainType, classId);
            if (r is not null)
            {
                SetResource(r);
                UpdateTransitions();
                sbyte hexLevel = GetHexLevel();
                sbyte hexY = GetHexY();
                sbyte hexX = GetHexX();
                Resource.Engine.HexSpriteMap.MapContainer.Map.InvalidateStaticScene(hexX, hexY, hexLevel);
            }
            else
            {
                sbyte hexLevel = GetHexLevel();
                sbyte hexY = GetHexY();
                sbyte hexX = GetHexX();
                if (Resource.GetTerrainControl().Place(Owner.Owner.Owner, hexX, hexY, hexLevel, message.TerrainType) is null)
                {
                    Dispose();
                }
            }
        }
    }

    // MAPOBJ.PAS:843
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo message) => UpdateTransitions();

    // MAPOBJ.PAS:823
    public override void Activate() => UpdateTransitions();

    // MAPOBJ.PAS:828
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool condition = hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < mapContainer.Width && hexY < mapContainer.Height && hexLevel < mapContainer.LevelCount;
        if (condition)
        {
            MapField fieldValue = mapContainer.GetField(hexX, hexY, hexLevel);
            sbyte terrain = Resource.TerrainList.GetTerrain(0);
            condition = fieldValue.TerrainType == terrain;
        }

        bool result;
        if (condition)
        {
            HexSprite hexSprite = mapContainer.GetField(hexX, hexY, hexLevel).GetMaxLevelNoneTransparentHexSprite();
            result = hexSprite is null || hexSprite.GetLevel() < 10 || hexSprite is AbstractRoad;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // MAPOBJ.PAS:819
    public virtual void UpdateTransitions()
    {
    }

}
