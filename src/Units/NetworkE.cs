// Translated from NetworkE.pas.
using Aow.Formats.Tree;
using Aow.Game.Commands;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Net;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net
{

    // FEngine, FOnReceive, FReceivedPackage, FSendStream, FReceiveStream, FPCC, FOnProcess
    public partial class PlayerCommunicator : EObject
    {
        internal Engine.Engine FEngine;

        internal EventStore<PlayerCommunicator> FOnReceiveStore;

        internal event NotifyEvent<PlayerCommunicator> FOnReceive
        {
            add => FOnReceiveStore.Add(value);
            remove => FOnReceiveStore.Remove(value);
        }

        internal DataPackage FReceivedPackage;
        internal MemoryStream FSendStream;
        internal MemoryStream FReceiveStream;
        internal PlayerCommunicationControl FPCC;

        internal EventStore<PlayerCommunicator> FOnProcessStore;

        internal event NotifyEvent<PlayerCommunicator> FOnProcess
        {
            add => FOnProcessStore.Add(value);
            remove => FOnProcessStore.Remove(value);
        }

        // NetworkE.pas:605
        public PlayerCommunicator(ECustomNode owner) : base(owner)
        {
            FReceivedPackage = null;
            FOnReceiveStore = new();
            FSendStream = new MemoryStream();
            FReceiveStream = new MemoryStream();
            FOnProcessStore = new();
        }

        // NetworkE.pas:629
        public virtual bool ActivatePCC(PlayerCommunicationControl PCC)
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

            FOnProcessStore.Raise(this);
        }

        // NetworkE.pas:615
        public override void Dispose()
        {
            FOnReceiveStore.Clear();
            FSendStream?.Dispose();
            FReceiveStream?.Dispose();
            FOnProcessStore.Clear();
            if (FPCC != null)
            {
                FPCC.Release();
                FPCC = null;
            }

            base.Dispose();
        }

        // NetworkE.pas:658
        public bool ReceiveDataPackage()
        {
            bool Result = default;
            EObject Obj = default;
            FReceiveStream.Clear();
            FReceiveStream.Position = 0;
            if (FPCC.ReceivePackageData(FReceiveStream))
            {
                try
                {
                    Obj = FEngine.RetrieveEObject(FReceiveStream, null);
                    if (Obj is DataPackage)
                    {
                        FReceivedPackage = (DataPackage)Obj;
                        FOnReceiveStore.Raise(this);
                        FReceivedPackage.Release();
                    }
                    else
                    {
                        Obj?.Dispose();
                    }
                }
                catch
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
        public bool SendDataPackage(DataPackage Package, int ASender, int AReceiver)
        {
            Package.FSender = ASender;
            Package.FReceiver = AReceiver;
            FSendStream.Clear();
            FEngine.StoreEObject(FSendStream, Package, null);
            FSendStream.Position = 0;
            return FPCC.SendPackageData(AReceiver, FSendStream);
        }

        public Engine.Engine Engine { get => FEngine; set => FEngine = value; }

        public event NotifyEvent<PlayerCommunicator> OnReceive
        {
            add => FOnReceiveStore.Add(value);
            remove => FOnReceiveStore.Remove(value);
        }

        public DataPackage ReceivedPackage { get => FReceivedPackage; }
        public PlayerCommunicationControl PCC { get => FPCC; }

        public event NotifyEvent<PlayerCommunicator> OnProcess
        {
            add => FOnProcessStore.Add(value);
            remove => FOnProcessStore.Remove(value);
        }

    }
}

namespace Aow.Game.Commands
{

    // FRefCount, FTokenCounter (stored with id 10. -1 until the executer numbers the event). The
    // base of the game's token events
    public partial class TokenEvent : EObject
    {
        // NetworkE.pas:950
        public new const int ClassID = CID_TokenEvent;

        public TokenEvent(ECustomNode owner) : base(owner)
        {
            FRefCount = 1;
            FTokenCounter = -1;
        }

        internal int FRefCount;
        internal int FTokenCounter;

        // NetworkE.pas:969
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FTokenCounter, 10);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            persister.Int("TokenCounter", ref FTokenCounter);
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
                Dispose();
            }

            return Result;
        }

        // NetworkE.pas:975
        public virtual ExecuteResult Execute(TokenExecuter TE) => ExecuteResult.Done;

        public int TokenCounter { get => FTokenCounter; set => FTokenCounter = value; }

        private const short CID_TokenEvent = 1361;

    }
}

namespace Aow.Game.Net
{

    // FList (stored with id 5), a reference node of token events
    public partial class TokenEventList : EObject
    {
        // NetworkE.pas:995
        public new const int ClassID = CID_TokenEventList;

        internal EReferenceNode FList;

