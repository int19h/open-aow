// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (unit IDs, players)
public partial class HealingTokenEvent : TokenEvent
{
    public HealingTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131826;

    public int SourceId;
    public byte SourcePlayer;
    public int TargetId;
    public byte TargetPlayer;

    // AoWAb.PAS:3853
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref SourceId, 25);
        stream.Byte(ref SourcePlayer, 26);
        stream.Int32(ref TargetId, 27);
        stream.Byte(ref TargetPlayer, 28);
    }

    // AoWAb.PAS:3861
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        Troop source;
        Troop target;
        HealingAbility ability;
        ImageSequenceAnimation animation;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        base.Execute(tokenEvent);
        source = StrategicMap.Current.UnitControl.FindUnit(SourceId);
        target = StrategicMap.Current.UnitControl.FindUnit(TargetId);
        if (source != null && (byte)source.Player == SourcePlayer && target != null && (byte)target.Player == TargetPlayer)
        {
            ability = (HealingAbility)GamePack.Current.AbilityControl.GetAbility(47);
            if (ability.GetEnabled(source))
            {
                ability.SetEnabled(source, false);
                ability.HealUnit(target);
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
