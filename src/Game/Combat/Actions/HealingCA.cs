// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class HealingCA : SingleTargetCA
{
    public new const int ClassID = 131773;

    public HealingCA(ECustomNode owner) : base(owner)
    { }

    // AoWAb.PAS:3579
    public override void Execute(Battle Combat)
    {
        CombatUnit Source;
        CombatUnit Target;
        Source = (CombatUnit)Combat.FCombatData.FindID(FSourceID);
        Target = (CombatUnit)Combat.FCombatData.FindID(FTargetID);
        ((HealingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(47)).SetEnabled(Source.FUnit, false);
        ((HealingAbility)AoWHSSet.Current.FAbilityControl.GetAbility(47)).HealUnit(Target.FUnit);
    }

    // AoWAb.PAS:3595
    public override bool Play(CombatViewer Viewer)
    {
        if (Viewer.FTime == 0)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(47).FSFXLibrary.Play(0);
        }

        if (Viewer.FDelay == Viewer.FTime)
        {
            Execute(Viewer.FCombat);
        }

        return Viewer.FDelay * 2 > Viewer.FTime;
    }

    // AoWAb.PAS:3588
    public override void ShowObject(CombatViewer Viewer, CombatObject Obj, int Frame, int Direction, int X, int Y, in SurfaceDesc Surface)
    {
        base.ShowObject(Viewer, Obj, Frame, Direction, X, Y, Surface);
        if (Obj.FID == FTargetID)
        {
            AoWHSSet.Current.FAbilityControl.GetAbility(47).FImageSequences.Show(0, X + 32, Y + 32, Viewer.FTime, Surface);
        }
    }

    // AoWAb.PAS:3574
    public void Initialize(byte SourceID, byte TargetID)
    {
        FSourceID = SourceID;
        FTargetID = TargetID;
    }

    // intf_P3_02.inc:38

}
