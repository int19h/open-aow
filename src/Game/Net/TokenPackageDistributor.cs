#nullable disable
// Translated from NetworkE.pas.

using Aow.Game.Commands;

namespace Aow.Game.Net;

// FTokenManager, FPC. Sends the token events of the local players in token packages and files
// the received ones with their players' token controls
public class TokenPackageDistributor(TokenManager manager)
{
    internal TokenManager TokenManager { get; } = manager;
    internal PlayerCommunicator Pc { get; private set; }

    // NetworkE.pas:1136
    // { The token events of a package from another system go to the token control of the sending player,
    //   unless they were already executed or are already there. }
    public virtual void TokenPackageReceived(TokenPackage tp)
    {
        if (TokenManager is not null)
        {
            if (Pc.PlayerCommunicationControl.LocalSystemId != Pc.PlayerCommunicationControl.GetPlayers(tp.Sender).SystemId)
            {
                for (int i = 0, last = tp.TokenEvents.Count - 1; i <= last; i++)
                {
                    if (tp.TokenEvents[i].TokenCounter >= TokenManager.TokenExecutor.TokenCounter)
                    {
                        TokenEvent tokenEvent = tp.TokenEvents[i];
                        if (TokenManager.GetTokenControl(tp.Sender).Events.IndexOfTokenCounter(tokenEvent.TokenCounter) == -1)
                        {
                            TokenEvent fTokenEventsItem = tp.TokenEvents[i];
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

    // NetworkE.pas:1157
    public virtual void SendTokenEvents(int player, int receiver, TokenEventList events)
    {
        TokenPackage tp = new(null) { Holder = Pc };
        tp.TokenEvents.Assign(events, engine: null);
        _ = Pc.SendDataPackage(tp, sender: player, receiver);
        _ = tp.Release();
    }

    // NetworkE.pas:1119
    public void SetPC(PlayerCommunicator pc)
    {
        Pc?.Received -= DataPackageReceived;

        Pc = pc;
        Pc?.Received += DataPackageReceived;
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
