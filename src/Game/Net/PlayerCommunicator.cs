#nullable disable
// Translated from NetworkE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Net;

// _engine, Received, _receivedPackage, SendStream, ReceiveStream, _playerCommunicationControl, Processed
public partial class PlayerCommunicator : EngineObject
{
    public PlayerCommunicator(CustomNode owner) : base(owner)
    {
        ReceivedPackage = null;
        OnReceiveStore = new();
        SendStream = new MemoryStream();
        ReceiveStream = new MemoryStream();
        OnProcessStore = new();
    }

    internal EventStore<PlayerCommunicator> OnReceiveStore { get; }

    internal event NotifyEvent<PlayerCommunicator> Received
    {
        add => OnReceiveStore.Add(value);
        remove => OnReceiveStore.Remove(value);
    }

    internal MemoryStream SendStream { get; }
    internal MemoryStream ReceiveStream { get; }

    internal EventStore<PlayerCommunicator> OnProcessStore { get; }

    internal event NotifyEvent<PlayerCommunicator> Processed
    {
        add => OnProcessStore.Add(value);
        remove => OnProcessStore.Remove(value);
    }

    // NetworkE.pas:629
    public virtual bool ActivatePlayerCommunicationControl(PlayerCommunicationControl playerCommunicationControl)
    {
        bool result;
        PlayerCommunicationControl?.Release();
        PlayerCommunicationControl = null;

        PlayerCommunicationControl = playerCommunicationControl;
        if (PlayerCommunicationControl != null)
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
        bool result = default;
        EngineObject objectValue = default;
        ReceiveStream.Clear();
        ReceiveStream.Position = 0;
        if (PlayerCommunicationControl.ReceivePackageData(ReceiveStream))
        {
            try
            {
                objectValue = Engine.RetrieveEngineObject(ReceiveStream, null);
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

    public GameEngine Engine { get; set; }

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
