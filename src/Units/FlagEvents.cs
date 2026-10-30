// Translated from FlagEvents.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.FlagEvents
{
    // { ot=1 }
    // { TFlagEvent (instance size 24, VMT 557D085C):
//     +$10 FTriggerMode, +$14 FTriggerPlayers (player 1 is bit 0). }
    public partial class TFlagEvent : Aow.Units.HSEngine.TMapObject
    {
        public TFlagEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        public Aow.Units.FlagEvents.TFlagEventTriggerMode FTriggerMode;
        public Aow.Units.FixedArray_byte_3 FReserved11;
        public int FTriggerPlayers;
        // FlagEvents.pas:98
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTriggerPlayers, 30);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FTriggerMode, 1), 1, 31);
        }

        // FlagEvents.pas:157
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if ((Msg.MsgID == 537001987) && (FTriggerMode == Aow.Units.FlagEvents.TFlagEventTriggerMode.etmNewTurn))
            {
                UpdateNewTurnTrigger(unchecked((sbyte)((sbyte)Msg.Payload.Value)));
            }
        }

        // FlagEvents.pas:164
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
            {
                base.Show(XhpSurf, YhpSurf, SurfaceDesc);
            }
        }

        // FlagEvents.pas:150
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            base.MapFieldMsgProc(ref Msg);
            if ((Msg.MsgID == 4365) && (FTriggerMode == Aow.Units.FlagEvents.TFlagEventTriggerMode.etmInstant))
            {
                UpdateInstantTrigger();
            }
        }

        // FlagEvents.pas:110
        public override bool CanSelect(int Xhp, int Yhp)
        {
            bool Result = default;
            Result = Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting);
            return Result;
        }

        // FlagEvents.pas:105
        public override bool CanPlace(Aow.Units.HSEngine.TMapContainer MapContainer, sbyte Xhx, sbyte Yhx, sbyte Lhx)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // FlagEvents.pas:129
        public virtual bool Trigger(sbyte Player)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // FlagEvents.pas:115
        // { +11C }
        public bool GetTriggerPlayer(sbyte Player)
        {
            bool Result = default;
            Result = (Player > 0) && ((FTriggerPlayers & (1 << (Player - 1))) != 0);
            return Result;
        }

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
            sbyte Player = default;
            var call__1 = ((Aow.Units.AoWE.TAoWMapField)Owner).GetPlayer();
            Player = unchecked((sbyte)call__1);
            bool bool__2 = (Player != -1);
            if (bool__2)
            {
                var call__3 = GetTriggerPlayer(unchecked((sbyte)Player));
                bool__2 = call__3;
            }

            bool bool__4 = bool__2;
            if (bool__4)
            {
                var call__5 = Trigger(unchecked((sbyte)Player));
                bool__4 = call__5;
            }

            if (bool__4)
            {
                SetTriggerPlayer(unchecked((sbyte)Player), false);
            }
        }

        // FlagEvents.pas:143
        public void UpdateNewTurnTrigger(sbyte Player)
        {
            var call__6 = ((Aow.Units.AoWE.TAoWMapField)Owner).GetPlayer();
            bool bool__7 = (Player == call__6);
            if (bool__7)
            {
                var call__8 = GetTriggerPlayer(unchecked((sbyte)Player));
                bool__7 = call__8;
            }

            bool bool__9 = bool__7;
            if (bool__9)
            {
                var call__10 = Trigger(unchecked((sbyte)Player));
                bool__9 = call__10;
            }

            if (bool__9)
            {
                SetTriggerPlayer(unchecked((sbyte)Player), false);
            }
        }
    }

    // { TFlagEventResource (instance size 52, VMT 557D09F0): no additional fields. }
    public partial class TFlagEventResource : Aow.Units.HSEngine.TMapObjectResource
    {
        public TFlagEventResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
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

    // { TFlagMessageEventLog (instance size 36, VMT 557D0AE4): no additional fields. }
    public partial class TFlagMessageEventLog : Aow.Units.EventLog.THelpMessageEventLog
    {
        // EventLog.pas:125
        public static new Aow.Units.FlagEvents.TFlagMessageEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.FlagEvents.TFlagMessageEventLog o = new Aow.Units.FlagEvents.TFlagMessageEventLog();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFlagMessageEvent (instance size 32, VMT 557D0BE8): +$18 FHelpFileName, +$1C FMessage. }
    public partial class TFlagMessageEvent : Aow.Units.FlagEvents.TFlagEvent
    {
        public TFlagMessageEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__11 = new Aow.Rtl.TStringList();
            FMessage = call__11;
        }

        // FlagEvents.pas:192
        public new const int ClassID = 132461;

        public Aow.Rtl.AnsiString FHelpFileName;
        public Aow.Rtl.TStringList FMessage;
        // FlagEvents.pas:186
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // FlagEvents.pas:197
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 40);
            Stream.rwString(ref FHelpFileName, 41);
        }

        // FlagEvents.pas:180


        // FlagEvents.pas:204
        public override bool Trigger(sbyte Player)
        {
            bool Result = default;
            Aow.Units.FlagEvents.TFlagMessageEventLog Log = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player)
            {
                Log = Aow.Units.FlagEvents.TFlagMessageEventLog.Create(null);
                Log.FHelpFileName = FHelpFileName;
                var call__12 = FMessage.Text;
                Log.FMessage.Text = call__12;
                Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Log, FTriggerMode == Aow.Units.FlagEvents.TFlagEventTriggerMode.etmInstant, null);
                Log.Release();
            }

            Result = true;
            return Result;
        }

        // FlagEvents.pas:62


        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // FlagEvents.pas:192
            public override int ClassID() => Aow.Units.FlagEvents.TFlagMessageEvent.ClassID;

            // FlagEvents.pas:62
            public new Aow.Units.FlagEvents.TFlagMessageEvent Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.FlagEvents.TFlagMessageEvent)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.FlagEvents.TFlagMessageEvent => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TFlagMessageEventResource (instance size 52, VMT 557D0D9C): no additional fields. }
    public partial class TFlagMessageEventResource : Aow.Units.FlagEvents.TFlagEventResource
    {
        public TFlagMessageEventResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // FlagEvents.pas:218
        public new const int ClassID = 132462;

        // FlagEvents.pas:228
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132461;
            return Result;
        }

        // FlagEvents.pas:223
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FlagMessageEventRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // FlagEvents.pas:218
            public override int ClassID() => Aow.Units.FlagEvents.TFlagMessageEventResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.FlagEvents.TFlagMessageEventResource => (Meta)MetaRegistry.Of(typeof(T));
            // FlagEvents.pas:46
            public new Aow.Units.FlagEvents.TFlagMessageEventResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.FlagEvents.TFlagMessageEventResource)base.Create(AOwner);
        }
    }

    // { TTutorialMessageEvent (instance size 28, VMT 557D0EA0): +$18 FMessage. }
    public partial class TTutorialMessageEvent : Aow.Units.FlagEvents.TFlagEvent
    {
        public TTutorialMessageEvent(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__13 = new Aow.Rtl.TStringList();
            FMessage = call__13;
        }

        // FlagEvents.pas:245
        public new const int ClassID = 132459;

        public Aow.Rtl.TStringList FMessage;
        // FlagEvents.pas:239
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // FlagEvents.pas:250
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 40);
        }

        // FlagEvents.pas:233


        // FlagEvents.pas:256
        public override bool Trigger(sbyte Player)
        {
            bool Result = default;
            Aow.Units.EventLog.TTutorialMessageEventLog Log = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == Player)
            {
                Log = Aow.Units.EventLog.TTutorialMessageEventLog.Create(null);
                var call__14 = FMessage.Text;
                Log.FMessage.Text = call__14;
                Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Log, FTriggerMode == Aow.Units.FlagEvents.TFlagEventTriggerMode.etmInstant, null);
                Log.Release();
            }

            Result = true;
            return Result;
        }

        // FlagEvents.pas:81


        public new partial class Meta : Aow.Units.HSEngine.TMapObject.Meta
        {
            // FlagEvents.pas:245
            public override int ClassID() => Aow.Units.FlagEvents.TTutorialMessageEvent.ClassID;

            // FlagEvents.pas:81
            public new Aow.Units.FlagEvents.TTutorialMessageEvent Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.FlagEvents.TTutorialMessageEvent)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.FlagEvents.TTutorialMessageEvent => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TTutorialMessageEventResource (instance size 52, VMT 557D104C): no additional fields. }
    public partial class TTutorialMessageEventResource : Aow.Units.FlagEvents.TFlagEventResource
    {
        public TTutorialMessageEventResource(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {}

        // FlagEvents.pas:269
        public new const int ClassID = 132460;

        // FlagEvents.pas:279
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 132459;
            return Result;
        }

        // FlagEvents.pas:274
        public override Aow.Rtl.AnsiString ResourceEditName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TutorialMessageEventRStr);
            return Result;
        }

        public new partial class Meta : Aow.Units.HSEngine.TMapObjectResource.Meta
        {
            // FlagEvents.pas:269
            public override int ClassID() => Aow.Units.FlagEvents.TTutorialMessageEventResource.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.FlagEvents.TTutorialMessageEventResource => (Meta)MetaRegistry.Of(typeof(T));
            // FlagEvents.pas:46
            public new Aow.Units.FlagEvents.TTutorialMessageEventResource Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.FlagEvents.TTutorialMessageEventResource)base.Create(AOwner);
        }
    }

    public enum TFlagEventTriggerMode : byte
    {
        etmInstant = 0,
        etmNewTurn = 1
    }

    public static partial class FlagEvents
    {
        // FlagEvents.pas:284
        public static void RegisterFlagEvents(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.FlagEvents.TFlagMessageEvent>(), Aow.Units.Engine.EClass.Of<Aow.Units.FlagEvents.TFlagMessageEventResource>(), Aow.Units.Engine.EClass.Of<Aow.Units.FlagEvents.TTutorialMessageEvent>(), Aow.Units.Engine.EClass.Of<Aow.Units.FlagEvents.TTutorialMessageEventResource>()]);
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
