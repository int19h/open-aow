#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class EnchantWeapon : UnitSpell
{
    // UnitSpells.pas:296
    public EnchantWeapon(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_enchantWeaponText);
        SpellId = 16;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Cosmos;
        TargetRange = 12;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
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
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(154))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(154).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = UnitDispelResistance.TruncateToSByte();
            }

            // { Original succeeds even when expansion fails. }
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_enchantWeaponText = "Enchant Weapon";
}
