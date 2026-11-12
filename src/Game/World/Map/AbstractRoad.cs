// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.World.Map;

// the base of roads and bridges
public partial class AbstractRoad : MapObject
{
    public new const int ClassId = 65906;

    public AbstractRoad(ECustomNode owner) : base(owner)
    { }

    // MAPOBJ.PAS:803
    public override byte GetLevel() => 10;

    // MAPOBJ.PAS:808
    public override int GetShowPriority()
    {
        int pixelX;
        int pixelY;
        pixelX = ((MapField)Owner).HexX * 32 + 8;
        pixelY = (((MapField)Owner).HexX & 1) * 16 + ((MapField)Owner).HexY * 32;
        return pixelX + ((pixelY << 13) + 671088640);
    }

    // MAPOBJ.PAS:848
    public override void ChangeTerrain(in HexSpriteTerrainInfo message)
    {
        MapObjectResource R;
        if (message.HexNumber == 0 && GetTerrain(0) != message.TerrainType)
        {
            var classId = _resource.EngineClass.ClassId;
            R = ((AbstractRoadResource)_resource).GetRoadControl().GetRandomResource(message.TerrainType, classId);
            if (R != null)
            {
                SetResource(R);
                UpdateTransitions();
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                _resource.Engine.HexSpriteMap.MapContainer.Map.InvalidateStaticScene(hexX, hexY, hexLevel);
            }
            else
            {
                var hexLevel = GetHexLevel();
                var hexY = GetHexY();
                var hexX = GetHexX();
                if (((AbstractRoadResource)_resource).GetRoadControl().Place((MapContainer)Owner.Owner.Owner, hexX, hexY, hexLevel, message.TerrainType) == null)
                {
                    Dispose();
                }
            }
        }
    }

    // MAPOBJ.PAS:843
    public override void NeighbourTerrainChanged(int hexNumber, in HexSpriteTerrainInfo message)
    {
        UpdateTransitions();
    }

    // MAPOBJ.PAS:823
    public override void Activate()
    {
        UpdateTransitions();
    }

    // MAPOBJ.PAS:828
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        HexSprite hexSprite;
        bool condition = hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < mapContainer.Width && hexY < mapContainer.Height && hexLevel < mapContainer.LevelCount;
        if (condition)
        {
            var fieldValue = mapContainer.GetField(hexX, hexY, hexLevel);
            var terrain = _resource.TerrainList.GetTerrain(0);
            condition = fieldValue.TerrainType == terrain;
        }

        if (condition)
        {
            hexSprite = mapContainer.GetField(hexX, hexY, hexLevel).GetMaxLevelNoneTransparentHexSprite();
            result = hexSprite == null || hexSprite.GetLevel() < 10 || hexSprite is AbstractRoad;
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
