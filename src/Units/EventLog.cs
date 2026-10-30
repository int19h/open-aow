// Translated from EventLog.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;

namespace Aow.Units.EventLog
{
    public enum TSaveGameEventLogModeElement : byte
    {
        sgmDisableCancel = 0
    }

    // { ot=1 }
    // { TEventLog: size $18; $08 day, $0C unique ID, $10 player,
//     $14 reference count. VMT 557FC1C8. }
    public partial class TEventLog : Aow.Units.Engine.TEObject
    {
        // EventLog.pas:323
        public TEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FRefCount = 1;
        }

        private protected TEventLog(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FRefCount = 1;
        }

        public int FDay = Aow.Units.AoWE.AoWE.AoWHSMap is { } map ? map.FDay : 0;
        // { $08 }
        public int FID;
        // { $0C }
        public sbyte FPlayer = Aow.Units.AoWE.AoWE.AoWHSMap is { } map ? unchecked((sbyte)map.FCurrentPlayer) : (sbyte)0;
        // { $10 }
        public Aow.Units.FixedArray_byte_3 F_EventLog_11;
        public int FRefCount;
        // EventLog.pas:334
        // { $14 }
        public override void SetOwner(Aow.Units.Engine.TECustomNode AOwner)
        {
            if (FRefCount != 0)
            {
                AddRef();
                base.SetOwner(AOwner);
                Release();
            }
            else
            {
                base.SetOwner(AOwner);
            }
        }

