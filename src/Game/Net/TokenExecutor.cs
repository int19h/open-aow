#nullable disable warnings
// Translated from NetworkE.pas.
using Aow.Engine;

using Aow.Game.Commands;

using Aow.Rtl;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.Net;

// _activeTacticalBattle, _prefetchTacticalBattle, _activeTacticalBattleIndex, Events, TacticalBattleList, _tokenCounter, _round,
// _executeCount, _state, EventsExecuted, TacticalBattleActivating, OnSyncError, OnGetSyncValue,
// OnExecuteTokenEvent, _executing, _lockCount, SystemEvents, _holdTacticalBattle, TokenManager
public sealed class TokenExecutor : IDisposable
{
    internal TokenEventList Events { get; }
    internal List<TokenControl> TacticalBattleList { get; }

    internal EventStore<TokenExecutor> OnEventsExecutedStore { get; }

    internal event NotifyEvent<TokenExecutor> EventsExecuted
    {
        add => OnEventsExecutedStore.Add(value);
        remove => OnEventsExecutedStore.Remove(value);
    }

    internal EventStore<TokenExecutor> OnActivateTacticalBattleStore { get; }

    internal event NotifyEvent<TokenExecutor> TacticalBattleActivating
    {
        add => OnActivateTacticalBattleStore.Add(value);
        remove => OnActivateTacticalBattleStore.Remove(value);
    }

    private NotifyEvent<object>? _syncError;

    internal void ReportSyncError(object sender) => _syncError?.Invoke(sender);

    internal bool ChecksSync => _syncError is not null;

    internal void SetSyncErrorHandler(NotifyEvent<object>? handler) => _syncError = handler;

    private GetSyncValueEvent? _getSyncValue;

    internal void SetGetSyncValueHandler(GetSyncValueEvent? handler) => _getSyncValue = handler;

    private Action<TokenEvent> _executeTokenEvent;

    internal void SetExecuteTokenEventHandler(Action<TokenEvent> handler) => _executeTokenEvent = handler;

    private int _lockCount;

    internal TokenEventList SystemEvents { get; }

    internal TokenManager? TokenManager { get; }

    // NetworkE.pas:1429
    public ExecuteResult ExecuteTokenEvent(TokenEvent tokenEvent)
    {
        Executing = true;
        _executeTokenEvent?.Invoke(tokenEvent);

        ExecuteResult result;
        try
        {
            result = tokenEvent.Execute(this);
        }
        catch
        {
            ShowMessage($"Exception occured during {tokenEvent.GetType().Name}");
            result = ExecuteResult.Done;
        }

        ExecuteCount += 1;
        Executing = false;
        return result;
    }

    // NetworkE.pas:1350
    public TokenExecutor(TokenManager? manager)
    {
        TokenManager = manager;
        // A construction test passes no manager.
        Events = new TokenEventList(null) { Holder = manager?.Owner };
        TacticalBattleList = [];
        OnEventsExecutedStore = new();
        OnActivateTacticalBattleStore = new();
        SystemEvents = new TokenEventList(null) { Holder = manager?.Owner };
    }

    // NetworkE.pas:1361
    public void Dispose()
    {
        Reset();
        Events?.Dispose();
        TacticalBattleList?.Clear();
        OnEventsExecutedStore.Clear();
        OnActivateTacticalBattleStore.Clear();
        SystemEvents?.Dispose();
    }

