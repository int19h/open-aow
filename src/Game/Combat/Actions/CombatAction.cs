// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// FRefCount
public partial class CombatAction : EngineObject
{

    // Combat2.pas:698
    public CombatAction(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    public int FRefCount;

    // Combat2.pas:701
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Combat2.pas:704
    public override int Release()
    {
        int N;
        FRefCount -= 1;
        N = FRefCount;
        int Result = N;
        if (N == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Combat2.pas:713
    public virtual string GetName() => this.GetType().Name;

    // intf_P3_02.inc:42
    public virtual void Execute(Battle Combat)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // intf_P3_02.inc:43
    public virtual bool Play(CombatViewer Viewer)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Combat2.pas:716
    public virtual void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        Obj.Show(Frame, Direction, X, Y, Surface);
    }

    // Combat2.pas:721
    public virtual void ShowFieldObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        Obj.Show(Frame, Direction, X, Y, Surface);
    }

    // Combat2.pas:726
    public virtual void ShowFightingObjects(CombatViewer Viewer, int Frame, int X, int Y, in LockedSurface Surface)
    {
    }

    // intf_P3_02.inc:38

}
