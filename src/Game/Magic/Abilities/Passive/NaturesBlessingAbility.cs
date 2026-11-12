// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public partial class NaturesBlessingAbility : DurationAbility
{
    // PassiveAb.pas:514
    public NaturesBlessingAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_naturesBlessingText);
        AbilityId = 99;
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = Set<AbilitySelectionType>.Of();
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:524
    public override sbyte GetAttack(AbilityOwner owner) => 1;

    // PassiveAb.pas:529
    public override sbyte GetDefense(AbilityOwner owner) => 1;

    // PassiveAb.pas:534
    public override sbyte GetResistance(AbilityOwner owner) => 1;

    // PassiveAb.pas:539
    public override bool Expand(AbilityOwner owner)
    {
        bool result = base.Expand(owner);
        if (owner is Troop)
        {
            var troop = (Troop)owner;
            troop.SetHitPoints(troop.GetHits());
        }

        return result;
    }

    private static readonly string s_naturesBlessingText = "Nature's Blessing";
}
