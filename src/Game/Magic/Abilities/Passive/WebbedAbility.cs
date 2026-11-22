#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public class WebbedAbility : DurationAbility
{
    // PassiveAb.pas:736
    public WebbedAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_webbedText);
        AbilityId = 97;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.CombatTurn;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:751
    public override int GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:746
    public override int TotalDuration => 3;

    private static readonly string s_webbedText = "Webbed";
}
