// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.AI.Groups;

// queued moves
public partial class AIMoveExecutorMoveQueue : ENode
{
    public AIMoveExecutorMoveQueue(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4522
    public override void Clear()
    {
        base.Clear();
    }

    // AIGroup.pas:4508
    public AIMoveExecutorMove GetMoves(int Index) => (AIMoveExecutorMove)FChildren.List[Index];

    // AIGroup.pas:4513
    public void Reset()
    {
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            ((AIMoveExecutorUnitList)FChildren.List[I]).SetStatus(255, AIMoveExecutorUnitStatus.Ready);
        }

        Clear();
    }

    // Engine.pas:631

}
