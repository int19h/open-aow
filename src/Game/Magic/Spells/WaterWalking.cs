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
        FSpellName = AbilityRules.TranslateRStr(GameText.WaterWalkingRStr);
        FSpellID = 42;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Water;
        FTargetRange = 12;
        FUnitDispelResistance = 6;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:694
    public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && !Target.GetAbilityEnabled(23) && !Target.GetAbilityEnabled(2) && !Target.GetAbilityEnabled(164) && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(59);
    }

    // UnitSpells.pas:675
    public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(164))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(164)).FEnchantment;
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
}
