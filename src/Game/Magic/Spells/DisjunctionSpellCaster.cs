#nullable disable
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Spells;

// enchantment ID, dispel chance. inherited spell ID, source unit ID, references
public partial class DisjunctionSpellCaster : SpellCaster
{
    public DisjunctionSpellCaster(CustomNode owner) : base(owner) => DispelChance = 50;

    public int EnchantmentId { get; set; }
    public int DispelChance { get; private set; }

    // GlobalSpells.pas:868
    public override bool CanCast(ref string reason)
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        Troop source;
        bool result = base.CanCast(ref reason);
        source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (result && SelectedEnchantment() != null)
        {
            if (!StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentType.SpellWard) || SelectedEnchantment().EnchantmentType == GlobalEnchantmentType.SpellWard)
            {
                if (!StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
                {
                    if (source is Hero hero)
                    {
                        Spell spell = GamePack.Current.SpellControl.GetSpell(51);
                        int mana = GetMana();
                        if (StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + hero.CastingProgress >= spell.BaseManaCost + mana)
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                            reason = AbilityRules.TranslateResourceString(GetEngine(), GameText.NotEnoughManaText);
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
                reason = AbilityRules.TranslateResourceString(GetEngine(), CannotDispelWhenSpellWardIsActiveText);
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
            source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
            if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
            {
                eventValue = new DisjunctionTokenEvent(null) { Holder = CreationHolder() };
                Spell spell = GamePack.Current.SpellControl.GetSpell(51);
                eventValue.Initialize(CasterSpellId, (byte)source.Player, source.UnitId, spell.BaseManaCost);
                eventValue.EnchantmentId = EnchantmentId;
                eventValue.DispelChance = DispelChance;
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                _ = eventValue.Release();
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
            StrategicMap.Current.ShowMessage(reason);
        }

        return result;
    }

    // GlobalSpells.pas:812
    public void SetEnchantmentId(int value) => EnchantmentId = value;

    // GlobalSpells.pas:817
    public GlobalEnchantment SelectedEnchantment()
    {
        GlobalEnchantment result;
        int index;
        index = StrategicMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        result = index != -1 ? StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index) : null;

        return result;
    }

    // GlobalSpells.pas:828
    public int GetMana()
    {
        int result;
        GlobalEnchantment enchantment;
        enchantment = SelectedEnchantment();
        result = enchantment != null ? Round((double)(enchantment.DispelMana * 0.02) * DispelChance) : -1;

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
        int mana2;
        bool result = false;
        source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (SelectedEnchantment() != null)
        {
            if (!StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
            {
                if (source is Hero hero)
                {
                    mana2 = StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + hero.CastingProgress;
                    Spell spell = GamePack.Current.SpellControl.GetSpell(51);
                    int mana = GetMana();
                    if (mana2 >= spell.BaseManaCost + mana)
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

    internal static string CannotDispelWhenSpellWardIsActiveText { get; } = "Cannot dispel when Spell Ward is active";

}
