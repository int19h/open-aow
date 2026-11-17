#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.AI;
using Aow.Game.World.Map;

namespace Aow.Game.World.Movement;

// reference children
public partial class MoveTargetList : ReferenceNode, IChildList<MoveTarget>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a MoveTarget, so the cast never fails.
    public MoveTarget this[int index] => Targets[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is MoveTarget;

    int IChildSink<MoveTarget>.AddChild(MoveTarget child) => AddChild(child);

    public MoveTargetList(CustomNode owner) : base(owner)
    { }

    /// <summary>The targets of this list, in their stored order.</summary>
    public MoveTargetCollection Targets => new(this);

    // AIGroup.pas:4240
    public MoveTarget GetTarget(int index) => Targets.GetTarget(index);

    // AIGroup.pas:4245
    public int IndexOfMapCoordinate(MapCoordinate location) => Targets.IndexOfMapCoordinate(location);

    // AIGroup.pas:4256
    public void AddTarget(MapCoordinate location, int movePoints, int transporterId, in AITargetData data)
    {
        MoveTarget target;
        target = new MoveTarget(null) { Holder = CreationHolder() };
        target.Location = location;
        target.MovePoints = movePoints;
        target.TransporterId = transporterId;
        target.Data = data;
        _ = AddChild(target);
        _ = target.Release();
    }

    // AIGroup.pas:4275
    public void SortOnMovePoints() => Targets.SortOnMovePoints();

    // Engine.pas:631

    // Engine.pas:631

}
