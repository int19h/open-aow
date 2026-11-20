#nullable disable
// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

// FRefCount, FPlayerCommunicator, FPCSList. The base player communication control: one system,
// no transport
public class PlayerCommunicationControl : IDisposable
{
    internal int RefCount { get; private set; }

    // NetworkE.pas:549
    public virtual int LocalSystemId => 1;

    // NetworkE.pas:532
    public virtual int GetSystemCount() => 1;

    // NetworkE.pas:516
    public virtual bool Activate(PlayerCommunicator pc)
    {
        PlayerCommunicator = pc;
        return true;
    }

    // NetworkE.pas:498
    public virtual PlayerCommunicationSettings CreatePlayerCommunicationSettings() => new(null) { Holder = Creator.CreationHolder() };

    // NetworkE.pas:493
    public virtual string Name() => "";

    // NetworkE.pas:554
    public virtual bool ReceivePackageData(MemoryStream stream) => false;

    // NetworkE.pas:559
    public virtual bool SendPackageData(int player, MemoryStream stream) => false;

    // NetworkE.pas:468
    public PlayerCommunicationControl(EngineObject creator)
    {
        Creator = creator;
        RefCount = 1;
        PlayerCommunicationSettingsList = new PlayerCommunicationSettingsList(null) { Holder = Creator.CreationHolder() };
    }

    // The node that created this control. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    // NetworkE.pas:475
    public virtual void Dispose() => PlayerCommunicationSettingsList?.Dispose();

    // NetworkE.pas:481
    public void AddRef() => RefCount += 1;

    // NetworkE.pas:486
    public void Release()
    {
        RefCount -= 1;
        if (RefCount == 0)
        {
            Dispose();
        }
    }

    // NetworkE.pas:503
    public PlayerCommunicationSettings GetPlayers(int index)
    {
        return index >= 0 && index < PlayerCommunicationSettingsList.Count
            ? PlayerCommunicationSettingsList.Players[index]
            : null;
    }

    // NetworkE.pas:511
    public int GetCount() => PlayerCommunicationSettingsList.Count;

    // NetworkE.pas:522
    public bool Setup(int count)
    {
        PlayerCommunicationSettingsList.Clear();
        for (int i = 0; i <= count - 1; i++)
        {
            CreatePlayerCommunicationSettings().Owner = PlayerCommunicationSettingsList;
        }

        return true;
    }

    // NetworkE.pas:537
    public int IndexOfSystemId(int systemId)
    {
        int result = GetCount();
        while (result != 0)
        {
            result -= 1;
            if (PlayerCommunicationSettingsList.Players[result].SystemId == systemId)
            {
                return result;
            }
        }

        return -1;
    }

    public int Count => GetCount();

    public sealed class PlayersAccessor(PlayerCommunicationControl target)
    {
        public PlayerCommunicationSettings this[int index] => target.GetPlayers(index);
    }

    public PlayersAccessor Players => new(this);
    public PlayerCommunicator PlayerCommunicator { get; private set; }
    public PlayerCommunicationSettingsList PlayerCommunicationSettingsList { get; }
}
