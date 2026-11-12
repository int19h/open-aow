// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// references, target data, location, move points, transporter ID, path
public partial class MoveTarget : EngineObject
{
    // AIGroup.pas:4215
    public MoveTarget(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
    }

    public int RefCount;
    public AITargetData Data;
    public MapCoordinate Location;
    public int MovePoints;
    public int TransporterId;
    public MovePath MovePath;

    // AIGroup.pas:4221
    public override void Dispose()
    {
        if (MovePath != null)
        {
            MovePath?.Dispose();
        }

        base.Dispose();
    }

    // AIGroup.pas:4227
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // AIGroup.pas:4233
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (result == 0)
        {
            Dispose();
        }

        return result;
    }

    // intf_P5_06.inc:204

}
