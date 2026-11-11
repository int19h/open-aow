// Translated from HSEngine.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.World.Map;

public partial class AbstractRoadResource : MapObjectResource
{
    public AbstractRoadResource(ECustomNode owner) : base(owner)
    { }

    // MAPOBJ.PAS:874
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65556)
        {
            GetRoadControl().RegisterResource(this);
        }

        if (Msg.MsgID == 65621)
        {
            Msg.Result = 6;
        }
    }

    // MAPOBJ.PAS:883
    public override MapObject PlaceResourceUser(HSMap HSMap, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        int Xhp = default;
        int Yhp = default;
        MapObject Result = (MapObject)CreateResourceUser();
        HexGeometry.HXtoHP(Xhx, Yhx, ref Xhp, ref Yhp);
        Xhp += 16;
        Yhp += 16;
        if (!HSMap.Place(Result, Xhp, Yhp, Lhx))
        {
            Result?.Dispose();
            Result = null;
        }

        return Result;
    }

    // MAPOBJ.PAS:869
    public virtual RoadControl GetRoadControl() => FEngine.FHSSet.FRoadControl;
}
