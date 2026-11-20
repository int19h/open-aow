#nullable disable
// Translated from IsoHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// DynamicLevel (26)
public class IsometricHexResource(CustomNode owner) : AbstractHexagonResource(owner)
{

    // IsoHex.pas:268
    public new const int ClassId = 131438;

    protected internal byte DynamicLevel { get; protected set; } = 26;

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
    public override MapObject PlaceResourceUser(GameMap map, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapField mapField = map.MapContainer.GetField(hexX, hexY, hexLevel);
        // The lookup finds only IsometricHex, including a hex that derives from it.
        IsometricHex found = mapField.FindOwnedHexSprite<IsometricHex>();
        MapObject result = found;
        if (found is not null)
        {
            if (found.GetTerrain(0) != TerrainList.GetTerrain(0))
            {
                found.SetResource(this);
                mapField.UpdateTerrainType();
                found.UpdateTransitions();
            }
            else
            {
                found.SetResource(this);
            }

            map.InvalidateStaticScene(hexX, hexY, hexLevel);
        }
        else
        {
            result = CreateResourceUser();
            if (!map.PlaceHex(result, hexX, hexY, L: hexLevel))
            {
                result?.Dispose();
                return null;
            }
        }

        return result;
    }
}
