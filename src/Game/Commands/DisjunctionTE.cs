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
public partial class DisjunctionTE : SpellTE
{
    public DisjunctionTE(ECustomNode owner) : base(owner)
    { }

    // GlobalSpells.pas:745
    public new const int ClassID = 135961;

    public int FDispelChance;
    public int FEnchantmentID;

    // GlobalSpells.pas:750
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FEnchantmentID, 20);
        Stream.rwInteger(ref FDispelChance, 21);
    }

    // GlobalSpells.pas:757
    public override bool Validate(Hero source)
    {
        int Index;
        int Mana;
        int RequiredMana;
        bool Result = base.Validate(source);
        Index = AoWHSMap.Current.FGlobalMagicControl.IndexOfID(FEnchantmentID);
        Result = Result && Index != -1;
        if (Result)
        {
            var globalEnchantment = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
            Mana = AoWHSMap.Current.FPlayers.GetPlayers(source.FPlayer).FMagicControl.FMana + source.FCastingProgress;
            RequiredMana = Round((double)((double)globalEnchantment.FDispelMana * 0.02) * FDispelChance) + FEventMana;
            Result = RequiredMana <= Mana;
        }

        return Result;
    }

    // GlobalSpells.pas:772
    public override bool Process(AbstractUnit source, int value)
    {
        GlobalEnchantment Enchantment;
        int Index;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TwinkleAnimation Twinkle;
        int Mana;
        base.Process(source, value);
        Index = AoWHSMap.Current.FGlobalMagicControl.IndexOfID(FEnchantmentID);
        Enchantment = AoWHSMap.Current.FGlobalMagicControl.FEnchantments.GetEnchantments(Index);
        source.GetLocation(ref X, ref Y, ref Layer);
        AoWHSMap.Current.ValidateObservation(source.FPlayer, X, Y, Layer, 0);
        Twinkle = new TwinkleAnimation(null);
        Twinkle.Setup(AoWHSSet.Current.FSpellControl.GetSpell(51).FCastImages.ImageSequences[0], Set<TwinkleModeElement>.Of(TwinkleModeElement.Expand, TwinkleModeElement.Explode), FEventMana + 50, 192, 50, 30);
        AoWHSMap.Current.PlaceHX(Twinkle, X, Y, Layer);
        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).FCastSounds.Play(0);
        Mana = Round((double)((double)Enchantment.FDispelMana * 0.02) * FDispelChance);
        var player = AoWHSMap.Current.FPlayers.GetPlayers(source.FPlayer);
        AoWHSMap.Current.FPlayers.GetPlayers(source.FPlayer).FMagicControl.SetMana(player.FMagicControl.FMana - Mana);
        if (AoWHSMap.Current.Random(100) < FDispelChance)
        {
            if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer)
            {
                var name = Enchantment.GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateRStr(XSuccessfullyDispelledRStr), [name]);
                AoWHSMap.Current.ShowMessage(format);
            }

            AoWHSMap.Current.FGlobalMagicControl.ExecuteDispelEnchantment(source.FPlayer, FEnchantmentID);
        }
        else
        {
            if (AoWHSMap.Current.FSeatedPlayer == source.FPlayer)
            {
                // { Original formats this message with an unused enchantment-name argument. }
                var name = Enchantment.GetName();
                var format = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.DispelMagicFailedRStr), [name]);
                AoWHSMap.Current.ShowMessage(format);
            }
        }

        AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
        return false;
    }

    private static readonly string XSuccessfullyDispelledRStr = "%s successfully dispelled";

    // NetworkE.pas:230

    // NetworkE.pas:230

}
