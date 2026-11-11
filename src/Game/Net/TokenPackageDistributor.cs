// Translated from NetworkE.pas.

namespace Aow.Game.Net;

// FTokenManager, FPC. Sends the token events of the local players in token packages and files
// the received ones with their players' token controls
public partial class TokenPackageDistributor
{
    internal TokenManager FTokenManager;
    internal PlayerCommunicator FPC;

    // NetworkE.pas:1136
    // { The token events of a package from another system go to the token control of the sending player,
    //   unless they were already executed or are already there. }
    public virtual void TokenPackageReceived(TokenPackage TP)
    {
        if (FTokenManager != null)
        {
            if (FPC.FPCC.GetLocalSystemID() != FPC.FPCC.GetPlayers(TP.FSender).FSystemID)
            {
                for (int I = 0, last = TP.FTokenEvents.Count - 1; I <= last; I++)
                {
                    if (TP.FTokenEvents[I].FTokenCounter >= FTokenManager.FTokenExecuter.FTokenCounter)
                    {
                        var tokenEvent = TP.FTokenEvents[I];
                        if (FTokenManager.GetTokenControl(TP.FSender).FEvents.IndexOfTokenCounter(tokenEvent.FTokenCounter) == -1)
                        {
                            var fTokenEventsItem = TP.FTokenEvents[I];
                            FTokenManager.GetTokenControl(TP.FSender).Add(fTokenEventsItem);
                            FTokenManager.GetTokenControl(TP.FSender).FEvents.SortOnTokenCounter();
                        }
                        else
                        {
                            FTokenManager.TriggerMessage("TokenEvent already exists");
                        }
                    }
                    else
                    {
                        FTokenManager.TriggerMessage("TokenEvent already executed");
                    }
                }
            }
        }
    }

    // NetworkE.pas:1113
    public TokenPackageDistributor(TokenManager AManager)
    {
        FTokenManager = AManager;
    }

    // NetworkE.pas:1157
    public virtual void SendTokenEvents(int APlayer, int AReceiver, TokenEventList Events)
    {
        TokenPackage TP;
        TP = new TokenPackage(null);
        TP.FTokenEvents.Assign(Events, null);
        FPC.SendDataPackage(TP, APlayer, AReceiver);
        TP.Release();
    }

    // NetworkE.pas:1119
    public void SetPC(PlayerCommunicator PC)
    {
        if (FPC != null)
        {
            FPC.FOnReceive -= DataPackageReceived;
        }

        FPC = PC;
        if (FPC != null)
        {
            FPC.FOnReceive += DataPackageReceived;
        }
    }

    // NetworkE.pas:1128
    public void DataPackageReceived(PlayerCommunicator Sender)
    {
        if (Sender.FReceivedPackage is TokenPackage package)
        {
            TokenPackageReceived(package);
        }
    }

    public PlayerCommunicator PC { get => FPC; set => SetPC(value); }

}
