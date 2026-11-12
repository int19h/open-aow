// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode. duration data
// countdown, player index
public partial class CrusaderAbility : DurationAbility
{
    // PassiveAb.pas:703
    public CrusaderAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_crusaderText);
        AbilityId = 101;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.Day;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:724
    public override void NewTurn(AbilityOwner owner, sbyte player)
    {
        DurationAbilityData data;
        if (DurationMode == DurationMode.Day)
        {
            data = (DurationAbilityData)owner.GetAbilityData(AbilityId);
            if (data != null && data.Duration != 0 && data.PlayerIndex == player)
            {
                data.Duration = (byte)(data.Duration - 1);
            }
        }
    }

    // PassiveAb.pas:713
    public override bool Remove(AbilityOwner owner)
    {
        bool result = base.Remove(owner);
        if (owner is Troop troop)
        {
            troop.Killed();
        }

        return result;
    }

    // PassiveAb.pas:719
    public override int GetTotalDuration() => 3;

    private static readonly string s_crusaderText = "Crusader";
}
