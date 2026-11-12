// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class HolyFear : DurationAbility
{
    // PassiveAb.pas:816
    public HolyFear(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_holyFearText);
        AbilityId = 53;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = Set<AbilitySelectionType>.Of();
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:833
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        // { Original assumes the ability data exists. }
        var durationAbilityData = (DurationAbilityData)owner.GetAbilityData(AbilityId);
        if (durationAbilityData.Source == 1)
        {
            result = AbilityRules.TranslateResourceString(GameText.LifeMasteryText);
        }
        else
        {
            result = base.GetSourceName(owner);
        }

        return result;
    }

    // PassiveAb.pas:827
    public override int GetTotalDuration() => 3;

    private static readonly string s_holyFearText = "Holy Fear";
}
