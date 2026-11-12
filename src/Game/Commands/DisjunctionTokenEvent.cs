// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
    public DisjunctionTokenEvent(ECustomNode owner) : base(owner)
    { }

    // GlobalSpells.pas:745
    public new const int ClassId = 135961;

    public int DispelChance;
    public int EnchantmentId;

    // GlobalSpells.pas:750
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref EnchantmentId, 20);
        stream.rwInteger(ref DispelChance, 21);
    }

    // GlobalSpells.pas:757
    public override bool Validate(Hero source)
    {
        int index;
        int mana;
        int requiredMana;
        bool result = base.Validate(source);
        index = AoWHSMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        result = result && index != -1;
        if (result)
        {
            var globalEnchantment = AoWHSMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index);
            mana = AoWHSMap.Current.Players.GetPlayers(source.Player).MagicControl.Mana + source.CastingProgress;
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
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        TwinkleAnimation twinkle;
        int mana;
        base.Process(source, value);
        index = AoWHSMap.Current.GlobalMagicControl.IndexOfId(EnchantmentId);
        enchantment = AoWHSMap.Current.GlobalMagicControl.Enchantments.GetEnchantments(index);
        source.GetLocation(ref X, ref Y, ref layer);
        AoWHSMap.Current.ValidateObservation(source.Player, X, Y, layer, 0);
        twinkle = new TwinkleAnimation(null);
        twinkle.Setup(AoWHSSet.Current.SpellControl.GetSpell(51).CastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand, TwinkleModeElement.Explode), EventMana + 50, 192, 50, 30);
        AoWHSMap.Current.PlaceHex(twinkle, X, Y, layer);
        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastSounds.Play(0);
        mana = Round((double)((double)enchantment.DispelMana * 0.02) * DispelChance);
        var player = AoWHSMap.Current.Players.GetPlayers(source.Player);
        AoWHSMap.Current.Players.GetPlayers(source.Player).MagicControl.SetMana(player.MagicControl.Mana - mana);
        if (AoWHSMap.Current.Random(100) < DispelChance)
        {
            if (AoWHSMap.Current.SeatedPlayer == source.Player)
            {
                var name = enchantment.GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateResourceString(s_successfullyDispelledFormat), [name]);
                AoWHSMap.Current.ShowMessage(format);
            }

            AoWHSMap.Current.GlobalMagicControl.ExecuteDispelEnchantment(source.Player, EnchantmentId);
        }
        else
        {
            if (AoWHSMap.Current.SeatedPlayer == source.Player)
            {
                // { Original formats this message with an unused enchantment-name argument. }
                var name = enchantment.GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.s_dispelMagicFailedText), [name]);
                AoWHSMap.Current.ShowMessage(format);
            }
        }

        AoWHSSet.Current.SpellControl.GetSpell(EventSpellId).CastingDone(source, EventMana);
        return false;
    }

    private static readonly string s_successfullyDispelledFormat = "%s successfully dispelled";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
