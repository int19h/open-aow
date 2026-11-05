// Translated from NetworkE.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using Aow.Units.Engine;

namespace Aow.Units.NetworkE
{
    // FEngine, FOnReceive, FReceivedPackage, FSendStream, FReceiveStream, FPCC, FOnProcess
    public partial class TPlayerCommunicator : TEObject
    {
        internal TEngine FEngine;
        internal TEventList FOnReceive;
        internal TDataPackage FReceivedPackage;
        internal TMemoryStream FSendStream;
        internal TMemoryStream FReceiveStream;
        internal TPlayerCommunicationControl FPCC;
        internal TEventList FOnProcess;
        // NetworkE.pas:605
        public TPlayerCommunicator(TECustomNode owner) : base(owner)
        {
            FReceivedPackage = null;
            FOnReceive = new TEventList();
            FSendStream = new TMemoryStream();
            FReceiveStream = new TMemoryStream();
            FOnProcess = new TEventList();
        }

        // NetworkE.pas:629
        public virtual bool ActivatePCC(TPlayerCommunicationControl PCC)
        {
            bool Result;
            if (FPCC != null)
            {
                FPCC.Release();
                FPCC = null;
            }

            FPCC = PCC;
            if (FPCC != null)
            {
                FPCC.AddRef();
                Result = FPCC.Activate(this);
                if (!Result)
                {
                    FPCC.Release();
                    FPCC = null;
                }
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // NetworkE.pas:651
        public virtual void Process()
        {
            while (ReceiveDataPackage())
            {
            }

            FOnProcess.TriggerNotifyEvents(this);
        }

        // NetworkE.pas:615
        public override void Destroy()
        {
            FOnReceive.Free();
            FSendStream.Free();
            FReceiveStream.Free();
            FOnProcess.Free();
            if (FPCC != null)
            {
                FPCC.Release();
                FPCC = null;
            }

            base.Destroy();
        }

        // NetworkE.pas:658
        public bool ReceiveDataPackage()
        {
            bool Result = default;
            TEObject Obj = default;
            FReceiveStream.Clear();
            FReceiveStream.Position = 0;
            if (FPCC.ReceivePackageData(FReceiveStream))
            {
                try
                {
                    Obj = FEngine.RetrieveEObject(FReceiveStream, null);
                    if (Obj is TDataPackage)
                    {
                        FReceivedPackage = (TDataPackage)Obj;
                        FOnReceive.TriggerNotifyEvents(this);
                        FReceivedPackage.Release();
                    }
                    else
                    {
                        Obj.Free();
                    }
                }
                catch (System.Exception)
                {
                }

                FReceivedPackage = null;
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:685
        public bool SendDataPackage(TDataPackage Package, int ASender, int AReceiver)
        {
            Package.FSender = ASender;
            Package.FReceiver = AReceiver;
            FSendStream.Clear();
            FEngine.StoreEObject(FSendStream, Package, null);
            FSendStream.Position = 0;
            return FPCC.SendPackageData(AReceiver, FSendStream);
        }

        public TEngine Engine { get => FEngine; set => FEngine = value; }
        public TEventList OnReceive { get => FOnReceive; }
        public TDataPackage ReceivedPackage { get => FReceivedPackage; }
        public TPlayerCommunicationControl PCC { get => FPCC; }
        public TEventList OnProcess { get => FOnProcess; }

    }

    // FRefCount, FTokenCounter (stored with id 10. -1 until the executer numbers the event). The
    // base of the game's token events
    public partial class TTokenEvent : TEObject
    {
        // NetworkE.pas:950
        public new const int ClassID = NetworkE.CID_TokenEvent;

        public TTokenEvent(TECustomNode owner) : base(owner)
        {
            FRefCount = 1;
            FTokenCounter = -1;
        }

        internal int FRefCount;
        internal int FTokenCounter;
        // NetworkE.pas:969
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTokenCounter, 10);
        }

        // NetworkE.pas:943


        // NetworkE.pas:955
        public override int AddRef()
        {
            FRefCount += 1;
            return FRefCount;
        }

        // NetworkE.pas:961
        public override int Release()
        {
            FRefCount -= 1;
            int Result = FRefCount;
            if (Result == 0)
            {
                Destroy();
            }

            return Result;
        }

        // NetworkE.pas:975
        public virtual TExecuteResult Execute(TTokenExecuter TE) => TExecuteResult.erDone;

        public int TokenCounter { get => FTokenCounter; set => FTokenCounter = value; }

    }

    // FList (stored with id 5), a reference node of token events
    public partial class TTokenEventList : TEObject
    {
        // NetworkE.pas:995
        public new const int ClassID = NetworkE.CID_TokenEventList;

        internal TEReferenceNode FList;
        // NetworkE.pas:1000
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FList, 5);
        }

        // NetworkE.pas:982
        public TTokenEventList(TECustomNode owner) : base(owner)
        {
            FList = new TEReferenceNode(null);
        }

        // NetworkE.pas:1046
        public override void Clear()
        {
            FList.Clear();
        }

        // NetworkE.pas:1006
        public override void Assign(TEObject Source, TEngine AEngine)
        {
            FList.Clear();
            AddEvents((TTokenEventList)Source);
        }

        // NetworkE.pas:988
        public override void Destroy()
        {
            Clear();
            FList.Free();
            base.Destroy();
        }

        // NetworkE.pas:1012
        public int IndexOfTokenCounter(int ATokenCounter)
        {
            int Result = FList.Count;
            while (Result != 0)
            {
                Result -= 1;
                if (((TTokenEvent)FList.FChildren.List[Result]).FTokenCounter == ATokenCounter)
                {
                    return Result;
                }
            }

            return -1;
        }

        // NetworkE.pas:1024
        public int FindTE(int AClassID)
        {
            int C;
            int Result = 0;
            C = FList.Count;
            while (Result != C && ((TTokenEvent)FList.FChildren.List[Result]).EClass.ClassID != AClassID)
            {
                Result += 1;
            }

            if (Result == C)
            {
                Result = -1;
            }

            return Result;
        }

        // NetworkE.pas:1036
        public TTokenEvent GetEvent(int Index) => (TTokenEvent)FList.FChildren.List[Index];

        // NetworkE.pas:1041
        public int GetCount() => FList.Count;

