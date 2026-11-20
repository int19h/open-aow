#nullable disable
// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

namespace Aow.Game.World.Map;

// the base of the terrain hexagons
public partial class AbstractHexagon(CustomNode owner) : MapObject(owner)
{
    public new const int ClassId = 65904;

    // MAPOBJ.PAS:682
    public override byte GetLevel() => 0;

    // MAPOBJ.PAS:671
    public override int GetShowPriority()
    {
        int pixelX;
        int pixelY;
        pixelX = (Owner.HexX * 32) + 8;
        pixelY = ((Owner.HexX & 1) * 16) + (Owner.HexY * 32);
        return pixelX + (pixelY << 13);
    }

    // MAPOBJ.PAS:704
    public override void ChangeTerrain(in HexSpriteTerrainInfo message)
    {
        MapObjectResourceTerrainControl control;
        MapObjectResource r;
        if (message.HexNumber == 0 && GetTerrain(0) != message.TerrainType)
        {
            control = Resource.GetTerrainControl();
            if (control is not null)
            {
                r = control.GetRandomResource(classId: Resource.EngineClass.ClassId, terrainType: message.TerrainType);
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
                    _ = Resource.GetTerrainControl().Place(Owner.Owner.Owner, hexX, hexY, hexLevel, message.TerrainType);
                }
            }
        }
    }

    // MAPOBJ.PAS:728
    public override void TerrainChanged(in HexSpriteTerrainInfo message) => ChangeTerrain(message);

    // MAPOBJ.PAS:762
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        if (message.MessageId != 16)
        {
            base.MapFieldMessageProc(ref message);
        }
    }

    // MAPOBJ.PAS:698
    public override void Activate()
    {
        base.Activate();
        UpdateTransitions();
    }

    // MAPOBJ.PAS:691
    public override void SetResource(MapObjectResource value)
    {
        base.SetResource(value);
        if (Status.Contains(MapObjectStatusElement.Activated))
        {
            UpdateTransitions();
        }
    }

    // MAPOBJ.PAS:733
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        bool result;
        MapField mapField;
        HexSprite hexSprite;
        if (hexX >= 0 && hexY >= 0 && hexLevel >= 0 && hexX < mapContainer.Width && hexY < mapContainer.Height && hexLevel < mapContainer.LevelCount)
        {
            mapField = mapContainer.GetField(hexX, hexY, hexLevel);
            hexSprite = mapField.GetMaxLevelNoneTransparentHexSprite();
            bool condition = hexSprite is null;
            if (!condition)
            {
                bool test = hexSprite.GetLevel() < 10;
                if (!test)
                {
                    test = mapField.CanChangeTerrain(overlay: GetOverlay(0), sender: this, terrain: GetTerrain(0));
                }

                condition = test && hexSprite is not AbstractBorderHexagon;
            }

            result = condition;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // MAPOBJ.PAS:750
    public override bool PlaceHex(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        HexSprite hexSprite;
        hexSprite = mapContainer.GetField(hexX, hexY, hexLevel).FindOwnedHexSprite(typeof(AbstractHexagon));
        hexSprite?.Dispose();

        bool result = base.PlaceHex(mapContainer, hexX, hexY, hexLevel);
        if (result)
        {
            UpdateTransitions();
        }

        return result;
    }

    // MAPOBJ.PAS:687
    public virtual void UpdateTransitions()
    {
    }

}
