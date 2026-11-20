#nullable disable
// Translated from NetworkE.pas.
using Aow.Game.Commands;

namespace Aow.Game.Net;

// FEvents, FTokenManager, FPlayer, FActive. The token events of one player
public partial class TokenControl : IDisposable
{
    private readonly TokenManager _tokenManager;

    // NetworkE.pas:1189
    // { Keeps the events sorted on their token counters; an event without one goes to the end }
    public virtual void Add(TokenEvent tokenEvent)
    {
        int i;
        if (tokenEvent.TokenCounter != -1)
        {
            i = 0;
            while (i != Events.Count && Events[i].TokenCounter < tokenEvent.TokenCounter)
            {
                i += 1;
            }

            if (i < Events.Count)
            {
                if (Events[i].TokenCounter == tokenEvent.TokenCounter)
                {
                    throw new InvalidOperationException("Double TokenCounter defined");
                }

                Events.Insert(i, tokenEvent);
            }
            else
            {
                Events.Add(tokenEvent);
            }
        }
        else
        {
            Events.Add(tokenEvent);
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
            if (Events.Count != 0 || !executor.HoldTacticalBattle)
            {
                if (Local())
                {
                    if (executor.HasSyncErrorHandlers)
                    {
                        SyncTokenEvent syncEvent = new(null) { Holder = _tokenManager.Owner };
                        int syncValue = executor.GetSyncValue();
                        syncEvent.SyncValue = syncValue;
                        Events.Insert(0, syncEvent);
                        _ = syncEvent.Release();
                    }

                    tokenEvent = new PassTokenTokenEvent(null) { Holder = _tokenManager.Owner };
                    Events.Add(tokenEvent);
                    _ = tokenEvent.Release();
                }

                for (int i = 0, last = Events.Count - 1; i <= last; i++)
                {
                    executor.PrefetchTokenEvent(Events[i]);
                }

                Events.Clear();
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
                        executor.PrefetchTokenEvent(Events[i]);
                    }

                    for (int i = n - 1; i >= 0; i--)
                    {
                        Events.Delete(i);
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
            _tokenManager.TokenPackageDistributor.SendTokenEvents(Player, NetworkSession.AllPlayers, events);
        }
    }

    // NetworkE.pas:1169
    public TokenControl(TokenManager manager, int player)
    {
        // A construction test passes no manager.
        Events = new TokenEventList(null) { Holder = manager?.Owner };
        _tokenManager = manager;
        Player = player;
        Active = true;
        if (_tokenManager is not null && _tokenManager.TokenExecutor is not null)
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
        if (_tokenManager is not null && _tokenManager.TokenExecutor is not null)
        {
            _tokenManager.TokenExecutor.RemoveTacticalBattle(this);
        }

        Events?.Dispose();
    }

    // NetworkE.pas:1223
    public void ClearEvents() => Events.Clear();

    // NetworkE.pas:1228
    public TokenEvent GetEvent(int index) => Events[index];

    // NetworkE.pas:1233
    public int GetCount() => Events.Count;

    // NetworkE.pas:1246
    public bool Local()
    {
        PlayerCommunicationControl playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(Player).ConnectionMode == ConnectionMode.Local;
    }

    // NetworkE.pas:1252
    public bool Global()
    {
        PlayerCommunicationControl playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(Player).ConnectionMode == ConnectionMode.Global;
    }

    // NetworkE.pas:1258
    public bool Remote()
    {
        PlayerCommunicationControl playerCommunicationControl = _tokenManager.TokenPackageDistributor.Pc.PlayerCommunicationControl;
        return playerCommunicationControl.GetPlayers(Player).ConnectionMode == ConnectionMode.Remote;
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
            while (inOrder && !passToken && i < Events.Count)
            {
                inOrder = Events[i].TokenCounter == executor.TokenCounter + i;
                passToken = Events[i].EngineClass.ClassId == NetworkSession.ClassIdPassTokenTokenEvent;
                i += 1;
            }

            result = inOrder ? i : 0;
        }
        else
        {
            result = Events.Count;
        }

        return result;
    }

    public TokenEventList Events { get; }
    public int Player { get; }
    public bool Active { get; set; }
    public int Count => GetCount();

}
