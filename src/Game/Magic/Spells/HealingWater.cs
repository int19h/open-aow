// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class HealingWater : UnitSpell
{
    // UnitSpells.pas:467
    public HealingWater(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_healingWaterText);
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
        if (ValidTargetUnit(source, target))
        {
            _ = target.RemoveAbility(127);
            target.SetHitPoints(target.GetHits());
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_healingWaterText = "Healing Water";
}
