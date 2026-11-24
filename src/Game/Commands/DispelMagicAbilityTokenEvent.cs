#nullable disable warnings
// Translated from AoWE.pas.
using System.Globalization;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (source unit ID/player, target unit ID/player)
public class DispelMagicAbilityTokenEvent(CustomNode? owner) : TokenEvent(owner)
{
    public new const int ClassId = 131_827;

    public int SourceId { get; set; }
    public PlayerId? SourcePlayer { get; set; } = PlayerId.Independent;
    public int TargetId { get; set; }
    public PlayerId? TargetPlayer { get; set; } = PlayerId.Independent;

    // AoWAb.PAS:4337
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceId = persister.Int32("SourceId", value: SourceId);
        SourcePlayer = persister.OptionalPlayer("SourcePlayer", value: SourcePlayer, width: IdWidth.Byte, fallback: 0);
        TargetId = persister.Int32("TargetId", value: TargetId);
        TargetPlayer = persister.OptionalPlayer("TargetPlayer", value: TargetPlayer, width: IdWidth.Byte, fallback: 0);
    }

    // AoWAb.PAS:4345
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        EnchantmentList enchantments = new(null) { Holder = CreationHolder() };
        Troop? source = StrategicMap.Current.UnitControl.FindUnit(SourceId);
        Troop? target = StrategicMap.Current.UnitControl.FindUnit(TargetId);
        DispelMagicAbility ability = GamePack.Current.AbilityControl.GetAbility<DispelMagicAbility>(60);
        bool success = source is not null && (byte)PlayerIdExtensions.ToOriginal(source.Player) == (byte)PlayerIdExtensions.ToOriginal(SourcePlayer) && target is not null && (byte)PlayerIdExtensions.ToOriginal(target.Player) == (byte)PlayerIdExtensions.ToOriginal(TargetPlayer);
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
                    string format = AbilityRules.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SpellsSuccessfullyDispelledFormat), arguments: [count.ToString(CultureInfo.InvariantCulture)]);
                    StrategicMap.Current.ShowMessage(format);
                }
                else
                {
                    string dispelMagicFailed = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.DispelMagicFailedText);
                    StrategicMap.Current.ShowMessage(dispelMagicFailed);
                }
            }

            ability.SetEnabled(source, enabled: false);
            MapCoordinate coordinate8 = target.GetLocation();
            int x = coordinate8.X;
            int y = coordinate8.Y;
            int l = coordinate8.MapLevel;
            _ = StrategicMap.Current.ValidateObservation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(SourcePlayer)), x, y, l, radius: 0);
            if (StrategicMap.Current.Visible(x, y, l, radius: 0))
            {
                ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                animation.SetImageSequence(ability.ImageSequences.GetImageSequence(0));
                _ = StrategicMap.Current.PlaceHex(animation, x, y, l);
            }

            if ((byte)PlayerIdExtensions.ToOriginal(StrategicMap.Current.SeatedPlayer) == (byte)PlayerIdExtensions.ToOriginal(SourcePlayer) || StrategicMap.Current.Watching(x, y, l, radius: 0))
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
