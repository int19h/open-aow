// Translated from FlagEvents.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World;

// { ot=1 }
// FTriggerMode, FTriggerPlayers (player 1 is bit 0)
public partial class FlagEvent : MapObject
{
    public FlagEvent(ECustomNode owner) : base(owner)
    { }

    public TriggerMode FTriggerMode;
    public int FTriggerPlayers;

    // FlagEvents.pas:98
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTriggerPlayers, 30);
        Stream.rwData(Bytes.Of(ref FTriggerMode, 1), 1, 31);
    }

    // FlagEvents.pas:157
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001987 && FTriggerMode == TriggerMode.NewTurn)
        {
            UpdateNewTurnTrigger((sbyte)Msg.Payload.Int);
        }
    }

    // FlagEvents.pas:164
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            base.Show(XhpSurf, YhpSurf, SurfaceDesc);
        }
    }

    // FlagEvents.pas:150
    public override void MapFieldMsgProc(ref MapFieldMsg Msg)
    {
        base.MapFieldMsgProc(ref Msg);
        if (Msg.MsgID == 4365 && FTriggerMode == TriggerMode.Instant)
        {
            UpdateInstantTrigger();
        }
    }

    // FlagEvents.pas:110
    public override bool CanSelect(int Xhp, int Yhp) => AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting);

    // FlagEvents.pas:105
    public override bool CanPlace(MapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

    // FlagEvents.pas:129
    public virtual bool Trigger(sbyte Player) => true;

    // FlagEvents.pas:115
    public bool GetTriggerPlayer(sbyte Player) => Player > 0 && (FTriggerPlayers & (1 << (Player - 1))) != 0;

    // FlagEvents.pas:120
    public void SetTriggerPlayer(sbyte Player, bool Value)
    {
        if (Player > 0)
        {
            if (!Value)
            {
                FTriggerPlayers = FTriggerPlayers & ~(1 << (Player - 1));
            }
            else
            {
                FTriggerPlayers = FTriggerPlayers | (1 << (Player - 1));
            }
        }
    }

    // FlagEvents.pas:134
    public void UpdateInstantTrigger()
    {
        sbyte Player;
        Player = ((AoWMapField)Owner).GetPlayer();
        if (Player != -1 && GetTriggerPlayer(Player) && Trigger(Player))
        {
            SetTriggerPlayer(Player, false);
        }
    }

    // FlagEvents.pas:143
    public void UpdateNewTurnTrigger(sbyte Player)
    {
        if (Player == ((AoWMapField)Owner).GetPlayer() && GetTriggerPlayer(Player) && Trigger(Player))
        {
            SetTriggerPlayer(Player, false);
        }
    }

    public enum TriggerMode : byte
    {
        Instant = 0, NewTurn = 1
    }
}
