// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// enchantment ID, dispel chance. inherited spell ID, source unit ID, references
public partial class DisjunctionSpellCaster : SpellCaster
{
    public int FEnchantmentID;
    public int FDispelChance;

    // GlobalSpells.pas:806
    public DisjunctionSpellCaster(ECustomNode owner) : base(owner)
    {
        FDispelChance = 50;
    }

    // GlobalSpells.pas:868
    public override bool CanCast(ref string Reason)
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        AbstractUnit Source;
        bool Result = base.CanCast(ref Reason);
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
        if (Result && SelectedEnchantment() != null)
        {
            if (!AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard) || SelectedEnchantment().FEnchantmentType == GlobalEnchantmentType.SpellWard)
            {
                if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
                {
                    if (Source is Hero hero)
                    {
                        var spell = AoWHSSet.Current.FSpellControl.GetSpell(51);
                        var mana = GetMana();
                        if (AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.FMana + hero.FCastingProgress >= spell.FBaseManaCost + mana)
                        {
                            Result = true;
                        }
                        else
                        {
                            Result = false;
                            Reason = AbilityRules.TranslateRStr(GameText.NotEnoughManaRStr);
                        }
                    }
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = false;
                Reason = AbilityRules.TranslateRStr(CannotDispelWhenSpellWardIsActiveRStr);
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalSpells.pas:901
    public override bool Cast()
    {
        bool Result;
        string Reason = "";
        AbstractUnit Source;
        DisjunctionTE Event;
        if (CanCast(ref Reason))
        {
            Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
            if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
            {
                Event = new DisjunctionTE(null);
                var spell = AoWHSSet.Current.FSpellControl.GetSpell(51);
                Event.Initialize(FCasterSpellID, (byte)Source.FPlayer, Source.FUnitID, spell.FBaseManaCost);
                Event.FEnchantmentID = FEnchantmentID;
                Event.FDispelChance = FDispelChance;
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Event.Release();
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
            AoWHSMap.Current.ShowMessage(Reason);
        }

        return Result;
    }

    // GlobalSpells.pas:812
    public void SetEnchantmentID(int Value)
    {
        FEnchantmentID = Value;
    }

    // GlobalSpells.pas:817
    public GlobalEnchantment SelectedEnchantment()
    {
        GlobalEnchantment Result;
        int Index;
        Index = AoWHSMap.Current.FGlobalMagicControl.IndexOfID(FEnchantmentID);
        if (Index != -1)
        {
            Result = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // GlobalSpells.pas:828
    public int GetMana()
    {
        int Result;
        GlobalEnchantment Enchantment;
        Enchantment = SelectedEnchantment();
        if (Enchantment != null)
        {
            Result = Round((double)((double)Enchantment.FDispelMana * 0.02) * FDispelChance);
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // GlobalSpells.pas:839
    public void SetDispelChance(int Value)
    {
        FDispelChance = Value;
        if (FDispelChance < 1)
        {
            FDispelChance = 1;
        }

        if (FDispelChance > 75)
        {
            FDispelChance = 75;
        }
    }

    // GlobalSpells.pas:846
    public int GetDispelChanceMax() => 75;

    // GlobalSpells.pas:851
    public bool GetEnoughMana()
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        AbstractUnit Source;
        int Mana;
        bool Result = false;
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FCasterSourceID);
        if (SelectedEnchantment() != null)
        {
            if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.GetCheatEnabled())
            {
                if (Source is Hero hero)
                {
                    Mana = AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FMagicControl.FMana + hero.FCastingProgress;
                    var spell = AoWHSSet.Current.FSpellControl.GetSpell(51);
                    var mana = GetMana();
                    if (Mana >= spell.FBaseManaCost + mana)
                    {
                        Result = true;
                    }
                }
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    internal static readonly string CannotDispelWhenSpellWardIsActiveRStr = "Cannot dispel when Spell Ward is active";

}
