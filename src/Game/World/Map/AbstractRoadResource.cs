#nullable disable warnings
// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public class AbstractRoadResource(CustomNode owner) : MapObjectResource(owner)
{

    // MAPOBJ.PAS:874
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65556)
        {
            RoadControl.RegisterResource(this);
        }

        if (message.MessageId == 65621)
        {
            message.Result = 6;
        }
    }

    // MAPOBJ.PAS:883
    public override MapObject? PlaceResourceUser(GameMap? hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapObject? result = CreateResourceUser();
        HexGeometry.HexToPixel(hexX, hexY, out int pixelX, out int pixelY);
        pixelX += 16;
        pixelY += 16;
        if (!hexSpriteMap.Place(result, pixelX, pixelY, L: hexLevel))
        {
            result?.Dispose();
            return null;
        }

        return result;
    }

    // MAPOBJ.PAS:869
    public virtual RoadControl RoadControl => Engine.HexSpriteSet.RoadControl;

    public override MapObjectResourceTerrainControl TerrainControl => RoadControl;
}
