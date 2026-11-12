// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class WebbedAbility : DurationAbility
{
    // PassiveAb.pas:736
    public WebbedAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_webbedText);
        AbilityId = 97;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.CombatTurn;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:751
    public override sbyte GetDefense(AbilityOwner owner) => -2;

    // PassiveAb.pas:746
    public override int GetTotalDuration() => 3;

    private static readonly string s_webbedText = "Webbed";
}
