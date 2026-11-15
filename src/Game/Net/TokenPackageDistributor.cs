#nullable disable
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
            if (Pc.PlayerCommunicationControl.GetLocalSystemId() != Pc.PlayerCommunicationControl.GetPlayers(tp.Sender).SystemId)
            {
                for (int i = 0, last = tp.TokenEvents.Count - 1; i <= last; i++)
                {
                    if (tp.TokenEvents[i].TokenCounter >= TokenManager.TokenExecutor.TokenCounter)
                    {
                        var tokenEvent = tp.TokenEvents[i];
                        if (TokenManager.GetTokenControl(tp.Sender).Events.IndexOfTokenCounter(tokenEvent.TokenCounter) == -1)
                        {
                            var fTokenEventsItem = tp.TokenEvents[i];
                            TokenManager.GetTokenControl(tp.Sender).Add(fTokenEventsItem);
                            TokenManager.GetTokenControl(tp.Sender).Events.SortOnTokenCounter();
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
        tp = new TokenPackage(null) { Holder = Pc };
        tp.TokenEvents.Assign(events, null);
        Pc.SendDataPackage(tp, player, receiver);
        tp.Release();
    }

    // NetworkE.pas:1119
    public void SetPC(PlayerCommunicator pc)
    {
        if (Pc != null)
        {
            Pc.Received -= DataPackageReceived;
        }

        Pc = pc;
        if (Pc != null)
        {
            Pc.Received += DataPackageReceived;
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
