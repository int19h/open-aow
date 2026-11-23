#nullable disable warnings
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Commands;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Spells;

// enchantment ID, dispel chance. inherited spell ID, source unit ID, references
public class DisjunctionSpellCaster(CustomNode? owner) : SpellCaster(owner)
{

    public override SpellCasterView InputView => SpellCasterView.Disjunction;

    public int EnchantmentId { get; set; }
    public int DispelChance { get; private set; } = 50;

    // GlobalSpells.pas:868
    public override bool CanCast(ref string reason)
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        bool result = base.CanCast(ref reason);
        Troop? source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (result && SelectedEnchantment() is not null)
        {
            if (!StrategicMap.Current.GlobalMagicControl.ActiveEnchantments.Contains(GlobalEnchantmentTypes.SpellWard) || SelectedEnchantment().EnchantmentType == GlobalEnchantmentType.SpellWard)
            {
                if (!StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
                {
                    if (source is Hero hero)
                    {
                        Spell? spell = GamePack.Current.SpellControl[51];
                        int mana = GetMana();
                        if (StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + hero.CastingProgress >= spell.BaseManaCost + mana)
                        {
                            return true;
                        }
                        else
                        {
                            result = false;
                            reason = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.NotEnoughManaText);
                        }
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                result = false;
                reason = AbilityRules.TranslateResourceString(GetEngine(), S: CannotDispelWhenSpellWardIsActiveText);
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // GlobalSpells.pas:901
    public override bool Cast()
    {
        string reason = "";
        if (CanCast(ref reason))
        {
            Troop? source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
            if (!StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
            {
                DisjunctionTokenEvent eventValue = new(null) { Holder = CreationHolder() };
                Spell? spell = GamePack.Current.SpellControl[51];
                eventValue.Initialize(CasterSpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: spell.BaseManaCost);
                eventValue.EnchantmentId = EnchantmentId;
                eventValue.DispelChance = DispelChance;
                StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
                _ = eventValue.Release();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            StrategicMap.Current.ShowMessage(reason);
        }

        return false;
    }

    // GlobalSpells.pas:812
    public void SetEnchantmentId(int value) => EnchantmentId = value;

    // GlobalSpells.pas:817
    public GlobalEnchantment? SelectedEnchantment()
    {
        int index = StrategicMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        return index != -1 ? StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index) : null;
    }

    // GlobalSpells.pas:828
    public int GetMana()
    {
        GlobalEnchantment? enchantment = SelectedEnchantment();
        return enchantment is not null ? Round((double)(enchantment.DispelMana * 0.02) * DispelChance) : -1;
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
    public int DispelChanceMax => 75;

    // GlobalSpells.pas:851
    public bool GetEnoughMana()
    {
        // FPlayer is on TAbstractUnit. FCastingProgress is on THero, so only that read tests for a hero.
        Troop? source = StrategicMap.Current.UnitControl.FindUnit(CasterSourceId);
        if (SelectedEnchantment() is not null)
        {
            if (!StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.GetCheatEnabled())
            {
                if (source is Hero hero)
                {
                    int mana2 = StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + hero.CastingProgress;
                    Spell? spell = GamePack.Current.SpellControl[51];
                    int mana = GetMana();
                    if (mana2 >= spell.BaseManaCost + mana)
                    {
                        return true;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        return false;
    }

    internal static string CannotDispelWhenSpellWardIsActiveText { get; } = "Cannot dispel when Spell Ward is active";

}
