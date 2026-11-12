// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Draw;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// RefCount
public partial class CombatAction : EngineObject, IOwned<CombatActionList>, IOwnedSink<CombatActionList>
{
    // A reference list holds the action and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the cast never fails.
    public new CombatActionList Owner { get => (CombatActionList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is CombatActionList;

    void IOwnedSink<CombatActionList>.SetOwner(CombatActionList owner) => SetOwner(owner);

    // Combat2.pas:698
    public CombatAction(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
    }

    public int RefCount { get; private set; }

    // Combat2.pas:701
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // Combat2.pas:704
    public override int Release()
    {
        int N;
        RefCount -= 1;
        N = RefCount;
        int result = N;
        if (N == 0)
        {
            Dispose();
        }

        return result;
    }

    // Combat2.pas:713
    public virtual string GetName() => this.GetType().Name;

    // intf_P3_02.inc:42
    public virtual void Execute(Battle combat)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // intf_P3_02.inc:43
    public virtual bool Play(CombatViewer viewer)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat2.pas:716
    public virtual void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        objectValue.Show(frame, direction, X, Y, surface);
    }

    // Combat2.pas:721
    public virtual void ShowFieldObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        objectValue.Show(frame, direction, X, Y, surface);
    }

    // Combat2.pas:726
    public virtual void ShowFightingObjects(CombatViewer viewer, int frame, int X, int Y, in LockedSurface surface)
    {
    }

    // intf_P3_02.inc:38

}
