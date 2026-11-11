// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class FireAura : UnitSpell
{
    // UnitSpells.pas:616
    public FireAura(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.FireAuraRStr);
        FSpellID = 40;
        FBaseManaCost = 5;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Fire;
        FTargetRange = 12;
        FUnitDispelResistance = 4;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:629
    public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
    {
        base.PlayDefaultCastEffects(Player, X, Y, Layer);
        if (Player == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
        }
    }

    // UnitSpells.pas:655
    public override bool ValidTargetUnit(Troop Source, Troop Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && (!Target.GetAbilityEnabled(7) || !Target.GetAbilityEnabled(43)) && !Target.GetAbilityEnabled(163);
    }

    // UnitSpells.pas:636
    public override bool ExecuteSpell(Troop Source, Troop Target, int Mana, ref Int32Array2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(163))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(163)).FEnchantment;
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
