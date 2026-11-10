// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Fury : UnitSpell
{
    // UnitSpells.pas:336
    public Fury(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.FuryRStr);
        FSpellID = 33;
        FAIValue = 175;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Fire;
        FTargetRange = 12;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        FUnitDispelResistance = 3;
    }

    // UnitSpells.pas:369
    public override void PlayDefaultCastEffects(sbyte Player, sbyte X, sbyte Y, sbyte Layer)
    {
        base.PlayDefaultCastEffects(Player, X, Y, Layer);
        if (Player == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(1);
        }
    }

    // UnitSpells.pas:376
    public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        Result = Result && !Target.GetAbilityEnabled(156);
        if (Result)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Target.FPlayer).FPlayerType != PlayerType.Human && Target is Leader)
            {
                Result = false;
            }
        }

        return Result;
    }

    // UnitSpells.pas:350
    public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
    {
        bool Result;
        UnitEnchantment Enchantment;
        if (ValidTargetUnit(Source, Target))
        {
            if (Target.ExpandAbility(156))
            {
                Enchantment = ((UnitEnchantmentAbilityData)Target.GetAbilityData(156)).FEnchantment;
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
