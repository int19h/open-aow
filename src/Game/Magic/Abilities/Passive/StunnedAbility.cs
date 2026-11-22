#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public class StunnedAbility : DurationAbility
{
    // PassiveAb.pas:793
    public StunnedAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_stunnedText);
        AbilityId = 92;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.CombatTurn;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:810
    public override int GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:804
    public override int TotalDuration => 1;

    private static readonly string s_stunnedText = "Stunned";
}
