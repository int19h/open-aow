// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

// FRefCount, FPlayerCommunicator, FPCSList. The base player communication control: one system,
// no transport
public partial class PlayerCommunicationControl : IDisposable
{
    internal int RefCount { get; private set; }

    private PlayerCommunicator _playerCommunicator;
    private readonly PlayerCommunicationSettingsList _playerCommunicationSettingsList;

    // NetworkE.pas:549
    public virtual int GetLocalSystemId() => 1;

    // NetworkE.pas:532
    public virtual int GetSystemCount() => 1;

    // NetworkE.pas:516
    public virtual bool Activate(PlayerCommunicator pc)
    {
        _playerCommunicator = pc;
        return true;
    }

    // NetworkE.pas:498
    public virtual PlayerCommunicationSettings CreatePlayerCommunicationSettings() => new PlayerCommunicationSettings(null) { Holder = Creator.CreationHolder() };

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
        _playerCommunicationSettingsList = new PlayerCommunicationSettingsList(null) { Holder = Creator.CreationHolder() };
    }

    // The node that created this control. It and the nodes it creates reach the engine through it.
    public EngineObject Creator { get; }

    // NetworkE.pas:475
    public virtual void Dispose()
    {
        _playerCommunicationSettingsList?.Dispose();
    }

    // NetworkE.pas:481
    public void AddRef()
    {
        RefCount += 1;
    }

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
        PlayerCommunicationSettings result;
        if (index >= 0 && index < _playerCommunicationSettingsList.Count)
        {
            result = _playerCommunicationSettingsList.Players[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // NetworkE.pas:511
    public int GetCount() => _playerCommunicationSettingsList.Count;

    // NetworkE.pas:522
    public bool Setup(int count)
    {
        _playerCommunicationSettingsList.Clear();
        for (int I = 0; I <= count - 1; I++)
        {
            CreatePlayerCommunicationSettings().Owner = _playerCommunicationSettingsList;
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
            if (_playerCommunicationSettingsList.Players[result]._systemId == systemId)
            {
                return result;
            }
        }

        return -1;
    }

    public int Count { get => GetCount(); }

    public sealed class PlayersAccessor(PlayerCommunicationControl target)
    {
        public PlayerCommunicationSettings this[int index] { get => target.GetPlayers(index); }
    }

    public PlayersAccessor Players => new PlayersAccessor(this);
    public PlayerCommunicator PlayerCommunicator { get => _playerCommunicator; }
    public PlayerCommunicationSettingsList PlayerCommunicationSettingsList { get => _playerCommunicationSettingsList; }

}
