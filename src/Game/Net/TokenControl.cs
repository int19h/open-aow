// Translated from NetworkE.pas.
using Aow.Game.Commands;

namespace Aow.Game.Net;

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
    public virtual bool PrefetchTokenEvents(TokenExecutor Executer)
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
                        TE = new SyncTokenEvent(null);
                        var syncValue = Executer.GetSyncValue();
                        ((SyncTokenEvent)TE).FSyncValue = syncValue;
                        FEvents.Insert(0, TE);
                        TE.Release();
                    }

                    TE = new PassTokenTokenEvent(null);
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
    public virtual void TokenEventsPrefetched(TokenExecutor Executer, TokenEventList Events)
    {
        if (Local() && !Global() && Events.Count != 0)
        {
            FTokenManager.FTokenPackageDistributer.SendTokenEvents(FPlayer, NetworkSession.AllPlayers, Events);
        }
    }

    // NetworkE.pas:1169
    public TokenControl(TokenManager AManager, int APlayer)
    {
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
    public int GetPrefetchableTokenEvents(TokenExecutor Executer)
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
