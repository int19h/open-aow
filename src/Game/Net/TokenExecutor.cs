// Translated from NetworkE.pas.
using Aow.Engine;

using Aow.Game.Commands;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// _activeTacticalBattle, _prefetchTacticalBattle, _activeTacticalBattleIndex, Events, TacticalBattleList, _tokenCounter, _round,
// _executeCount, _state, EventsExecuted, TacticalBattleActivating, OnSyncError, OnGetSyncValue,
// OnExecuteTokenEvent, _executing, _lockCount, SystemEvents, _holdTacticalBattle, TokenManager
public partial class TokenExecutor : IDisposable
{
    private TokenControl _activeTacticalBattle;
    private TokenControl _prefetchTacticalBattle;
    private int _activeTacticalBattleIndex;

    internal TokenEventList Events { get; }
    internal List<TokenControl> TacticalBattleList { get; }

    private int _tokenCounter;
    private int _round;
    private int _executeCount;
    private Set<TokenExecutorState> _state;

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

    public event NotifyEvent<object> OnSyncError;

    internal void RaiseSyncError(object sender)
    {
        if (OnSyncError != null)
        {
            OnSyncError(sender);
        }
    }

    internal bool HasSyncErrorHandlers => OnSyncError != null;

    internal void SetSyncErrorHandler(NotifyEvent<object> handler) => OnSyncError = handler;

    internal void ClearSyncErrorHandler() => OnSyncError = null;

    public event GetSyncValueEvent OnGetSyncValue;

    internal void SetGetSyncValueHandler(GetSyncValueEvent handler) => OnGetSyncValue = handler;

    internal void ClearGetSyncValueHandler() => OnGetSyncValue = null;

    public event TokenEventEvent OnExecuteTokenEvent;

    internal void SetExecuteTokenEventHandler(TokenEventEvent handler) => OnExecuteTokenEvent = handler;

    private bool _executing;
    private int _lockCount;

    internal TokenEventList SystemEvents { get; }

    private bool _holdTacticalBattle;

    internal TokenManager TokenManager { get; }

    // NetworkE.pas:1429
    public virtual ExecuteResult ExecuteTokenEvent(TokenEvent tokenEvent)
    {
        ExecuteResult result = default;
        _executing = true;
        if (OnExecuteTokenEvent != null)
        {
            OnExecuteTokenEvent(tokenEvent);
        }

        try
        {
            result = tokenEvent.Execute(this);
        }
        catch
        {
            ShowMessage("Exception occured during " + tokenEvent.GetType().Name);
            result = ExecuteResult.Done;
        }

        _executeCount += 1;
        _executing = false;
        return result;
    }

    // NetworkE.pas:1350
    public TokenExecutor(TokenManager manager)
    {
        TokenManager = manager;
        Events = new TokenEventList(null);
        TacticalBattleList = new List<TokenControl>();
        OnEventsExecutedStore = new();
        OnActivateTacticalBattleStore = new();
        SystemEvents = new TokenEventList(null);
    }

