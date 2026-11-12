// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class BurningAbility : DurationAbility
{
    // PassiveAb.pas:756
    public BurningAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_burningText);
        AbilityId = 127;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.CombatTurn;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:773
    public override void NewCombatTurn(CombatObject combatObject, sbyte player)
    {
        DurationAbilityData data;
        data = (DurationAbilityData)combatObject.GetAbilityOwner().GetAbilityData(AbilityId);
        if (data != null)
        {
            if (data.Duration != 0 && data.PlayerIndex == player)
            {
                data.Duration = (byte)(data.Duration - 1);
                if (data.Duration == 0)
                {
                    combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
                }

                if (combatObject.GetAbilityOwner() is Troop troop)
                {
                    if (HitChance.HitRole(4 - troop.GetResistance()))
                    {
                        combatObject.ExecuteDamage(1);
                    }
                }
            }
        }
        else
        {
            combatObject.GetAbilityOwner().RemoveAbility(AbilityId);
        }
    }

    // PassiveAb.pas:767
    public override int GetTotalDuration() => 3;

    private static readonly string s_burningText = "Burning";
}
