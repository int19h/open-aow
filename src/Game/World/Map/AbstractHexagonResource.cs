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
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65556)
        {
            if (GetHexagonControl() != null)
            {
                GetHexagonControl().RegisterResource(this);
            }
        }
    }

    // MAPOBJ.PAS:776
    public virtual HexagonControl GetHexagonControl() => FEngine.FHSSet.FHexagonControl;

}
