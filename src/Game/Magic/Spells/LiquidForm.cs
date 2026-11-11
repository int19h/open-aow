// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class LiquidForm : UnitSpell
{
    // UnitSpells.pas:741
    public LiquidForm(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.LiquidFormRStr);
        FSpellID = 49;
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Water;
        FAIValue = 300;
        FTargetRange = 12;
        FUnitDispelResistance = 5;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:783
    public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
    {
        base.PlayDefaultCastEffects(Player, X, Y, Layer);
        if (Player == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
        }
    }

    // UnitSpells.pas:774
    public override bool ValidTargetUnit(Troop Source, Troop Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        Result = Result && !Target.GetAbilityEnabled(166);
        return Result && (!Target.GetAbilityEnabled(13) && !Target.GetAbilityEnabled(77) || !Target.GetAbilityEnabled(2) && !Target.GetAbilityEnabled(1) && !Target.GetAbilityEnabled(59));
    }

    // UnitSpells.pas:755
    public override bool ExecuteSpell(Troop Source, Troop Target, int Mana, ref Int32Array2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(166))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(166)).FEnchantment;
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
