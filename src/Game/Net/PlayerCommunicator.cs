// Translated from NetworkE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Net;

// _engine, FOnReceive, _receivedPackage, SendStream, ReceiveStream, _playerCommunicationControl, FOnProcess
public partial class PlayerCommunicator : EngineObject
{
    private GameEngine _engine;

    internal EventStore<PlayerCommunicator> OnReceiveStore { get; }

    internal event NotifyEvent<PlayerCommunicator> FOnReceive
    {
        add => OnReceiveStore.Add(value);
        remove => OnReceiveStore.Remove(value);
    }

    private DataPackage _receivedPackage;

    internal MemoryStream SendStream { get; }
    internal MemoryStream ReceiveStream { get; }

    private PlayerCommunicationControl _playerCommunicationControl;

    internal EventStore<PlayerCommunicator> OnProcessStore { get; }

    internal event NotifyEvent<PlayerCommunicator> FOnProcess
    {
        add => OnProcessStore.Add(value);
        remove => OnProcessStore.Remove(value);
    }

    // NetworkE.pas:605
    public PlayerCommunicator(ECustomNode owner) : base(owner)
    {
        _receivedPackage = null;
        OnReceiveStore = new();
        SendStream = new MemoryStream();
        ReceiveStream = new MemoryStream();
        OnProcessStore = new();
    }

    // NetworkE.pas:629
    public virtual bool ActivatePlayerCommunicationControl(PlayerCommunicationControl playerCommunicationControl)
    {
        bool result;
        if (_playerCommunicationControl != null)
        {
            _playerCommunicationControl.Release();
            _playerCommunicationControl = null;
        }

        _playerCommunicationControl = playerCommunicationControl;
        if (_playerCommunicationControl != null)
        {
            _playerCommunicationControl.AddRef();
            result = _playerCommunicationControl.Activate(this);
            if (!result)
            {
                _playerCommunicationControl.Release();
                _playerCommunicationControl = null;
            }
        }
        else
        {
            result = true;
        }

        return result;
    }

    // NetworkE.pas:651
    public virtual void Process()
    {
        while (ReceiveDataPackage())
        {
        }

        OnProcessStore.Raise(this);
    }

    // NetworkE.pas:615
    public override void Dispose()
    {
        OnReceiveStore.Clear();
        SendStream?.Dispose();
        ReceiveStream?.Dispose();
        OnProcessStore.Clear();
        if (_playerCommunicationControl != null)
        {
            _playerCommunicationControl.Release();
            _playerCommunicationControl = null;
        }

        base.Dispose();
    }

    // NetworkE.pas:658
    public bool ReceiveDataPackage()
    {
        bool result = default;
        EngineObject objectValue = default;
        ReceiveStream.Clear();
        ReceiveStream.Position = 0;
        if (_playerCommunicationControl.ReceivePackageData(ReceiveStream))
        {
            try
            {
                objectValue = _engine.RetrieveEngineObject(ReceiveStream, null);
                if (objectValue is DataPackage)
                {
                    _receivedPackage = (DataPackage)objectValue;
                    OnReceiveStore.Raise(this);
                    _receivedPackage.Release();
                }
                else
                {
                    objectValue?.Dispose();
                }
            }
            catch
            {
            }

            _receivedPackage = null;
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // NetworkE.pas:685
    public bool SendDataPackage(DataPackage package, int sender, int receiver)
    {
        package._sender = sender;
        package._receiver = receiver;
        SendStream.Clear();
        _engine.StoreEngineObject(SendStream, package, null);
        SendStream.Position = 0;
        return _playerCommunicationControl.SendPackageData(receiver, SendStream);
    }

    public GameEngine Engine { get => _engine; set => _engine = value; }

    public event NotifyEvent<PlayerCommunicator> OnReceive
    {
        add => OnReceiveStore.Add(value);
        remove => OnReceiveStore.Remove(value);
    }

    public DataPackage ReceivedPackage { get => _receivedPackage; }
    public PlayerCommunicationControl PlayerCommunicationControl { get => _playerCommunicationControl; }

    public event NotifyEvent<PlayerCommunicator> OnProcess
    {
        add => OnProcessStore.Add(value);
        remove => OnProcessStore.Remove(value);
    }

}
