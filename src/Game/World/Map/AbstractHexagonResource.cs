// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public partial class AbstractHexagonResource : MapObjectResource
{

    public AbstractHexagonResource(ECustomNode owner) : base(owner)
    {
        SetLevel(0);
    }

    // MAPOBJ.PAS:770

    // MAPOBJ.PAS:781
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65556)
        {
            if (GetHexControl() != null)
            {
                GetHexControl().RegisterResource(this);
            }
        }
    }

    // MAPOBJ.PAS:776
    public virtual HexControl GetHexControl() => Engine.HexSpriteSet.HexControl;

}
