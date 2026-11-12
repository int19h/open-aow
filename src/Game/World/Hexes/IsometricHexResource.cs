// Translated from IsoHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// DynamicLevel (26)
public partial class IsometricHexResource : AbstractHexagonResource
{
    public IsometricHexResource(ECustomNode owner) : base(owner)
    {
        DynamicLevel = 26;
    }

    // IsoHex.pas:268
    public new const int ClassId = 131438;

    protected internal byte DynamicLevel { get; protected set; }

    // IsoHex.pas:262

    // IsoHex.pas:302
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65621)
        {
            message.Result = 5;
        }
    }

    // IsoHex.pas:273
    public override MapObject PlaceResourceUser(HSMap map, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField;
        mapField = map.MapContainer.GetField(hexX, hexY, hexLevel);
        MapObject result = (MapObject)mapField.FindOwnedHexSprite(typeof(IsometricHex));
        if (result != null)
        {
            if (result.GetTerrain(0) != TerrainList.GetTerrain(0))
            {
                result.SetResource(this);
                mapField.UpdateTerrainType();
                ((AbstractHexagon)result).UpdateTransitions();
            }
            else
            {
                result.SetResource(this);
            }

            map.InvalidateStaticScene(hexX, hexY, hexLevel);
        }
        else
        {
            result = (MapObject)CreateResourceUser();
            if (!map.PlaceHex(result, hexX, hexY, hexLevel))
            {
                result?.Dispose();
                result = null;
            }
        }

        return result;
    }

    // IsoHex.pas:64

}
