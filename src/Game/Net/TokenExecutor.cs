// Translated from NetworkE.pas.
using Aow.Engine;

using Aow.Game.Commands;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net;

// _activeTacticalBattle, _prefetchTacticalBattle, _activeTacticalBattleIndex, _events, _tacticalBattleList, _tokenCounter, _round,
// _executeCount, _state, FOnEventsExecuted, FOnActivateTacticalBattle, _onSyncError, _onGetSyncValue,
// _onExecuteTokenEvent, _executing, _lockCount, _systemEvents, _holdTacticalBattle, _tokenManager
public partial class TokenExecutor : IDisposable
{
    internal TokenControl _activeTacticalBattle;
    internal TokenControl _prefetchTacticalBattle;
    internal int _activeTacticalBattleIndex;
    internal TokenEventList _events;
    internal List<TokenControl> _tacticalBattleList;
    internal int _tokenCounter;
    internal int _round;
    internal int _executeCount;
    internal Set<TokenExecutorState> _state;

    internal EventStore<TokenExecutor> _onEventsExecutedStore;

    internal event NotifyEvent<TokenExecutor> FOnEventsExecuted
    {
        add => _onEventsExecutedStore.Add(value);
        remove => _onEventsExecutedStore.Remove(value);
    }

    internal EventStore<TokenExecutor> _onActivateTacticalBattleStore;

    internal event NotifyEvent<TokenExecutor> FOnActivateTacticalBattle
    {
        add => _onActivateTacticalBattleStore.Add(value);
        remove => _onActivateTacticalBattleStore.Remove(value);
    }

    internal NotifyEvent<object> _onSyncError;
    internal GetSyncValueEvent _onGetSyncValue;
    internal TokenEventEvent _onExecuteTokenEvent;
    internal bool _executing;
    internal int _lockCount;
    internal TokenEventList _systemEvents;
    internal bool _holdTacticalBattle;
    internal TokenManager _tokenManager;

