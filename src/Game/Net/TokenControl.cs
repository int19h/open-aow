#nullable disable
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
        int i;
        if (tokenEvent.TokenCounter != -1)
        {
            i = 0;
            while (i != _events.Count && _events[i].TokenCounter < tokenEvent.TokenCounter)
            {
                i += 1;
            }

            if (i < _events.Count)
            {
                if (_events[i].TokenCounter == tokenEvent.TokenCounter)
                {
                    throw new InvalidOperationException("Double TokenCounter defined");
                }

                _events.Insert(i, tokenEvent);
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
        int n;
        TokenEvent tokenEvent;
        if (Global() || Local())
        {
            if (_events.Count != 0 || !executor.HoldTacticalBattle)
            {
                if (Local())
                {
                    if (executor.HasSyncErrorHandlers)
                    {
                        tokenEvent = new SyncTokenEvent(null) { Holder = _tokenManager.Owner };
                        int syncValue = executor.GetSyncValue();
                        ((SyncTokenEvent)tokenEvent).SyncValue = syncValue;
                        _events.Insert(0, tokenEvent);
                        tokenEvent.Release();
                    }

                    tokenEvent = new PassTokenTokenEvent(null) { Holder = _tokenManager.Owner };
                    _events.Add(tokenEvent);
                    tokenEvent.Release();
                }

                for (int i = 0, last = _events.Count - 1; i <= last; i++)
                {
                    executor.PrefetchTokenEvent(_events[i]);
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
                n = GetPrefetchableTokenEvents(executor);
                if (n > 0)
                {
                    for (int i = 0; i <= n - 1; i++)
                    {
                        executor.PrefetchTokenEvent(_events[i]);
                    }

                    for (int i = n - 1; i >= 0; i--)
                    {
                        _events.Delete(i);
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
        // A construction test passes no manager.
        _events = new TokenEventList(null) { Holder = manager?.Owner };
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
    public virtual void AddLocalEvent(TokenEvent tokenEvent) => Add(tokenEvent);

    // NetworkE.pas:1238
    public virtual void AddEvents(TokenEventList events)
    {
        for (int i = 0, last = events.Count - 1; i <= last; i++)
        {
            AddEvent(events[i]);
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
    public void ClearEvents() => _events.Clear();

    // NetworkE.pas:1228
    public TokenEvent GetEvent(int index) => _events[index];

    // NetworkE.pas:1233
    public int GetCount() => _events.Count;

    // NetworkE.pas:1246
    public bool Local()
    {
        PlayerCommunicationControl playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(_player).ConnectionMode == ConnectionMode.Local;
    }

    // NetworkE.pas:1252
    public bool Global()
    {
        PlayerCommunicationControl playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(_player).ConnectionMode == ConnectionMode.Global;
    }

    // NetworkE.pas:1258
    public bool Remote()
    {
        PlayerCommunicationControl playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(_player).ConnectionMode == ConnectionMode.Remote;
    }

    // NetworkE.pas:1266
    // { The number of events of a remote player that can be executed now: they must follow the
    //   executer's token counter without a gap, up to and including the first TPassTokenTE }
    public int GetPrefetchableTokenEvents(TokenExecutor executor)
    {
        int result;
        int i;
        bool inOrder;
        bool passToken;
        if (Remote())
        {
            i = 0;
            inOrder = true;
            passToken = false;
            while (inOrder && !passToken && i < _events.Count)
            {
                inOrder = _events[i].TokenCounter == executor.TokenCounter + i;
                passToken = _events[i].EngineClass.ClassId == NetworkSession.ClassIdPassTokenTokenEvent;
                i += 1;
            }

            if (inOrder)
            {
                result = i;
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

    public TokenEventList Events => _events;
    public int Player => _player;
    public bool Active { get => _active; set => _active = value; }
    public int Count => GetCount();

}
