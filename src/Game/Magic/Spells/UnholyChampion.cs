// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class UnholyChampion : UnitSpell
{
    // UnitSpells.pas:537
    public UnholyChampion(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.UnholyChampionRStr);
        FSpellID = 37;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Death;
        FTargetRange = 12;
        FUnitDispelResistance = 4;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:569
    public override bool ValidTargetUnit(Troop Source, Troop Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && !Target.GetAbilityEnabled(147) && !Target.GetAbilityEnabled(161) && AoWHSSet.Current.FAbilityControl.GetAbility(147).CanExpand(Target);
    }

    // UnitSpells.pas:550
    public override bool ExecuteSpell(Troop Source, Troop Target, int Mana, ref Int32Array2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(161))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(161)).FEnchantment;
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
