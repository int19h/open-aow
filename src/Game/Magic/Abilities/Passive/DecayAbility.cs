// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class DecayAbility : DurationAbility
{
    // PassiveAb.pas:844
    public DecayAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_decayText);
        AbilityId = 131;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.EntireCombat;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:855
    public override void NewCombatTurn(CombatObject combatObject, sbyte player)
    {
        DurationAbilityData data;
        data = (DurationAbilityData)combatObject.GetAbilityOwner().GetAbilityData(AbilityId);
        if (data != null && data.PlayerIndex == player)
        {
            if (combatObject.GetAbilityOwner() is Troop)
            {
                combatObject.ExecuteDamage(1);
            }
        }
    }

    // PassiveAb.pas:868
    public override void CombatDone(CombatObject combatObject)
    {
        combatObject.DestroyObject();
    }

    private static readonly string s_decayText = "Decay";
}
