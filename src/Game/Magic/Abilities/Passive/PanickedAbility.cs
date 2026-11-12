// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class PanickedAbility : DurationAbility
{
    // PassiveAb.pas:874
    public PanickedAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(s_panickedText);
        AbilityId = 108;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.EntireCombat;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:885
    public override bool Expand(AbilityOwner owner)
    {
        bool result = base.Expand(owner);
        if (owner is Troop)
        {
            owner.Changed();
        }

        return result;
    }

    private static readonly string s_panickedText = "Panicked";
}
