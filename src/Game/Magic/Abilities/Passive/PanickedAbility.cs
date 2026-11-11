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
        FAbilityName = AbilityRules.TranslateRStr(PanickedRStr);
        FAbilityID = 108;
        FAbilityType = AbilityType.Handicap;
        FDurationMode = DurationMode.EntireCombat;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:885
    public override bool Expand(AbilityOwner Owner)
    {
        bool Result = base.Expand(Owner);
        if (Owner is Troop)
        {
            Owner.Changed();
        }

        return Result;
    }

    private static readonly string PanickedRStr = "Panicked";
}
