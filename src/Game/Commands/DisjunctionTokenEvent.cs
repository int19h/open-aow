#nullable disable
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Commands;

// dispel chance, enchantment ID
public class DisjunctionTokenEvent(CustomNode owner) : SpellTokenEvent(owner)
{

    // GlobalSpells.pas:745
    public new const int ClassId = 135961;

    public int DispelChance { get; set; }
    public int EnchantmentId { get; set; }

    // GlobalSpells.pas:750
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        EnchantmentId = persister.Int32("EnchantmentId", value: EnchantmentId);
        DispelChance = persister.Int32("DispelChance", value: DispelChance);
    }

    // GlobalSpells.pas:757
    public override bool Validate(Hero source)
    {
        bool result = base.Validate(source);
        int index = StrategicMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        result = result && index != -1;
        if (result)
        {
            GlobalEnchantment globalEnchantment = StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index);
            int mana = StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + source.CastingProgress;
            int requiredMana = Round((double)(globalEnchantment.DispelMana * 0.02) * DispelChance) + EventMana;
            return requiredMana <= mana;
        }

        return result;
    }

    // GlobalSpells.pas:772
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        int index = StrategicMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        GlobalEnchantment enchantment = StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index);
        MapCoordinate coordinate7 = source.GetLocation();
        sbyte x = coordinate7.X;
        sbyte y = coordinate7.Y;
        sbyte layer = coordinate7.MapLevel;
        _ = StrategicMap.Current.ValidateObservation(source.Player, x, y, layer, 0);
        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
        twinkle.Setup(GamePack.Current.SpellControl.GetSpell(51).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand, TwinkleModeElement.Explode), EventMana + 50, 192, 50, 30);
        _ = StrategicMap.Current.PlaceHex(twinkle, x, y, layer);
        _ = GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
        int mana = Round((double)(enchantment.DispelMana * 0.02) * DispelChance);
        Player player = StrategicMap.Current.Players.GetPlayers(source.Player);
        StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.SetMana(player.MagicControl.Mana - mana);
        if (StrategicMap.Current.Random(100) < DispelChance)
        {
            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                string name = enchantment.GetName();
                string format = Format(AbilityRules.TranslateResourceString(GetEngine(), s_successfullyDispelledFormat), [name]);
                StrategicMap.Current.ShowMessage(format);
            }

            _ = StrategicMap.Current.GlobalMagicControl.ExecuteDispelEnchantment(source.Player, EnchantmentId);
        }
        else
        {
            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                // { Original formats this message with an unused enchantment-name argument. }
                string name = enchantment.GetName();
                string format = Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicFailedText), [name]);
                StrategicMap.Current.ShowMessage(format);
            }
        }

        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    private static readonly string s_successfullyDispelledFormat = "%s successfully dispelled";
}
