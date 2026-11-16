#nullable disable
// Translated from NetworkE.pas.
namespace Aow.Game.Net;

// _tokenExecutor, _tokenPackageDistributor, _owner, _tokenControls (one per player, nil for a
// free slot), OnMessage
public partial class TokenManager : IDisposable
{

    public TokenManager(GameEngine owner)
    {
        _owner = owner;
        _tokenControls = [];
        _tokenExecutor = CreateTokenExecutor();
        _tokenPackageDistributor = CreateTokenPackageDistributor();
    }

    private TokenExecutor _tokenExecutor;
    private readonly TokenPackageDistributor _tokenPackageDistributor;
    private readonly GameEngine _owner;
    private readonly List<TokenControl> _tokenControls;
    public event NetworkMessageEvent OnMessage;

    // NetworkE.pas:1642
    public virtual TokenExecutor CreateTokenExecutor() => new(this);

    // NetworkE.pas:1647
    public virtual TokenPackageDistributor CreateTokenPackageDistributor() => new(this);

    // NetworkE.pas:1652
    public virtual TokenControl CreateTokenControl(int player) => new(this, player);

    // NetworkE.pas:1628
    public virtual void Initialize(PlayerCommunicator pc)
    {
        _tokenPackageDistributor.SetPC(pc);
        if (pc.PlayerCommunicationControl.GetCount() > 0)
        {
            ClearPlayers();
            for (int i = 0, last = pc.PlayerCommunicationControl.GetCount() - 1; i <= last; i++)
            {
                CreatePlayer(i);
            }
        }

        _tokenExecutor.Initialize(0);
    }

    // NetworkE.pas:1657
    public virtual void Process() => _tokenExecutor?.Process();

    // NetworkE.pas:1694
    // { A player is ready when nothing of his waits: no system events, no token events, and his events
    //   are not being executed }
    public virtual bool PlayerReady(int player)
    {
        bool result;
        if (player < _tokenControls.Count && _tokenControls[player] != null)
        {
            bool condition = _tokenExecutor.SystemEvents.Count != 0 || _tokenControls[player].GetCount() != 0;
            if (!condition)
            {
                TokenControl item = _tokenControls[player];
                condition = _tokenExecutor.PrefetchTacticalBattle == item && _tokenExecutor.State.Contains(TokenExecutorState.Executing);
            }

            result = !condition;
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

            TokenControl createTokenControl = CreateTokenControl(player);
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
        TokenControl result = player < _tokenControls.Count ? _tokenControls[player] : null;
        return result;
    }

    // NetworkE.pas:1718
    public void TriggerMessage(string message) => OnMessage?.Invoke(this, message);

    public TokenExecutor TokenExecutor => _tokenExecutor;
    public TokenPackageDistributor TokenPackageDistributor => _tokenPackageDistributor;
    public GameEngine Owner => _owner;
    public int Count => GetCount();

    public sealed class TokenControlsAccessor(TokenManager target)
    {
        public TokenControl this[int player] => target.GetTokenControl(player);
    }

    public TokenControlsAccessor TokenControls => new(this);

    public delegate void NetworkMessageEvent(object Sender, string Msg);

}
