// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class EnchantWeapon : UnitSpell
{
    // UnitSpells.pas:296
    public EnchantWeapon(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_enchantWeaponText);
        SpellId = 16;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Cosmos;
        TargetRange = 12;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        UnitDispelResistance = 3;
        AIValue = 225;
        CastDuration = 5;
    }

    // UnitSpells.pas:330
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(154) && target.GetAbilityEnabled(21);
    }

    // UnitSpells.pas:311
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(154))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(154)).Enchantment;
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

    private static readonly string s_enchantWeaponText = "Enchant Weapon";
}
