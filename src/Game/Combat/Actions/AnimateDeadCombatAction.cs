#nullable disable
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// inherited source ID, target ID, mana, success, spell ID
public partial class AnimateDeadCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:2021
    public new const int ClassId = 131782;

    // CombatSpells.pas:2024
    public override void Execute(Battle combat)
    {
        CombatUnit source;
        CombatUnit target;
        Unit newUnit;
        base.Execute(combat);
        if (Succeeded)
        {
            source = combat.CombatData.FindId<CombatUnit>(SourceId);
            target = combat.CombatData.FindId<CombatUnit>(TargetId);
            newUnit = new Unit(null) { Holder = CreationHolder() };
            newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(201));
            newUnit.SetPlayer(source.Troop.Player);
            _ = newUnit.ExpandAbility(130);
            newUnit.Activate();
            target.SetUnit(newUnit);
            _ = newUnit.Release();
            target.SetStatus(target.Status - Set<CombatObjectStatusElement>.Of(CombatObjectStatusElement.Destroyed));
            target.SetCombatPlayer((byte)source.CombatPlayer);
        }
    }

}
