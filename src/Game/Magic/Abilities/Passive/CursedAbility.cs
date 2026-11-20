// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class CursedAbility : DurationAbility
{
    // PassiveAb.pas:981
    public CursedAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_cursedText);
        AbilityId = 93;
        SelectionTypes = Set<AbilitySelectionType>.Of();
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:1009
    public override sbyte GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:1015
    public override sbyte GetResistance(AbilityOwner owner) => -2;

    // PassiveAb.pas:991
    public override string GetSourceName(AbilityOwner owner)
    {
        string result;
        DurationAbilityData data;
        data = GetData(owner);
        result = data is not null && data.Source == 1
            ? AbilityRules.TranslateResourceString(GetEngine(), GameText.DeathMasteryText)
            : base.GetSourceName(owner);

        return result;
    }

    // PassiveAb.pas:1003
    public override int GetTotalDuration() => 3;

    private static readonly string s_cursedText = "Cursed";
}
