#nullable disable warnings
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Combat.Actions;

// inherited source ID, target ID, mana, success, spell ID
public class AnimateDeadCombatAction(CustomNode? owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:2021
    public new const int ClassId = 131_782;

    // CombatSpells.pas:2024
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            Unit newUnit = new(null) { Holder = CreationHolder() };
            newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(201));
            newUnit.SetPlayer(source.Troop.Player);
            _ = newUnit.ExpandAbility(130);
            newUnit.Activate();
            target.SetUnit(newUnit);
            _ = newUnit.Release();
            target.SetStatus(target.Status & ~CombatObjectStatuses.Destroyed);
            target.SetCombatPlayer((byte)source.CombatPlayer);
        }
    }
}
