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
        FSpellName = AbilityRules.TranslateRStr(GameText.WindWalkingRStr);
        FSpellID = 43;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Air;
        FTargetRange = 12;
        FUnitDispelResistance = 6;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:734
    public override bool ValidTargetUnit(Troop Source, Troop Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(165) && !Target.GetAbilityEnabled(59);
    }

    // UnitSpells.pas:715
    public override bool ExecuteSpell(Troop Source, Troop Target, int Mana, ref Int32Array2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(165))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(165)).FEnchantment;
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
