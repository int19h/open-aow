#nullable disable warnings
// Translated from NetworkE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Session;
using Aow.Rtl;

namespace Aow.Game.Net;

// _engine, Received, _receivedPackage, SendStream, ReceiveStream, _playerCommunicationControl, Processed
public sealed class PlayerCommunicator(CustomNode? owner) : EngineObject(owner)
{

    internal event NotifyEvent<PlayerCommunicator>? Received;

    internal MemoryStream SendStream { get; } = new MemoryStream();
    internal MemoryStream ReceiveStream { get; } = new MemoryStream();

    internal event NotifyEvent<PlayerCommunicator>? Processed;

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

        NotifyEvent<PlayerCommunicator>? handlers = Processed;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(Processed));
        handlers?.Invoke(this);
    }

    // NetworkE.pas:615
    public override void Dispose()
    {
        Received = null;
        SendStream?.Dispose();
        ReceiveStream?.Dispose();
        Processed = null;
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
                    NotifyEvent<PlayerCommunicator>? handlers = Received;
                    EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(Received));
                    handlers?.Invoke(this);
                    _ = ReceivedPackage.Release();
                }
                else
                {
                    objectValue?.Dispose();
                }
            }
            catch (Exception error)
            {
                HostLog.FallbackAtCallSite(RtlHost.Logger, error: error);
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
    public bool SendDataPackage(DataPackage package, PlayerId? sender, int receiver)
    {
        package.Sender = PlayerIdExtensions.ToOriginal(sender);
        package.Receiver = receiver;
        SendStream.Clear();
        Engine.StoreEngineObject(SendStream, objectValue: package, progress: null);
        SendStream.Position = 0;
        return PlayerCommunicationControl.SendPackageData(receiver, stream: SendStream);
    }

    public GameEngine? Engine { get; init; }

    public DataPackage? ReceivedPackage { get; private set; }
    public PlayerCommunicationControl? PlayerCommunicationControl { get; private set; }
}
