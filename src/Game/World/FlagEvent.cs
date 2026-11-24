#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.World.Map;

namespace Aow.Game.World;

// { ot=1 }
// TriggerMode, TriggerPlayers (player 1 is bit 0)
public class FlagEvent(CustomNode owner) : MapObject(owner)
{
    public FlagEventTriggerMode TriggerMode { get; private set; }

    private int _triggerPlayers;

    // FlagEvents.pas:98
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _triggerPlayers = persister.Int32("TriggerPlayers", value: _triggerPlayers);
        TriggerMode = persister.Enum("TriggerMode", value: TriggerMode);
    }

    // FlagEvents.pas:157
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001987 && TriggerMode == FlagEventTriggerMode.NewTurn)
        {
            UpdateNewTurnTrigger(PlayerId.FromOriginal((sbyte)message.Payload.Int));
        }
    }

    // FlagEvents.pas:164
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            base.Show(pixelXSurface, pixelYSurface, lockedSurface);
        }
    }

    // FlagEvents.pas:150
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4365 && TriggerMode == FlagEventTriggerMode.Instant)
        {
            UpdateInstantTrigger();
        }
    }

    // FlagEvents.pas:110
    public override bool CanSelect(int pixelX, int pixelY) => StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting);

    // FlagEvents.pas:105
    public override bool CanPlace(MapContainer mapContainer, int hexX, int hexY, int hexLevel) => true;

    // FlagEvents.pas:129
    public virtual bool Trigger(PlayerId? player) => true;

    // FlagEvents.pas:115
    public bool GetTriggerPlayer(PlayerId? player) => PlayerIdExtensions.ToOriginal(player) > 0 && (_triggerPlayers & (1 << (PlayerIdExtensions.ToOriginal(player) - 1))) != 0;

    // FlagEvents.pas:120
    public void SetTriggerPlayer(PlayerId? player, bool value)
    {
        if (PlayerIdExtensions.ToOriginal(player) > 0)
        {
            _triggerPlayers = !value ? _triggerPlayers & ~(1 << (PlayerIdExtensions.ToOriginal(player) - 1)) : _triggerPlayers | (1 << (PlayerIdExtensions.ToOriginal(player) - 1));
        }
    }

    // FlagEvents.pas:134
    public void UpdateInstantTrigger()
    {
        sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(Owner.GetPlayer());
        if (player != -1 && GetTriggerPlayer(PlayerId.FromOriginal(player)) && Trigger(PlayerId.FromOriginal(player)))
        {
            SetTriggerPlayer(PlayerId.FromOriginal(player), value: false);
        }
    }

    // FlagEvents.pas:143
    public void UpdateNewTurnTrigger(PlayerId? player)
    {
        if (player == Owner.GetPlayer() && GetTriggerPlayer(player) && Trigger(player))
        {
            SetTriggerPlayer(player, value: false);
        }
    }
}

public enum FlagEventTriggerMode : byte
{
    Instant = 0, NewTurn = 1
}