        // NetworkE.pas:1056
        public void SortOnTokenCounter()
        {
            FList.FChildren.Sort(NetworkE.CompareTokenCounter);
        }

        // NetworkE.pas:1061
        public void AddEvents(TTokenEventList Events)
        {
            for (int I = 0, last = Events.Count - 1; I <= last; I++)
            {
                var item = Events[I];
                FList.AddChild(item);
            }
        }

        // NetworkE.pas:1069
        public void Add(TTokenEvent TE)
        {
            FList.AddChild(TE);
        }

        // NetworkE.pas:1074
        public void Delete(int Index)
        {
            var item = FList.Children[Index];
            FList.RemoveChild(item);
        }

        // NetworkE.pas:1080
        // { Inserts without TEReferenceNode.AddChild, so the reference is taken here }
        public void Insert(int Index, TTokenEvent TE)
        {
            TE.AddRef();
            FList.FChildren.Insert(Index, TE);
        }

        public int Count { get => GetCount(); }

        public TTokenEvent this[int Index] { get => GetEvent(Index); }

    }

    // FEvents, FTokenManager, FPlayer, FActive. The token events of one player
    public partial class TTokenControl : TObject
    {
        internal TTokenEventList FEvents;
        internal TTokenManager FTokenManager;
        internal int FPlayer;
        internal bool FActive;
        // NetworkE.pas:1189
        // { Keeps the events sorted on their token counters; an event without one goes to the end }
        public virtual void Add(TTokenEvent TE)
        {
            int I;
            if (TE.FTokenCounter != -1)
            {
                I = 0;
                while (I != FEvents.Count && FEvents[I].FTokenCounter < TE.FTokenCounter)
                {
                    I += 1;
                }

                if (I < FEvents.Count)
                {
                    if (FEvents[I].FTokenCounter == TE.FTokenCounter)
                    {
                        throw Exception.Create((AnsiString)"Double TokenCounter defined");
                    }

                    FEvents.Insert(I, TE);
                }
                else
                {
                    FEvents.Add(TE);
                }
            }
            else
            {
                FEvents.Add(TE);
            }
        }

