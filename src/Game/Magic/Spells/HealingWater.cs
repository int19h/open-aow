// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class HealingWater : UnitSpell
{
    // UnitSpells.pas:467
    public HealingWater(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_healingWaterText);
        SpellId = 55;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Cosmos;
        TargetRange = 12;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:491
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && (target.GetHitPoints() != target.GetHits() || target.GetAbilityEnabled(127));
    }

    // UnitSpells.pas:479
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        bool result;
        if (ValidTargetUnit(source, target))
        {
            target.RemoveAbility(127);
            target.SetHitPoints(target.GetHits());
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_healingWaterText = "Healing Water";
}
