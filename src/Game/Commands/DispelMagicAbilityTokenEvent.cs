// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
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
    public DispelMagicAbilityTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131827;

    public int SourceId { get; set; }
    public byte SourcePlayer { get; set; }
    public int TargetId { get; set; }
    public byte TargetPlayer { get; set; }

    // AoWAb.PAS:4337
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceId = persister.Int32("SourceId", value: SourceId);
        SourcePlayer = persister.Byte("SourcePlayer", value: SourcePlayer);
        TargetId = persister.Int32("TargetId", value: TargetId);
        TargetPlayer = persister.Byte("TargetPlayer", value: TargetPlayer);
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
        sbyte x = default;
        sbyte y = default;
        sbyte l = default;
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
            for (int i = enchantments.GetCount() - 1; i >= 0; i--)
            {
                chance = enchantments.GetEnchantments(i).DispelChance(mana);
                if (chance > StrategicMap.Current.Random(100))
                {
                    count += 1;
                    enchantments.GetEnchantments(i).Dispel();
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
            var coordinate8 = target.GetLocation();
            x = coordinate8.X;
            y = coordinate8.Y;
            l = coordinate8.MapLevel;
            StrategicMap.Current.ValidateObservation((sbyte)SourcePlayer, x, y, l, 0);
            if (StrategicMap.Current.Visible(x, y, l, 0))
            {
                animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                animation.SetImageSequence(ability.ImageSequences.GetImageSequence(0));
                StrategicMap.Current.PlaceHex(animation, x, y, l);
            }

            if ((byte)StrategicMap.Current.SeatedPlayer == SourcePlayer || StrategicMap.Current.Watching(x, y, l, 0))
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
