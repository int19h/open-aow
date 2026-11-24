#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Session;
using Aow.Rtl;

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
    public override void NewCombatTurn(CombatObject combatObject, PlayerId player)
    {
        DurationAbilityData data = GetData(combatObject.AbilityOwner);
        if (data is not null)
        {
            if (data.Duration != 0 && data.PlayerIndex == player)
            {
                data.Duration = (data.Duration - 1).TruncateToByte();
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