    // NetworkE.pas:1372
    public int GetSyncValue()
    {
        int result;
        if (_getSyncValue is not null)
        {
            _getSyncValue(out result);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // NetworkE.pas:1380
    public TokenControl GetTacticalBattle(int index) => TacticalBattleList[index];

    // NetworkE.pas:1385
    public void AddTacticalBattle(TokenControl tacticalBattle) => TacticalBattleList.Add(tacticalBattle);

    // NetworkE.pas:1390
    public void DeleteTacticalBattle(int index) => TacticalBattleList.RemoveAt(index);

    // NetworkE.pas:1395
    public void RemoveTacticalBattle(TokenControl tacticalBattle) => TacticalBattleList.Remove(tacticalBattle);

    // NetworkE.pas:1400
    public void InsertTacticalBattle(int index, TokenControl tacticalBattle) => TacticalBattleList.Insert(index, item: tacticalBattle);

    // NetworkE.pas:1405
    public void Reset()
    {
        TacticalBattleList.Clear();
        ActiveTacticalBattle = null;
        ActiveTacticalBattleIndex = -1;
        State = TokenExecutorStates.None;
        Round = 0;
        TokenCounter = 0;
    }

    // NetworkE.pas:1416
    // { Numbers an event that has no token counter yet (or an old one) and queues it }
    public void PrefetchTokenEvent(TokenEvent tokenEvent)
    {
        if (State.Contains(TokenExecutorStates.Prefetching))
        {
            if (tokenEvent.TokenCounter >= TokenCounter || tokenEvent.TokenCounter == -1)
            {
                tokenEvent.TokenCounter = Events.Count + TokenCounter;
                Events.Add(tokenEvent);
            }

            ExecuteCount = 0;
        }
    }

    // NetworkE.pas:1444
    public TokenEvent? ExecutingTokenEvent => State.Contains(TokenExecutorStates.Executing) && Events.Count != 0 ? Events[0] : null;

    // NetworkE.pas:1452
    public bool InsertExecuteEvent(TokenEvent? tokenEvent)
    {
        if (State.Contains(TokenExecutorStates.Executing))
        {
            Events.Add(tokenEvent);
            return true;
        }
        else
        {
            return false;
        }
    }

    // NetworkE.pas:1463
    public void Initialize(int index)
    {
        State = TokenExecutorStates.None;
        if (index < TacticalBattleList.Count)
        {
            ActiveTacticalBattle = TacticalBattleList[index];
            ActiveTacticalBattleIndex = index;
            Round = 0;
        }
    }

    // NetworkE.pas:1474
    public void ActivateTacticalBattle(int index)
    {
        ActiveTacticalBattle = TacticalBattleList[index];
        ActiveTacticalBattle.Active = true;
        ActiveTacticalBattleIndex = index;
        OnActivateTacticalBattleStore.Raise(this);
        if (State.Contains(TokenExecutorStates.Executing))
        {
            State |= TokenExecutorStates.TacticalBattleActivated;
        }
    }

    // NetworkE.pas:1485
    // { The next active token control after the current one; a wrap starts the next round }
    public void ActivateNextTacticalBattle()
    {
        int n = 0;
        while (true)
        {
            ActiveTacticalBattleIndex += 1;
            n += 1;
            if (TacticalBattleList.Count <= ActiveTacticalBattleIndex)
            {
                ActiveTacticalBattleIndex = 0;
                Round += 1;
            }

            if (TacticalBattleList[ActiveTacticalBattleIndex].Active || n > TacticalBattleList.Count)
            {
                break;
            }
        }

        if (TacticalBattleList[ActiveTacticalBattleIndex].Active)
        {
            ActivateTacticalBattle(ActiveTacticalBattleIndex);
        }
    }

    // NetworkE.pas:1503
    public void Lock() => _lockCount += 1;

    // NetworkE.pas:1508
    public void Unlock() => _lockCount -= 1;

    // NetworkE.pas:1513
    public bool Locked => _lockCount != 0;

    // NetworkE.pas:1519
    // { The system events run between the token controls' events; True while some remain }
    public bool ProcessSystemTokenEvents()
    {
        if (!State.Contains(TokenExecutorStates.Executing) && SystemEvents.Count > 0 && _lockCount == 0)
        {
            if (SystemEvents[0].TokenCounter == -1)
            {
                ExecuteCount = 0;
                SystemEvents[0].TokenCounter = TokenCounter;
            }

            ExecuteResult r = ExecuteTokenEvent(SystemEvents[0]);
            if (r is ExecuteResult.Done or ExecuteResult.DoneHalt)
            {
                SystemEvents.Delete(0);
                TokenCounter += 1;
                ExecuteCount = 0;
            }

            return SystemEvents.Count != 0;
        }
        else
        {
            return false;
        }
    }

    // NetworkE.pas:1543
    public void AddSystemEvent(TokenEvent tokenEvent) => SystemEvents.Add(tokenEvent);

    // NetworkE.pas:1550
    // { Lets the active token control prefetch its events, executes them and activates the next token
    //   control, for at most one round of token controls }
    public void Process()
    {
        if (ActiveTacticalBattle is not null && !ProcessSystemTokenEvents())
        {
            int i = 0;
            bool prefetched = true;
            ExecuteResult r = ExecuteResult.Done;
            while (_lockCount == 0 && prefetched && r == ExecuteResult.Done && i < TacticalBattleList.Count)
            {
                if (!State.Contains(TokenExecutorStates.Executing))
                {
                    State |= TokenExecutorStates.Prefetching;
                    Events.Clear();
                    prefetched = ActiveTacticalBattle.PrefetchTokenEvents(this);
                    State &= ~TokenExecutorStates.Prefetching;
                    if (prefetched)
                    {
                        State |= TokenExecutorStates.Executing;
                        PrefetchTacticalBattle = ActiveTacticalBattle;
                        ActiveTacticalBattle.TokenEventsPrefetched(this, Events);
                    }
                }

                if (State.Contains(TokenExecutorStates.Executing))
                {
                    while (r == ExecuteResult.Done && Events.Count != 0)
                    {
                        r = ExecuteTokenEvent(Events[0]);
                        if (r is ExecuteResult.Done or ExecuteResult.DoneHalt)
                        {
                            Events.Delete(0);
                            TokenCounter += 1;
                            ExecuteCount = 0;
                        }
                    }

                    if (Events.Count == 0)
                    {
                        State &= ~TokenExecutorStates.Executing;
                        OnEventsExecutedStore.Raise(this);
                        if (!HoldTacticalBattle && !State.Contains(TokenExecutorStates.TacticalBattleActivated))
                        {
                            ActivateNextTacticalBattle();
                        }

                        State &= ~TokenExecutorStates.TacticalBattleActivated;
                        PrefetchTacticalBattle = null;
                        i += 1;
                    }
                }
            }
        }
    }

    public TokenControl? ActiveTacticalBattle { get; private set; }
    public TokenControl? PrefetchTacticalBattle { get; private set; }
    public int ActiveTacticalBattleIndex { get; private set; }
    public int TokenCounter { get; private set; }
    public int Round { get; private set; }
    public int ExecuteCount { get; internal set; }
    public TokenExecutorStates State { get; private set; }
    public bool HoldTacticalBattle { get; set; }
    public bool Executing { get; private set; }

    public event NotifyEvent<TokenExecutor> OnEventsExecuted
    {
        add => OnEventsExecutedStore.Add(value);
        remove => OnEventsExecutedStore.Remove(value);
    }

    public event NotifyEvent<TokenExecutor> OnActivateTacticalBattle
    {
        add => OnActivateTacticalBattleStore.Add(value);
        remove => OnActivateTacticalBattleStore.Remove(value);
    }

    public delegate void GetSyncValueEvent(out int Value);
}
