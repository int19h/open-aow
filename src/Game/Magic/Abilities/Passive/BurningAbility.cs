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
        FAbilityName = AbilityRules.TranslateRStr(BurningRStr);
        FAbilityID = 127;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.CombatTurn;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:773
    public override void NewCombatTurn(CombatObject CombatObject, sbyte Player)
    {
        DurationAbilityData Data;
        Data = (DurationAbilityData)CombatObject.GetAbilityOwner().GetAbilityData(FAbilityID);
        if (Data != null)
        {
            if (Data.FDuration != 0 && Data.FPlayerIndex == Player)
            {
                Data.FDuration = (byte)(Data.FDuration - 1);
                if (Data.FDuration == 0)
                {
                    CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
                }

                if (CombatObject.GetAbilityOwner() is AbstractUnit unit)
                {
                    if (HitChance.HitRole(4 - unit.GetResistance()))
                    {
                        CombatObject.ExecuteDamage(1);
                    }
                }
            }
        }
        else
        {
            CombatObject.GetAbilityOwner().RemoveAbility(FAbilityID);
        }
    }

    // PassiveAb.pas:767
    public override int GetTotalDuration() => 3;

    private static readonly string BurningRStr = "Burning";
}
