#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// (ability ID)
public class RangedAttackCombatAction(CustomNode owner) : DamageCombatAction(owner)
{
    public new const int ClassId = 131753;

    public int AbilityId { get; set; }

    // AoWAb.PAS:4631
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        AbilityId = persister.RawInt32("AbilityId", value: AbilityId);
    }

    // AoWAb.PAS:4643
    public override string GetName() => GamePack.Current.AbilityControl[AbilityId].GetName(null);

    // AoWAb.PAS:4647
    public override bool Play(CombatViewer viewer)
    {
        if (viewer.Time == 1)
        {
            _ = GamePack.Current.AbilityControl[AbilityId].SfxLibrary.Play(0);
        }

        return base.Play(viewer);
    }

    // AoWAb.PAS:4636
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId && viewer.Time != 0)
        {
            GamePack.Current.AbilityControl[AbilityId].ImageSequences.ShowLooped(40, X + 32, Y + 32, frame: viewer.Time, lockedSurface: surface);
        }
    }
}
