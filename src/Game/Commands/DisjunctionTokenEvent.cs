#nullable disable warnings
// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Commands;

// dispel chance, enchantment ID
public class DisjunctionTokenEvent(CustomNode? owner) : SpellTokenEvent(owner)
{

    // GlobalSpells.pas:745
    public new const int ClassId = 135_961;

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
        StrategicMap strategicMap = GetEngine().StrategicMap;
        int index = strategicMap.GlobalMagicControl.IndexOfId(EnchantmentId);
        result = result && index != -1;
        if (result)
        {
            GlobalEnchantment globalEnchantment = strategicMap.GlobalMagicControl.Enchantments.GetEnchantments(index);
            int mana = strategicMap.Players.GetPlayers(source.Player).MagicControl.Mana + source.CastingProgress;
            int requiredMana = Round((double)(globalEnchantment.DispelMana * 0.02) * DispelChance) + EventMana;
            return requiredMana <= mana;
        }

        return result;
    }

    // GlobalSpells.pas:772
    public override bool Process(Troop source, int value)
    {
        _ = base.Process(source, value);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        int index = strategicMap.GlobalMagicControl.IndexOfId(EnchantmentId);
        GlobalEnchantment enchantment = strategicMap.GlobalMagicControl.Enchantments.GetEnchantments(index);
        MapCoordinate coordinate7 = source.GetLocation();
        int x = coordinate7.X;
        int y = coordinate7.Y;
        int layer = coordinate7.MapLevel;
        _ = strategicMap.ValidateObservation(source.Player, x, y, layer, radius: 0);
        TwinkleAnimation twinkle = new(null) { Holder = CreationHolder() };
        twinkle.Setup(GetEngine().GamePack.SpellControl[51].CastImages.ImageSequences[0], mode: TwinkleModes.Expand | TwinkleModes.Explode, count: EventMana + 50, radius: 192, duration: 50, startDelay: 30);
        _ = GetEngine().StrategicMap.PlaceHex(twinkle, x, y, L: layer);
        _ = GetEngine().GamePack.SpellControl[EventSpellId].CastSounds.Play(0);
        int mana = Round((double)(enchantment.DispelMana * 0.02) * DispelChance);
        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
        Player? player = engineStrategicMap.Players.GetPlayers(source.Player);
        engineStrategicMap.Players.GetPlayers(source.Player).MagicControl.SetMana(player.MagicControl.Mana - mana);
        if (GetEngine().StrategicMap.Random(100) < DispelChance)
        {
            if (GetEngine().StrategicMap.SeatedPlayer == source.Player)
            {
                string name = enchantment.GetName();
                string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_successfullyDispelledFormat), args: [name]);
                GetEngine().StrategicMap.ShowMessage(format);
            }

            _ = GetEngine().StrategicMap.GlobalMagicControl.ExecuteDispelEnchantment(source.Player, EnchantmentId);
        }
        else
        {
            if (GetEngine().StrategicMap.SeatedPlayer == source.Player)
            {
                // { Original formats this message with an unused enchantment-name argument. }
                string name = enchantment.GetName();
                string format = Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DispelMagicFailedText), args: [name]);
                GetEngine().StrategicMap.ShowMessage(format);
            }
        }

        GetEngine().GamePack.SpellControl[EventSpellId].CastingDone(source, mana: EventMana);
        return false;
    }

    private static readonly string s_successfullyDispelledFormat = "%s successfully dispelled";
}
