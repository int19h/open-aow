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
        FAbilityName = AbilityRules.TranslateRStr(DecayRStr);
        FAbilityID = 131;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.EntireCombat;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:855
    public override void NewCombatTurn(CombatObject CombatObject, sbyte Player)
    {
        DurationAbilityData Data;
        Data = (DurationAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
        if (Data != null && Data.FPlayerIndex == Player)
        {
            if (CombatObject.GetAbilityOwner() is AbstractUnit)
            {
                CombatObject.ExecuteDamage(1);
            }
        }
    }

    // PassiveAb.pas:868
    public override void CombatDone(CombatObject CombatObject)
    {
        CombatObject.DestroyObject();
    }

    private static readonly string DecayRStr = "Decay";
}
