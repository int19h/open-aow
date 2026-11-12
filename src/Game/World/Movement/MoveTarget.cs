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
        _refCount = 1;
    }

    private int _refCount;

    public AITargetData Data { get; set; }

    public MapCoordinate Location;

    public int MovePoints { get; set; }
    public int TransporterId { get; set; }
    public MovePath MovePath { get; set; }

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
        _refCount += 1;
        return _refCount;
    }

    // AIGroup.pas:4233
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (result == 0)
        {
            Dispose();
        }

        return result;
    }

    // intf_P5_06.inc:204

}
