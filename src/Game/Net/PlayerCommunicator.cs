// Translated from NetworkE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Net;

// _engine, FOnReceive, _receivedPackage, _sendStream, _receiveStream, _playerCommunicationControl, FOnProcess
public partial class PlayerCommunicator : EngineObject
{
    internal GameEngine _engine;

    internal EventStore<PlayerCommunicator> _onReceiveStore;

    internal event NotifyEvent<PlayerCommunicator> FOnReceive
    {
        add => _onReceiveStore.Add(value);
        remove => _onReceiveStore.Remove(value);
    }

    internal DataPackage _receivedPackage;
    internal MemoryStream _sendStream;
    internal MemoryStream _receiveStream;
    internal PlayerCommunicationControl _playerCommunicationControl;

    internal EventStore<PlayerCommunicator> _onProcessStore;

    internal event NotifyEvent<PlayerCommunicator> FOnProcess
    {
        add => _onProcessStore.Add(value);
        remove => _onProcessStore.Remove(value);
    }

    // NetworkE.pas:605
    public PlayerCommunicator(ECustomNode owner) : base(owner)
    {
        _receivedPackage = null;
        _onReceiveStore = new();
        _sendStream = new MemoryStream();
        _receiveStream = new MemoryStream();
        _onProcessStore = new();
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

        _onProcessStore.Raise(this);
    }

    // NetworkE.pas:615
    public override void Dispose()
    {
        _onReceiveStore.Clear();
        _sendStream?.Dispose();
        _receiveStream?.Dispose();
        _onProcessStore.Clear();
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
        _receiveStream.Clear();
        _receiveStream.Position = 0;
        if (_playerCommunicationControl.ReceivePackageData(_receiveStream))
        {
            try
            {
                objectValue = _engine.RetrieveEngineObject(_receiveStream, null);
                if (objectValue is DataPackage)
                {
                    _receivedPackage = (DataPackage)objectValue;
                    _onReceiveStore.Raise(this);
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
        _sendStream.Clear();
        _engine.StoreEngineObject(_sendStream, package, null);
        _sendStream.Position = 0;
        return _playerCommunicationControl.SendPackageData(receiver, _sendStream);
    }

    public GameEngine Engine { get => _engine; set => _engine = value; }

    public event NotifyEvent<PlayerCommunicator> OnReceive
    {
        add => _onReceiveStore.Add(value);
        remove => _onReceiveStore.Remove(value);
    }

    public DataPackage ReceivedPackage { get => _receivedPackage; }
    public PlayerCommunicationControl PlayerCommunicationControl { get => _playerCommunicationControl; }

    public event NotifyEvent<PlayerCommunicator> OnProcess
    {
        add => _onProcessStore.Add(value);
        remove => _onProcessStore.Remove(value);
    }

}
