#nullable disable
// Translated from NetworkE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Net;

// _engine, Received, _receivedPackage, SendStream, ReceiveStream, _playerCommunicationControl, Processed
public partial class PlayerCommunicator(CustomNode owner) : EngineObject(owner)
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
    public virtual bool ActivatePlayerCommunicationControl(PlayerCommunicationControl playerCommunicationControl)
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
        PlayerCommunicationControl?.Release();
        PlayerCommunicationControl = null;

        base.Dispose();
    }

    // NetworkE.pas:658
    public bool ReceiveDataPackage()
    {
        ReceiveStream.Clear();
        ReceiveStream.Position = 0;
        bool result;
        if (PlayerCommunicationControl.ReceivePackageData(ReceiveStream))
        {
            try
            {
                EngineObject objectValue = Engine.RetrieveEngineObject(ReceiveStream, null);
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
        package.Sender = sender;
        package.Receiver = receiver;
        SendStream.Clear();
        Engine.StoreEngineObject(SendStream, package, null);
        SendStream.Position = 0;
        return PlayerCommunicationControl.SendPackageData(receiver, SendStream);
    }

    public GameEngine Engine { get; init; }

    public event NotifyEvent<PlayerCommunicator> OnReceive
    {
        add => OnReceiveStore.Add(value);
        remove => OnReceiveStore.Remove(value);
    }

    public DataPackage ReceivedPackage { get; private set; }
    public PlayerCommunicationControl PlayerCommunicationControl { get; private set; }

    public event NotifyEvent<PlayerCommunicator> OnProcess
    {
        add => OnProcessStore.Add(value);
        remove => OnProcessStore.Remove(value);
    }

}
