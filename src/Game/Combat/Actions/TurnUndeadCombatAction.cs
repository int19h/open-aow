// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

public class TurnUndeadCombatAction(CustomNode owner) : DamageCombatAction(owner)
{
    public new const int ClassId = 131_769;

    // AoWAb.PAS:3088
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Damage != 0)
        {
            CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            if (!target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                _ = GamePack.Current.AbilityControl[34].Expand(target.Troop);
                UnitEnchantmentAbilityData data = target.Troop.GetAbilityData<UnitEnchantmentAbilityData>(34);

                data?.EnchantmentPlayerIndex = source.CombatPlayer;
            }
        }
    }

    // AoWAb.PAS:3115
    public override bool Play(CombatViewer viewer)
    {
        bool result = base.Play(viewer);
        if (viewer.Time == 0)
        {
            _ = GamePack.Current.AbilityControl[38].SfxLibrary.Play(0);
        }

        return result;
    }

    // AoWAb.PAS:3108
    public override void ShowObject(CombatViewer viewer, CombatObject objectValue, int frame, int direction, int X, int Y, in LockedSurface surface)
    {
        base.ShowObject(viewer, objectValue, frame, direction, X, Y, surface);
        if (objectValue.Id == TargetId)
        {
            _ = GamePack.Current.AbilityControl[38].ImageSequences.Show(0, X + 32, Y + 32, frame: viewer.Time, lockedSurface: surface);
        }
    }

    // AoWAb.PAS:3084
    public bool Successful => Damage != 0;

    public override bool TouchSucceeded => Successful;
}
