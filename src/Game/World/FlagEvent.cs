// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// { ot=1 }
// TriggerMode, TriggerPlayers (player 1 is bit 0)
public partial class FlagEvent : MapObject
{
    public FlagEvent(ECustomNode owner) : base(owner)
    { }

    public FlagEventTriggerMode TriggerMode;
    private int _triggerPlayers;

    // FlagEvents.pas:98
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref _triggerPlayers, 30);
        stream.Bytes(Bytes.Of(ref TriggerMode, 1), 1, 31);
    }

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
            UpdateNewTurnTrigger((sbyte)message.Payload.Int);
        }
    }

    // FlagEvents.pas:164
    public override void Show(int pixelXSurface, int pixelYSurface, in LockedSurface lockedSurface)
    {
        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
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
    public override bool CanSelect(int pixelX, int pixelY) => StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting);

    // FlagEvents.pas:105
    public override bool CanPlace(MapContainer mapContainer, sbyte hexX, sbyte hexY, sbyte hexLevel) => true;

    // FlagEvents.pas:129
    public virtual bool Trigger(sbyte player) => true;

    // FlagEvents.pas:115
    public bool GetTriggerPlayer(sbyte player) => player > 0 && (_triggerPlayers & (1 << (player - 1))) != 0;

    // FlagEvents.pas:120
    public void SetTriggerPlayer(sbyte player, bool value)
    {
        if (player > 0)
        {
            if (!value)
            {
                _triggerPlayers = _triggerPlayers & ~(1 << (player - 1));
            }
            else
            {
                _triggerPlayers = _triggerPlayers | (1 << (player - 1));
            }
        }
    }

    // FlagEvents.pas:134
    public void UpdateInstantTrigger()
    {
        sbyte player;
        player = Owner.GetPlayer();
        if (player != -1 && GetTriggerPlayer(player) && Trigger(player))
        {
            SetTriggerPlayer(player, false);
        }
    }

    // FlagEvents.pas:143
    public void UpdateNewTurnTrigger(sbyte player)
    {
        if (player == Owner.GetPlayer() && GetTriggerPlayer(player) && Trigger(player))
        {
            SetTriggerPlayer(player, false);
        }
    }
}

public enum FlagEventTriggerMode : byte
{
    Instant = 0, NewTurn = 1
}
