// Translated from FlagEvents.pas.
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World
{

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
            if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
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
        public override bool CanSelect(int Xhp, int Yhp) => Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting);

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

    public partial class FlagEventResource : MapObjectResource
    {
        public FlagEventResource(ECustomNode owner) : base(owner)
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

    public partial class FlagMessageEventLog : HelpMessageEventLog
    {
        public FlagMessageEventLog(ECustomNode owner) : base(owner)
        { }

    }
}

namespace Aow.Game.World
{

    // FHelpFileName, FMessage
    public partial class FlagMessageEvent : FlagEvent
    {
        public FlagMessageEvent(ECustomNode owner) : base(owner)
        {
            FMessage = new StringList();
        }

        // FlagEvents.pas:192
        public new const int ClassID = 132461;

        public string FHelpFileName = "";
        public StringList FMessage;

        // FlagEvents.pas:186
        public override void Dispose()
        {
            FMessage?.Dispose();
            base.Dispose();
        }

        // FlagEvents.pas:197
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 40);
            Stream.rwString(ref FHelpFileName, 41);
        }

        // FlagEvents.pas:180

        // FlagEvents.pas:204
        public override bool Trigger(sbyte Player)
        {
            FlagMessageEventLog Log;
            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Player)
            {
                Log = new FlagMessageEventLog(null);
                Log.FHelpFileName = FHelpFileName;
                var text = FMessage.Text;
                Log.FMessage.Text = text;
                Aow.Game.World.Map.AoWHSMap.Current.TriggerExecuteEventLog(Log, FTriggerMode == Aow.Game.World.FlagEvent.TriggerMode.Instant, null);
                Log.Release();
            }

            return true;
        }

        public new partial class Resource : FlagEventResource
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // FlagEvents.pas:218
            public new const int ClassID = 132462;

            // FlagEvents.pas:228
            public override int ResourceUserClassID() => 132461;

            // FlagEvents.pas:223
            public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.FlagMessageEvent.Resource.FlagMessageEventRStr);

            private static readonly string FlagMessageEventRStr = "Flag Message Event";

        }

        // FlagEvents.pas:62

    }

    // FMessage
    public partial class TutorialMessageEvent : FlagEvent
    {
        public TutorialMessageEvent(ECustomNode owner) : base(owner)
        {
            FMessage = new StringList();
        }

        // FlagEvents.pas:245
        public new const int ClassID = 132459;

        public StringList FMessage;

        // FlagEvents.pas:239
        public override void Dispose()
        {
            FMessage?.Dispose();
            base.Dispose();
        }

        // FlagEvents.pas:250
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 40);
        }

        // FlagEvents.pas:233

        // FlagEvents.pas:256
        public override bool Trigger(sbyte Player)
        {
            TutorialMessageEventLog Log;
            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Player)
            {
                Log = new TutorialMessageEventLog(null);
                var text = FMessage.Text;
                Log.FMessage.Text = text;
                Aow.Game.World.Map.AoWHSMap.Current.TriggerExecuteEventLog(Log, FTriggerMode == Aow.Game.World.FlagEvent.TriggerMode.Instant, null);
                Log.Release();
            }

            return true;
        }

        public new partial class Resource : FlagEventResource
        {
            public Resource(ECustomNode owner) : base(owner)
            { }

            // FlagEvents.pas:269
            public new const int ClassID = 132460;

            // FlagEvents.pas:279
            public override int ResourceUserClassID() => 132459;

            // FlagEvents.pas:274
            public override string ResourceEditName() => Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.World.TutorialMessageEvent.Resource.TutorialMessageEventRStr);

            private static readonly string TutorialMessageEventRStr = "Tutorial Message Event";

        }

        // FlagEvents.pas:81

    }
}
