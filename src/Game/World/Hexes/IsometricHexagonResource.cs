// Translated from IsoHex.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Hexes;

// FDynamicLevel (26)
public partial class IsometricHexagonResource : AbstractHexagonResource
{
    public IsometricHexagonResource(ECustomNode owner) : base(owner)
    {
        FDynamicLevel = 26;
    }

    // IsoHex.pas:268
    public new const int ClassID = 131438;

    protected internal byte FDynamicLevel;

    // IsoHex.pas:262

    // IsoHex.pas:302
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65621)
        {
            Msg.Result = 5;
        }
    }

    // IsoHex.pas:273
    public override MapObject PlaceResourceUser(HSMap Map, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapField MapField;
        MapField = Map.MapContainer.GetField(Xhx, Yhx, Lhx);
        MapObject Result = (MapObject)MapField.FindOwnedHS(typeof(IsometricHexagon));
        if (Result != null)
        {
            if (Result.GetTerrain(0) != FTerrainList.GetTerrain(0))
            {
                Result.SetResource(this);
                MapField.UpdateTerrainType();
                ((AbstractHexagon)Result).UpdateTransitions();
            }
            else
            {
                Result.SetResource(this);
            }

            Map.InvalidateStaticScene(Xhx, Yhx, Lhx);
        }
        else
        {
            Result = (MapObject)CreateResourceUser();
            if (!Map.PlaceHX(Result, Xhx, Yhx, Lhx))
            {
                Result?.Dispose();
                Result = null;
            }
        }

        return Result;
    }

    // IsoHex.pas:64

}
