// Translated from NetworkE.pas.

namespace Aow.Game.Net;

// FTokenManager, FPC. Sends the token events of the local players in token packages and files
// the received ones with their players' token controls
public partial class TokenPackageDistributor
{
    internal TokenManager _tokenManager;
    internal PlayerCommunicator _pc;

    // NetworkE.pas:1136
    // { The token events of a package from another system go to the token control of the sending player,
    //   unless they were already executed or are already there. }
    public virtual void TokenPackageReceived(TokenPackage tp)
    {
        if (_tokenManager != null)
        {
            if (_pc._playerCommunicationControl.GetLocalSystemId() != _pc._playerCommunicationControl.GetPlayers(tp._sender)._systemId)
            {
                for (int I = 0, last = tp._tokenEvents.Count - 1; I <= last; I++)
                {
                    if (tp._tokenEvents[I]._tokenCounter >= _tokenManager._tokenExecutor._tokenCounter)
                    {
                        var tokenEvent = tp._tokenEvents[I];
                        if (_tokenManager.GetTokenControl(tp._sender)._events.IndexOfTokenCounter(tokenEvent._tokenCounter) == -1)
                        {
                            var fTokenEventsItem = tp._tokenEvents[I];
                            _tokenManager.GetTokenControl(tp._sender).Add(fTokenEventsItem);
                            _tokenManager.GetTokenControl(tp._sender)._events.SortOnTokenCounter();
                        }
                        else
                        {
                            _tokenManager.TriggerMessage("TokenEvent already exists");
                        }
                    }
                    else
                    {
                        _tokenManager.TriggerMessage("TokenEvent already executed");
                    }
                }
            }
        }
    }

    // NetworkE.pas:1113
    public TokenPackageDistributor(TokenManager manager)
    {
        _tokenManager = manager;
    }

    // NetworkE.pas:1157
    public virtual void SendTokenEvents(int player, int receiver, TokenEventList events)
    {
        TokenPackage tp;
        tp = new TokenPackage(null);
        tp._tokenEvents.Assign(events, null);
        _pc.SendDataPackage(tp, player, receiver);
        tp.Release();
    }

    // NetworkE.pas:1119
    public void SetPC(PlayerCommunicator pc)
    {
        if (_pc != null)
        {
            _pc.FOnReceive -= DataPackageReceived;
        }

        _pc = pc;
        if (_pc != null)
        {
            _pc.FOnReceive += DataPackageReceived;
        }
    }

    // NetworkE.pas:1128
    public void DataPackageReceived(PlayerCommunicator sender)
    {
        if (sender._receivedPackage is TokenPackage package)
        {
            TokenPackageReceived(package);
        }
    }

    public PlayerCommunicator PC { get => _pc; set => SetPC(value); }

}
