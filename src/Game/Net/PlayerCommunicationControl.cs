// Translated from NetworkE.pas.

namespace Aow.Game.Net;

// FRefCount, FPlayerCommunicator, FPCSList. The base player communication control: one system,
// no transport
public partial class PlayerCommunicationControl : IDisposable
{
    internal int FRefCount;
    internal PlayerCommunicator FPlayerCommunicator;
    internal PlayerCommunicationSettingsList FPCSList;

    // NetworkE.pas:549
    public virtual int GetLocalSystemID() => 1;

    // NetworkE.pas:532
    public virtual int GetSystemCount() => 1;

    // NetworkE.pas:516
    public virtual bool Activate(PlayerCommunicator PC)
    {
        FPlayerCommunicator = PC;
        return true;
    }

    // NetworkE.pas:498
    public virtual PlayerCommunicationSettings CreatePCS() => new PlayerCommunicationSettings(null);

    // NetworkE.pas:493
    public virtual string Name() => "";

    // NetworkE.pas:554
    public virtual bool ReceivePackageData(MemoryStream Stream) => false;

    // NetworkE.pas:559
    public virtual bool SendPackageData(int APlayer, MemoryStream Stream) => false;

    // NetworkE.pas:468
    public PlayerCommunicationControl()
    {
        FRefCount = 1;
        FPCSList = new PlayerCommunicationSettingsList(null);
    }

    // NetworkE.pas:475
    public virtual void Dispose()
    {
        FPCSList?.Dispose();
    }

    // NetworkE.pas:481
    public void AddRef()
    {
        FRefCount += 1;
    }

    // NetworkE.pas:486
    public void Release()
    {
        FRefCount -= 1;
        if (FRefCount == 0)
        {
            Dispose();
        }
    }

    // NetworkE.pas:503
    public PlayerCommunicationSettings GetPlayers(int Index)
    {
        PlayerCommunicationSettings Result;
        if (Index >= 0 && Index < FPCSList.Count)
        {
            Result = FPCSList.Players[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // NetworkE.pas:511
    public int GetCount() => FPCSList.Count;

    // NetworkE.pas:522
    public bool Setup(int ACount)
    {
        FPCSList.Clear();
        for (int I = 0; I <= ACount - 1; I++)
        {
            CreatePCS().Owner = FPCSList;
        }

        return true;
    }

    // NetworkE.pas:537
    public int IndexOfSystemID(int ASystemID)
    {
        int Result = GetCount();
        while (Result != 0)
        {
            Result -= 1;
            if (FPCSList.Players[Result].FSystemID == ASystemID)
            {
                return Result;
            }
        }

        return -1;
    }

    public int Count { get => GetCount(); }

    public sealed class PlayersAccessor(PlayerCommunicationControl target)
    {
        public PlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
    }

    public PlayersAccessor Players => new PlayersAccessor(this);
    public PlayerCommunicator PlayerCommunicator { get => FPlayerCommunicator; }
    public PlayerCommunicationSettingsList PCSList { get => FPCSList; }

}
