// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public partial class CharmAbility : CommandAbility
{
    // AoWAb.PAS:5339
    public CharmAbility(ECustomNode owner) : base(owner)
    {
        AbilityId = 148;
        AbilityName = AbilityRules.TranslateResourceString(s_charmText);
        CommandedAbilityId = 149;
    }

    // AoWAb.PAS:5346
    public override sbyte GetTouchAttack(int level) => 5;

    // AoWAb.PAS:5355
    public override bool CanTouch(CombatObject source, CombatObject target)
    {
        bool result = base.CanTouch(source, target);
        if (result)
        {
            var combatUnit = (CombatUnit)target;
            result = combatUnit.Troop.GetUnitType() == UnitType.Humanoid && (ushort)combatUnit.Troop.GetRace() != 11 && !combatUnit.Troop.GetAbilityEnabled(129);
        }

        return result;
    }

    // AoWAb.PAS:5350
    public override bool CanTouchUnit(Troop source, Troop target) => base.CanTouchUnit(source, target) && (target.GetUnitType() == UnitType.Humanoid && (ushort)target.GetRace() != 11 && !target.GetAbilityEnabled(129));

    private static readonly string s_charmText = "Charm";
}
