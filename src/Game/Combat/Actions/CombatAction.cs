// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// RefCount
public partial class CombatAction(CustomNode owner) : EngineObject(owner), IOwned<CombatActionList>, IOwnedSink<CombatActionList>
{
    // A reference list holds the action and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the cast never fails.
    public new CombatActionList Owner { get => GetOwner<CombatActionList>(); set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is CombatActionList;

    void IOwnedSink<CombatActionList>.SetOwner(CombatActionList owner) => SetOwner(owner);

    public int RefCount { get; private set; } = 1;

    // Combat2.pas:701
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // Combat2.pas:704
    public override int Release()
    {
        RefCount -= 1;
        int n = RefCount;
        int result = n;
        if (n == 0)
        {
            Dispose();
        }

        return result;
    }

    // A touch move reads this after it builds the action. Healing always succeeds. The other touch
    // actions report the result they stored. A combat action that is not a touch action is not asked.
    public virtual bool TouchSucceeded => false;

    // Combat2.pas:713
    public virtual string GetName() => GetType().Name;

    // intf_P3_02.inc:42
    public virtual void Execute(Battle combat) => throw AbstractErrorException.Create("Abstract Error");

    // intf_P3_02.inc:43
    public virtual bool Play(CombatViewer viewer) => throw AbstractErrorException.Create("Abstract Error");

    // Combat2.pas:716
    public virtual void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface) => objectValue.Show(frame, direction, X, Y, surface);

    // Combat2.pas:721
    public virtual void ShowFieldObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface) => objectValue.Show(frame, direction, X, Y, surface);

    // Combat2.pas:726
    public virtual void ShowFightingObjects(CombatViewer viewer, int frame, int X, int Y, in LockedSurface surface)
    {
    }

    // intf_P3_02.inc:38

}