        // NetworkE.pas:1293
        // { A local player's events end with a TPassTokenTE (after a TSyncTE when the executer checks the
//   sync); a remote player's events are prefetched only when they are complete }
        public virtual bool PrefetchTokenEvents(TTokenExecuter Executer)
        {
            bool Result;
            int N;
            TTokenEvent TE;
            if (Global() || Local())
            {
                if (FEvents.Count != 0 || Executer.FHoldTC != true)
                {
                    if (Local())
                    {
                        if (Executer.FOnSyncError != null)
                        {
                            TE = new TSyncTE(null);
                            var syncValue = Executer.GetSyncValue();
                            ((TSyncTE)TE).FSyncValue = syncValue;
                            FEvents.Insert(0, TE);
                            TE.Release();
                        }

                        TE = new TPassTokenTE(null);
                        FEvents.Add(TE);
                        TE.Release();
                    }

                    for (int I = 0, last = FEvents.Count - 1; I <= last; I++)
                    {
                        Executer.PrefetchTokenEvent(FEvents[I]);
                    }

                    FEvents.Clear();
                    Result = true;
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                if (Remote())
                {
                    N = GetPrefetchableTokenEvents(Executer);
                    if (N > 0)
                    {
                        for (int I = 0; I <= N - 1; I++)
                        {
                            Executer.PrefetchTokenEvent(FEvents[I]);
                        }

                        for (int I = N - 1; I >= 0; I--)
                        {
                            FEvents.Delete(I);
                        }

                        Result = true;
                    }
                    else
                    {
                        Result = false;
                    }
                }
                else
                {
                    Result = true;
                }
            }

            return Result;
        }

        // NetworkE.pas:1342
        // { A local player's prefetched events go to every other player }
        public virtual void TokenEventsPrefetched(TTokenExecuter Executer, TTokenEventList Events)
        {
            if (Local() && !Global() && Events.Count != 0)
            {
                FTokenManager.FTokenPackageDistributer.SendTokenEvents(FPlayer, NetworkE.AllPlayers, Events);
            }
        }

        // NetworkE.pas:1169
        public TTokenControl(TTokenManager AManager, int APlayer)
        {
            FEvents = new TTokenEventList(null);
            FTokenManager = AManager;
            FPlayer = APlayer;
            FActive = true;
            if (FTokenManager != null && FTokenManager.FTokenExecuter != null)
            {
                FTokenManager.FTokenExecuter.AddTC(this);
            }
        }

        // NetworkE.pas:1211
        public virtual void AddEvent(TTokenEvent TE)
        {
            if (Remote())
            {
                throw Exception.Create((AnsiString)"Cannot Add Remote events");
            }

            Add(TE);
        }

        // NetworkE.pas:1218
        public virtual void AddEventEx(TTokenEvent TE)
        {
            Add(TE);
        }

        // NetworkE.pas:1238
        public virtual void AddEvents(TTokenEventList Events)
        {
            for (int I = 0, last = Events.Count - 1; I <= last; I++)
            {
                AddEvent(Events[I]);
            }
        }

        // NetworkE.pas:1180
        public override void Destroy()
        {
            if (FTokenManager != null && FTokenManager.FTokenExecuter != null)
            {
                FTokenManager.FTokenExecuter.RemoveTC(this);
            }

            FEvents.Free();
            base.Destroy();
        }

        // NetworkE.pas:1223
        public void ClearEvents()
        {
            FEvents.Clear();
        }

        // NetworkE.pas:1228
        public TTokenEvent GetEvent(int Index) => FEvents[Index];

        // NetworkE.pas:1233
        public int GetCount() => FEvents.Count;

        // NetworkE.pas:1246
        public bool Local()
        {
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            return playerCommunicationControl.GetPlayers(FPlayer).FConnectionMode == TConnectionMode.cmLocal;
        }

        // NetworkE.pas:1252
        public bool Global()
        {
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            return playerCommunicationControl.GetPlayers(FPlayer).FConnectionMode == TConnectionMode.cmGlobal;
        }

        // NetworkE.pas:1258
        public bool Remote()
        {
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            return playerCommunicationControl.GetPlayers(FPlayer).FConnectionMode == TConnectionMode.cmRemote;
        }

        // NetworkE.pas:1266
        // { The number of events of a remote player that can be executed now: they must follow the
//   executer's token counter without a gap, up to and including the first TPassTokenTE }
        public int GetPrefetchableTokenEvents(TTokenExecuter Executer)
        {
            int Result;
            int I;
            bool InOrder;
            bool PassToken;
            if (Remote())
            {
                I = 0;
                InOrder = true;
                PassToken = false;
                while (InOrder && !PassToken && I < FEvents.Count)
                {
                    InOrder = FEvents[I].FTokenCounter == Executer.FTokenCounter + I;
                    PassToken = FEvents[I].EClass.ClassID == NetworkE.CID_PassTokenTE;
                    I += 1;
                }

                if (InOrder)
                {
                    Result = I;
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                Result = FEvents.Count;
            }

            return Result;
        }

        public TTokenEventList Events { get => FEvents; }
        public int Player { get => FPlayer; }
        public bool Active { get => FActive; set => FActive = value; }
        public int Count { get => GetCount(); }

    }

    // FActiveTC, FPrefetchTC, FActiveTCIndex, FEvents, FTCList, FTokenCounter, FRound,
    // FExecuteCount, FState, FOnEventsExecuted, FOnActivateTC, FOnSyncError, FOnGetSyncValue,
    // FOnExecuteTokenEvent, FExecuting, FLockCount, FSystemEvents, FHoldTC, FTokenManager
    public partial class TTokenExecuter : TObject
    {
        internal TTokenControl FActiveTC;
        internal TTokenControl FPrefetchTC;
        internal int FActiveTCIndex;
        internal TTokenEventList FEvents;
        internal TList FTCList;
        internal int FTokenCounter;
        internal int FRound;
        internal int FExecuteCount;
        internal TSet<TTokenExecuterState> FState;
        internal TEventList FOnEventsExecuted;
        internal TEventList FOnActivateTC;
        internal TNotifyEvent FOnSyncError;
        internal TGetSyncValueEvent FOnGetSyncValue;
        internal TTokenEventEvent FOnExecuteTokenEvent;
        internal bool FExecuting;
        internal int FLockCount;
        internal TTokenEventList FSystemEvents;
        internal bool FHoldTC;
        internal TTokenManager FTokenManager;
        // NetworkE.pas:1429
        public virtual TExecuteResult ExecuteTokenEvent(TTokenEvent TE)
        {
            TExecuteResult Result = default;
            FExecuting = true;
            if (FOnExecuteTokenEvent != null)
            {
                FOnExecuteTokenEvent(TE);
            }

            try
            {
                Result = TE.Execute(this);
            }
            catch (System.Exception)
            {
                ShowMessage((AnsiString)"Exception occured during " + (AnsiString)ClassNameOf(TE.GetType()));
                Result = TExecuteResult.erDone;
            }

            FExecuteCount += 1;
            FExecuting = false;
            return Result;
        }

        // NetworkE.pas:1350
        public TTokenExecuter(TTokenManager AManager)
        {
            FTokenManager = AManager;
            FEvents = new TTokenEventList(null);
            FTCList = new TList();
            FOnEventsExecuted = new TEventList();
            FOnActivateTC = new TEventList();
            FSystemEvents = new TTokenEventList(null);
        }

        // NetworkE.pas:1361
        public override void Destroy()
        {
            Reset();
            FEvents.Free();
            FTCList.Free();
            FOnEventsExecuted.Free();
            FOnActivateTC.Free();
            FSystemEvents.Free();
            base.Destroy();
        }

        // NetworkE.pas:1372
        public int GetSyncValue()
        {
            int Result = default;
            if (FOnGetSyncValue != null)
            {
                FOnGetSyncValue(ref Result);
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // NetworkE.pas:1380
        public TTokenControl GetTC(int Index) => (TTokenControl)FTCList.List[Index];

        // NetworkE.pas:1385
        public void AddTC(TTokenControl TC)
        {
            FTCList.Add(TC);
        }

        // NetworkE.pas:1390
        public void DeleteTC(int Index)
        {
            FTCList.Delete(Index);
        }

        // NetworkE.pas:1395
        public void RemoveTC(TTokenControl TC)
        {
            FTCList.Remove(TC);
        }

        // NetworkE.pas:1400
        public void InsertTC(int Index, TTokenControl TC)
        {
            FTCList.Insert(Index, TC);
        }

        // NetworkE.pas:1405
        public void Reset()
        {
            FTCList.Clear();
            FActiveTC = null;
            FActiveTCIndex = -1;
            FState = TSet<TTokenExecuterState>.Of();
            FRound = 0;
            FTokenCounter = 0;
        }

        // NetworkE.pas:1416
        // { Numbers an event that has no token counter yet (or an old one) and queues it }
        public void PrefetchTokenEvent(TTokenEvent TE)
        {
            if (FState.Contains(TTokenExecuterState.tsPrefetching))
            {
                if (TE.FTokenCounter >= FTokenCounter || TE.FTokenCounter == -1)
                {
                    TE.FTokenCounter = FEvents.Count + FTokenCounter;
                    FEvents.Add(TE);
                }

                FExecuteCount = 0;
            }
        }

        // NetworkE.pas:1444
        public TTokenEvent GetExecutingTokenEvent()
        {
            TTokenEvent Result;
            if (FState.Contains(TTokenExecuterState.tsExecuting) && FEvents.Count != 0)
            {
                Result = FEvents[0];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // NetworkE.pas:1452
        public bool InsertExecuteEvent(TTokenEvent TE)
        {
            bool Result;
            if (FState.Contains(TTokenExecuterState.tsExecuting))
            {
                FEvents.Add(TE);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:1463
        public void Initialize(int AIndex)
        {
            FState = TSet<TTokenExecuterState>.Of();
            if (AIndex < FTCList.Count)
            {
                FActiveTC = (TTokenControl)FTCList[AIndex];
                FActiveTCIndex = AIndex;
                FRound = 0;
            }
        }

        // NetworkE.pas:1474
        public void ActivateTC(int Index)
        {
            FActiveTC = (TTokenControl)FTCList[Index];
            FActiveTC.FActive = true;
            FActiveTCIndex = Index;
            FOnActivateTC.TriggerNotifyEvents(this);
            if (FState.Contains(TTokenExecuterState.tsExecuting))
            {
                FState = FState + TSet<TTokenExecuterState>.Of(TTokenExecuterState.tsTCActivated);
            }
        }

        // NetworkE.pas:1485
        // { The next active token control after the current one; a wrap starts the next round }
        public void ActivateNextTC()
        {
            int N;
            N = 0;
            while (true)
            {
                FActiveTCIndex += 1;
                N += 1;
                if (FTCList.Count <= FActiveTCIndex)
                {
                    FActiveTCIndex = 0;
                    FRound += 1;
                }

                if (((TTokenControl)FTCList[FActiveTCIndex]).FActive || N > FTCList.Count)
                    break;
            }

            if (((TTokenControl)FTCList[FActiveTCIndex]).FActive)
            {
                ActivateTC(FActiveTCIndex);
            }
        }

        // NetworkE.pas:1503
        public void Lock()
        {
            FLockCount += 1;
        }

        // NetworkE.pas:1508
        public void Unlock()
        {
            FLockCount -= 1;
        }

        // NetworkE.pas:1513
        public bool GetLocked() => FLockCount != 0;

        // NetworkE.pas:1519
        // { The system events run between the token controls' events; True while some remain }
        public bool ProcessSystemTokenEvents()
        {
            bool Result;
            TExecuteResult R;
            if (!FState.Contains(TTokenExecuterState.tsExecuting) && FSystemEvents.Count > 0 && FLockCount == 0)
            {
                if (FSystemEvents[0].FTokenCounter == -1)
                {
                    FExecuteCount = 0;
                    FSystemEvents[0].FTokenCounter = FTokenCounter;
                }

                R = ExecuteTokenEvent(FSystemEvents[0]);
                if (R == TExecuteResult.erDone || R == TExecuteResult.erDoneHalt)
                {
                    FSystemEvents.Delete(0);
                    FTokenCounter += 1;
                    FExecuteCount = 0;
                }

                Result = FSystemEvents.Count != 0;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:1543
        public void AddSystemEvent(TTokenEvent TE)
        {
            FSystemEvents.Add(TE);
        }

        // NetworkE.pas:1550
        // { Lets the active token control prefetch its events, executes them and activates the next token
//   control, for at most one round of token controls }
        public void Process()
        {
            int I;
            bool Prefetched;
            TExecuteResult R;
            if (FActiveTC != null && !ProcessSystemTokenEvents())
            {
                I = 0;
                Prefetched = true;
                R = TExecuteResult.erDone;
                while (FLockCount == 0 && Prefetched == true && R == TExecuteResult.erDone && I < FTCList.Count)
                {
                    if (!FState.Contains(TTokenExecuterState.tsExecuting))
                    {
                        FState = FState + TSet<TTokenExecuterState>.Of(TTokenExecuterState.tsPrefetching);
                        FEvents.Clear();
                        Prefetched = FActiveTC.PrefetchTokenEvents(this);
                        FState = FState - TSet<TTokenExecuterState>.Of(TTokenExecuterState.tsPrefetching);
                        if (Prefetched == true)
                        {
                            FState = FState + TSet<TTokenExecuterState>.Of(TTokenExecuterState.tsExecuting);
                            FPrefetchTC = FActiveTC;
                            FActiveTC.TokenEventsPrefetched(this, FEvents);
                        }
                    }

                    if (FState.Contains(TTokenExecuterState.tsExecuting))
                    {
                        while (R == TExecuteResult.erDone && FEvents.Count != 0)
                        {
                            R = ExecuteTokenEvent(FEvents[0]);
                            if (R == TExecuteResult.erDone || R == TExecuteResult.erDoneHalt)
                            {
                                FEvents.Delete(0);
                                FTokenCounter += 1;
                                FExecuteCount = 0;
                            }
                        }

                        if (FEvents.Count == 0)
                        {
                            FState = FState - TSet<TTokenExecuterState>.Of(TTokenExecuterState.tsExecuting);
                            FOnEventsExecuted.TriggerNotifyEvents(this);
                            if (!FHoldTC && !FState.Contains(TTokenExecuterState.tsTCActivated))
                            {
                                ActivateNextTC();
                            }

                            FState = FState - TSet<TTokenExecuterState>.Of(TTokenExecuterState.tsTCActivated);
                            FPrefetchTC = null;
                            I += 1;
                        }
                    }
                }
            }
        }

        public TTokenControl ActiveTC { get => FActiveTC; }
        public TTokenControl PrefetchTC { get => FPrefetchTC; }
        public int ActiveTCIndex { get => FActiveTCIndex; }
        public int TokenCounter { get => FTokenCounter; }
        public int Round { get => FRound; }
        public int ExecuteCount { get => FExecuteCount; }
        public TSet<TTokenExecuterState> State { get => FState; }
        public bool Locked { get => GetLocked(); }
        public bool HoldTC { get => FHoldTC; set => FHoldTC = value; }
        public bool Executing { get => FExecuting; }
        public TEventList OnEventsExecuted { get => FOnEventsExecuted; }
        public TEventList OnActivateTC { get => FOnActivateTC; }
        public TNotifyEvent OnSyncError { get => FOnSyncError; set => FOnSyncError = value; }
        public TGetSyncValueEvent OnGetSyncValue { get => FOnGetSyncValue; set => FOnGetSyncValue = value; }
        public TTokenEventEvent OnExecuteTokenEvent { get => FOnExecuteTokenEvent; set => FOnExecuteTokenEvent = value; }

    }

    // FTokenExecuter, FTokenPackageDistributer, FOwner, FTokenControls (one per player, nil for a
    // free slot), FOnMessage
    public partial class TTokenManager : TObject
    {

        public TTokenManager(TObject owner)
        {
            FOwner = owner;
            FTokenControls = new TList();
            FTokenExecuter = CreateTokenExecuter();
            FTokenPackageDistributer = CreateTokenPackageDistributer();
        }

        internal TTokenExecuter FTokenExecuter;
        internal TTokenPackageDistributer FTokenPackageDistributer;
        internal TObject FOwner;
        internal TList FTokenControls;
        internal TNetworkMessageEvent FOnMessage;
        // NetworkE.pas:1642
        public virtual TTokenExecuter CreateTokenExecuter() => new TTokenExecuter(this);

        // NetworkE.pas:1647
        public virtual TTokenPackageDistributer CreateTokenPackageDistributer() => new TTokenPackageDistributer(this);

        // NetworkE.pas:1652
        public virtual TTokenControl CreateTokenControl(int APlayer) => new TTokenControl(this, APlayer);

        // NetworkE.pas:1628
        public virtual void Initialize(TPlayerCommunicator PC)
        {
            FTokenPackageDistributer.SetPC(PC);
            if (PC.FPCC.GetCount() > 0)
            {
                ClearPlayers();
                for (int I = 0, last = PC.FPCC.GetCount() - 1; I <= last; I++)
                {
                    CreatePlayer(I);
                }
            }

            FTokenExecuter.Initialize(0);
        }

        // NetworkE.pas:1657
        public virtual void Process()
        {
            if (FTokenExecuter != null)
            {
                FTokenExecuter.Process();
            }
        }

        // NetworkE.pas:1694
        // { A player is ready when nothing of his waits: no system events, no token events, and his events
//   are not being executed }
        public virtual bool PlayerReady(int APlayer)
        {
            bool Result;
            if (APlayer < FTokenControls.Count && FTokenControls[APlayer] != null)
            {
                bool condition = FTokenExecuter.FSystemEvents.Count != 0 || ((TTokenControl)FTokenControls[APlayer]).GetCount() != 0;
                if (!condition)
                {
                    var item = FTokenControls[APlayer];
                    condition = FTokenExecuter.FPrefetchTC == item && FTokenExecuter.FState.Contains(TTokenExecuterState.tsExecuting);
                }

                if (condition)
                {
                    Result = false;
                }
                else
                {
                    Result = true;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:1663
        public virtual void CreatePlayer(int APlayer)
        {
            if (APlayer >= FTokenControls.Count || FTokenControls[APlayer] == null)
            {
                while (APlayer >= FTokenControls.Count)
                {
                    FTokenControls.Add(null);
                }

                var createTokenControl = CreateTokenControl(APlayer);
                FTokenControls[APlayer] = createTokenControl;
            }
        }

        // NetworkE.pas:1673
        public virtual void DeletePlayer(int APlayer)
        {
            if (APlayer < FTokenControls.Count && FTokenControls[APlayer] != null)
            {
                ((TObject)FTokenControls[APlayer]).Free();
                FTokenControls[APlayer] = null;
            }
        }

        // NetworkE.pas:1682
        public virtual void ClearPlayers()
        {
            while (FTokenControls.Count != 0)
            {
                if (FTokenControls[FTokenControls.Count - 1] != null)
                {
                    ((TObject)FTokenControls[FTokenControls.Count - 1]).Free();
                }

                FTokenControls.Delete(FTokenControls.Count - 1);
            }
        }

        // NetworkE.pas:1606
        // { The original does not call the inherited constructor. }


        // NetworkE.pas:1614
        public override void Destroy()
        {
            ClearPlayers();
            FTokenControls.Free();
            FTokenExecuter.Free();
            FTokenPackageDistributer.Free();
            base.Destroy();
        }

        // NetworkE.pas:1623
        public int GetCount() => FTokenControls.Count;

        // NetworkE.pas:1710
        public TTokenControl GetTokenControl(int APlayer)
        {
            TTokenControl Result;
            if (APlayer < FTokenControls.Count)
            {
                Result = (TTokenControl)FTokenControls[APlayer];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // NetworkE.pas:1718
        public void TriggerMessage(AnsiString Msg)
        {
            if (FOnMessage != null)
            {
                FOnMessage(this, Msg);
            }
        }

        public TTokenExecuter TokenExecuter { get => FTokenExecuter; }
        public TTokenPackageDistributer TokenPackageDistributer { get => FTokenPackageDistributer; }
        public TObject Owner { get => FOwner; }
        public int Count { get => GetCount(); }

        public sealed class TokenControlsAccessor(TTokenManager target)
        {
            public TTokenControl this[int APlayer] { get => target.GetTokenControl(APlayer); }
        }

        public TokenControlsAccessor TokenControls => new TokenControlsAccessor(this);
        public TNetworkMessageEvent OnMessage { get => FOnMessage; set => FOnMessage = value; }

    }

    public enum TConnectionMode : byte
    {
        cmLocal = 0,
        cmRemote = 1,
        cmGlobal = 2
    }

    public enum TExecuteResult : byte
    {
        erRetry = 0,
        erDone = 1,
        erDoneHalt = 2
    }

    public enum TTokenExecuterState : byte
    {
        tsPrefetching = 0,
        tsExecuting = 1,
        tsTCActivated = 2
    }

    public delegate bool TSendDataEvent(object Data, int Size, int SystemID);
    public delegate void TGetSyncValueEvent(ref int Value);
    public delegate void TTokenEventEvent(TTokenEvent TE);
    public delegate void TNetworkMessageEvent(TObject Sender, AnsiString Msg);
    // { The original reaches TENode's child list (Engine.TENode.FChildren, +8) from this unit; the
//     recovered Engine.pas keeps the field private, so this layout view of TENode reads it (a
//     typecast of a field or parameter compiles to no code) }
    public partial class TENodeView : TECustomNode
    {
        public TENodeView(TECustomNode owner) : base(owner)
        { }

        public TEObjectList FChildren;
    }

    // FSender, FReceiver (player indices, stored with ids 1 and 2), FRefCount. The base of the
    // packages that TPlayerCommunicator sends
    public partial class TDataPackage : TEObject
    {
        internal int FSender;
        internal int FReceiver;
        internal int FRefCount;
        // NetworkE.pas:592
        // { The original does not call the inherited ReadWrite. }
        public override void ReadWrite(TEStorageStream Stream)
        {
            Stream.rwInteger(ref FSender, 1);
            Stream.rwInteger(ref FReceiver, 2);
        }

        // NetworkE.pas:598
        public override void MainReadWrite(TEStorageStream Stream)
        {
            ReadWrite(Stream);
        }

        // NetworkE.pas:566
        public TDataPackage(TECustomNode owner) : base(owner)
        {
            FRefCount = 1;
        }

        // NetworkE.pas:572
        public override int AddRef()
        {
            FRefCount += 1;
            return FRefCount;
        }

        // NetworkE.pas:578
        public override int Release()
        {
            FRefCount -= 1;
            int Result = FRefCount;
            if (Result == 0)
            {
                Destroy();
            }

            return Result;
        }

        // NetworkE.pas:586
        public virtual AnsiString Name() => (AnsiString)ClassNameOf(this.GetType());

        public int Sender { get => FSender; set => FSender = value; }
        public int Receiver { get => FReceiver; set => FReceiver = value; }

    }

    // FConnectionMode (stored with id 5), FSystemID (id 6). One per player
    public partial class TPlayerCommunicationSettings : TEObject
    {
        internal TConnectionMode FConnectionMode;
        internal int FSystemID;
        // NetworkE.pas:452
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FConnectionMode, 1), 1, 5);
            Stream.rwInteger(ref FSystemID, 6);
        }

        // NetworkE.pas:446
        public TPlayerCommunicationSettings(TECustomNode owner) : base(owner)
        {
            FConnectionMode = TConnectionMode.cmLocal;
        }

        public TConnectionMode ConnectionMode { get => FConnectionMode; set => FConnectionMode = value; }
        public int SystemID { get => FSystemID; set => FSystemID = value; }

    }

    // the node that owns the players' settings
    public partial class TPlayerCommunicationSettingsList : TENode
    {
        public TPlayerCommunicationSettingsList(TECustomNode owner) : base(owner)
        { }

        // NetworkE.pas:461
        public TPlayerCommunicationSettings GetPlayers(int Index) => (TPlayerCommunicationSettings)this.FChildren.List[Index];

        public sealed class PlayersAccessor(TPlayerCommunicationSettingsList target)
        {
            public TPlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
        }

        public PlayersAccessor Players => new PlayersAccessor(this);

    }

    // FRefCount, FPlayerCommunicator, FPCSList. The base player communication control: one system,
    // no transport
    public partial class TPlayerCommunicationControl : TObject
    {
        internal int FRefCount;
        internal TPlayerCommunicator FPlayerCommunicator;
        internal TPlayerCommunicationSettingsList FPCSList;
        // NetworkE.pas:549
        public virtual int GetLocalSystemID() => 1;

        // NetworkE.pas:532
        public virtual int GetSystemCount() => 1;

        // NetworkE.pas:516
        public virtual bool Activate(TPlayerCommunicator PC)
        {
            FPlayerCommunicator = PC;
            return true;
        }

        // NetworkE.pas:498
        public virtual TPlayerCommunicationSettings CreatePCS() => new TPlayerCommunicationSettings(null);

        // NetworkE.pas:493
        public virtual AnsiString Name() => (AnsiString)"";

        // NetworkE.pas:554
        public virtual bool ReceivePackageData(TMemoryStream Stream) => false;

        // NetworkE.pas:559
        public virtual bool SendPackageData(int APlayer, TMemoryStream Stream) => false;

        // NetworkE.pas:468
        public TPlayerCommunicationControl()
        {
            FRefCount = 1;
            FPCSList = new TPlayerCommunicationSettingsList(null);
        }

        // NetworkE.pas:475
        public override void Destroy()
        {
            FPCSList.Free();
            base.Destroy();
        }

        // NetworkE.pas:481
        public void AddRef()
        {
            FRefCount += 1;
        }

        // NetworkE.pas:486
        public void Release()
        {
            FRefCount -= 1;
            if (FRefCount == 0)
            {
                Destroy();
            }
        }

        // NetworkE.pas:503
        public TPlayerCommunicationSettings GetPlayers(int Index)
        {
            TPlayerCommunicationSettings Result;
            if (Index >= 0 && Index < FPCSList.Count)
            {
                Result = FPCSList.Players[Index];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // NetworkE.pas:511
        public int GetCount() => FPCSList.Count;

        // NetworkE.pas:522
        public bool Setup(int ACount)
        {
            FPCSList.Clear();
            for (int I = 0; I <= ACount - 1; I++)
            {
                CreatePCS().Owner = FPCSList;
            }

            return true;
        }

        // NetworkE.pas:537
        public int IndexOfSystemID(int ASystemID)
        {
            int Result = GetCount();
            while (Result != 0)
            {
                Result -= 1;
                if (FPCSList.Players[Result].FSystemID == ASystemID)
                {
                    return Result;
                }
            }

            return -1;
        }

        public int Count { get => GetCount(); }

        public sealed class PlayersAccessor(TPlayerCommunicationControl target)
        {
            public TPlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
        }

        public PlayersAccessor Players => new PlayersAccessor(this);
        public TPlayerCommunicator PlayerCommunicator { get => FPlayerCommunicator; }
        public TPlayerCommunicationSettingsList PCSList { get => FPCSList; }

    }

    // FList, the memory streams of the packages that wait to be received
    public partial class TPackageDataList : TObject
    {
        internal TList FList;
        // NetworkE.pas:870
        public TPackageDataList()
        {
            FList = new TList();
        }

        // NetworkE.pas:876
        public override void Destroy()
        {
            Clear();
            FList.Free();
            base.Destroy();
        }

        // NetworkE.pas:883
        public int GetCount() => FList.Count;

        // NetworkE.pas:888
        public TMemoryStream GetPackageData(int Index) => (TMemoryStream)FList.List[Index];

        // NetworkE.pas:893
        public void Clear()
        {
            TList L;
            L = FList;
            while (L.Count != 0)
            {
                ((TObject)L.List[L.Count - 1]).Free();
                L.Delete(L.Count - 1);
            }
        }

        // NetworkE.pas:905
        public void Add(TMemoryStream Stream)
        {
            TMemoryStream S;
            S = new TMemoryStream();
            S.Size = Stream.Size;
            var size = S.Size;
            Move(Stream.Memory.AsSpan(0, size), S.Memory.AsSpan(0, size), size);
            FList.Add(S);
        }

        // NetworkE.pas:915
        public void AddData(object Data, int Size)
        {
            TMemoryStream S;
            S = new TMemoryStream();
            S.Size = Size;
            Move(BufferBytes(Data)[..Size], S.Memory.AsSpan(0, Size), Size);
            FList.Add(S);
        }

        // NetworkE.pas:925
        public void Insert(int Index, TMemoryStream Stream)
        {
            TMemoryStream S;
            S = new TMemoryStream();
            S.Size = Stream.Size;
            var size = S.Size;
            Move(Stream.Memory.AsSpan(0, size), S.Memory.AsSpan(0, size), size);
            FList.Insert(Index, S);
        }

        // NetworkE.pas:935
        public void Delete(int Index)
        {
            ((TObject)FList.List[Index]).Free();
            FList.Delete(Index);
        }

        public int Count { get => GetCount(); }

        public sealed class PackageDataAccessor(TPackageDataList target)
        {
            public TMemoryStream this[int Index] { get => target.GetPackageData(Index); }
        }

        public PackageDataAccessor PackageData => new PackageDataAccessor(this);

    }

    // FLocalSystemID, FPackageDataList, FOnSendData. The control of a network game: the transport
    // sends through OnSendData and hands what arrives to DataReceived
    public partial class TRemotePCC : TPlayerCommunicationControl
    {
        internal int FLocalSystemID;
        internal TPackageDataList FPackageDataList;
        internal TSendDataEvent FOnSendData;
        // NetworkE.pas:819
        public override int GetLocalSystemID() => FLocalSystemID;

        // NetworkE.pas:782
        // { The number of different system IDs among the players }
        public override int GetSystemCount()
        {
            int J;
            int ID;
            bool Unique;
            int Result = 0;
            for (int I = 0, last = FPCSList.Count - 1; I <= last; I++)
            {
                ID = FPCSList.Players[I].FSystemID;
                Unique = true;
                J = 0;
                while (J != I && Unique)
                {
                    Unique = ID != FPCSList.Players[J].FSystemID;
                    J += 1;
                }

                if (Unique)
                {
                    Result += 1;
                }
            }

            return Result;
        }

        // NetworkE.pas:776
        public override AnsiString Name() => (AnsiString)"Remote";

        // NetworkE.pas:824
        public override bool ReceivePackageData(TMemoryStream Stream)
        {
            bool Result;
            if (FPackageDataList.Count != 0)
            {
                Stream.Size = FPackageDataList.PackageData[0].Size;
                var size = Stream.Size;
                Move(FPackageDataList.PackageData[0].Memory.AsSpan(0, size), Stream.Memory.AsSpan(0, size), size);
                FPackageDataList.Delete(0);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:852
        // { A package for every player (AllPlayers) goes to system 0, the transport's broadcast; a package
//   for every player or for a player on this system is also received here. }
        public override bool SendPackageData(int APlayer, TMemoryStream Stream)
        {
            bool Result;
            int SystemID;
            if (APlayer == NetworkE.AllPlayers)
            {
                SystemID = 0;
            }
            else
            {
                SystemID = FPCSList.Players[APlayer].FSystemID;
            }

            if (SystemID != FLocalSystemID)
            {
                var size = Stream.Size;
                Result = SendData(Stream.Memory, size, SystemID);
            }
            else
            {
                Result = true;
            }

            if (APlayer == NetworkE.AllPlayers || SystemID == FLocalSystemID)
            {
                FPackageDataList.Add(Stream);
            }

            return Result;
        }

        // NetworkE.pas:759
        public TRemotePCC() : base()
        {
            FPackageDataList = new TPackageDataList();
        }

        // NetworkE.pas:765
        public override void Destroy()
        {
            FPackageDataList.Free();
            base.Destroy();
        }

        // NetworkE.pas:771
        public void Clear()
        {
            FPackageDataList.Clear();
        }

        // NetworkE.pas:803
        public void SetConnectionMode(int Index, TConnectionMode Mode)
        {
            FPCSList.Players[Index].FConnectionMode = Mode;
        }

        // NetworkE.pas:808
        public void SetSystemID(int Index, int ASystemID)
        {
            FPCSList.Players[Index].FSystemID = ASystemID;
        }

        // NetworkE.pas:813
        public void Setup(int ACount, int ALocalSystemID)
        {
            base.Setup(ACount);
            FLocalSystemID = ALocalSystemID;
        }

        // NetworkE.pas:837
        public void DataReceived(object Data, int Size, int SystemID)
        {
            FPackageDataList.AddData(Data, Size);
        }

        // NetworkE.pas:842
        public bool SendData(object Data, int Size, int SystemID)
        {
            bool Result = default;
            if (FOnSendData != null)
            {
                Result = FOnSendData(Data, Size, SystemID);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        public TSendDataEvent OnSendData { get => FOnSendData; set => FOnSendData = value; }

    }

    // FPackageDataList. The control of a game on one system: what is sent is received again
    public partial class TLocalPCC : TPlayerCommunicationControl
    {
        internal TPackageDataList FPackageDataList;
        // NetworkE.pas:709
        public override int GetSystemCount() => 1;

        // NetworkE.pas:714
        public override AnsiString Name() => (AnsiString)"Local";

        // NetworkE.pas:724
        public override bool ReceivePackageData(TMemoryStream Stream)
        {
            bool Result;
            if (FPackageDataList.Count != 0)
            {
                Stream.Size = FPackageDataList.PackageData[0].Size;
                var size = Stream.Size;
                Move(FPackageDataList.PackageData[0].Memory.AsSpan(0, size), Stream.Memory.AsSpan(0, size), size);
                FPackageDataList.Delete(0);
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // NetworkE.pas:737
        public override bool SendPackageData(int APlayer, TMemoryStream Stream)
        {
            FPackageDataList.Add(Stream);
            return true;
        }

        // NetworkE.pas:697
        public TLocalPCC() : base()
        {
            FPackageDataList = new TPackageDataList();
        }

        // NetworkE.pas:703
        public override void Destroy()
        {
            FPackageDataList.Free();
            base.Destroy();
        }

        // NetworkE.pas:719
        public void Clear()
        {
            FPackageDataList.Clear();
        }

        // NetworkE.pas:743
        public new void Setup(int ACount)
        {
            base.Setup(ACount);
            for (int I = FPCSList.Count - 1; I >= 0; I--)
            {
                var localSystemID = GetLocalSystemID();
                FPCSList.Players[I].FSystemID = localSystemID;
            }
        }

        // NetworkE.pas:752
        public void SetConnectionMode(int Index, TConnectionMode Mode)
        {
            FPCSList.Players[Index].FConnectionMode = Mode;
        }

    }

    // FTokenEvents (stored with id 5)
    public partial class TTokenPackage : TDataPackage
    {
        // NetworkE.pas:1100
        public new const int ClassID = NetworkE.CID_TokenPackage;

        internal TTokenEventList FTokenEvents;
        // NetworkE.pas:1105
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FTokenEvents, 5);
        }

        // NetworkE.pas:1088
        public TTokenPackage(TECustomNode owner) : base(owner)
        {
            FTokenEvents = new TTokenEventList(null);
        }

        // NetworkE.pas:1094
        public override void Destroy()
        {
            FTokenEvents.Free();
            base.Destroy();
        }

        public TTokenEventList TokenEvents { get => FTokenEvents; }

    }

    // FTokenManager, FPC. Sends the token events of the local players in token packages and files
    // the received ones with their players' token controls
    public partial class TTokenPackageDistributer : TObject
    {
        internal TTokenManager FTokenManager;
        internal TPlayerCommunicator FPC;
        // NetworkE.pas:1136
        // { The token events of a package from another system go to the token control of the sending player,
//   unless they were already executed or are already there. }
        public virtual void TokenPackageReceived(TTokenPackage TP)
        {
            if (FTokenManager != null)
            {
                if (FPC.FPCC.GetLocalSystemID() != FPC.FPCC.GetPlayers(TP.FSender).FSystemID)
                {
                    for (int I = 0, last = TP.FTokenEvents.Count - 1; I <= last; I++)
                    {
                        if (TP.FTokenEvents[I].FTokenCounter >= FTokenManager.FTokenExecuter.FTokenCounter)
                        {
                            var item = TP.FTokenEvents[I];
                            if (FTokenManager.GetTokenControl(TP.FSender).FEvents.IndexOfTokenCounter(item.FTokenCounter) == -1)
                            {
                                var fTokenEventsItem = TP.FTokenEvents[I];
                                FTokenManager.GetTokenControl(TP.FSender).Add(fTokenEventsItem);
                                FTokenManager.GetTokenControl(TP.FSender).FEvents.SortOnTokenCounter();
                            }
                            else
                            {
                                FTokenManager.TriggerMessage((AnsiString)"TokenEvent already exists");
                            }
                        }
                        else
                        {
                            FTokenManager.TriggerMessage((AnsiString)"TokenEvent already executed");
                        }
                    }
                }
            }
        }

        // NetworkE.pas:1113
        public TTokenPackageDistributer(TTokenManager AManager)
        {
            FTokenManager = AManager;
        }

        // NetworkE.pas:1157
        public virtual void SendTokenEvents(int APlayer, int AReceiver, TTokenEventList Events)
        {
            TTokenPackage TP;
            TP = new TTokenPackage(null);
            TP.FTokenEvents.Assign(Events, null);
            FPC.SendDataPackage(TP, APlayer, AReceiver);
            TP.Release();
        }

        // NetworkE.pas:1119
        public void SetPC(TPlayerCommunicator PC)
        {
            if (FPC != null)
            {
                FPC.FOnReceive.Remove(DataPackageReceived);
            }

            FPC = PC;
            if (FPC != null)
            {
                FPC.FOnReceive.Add(DataPackageReceived);
            }
        }

        // NetworkE.pas:1128
        public void DataPackageReceived(TObject Sender)
        {
            if (((TPlayerCommunicator)Sender).FReceivedPackage is TTokenPackage)
            {
                TokenPackageReceived((TTokenPackage)((TPlayerCommunicator)Sender).FReceivedPackage);
            }
        }

        public TPlayerCommunicator PC { get => FPC; set => SetPC(value); }

    }

    // ends the turn of the token control that prefetched it
    public partial class TPassTokenTE : TTokenEvent
    {
        public new const int ClassID = NetworkE.CID_PassTokenTE;

        public TPassTokenTE(TECustomNode owner) : base(owner)
        { }

    }

    // FSyncValue (stored with id), the sync value of the system that sent it. another value on
    // execution triggers OnSyncError
    public partial class TSyncTE : TTokenEvent
    {
        public new const int ClassID = NetworkE.CID_SyncTE;

        public TSyncTE(TECustomNode owner) : base(owner)
        { }

        internal int FSyncValue;
        // NetworkE.pas:1738
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSyncValue, 20);
        }

        // NetworkE.pas:1744
        public override TExecuteResult Execute(TTokenExecuter TE)
        {
            TExecuteResult Result = base.Execute(TE);
            if (TE.GetSyncValue() != FSyncValue && TE.FOnSyncError != null)
            {
                TE.FOnSyncError(TE);
            }

            return Result;
        }

        public int SyncValue { get => FSyncValue; set => FSyncValue = value; }

    }

    public static partial class NetworkE
    {
        // The NetworkE unit of Network.dpl (the network message layer of Age of Wonders: the player
        // communication controls that move data packages between the players' systems, and the
        // token system that orders the game's token events and executes them on every system in the
        // same order). A data package travels as a stored TEObject:
        // TPlayerCommunicator.SendDataPackage stores it in a memory stream with
        // TEngine.StoreEObject and hands the stream to the active player communication control
        // (TLocalPCC queues it for itself, TRemotePCC sends it through OnSendData to every other
        // system). TPlayerCommunicator.Process retrieves the packages that arrived and triggers
        // OnReceive. The token manager keeps one token control per player. the token executer
        // activates them in turn, lets the active one prefetch its events (a local player sends
        // them to the others in a token package, a TPassTokenTE ends the turn of the token) and
        // executes them in token counter order
        // { TRemotePCC.SendPackageData / TTokenControl.TokenEventsPrefetched: the package goes to every
//     player }
        public const int AllPlayers = 32768;
        // { Class IDs }
        public const short CID_TokenPackage = 1360;
        public const short CID_TokenEvent = 1361;
        public const short CID_TokenEventList = 1362;
        public const short CID_PassTokenTE = 1364;
        public const short CID_SyncTE = 1365;
        // NetworkE.pas:1751
        public static void RegisterNetworkEngine(TEngine AEngine)
        {
            NetworkEngineDID = AEngine.RegisterDebugID((AnsiString)"Network Engine");
            AEngine.RegisterEClasses([EClass.Of<TTokenPackage>(), EClass.Of<TPassTokenTE>(), EClass.Of<TSyncTE>()]);
        }

        public static int NetworkEngineDID;
        // NetworkE.pas:1051
        public static int CompareTokenCounter(object Item1, object Item2) => ((TTokenEvent)Item1).FTokenCounter - ((TTokenEvent)Item2).FTokenCounter;
    }
}
