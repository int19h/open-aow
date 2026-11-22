#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type. duration mode
public class NaturesBlessingAbility : DurationAbility
{
    // PassiveAb.pas:514
    public NaturesBlessingAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_naturesBlessingText);
        AbilityId = 99;
        AbilityType = AbilityType.Enchantment;
        SelectionTypes = AbilitySelectionTypes.None;
        DurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:524
    public override int GetAttack(AbilityOwner owner) => 1;

    // PassiveAb.pas:529
    public override int GetDefense(AbilityOwner owner) => 1;

    // PassiveAb.pas:534
    public override int GetResistance(AbilityOwner owner) => 1;

    // PassiveAb.pas:539
    public override bool Expand(AbilityOwner owner)
    {
        bool result = base.Expand(owner);
        if (owner is Troop troop)
        {
            troop.SetHitPoints((sbyte)troop.GetHits());
        }

        return result;
    }

    private static readonly string s_naturesBlessingText = "Nature's Blessing";
}
