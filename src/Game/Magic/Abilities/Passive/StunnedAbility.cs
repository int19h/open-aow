// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

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
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:810
    public override sbyte GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:804
    public override int TotalDuration => 1;

    private static readonly string s_stunnedText = "Stunned";
}
