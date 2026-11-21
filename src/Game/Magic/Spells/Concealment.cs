#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;

namespace Aow.Game.Magic.Spells;

public class Concealment : UnitSpell
{
    // UnitSpells.pas:577
    public Concealment(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.ConcealmentText);
        SpellId = 39;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
        TargetRange = 12;
        UnitDispelResistance = 5;
        SpellControlTypes = SpellControlTypes.Global;
    }

    // UnitSpells.pas:609
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(145) && !target.GetAbilityEnabled(162) && !target.GetAbilityEnabled(54);
    }

    // UnitSpells.pas:590
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(162))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(162).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = (sbyte)UnitDispelResistance;
            }

            // { Original succeeds even when expansion fails. }
            return true;
        }
        else
        {
            return false;
        }
    }
}
