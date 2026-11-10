// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Ai.Groups;

// queued moves
public partial class AIMoveExecuterMoveQueue : ENode
{
    public AIMoveExecuterMoveQueue(ECustomNode owner) : base(owner)
    { }

    // AIGroup.pas:4522
    public override void Clear()
    {
        base.Clear();
    }

    // AIGroup.pas:4508
    public AIMoveExecuterMove GetMoves(int Index) => (AIMoveExecuterMove)FChildren.List[Index];

    // AIGroup.pas:4513
    public void Reset()
    {
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            ((AIMoveExecuterUnitList)FChildren.List[I]).SetStatus(255, AIMoveExecuterUnitStatus.Ready);
        }

        Clear();
    }

    // Engine.pas:631

}
