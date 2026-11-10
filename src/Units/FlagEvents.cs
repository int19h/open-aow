// Translated from FlagEvents.pas.
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Game.Magic.Abilities.AoWEUnit;

namespace Aow.Game.World
{

    // { ot=1 }
    // FTriggerMode, FTriggerPlayers (player 1 is bit 0)
    public partial class TFlagEvent : TMapObject
    {
        public TFlagEvent(TECustomNode owner) : base(owner)
        { }

        public TFlagEventTriggerMode FTriggerMode;
        public int FTriggerPlayers;

        // FlagEvents.pas:98
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTriggerPlayers, 30);
            Stream.rwData(Bytes.Of(ref FTriggerMode, 1), 1, 31);
        }

        // FlagEvents.pas:157
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001987 && FTriggerMode == TFlagEventTriggerMode.etmNewTurn)
            {
                UpdateNewTurnTrigger((sbyte)Msg.Payload.Int);
            }
        }

        // FlagEvents.pas:164
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            if (AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // FlagEvents.pas:150
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if (Msg.MsgID == 4365 && FTriggerMode == TFlagEventTriggerMode.etmInstant)
            {
                UpdateInstantTrigger();
            }
        }

        // FlagEvents.pas:110
        public override bool CanSelect(int Xhp, int Yhp) => AoWHSMap.Status.Contains(THSMapStatusElement.msEditting);

        // FlagEvents.pas:105
        public override bool CanPlace(TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx) => true;

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
            Player = ((TAoWMapField)Owner).GetPlayer();
            if (Player != -1 && GetTriggerPlayer(Player) && Trigger(Player))
            {
                SetTriggerPlayer(Player, false);
            }
        }

        // FlagEvents.pas:143
        public void UpdateNewTurnTrigger(sbyte Player)
        {
            if (Player == ((TAoWMapField)Owner).GetPlayer() && GetTriggerPlayer(Player) && Trigger(Player))
            {
                SetTriggerPlayer(Player, false);
            }
        }
    }

    public partial class TFlagEventResource : TMapObjectResource
    {
        public TFlagEventResource(TECustomNode owner) : base(owner)
        {
            Level = 27;
        }

        // FlagEvents.pas:169

        // FlagEvents.pas:175
        public override void SetLevel(byte Value)
        {
            base.SetLevel(27);
        }

        // FlagEvents.pas:46

    }
}

namespace Aow.Game.Events
{

    public partial class TFlagMessageEventLog : THelpMessageEventLog
    {
        public TFlagMessageEventLog(TECustomNode owner) : base(owner)
        { }

    }
}

namespace Aow.Game.World
{

    // FHelpFileName, FMessage
    public partial class TFlagMessageEvent : TFlagEvent
    {
        public TFlagMessageEvent(TECustomNode owner) : base(owner)
        {
            FMessage = new TStringList();
        }

        // FlagEvents.pas:192
        public new const int ClassID = 132461;

        public string FHelpFileName = "";
        public TStringList FMessage;

        // FlagEvents.pas:186
        public override void Dispose()
        {
            FMessage?.Dispose();
            base.Dispose();
        }

        // FlagEvents.pas:197
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 40);
            Stream.rwString(ref FHelpFileName, 41);
        }

        // FlagEvents.pas:180

        // FlagEvents.pas:204
        public override bool Trigger(sbyte Player)
        {
            TFlagMessageEventLog Log;
            if (AoWHSMap.FSeatedPlayer == Player)
            {
                Log = new TFlagMessageEventLog(null);
                Log.FHelpFileName = FHelpFileName;
                var text = FMessage.Text;
                Log.FMessage.Text = text;
                AoWHSMap.TriggerExecuteEventLog(Log, FTriggerMode == TFlagEventTriggerMode.etmInstant, null);
                Log.Release();
            }

            return true;
        }

        // FlagEvents.pas:62

    }

    public partial class TFlagMessageEventResource : TFlagEventResource
    {
        public TFlagMessageEventResource(TECustomNode owner) : base(owner)
        { }

        // FlagEvents.pas:218
        public new const int ClassID = 132462;

        // FlagEvents.pas:228
        public override int ResourceUserClassID() => 132461;

        // FlagEvents.pas:223
        public override string ResourceEditName() => TranslateRStr(FlagMessageEventRStr);

    }

    // FMessage
    public partial class TTutorialMessageEvent : TFlagEvent
    {
        public TTutorialMessageEvent(TECustomNode owner) : base(owner)
        {
            FMessage = new TStringList();
        }

        // FlagEvents.pas:245
        public new const int ClassID = 132459;

        public TStringList FMessage;

        // FlagEvents.pas:239
        public override void Dispose()
        {
            FMessage?.Dispose();
            base.Dispose();
        }

        // FlagEvents.pas:250
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 40);
        }

        // FlagEvents.pas:233

        // FlagEvents.pas:256
        public override bool Trigger(sbyte Player)
        {
            TTutorialMessageEventLog Log;
            if (AoWHSMap.FSeatedPlayer == Player)
            {
                Log = new TTutorialMessageEventLog(null);
                var text = FMessage.Text;
                Log.FMessage.Text = text;
                AoWHSMap.TriggerExecuteEventLog(Log, FTriggerMode == TFlagEventTriggerMode.etmInstant, null);
                Log.Release();
            }

            return true;
        }

        // FlagEvents.pas:81

    }

    public partial class TTutorialMessageEventResource : TFlagEventResource
    {
        public TTutorialMessageEventResource(TECustomNode owner) : base(owner)
        { }

        // FlagEvents.pas:269
        public new const int ClassID = 132460;

        // FlagEvents.pas:279
        public override int ResourceUserClassID() => 132459;

        // FlagEvents.pas:274
        public override string ResourceEditName() => TranslateRStr(TutorialMessageEventRStr);

    }

    public enum TFlagEventTriggerMode : byte
    {
        etmInstant = 0,
        etmNewTurn = 1
    }

    public static partial class FlagEventsUnit
    {
        // FlagEvents.pas:284
        public static void RegisterFlagEvents(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TFlagMessageEvent>(), EClass.Of<TFlagMessageEventResource>(), EClass.Of<TTutorialMessageEvent>(), EClass.Of<TTutorialMessageEventResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
