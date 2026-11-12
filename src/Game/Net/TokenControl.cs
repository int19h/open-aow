// Translated from NetworkE.pas.
using Aow.Game.Commands;

namespace Aow.Game.Net;

// FEvents, FTokenManager, FPlayer, FActive. The token events of one player
public partial class TokenControl : IDisposable
{
    private readonly TokenEventList _events;
    private readonly TokenManager _tokenManager;
    private readonly int _player;
    private bool _active;

    // NetworkE.pas:1189
    // { Keeps the events sorted on their token counters; an event without one goes to the end }
    public virtual void Add(TokenEvent tokenEvent)
    {
        int I;
        if (tokenEvent._tokenCounter != -1)
        {
            I = 0;
            while (I != _events.Count && _events[I]._tokenCounter < tokenEvent._tokenCounter)
            {
                I += 1;
            }

            if (I < _events.Count)
            {
                if (_events[I]._tokenCounter == tokenEvent._tokenCounter)
                {
                    throw new InvalidOperationException("Double TokenCounter defined");
                }

                _events.Insert(I, tokenEvent);
            }
            else
            {
                _events.Add(tokenEvent);
            }
        }
        else
        {
            _events.Add(tokenEvent);
        }
    }

    // NetworkE.pas:1293
    // { A local player's events end with a TPassTokenTE (after a TSyncTE when the executer checks the
    //   sync); a remote player's events are prefetched only when they are complete }
    public virtual bool PrefetchTokenEvents(TokenExecutor executor)
    {
        bool result;
        int N;
        TokenEvent tokenEvent;
        if (Global() || Local())
        {
            if (_events.Count != 0 || executor.HoldTacticalBattle != true)
            {
                if (Local())
                {
                    if (executor.HasSyncErrorHandlers)
                    {
                        tokenEvent = new SyncTokenEvent(null);
                        var syncValue = executor.GetSyncValue();
                        ((SyncTokenEvent)tokenEvent)._syncValue = syncValue;
                        _events.Insert(0, tokenEvent);
                        tokenEvent.Release();
                    }

                    tokenEvent = new PassTokenTokenEvent(null);
                    _events.Add(tokenEvent);
                    tokenEvent.Release();
                }

                for (int I = 0, last = _events.Count - 1; I <= last; I++)
                {
                    executor.PrefetchTokenEvent(_events[I]);
                }

                _events.Clear();
                result = true;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            if (Remote())
            {
                N = GetPrefetchableTokenEvents(executor);
                if (N > 0)
                {
                    for (int I = 0; I <= N - 1; I++)
                    {
                        executor.PrefetchTokenEvent(_events[I]);
                    }

                    for (int I = N - 1; I >= 0; I--)
                    {
                        _events.Delete(I);
                    }

                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = true;
            }
        }

        return result;
    }

    // NetworkE.pas:1342
    // { A local player's prefetched events go to every other player }
    public virtual void TokenEventsPrefetched(TokenExecutor executor, TokenEventList events)
    {
        if (Local() && !Global() && events.Count != 0)
        {
            _tokenManager.TokenPackageDistributor.SendTokenEvents(_player, NetworkSession.AllPlayers, events);
        }
    }

    // NetworkE.pas:1169
    public TokenControl(TokenManager manager, int player)
    {
        _events = new TokenEventList(null);
        _tokenManager = manager;
        _player = player;
        _active = true;
        if (_tokenManager != null && _tokenManager.TokenExecutor != null)
        {
            _tokenManager.TokenExecutor.AddTacticalBattle(this);
        }
    }

    // NetworkE.pas:1211
    public virtual void AddEvent(TokenEvent tokenEvent)
    {
        if (Remote())
        {
            throw new InvalidOperationException("Cannot Add Remote events");
        }

        Add(tokenEvent);
    }

    // NetworkE.pas:1218
    public virtual void AddLocalEvent(TokenEvent tokenEvent)
    {
        Add(tokenEvent);
    }

    // NetworkE.pas:1238
    public virtual void AddEvents(TokenEventList events)
    {
        for (int I = 0, last = events.Count - 1; I <= last; I++)
        {
            AddEvent(events[I]);
        }
    }

    // NetworkE.pas:1180
    public virtual void Dispose()
    {
        if (_tokenManager != null && _tokenManager.TokenExecutor != null)
        {
            _tokenManager.TokenExecutor.RemoveTacticalBattle(this);
        }

        _events?.Dispose();
    }

    // NetworkE.pas:1223
    public void ClearEvents()
    {
        _events.Clear();
    }

    // NetworkE.pas:1228
    public TokenEvent GetEvent(int index) => _events[index];

    // NetworkE.pas:1233
    public int GetCount() => _events.Count;

    // NetworkE.pas:1246
    public bool Local()
    {
        var playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(_player)._connectionMode == ConnectionMode.Local;
    }

    // NetworkE.pas:1252
    public bool Global()
    {
        var playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(_player)._connectionMode == ConnectionMode.Global;
    }

    // NetworkE.pas:1258
    public bool Remote()
    {
        var playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(_player)._connectionMode == ConnectionMode.Remote;
    }

    // NetworkE.pas:1266
    // { The number of events of a remote player that can be executed now: they must follow the
    //   executer's token counter without a gap, up to and including the first TPassTokenTE }
    public int GetPrefetchableTokenEvents(TokenExecutor executor)
    {
        int result;
        int I;
        bool inOrder;
        bool passToken;
        if (Remote())
        {
            I = 0;
            inOrder = true;
            passToken = false;
            while (inOrder && !passToken && I < _events.Count)
            {
                inOrder = _events[I]._tokenCounter == executor.TokenCounter + I;
                passToken = _events[I].EngineClass.ClassId == NetworkSession.ClassIdPassTokenTokenEvent;
                I += 1;
            }

            if (inOrder)
            {
                result = I;
            }
            else
            {
                result = 0;
            }
        }
        else
        {
            result = _events.Count;
        }

        return result;
    }

    public TokenEventList Events { get => _events; }
    public int Player { get => _player; }
    public bool Active { get => _active; set => _active = value; }
    public int Count { get => GetCount(); }

}
