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
        FSpellName = AbilityRules.TranslateRStr(EnchantWeaponRStr);
        FSpellID = 16;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Cosmos;
        FTargetRange = 12;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        FUnitDispelResistance = 3;
        FAIValue = 225;
        FCastDuration = 5;
    }

    // UnitSpells.pas:330
    public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && !Target.GetAbilityEnabled(154) && Target.GetAbilityEnabled(21);
    }

    // UnitSpells.pas:311
    public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(154))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(154)).FEnchantment;
                Enchantment.SetUpkeep(FManaUpkeep);
                Enchantment.SetSource(Source.FPlayer);
                Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
            }

            // { Original succeeds even when expansion fails. }
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string EnchantWeaponRStr = "Enchant Weapon";
}
