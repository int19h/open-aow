#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw.Animations;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (unit IDs, players)
public class HealingTokenEvent(CustomNode? owner) : TokenEvent(owner)
{
    public new const int ClassId = 131_826;

    public int SourceId { get; set; }
    public PlayerId? SourcePlayer { get; set; } = PlayerId.Independent;
    public int TargetId { get; set; }
    public PlayerId? TargetPlayer { get; set; } = PlayerId.Independent;

    // AoWAb.PAS:3853
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        SourceId = persister.Int32("SourceId", value: SourceId);
        SourcePlayer = persister.OptionalPlayer("SourcePlayer", value: SourcePlayer, width: IdWidth.Byte, fallback: 0);
        TargetId = persister.Int32("TargetId", value: TargetId);
        TargetPlayer = persister.OptionalPlayer("TargetPlayer", value: TargetPlayer, width: IdWidth.Byte, fallback: 0);
    }

    // AoWAb.PAS:3861
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        StrategicMap strategicMap = GetEngine().StrategicMap;
        Troop? source = strategicMap.UnitControl.FindUnit(SourceId);
        Troop? target = strategicMap.UnitControl.FindUnit(TargetId);
        if (source is not null && (byte)PlayerIdExtensions.ToOriginal(source.Player) == (byte)PlayerIdExtensions.ToOriginal(SourcePlayer) && target is not null && (byte)PlayerIdExtensions.ToOriginal(target.Player) == (byte)PlayerIdExtensions.ToOriginal(TargetPlayer))
        {
            HealingAbility ability = GetEngine().GamePack.AbilityControl.GetAbility<HealingAbility>(47);
            if (ability.GetEnabled(source))
            {
                ability.SetEnabled(source, enabled: false);
                _ = ability.HealUnit(target);
                MapCoordinate coordinate9 = target.GetLocation();
                int x = coordinate9.X;
                int y = coordinate9.Y;
                int l = coordinate9.MapLevel;
                _ = GetEngine().StrategicMap.ValidateObservation(PlayerId.FromOriginal((sbyte)PlayerIdExtensions.ToOriginal(SourcePlayer)), x, y, l, radius: 0);
                StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                if (engineStrategicMap.Visible(x, y, l, radius: 0))
                {
                    ImageSequenceAnimation animation = new(null) { Holder = CreationHolder() };
                    animation.SetImageSequence(ability.ImageSequences.GetImageSequence(0));
                    _ = engineStrategicMap.PlaceHex(animation, x, y, l);
                }

                StrategicMap toOriginalStrategicMap = GetEngine().StrategicMap;
                if ((byte)PlayerIdExtensions.ToOriginal(toOriginalStrategicMap.SeatedPlayer) == (byte)PlayerIdExtensions.ToOriginal(SourcePlayer) || toOriginalStrategicMap.Watching(x, y, l, radius: 0))
                {
                    _ = ability.SfxLibrary.Play(0);
                }
            }

            return ExecuteResult.Done;
        }
        else
        {
            return ExecuteResult.DoneHalt;
        }
    }
}
