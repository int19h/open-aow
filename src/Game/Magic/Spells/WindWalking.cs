// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class WindWalking : UnitSpell
{
    // UnitSpells.pas:702
    public WindWalking(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_windWalkingText);
        SpellId = 43;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Air;
        TargetRange = 12;
        UnitDispelResistance = 6;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:734
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(165) && !target.GetAbilityEnabled(59);
    }

    // UnitSpells.pas:715
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(165))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(165)).Enchantment;
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