    // NetworkE.pas:1361
    public virtual void Dispose()
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
        int result = default;
        if (OnGetSyncValue != null)
        {
            OnGetSyncValue(out result);
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
    public void AddTacticalBattle(TokenControl tacticalBattle)
    {
        TacticalBattleList.Add(tacticalBattle);
    }

    // NetworkE.pas:1390
    public void DeleteTacticalBattle(int index)
    {
        TacticalBattleList.RemoveAt(index);
    }

    // NetworkE.pas:1395
    public void RemoveTacticalBattle(TokenControl tacticalBattle)
    {
        TacticalBattleList.Remove(tacticalBattle);
    }

    // NetworkE.pas:1400
    public void InsertTacticalBattle(int index, TokenControl tacticalBattle)
    {
        TacticalBattleList.Insert(index, tacticalBattle);
    }

    // NetworkE.pas:1405
    public void Reset()
    {
        TacticalBattleList.Clear();
        _activeTacticalBattle = null;
        _activeTacticalBattleIndex = -1;
        _state = Set<TokenExecutorState>.Of();
        _round = 0;
        _tokenCounter = 0;
    }

    // NetworkE.pas:1416
    // { Numbers an event that has no token counter yet (or an old one) and queues it }
    public void PrefetchTokenEvent(TokenEvent tokenEvent)
    {
        if (_state.Contains(TokenExecutorState.Prefetching))
        {
            if (tokenEvent._tokenCounter >= _tokenCounter || tokenEvent._tokenCounter == -1)
            {
                tokenEvent._tokenCounter = Events.Count + _tokenCounter;
                Events.Add(tokenEvent);
            }

            _executeCount = 0;
        }
    }

    // NetworkE.pas:1444
    public TokenEvent GetExecutingTokenEvent()
    {
        TokenEvent result;
        if (_state.Contains(TokenExecutorState.Executing) && Events.Count != 0)
        {
            result = Events[0];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // NetworkE.pas:1452
    public bool InsertExecuteEvent(TokenEvent tokenEvent)
    {
        bool result;
        if (_state.Contains(TokenExecutorState.Executing))
        {
            Events.Add(tokenEvent);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // NetworkE.pas:1463
    public void Initialize(int index)
    {
        _state = Set<TokenExecutorState>.Of();
        if (index < TacticalBattleList.Count)
        {
            _activeTacticalBattle = TacticalBattleList[index];
            _activeTacticalBattleIndex = index;
            _round = 0;
        }
    }

    // NetworkE.pas:1474
    public void ActivateTacticalBattle(int index)
    {
        _activeTacticalBattle = TacticalBattleList[index];
        _activeTacticalBattle.Active = true;
        _activeTacticalBattleIndex = index;
        OnActivateTacticalBattleStore.Raise(this);
        if (_state.Contains(TokenExecutorState.Executing))
        {
            _state = _state + Set<TokenExecutorState>.Of(TokenExecutorState.TacticalBattleActivated);
        }
    }

    // NetworkE.pas:1485
    // { The next active token control after the current one; a wrap starts the next round }
    public void ActivateNextTacticalBattle()
    {
        int N;
        N = 0;
        while (true)
        {
            _activeTacticalBattleIndex += 1;
            N += 1;
            if (TacticalBattleList.Count <= _activeTacticalBattleIndex)
            {
                _activeTacticalBattleIndex = 0;
                _round += 1;
            }

            if ((TacticalBattleList[_activeTacticalBattleIndex]).Active || N > TacticalBattleList.Count)
                break;
        }

        if ((TacticalBattleList[_activeTacticalBattleIndex]).Active)
        {
            ActivateTacticalBattle(_activeTacticalBattleIndex);
        }
    }

    // NetworkE.pas:1503
    public void Lock()
    {
        _lockCount += 1;
    }

    // NetworkE.pas:1508
    public void Unlock()
    {
        _lockCount -= 1;
    }

    // NetworkE.pas:1513
    public bool GetLocked() => _lockCount != 0;

    // NetworkE.pas:1519
    // { The system events run between the token controls' events; True while some remain }
    public bool ProcessSystemTokenEvents()
    {
        bool result;
        ExecuteResult R;
        if (!_state.Contains(TokenExecutorState.Executing) && SystemEvents.Count > 0 && _lockCount == 0)
        {
            if (SystemEvents[0]._tokenCounter == -1)
            {
                _executeCount = 0;
                SystemEvents[0]._tokenCounter = _tokenCounter;
            }

            R = ExecuteTokenEvent(SystemEvents[0]);
            if (R == ExecuteResult.Done || R == ExecuteResult.DoneHalt)
            {
                SystemEvents.Delete(0);
                _tokenCounter += 1;
                _executeCount = 0;
            }

            result = SystemEvents.Count != 0;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // NetworkE.pas:1543
    public void AddSystemEvent(TokenEvent tokenEvent)
    {
        SystemEvents.Add(tokenEvent);
    }

    // NetworkE.pas:1550
    // { Lets the active token control prefetch its events, executes them and activates the next token
    //   control, for at most one round of token controls }
    public void Process()
    {
        int I;
        bool prefetched;
        ExecuteResult R;
        if (_activeTacticalBattle != null && !ProcessSystemTokenEvents())
        {
            I = 0;
            prefetched = true;
            R = ExecuteResult.Done;
            while (_lockCount == 0 && prefetched == true && R == ExecuteResult.Done && I < TacticalBattleList.Count)
            {
                if (!_state.Contains(TokenExecutorState.Executing))
                {
                    _state = _state + Set<TokenExecutorState>.Of(TokenExecutorState.Prefetching);
                    Events.Clear();
                    prefetched = _activeTacticalBattle.PrefetchTokenEvents(this);
                    _state = _state - Set<TokenExecutorState>.Of(TokenExecutorState.Prefetching);
                    if (prefetched == true)
                    {
                        _state = _state + Set<TokenExecutorState>.Of(TokenExecutorState.Executing);
                        _prefetchTacticalBattle = _activeTacticalBattle;
                        _activeTacticalBattle.TokenEventsPrefetched(this, Events);
                    }
                }

                if (_state.Contains(TokenExecutorState.Executing))
                {
                    while (R == ExecuteResult.Done && Events.Count != 0)
                    {
                        R = ExecuteTokenEvent(Events[0]);
                        if (R == ExecuteResult.Done || R == ExecuteResult.DoneHalt)
                        {
                            Events.Delete(0);
                            _tokenCounter += 1;
                            _executeCount = 0;
                        }
                    }

                    if (Events.Count == 0)
                    {
                        _state = _state - Set<TokenExecutorState>.Of(TokenExecutorState.Executing);
                        OnEventsExecutedStore.Raise(this);
                        if (!_holdTacticalBattle && !_state.Contains(TokenExecutorState.TacticalBattleActivated))
                        {
                            ActivateNextTacticalBattle();
                        }

                        _state = _state - Set<TokenExecutorState>.Of(TokenExecutorState.TacticalBattleActivated);
                        _prefetchTacticalBattle = null;
                        I += 1;
                    }
                }
            }
        }
    }

    public TokenControl ActiveTacticalBattle { get => _activeTacticalBattle; }
    public TokenControl PrefetchTacticalBattle { get => _prefetchTacticalBattle; }
    public int ActiveTacticalBattleIndex { get => _activeTacticalBattleIndex; }
    public int TokenCounter { get => _tokenCounter; }
    public int Round { get => _round; }
    public int ExecuteCount { get => _executeCount; internal set => _executeCount = value; }
    public Set<TokenExecutorState> State { get => _state; }
    public bool Locked { get => GetLocked(); }
    public bool HoldTacticalBattle { get => _holdTacticalBattle; set => _holdTacticalBattle = value; }
    public bool Executing { get => _executing; }

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
    public delegate void TokenEventEvent(TokenEvent TE);

}
