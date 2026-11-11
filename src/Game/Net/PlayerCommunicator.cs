// Translated from NetworkE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Net;

// FEngine, FOnReceive, FReceivedPackage, FSendStream, FReceiveStream, FPCC, FOnProcess
public partial class PlayerCommunicator : EngineObject
{
    internal GameEngine FEngine;

    internal EventStore<PlayerCommunicator> FOnReceiveStore;

    internal event NotifyEvent<PlayerCommunicator> FOnReceive
    {
        add => FOnReceiveStore.Add(value);
        remove => FOnReceiveStore.Remove(value);
    }

    internal DataPackage FReceivedPackage;
    internal MemoryStream FSendStream;
    internal MemoryStream FReceiveStream;
    internal PlayerCommunicationControl FPCC;

    internal EventStore<PlayerCommunicator> FOnProcessStore;

    internal event NotifyEvent<PlayerCommunicator> FOnProcess
    {
        add => FOnProcessStore.Add(value);
        remove => FOnProcessStore.Remove(value);
    }

    // NetworkE.pas:605
    public PlayerCommunicator(ECustomNode owner) : base(owner)
    {
        FReceivedPackage = null;
        FOnReceiveStore = new();
        FSendStream = new MemoryStream();
        FReceiveStream = new MemoryStream();
        FOnProcessStore = new();
    }

    // NetworkE.pas:629
    public virtual bool ActivatePCC(PlayerCommunicationControl PCC)
    {
        bool Result;
        if (FPCC != null)
        {
            FPCC.Release();
            FPCC = null;
        }

        FPCC = PCC;
        if (FPCC != null)
        {
            FPCC.AddRef();
            Result = FPCC.Activate(this);
            if (!Result)
            {
                FPCC.Release();
                FPCC = null;
            }
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // NetworkE.pas:651
    public virtual void Process()
    {
        while (ReceiveDataPackage())
        {
        }

        FOnProcessStore.Raise(this);
    }

    // NetworkE.pas:615
    public override void Dispose()
    {
        FOnReceiveStore.Clear();
        FSendStream?.Dispose();
        FReceiveStream?.Dispose();
        FOnProcessStore.Clear();
        if (FPCC != null)
        {
            FPCC.Release();
            FPCC = null;
        }

        base.Dispose();
    }

    // NetworkE.pas:658
    public bool ReceiveDataPackage()
    {
        bool Result = default;
        EngineObject Obj = default;
        FReceiveStream.Clear();
        FReceiveStream.Position = 0;
        if (FPCC.ReceivePackageData(FReceiveStream))
        {
            try
            {
                Obj = FEngine.RetrieveEObject(FReceiveStream, null);
                if (Obj is DataPackage)
                {
                    FReceivedPackage = (DataPackage)Obj;
                    FOnReceiveStore.Raise(this);
                    FReceivedPackage.Release();
                }
                else
                {
                    Obj?.Dispose();
                }
            }
            catch
            {
            }

            FReceivedPackage = null;
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // NetworkE.pas:685
    public bool SendDataPackage(DataPackage Package, int ASender, int AReceiver)
    {
        Package.FSender = ASender;
        Package.FReceiver = AReceiver;
        FSendStream.Clear();
        FEngine.StoreEObject(FSendStream, Package, null);
        FSendStream.Position = 0;
        return FPCC.SendPackageData(AReceiver, FSendStream);
    }

    public GameEngine Engine { get => FEngine; set => FEngine = value; }

    public event NotifyEvent<PlayerCommunicator> OnReceive
    {
        add => FOnReceiveStore.Add(value);
        remove => FOnReceiveStore.Remove(value);
    }

    public DataPackage ReceivedPackage { get => FReceivedPackage; }
    public PlayerCommunicationControl PCC { get => FPCC; }

    public event NotifyEvent<PlayerCommunicator> OnProcess
    {
        add => FOnProcessStore.Add(value);
        remove => FOnProcessStore.Remove(value);
    }

}
