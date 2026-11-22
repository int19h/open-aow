#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public class CursedAbility : DurationAbility
{
    // PassiveAb.pas:981
    public CursedAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_cursedText);
        AbilityId = 93;
        SelectionTypes = AbilitySelectionTypes.None;
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:1009
    public override int GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:1015
    public override int GetResistance(AbilityOwner owner) => -2;

    // PassiveAb.pas:991
    public override string GetSourceName(AbilityOwner owner)
    {
        DurationAbilityData data = GetData(owner);
        return data is not null && data.Source == 1
            ? AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DeathMasteryText)
            : base.GetSourceName(owner);
    }

    // PassiveAb.pas:1003
    public override int TotalDuration => 3;

    private static readonly string s_cursedText = "Cursed";
}
