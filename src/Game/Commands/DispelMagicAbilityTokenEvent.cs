#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.World.Map;
using static Aow.Rtl.DelphiConversions;

namespace Aow.Game.Commands;

// (source unit ID/player, target unit ID/player)
public class DispelMagicAbilityTokenEvent(CustomNode owner) : TokenEvent(owner)
{
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
        _ = base.Execute(tokenEvent);
        EnchantmentList enchantments = new(null) { Holder = CreationHolder() };
        Troop source = StrategicMap.Current.UnitControl.FindUnit(SourceId);
        Troop target = StrategicMap.Current.UnitControl.FindUnit(TargetId);
        DispelMagicAbility ability = GamePack.Current.AbilityControl.GetAbility<DispelMagicAbility>(60);
        bool success = source is not null && (byte)source.Player == SourcePlayer && target is not null && (byte)target.Player == TargetPlayer;
        success = success && ability.GetEnabled(source);
        if (success)
        {
            target.ListEnchantments(enchantments);
            success = enchantments.GetCount() > 0;
        }

        ExecuteResult result;
        if (success)
        {
            int count = 0;
            int mana = ability.GetDispelMana(source);
            for (int i = enchantments.GetCount() - 1; i >= 0; i--)
            {
                int chance = enchantments.GetEnchantments(i).DispelChance(mana);
                if (chance > StrategicMap.Current.Random(100))
                {
                    count += 1;
                    _ = enchantments.GetEnchantments(i).Dispel();
                }
            }

            if (source.Player == StrategicMap.Current.SeatedPlayer)
            {
                if (count > 0)
                {
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.SpellsSuccessfullyDispelledFormat), [IntToStr(count)]);
                    StrategicMap.Current.ShowMessage(format);
                }
                else
                {
                    string dispelMagicFailed = AbilityRules.TranslateResourceString(GetEngine(), GameText.DispelMagicFailedText);
                    StrategicMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            ability.SetEnabled(source, false);
            MapCoordinate coordinate8 = target.GetLocation();
            sbyte x = coordinate8.X;
            sbyte y = coordinate8.Y;
            sbyte l = coordinate8.MapLevel;
            _ = StrategicMap.Current.ValidateObservation((sbyte)SourcePlayer, x, y, l, 0);
            if (StrategicMap.Current.Visible(x, y, l, 0))
            {
                ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                animation.SetImageSequence(ability.ImageSequences.GetImageSequence(0));
                _ = StrategicMap.Current.PlaceHex(animation, x, y, l);
            }

            if ((byte)StrategicMap.Current.SeatedPlayer == SourcePlayer || StrategicMap.Current.Watching(x, y, l, 0))
            {
                _ = ability.SfxLibrary.Play(0);
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
}
