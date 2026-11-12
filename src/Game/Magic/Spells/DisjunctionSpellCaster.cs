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
    public int EnchantmentId;
    public int DispelChance;

    // GlobalSpells.pas:806
    public DisjunctionSpellCaster(ECustomNode owner) : base(owner)
    {
        DispelChance = 50;
    }

    // GlobalSpells.pas:868
    public override bool CanCast(ref string reason)
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        Troop source;
        bool result = base.CanCast(ref reason);
        source = AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (result && SelectedEnchantment() != null)
        {
            if (!AoWHSMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard) || SelectedEnchantment().EnchantmentType == GlobalEnchantmentType.SpellWard)
            {
                if (!AoWHSMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
                {
                    if (source is Hero hero)
                    {
                        var spell = AoWHSSet.Current.SpellControl.GetSpell(51);
                        var mana = GetMana();
                        if (AoWHSMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + hero.CastingProgress >= spell.BaseManaCost + mana)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                            reason = AbilityRules.TranslateResourceString(GameText.s_notEnoughManaText);
                        }
                    }
                }
                else
                {
                    result = true;
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(s_cannotDispelWhenSpellWardIsActiveText);
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // GlobalSpells.pas:901
    public override bool Cast()
    {
        bool result;
        string reason = "";
        Troop source;
        DisjunctionTokenEvent eventValue;
        if (CanCast(ref reason))
        {
            source = AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId);
            if (!AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
            {
                eventValue = new DisjunctionTokenEvent(null);
                var spell = AoWHSSet.Current.SpellControl.GetSpell(51);
                eventValue.Initialize(CasterSpellId, (byte)source.Player, source.UnitId, spell.BaseManaCost);
                eventValue.EnchantmentId = EnchantmentId;
                eventValue.DispelChance = DispelChance;
                AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                eventValue.Release();
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
            AoWHSMap.Current.ShowMessage(reason);
        }

        return result;
    }

    // GlobalSpells.pas:812
    public void SetEnchantmentId(int value)
    {
        EnchantmentId = value;
    }

    // GlobalSpells.pas:817
    public GlobalEnchantment SelectedEnchantment()
    {
        GlobalEnchantment result;
        int index;
        index = AoWHSMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        if (index != -1)
        {
            result = AoWHSMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // GlobalSpells.pas:828
    public int GetMana()
    {
        int result;
        GlobalEnchantment enchantment;
        enchantment = SelectedEnchantment();
        if (enchantment != null)
        {
            result = Round((double)((double)enchantment.DispelMana * 0.02) * DispelChance);
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // GlobalSpells.pas:839
    public void SetDispelChance(int value)
    {
        DispelChance = value;
        if (DispelChance < 1)
        {
            DispelChance = 1;
        }

        if (DispelChance > 75)
        {
            DispelChance = 75;
        }
    }

    // GlobalSpells.pas:846
    public int GetDispelChanceMax() => 75;

    // GlobalSpells.pas:851
    public bool GetEnoughMana()
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        Troop source;
        int Mana;
        bool result = false;
        source = AoWHSMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (SelectedEnchantment() != null)
        {
            if (!AoWHSMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
            {
                if (source is Hero hero)
                {
                    Mana = AoWHSMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + hero.CastingProgress;
                    var spell = AoWHSSet.Current.SpellControl.GetSpell(51);
                    var mana = GetMana();
                    if (Mana >= spell.BaseManaCost + mana)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    internal static readonly string s_cannotDispelWhenSpellWardIsActiveText = "Cannot dispel when Spell Ward is active";

}
