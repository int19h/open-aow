#nullable disable
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// dispel chance, enchantment ID
public partial class DisjunctionTokenEvent : SpellTokenEvent
{
    public DisjunctionTokenEvent(CustomNode owner) : base(owner)
    { }

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
        int index;
        int mana;
        int requiredMana;
        bool result = base.Validate(source);
        index = StrategicMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        result = result && index != -1;
        if (result)
        {
            var globalEnchantment = StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index);
            mana = StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + source.CastingProgress;
            requiredMana = Round((double)((double)globalEnchantment.DispelMana * 0.02) * DispelChance) + EventMana;
            result = requiredMana <= mana;
        }

        return result;
    }

    // GlobalSpells.pas:772
    public override bool Process(Troop source, int value)
    {
        GlobalEnchantment enchantment;
        int index;
        sbyte x = default;
        sbyte y = default;
        sbyte layer = default;
        TwinkleAnimation twinkle;
        int mana;
        base.Process(source, value);
        index = StrategicMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        enchantment = StrategicMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index);
        source.GetLocation(out x, out y, out layer);
        StrategicMap.Current.ValidateObservation(source.Player, x, y, layer, 0);
        twinkle = new TwinkleAnimation(null) { Holder = CreationHolder() };
        twinkle.Setup(GamePack.Current.SpellControl.GetSpell(51).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand, TwinkleModeElement.Explode), EventMana + 50, 192, 50, 30);
        StrategicMap.Current.PlaceHex(twinkle, x, y, layer);
        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
        mana = Round((double)((double)enchantment.DispelMana * 0.02) * DispelChance);
        var player = StrategicMap.Current.Players.GetPlayers(source.Player);
        StrategicMap.Current.Players.GetPlayers(source.Player).MagicControl.SetMana(player.MagicControl.Mana - mana);
        if (StrategicMap.Current.Random(100) < DispelChance)
        {
            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                var name = enchantment.GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_successfullyDispelledFormat), [name]);
                StrategicMap.Current.ShowMessage(format);
            }

            StrategicMap.Current.GlobalMagicControl.ExecuteDispelEnchantment(source.Player, EnchantmentId);
        }
        else
        {
            if (StrategicMap.Current.SeatedPlayer == source.Player)
            {
                // { Original formats this message with an unused enchantment-name argument. }
                var name = enchantment.GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicFailedText), [name]);
                StrategicMap.Current.ShowMessage(format);
            }
        }

        GamePack.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    private static readonly string s_successfullyDispelledFormat = "%s successfully dispelled";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
