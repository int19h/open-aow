// Translated from NetworkE.pas.
using Aow.Engine;

using Aow.Game.Commands;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

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
