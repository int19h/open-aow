#nullable disable warnings
// Translated from NetworkE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Net;

// _engine, Received, _receivedPackage, SendStream, ReceiveStream, _playerCommunicationControl, Processed
public sealed class PlayerCommunicator(CustomNode? owner) : EngineObject(owner)
{

    internal EventStore<PlayerCommunicator> OnReceiveStore { get; } = new();

    internal event NotifyEvent<PlayerCommunicator> Received
    {
        add => OnReceiveStore.Add(value);
        remove => OnReceiveStore.Remove(value);
    }

    internal MemoryStream SendStream { get; } = new MemoryStream();
    internal MemoryStream ReceiveStream { get; } = new MemoryStream();

    internal EventStore<PlayerCommunicator> OnProcessStore { get; } = new();

    internal event NotifyEvent<PlayerCommunicator> Processed
    {
        add => OnProcessStore.Add(value);
        remove => OnProcessStore.Remove(value);
    }

    // NetworkE.pas:629
    public bool ActivatePlayerCommunicationControl(PlayerCommunicationControl playerCommunicationControl)
    {
        PlayerCommunicationControl?.Release();
        PlayerCommunicationControl = null;

        PlayerCommunicationControl = playerCommunicationControl;
        bool result;
        if (PlayerCommunicationControl is not null)
        {
            PlayerCommunicationControl.AddRef();
            result = PlayerCommunicationControl.Activate(this);
            if (!result)
            {
                PlayerCommunicationControl.Release();
                PlayerCommunicationControl = null;
            }
        }
        else
        {
            return true;
        }

        return result;
    }

    // NetworkE.pas:651
    public void Process()
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
        PlayerCommunicationControl?.Release();
        PlayerCommunicationControl = null;

        base.Dispose();
    }

    // NetworkE.pas:658
    public bool ReceiveDataPackage()
    {
        ReceiveStream.Clear();
        ReceiveStream.Position = 0;
        if (PlayerCommunicationControl.ReceivePackageData(ReceiveStream))
        {
            try
            {
                EngineObject? objectValue = Engine.RetrieveEngineObject(ReceiveStream, progress: null);
                if (objectValue is DataPackage received)
                {
                    ReceivedPackage = received;
                    OnReceiveStore.Raise(this);
                    _ = ReceivedPackage.Release();
                }
                else
                {
                    objectValue?.Dispose();
                }
            }
            catch
            {
            }

            ReceivedPackage = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    // NetworkE.pas:685
    public bool SendDataPackage(DataPackage package, int sender, int receiver)
    {
        package.Sender = sender;
        package.Receiver = receiver;
        SendStream.Clear();
        Engine.StoreEngineObject(SendStream, objectValue: package, progress: null);
        SendStream.Position = 0;
        return PlayerCommunicationControl.SendPackageData(receiver, stream: SendStream);
    }

    public GameEngine? Engine { get; init; }

    public event NotifyEvent<PlayerCommunicator> OnReceive
    {
        add => OnReceiveStore.Add(value);
        remove => OnReceiveStore.Remove(value);
    }

    public DataPackage? ReceivedPackage { get; private set; }
    public PlayerCommunicationControl? PlayerCommunicationControl { get; private set; }

    public event NotifyEvent<PlayerCommunicator> OnProcess
    {
        add => OnProcessStore.Add(value);
        remove => OnProcessStore.Remove(value);
    }
}
