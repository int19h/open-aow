// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public class HolyFear : DurationAbility
{
    // PassiveAb.pas:816
    public HolyFear(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_holyFearText);
        AbilityId = 53;
        AbilityType = AbilityType.Handicap;
        SelectionTypes = Set<AbilitySelectionType>.Of();
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:833
    public override string GetSourceName(AbilityOwner owner)
    {
        // { Original assumes the ability data exists. }
        DurationAbilityData durationAbilityData = GetData(owner);
        return durationAbilityData.Source == 1
            ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.LifeMasteryText)
            : base.GetSourceName(owner);
    }

    // PassiveAb.pas:827
    public override int TotalDuration => 3;

    private static readonly string s_holyFearText = "Holy Fear";
}
