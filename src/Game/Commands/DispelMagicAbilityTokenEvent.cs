// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// (source unit ID/player, target unit ID/player)
public partial class DispelMagicAbilityTokenEvent : TokenEvent
{
    public DispelMagicAbilityTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 131827;

    public int FSourceID;
    public byte FSourcePlayer;
    public int FTargetID;
    public byte FTargetPlayer;

    // AoWAb.PAS:4337
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FSourceID, 25);
        Stream.rwByte(ref FSourcePlayer, 26);
        Stream.rwInteger(ref FTargetID, 27);
        Stream.rwByte(ref FTargetPlayer, 28);
    }

    // AoWAb.PAS:4345
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        ExecuteResult Result;
        int Count;
        int Mana;
        DispelMagicAbility Ability;
        EnchantmentList Enchantments;
        Troop Source;
        Troop Target;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool Success;
        int Chance;
        ImageSequenceAnimation Anim;
        base.Execute(TE);
        Enchantments = new EnchantmentList(null);
        Source = AoWHSMap.Current.FUnitControl.FindUnit(FSourceID);
        Target = AoWHSMap.Current.FUnitControl.FindUnit(FTargetID);
        Ability = (DispelMagicAbility)AoWHSSet.Current.FAbilityControl.GetAbility(60);
        Success = Source != null && (byte)Source.FPlayer == FSourcePlayer && Target != null && (byte)Target.FPlayer == FTargetPlayer;
        Success = Success && Ability.GetEnabled(Source);
        if (Success)
        {
            Target.ListEnchantments(Enchantments);
            Success = Enchantments.GetCount() > 0;
        }

        if (Success)
        {
            Count = 0;
            Mana = ((DispelMagicAbility)AoWHSSet.Current.FAbilityControl.GetAbility(60)).GetDispelMana(Source);
            for (int I = Enchantments.GetCount() - 1; I >= 0; I--)
            {
                Chance = Enchantments.GetEnchantments(I).DispelChance(Mana);
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
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XSpellsSuccessfullyDispelledRStr), [IntToStr(Count)]);
                    AoWHSMap.Current.ShowMessage(format);
                }
                else
                {
                    var dispelMagicFailed = AbilityRules.TranslateRStr(GameText.DispelMagicFailedRStr);
                    AoWHSMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            Ability.SetEnabled(Source, false);
            Target.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.Current.ValidateObservation((sbyte)FSourcePlayer, X, Y, L, 0);
            if (AoWHSMap.Current.Visible(X, Y, L, 0))
            {
                Anim = new ImageSequenceAnimation(null);
                Anim.SetImageSequence(Ability.FImageSequences.GetImageSequence(0));
                AoWHSMap.Current.PlaceHX(Anim, X, Y, L);
            }

            if ((byte)AoWHSMap.Current.FSeatedPlayer == FSourcePlayer || AoWHSMap.Current.Watching(X, Y, L, 0))
            {
                Ability.FSFXLibrary.Play(0);
            }

            Result = ExecuteResult.Done;
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        Enchantments?.Dispose();
        return Result;
    }

    // NetworkE.pas:230

}
