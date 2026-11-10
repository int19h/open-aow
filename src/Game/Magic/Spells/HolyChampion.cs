// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class HolyChampion : UnitSpell
{
    // UnitSpells.pas:497
    public HolyChampion(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.HolyChampionRStr);
        FSpellID = 36;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Life;
        FTargetRange = 12;
        FUnitDispelResistance = 4;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:529
    public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && !Target.GetAbilityEnabled(146) && !Target.GetAbilityEnabled(160) && AoWHSSet.Current.FAbilityControl.GetAbility(146).CanExpand(Target);
    }

    // UnitSpells.pas:510
    public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(160))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(160)).FEnchantment;
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
