// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Concealment : UnitSpell
{
    // UnitSpells.pas:577
    public Concealment(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.ConcealmentRStr);
        FSpellID = 39;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Earth;
        FTargetRange = 12;
        FUnitDispelResistance = 5;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
    }

    // UnitSpells.pas:609
    public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && !Target.GetAbilityEnabled(145) && !Target.GetAbilityEnabled(162) && !Target.GetAbilityEnabled(54);
    }

    // UnitSpells.pas:590
    public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(162))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(162)).FEnchantment;
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
