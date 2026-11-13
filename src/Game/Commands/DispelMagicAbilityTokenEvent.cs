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
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref SourceId, 25);
        stream.Byte(ref SourcePlayer, 26);
        stream.Int32(ref TargetId, 27);
        stream.Byte(ref TargetPlayer, 28);
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
        enchantments = new EnchantmentList(null) { Holder = CreationHolder() };
        source = StrategicMap.Current.UnitControl.FindUnit(SourceId);
        target = StrategicMap.Current.UnitControl.FindUnit(TargetId);
        ability = (DispelMagicAbility)GamePack.Current.AbilityControl.GetAbility(60);
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
            mana = ((DispelMagicAbility)GamePack.Current.AbilityControl.GetAbility(60)).GetDispelMana(source);
            for (int I = enchantments.GetCount() - 1; I >= 0; I--)
            {
                chance = enchantments.GetEnchantments(I).DispelChance(mana);
                if (chance > StrategicMap.Current.Random(100))
                {
                    count += 1;
                    enchantments.GetEnchantments(I).Dispel();
                }
            }

            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                if (count > 0)
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.SpellsSuccessfullyDispelledFormat), [IntToStr(count)]);
                    StrategicMap.Current.ShowMessage(format);
                }
                else
                {
                    var dispelMagicFailed = AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicFailedText);
                    StrategicMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            ability.SetEnabled(source, false);
            target.GetLocation(out X, out Y, out L);
            StrategicMap.Current.ValidateObservation((sbyte)SourcePlayer, X, Y, L, 0);
            if (StrategicMap.Current.Visible(X, Y, L, 0))
            {
                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                animation.SetImageSequence(ability.ImageSequences.GetImageSequence(0));
                StrategicMap.Current.PlaceHex(animation, X, Y, L);
            }

            if ((byte)StrategicMap.Current.SeatedPlayer == SourcePlayer || StrategicMap.Current.Watching(X, Y, L, 0))
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
