#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (unit IDs, players)
public partial class HealingTokenEvent : TokenEvent
{
    public HealingTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131826;

    public int SourceId { get; set; }
    public byte SourcePlayer { get; set; }
    public int TargetId { get; set; }
    public byte TargetPlayer { get; set; }

    // AoWAb.PAS:3853
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceId = persister.Int32("SourceId", value: SourceId);
        SourcePlayer = persister.Byte("SourcePlayer", value: SourcePlayer);
        TargetId = persister.Int32("TargetId", value: TargetId);
        TargetPlayer = persister.Byte("TargetPlayer", value: TargetPlayer);
    }

    // AoWAb.PAS:3861
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        Troop source;
        Troop target;
        HealingAbility ability;
        ImageSequenceAnimation animation;
        _ = base.Execute(tokenEvent);
        source = StrategicMap.Current.UnitControl.FindUnit(SourceId);
        target = StrategicMap.Current.UnitControl.FindUnit(TargetId);
        if (source != null && (byte)source.Player == SourcePlayer && target != null && (byte)target.Player == TargetPlayer)
        {
            ability = GamePack.Current.AbilityControl.GetAbility<HealingAbility>(47);
            if (ability.GetEnabled(source))
            {
                ability.SetEnabled(source, false);
                _ = ability.HealUnit(target);
                MapCoordinate coordinate9 = target.GetLocation();
                sbyte x = coordinate9.X;
                sbyte y = coordinate9.Y;
                sbyte l = coordinate9.MapLevel;
                _ = StrategicMap.Current.ValidateObservation((sbyte)SourcePlayer, x, y, l, 0);
                if (StrategicMap.Current.Visible(x, y, l, 0))
                {
                    animation = new ImageSequenceAnimation(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(ability.ImageSequences.GetImageSequence(0));
                    _ = StrategicMap.Current.PlaceHex(animation, x, y, l);
                }

                if ((byte)StrategicMap.Current.SeatedPlayer == SourcePlayer || StrategicMap.Current.Watching(x, y, l, 0))
                {
                    _ = ability.SfxLibrary.Play(0);
                }
            }

            result = ExecuteResult.Done;
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // NetworkE.pas:230

}
