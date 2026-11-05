// Translated from EventLog.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Sound.Sound;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.EventLog
{
    public enum TSaveGameEventLogModeElement : byte
    {
        sgmDisableCancel = 0
    }

    // { ot=1 }
    // day, unique ID, player, reference count
    public partial class TEventLog : TEObject
    {

        public TEventLog(TECustomNode owner) : base(owner)
        {
            FRefCount = 1;
        }

        public int FDay = AoWHSMap is { } map ? map.FDay : 0;
        public int FID;
        public sbyte FPlayer = AoWHSMap is { } map ? map.FCurrentPlayer : (sbyte)0;
        public int FRefCount;
        // EventLog.pas:334
        public override void SetOwner(TECustomNode AOwner)
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
        public override void ReadWrite(TEStorageStream Stream)
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
        public virtual AnsiString GetText()
        {
            AnsiString Result = default;
            Result = (AnsiString)"Default TEventLog";
            return Result;
        }

        // EventLog.pas:376
        public virtual int GetTextColor()
        {
            int Result = default;
            int Index = default;
            bool bool__1 = FPlayer > 0;
            if (bool__1)
            {
                var call__2 = AoWHSMap.FPlayers.GetCount();
                bool__1 = FPlayer < call__2;
            }

            if (bool__1)
            {
                var call__3 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Index = call__3.FPlayerRace;
            }
            else
            {
                Index = 12;
            }

            Result = RaceColors[Index];
            return Result;
        }

        // EventLog.pas:386
        public virtual int GetLeftIcon()
        {
            int Result = default;
            bool bool__4 = FPlayer > 0;
            if (bool__4)
            {
                var call__5 = AoWHSMap.FPlayers.GetCount();
                bool__4 = FPlayer < call__5;
            }

            if (bool__4)
            {
                var call__6 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                Result = call__6.FPlayerRace;
            }
            else
            {
                Result = 12;
            }

            return Result;
        }

        // EventLog.pas:393
        public virtual int GetMiddleIcon()
        {
            int Result = default;
            Result = -1;
            return Result;
        }

        // EventLog.pas:398
        public virtual int GetRightIcon()
        {
            int Result = default;
            Result = -1;
            return Result;
        }

        // EventLog.pas:366
        public virtual bool GetVisible()
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // EventLog.pas:425
        public virtual void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            X = -1;
            Y = -1;
            L = -1;
        }

        // EventLog.pas:444
        public virtual void PlayLoggedSFX(TEventLogbook A1)
        {
            PlaySample((AnsiString)"SFX\\EVENTS\\NEWEVENT.WAV");
        }

        // EventLog.pas:449
        public virtual void NotifyLogged(TEventLogbook A1)
        {
            bool bool__7 = AoWHSMap.FSeatedPlayer == A1.FPlayer;
            if (bool__7)
            {
                var call__8 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                bool__7 = call__8.FGameOverStatus == TGameOverStatus.goNone;
            }

            if (bool__7)
            {
                PlayLoggedSFX(A1);
            }
        }

        // EventLog.pas:403
        public virtual bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            if (Trigger == TExecuteTrigger.etUser || ((Trigger == TExecuteTrigger.etEvent || (Trigger == TExecuteTrigger.etNewTurn && AoWHSMap.FRequestCombatType)) && AoWHSMap.FSeatedPlayer == FPlayer))
            {
                AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
                Result = true;
            }
            else
            {
                if (Trigger == TExecuteTrigger.etEvent)
                {
                    PlaySample((AnsiString)"SFX\\EVENTS\\NEWEVENT.WAV");
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
                AoWHSMap.CenterViewTo(X, Y, L);
                AoWHSMap.PlayCenterAnimation(X, Y, L);
            }
        }

        // EventLog.pas:419
        public void CenterViewTo(sbyte X, sbyte Y, sbyte L)
        {
            AoWHSMap.CenterViewTo(X, Y, L);
            AoWHSMap.PlayCenterAnimation(X, Y, L);
        }

    }

    // text
    public partial class TTextEventLog : TEventLog
    {
        public TTextEventLog(TECustomNode owner) : base(owner)
        { }

        // EventLog.pas:713
        public new const int ClassID = 131809;

        public AnsiString FText;
        // EventLog.pas:716
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwString(ref FText, 15);
        }

        // EventLog.pas:719
        public override AnsiString GetText()
        {
            AnsiString Result = default;
            Result = FText;
            return Result;
        }

        // EventLog.pas:722
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

    }

    // location
    public partial class TLocationTextEventLog : TTextEventLog
    {
        // EventLog.pas:725
        public TLocationTextEventLog(TECustomNode owner) : base(owner)
        {
            FXhx = -1;
            FYhx = -1;
            FLhx = -1;
        }

        // EventLog.pas:728
        public new const int ClassID = 131816;

        public sbyte FXhx;
        public sbyte FYhx;
        public sbyte FLhx;
        // EventLog.pas:731
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FXhx), 20);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FYhx), 21);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FLhx), 22);
        }

        // EventLog.pas:740
        public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            X = FXhx;
            Y = FYhx;
            L = FLhx;
        }

        // EventLog.pas:743
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            base.Execute(Mode, Trigger, Done);
            if (Trigger == TExecuteTrigger.etUser)
            {
                ViewLocation();
            }

            Result = false;
            return Result;
        }

        // EventLog.pas:737
        public void SetLocation(sbyte X, sbyte Y, sbyte L)
        {
            FXhx = X;
            FYhx = Y;
            FLhx = L;
        }

    }

    // title, message, layout, buttons, pressed button
    public partial class TMessageEventLog : TEventLog
    {
        // EventLog.pas:824
        public TMessageEventLog(TECustomNode owner) : base(owner)
        {
            var call__9 = new TStringList();
            FMessage = call__9;
            var call__10 = TranslateRStr(MessageRStr);
            FTitle = call__10;
            FButtons = TMessageEventLogButtons.mebOk;
            FPressedButton = TPressedEventLogButton.pebNone;
            FLayout = TMessageEventLogLayout.melNormal;
        }

        public AnsiString FTitle;
        public AnsiString FHeader;
        public TStrings FMessage;
        public TMessageEventLogLayout FLayout;
        public TMessageEventLogButtons FButtons;
        public TPressedEventLogButton FPressedButton;
        // EventLog.pas:832
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // EventLog.pas:835
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 30);
            Stream.rwData(Bytes.Of(ref FLayout, 1), 1, 31);
            Stream.rwData(Bytes.Of(ref FButtons, 1), 1, 32);
            Stream.rwString(ref FTitle, 33);
        }

        // EventLog.pas:842
        public virtual bool OK()
        {
            bool Result = default;
            FPressedButton = TPressedEventLogButton.pebOk;
            Result = true;
            return Result;
        }

        // EventLog.pas:845
        public virtual bool Cancel()
        {
            bool Result = default;
            FPressedButton = TPressedEventLogButton.pebCancel;
            Result = true;
            return Result;
        }

        // EventLog.pas:848
        public virtual bool Yes()
        {
            bool Result = default;
            FPressedButton = TPressedEventLogButton.pebYes;
            Result = true;
            return Result;
        }

        // EventLog.pas:854
        public virtual bool No()
        {
            bool Result = default;
            FPressedButton = TPressedEventLogButton.pebNo;
            Result = true;
            return Result;
        }

    }

    // message, help title, mode
    public partial class THelpMessageEventLog : TEventLog
    {
        // EventLog.pas:860
        public THelpMessageEventLog(TECustomNode owner) : base(owner)
        {
            var call__11 = new TStringList();
            FMessage = call__11;
            var call__12 = TranslateRStr(HelpRStr);
            FHelpFileName = call__12;
            FMode = 0;
        }

        public TStringList FMessage;
        public AnsiString FHelpFileName;
        public byte FMode;
        // EventLog.pas:868
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

    }

    // list of -byte callback records
    public partial class TExecuteEventLogCallBackList : TObject
    {
        public List<TExecuteEventLogInfo> FList = [];
        // EventLog.pas:283
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // EventLog.pas:277
        public TExecuteEventLogCallBackList()
        {
            ;
            var call__13 = new List<TExecuteEventLogInfo>();
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
        public void Add(in TExecuteEventLogInfo Info)
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

    // update count, changed callback
    public partial class TEventLogList : TENode
    {
        public TEventLogList(TECustomNode owner) : base(owner)
        { }

        public int FUpdateCount;
        public TNotifyEvent FOnChanged;
        // EventLog.pas:481
        public override void MainReadWrite(TEStorageStream Stream)
        {
            int I = default;
            if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
            {
                BeginUpdate();
            }

            base.MainReadWrite(Stream);
            if (Stream.StorageMode().Contains(TStorageModeElement.smRead))
            {
                if ((Stream.StorageMode() & TSet<TStorageModeElement>.Of(TStorageModeElement.smRead, TStorageModeElement.smCompare, TStorageModeElement.smTranslate)) == TSet<TStorageModeElement>.Of(TStorageModeElement.smRead))
                {
                    {
                        I = 0;
                        int end__14 = FChildren.Count - 1;
                        for (; I <= end__14; I += 1)
                        {
                            ((TEObject)FChildren.List[I]).Release();
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
                    ((TEObject)FChildren.List[I]).Release();
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
        public override int RemoveChild(TEObject Child)
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
        public override int AddChild(TEObject Child)
        {
            int Result = default;
            FChildren.Add(Child);
            if (Child.AddRef() == -1)
            {
                throw Exception.Create((AnsiString)"EObject doesn't support ReferenceCounting");
            }

            Result = FChildren.Count - 1;
            Changed();
            return Result;
        }

        // EventLog.pas:494
        public void Changed()
        {
            if (FUpdateCount == 0 && FOnChanged != null)
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
        public TEventLog GetEvents(int Index)
        {
            TEventLog Result = default;
            Result = (TEventLog)FChildren.List[Index];
            return Result;
        }

        // EventLog.pas:508
        public void AddEvent(TEventLog Event)
        {
            AddChild(Event);
        }

        // EventLog.pas:511
        public void RemoveEvent(TEventLog Event)
        {
            RemoveChild(Event);
        }

        // EventLog.pas:514
        public TEventLog FindID(int ID)
        {
            TEventLog Result = default;
            int I = default;
            I = GetCount();
            Result = null;
            while (I != 0 && Result == null)
            {
                I -= 1;
                if (((TEventLog)FChildren.List[I]).FID == ID)
                {
                    Result = (TEventLog)FChildren.List[I];
                    break;
                }
            }

            return Result;
        }

        // Engine.pas:631
    }

    // events, player, next ID
    public partial class TEventLogbook : TEObject
    {
        // EventLog.pas:532
        public new const int ClassID = 131808;


        public TEventLogList FEvents;
        public sbyte FPlayer;
        public int FNextID;
        // EventLog.pas:529
        public override void Destroy()
        {
            FEvents.Free();
            base.Destroy();
        }

        // EventLog.pas:547
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FEvents, 10);
            Stream.rwInteger(ref FNextID, 11);
        }

        // EventLog.pas:526
        public TEventLogbook(TECustomNode owner) : base(owner)
        {
            var call__17 = new TEventLogList(null);
            FEvents = call__17;
        }

        // EventLog.pas:535
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
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
        public TNotifyEvent GetOnChanged()
        {
            TNotifyEvent Result = default;
            Result = FEvents.FOnChanged;
            return Result;
        }

        // EventLog.pas:544
        public void SetOnChanged(TNotifyEvent Value)
        {
            FEvents.FOnChanged = Value;
        }

        // EventLog.pas:550
        public void ListEvents(TStrings Strings)
        {
            int I = default;
            {
                I = 0;
                int end__19 = FEvents.FChildren.Count - 1;
                for (; I <= end__19; I += 1)
                {
                    var call__20 = ((TEventLog)FEvents.FChildren.List[I]).GetVisible();
                    if (call__20)
                    {
                        var call__21 = ((TEventLog)FEvents.FChildren.List[I]).GetText();
                        Strings.AddObject(call__21, IntToObject(I));
                    }
                }
            }
        }

        // EventLog.pas:558
        public TEventLog GetLogs(int Index)
        {
            TEventLog Result = default;
            Result = (TEventLog)FEvents.FChildren.List[Index];
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
        public TEventLog FindID(int ID)
        {
            TEventLog Result = default;
            var call__23 = FEvents.FindID(ID);
            Result = call__23;
            return Result;
        }

        // EventLog.pas:575
        public int FindFirstLog(int Player, int Day)
        {
            int Result = default;
            Result = 0;
            while (Result != this.FEvents.FChildren.Count && (((TEventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player || ((TEventLog)this.FEvents.FChildren.List[Result]).FDay != Day))
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
                bool bool__26 = Result != this.FEvents.FChildren.Count;
                if (bool__26)
                {
                    bool bool__27 = ((TEventLog)this.FEvents.FChildren.List[Result]).FPlayer != Player || ((TEventLog)this.FEvents.FChildren.List[Result]).FDay != Day;
                    if (!bool__27)
                    {
                        var call__28 = ((TEventLog)this.FEvents.FChildren.List[Result]).EClass.ClassID;
                        bool__27 = call__28 != AClassID;
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
        public void AddEvent(TEventLog Event)
        {
            var call__29 = AoWHSMap.GetSynchronised();
            Assert(call__29, (AnsiString)"", "EventLog.pas", 704);
            Event.SetOwner(FEvents);
            var call__30 = CreateUniqueID();
            Event.FID = call__30;
            Event.NotifyLogged(this);
        }

        // EventLog.pas:710
        public void RemoveEvent(TEventLog Event)
        {
            FEvents.RemoveChild(Event);
        }

    }

    public partial class TUserEventLog : TEventLog
    {
        public TUserEventLog(TECustomNode owner) : base(owner)
        { }

        public TObject FData;
    }

    public partial class TEndTurnEventLog : TEventLog
    {
        public TEndTurnEventLog(TECustomNode owner) : base(owner)
        { }

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
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            Result = false;
            return Result;
        }

    }

    // message
    public partial class TShowMessageEventLog : TEventLog
    {
        // EventLog.pas:759
        public TShowMessageEventLog(TECustomNode owner) : base(owner)
        {
            var call__31 = new TStringList();
            FMessage = call__31;
        }

        // EventLog.pas:765
        public new const int ClassID = 131830;

        public TStringList FMessage;
        // EventLog.pas:762
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // EventLog.pas:768
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 20);
        }

    }

    // message, inherited player
    public partial class TGameOverEventLog : TEventLog
    {
        // EventLog.pas:771
        public TGameOverEventLog(TECustomNode owner) : base(owner)
        {
            var call__32 = new TStringList();
            FMessage = call__32;
        }

        // EventLog.pas:777
        public new const int ClassID = 131812;

        public TStringList FMessage;
        public sbyte FTargetPlayer;
        // EventLog.pas:774
        public override void Destroy()
        {
            FMessage.Free();
            base.Destroy();
        }

        // EventLog.pas:780
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwStrings(FMessage, 20);
        }

        // EventLog.pas:783
        public override AnsiString GetText()
        {
            AnsiString Result = default;
            var call__33 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
            switch (call__33.FGameOverStatus)
            {
                case TGameOverStatus.goLost:
                    var call__34 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__35 = call__34.GetName();
                    var call__36 = TranslateRStr(GameOverDefeatEventLogRStr);
                    var call__37 = Rtl.Rtl.Format(call__36, [call__35]);
                    Result = call__37;
                    break;
                case TGameOverStatus.goWon:
                    var call__38 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    var call__39 = call__38.GetName();
                    var call__40 = TranslateRStr(GameOverVictoryEventLogRStr);
                    var call__41 = Rtl.Rtl.Format(call__40, [call__39]);
                    Result = call__41;
                    break;
            }

            return Result;
        }

        // EventLog.pas:794
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            switch (Trigger)
            {
                case TExecuteTrigger.etUser:
                    if (AoWHSMap.FSeatedPlayer != FPlayer)
                    {
                        AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }

                    break;
                case TExecuteTrigger.etEvent:
                    AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
                    Result = true;
                    break;
                case TExecuteTrigger.etNewTurn:
                    AoWHSMap.TriggerExecuteEventLog(this, Mode, Done);
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
            AoWHSMap.GameOverEventHandled(FPlayer);
        }

    }

    // inherited text
    public partial class TNewDayEventLog : TTextEventLog
    {
        public TNewDayEventLog(TECustomNode owner) : base(owner)
        { }

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
        public override void PlayLoggedSFX(TEventLogbook A1)
        {
            if (AoWHSMap.FDay > 1)
            {
                base.PlayLoggedSFX(A1);
            }
        }

    }

    public partial class TShowBookEventLog : TEventLog
    {
        public TShowBookEventLog(TECustomNode owner) : base(owner)
        { }

        // EventLog.pas:818
        public new const int ClassID = 131818;

        // EventLog.pas:821
        public override bool GetVisible()
        {
            bool Result = default;
            Result = false;
            return Result;
        }

    }

    // inherited message and title
    public partial class TTutorialMessageEventLog : THelpMessageEventLog
    {
        // EventLog.pas:871
        public TTutorialMessageEventLog(TECustomNode owner) : base(owner)
        {
            var call__42 = TranslateRStr(TutorialRStr);
            FHelpFileName = call__42;
        }

    }

    // directory, default name, file name, title
    public partial class TSaveGameEventLog : TEventLog
    {
        // EventLog.pas:874
        public TSaveGameEventLog(TECustomNode owner) : base(owner)
        {
            var call__43 = TranslateRStr(SaveRStr);
            FTitle = call__43;
            FMode = TSet<TSaveGameEventLogModeElement>.Of();
        }

        public TSet<TSaveGameEventLogModeElement> FMode;
        public AnsiString FDirectory;
        public AnsiString FDefaultName;
        public AnsiString FFileName;
        public AnsiString FTitle;

    }

    // password, title, prompt, button
    public partial class TPasswordEventLog : TEventLog
    {
        // EventLog.pas:877
        public TPasswordEventLog(TECustomNode owner) : base(owner)
        {
            FPassword = (AnsiString)"";
            var call__44 = TranslateRStr(EnterPasswordRStr);
            FPrompt = call__44;
            var call__45 = TranslateRStr(PasswordVerificationRStr);
            FTitle = call__45;
            FPressedButton = TPressedEventLogButton.pebNone;
        }

        public AnsiString FPassword;
        public AnsiString FTitle;
        public AnsiString FPrompt;
        public TPressedEventLogButton FPressedButton;

    }

    public delegate void TExecuteDoneEvent(TEventLog Log);
    public struct TExecuteEventLogInfo
    {
        public TEventLog Log;
        public TExecuteDoneEvent Done;
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
        public static void RegisterEventLog(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TTextEventLog>(), EClass.Of<TEndTurnEventLog>(), EClass.Of<TGameOverEventLog>(), EClass.Of<TLocationTextEventLog>()]);
        }

        // EventLog.pas:889
        public static void DistributeEventLog(TEventLog Log, bool Execute)
        {
            int I = default;
            TPlayer Player = default;
            TEventLog CopyLog = default;
            {
                I = 0;
                int end__46 = AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__46; I += 1)
                {
                    var call__47 = AoWHSMap.FPlayers.GetPlayers(I);
                    Player = call__47;
                    if (Player.FStatus == TPlayerStatus.psPlaying)
                    {
                        var call__48 = Log.Copy(AoWEngine);
                        CopyLog = (TEventLog)call__48;
                        Player.FEventLogbook.AddEvent(CopyLog);
                        if (I == AoWHSMap.FSeatedPlayer && Execute)
                        {
                            CopyLog.Execute(false, TExecuteTrigger.etEvent, null);
                        }

                        CopyLog.Release();
                    }
                }
            }
        }

        // EventLog.pas:905
        public static void DistributeLocationEventLog(TEventLog Log, sbyte Player, sbyte X, sbyte Y, sbyte L, bool Execute)
        {
            int I = default;
            TPlayer TargetPlayer = default;
            TEventLog CopyLog = default;
            {
                I = 0;
                int end__49 = AoWHSMap.FPlayers.GetCount() - 1;
                for (; I <= end__49; I += 1)
                {
                    var call__50 = AoWHSMap.FPlayers.GetPlayers(I);
                    TargetPlayer = call__50;
                    bool bool__51 = TargetPlayer.FStatus == TPlayerStatus.psPlaying;
                    if (bool__51)
                    {
                        bool bool__52 = I == Player;
                        if (!bool__52)
                        {
                            var call__53 = AoWHSMap.FoggedForPlayer(X, Y, L, (sbyte)I);
                            bool__52 = !call__53;
                        }

                        bool__51 = bool__52;
                    }

                    if (bool__51)
                    {
                        var call__54 = Log.Copy(AoWEngine);
                        CopyLog = (TEventLog)call__54;
                        TargetPlayer.FEventLogbook.AddEvent(CopyLog);
                        bool bool__55 = I == AoWHSMap.FSeatedPlayer;
                        if (bool__55)
                        {
                            var call__56 = AoWHSMap.FPlayers.GetPlayers(I);
                            bool__55 = call__56.FPlayerType == TPlayerType.ptHuman;
                        }

                        if (bool__55 && Execute)
                        {
                            CopyLog.Execute(false, TExecuteTrigger.etEvent, null);
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
