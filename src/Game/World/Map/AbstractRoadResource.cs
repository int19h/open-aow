// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public partial class AbstractRoadResource : MapObjectResource
{
    public AbstractRoadResource(ECustomNode owner) : base(owner)
    { }

    // MAPOBJ.PAS:874
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65556)
        {
            GetRoadControl().RegisterResource(this);
        }

        if (message.MessageId == 65621)
        {
            message.Result = 6;
        }
    }

    // MAPOBJ.PAS:883
    public override MapObject PlaceResourceUser(GameMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        int pixelX = default;
        int pixelY = default;
        MapObject result = (MapObject)CreateResourceUser();
        HexGeometry.HexToPixel(hexX, hexY, out pixelX, out pixelY);
        pixelX += 16;
        pixelY += 16;
        if (!hexSpriteMap.Place(result, pixelX, pixelY, hexLevel))
        {
            result?.Dispose();
            result = null;
        }

        return result;
    }

    // MAPOBJ.PAS:869
    public virtual RoadControl GetRoadControl() => Engine.HexSpriteSet.RoadControl;
}
