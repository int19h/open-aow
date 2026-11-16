// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// queued moves
public partial class AIMoveExecutorMoveQueue : OwningNode, IChildList<AIMoveExecutorMove>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIMoveExecutorMove.
    public AIMoveExecutorMove this[int index] => GetChild<AIMoveExecutorMove>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIMoveExecutorMove;

    int IChildSink<AIMoveExecutorMove>.AddChild(AIMoveExecutorMove child) => AddChild(child);

    public AIMoveExecutorMoveQueue(CustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4522
    public override void Clear() => base.Clear();

    // AIGroup.pas:4508
    public AIMoveExecutorMove GetMoves(int index) => this[index];

    // AIGroup.pas:4513
    public void Reset()
    {
        for (int i = Count - 1; i >= 0; i--)
        {
            this[i].SetStatus(255, AIMoveExecutorUnitStatus.Ready);
        }

        Clear();
    }

    // Engine.pas:631

}
