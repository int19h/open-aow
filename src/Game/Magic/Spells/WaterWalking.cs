// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class WaterWalking : UnitSpell
{
    // UnitSpells.pas:662
    public WaterWalking(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.WaterWalkingText);
        SpellId = 42;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Water;
        TargetRange = 12;
        UnitDispelResistance = 6;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:694
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(23) && !target.GetAbilityEnabled(2) && !target.GetAbilityEnabled(164) && !target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(59);
    }

    // UnitSpells.pas:675
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(164))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(164)).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = (sbyte)UnitDispelResistance;
            }

            // { Original succeeds even when expansion fails. }
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }
}
