// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public partial class HealingCombatAction : SingleTargetCombatAction
{
    public new const int ClassId = 131773;

    public HealingCombatAction(CustomNode owner) : base(owner)
    { }

    public override bool TouchSucceeded => true;

    // AoWAb.PAS:3579
    public override void Execute(Battle combat)
    {
        CombatUnit source;
        CombatUnit target;
        source = combat.CombatData.FindId<CombatUnit>(SourceId);
        target = combat.CombatData.FindId<CombatUnit>(TargetId);
        GamePack.Current.AbilityControl.GetAbility<HealingAbility>(47).SetEnabled(source.Troop, false);
        _ = GamePack.Current.AbilityControl.GetAbility<HealingAbility>(47).HealUnit(target.Troop);
    }

    // AoWAb.PAS:3595
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 0)
        {
            _ = GamePack.Current.AbilityControl.GetAbility(47).SfxLibrary.Play(0);
        }

        if (viewer.Delay == viewer.Time)
        {
            Execute(viewer.Combat);
        }

        return viewer.Delay * 2 > viewer.Time;
    }

    // AoWAb.PAS:3588
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId)
        {
            _ = GamePack.Current.AbilityControl.GetAbility(47).ImageSequences.Show(0, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // AoWAb.PAS:3574
    public void Initialize(byte sourceId, byte targetId)
    {
        SourceId = sourceId;
        TargetId = targetId;
    }

    // intf_P3_02.inc:38

}
