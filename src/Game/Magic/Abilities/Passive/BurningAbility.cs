// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public class BurningAbility : DurationAbility
{
    // PassiveAb.pas:756
    public BurningAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_burningText);
        AbilityId = 127;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.CombatTurn;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:773
    public override void NewCombatTurn(CombatObject combatObject, sbyte player)
    {
        DurationAbilityData data = GetData(combatObject.AbilityOwner);
        if (data is not null)
        {
            if (data.Duration != 0 && data.PlayerIndex == player)
            {
                data.Duration = (byte)(data.Duration - 1);
                if (data.Duration == 0)
                {
                    _ = combatObject.AbilityOwner.RemoveAbility(AbilityId);
                }

                if (combatObject.AbilityOwner is Troop troop)
                {
                    if (HitChance.HitRole(4 - troop.GetResistance()))
                    {
                        _ = combatObject.ExecuteDamage(1);
                    }
                }
            }
        }
        else
        {
            _ = combatObject.AbilityOwner.RemoveAbility(AbilityId);
        }
    }

    // PassiveAb.pas:767
    public override int TotalDuration => 3;

    private static readonly string s_burningText = "Burning";
}
