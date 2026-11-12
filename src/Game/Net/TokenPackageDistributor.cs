// Translated from NetworkE.pas.

namespace Aow.Game.Net;

// FTokenManager, FPC. Sends the token events of the local players in token packages and files
// the received ones with their players' token controls
public partial class TokenPackageDistributor
{
    internal TokenManager TokenManager { get; }
    internal PlayerCommunicator Pc { get; private set; }

    // NetworkE.pas:1136
    // { The token events of a package from another system go to the token control of the sending player,
    //   unless they were already executed or are already there. }
    public virtual void TokenPackageReceived(TokenPackage tp)
    {
        if (TokenManager != null)
        {
            if (Pc.PlayerCommunicationControl.GetLocalSystemId() != Pc.PlayerCommunicationControl.GetPlayers(tp._sender)._systemId)
            {
                for (int I = 0, last = tp.TokenEvents.Count - 1; I <= last; I++)
                {
                    if (tp.TokenEvents[I]._tokenCounter >= TokenManager.TokenExecutor.TokenCounter)
                    {
                        var tokenEvent = tp.TokenEvents[I];
                        if (TokenManager.GetTokenControl(tp._sender).Events.IndexOfTokenCounter(tokenEvent._tokenCounter) == -1)
                        {
                            var fTokenEventsItem = tp.TokenEvents[I];
                            TokenManager.GetTokenControl(tp._sender).Add(fTokenEventsItem);
                            TokenManager.GetTokenControl(tp._sender).Events.SortOnTokenCounter();
                        }
                        else
                        {
                            TokenManager.TriggerMessage("TokenEvent already exists");
                        }
                    }
                    else
                    {
                        TokenManager.TriggerMessage("TokenEvent already executed");
                    }
                }
            }
        }
    }

    // NetworkE.pas:1113
    public TokenPackageDistributor(TokenManager manager)
    {
        TokenManager = manager;
    }

    // NetworkE.pas:1157
    public virtual void SendTokenEvents(int player, int receiver, TokenEventList events)
    {
        TokenPackage tp;
        tp = new TokenPackage(null);
        tp.TokenEvents.Assign(events, null);
        Pc.SendDataPackage(tp, player, receiver);
        tp.Release();
    }

    // NetworkE.pas:1119
    public void SetPC(PlayerCommunicator pc)
    {
        if (Pc != null)
        {
            Pc.FOnReceive -= DataPackageReceived;
        }

        Pc = pc;
        if (Pc != null)
        {
            Pc.FOnReceive += DataPackageReceived;
        }
    }

    // NetworkE.pas:1128
    public void DataPackageReceived(PlayerCommunicator sender)
    {
        if (sender.ReceivedPackage is TokenPackage package)
        {
            TokenPackageReceived(package);
        }
    }

    public PlayerCommunicator PC { get => Pc; set => SetPC(value); }

}