        // EventLog.pas:358
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FDay, 10);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 11);
            Stream.rwInteger(ref FID, 12);
        }

        // EventLog.pas:345
        public override int AddRef()
        {
            int Result = default;
            FRefCount += 1;
            Result = FRefCount;
            return Result;
        }

        // EventLog.pas:351
        public override int Release()
        {
            int Result = default;
            FRefCount -= 1;
            Result = FRefCount;
            if (FRefCount == 0)
            {
                Destroy();
            }

            return Result;
        }

        // EventLog.pas:371
        public virtual Aow.Rtl.AnsiString GetText()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = (AnsiString)"Default TEventLog";
            return Result;
        }

        // EventLog.pas:376
        // { $04C }
        public virtual int GetTextColor()
        {
            int Result = default;
            int Index = default;
            bool bool__1 = (FPlayer > 0);
            if (bool__1)
            {
                var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount();
                bool__1 = (FPlayer < call__2);
            }

            if (bool__1)
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Index = call__3.FPlayerRace;
            }
            else
            {
                Index = 12;
            }

            Result = Aow.Units.AoWE.AoWE.RaceColors[Index];
            return Result;
        }

        // EventLog.pas:386
        // { $050 }
        public virtual int GetLeftIcon()
        {
            int Result = default;
            bool bool__4 = (FPlayer > 0);
            if (bool__4)
            {
                var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount();
                bool__4 = (FPlayer < call__5);
            }

            if (bool__4)
            {
                var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Result = call__6.FPlayerRace;
            }
            else
            {
                Result = 12;
            }

            return Result;
        }

        // EventLog.pas:393
        // { $054 }
        public virtual int GetMiddleIcon()
        {
            int Result = default;
            Result = -1;
            return Result;
        }

        // EventLog.pas:398
        // { $058 }
        public virtual int GetRightIcon()
        {
            int Result = default;
            Result = -1;
            return Result;
        }

        // EventLog.pas:366
        // { $05C }
        public virtual bool GetVisible()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // EventLog.pas:425
        // { $060 }
        public virtual void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            X = unchecked((sbyte)-1);
            Y = unchecked((sbyte)-1);
            L = unchecked((sbyte)-1);
        }

        // EventLog.pas:444
        // { $064 }
        public virtual void PlayLoggedSFX(Aow.Units.EventLog.TEventLogbook A1)
        {
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\EVENTS\\NEWEVENT.WAV");
        }

        // EventLog.pas:449
        // { $068 }
        public virtual void NotifyLogged(Aow.Units.EventLog.TEventLogbook A1)
        {
            bool bool__7 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == A1.FPlayer);
            if (bool__7)
            {
                var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                bool__7 = (call__8.FGameOverStatus == Aow.Units.AoWE.TGameOverStatus.goNone);
            }

            if (bool__7)
            {
                PlayLoggedSFX(A1);
            }
        }

        // EventLog.pas:403
        // { $06C }
        public virtual bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            // { Tried Self identity, Pointer identity, typed with; still differs. }
            if ((Trigger == Aow.Units.EventLog.TExecuteTrigger.etUser) || (((Trigger == Aow.Units.EventLog.TExecuteTrigger.etEvent) || ((Trigger == Aow.Units.EventLog.TExecuteTrigger.etNewTurn) && Aow.Units.AoWE.AoWE.AoWHSMap.FRequestCombatType)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == FPlayer)))
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
                Result = true;
            }
            else
            {
                if (Trigger == Aow.Units.EventLog.TExecuteTrigger.etEvent)
                {
                    Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\EVENTS\\NEWEVENT.WAV");
                }

                Result = false;
            }

            return Result;
        }

        // EventLog.pas:432
        public virtual void ViewLocation()
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            GetLocation(ref X, ref Y, ref L);
            if (X != -1)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(X, Y, L);
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
            }
        }

        // EventLog.pas:419
        // { +074 }
        public void CenterViewTo(sbyte X, sbyte Y, sbyte L)
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(X, Y, L);
            Aow.Units.AoWE.AoWE.AoWHSMap.PlayCenterAnimation(unchecked((sbyte)X), unchecked((sbyte)Y), unchecked((sbyte)L));
        }

    }

    // { TTextEventLog: size $1C; $18 text. VMT 557FC564. }
    public partial class TTextEventLog : Aow.Units.EventLog.TEventLog
    {
        public TTextEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // EventLog.pas:713
        public new const int ClassID = 131809;

        public Aow.Rtl.AnsiString FText;
        // EventLog.pas:716
        // { $18 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwString(ref FText, 15);
        }

        // EventLog.pas:719
        public override Aow.Rtl.AnsiString GetText()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = FText;
            return Result;
        }

        // EventLog.pas:722
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // EventLog.pas:713
            public override int ClassID() => Aow.Units.EventLog.TTextEventLog.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TTextEventLog => (Meta)MetaRegistry.Of(typeof(T));
            // EventLog.pas:56
            public new Aow.Units.EventLog.TTextEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TTextEventLog)base.Create(AOwner);
        }
    }

    // { TLocationTextEventLog: size $20; $1C/$1D/$1E location. VMT 557FC664. }
    public partial class TLocationTextEventLog : Aow.Units.EventLog.TTextEventLog
    {
        // EventLog.pas:725
        public TLocationTextEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            FXhx = unchecked((sbyte)-1);
            FYhx = unchecked((sbyte)-1);
            FLhx = unchecked((sbyte)-1);
        }

        // EventLog.pas:728
        public new const int ClassID = 131816;

        public sbyte FXhx;
        // { $1C }
        public sbyte FYhx;
        // { $1D }
        public sbyte FLhx;
        // { $1E }
        public Aow.Units.FixedArray_byte_1 F_LocationTextEventLog_1F;
        // EventLog.pas:731
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 21);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 22);
        }

        // EventLog.pas:740
        public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            X = unchecked((sbyte)FXhx);
            Y = unchecked((sbyte)FYhx);
            L = unchecked((sbyte)FLhx);
        }

        // EventLog.pas:743
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            base.Execute(Mode, Trigger, Done);
            if (Trigger == Aow.Units.EventLog.TExecuteTrigger.etUser)
            {
                ViewLocation();
            }

            Result = false;
            return Result;
        }

        // EventLog.pas:737
        public void SetLocation(sbyte X, sbyte Y, sbyte L)
        {
            FXhx = unchecked((sbyte)X);
            FYhx = unchecked((sbyte)Y);
            FLhx = unchecked((sbyte)L);
        }

        public new partial class Meta : Aow.Units.EventLog.TTextEventLog.Meta
        {
            // EventLog.pas:728
            public override int ClassID() => Aow.Units.EventLog.TLocationTextEventLog.ClassID;

            // EventLog.pas:91
            public new Aow.Units.EventLog.TLocationTextEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TLocationTextEventLog)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TLocationTextEventLog => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TMessageEventLog: size $28; $18 title, $20 message, $24 layout, $25 buttons, $26 pressed button. VMT 557FCD24. }
    public partial class TMessageEventLog : Aow.Units.EventLog.TEventLog
    {
        // EventLog.pas:824
        private protected TMessageEventLog(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__9 = new Aow.Rtl.TStringList();
            FMessage = call__9;
            var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MessageRStr);
            FTitle = call__10;
            FButtons = Aow.Units.EventLog.TMessageEventLogButtons.mebOk;
            FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebNone;
            FLayout = Aow.Units.EventLog.TMessageEventLogLayout.melNormal;
        }

        public static new TMessageEventLog Create(Aow.Units.Engine.TECustomNode AOwner) =>
            Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TMessageEventLog(context, owner));

        public Aow.Rtl.AnsiString FTitle;
        // { $18 }
        public Aow.Rtl.AnsiString FHeader;
        // { $1C; 0040E89C }
        public Aow.Rtl.TStrings FMessage;
        // { $20 }
        public Aow.Units.EventLog.TMessageEventLogLayout FLayout;
        // { $24 }
        public Aow.Units.EventLog.TMessageEventLogButtons FButtons;
        // { $25 }
        public Aow.Units.EventLog.TPressedEventLogButton FPressedButton;
        // { $26 }
        public Aow.Units.FixedArray_byte_1 F_MessageEventLog_27;
        // EventLog.pas:832
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // EventLog.pas:835
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 30);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FLayout, 1), 1, 31);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FButtons, 1), 1, 32);
            Stream.rwString(ref FTitle, 33);
        }

        // EventLog.pas:842
        public virtual bool OK()
        {
            bool Result = default;
            FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebOk;
            Result = true;
            return Result;
        }

        // EventLog.pas:845
        // { $078; 557FE1A8 }
        public virtual bool Cancel()
        {
            bool Result = default;
            FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebCancel;
            Result = true;
            return Result;
        }

        // EventLog.pas:848
        // { $07C; 557FE1B0 }
        public virtual bool Yes()
        {
            bool Result = default;
            FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebYes;
            Result = true;
            return Result;
        }

        // EventLog.pas:854
        public virtual bool No()
        {
            bool Result = default;
            FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebNo;
            Result = true;
            return Result;
        }

    }

    // { THelpMessageEventLog: size $24; $18 message, $1C help title, $20 mode. VMT 557FCE44. }
    public partial class THelpMessageEventLog : Aow.Units.EventLog.TEventLog
    {
        // EventLog.pas:860
        private protected THelpMessageEventLog(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__11 = new Aow.Rtl.TStringList();
            FMessage = call__11;
            var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HelpRStr);
            FHelpFileName = call__12;
            FMode = 0;
        }

        public static new THelpMessageEventLog Create(Aow.Units.Engine.TECustomNode AOwner) =>
            Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new THelpMessageEventLog(context, owner));

        public Aow.Rtl.TStringList FMessage;
        // { $18 }
        public Aow.Rtl.AnsiString FHelpFileName;
        // { $1C }
        public byte FMode;
        // { $20 }
        public Aow.Units.FixedArray_byte_3 F_HelpMessageEventLog_21;
        // EventLog.pas:868
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

    }

    // { TExecuteEventLogCallBackList: size $08; $04 list of $10-byte callback records. VMT 557FC130. }
    public partial class TExecuteEventLogCallBackList : Aow.Rtl.TObject
    {
        public System.Collections.Generic.List<TExecuteEventLogInfo> FList = [];
        // EventLog.pas:283
        // { $04 }
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // EventLog.pas:277
        public TExecuteEventLogCallBackList()
        {
            ;
            var call__13 = new System.Collections.Generic.List<TExecuteEventLogInfo>();
            FList = call__13;
        }

        // EventLog.pas:290
        public void Clear()
        {
            while (FList.Count != 0)
            {
                Delete(FList.Count - 1);
            }
        }

        // EventLog.pas:295
        public int GetCount()
        {
            int Result = default;
            Result = FList.Count;
            return Result;
        }

        // EventLog.pas:300
        public ref TExecuteEventLogInfo GetItem(int Index) => ref CollectionsMarshal.AsSpan(FList)[Index];

        // EventLog.pas:305
        public void Add(in Aow.Units.EventLog.TExecuteEventLogInfo Info)
        {
            FList.Add(Info);
        }

        // EventLog.pas:314
        public void Delete(int Index)
        {
            if (Index < FList.Count)
            {
                ;
                FList.RemoveAt(Index);
            }
        }

        // EventLog.pas:133
    }

    // { TEventLogList: size $18; $0C update count, $10 changed callback. VMT 557FC2B0. }
    public partial class TEventLogList : Aow.Units.Engine.TENode
    {
        public TEventLogList(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public int FUpdateCount;
        // { $0C }
        public Aow.Rtl.TNotifyEvent FOnChanged;
        // EventLog.pas:481
        // { $10 }
        public override void MainReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            int I = default;
            if (Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smRead))
            {
                BeginUpdate();
            }

            base.MainReadWrite(Stream);
            if (Stream.StorageMode().Contains(Aow.Units.Engine.TStorageModeElement.smRead))
            {
                if ((Stream.StorageMode() & TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead, Aow.Units.Engine.TStorageModeElement.smCompare, Aow.Units.Engine.TStorageModeElement.smTranslate)) == TSet<Aow.Units.Engine.TStorageModeElement>.Of(Aow.Units.Engine.TStorageModeElement.smRead))
                {
                    {
                        I = 0;
                        int end__14 = FChildren.Count - 1;
                        for (; I <= end__14; I += 1)
                        {
                            ((Aow.Units.Engine.TEObject)FChildren.List[I]).Release();
                        }
                    }
                }

                EndUpdate();
            }
        }

        // EventLog.pas:471
        public override void Clear()
        {
            int I = default;
            {
                I = GetCount() - 1;
                byte end__15 = 0;
                for (; I >= end__15; I -= 1)
                {
                    ((Aow.Units.Engine.TEObject)FChildren.List[I]).Release();
                }
            }

            FChildren.Clear();
        }

        // EventLog.pas:478
        public override void DestroyAllChildren()
        {
            Clear();
        }

        // EventLog.pas:464
        public override int RemoveChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            var call__16 = FChildren.Remove(Child);
            Result = call__16;
            if (Result != -1)
            {
                Child.SetOwner(null);
                Child.Release();
                Changed();
            }

            return Result;
        }

        // EventLog.pas:456
        public override int AddChild(Aow.Units.Engine.TEObject Child)
        {
            int Result = default;
            FChildren.Add(Child);
            if (Child.AddRef() == -1)
            {
                throw Aow.Rtl.Exception.Create((AnsiString)"EObject doesn't support ReferenceCounting");
            }

            Result = FChildren.Count - 1;
            Changed();
            return Result;
        }

        // EventLog.pas:494
        public void Changed()
        {
            if ((FUpdateCount == 0) && (FOnChanged != null))
            {
                FOnChanged(this);
            }
        }

        // EventLog.pas:499
        public void BeginUpdate()
        {
            FUpdateCount += 1;
        }

        // EventLog.pas:502
        public void EndUpdate()
        {
            FUpdateCount -= 1;
            if (FUpdateCount == 0)
            {
                Changed();
            }
        }

        // EventLog.pas:505
        public Aow.Units.EventLog.TEventLog GetEvents(int Index)
        {
            Aow.Units.EventLog.TEventLog Result = default;
            Result = (Aow.Units.EventLog.TEventLog)FChildren.List[Index];
            return Result;
        }

        // EventLog.pas:508
        public void AddEvent(Aow.Units.EventLog.TEventLog Event)
        {
            AddChild(Event);
        }

        // EventLog.pas:511
        public void RemoveEvent(Aow.Units.EventLog.TEventLog Event)
        {
            RemoveChild(Event);
        }

        // EventLog.pas:514
        public Aow.Units.EventLog.TEventLog FindID(int ID)
        {
            Aow.Units.EventLog.TEventLog Result = default;
            int I = default;
            I = GetCount();
            Result = null;
            while ((I != 0) && (Result == null))
            {
                I -= 1;
                if (((Aow.Units.EventLog.TEventLog)FChildren.List[I]).FID == ID)
                {
                    Result = (Aow.Units.EventLog.TEventLog)FChildren.List[I];
                    break;
                }
            }

            return Result;
        }

        // Engine.pas:631
    }

    // { TEventLogbook: size $14; $08 events, $0C player, $10 next ID. VMT 557FC3B0. }
    public partial class TEventLogbook : Aow.Units.Engine.TEObject
    {
        // EventLog.pas:532
        public new const int ClassID = 131808;


        public Aow.Units.EventLog.TEventLogList FEvents;
        // { $08 }
        public sbyte FPlayer;
        // { $0C }
        public Aow.Units.FixedArray_byte_3 FReserved0D;
        public int FNextID;
        // EventLog.pas:529
        // { $10 }
        public override void Destroy()
        {
            FEvents.Free();
            base.Destroy();
        }

        // EventLog.pas:547
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FEvents, 10);
            Stream.rwInteger(ref FNextID, 11);
        }

        // EventLog.pas:526
        public TEventLogbook(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__17 = new Aow.Units.EventLog.TEventLogList(null);
            FEvents = call__17;
        }

        // EventLog.pas:535
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            FEvents.MsgProc(ref Msg, Sender);
        }

        // EventLog.pas:538
        public int GetCount()
        {
            int Result = default;
            var call__18 = FEvents.GetCount();
            Result = call__18;
            return Result;
        }

        // EventLog.pas:541
        public Aow.Rtl.TNotifyEvent GetOnChanged()
        {
            Aow.Rtl.TNotifyEvent Result = default;
            Result = FEvents.FOnChanged;
            return Result;
        }

        // EventLog.pas:544
        public void SetOnChanged(Aow.Rtl.TNotifyEvent Value)
        {
            FEvents.FOnChanged = Value;
        }

        // EventLog.pas:550
        public void ListEvents(Aow.Rtl.TStrings Strings)
        {
            int I = default;
            {
                I = 0;
                int end__19 = FEvents.FChildren.Count - 1;
                for (; I <= end__19; I += 1)
                {
                    var call__20 = ((Aow.Units.EventLog.TEventLog)FEvents.FChildren.List[I]).GetVisible();
                    if (call__20)
                    {
                        var call__21 = ((Aow.Units.EventLog.TEventLog)FEvents.FChildren.List[I]).GetText();
                        Strings.AddObject(call__21, Aow.Rtl.Rtl.IntToObject((int)I));
                    }
                }
            }
        }

        // EventLog.pas:558
        public Aow.Units.EventLog.TEventLog GetLogs(int Index)
        {
            Aow.Units.EventLog.TEventLog Result = default;
            Result = (Aow.Units.EventLog.TEventLog)FEvents.FChildren.List[Index];
            return Result;
        }

        // EventLog.pas:561
        public int CreateUniqueID()
        {
            int Result = default;
            FNextID += 1;
            while (true)
            {
                var call__22 = FindID(FNextID);
                if (!(call__22 != null))
                    break;
                {
                    FNextID += 1;
                    if (FNextID > 16777215)
                    {
                        FNextID = 1;
                    }
                }
            }

            Result = FNextID;
            return Result;
        }

        // EventLog.pas:572
        public Aow.Units.EventLog.TEventLog FindID(int ID)
        {
            Aow.Units.EventLog.TEventLog Result = default;
            var call__23 = FEvents.FindID(ID);
            Result = call__23;
            return Result;
        }

        // EventLog.pas:575
        public int FindFirstLog(int Player, int Day)
        {
            int Result = default;
            Result = 0;
            while ((Result != this.FEvents.FChildren.Count) && ((((Aow.Units.EventLog.TEventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player) || (((Aow.Units.EventLog.TEventLog)this.FEvents.FChildren.List[Result]).FDay != Day)))
            {
                Result += 1;
            }

            if (Result == this.FEvents.FChildren.Count)
            {
                Result = -1;
            }

            return Result;
        }

        // EventLog.pas:587
        public int FindFirstLogEx(int AClassID, int Player, int Day)
        {
            int Result = default;
            Result = 0;
            while (true)
            {
                bool bool__26 = (Result != this.FEvents.FChildren.Count);
                if (bool__26)
                {
                    bool bool__27 = (((Aow.Units.EventLog.TEventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player) || (((Aow.Units.EventLog.TEventLog)this.FEvents.FChildren.List[Result]).FDay != Day);
                    if (!bool__27)
                    {
                        var call__28 = ((Aow.Units.EventLog.TEventLog)this.FEvents.FChildren.List[Result]).EClass.ClassID;
                        bool__27 = (call__28 != AClassID);
                    }

                    bool__26 = bool__27;
                }

                if (!bool__26)
                    break;
                {
                    Result += 1;
                }
            }

            if (Result == this.FEvents.FChildren.Count)
            {
                Result = -1;
            }

            return Result;
        }

        // EventLog.pas:600
        // { conflict: function FindFirstLogEx(ClassID, Player, Day: Integer): Integer; }
        public void AddEvent(Aow.Units.EventLog.TEventLog Event)
        {
            var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.GetSynchronised();
            Aow.Rtl.Rtl.Assert(call__29, (AnsiString)"", "D:\\git\\aow\\src\\delphi3\\AoWEPACK\\EventLog.pas", 704);
            Event.SetOwner(FEvents);
            var call__30 = CreateUniqueID();
            Event.FID = call__30;
            Event.NotifyLogged(this);
        }

        // EventLog.pas:710
        public void RemoveEvent(Aow.Units.EventLog.TEventLog Event)
        {
            FEvents.RemoveChild(Event);
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // EventLog.pas:532
            public override int ClassID() => Aow.Units.EventLog.TEventLogbook.ClassID;

            // EventLog.pas:169
            public new Aow.Units.EventLog.TEventLogbook Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TEventLogbook)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TEventLogbook => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TUserEventLog (instance size 32, VMT 557FC474) }
    public partial class TUserEventLog : Aow.Units.EventLog.TEventLog
    {
        public TUserEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        public Aow.Rtl.TObject FData;
        public Aow.Units.FixedArray_byte_4 FReserved1C;
    }

    // { $18; 55749C0D }
    // { TEndTurnEventLog: size $18; no additional fields. VMT 557FC764. }
    public partial class TEndTurnEventLog : Aow.Units.EventLog.TEventLog
    {
        public TEndTurnEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // EventLog.pas:753
        public new const int ClassID = 131811;

        // EventLog.pas:750
        public override bool GetVisible()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        // EventLog.pas:756
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // EventLog.pas:753
            public override int ClassID() => Aow.Units.EventLog.TEndTurnEventLog.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TEndTurnEventLog => (Meta)MetaRegistry.Of(typeof(T));
            // EventLog.pas:56
            public new Aow.Units.EventLog.TEndTurnEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TEndTurnEventLog)base.Create(AOwner);
        }
    }

    // { TShowMessageEventLog: size $1C; $18 message. VMT 557FC85C. }
    public partial class TShowMessageEventLog : Aow.Units.EventLog.TEventLog
    {
        // EventLog.pas:759
        public TShowMessageEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__31 = new Aow.Rtl.TStringList();
            FMessage = call__31;
        }

        // EventLog.pas:765
        public new const int ClassID = 131830;

        public Aow.Rtl.TStringList FMessage;
        // EventLog.pas:762
        // { $18 }
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // EventLog.pas:768
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 20);
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // EventLog.pas:765
            public override int ClassID() => Aow.Units.EventLog.TShowMessageEventLog.ClassID;

            // EventLog.pas:206
            public new Aow.Units.EventLog.TShowMessageEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TShowMessageEventLog)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TShowMessageEventLog => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TGameOverEventLog: size $20; $18 message, inherited $10 player. VMT 557FC95C. }
    public partial class TGameOverEventLog : Aow.Units.EventLog.TEventLog
    {
        // EventLog.pas:771
        public TGameOverEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            var call__32 = new Aow.Rtl.TStringList();
            FMessage = call__32;
        }

        // EventLog.pas:777
        public new const int ClassID = 131812;

        public Aow.Rtl.TStringList FMessage;
        // { $18 }
        public sbyte FTargetPlayer;
        // { $1C; 55776744 }
        public Aow.Units.FixedArray_byte_3 F_GameOverEventLog_1D;
        // EventLog.pas:774
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // EventLog.pas:780
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 20);
        }

        // EventLog.pas:783
        public override Aow.Rtl.AnsiString GetText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__33 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
            switch (call__33.FGameOverStatus)
            {
                case Aow.Units.AoWE.TGameOverStatus.goLost:
                    var call__34 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__35 = call__34.GetName();
                    var call__36 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GameOverDefeatEventLogRStr);
                    var call__37 = Aow.Rtl.Rtl.Format(call__36, [call__35]);
                    Result = call__37;
                    break;
                case Aow.Units.AoWE.TGameOverStatus.goWon:
                    var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__39 = call__38.GetName();
                    var call__40 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GameOverVictoryEventLogRStr);
                    var call__41 = Aow.Rtl.Rtl.Format(call__40, [call__39]);
                    Result = call__41;
                    break;
            }

            return Result;
        }

        // EventLog.pas:794
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            // { Tried Self identity, Pointer identity, typed with; still differs. }
            switch (Trigger)
            {
                case Aow.Units.EventLog.TExecuteTrigger.etUser:
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != FPlayer)
                    {
                        Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }

                    break;
                case Aow.Units.EventLog.TExecuteTrigger.etEvent:
                    Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
                    Result = true;
                    break;
                case Aow.Units.EventLog.TExecuteTrigger.etNewTurn:
                    Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
                    Result = true;
                    break;
                default:
                    Result = false;
                    break;
            }

            return Result;
        }

        // EventLog.pas:791
        public void Handled()
        {
            Aow.Units.AoWE.AoWE.AoWHSMap.GameOverEventHandled(unchecked((sbyte)FPlayer));
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // EventLog.pas:777
            public override int ClassID() => Aow.Units.EventLog.TGameOverEventLog.ClassID;

            // EventLog.pas:218
            public new Aow.Units.EventLog.TGameOverEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TGameOverEventLog)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TGameOverEventLog => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TNewDayEventLog: size $1C; inherited $18 text. VMT 557FCA54. }
    public partial class TNewDayEventLog : Aow.Units.EventLog.TTextEventLog
    {
        public TNewDayEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // EventLog.pas:809
        public new const int ClassID = 131813;

        // EventLog.pas:812
        public override int GetLeftIcon()
        {
            int Result = default;
            Result = 18;
            return Result;
        }

        // EventLog.pas:815
        public override void PlayLoggedSFX(Aow.Units.EventLog.TEventLogbook A1)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay > 1)
            {
                base.PlayLoggedSFX(A1);
            }
        }

        public new partial class Meta : Aow.Units.EventLog.TTextEventLog.Meta
        {
            // EventLog.pas:809
            public override int ClassID() => Aow.Units.EventLog.TNewDayEventLog.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TNewDayEventLog => (Meta)MetaRegistry.Of(typeof(T));
            // EventLog.pas:56
            public new Aow.Units.EventLog.TNewDayEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TNewDayEventLog)base.Create(AOwner);
        }
    }

    // { TShowBookEventLog: size $18; no additional fields. VMT 557FCB48. }
    public partial class TShowBookEventLog : Aow.Units.EventLog.TEventLog
    {
        public TShowBookEventLog(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // EventLog.pas:818
        public new const int ClassID = 131818;

        // EventLog.pas:821
        public override bool GetVisible()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // EventLog.pas:818
            public override int ClassID() => Aow.Units.EventLog.TShowBookEventLog.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.EventLog.TShowBookEventLog => (Meta)MetaRegistry.Of(typeof(T));
            // EventLog.pas:56
            public new Aow.Units.EventLog.TShowBookEventLog Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.EventLog.TShowBookEventLog)base.Create(AOwner);
        }
    }

    // { TTutorialMessageEventLog: size $24; inherited $18 message and $1C title. VMT 557FCF54. }
    public partial class TTutorialMessageEventLog : Aow.Units.EventLog.THelpMessageEventLog
    {
        // EventLog.pas:871
        private TTutorialMessageEventLog(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__42 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TutorialRStr);
            FHelpFileName = call__42;
        }

        public static new TTutorialMessageEventLog Create(Aow.Units.Engine.TECustomNode AOwner) =>
            Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TTutorialMessageEventLog(context, owner));

    }

    // { TSaveGameEventLog: size $2C; $1C directory, $20 default name, $24 file name, $28 title. VMT 557FD0A4. }
    public partial class TSaveGameEventLog : Aow.Units.EventLog.TEventLog
    {
        // EventLog.pas:874
        // { $28 }
        private TSaveGameEventLog(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SaveRStr);
            FTitle = call__43;
            FMode = TSet<Aow.Units.EventLog.TSaveGameEventLogModeElement>.Of();
        }

        public static new TSaveGameEventLog Create(Aow.Units.Engine.TECustomNode AOwner) =>
            Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TSaveGameEventLog(context, owner));

        public TSet<Aow.Units.EventLog.TSaveGameEventLogModeElement> FMode;
        // { $18 }
        public Aow.Units.FixedArray_byte_3 FReserved19;
        public Aow.Rtl.AnsiString FDirectory;
        // { $1C }
        public Aow.Rtl.AnsiString FDefaultName;
        // { $20 }
        public Aow.Rtl.AnsiString FFileName;
        // { $24 }
        public Aow.Rtl.AnsiString FTitle;

    }

    // { TPasswordEventLog: size $28; $18 password, $1C title, $20 prompt, $24 button. VMT 557FD1C4. }
    public partial class TPasswordEventLog : Aow.Units.EventLog.TEventLog
    {
        // EventLog.pas:877
        private TPasswordEventLog(Aow.Rtl.ConstructionContext context, Aow.Units.Engine.TECustomNode AOwner) : base(context, AOwner)
        {
            FPassword = (AnsiString)"";
            var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnterPasswordRStr);
            FPrompt = call__44;
            var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PasswordVerificationRStr);
            FTitle = call__45;
            FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebNone;
        }

        public static new TPasswordEventLog Create(Aow.Units.Engine.TECustomNode AOwner) =>
            Aow.Rtl.ConstructionContext.Create(AOwner, static (context, owner) => new TPasswordEventLog(context, owner));

        public Aow.Rtl.AnsiString FPassword;
        // { $18; 557563E0 }
        public Aow.Rtl.AnsiString FTitle;
        // { $1C }
        public Aow.Rtl.AnsiString FPrompt;
        // { $20 }
        public Aow.Units.EventLog.TPressedEventLogButton FPressedButton;
        // { $24; 557563EA }
        public Aow.Units.FixedArray_byte_3 F_PasswordEventLog_25;

    }

    public delegate void TExecuteDoneEvent(Aow.Units.EventLog.TEventLog Log);
    public struct TExecuteEventLogInfo
    {
        public Aow.Units.EventLog.TEventLog Log;
        public Aow.Units.EventLog.TExecuteDoneEvent Done;
        public bool Immediate;
    }

    public enum TExecuteTrigger : byte
    {
        etEvent = 0,
        etUser = 1,
        etNewTurn = 2
    }

    public enum TMessageEventLogButtons : byte
    {
        mebOk = 0,
        mebOkCancel = 1,
        mebYesNo = 2,
        mebYesNoCancel = 3
    }

    public enum TPressedEventLogButton : byte
    {
        pebNone = 0,
        pebOk = 1,
        pebCancel = 2,
        pebYes = 3,
        pebNo = 4
    }

    public enum TMessageEventLogLayout : byte
    {
        melNormal = 0,
        melHeader = 1
    }

    public static partial class EventLog
    {
        // EventLog.pas:886
        public static void RegisterEventLog(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.EventLog.TTextEventLog>(), Aow.Units.Engine.EClass.Of<Aow.Units.EventLog.TEndTurnEventLog>(), Aow.Units.Engine.EClass.Of<Aow.Units.EventLog.TGameOverEventLog>(), Aow.Units.Engine.EClass.Of<Aow.Units.EventLog.TLocationTextEventLog>()]);
        }

        // EventLog.pas:889
        public static void DistributeEventLog(Aow.Units.EventLog.TEventLog Log, bool Execute)
        {
            int I = default;
            Aow.Units.AoWE.TPlayer Player = default;
            Aow.Units.EventLog.TEventLog CopyLog = default;
            {
                I = 0;
                int end__46 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__46; I += 1)
                {
                    var call__47 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    Player = call__47;
                    if (Player.FStatus == Aow.Units.AoWE.TPlayerStatus.psPlaying)
                    {
                        var call__48 = Log.Copy(Aow.Units.AoWE.AoWE.AoWEngine);
                        CopyLog = (Aow.Units.EventLog.TEventLog)call__48;
                        Player.FEventLogbook.AddEvent(CopyLog);
                        if ((I == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer) && Execute)
                        {
                            CopyLog.Execute(false, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                        }

                        CopyLog.Release();
                    }
                }
            }
        }

        // EventLog.pas:905
        public static void DistributeLocationEventLog(Aow.Units.EventLog.TEventLog Log, sbyte Player, sbyte X, sbyte Y, sbyte L, bool Execute)
        {
            int I = default;
            Aow.Units.AoWE.TPlayer TargetPlayer = default;
            Aow.Units.EventLog.TEventLog CopyLog = default;
            {
                I = 0;
                int end__49 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__49; I += 1)
                {
                    var call__50 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                    TargetPlayer = call__50;
                    bool bool__51 = (TargetPlayer.FStatus == Aow.Units.AoWE.TPlayerStatus.psPlaying);
                    if (bool__51)
                    {
                        bool bool__52 = (I == Player);
                        if (!bool__52)
                        {
                            var call__53 = Aow.Units.AoWE.AoWE.AoWHSMap.FoggedForPlayer(X, Y, L, unchecked((sbyte)I));
                            bool__52 = !call__53;
                        }

                        bool__51 = bool__52;
                    }

                    if (bool__51)
                    {
                        var call__54 = Log.Copy(Aow.Units.AoWE.AoWE.AoWEngine);
                        CopyLog = (Aow.Units.EventLog.TEventLog)call__54;
                        TargetPlayer.FEventLogbook.AddEvent(CopyLog);
                        bool bool__55 = (I == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        if (bool__55)
                        {
                            var call__56 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                            bool__55 = (call__56.FPlayerType == Aow.Units.AoWE.TPlayerType.ptHuman);
                        }

                        if (bool__55 && Execute)
                        {
                            CopyLog.Execute(false, Aow.Units.EventLog.TExecuteTrigger.etEvent, null);
                        }

                        CopyLog.Release();
                    }
                }
            }
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
