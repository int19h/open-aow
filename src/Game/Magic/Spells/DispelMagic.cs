// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited multiple targets, target range, dispel resistance, AI value
public partial class DispelMagic : UnitSpell
{
    // UnitSpells.pas:1000
    public DispelMagic(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(GameText.DispelMagicRStr);
        FSpellID = 8;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Cosmos;
        FTargetRange = 4;
        FAllowMultipleTargets = true;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:1023
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        DispelMagicSpellCombatAction Action;
        Action = new DispelMagicSpellCombatAction(null);
        Action.FSpellID = FSpellID;
        Action.FSourceID = Source.FID;
        Action.FTargetID = Target.FID;
        Action.Generate(Source, Target, GetDispelMana(((CombatUnit)Source).FUnit));
        if (ChargeMana)
        {
            Action.FManaCost = FBaseManaCost;
        }

        return Action;
    }

    // UnitSpells.pas:1074
    public override void GetCombatInfo(Troop Source, ref SpellCombatInfo Info)
    {
        base.GetCombatInfo(Source, ref Info);
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend, CombatTargetTypeElement.Enemy);
    }

    // UnitSpells.pas:1068
    public override bool ValidTargetUnit(Troop Source, Troop Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && Target.CanDispelEnchantment(Source.FPlayer);
    }

    // UnitSpells.pas:1018
    public override bool ValidTargetParty(Troop Source, ArmyHexSprite Party) => Party.FArmy.FPlayer == Source.FPlayer;

    // UnitSpells.pas:1036
    public override bool ExecuteSpell(Troop Source, Troop Target, int Mana, ref Int32Array2 Data)
    {
        bool Result;
        int Count;
        EnchantmentList Enchantments;
        int Chance;
        if (ValidTargetUnit(Source, Target))
        {
            Count = 0;
            Enchantments = new EnchantmentList(null);
            Target.ListEnchantments(Enchantments);
            for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
            {
                var dispelMana = GetDispelMana(Source);
                Chance = Enchantments.GetEnchantments(I).DispelChance(dispelMana);
                if (Chance > AoWHSMap.Current.Random(100))
                {
                    Count += 1;
                    Enchantments.GetEnchantments(I).Dispel();
                }
            }

            if (Source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                if (Count > 0)
                {
                    var format = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XSpellsSuccessfullyDispelledRStr), [IntToStr(Count)]);
                    AoWHSMap.Current.ShowMessage(format);
                }
                else
                {
                    var dispelMagicFailed = AbilityRules.TranslateRStr(GameText.DispelMagicFailedRStr);
                    AoWHSMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            Result = Enchantments.GetCount() > 0;
            Enchantments?.Dispose();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // UnitSpells.pas:1013
    public int GetDispelMana(Troop Source) => Source.GetAbLevel(52) * 15 + 25;
}
