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

    public new const int ClassId = 131827;

    public int SourceId;
    public byte SourcePlayer;
    public int TargetId;
    public byte TargetPlayer;

    // AoWAb.PAS:4337
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref SourceId, 25);
        stream.rwByte(ref SourcePlayer, 26);
        stream.rwInteger(ref TargetId, 27);
        stream.rwByte(ref TargetPlayer, 28);
    }

    // AoWAb.PAS:4345
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        int count;
        int mana;
        DispelMagicAbility ability;
        EnchantmentList enchantments;
        Troop source;
        Troop target;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        bool success;
        int chance;
        ImageSequenceAnimation animation;
        base.Execute(tokenEvent);
        enchantments = new EnchantmentList(null);
        source = AoWHSMap.Current.UnitControl.FindUnit(SourceId);
        target = AoWHSMap.Current.UnitControl.FindUnit(TargetId);
        ability = (DispelMagicAbility)AoWHSSet.Current.AbilityControl.GetAbility(60);
        success = source != null && (byte)source.Player == SourcePlayer && target != null && (byte)target.Player == TargetPlayer;
        success = success && ability.GetEnabled(source);
        if (success)
        {
            target.ListEnchantments(enchantments);
            success = enchantments.GetCount() > 0;
        }

        if (success)
        {
            count = 0;
            mana = ((DispelMagicAbility)AoWHSSet.Current.AbilityControl.GetAbility(60)).GetDispelMana(source);
            for (int I = enchantments.GetCount() - 1; I >= 0; I--)
            {
                chance = enchantments.GetEnchantments(I).DispelChance(mana);
                if (chance > AoWHSMap.Current.Random(100))
                {
                    count += 1;
                    enchantments.GetEnchantments(I).Dispel();
                }
            }

            if (source.Player == AoWHSMap.Current.SeatedPlayer)
            {
                if (count > 0)
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.SpellsSuccessfullyDispelledFormat), [IntToStr(count)]);
                    AoWHSMap.Current.ShowMessage(format);
                }
                else
                {
                    var dispelMagicFailed = AbilityRules.TranslateResourceString(GameText.DispelMagicFailedText);
                    AoWHSMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            ability.SetEnabled(source, false);
            target.GetLocation(ref X, ref Y, ref L);
            AoWHSMap.Current.ValidateObservation((sbyte)SourcePlayer, X, Y, L, 0);
            if (AoWHSMap.Current.Visible(X, Y, L, 0))
            {
                animation = new ImageSequenceAnimation(null);
                animation.SetImageSequence(ability.ImageSequences.GetImageSequence(0));
                AoWHSMap.Current.PlaceHex(animation, X, Y, L);
            }

            if ((byte)AoWHSMap.Current.SeatedPlayer == SourcePlayer || AoWHSMap.Current.Watching(X, Y, L, 0))
            {
                ability.SfxLibrary.Play(0);
            }

            result = ExecuteResult.Done;
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        enchantments?.Dispose();
        return result;
    }

    // NetworkE.pas:230

}
