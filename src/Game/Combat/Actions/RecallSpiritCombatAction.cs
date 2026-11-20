#nullable disable
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat.Actions;

// inherited source ID, target ID, mana, success, spell ID
public class RecallSpiritCombatAction(CustomNode owner) : ExclusiveCombatSpellCombatAction(owner)
{
    // CombatSpells.pas:1950
    public new const int ClassId = 131_781;

    // CombatSpells.pas:1953
    public override void Execute(Battle combat)
    {
        base.Execute(combat);
        if (Succeeded)
        {
            CombatUnit source = combat.CombatData.FindId<CombatUnit>(SourceId);
            CombatUnit target = combat.CombatData.FindId<CombatUnit>(TargetId);
            Unit newUnit = new(null) { Holder = CreationHolder() };
            newUnit.SetUnitResource(GamePack.Current.UnitResources.GetUnitResource(130));
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
