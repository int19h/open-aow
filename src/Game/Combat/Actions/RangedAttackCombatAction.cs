// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (ability ID)
public partial class RangedAttackCombatAction : DamageCombatAction
{
    public new const int ClassId = 131753;

    public RangedAttackCombatAction(ECustomNode owner) : base(owner)
    { }

    public int AbilityId { get; set; }

    // AoWAb.PAS:4631
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.RawInt32("AbilityId", value: AbilityId);
    }

    // AoWAb.PAS:4643
    public override string GetName() => GamePack.Current.AbilityControl.GetAbility(AbilityId).GetName(null);

    // AoWAb.PAS:4647
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 1)
        {
            GamePack.Current.AbilityControl.GetAbility(AbilityId).SfxLibrary.Play(0);
        }

        return base.Play(viewer);
    }

    // AoWAb.PAS:4636
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId && viewer.Time != 0)
        {
            GamePack.Current.AbilityControl.GetAbility(AbilityId).ImageSequences.ShowLooped(40, X + 32, Y + 32, viewer.Time, surface);
        }
    }

    // intf_P3_02.inc:38

}
