// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public partial class NaturesBlessingAbility : DurationAbility
{
    // PassiveAb.pas:514
    public NaturesBlessingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(NaturesBlessingRStr);
        FAbilityID = 99;
        FAbilityType = AbilityType.Enchantment;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:524
    public override sbyte GetAttack(AbilityOwner Owner) => 1;

    // PassiveAb.pas:529
    public override sbyte GetDefense(AbilityOwner Owner) => 1;

    // PassiveAb.pas:534
    public override sbyte GetResistance(AbilityOwner Owner) => 1;

    // PassiveAb.pas:539
    public override bool Expand(AbilityOwner Owner)
    {
        bool Result = base.Expand(Owner);
        if (Owner is AbstractUnit)
        {
            var unit = (AbstractUnit)Owner;
            unit.SetHitPoints(unit.GetHits());
        }

        return Result;
    }

    private static readonly string NaturesBlessingRStr = "Nature's Blessing";
}
