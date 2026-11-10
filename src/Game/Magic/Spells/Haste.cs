// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Haste : UnitSpell
{
    // UnitSpells.pas:215
    public Haste(ECustomNode owner) : base(owner)
    {
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        FSpellName = AbilityRules.TranslateRStr(GameText.HasteRStr);
        FSpellID = 15;
        FBaseManaCost = 5;
        FManaUpkeep = 2;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Air;
        FTargetRange = 12;
        FUnitDispelResistance = 3;
    }

    // UnitSpells.pas:250
    public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && !Target.GetAbilityEnabled(152);
    }

    // UnitSpells.pas:229
    public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(152))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(152)).FEnchantment;
                Enchantment.SetUpkeep(FManaUpkeep);
                Enchantment.SetSource(Source.FPlayer);
                Enchantment.FDispelResistance = (sbyte)FUnitDispelResistance;
                Result = true;
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }
}
