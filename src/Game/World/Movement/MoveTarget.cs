#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// references, target data, location, move points, transporter ID, path
public sealed class MoveTarget(CustomNode owner) : EngineObject(owner), IOwned<MoveTargetList>, IOwnedSink<MoveTargetList>
{
    // A reference list holds the target and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the cast never fails.
    public new MoveTargetList Owner => GetOwner<MoveTargetList>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is MoveTargetList;

    void IOwnedSink<MoveTargetList>.SetOwner(MoveTargetList owner) => SetOwner(owner);

    private int _refCount = 1;

    public AITargetData Data { get; set; }

    public MapCoordinate Location { get; internal set; }

    public int MovePoints { get; set; }
    public int TransporterId { get; set; }
    public MovePath MovePath { get; set; }

    // AIGroup.pas:4221
    public override void Dispose()
    {
        MovePath?.Dispose();

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
}
