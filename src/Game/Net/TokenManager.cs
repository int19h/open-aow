// Translated from NetworkE.pas.
using Aow.Rtl;

namespace Aow.Game.Net;

// _tokenExecutor, _tokenPackageDistributor, _owner, _tokenControls (one per player, nil for a
// free slot), _onMessage
public partial class TokenManager : IDisposable
{

    public TokenManager(object owner)
    {
        _owner = owner;
        _tokenControls = new List<TokenControl>();
        _tokenExecutor = CreateTokenExecutor();
        _tokenPackageDistributor = CreateTokenPackageDistributor();
    }

    internal TokenExecutor _tokenExecutor;
    internal TokenPackageDistributor _tokenPackageDistributor;
    internal object _owner;
    internal List<TokenControl> _tokenControls;
    internal NetworkMessageEvent _onMessage;

    // NetworkE.pas:1642
    public virtual TokenExecutor CreateTokenExecutor() => new TokenExecutor(this);

    // NetworkE.pas:1647
    public virtual TokenPackageDistributor CreateTokenPackageDistributor() => new TokenPackageDistributor(this);

    // NetworkE.pas:1652
    public virtual TokenControl CreateTokenControl(int player) => new TokenControl(this, player);

    // NetworkE.pas:1628
    public virtual void Initialize(PlayerCommunicator pc)
    {
        _tokenPackageDistributor.SetPC(pc);
        if (pc._playerCommunicationControl.GetCount() > 0)
        {
            ClearPlayers();
            for (int I = 0, last = pc._playerCommunicationControl.GetCount() - 1; I <= last; I++)
            {
                CreatePlayer(I);
            }
        }

        _tokenExecutor.Initialize(0);
    }

    // NetworkE.pas:1657
    public virtual void Process()
    {
        if (_tokenExecutor != null)
        {
            _tokenExecutor.Process();
        }
    }

    // NetworkE.pas:1694
    // { A player is ready when nothing of his waits: no system events, no token events, and his events
    //   are not being executed }
    public virtual bool PlayerReady(int player)
    {
        bool result;
        if (player < _tokenControls.Count && _tokenControls[player] != null)
        {
            bool condition = _tokenExecutor._systemEvents.Count != 0 || (_tokenControls[player]).GetCount() != 0;
            if (!condition)
            {
                var item = _tokenControls[player];
                condition = _tokenExecutor._prefetchTacticalBattle == item && _tokenExecutor._state.Contains(TokenExecutorState.Executing);
            }

            if (condition)
            {
                result = false;
            }
            else
            {
                result = true;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // NetworkE.pas:1663
    public virtual void CreatePlayer(int player)
    {
        if (player >= _tokenControls.Count || _tokenControls[player] == null)
        {
            while (player >= _tokenControls.Count)
            {
                _tokenControls.Add(null);
            }

            var createTokenControl = CreateTokenControl(player);
            _tokenControls[player] = createTokenControl;
        }
    }

    // NetworkE.pas:1673
    public virtual void DeletePlayer(int player)
    {
        if (player < _tokenControls.Count && _tokenControls[player] != null)
        {
            (_tokenControls[player] as IDisposable)?.Dispose();
            _tokenControls[player] = null;
        }
    }

    // NetworkE.pas:1682
    public virtual void ClearPlayers()
    {
        while (_tokenControls.Count != 0)
        {
            if (_tokenControls[_tokenControls.Count - 1] != null)
            {
                (_tokenControls[_tokenControls.Count - 1] as IDisposable)?.Dispose();
            }

            _tokenControls.RemoveAt(_tokenControls.Count - 1);
        }
    }

    // NetworkE.pas:1606
    // { The original does not call the inherited constructor. }

    // NetworkE.pas:1614
    public virtual void Dispose()
    {
        ClearPlayers();
        _tokenControls?.Clear();
        _tokenExecutor?.Dispose();
    }

    // NetworkE.pas:1623
    public int GetCount() => _tokenControls.Count;

    // NetworkE.pas:1710
    public TokenControl GetTokenControl(int player)
    {
        TokenControl result;
        if (player < _tokenControls.Count)
        {
            result = _tokenControls[player];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // NetworkE.pas:1718
    public void TriggerMessage(string message)
    {
        if (_onMessage != null)
        {
            _onMessage(this, message);
        }
    }

    public TokenExecutor TokenExecutor { get => _tokenExecutor; }
    public TokenPackageDistributor TokenPackageDistributor { get => _tokenPackageDistributor; }
    public object Owner { get => _owner; }
    public int Count { get => GetCount(); }

    public sealed class TokenControlsAccessor(TokenManager target)
    {
        public TokenControl this[int player] { get => target.GetTokenControl(player); }
    }

    public TokenControlsAccessor TokenControls => new TokenControlsAccessor(this);
    public NetworkMessageEvent OnMessage { get => _onMessage; set => _onMessage = value; }

    public delegate void NetworkMessageEvent(object Sender, string Msg);

}
