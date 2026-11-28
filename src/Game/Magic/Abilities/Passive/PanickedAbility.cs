#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public class PanickedAbility : DurationAbility
{
    // PassiveAb.pas:874
    public PanickedAbility(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_panickedText);
        AbilityId = 108;
        AbilityType = AbilityType.Handicap;
        DurationMode = DurationMode.EntireCombat;
        SelectionTypes = AbilitySelectionTypes.None;
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