        // NetworkE.pas:1000
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FList, 5);
        }

        // NetworkE.pas:982
        public TokenEventList(ECustomNode owner) : base(owner)
        {
            FList = new EReferenceNode(null);
        }

        // NetworkE.pas:1046
        public override void Clear()
        {
            FList.Clear();
        }

        // NetworkE.pas:1006
        public override void Assign(EObject Source, Engine.Engine AEngine)
        {
            FList.Clear();
            AddEvents((TokenEventList)Source);
        }

        // NetworkE.pas:988
        public override void Dispose()
        {
            Clear();
            FList?.Dispose();
            base.Dispose();
        }

        // NetworkE.pas:1012
        public int IndexOfTokenCounter(int ATokenCounter)
        {
            int Result = FList.Count;
            while (Result != 0)
            {
                Result -= 1;
                if (((TokenEvent)FList.FChildren.List[Result]).FTokenCounter == ATokenCounter)
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
            while (Result != C && ((EObject)FList.FChildren.List[Result]).EClass.ClassID != AClassID)
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
        public TokenEvent GetEvent(int Index) => (TokenEvent)FList.FChildren.List[Index];

        // NetworkE.pas:1041
        public int GetCount() => FList.Count;

        // NetworkE.pas:1056
        public void SortOnTokenCounter()
        {
            FList.FChildren.Sort(Aow.Game.Net.TokenEventList.CompareTokenCounter);
        }

        // NetworkE.pas:1061
        public void AddEvents(TokenEventList Events)
        {
            for (int I = 0, last = Events.Count - 1; I <= last; I++)
            {
                var tokenEvent = Events[I];
                FList.AddChild(tokenEvent);
            }
        }

        // NetworkE.pas:1069
        public void Add(TokenEvent TE)
        {
            FList.AddChild(TE);
        }

        // NetworkE.pas:1074
        public void Delete(int Index)
        {
            var child = FList.Children[Index];
            FList.RemoveChild(child);
        }

        // NetworkE.pas:1080
        // { Inserts without TEReferenceNode.AddChild, so the reference is taken here }
        public void Insert(int Index, TokenEvent TE)
        {
            TE.AddRef();
            FList.FChildren.Insert(Index, TE);
        }

        public int Count { get => GetCount(); }

        public TokenEvent this[int Index] { get => GetEvent(Index); }

        private const short CID_TokenEventList = 1362;

        private static int CompareTokenCounter(object Item1, object Item2)
        {
            return ((TokenEvent)Item1).FTokenCounter - ((TokenEvent)Item2).FTokenCounter;
        }

    }

    [LegacyObject]
    // FEvents, FTokenManager, FPlayer, FActive. The token events of one player
    public partial class TokenControl : IDisposable
    {
        internal TokenEventList FEvents;
        internal TokenManager FTokenManager;
        internal int FPlayer;
        internal bool FActive;

        // NetworkE.pas:1189
        // { Keeps the events sorted on their token counters; an event without one goes to the end }
        public virtual void Add(TokenEvent TE)
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
                        throw new InvalidOperationException("Double TokenCounter defined");
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
        public virtual bool PrefetchTokenEvents(TokenExecuter Executer)
        {
            bool Result;
            int N;
            TokenEvent TE;
            if (Global() || Local())
            {
                if (FEvents.Count != 0 || Executer.FHoldTC != true)
                {
                    if (Local())
                    {
                        if (Executer.FOnSyncError != null)
                        {
                            TE = new SyncTE(null);
                            var syncValue = Executer.GetSyncValue();
                            ((SyncTE)TE).FSyncValue = syncValue;
                            FEvents.Insert(0, TE);
                            TE.Release();
                        }

                        TE = new PassTokenTE(null);
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
        public virtual void TokenEventsPrefetched(TokenExecuter Executer, TokenEventList Events)
        {
            if (Local() && !Global() && Events.Count != 0)
            {
                FTokenManager.FTokenPackageDistributer.SendTokenEvents(FPlayer, NetworkSession.AllPlayers, Events);
            }
        }

        // NetworkE.pas:1169
        public TokenControl(TokenManager AManager, int APlayer)
        {
            CaptureAddress.Assign(this);
            FEvents = new TokenEventList(null);
            FTokenManager = AManager;
            FPlayer = APlayer;
            FActive = true;
            if (FTokenManager != null && FTokenManager.FTokenExecuter != null)
            {
                FTokenManager.FTokenExecuter.AddTC(this);
            }
        }

        // NetworkE.pas:1211
        public virtual void AddEvent(TokenEvent TE)
        {
            if (Remote())
            {
                throw new InvalidOperationException("Cannot Add Remote events");
            }

            Add(TE);
        }

        // NetworkE.pas:1218
        public virtual void AddEventEx(TokenEvent TE)
        {
            Add(TE);
        }

        // NetworkE.pas:1238
        public virtual void AddEvents(TokenEventList Events)
        {
            for (int I = 0, last = Events.Count - 1; I <= last; I++)
            {
                AddEvent(Events[I]);
            }
        }

        // NetworkE.pas:1180
        public virtual void Dispose()
        {
            if (FTokenManager != null && FTokenManager.FTokenExecuter != null)
            {
                FTokenManager.FTokenExecuter.RemoveTC(this);
            }

            FEvents?.Dispose();
        }

        // NetworkE.pas:1223
        public void ClearEvents()
        {
            FEvents.Clear();
        }

        // NetworkE.pas:1228
        public TokenEvent GetEvent(int Index) => FEvents[Index];

        // NetworkE.pas:1233
        public int GetCount() => FEvents.Count;

        // NetworkE.pas:1246
        public bool Local()
        {
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            return playerCommunicationControl.GetPlayers(FPlayer).FConnectionMode == ConnectionMode.Local;
        }

        // NetworkE.pas:1252
        public bool Global()
        {
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            return playerCommunicationControl.GetPlayers(FPlayer).FConnectionMode == ConnectionMode.Global;
        }

        // NetworkE.pas:1258
        public bool Remote()
        {
            var playerCommunicationControl = FTokenManager.FTokenPackageDistributer.FPC.FPCC;
            return playerCommunicationControl.GetPlayers(FPlayer).FConnectionMode == ConnectionMode.Remote;
        }

        // NetworkE.pas:1266
        // { The number of events of a remote player that can be executed now: they must follow the
        //   executer's token counter without a gap, up to and including the first TPassTokenTE }
        public int GetPrefetchableTokenEvents(TokenExecuter Executer)
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
                    PassToken = FEvents[I].EClass.ClassID == NetworkSession.CID_PassTokenTE;
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

        public TokenEventList Events { get => FEvents; }
        public int Player { get => FPlayer; }
        public bool Active { get => FActive; set => FActive = value; }
        public int Count { get => GetCount(); }

    }

    [LegacyObject]
    // FActiveTC, FPrefetchTC, FActiveTCIndex, FEvents, FTCList, FTokenCounter, FRound,
    // FExecuteCount, FState, FOnEventsExecuted, FOnActivateTC, FOnSyncError, FOnGetSyncValue,
    // FOnExecuteTokenEvent, FExecuting, FLockCount, FSystemEvents, FHoldTC, FTokenManager
    public partial class TokenExecuter : IDisposable
    {
        internal TokenControl FActiveTC;
        internal TokenControl FPrefetchTC;
        internal int FActiveTCIndex;
        internal TokenEventList FEvents;
        internal DelphiList FTCList;
        internal int FTokenCounter;
        internal int FRound;
        internal int FExecuteCount;
        internal Set<TokenExecuterState> FState;

        internal EventStore<TokenExecuter> FOnEventsExecutedStore;

        internal event NotifyEvent<TokenExecuter> FOnEventsExecuted
        {
            add => FOnEventsExecutedStore.Add(value);
            remove => FOnEventsExecutedStore.Remove(value);
        }

        internal EventStore<TokenExecuter> FOnActivateTCStore;

        internal event NotifyEvent<TokenExecuter> FOnActivateTC
        {
            add => FOnActivateTCStore.Add(value);
            remove => FOnActivateTCStore.Remove(value);
        }

        internal NotifyEvent<object> FOnSyncError;
        internal GetSyncValueEvent FOnGetSyncValue;
        internal TokenEventEvent FOnExecuteTokenEvent;
        internal bool FExecuting;
        internal int FLockCount;
        internal TokenEventList FSystemEvents;
        internal bool FHoldTC;
        internal TokenManager FTokenManager;

        // NetworkE.pas:1429
        public virtual ExecuteResult ExecuteTokenEvent(TokenEvent TE)
        {
            ExecuteResult Result = default;
            FExecuting = true;
            if (FOnExecuteTokenEvent != null)
            {
                FOnExecuteTokenEvent(TE);
            }

            try
            {
                Result = TE.Execute(this);
            }
            catch
            {
                ShowMessage("Exception occured during " + TE.GetType().Name);
                Result = ExecuteResult.Done;
            }

            FExecuteCount += 1;
            FExecuting = false;
            return Result;
        }

        // NetworkE.pas:1350
        public TokenExecuter(TokenManager AManager)
        {
            CaptureAddress.Assign(this);
            FTokenManager = AManager;
            FEvents = new TokenEventList(null);
            FTCList = new DelphiList();
            FOnEventsExecutedStore = new();
            FOnActivateTCStore = new();
            FSystemEvents = new TokenEventList(null);
        }

        // NetworkE.pas:1361
        public virtual void Dispose()
        {
            Reset();
            FEvents?.Dispose();
            FTCList?.Dispose();
            FOnEventsExecutedStore.Clear();
            FOnActivateTCStore.Clear();
            FSystemEvents?.Dispose();
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
        public TokenControl GetTC(int Index) => (TokenControl)FTCList.List[Index];

        // NetworkE.pas:1385
        public void AddTC(TokenControl TC)
        {
            FTCList.Add(TC);
        }

        // NetworkE.pas:1390
        public void DeleteTC(int Index)
        {
            FTCList.Delete(Index);
        }

        // NetworkE.pas:1395
        public void RemoveTC(TokenControl TC)
        {
            FTCList.Remove(TC);
        }

        // NetworkE.pas:1400
        public void InsertTC(int Index, TokenControl TC)
        {
            FTCList.Insert(Index, TC);
        }

        // NetworkE.pas:1405
        public void Reset()
        {
            FTCList.Clear();
            FActiveTC = null;
            FActiveTCIndex = -1;
            FState = Set<TokenExecuterState>.Of();
            FRound = 0;
            FTokenCounter = 0;
        }

        // NetworkE.pas:1416
        // { Numbers an event that has no token counter yet (or an old one) and queues it }
        public void PrefetchTokenEvent(TokenEvent TE)
        {
            if (FState.Contains(TokenExecuterState.Prefetching))
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
        public TokenEvent GetExecutingTokenEvent()
        {
            TokenEvent Result;
            if (FState.Contains(TokenExecuterState.Executing) && FEvents.Count != 0)
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
        public bool InsertExecuteEvent(TokenEvent TE)
        {
            bool Result;
            if (FState.Contains(TokenExecuterState.Executing))
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
            FState = Set<TokenExecuterState>.Of();
            if (AIndex < FTCList.Count)
            {
                FActiveTC = (TokenControl)FTCList[AIndex];
                FActiveTCIndex = AIndex;
                FRound = 0;
            }
        }

        // NetworkE.pas:1474
        public void ActivateTC(int Index)
        {
            FActiveTC = (TokenControl)FTCList[Index];
            FActiveTC.FActive = true;
            FActiveTCIndex = Index;
            FOnActivateTCStore.Raise(this);
            if (FState.Contains(TokenExecuterState.Executing))
            {
                FState = FState + Set<TokenExecuterState>.Of(TokenExecuterState.TCActivated);
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

                if (((TokenControl)FTCList[FActiveTCIndex]).FActive || N > FTCList.Count)
                    break;
            }

            if (((TokenControl)FTCList[FActiveTCIndex]).FActive)
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
            ExecuteResult R;
            if (!FState.Contains(TokenExecuterState.Executing) && FSystemEvents.Count > 0 && FLockCount == 0)
            {
                if (FSystemEvents[0].FTokenCounter == -1)
                {
                    FExecuteCount = 0;
                    FSystemEvents[0].FTokenCounter = FTokenCounter;
                }

                R = ExecuteTokenEvent(FSystemEvents[0]);
                if (R == ExecuteResult.Done || R == ExecuteResult.DoneHalt)
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
        public void AddSystemEvent(TokenEvent TE)
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
            ExecuteResult R;
            if (FActiveTC != null && !ProcessSystemTokenEvents())
            {
                I = 0;
                Prefetched = true;
                R = ExecuteResult.Done;
                while (FLockCount == 0 && Prefetched == true && R == ExecuteResult.Done && I < FTCList.Count)
                {
                    if (!FState.Contains(TokenExecuterState.Executing))
                    {
                        FState = FState + Set<TokenExecuterState>.Of(TokenExecuterState.Prefetching);
                        FEvents.Clear();
                        Prefetched = FActiveTC.PrefetchTokenEvents(this);
                        FState = FState - Set<TokenExecuterState>.Of(TokenExecuterState.Prefetching);
                        if (Prefetched == true)
                        {
                            FState = FState + Set<TokenExecuterState>.Of(TokenExecuterState.Executing);
                            FPrefetchTC = FActiveTC;
                            FActiveTC.TokenEventsPrefetched(this, FEvents);
                        }
                    }

                    if (FState.Contains(TokenExecuterState.Executing))
                    {
                        while (R == ExecuteResult.Done && FEvents.Count != 0)
                        {
                            R = ExecuteTokenEvent(FEvents[0]);
                            if (R == ExecuteResult.Done || R == ExecuteResult.DoneHalt)
                            {
                                FEvents.Delete(0);
                                FTokenCounter += 1;
                                FExecuteCount = 0;
                            }
                        }

                        if (FEvents.Count == 0)
                        {
                            FState = FState - Set<TokenExecuterState>.Of(TokenExecuterState.Executing);
                            FOnEventsExecutedStore.Raise(this);
                            if (!FHoldTC && !FState.Contains(TokenExecuterState.TCActivated))
                            {
                                ActivateNextTC();
                            }

                            FState = FState - Set<TokenExecuterState>.Of(TokenExecuterState.TCActivated);
                            FPrefetchTC = null;
                            I += 1;
                        }
                    }
                }
            }
        }

        public TokenControl ActiveTC { get => FActiveTC; }
        public TokenControl PrefetchTC { get => FPrefetchTC; }
        public int ActiveTCIndex { get => FActiveTCIndex; }
        public int TokenCounter { get => FTokenCounter; }
        public int Round { get => FRound; }
        public int ExecuteCount { get => FExecuteCount; }
        public Set<TokenExecuterState> State { get => FState; }
        public bool Locked { get => GetLocked(); }
        public bool HoldTC { get => FHoldTC; set => FHoldTC = value; }
        public bool Executing { get => FExecuting; }

        public event NotifyEvent<TokenExecuter> OnEventsExecuted
        {
            add => FOnEventsExecutedStore.Add(value);
            remove => FOnEventsExecutedStore.Remove(value);
        }

        public event NotifyEvent<TokenExecuter> OnActivateTC
        {
            add => FOnActivateTCStore.Add(value);
            remove => FOnActivateTCStore.Remove(value);
        }

        public NotifyEvent<object> OnSyncError { get => FOnSyncError; set => FOnSyncError = value; }
        public GetSyncValueEvent OnGetSyncValue { get => FOnGetSyncValue; set => FOnGetSyncValue = value; }
        public TokenEventEvent OnExecuteTokenEvent { get => FOnExecuteTokenEvent; set => FOnExecuteTokenEvent = value; }

        public delegate void GetSyncValueEvent(ref int Value);
        public delegate void TokenEventEvent(TokenEvent TE);

    }

    [LegacyObject]
    // FTokenExecuter, FTokenPackageDistributer, FOwner, FTokenControls (one per player, nil for a
    // free slot), FOnMessage
    public partial class TokenManager : IDisposable
    {

        public TokenManager(object owner)
        {
            CaptureAddress.Assign(this);
            FOwner = owner;
            FTokenControls = new DelphiList();
            FTokenExecuter = CreateTokenExecuter();
            FTokenPackageDistributer = CreateTokenPackageDistributer();
        }

        internal TokenExecuter FTokenExecuter;
        internal TokenPackageDistributer FTokenPackageDistributer;
        internal object FOwner;
        internal DelphiList FTokenControls;
        internal NetworkMessageEvent FOnMessage;

        // NetworkE.pas:1642
        public virtual TokenExecuter CreateTokenExecuter() => new TokenExecuter(this);

        // NetworkE.pas:1647
        public virtual TokenPackageDistributer CreateTokenPackageDistributer() => new TokenPackageDistributer(this);

        // NetworkE.pas:1652
        public virtual TokenControl CreateTokenControl(int APlayer) => new TokenControl(this, APlayer);

        // NetworkE.pas:1628
        public virtual void Initialize(PlayerCommunicator PC)
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
                bool condition = FTokenExecuter.FSystemEvents.Count != 0 || ((TokenControl)FTokenControls[APlayer]).GetCount() != 0;
                if (!condition)
                {
                    var item = FTokenControls[APlayer];
                    condition = FTokenExecuter.FPrefetchTC == item && FTokenExecuter.FState.Contains(TokenExecuterState.Executing);
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
                (FTokenControls[APlayer] as IDisposable)?.Dispose();
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
                    (FTokenControls[FTokenControls.Count - 1] as IDisposable)?.Dispose();
                }

                FTokenControls.Delete(FTokenControls.Count - 1);
            }
        }

        // NetworkE.pas:1606
        // { The original does not call the inherited constructor. }

        // NetworkE.pas:1614
        public virtual void Dispose()
        {
            ClearPlayers();
            FTokenControls?.Dispose();
            FTokenExecuter?.Dispose();
        }

        // NetworkE.pas:1623
        public int GetCount() => FTokenControls.Count;

        // NetworkE.pas:1710
        public TokenControl GetTokenControl(int APlayer)
        {
            TokenControl Result;
            if (APlayer < FTokenControls.Count)
            {
                Result = (TokenControl)FTokenControls[APlayer];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // NetworkE.pas:1718
        public void TriggerMessage(string Msg)
        {
            if (FOnMessage != null)
            {
                FOnMessage(this, Msg);
            }
        }

        public TokenExecuter TokenExecuter { get => FTokenExecuter; }
        public TokenPackageDistributer TokenPackageDistributer { get => FTokenPackageDistributer; }
        public object Owner { get => FOwner; }
        public int Count { get => GetCount(); }

        public sealed class TokenControlsAccessor(TokenManager target)
        {
            public TokenControl this[int APlayer] { get => target.GetTokenControl(APlayer); }
        }

        public TokenControlsAccessor TokenControls => new TokenControlsAccessor(this);
        public NetworkMessageEvent OnMessage { get => FOnMessage; set => FOnMessage = value; }

        public delegate void NetworkMessageEvent(object Sender, string Msg);

    }

    public enum ConnectionMode : byte
    {
        Local = 0,
        Remote = 1,
        Global = 2
    }

    public enum ExecuteResult : byte
    {
        Retry = 0,
        Done = 1,
        DoneHalt = 2
    }

    public enum TokenExecuterState : byte
    {
        Prefetching = 0,
        Executing = 1,
        TCActivated = 2
    }

    public delegate bool SendDataEvent(object Data, int Size, int SystemID);

    // { The original reaches TENode's child list (Engine.TENode.FChildren, +8) from this unit; the
    //     recovered Engine.pas keeps the field private, so this layout view of TENode reads it (a
    //     typecast of a field or parameter compiles to no code) }
    public partial class ENodeView : ECustomNode
    {
        public ENodeView(ECustomNode owner) : base(owner)
        { }

        public EObjectList FChildren;
    }

    // FSender, FReceiver (player indices, stored with ids 1 and 2), FRefCount. The base of the
    // packages that TPlayerCommunicator sends
    public partial class DataPackage : EObject
    {
        internal int FSender;
        internal int FReceiver;
        internal int FRefCount;

        // NetworkE.pas:592
        // { The original does not call the inherited ReadWrite. }
        public override void ReadWrite(EStorageStream Stream)
        {
            Stream.rwInteger(ref FSender, 1);
            Stream.rwInteger(ref FReceiver, 2);
        }

        // NetworkE.pas:598
        public override void MainReadWrite(EStorageStream Stream)
        {
            ReadWrite(Stream);
        }

        // NetworkE.pas:566
        public DataPackage(ECustomNode owner) : base(owner)
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
                Dispose();
            }

            return Result;
        }

        // NetworkE.pas:586
        public virtual string Name() => this.GetType().Name;

        public int Sender { get => FSender; set => FSender = value; }
        public int Receiver { get => FReceiver; set => FReceiver = value; }

    }

    // FConnectionMode (stored with id 5), FSystemID (id 6). One per player
    public partial class PlayerCommunicationSettings : EObject
    {
        internal ConnectionMode FConnectionMode;
        internal int FSystemID;

        // NetworkE.pas:452
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FConnectionMode, 1), 1, 5);
            Stream.rwInteger(ref FSystemID, 6);
        }

        // NetworkE.pas:446
        public PlayerCommunicationSettings(ECustomNode owner) : base(owner)
        {
            FConnectionMode = Net.ConnectionMode.Local;
        }

        public ConnectionMode ConnectionMode { get => FConnectionMode; set => FConnectionMode = value; }
        public int SystemID { get => FSystemID; set => FSystemID = value; }

    }

    // the node that owns the players' settings
    public partial class PlayerCommunicationSettingsList : ENode
    {
        public PlayerCommunicationSettingsList(ECustomNode owner) : base(owner)
        { }

        // NetworkE.pas:461
        public PlayerCommunicationSettings GetPlayers(int Index) => (PlayerCommunicationSettings)this.FChildren.List[Index];

        public sealed class PlayersAccessor(PlayerCommunicationSettingsList target)
        {
            public PlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
        }

        public PlayersAccessor Players => new PlayersAccessor(this);

    }

    [LegacyObject]
    // FRefCount, FPlayerCommunicator, FPCSList. The base player communication control: one system,
    // no transport
    public partial class PlayerCommunicationControl : IDisposable
    {
        internal int FRefCount;
        internal PlayerCommunicator FPlayerCommunicator;
        internal PlayerCommunicationSettingsList FPCSList;

        // NetworkE.pas:549
        public virtual int GetLocalSystemID() => 1;

        // NetworkE.pas:532
        public virtual int GetSystemCount() => 1;

        // NetworkE.pas:516
        public virtual bool Activate(PlayerCommunicator PC)
        {
            FPlayerCommunicator = PC;
            return true;
        }

        // NetworkE.pas:498
        public virtual PlayerCommunicationSettings CreatePCS() => new PlayerCommunicationSettings(null);

        // NetworkE.pas:493
        public virtual string Name() => "";

        // NetworkE.pas:554
        public virtual bool ReceivePackageData(MemoryStream Stream) => false;

        // NetworkE.pas:559
        public virtual bool SendPackageData(int APlayer, MemoryStream Stream) => false;

        // NetworkE.pas:468
        public PlayerCommunicationControl()
        {
            CaptureAddress.Assign(this);
            FRefCount = 1;
            FPCSList = new PlayerCommunicationSettingsList(null);
        }

        // NetworkE.pas:475
        public virtual void Dispose()
        {
            FPCSList?.Dispose();
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
                Dispose();
            }
        }

        // NetworkE.pas:503
        public PlayerCommunicationSettings GetPlayers(int Index)
        {
            PlayerCommunicationSettings Result;
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

        public sealed class PlayersAccessor(PlayerCommunicationControl target)
        {
            public PlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
        }

        public PlayersAccessor Players => new PlayersAccessor(this);
        public PlayerCommunicator PlayerCommunicator { get => FPlayerCommunicator; }
        public PlayerCommunicationSettingsList PCSList { get => FPCSList; }

    }

    [LegacyObject]
    // FList, the memory streams of the packages that wait to be received
    public partial class PackageDataList : IDisposable
    {
        internal DelphiList FList;

        // NetworkE.pas:870
        public PackageDataList()
        {
            CaptureAddress.Assign(this);
            FList = new DelphiList();
        }

        // NetworkE.pas:876
        public virtual void Dispose()
        {
            Clear();
            FList?.Dispose();
        }

        // NetworkE.pas:883
        public int GetCount() => FList.Count;

        // NetworkE.pas:888
        public MemoryStream GetPackageData(int Index) => (MemoryStream)FList.List[Index];

        // NetworkE.pas:893
        public void Clear()
        {
            DelphiList L;
            L = FList;
            while (L.Count != 0)
            {
                (L.List[L.Count - 1] as IDisposable)?.Dispose();
                L.Delete(L.Count - 1);
            }
        }

        // NetworkE.pas:905
        public void Add(MemoryStream Stream)
        {
            MemoryStream S;
            S = new MemoryStream();
            S.SetLength(Stream.Length);
            var size = (int)S.Length;
            Move(Stream.GetBuffer().AsSpan(0, size), S.GetBuffer().AsSpan(0, size), size);
            FList.Add(S);
        }

        // NetworkE.pas:915
        public void AddData(object Data, int Size)
        {
            MemoryStream S;
            S = new MemoryStream();
            S.SetLength(Size);
            Move(BufferBytes(Data)[..Size], S.GetBuffer().AsSpan(0, Size), Size);
            FList.Add(S);
        }

        // NetworkE.pas:925
        public void Insert(int Index, MemoryStream Stream)
        {
            MemoryStream S;
            S = new MemoryStream();
            S.SetLength(Stream.Length);
            var size = (int)S.Length;
            Move(Stream.GetBuffer().AsSpan(0, size), S.GetBuffer().AsSpan(0, size), size);
            FList.Insert(Index, S);
        }

        // NetworkE.pas:935
        public void Delete(int Index)
        {
            (FList.List[Index] as IDisposable)?.Dispose();
            FList.Delete(Index);
        }

        public int Count { get => GetCount(); }

        public sealed class PackageDataAccessor(PackageDataList target)
        {
            public MemoryStream this[int Index] { get => target.GetPackageData(Index); }
        }

        public PackageDataAccessor PackageData => new PackageDataAccessor(this);

    }

    // FLocalSystemID, FPackageDataList, FOnSendData. The control of a network game: the transport
    // sends through OnSendData and hands what arrives to DataReceived
    public partial class RemotePCC : PlayerCommunicationControl
    {
        internal int FLocalSystemID;
        internal PackageDataList FPackageDataList;
        internal SendDataEvent FOnSendData;

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
        public override string Name() => "Remote";

        // NetworkE.pas:824
        public override bool ReceivePackageData(MemoryStream Stream)
        {
            bool Result;
            if (FPackageDataList.Count != 0)
            {
                Stream.SetLength(FPackageDataList.PackageData[0].Length);
                var size = (int)Stream.Length;
                Move(FPackageDataList.PackageData[0].GetBuffer().AsSpan(0, size), Stream.GetBuffer().AsSpan(0, size), size);
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
        public override bool SendPackageData(int APlayer, MemoryStream Stream)
        {
            bool Result;
            int SystemID;
            if (APlayer == NetworkSession.AllPlayers)
            {
                SystemID = 0;
            }
            else
            {
                SystemID = FPCSList.Players[APlayer].FSystemID;
            }

            if (SystemID != FLocalSystemID)
            {
                var size = (int)Stream.Length;
                Result = SendData(Stream.GetBuffer(), size, SystemID);
            }
            else
            {
                Result = true;
            }

            if (APlayer == NetworkSession.AllPlayers || SystemID == FLocalSystemID)
            {
                FPackageDataList.Add(Stream);
            }

            return Result;
        }

        // NetworkE.pas:759
        public RemotePCC() : base()
        {
            FPackageDataList = new PackageDataList();
        }

        // NetworkE.pas:765
        public override void Dispose()
        {
            FPackageDataList?.Dispose();
            base.Dispose();
        }

        // NetworkE.pas:771
        public void Clear()
        {
            FPackageDataList.Clear();
        }

        // NetworkE.pas:803
        public void SetConnectionMode(int Index, ConnectionMode Mode)
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

        public SendDataEvent OnSendData { get => FOnSendData; set => FOnSendData = value; }

    }

    // FPackageDataList. The control of a game on one system: what is sent is received again
    public partial class LocalPCC : PlayerCommunicationControl
    {
        internal PackageDataList FPackageDataList;

        // NetworkE.pas:709
        public override int GetSystemCount() => 1;

        // NetworkE.pas:714
        public override string Name() => "Local";

        // NetworkE.pas:724
        public override bool ReceivePackageData(MemoryStream Stream)
        {
            bool Result;
            if (FPackageDataList.Count != 0)
            {
                Stream.SetLength(FPackageDataList.PackageData[0].Length);
                var size = (int)Stream.Length;
                Move(FPackageDataList.PackageData[0].GetBuffer().AsSpan(0, size), Stream.GetBuffer().AsSpan(0, size), size);
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
        public override bool SendPackageData(int APlayer, MemoryStream Stream)
        {
            FPackageDataList.Add(Stream);
            return true;
        }

        // NetworkE.pas:697
        public LocalPCC() : base()
        {
            FPackageDataList = new PackageDataList();
        }

        // NetworkE.pas:703
        public override void Dispose()
        {
            FPackageDataList?.Dispose();
            base.Dispose();
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
        public void SetConnectionMode(int Index, ConnectionMode Mode)
        {
            FPCSList.Players[Index].FConnectionMode = Mode;
        }

    }

    // FTokenEvents (stored with id 5)
    public partial class TokenPackage : DataPackage
    {
        // NetworkE.pas:1100
        public new const int ClassID = CID_TokenPackage;

        internal TokenEventList FTokenEvents;

        // NetworkE.pas:1105
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwEObject(FTokenEvents, 5);
        }

        // NetworkE.pas:1088
        public TokenPackage(ECustomNode owner) : base(owner)
        {
            FTokenEvents = new TokenEventList(null);
        }

        // NetworkE.pas:1094
        public override void Dispose()
        {
            FTokenEvents?.Dispose();
            base.Dispose();
        }

        public TokenEventList TokenEvents { get => FTokenEvents; }

        private const short CID_TokenPackage = 1360;

    }

    [LegacyObject]
    // FTokenManager, FPC. Sends the token events of the local players in token packages and files
    // the received ones with their players' token controls
    public partial class TokenPackageDistributer
    {
        internal TokenManager FTokenManager;
        internal PlayerCommunicator FPC;

        // NetworkE.pas:1136
        // { The token events of a package from another system go to the token control of the sending player,
        //   unless they were already executed or are already there. }
        public virtual void TokenPackageReceived(TokenPackage TP)
        {
            if (FTokenManager != null)
            {
                if (FPC.FPCC.GetLocalSystemID() != FPC.FPCC.GetPlayers(TP.FSender).FSystemID)
                {
                    for (int I = 0, last = TP.FTokenEvents.Count - 1; I <= last; I++)
                    {
                        if (TP.FTokenEvents[I].FTokenCounter >= FTokenManager.FTokenExecuter.FTokenCounter)
                        {
                            var tokenEvent = TP.FTokenEvents[I];
                            if (FTokenManager.GetTokenControl(TP.FSender).FEvents.IndexOfTokenCounter(tokenEvent.FTokenCounter) == -1)
                            {
                                var fTokenEventsItem = TP.FTokenEvents[I];
                                FTokenManager.GetTokenControl(TP.FSender).Add(fTokenEventsItem);
                                FTokenManager.GetTokenControl(TP.FSender).FEvents.SortOnTokenCounter();
                            }
                            else
                            {
                                FTokenManager.TriggerMessage("TokenEvent already exists");
                            }
                        }
                        else
                        {
                            FTokenManager.TriggerMessage("TokenEvent already executed");
                        }
                    }
                }
            }
        }

        // NetworkE.pas:1113
        public TokenPackageDistributer(TokenManager AManager)
        {
            CaptureAddress.Assign(this);
            FTokenManager = AManager;
        }

        // NetworkE.pas:1157
        public virtual void SendTokenEvents(int APlayer, int AReceiver, TokenEventList Events)
        {
            TokenPackage TP;
            TP = new TokenPackage(null);
            TP.FTokenEvents.Assign(Events, null);
            FPC.SendDataPackage(TP, APlayer, AReceiver);
            TP.Release();
        }

        // NetworkE.pas:1119
        public void SetPC(PlayerCommunicator PC)
        {
            if (FPC != null)
            {
                FPC.FOnReceive -= DataPackageReceived;
            }

            FPC = PC;
            if (FPC != null)
            {
                FPC.FOnReceive += DataPackageReceived;
            }
        }

        // NetworkE.pas:1128
        public void DataPackageReceived(PlayerCommunicator Sender)
        {
            if (Sender.FReceivedPackage is TokenPackage package)
            {
                TokenPackageReceived(package);
            }
        }

        public PlayerCommunicator PC { get => FPC; set => SetPC(value); }

    }
}

namespace Aow.Game.Commands
{

    // ends the turn of the token control that prefetched it
    public partial class PassTokenTE : TokenEvent
    {
        public new const int ClassID = NetworkSession.CID_PassTokenTE;

        public PassTokenTE(ECustomNode owner) : base(owner)
        { }

    }

    // FSyncValue (stored with id), the sync value of the system that sent it. another value on
    // execution triggers OnSyncError
    public partial class SyncTE : TokenEvent
    {
        public new const int ClassID = CID_SyncTE;

        public SyncTE(ECustomNode owner) : base(owner)
        { }

        internal int FSyncValue;

        // NetworkE.pas:1738
        public override void ReadWrite(EStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FSyncValue, 20);
        }

        // NetworkE.pas:1744
        public override ExecuteResult Execute(TokenExecuter TE)
        {
            ExecuteResult Result = base.Execute(TE);
            if (TE.GetSyncValue() != FSyncValue && TE.FOnSyncError != null)
            {
                TE.FOnSyncError(TE);
            }

            return Result;
        }

        public int SyncValue { get => FSyncValue; set => FSyncValue = value; }

        private const short CID_SyncTE = 1365;

    }
}