    // NetworkE.pas:1429
    public virtual ExecuteResult ExecuteTokenEvent(TokenEvent tokenEvent)
    {
        ExecuteResult result = default;
        _executing = true;
        if (_onExecuteTokenEvent != null)
        {
            _onExecuteTokenEvent(tokenEvent);
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
        _tokenManager = manager;
        _events = new TokenEventList(null);
        _tacticalBattleList = new List<TokenControl>();
        _onEventsExecutedStore = new();
        _onActivateTacticalBattleStore = new();
        _systemEvents = new TokenEventList(null);
    }

    // NetworkE.pas:1361
    public virtual void Dispose()
    {
        Reset();
        _events?.Dispose();
        _tacticalBattleList?.Clear();
        _onEventsExecutedStore.Clear();
        _onActivateTacticalBattleStore.Clear();
        _systemEvents?.Dispose();
    }

    // NetworkE.pas:1372
    public int GetSyncValue()
    {
        int result = default;
        if (_onGetSyncValue != null)
        {
            _onGetSyncValue(ref result);
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // NetworkE.pas:1380
    public TokenControl GetTacticalBattle(int index) => _tacticalBattleList[index];

    // NetworkE.pas:1385
    public void AddTacticalBattle(TokenControl tacticalBattle)
    {
        _tacticalBattleList.Add(tacticalBattle);
    }

    // NetworkE.pas:1390
    public void DeleteTacticalBattle(int index)
    {
        _tacticalBattleList.RemoveAt(index);
    }

    // NetworkE.pas:1395
    public void RemoveTacticalBattle(TokenControl tacticalBattle)
    {
        _tacticalBattleList.Remove(tacticalBattle);
    }

    // NetworkE.pas:1400
    public void InsertTacticalBattle(int index, TokenControl tacticalBattle)
    {
        _tacticalBattleList.Insert(index, tacticalBattle);
    }

    // NetworkE.pas:1405
    public void Reset()
    {
        _tacticalBattleList.Clear();
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
                tokenEvent._tokenCounter = _events.Count + _tokenCounter;
                _events.Add(tokenEvent);
            }

            _executeCount = 0;
        }
    }

    // NetworkE.pas:1444
    public TokenEvent GetExecutingTokenEvent()
    {
        TokenEvent result;
        if (_state.Contains(TokenExecutorState.Executing) && _events.Count != 0)
        {
            result = _events[0];
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
            _events.Add(tokenEvent);
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
        if (index < _tacticalBattleList.Count)
        {
            _activeTacticalBattle = _tacticalBattleList[index];
            _activeTacticalBattleIndex = index;
            _round = 0;
        }
    }

    // NetworkE.pas:1474
    public void ActivateTacticalBattle(int index)
    {
        _activeTacticalBattle = _tacticalBattleList[index];
        _activeTacticalBattle._active = true;
        _activeTacticalBattleIndex = index;
        _onActivateTacticalBattleStore.Raise(this);
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
            if (_tacticalBattleList.Count <= _activeTacticalBattleIndex)
            {
                _activeTacticalBattleIndex = 0;
                _round += 1;
            }

            if ((_tacticalBattleList[_activeTacticalBattleIndex])._active || N > _tacticalBattleList.Count)
                break;
        }

        if ((_tacticalBattleList[_activeTacticalBattleIndex])._active)
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
        if (!_state.Contains(TokenExecutorState.Executing) && _systemEvents.Count > 0 && _lockCount == 0)
        {
            if (_systemEvents[0]._tokenCounter == -1)
            {
                _executeCount = 0;
                _systemEvents[0]._tokenCounter = _tokenCounter;
            }

            R = ExecuteTokenEvent(_systemEvents[0]);
            if (R == ExecuteResult.Done || R == ExecuteResult.DoneHalt)
            {
                _systemEvents.Delete(0);
                _tokenCounter += 1;
                _executeCount = 0;
            }

            result = _systemEvents.Count != 0;
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
        _systemEvents.Add(tokenEvent);
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
            while (_lockCount == 0 && prefetched == true && R == ExecuteResult.Done && I < _tacticalBattleList.Count)
            {
                if (!_state.Contains(TokenExecutorState.Executing))
                {
                    _state = _state + Set<TokenExecutorState>.Of(TokenExecutorState.Prefetching);
                    _events.Clear();
                    prefetched = _activeTacticalBattle.PrefetchTokenEvents(this);
                    _state = _state - Set<TokenExecutorState>.Of(TokenExecutorState.Prefetching);
                    if (prefetched == true)
                    {
                        _state = _state + Set<TokenExecutorState>.Of(TokenExecutorState.Executing);
                        _prefetchTacticalBattle = _activeTacticalBattle;
                        _activeTacticalBattle.TokenEventsPrefetched(this, _events);
                    }
                }

                if (_state.Contains(TokenExecutorState.Executing))
                {
                    while (R == ExecuteResult.Done && _events.Count != 0)
                    {
                        R = ExecuteTokenEvent(_events[0]);
                        if (R == ExecuteResult.Done || R == ExecuteResult.DoneHalt)
                        {
                            _events.Delete(0);
                            _tokenCounter += 1;
                            _executeCount = 0;
                        }
                    }

                    if (_events.Count == 0)
                    {
                        _state = _state - Set<TokenExecutorState>.Of(TokenExecutorState.Executing);
                        _onEventsExecutedStore.Raise(this);
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
    public int ExecuteCount { get => _executeCount; }
    public Set<TokenExecutorState> State { get => _state; }
    public bool Locked { get => GetLocked(); }
    public bool HoldTacticalBattle { get => _holdTacticalBattle; set => _holdTacticalBattle = value; }
    public bool Executing { get => _executing; }

    public event NotifyEvent<TokenExecutor> OnEventsExecuted
    {
        add => _onEventsExecutedStore.Add(value);
        remove => _onEventsExecutedStore.Remove(value);
    }

    public event NotifyEvent<TokenExecutor> OnActivateTacticalBattle
    {
        add => _onActivateTacticalBattleStore.Add(value);
        remove => _onActivateTacticalBattleStore.Remove(value);
    }

    public NotifyEvent<object> OnSyncError { get => _onSyncError; set => _onSyncError = value; }
    public GetSyncValueEvent OnGetSyncValue { get => _onGetSyncValue; set => _onGetSyncValue = value; }
    public TokenEventEvent OnExecuteTokenEvent { get => _onExecuteTokenEvent; set => _onExecuteTokenEvent = value; }

    public delegate void GetSyncValueEvent(ref int Value);
    public delegate void TokenEventEvent(TokenEvent TE);

}
