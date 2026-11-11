// Translated from NetworkE.pas.
using Aow.Rtl;

namespace Aow.Game.Net;

// FTokenExecuter, FTokenPackageDistributer, FOwner, FTokenControls (one per player, nil for a
// free slot), FOnMessage
public partial class TokenManager : IDisposable
{

    public TokenManager(object owner)
    {
        FOwner = owner;
        FTokenControls = new List<TokenControl>();
        FTokenExecuter = CreateTokenExecuter();
        FTokenPackageDistributer = CreateTokenPackageDistributer();
    }

    internal TokenExecutor FTokenExecuter;
    internal TokenPackageDistributor FTokenPackageDistributer;
    internal object FOwner;
    internal List<TokenControl> FTokenControls;
    internal NetworkMessageEvent FOnMessage;

    // NetworkE.pas:1642
    public virtual TokenExecutor CreateTokenExecuter() => new TokenExecutor(this);

    // NetworkE.pas:1647
    public virtual TokenPackageDistributor CreateTokenPackageDistributer() => new TokenPackageDistributor(this);

    // NetworkE.pas:1652
    public virtual TokenControl CreateTokenControl(int APlayer) => new TokenControl(this, APlayer);

    // NetworkE.pas:1628
    public virtual void Initialize(PlayerCommunicator PC)
    {
        FTokenPackageDistributer.SetPC(PC);
        if (PC.FPCC.GetCount() > 0)
        {
            ClearPlayers();
            for (int I = 0, last = PC.FPCC.GetCount() - 1; I <= last; I++)
            {
                CreatePlayer(I);
            }
        }

        FTokenExecuter.Initialize(0);
    }

    // NetworkE.pas:1657
    public virtual void Process()
    {
        if (FTokenExecuter != null)
        {
            FTokenExecuter.Process();
        }
    }

    // NetworkE.pas:1694
    // { A player is ready when nothing of his waits: no system events, no token events, and his events
    //   are not being executed }
    public virtual bool PlayerReady(int APlayer)
    {
        bool Result;
        if (APlayer < FTokenControls.Count && FTokenControls[APlayer] != null)
        {
            bool condition = FTokenExecuter.FSystemEvents.Count != 0 || (FTokenControls[APlayer]).GetCount() != 0;
            if (!condition)
            {
                var item = FTokenControls[APlayer];
                condition = FTokenExecuter.FPrefetchTC == item && FTokenExecuter.FState.Contains(TokenExecutorState.Executing);
            }

            if (condition)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // NetworkE.pas:1663
    public virtual void CreatePlayer(int APlayer)
    {
        if (APlayer >= FTokenControls.Count || FTokenControls[APlayer] == null)
        {
            while (APlayer >= FTokenControls.Count)
            {
                FTokenControls.Add(null);
            }

            var createTokenControl = CreateTokenControl(APlayer);
            FTokenControls[APlayer] = createTokenControl;
        }
    }

    // NetworkE.pas:1673
    public virtual void DeletePlayer(int APlayer)
    {
        if (APlayer < FTokenControls.Count && FTokenControls[APlayer] != null)
        {
            (FTokenControls[APlayer] as IDisposable)?.Dispose();
            FTokenControls[APlayer] = null;
        }
    }

    // NetworkE.pas:1682
    public virtual void ClearPlayers()
    {
        while (FTokenControls.Count != 0)
        {
            if (FTokenControls[FTokenControls.Count - 1] != null)
            {
                (FTokenControls[FTokenControls.Count - 1] as IDisposable)?.Dispose();
            }

            FTokenControls.RemoveAt(FTokenControls.Count - 1);
        }
    }

    // NetworkE.pas:1606
    // { The original does not call the inherited constructor. }

    // NetworkE.pas:1614
    public virtual void Dispose()
    {
        ClearPlayers();
        FTokenControls?.Clear();
        FTokenExecuter?.Dispose();
    }

    // NetworkE.pas:1623
    public int GetCount() => FTokenControls.Count;

    // NetworkE.pas:1710
    public TokenControl GetTokenControl(int APlayer)
    {
        TokenControl Result;
        if (APlayer < FTokenControls.Count)
        {
            Result = FTokenControls[APlayer];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // NetworkE.pas:1718
    public void TriggerMessage(string Msg)
    {
        if (FOnMessage != null)
        {
            FOnMessage(this, Msg);
        }
    }

    public TokenExecutor TokenExecuter { get => FTokenExecuter; }
    public TokenPackageDistributor TokenPackageDistributer { get => FTokenPackageDistributer; }
    public object Owner { get => FOwner; }
    public int Count { get => GetCount(); }

    public sealed class TokenControlsAccessor(TokenManager target)
    {
        public TokenControl this[int APlayer] { get => target.GetTokenControl(APlayer); }
    }

    public TokenControlsAccessor TokenControls => new TokenControlsAccessor(this);
    public NetworkMessageEvent OnMessage { get => FOnMessage; set => FOnMessage = value; }

    public delegate void NetworkMessageEvent(object Sender, string Msg);

}
