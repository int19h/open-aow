// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public class AbstractHexagonResource : MapObjectResource
{

    public AbstractHexagonResource(CustomNode owner) : base(owner) => SetLevel(0);

    // MAPOBJ.PAS:781
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65556)
        {
            GetHexControl()?.RegisterResource(this);
        }
    }

    // MAPOBJ.PAS:776
    public virtual HexControl GetHexControl() => Engine.HexSpriteSet.HexControl;

    public override MapObjectResourceTerrainControl GetTerrainControl() => GetHexControl();

}
