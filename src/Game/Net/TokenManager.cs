#nullable disable
// Translated from NetworkE.pas.
namespace Aow.Game.Net;

// _tokenExecutor, _tokenPackageDistributor, _owner, _tokenControls (one per player, nil for a
// free slot), OnMessage
public class TokenManager : IDisposable
{

    public TokenManager(GameEngine owner)
    {
        Owner = owner;
        _tokenControls = [];
        TokenExecutor = CreateTokenExecutor();
        TokenPackageDistributor = CreateTokenPackageDistributor();
    }

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
        TokenPackageDistributor.SetPC(pc);
        if (pc.PlayerCommunicationControl.GetCount() > 0)
        {
            ClearPlayers();
            for (int i = 0, last = pc.PlayerCommunicationControl.GetCount() - 1; i <= last; i++)
            {
                CreatePlayer(i);
            }
        }

        TokenExecutor.Initialize(0);
    }

    // NetworkE.pas:1657
    public virtual void Process() => TokenExecutor?.Process();

    // NetworkE.pas:1694
    // { A player is ready when nothing of his waits: no system events, no token events, and his events
    //   are not being executed }
    public virtual bool PlayerReady(int player)
    {
        if (player < _tokenControls.Count && _tokenControls[player] is not null)
        {
            bool condition = TokenExecutor.SystemEvents.Count != 0 || _tokenControls[player].GetCount() != 0;
            if (!condition)
            {
                TokenControl item = _tokenControls[player];
                condition = TokenExecutor.PrefetchTacticalBattle == item && TokenExecutor.State.Contains(TokenExecutorState.Executing);
            }

            return !condition;
        }
        else
        {
            return false;
        }
    }

    // NetworkE.pas:1663
    public virtual void CreatePlayer(int player)
    {
        if (player >= _tokenControls.Count || _tokenControls[player] is null)
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
        if (player < _tokenControls.Count && _tokenControls[player] is not null)
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
            if (_tokenControls[^1] is not null)
            {
                (_tokenControls[^1] as IDisposable)?.Dispose();
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
        TokenExecutor?.Dispose();
    }

    // NetworkE.pas:1623
    public int GetCount() => _tokenControls.Count;

    // NetworkE.pas:1710
    public TokenControl GetTokenControl(int player) => player < _tokenControls.Count ? _tokenControls[player] : null;

    // NetworkE.pas:1718
    public void TriggerMessage(string message) => OnMessage?.Invoke(this, Msg: message);

    public TokenExecutor TokenExecutor { get; }
    public TokenPackageDistributor TokenPackageDistributor { get; }
    public GameEngine Owner { get; }
    public int Count => GetCount();

    public sealed class TokenControlsAccessor(TokenManager target)
    {
        public TokenControl this[int player] => target.GetTokenControl(player);
    }

    public TokenControlsAccessor TokenControls => new(this);

    public delegate void NetworkMessageEvent(object Sender, string Msg);

}
