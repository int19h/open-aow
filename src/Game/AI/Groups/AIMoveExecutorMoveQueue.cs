// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// queued moves
public partial class AIMoveExecutorMoveQueue : ENode, IChildList<AIMoveExecutorMove>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an AIMoveExecutorMove, so the cast never fails.
    public AIMoveExecutorMove this[int index] => (AIMoveExecutorMove)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is AIMoveExecutorMove;

    int IChildSink<AIMoveExecutorMove>.AddChild(AIMoveExecutorMove child) => AddChild(child);

    public AIMoveExecutorMoveQueue(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4522
    public override void Clear()
    {
        base.Clear();
    }

    // AIGroup.pas:4508
    public AIMoveExecutorMove GetMoves(int index) => this[index];

    // AIGroup.pas:4513
    public void Reset()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            this[I].SetStatus(255, AIMoveExecutorUnitStatus.Ready);
        }

        Clear();
    }

    // Engine.pas:631

}
