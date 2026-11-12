// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Net;

// FSender, FReceiver (player indices, stored with ids 1 and 2), FRefCount. The base of the
// packages that TPlayerCommunicator sends
public partial class DataPackage : EngineObject
{
    internal int _sender;
    internal int _receiver;

    internal int RefCount { get; private set; }

    // NetworkE.pas:592
    // { The original does not call the inherited ReadWrite. }
    public override void ReadWrite(EStorageStream stream)
    {
        stream.rwInteger(ref _sender, 1);
        stream.rwInteger(ref _receiver, 2);
    }

    // NetworkE.pas:598
    public override void MainReadWrite(EStorageStream stream)
    {
        ReadWrite(stream);
    }

    // NetworkE.pas:566
    public DataPackage(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
    }

    // NetworkE.pas:572
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // NetworkE.pas:578
    public override int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (result == 0)
        {
            Dispose();
        }

        return result;
    }

    // NetworkE.pas:586
    public virtual string Name() => this.GetType().Name;

    public int Sender { get => _sender; set => _sender = value; }
    public int Receiver { get => _receiver; set => _receiver = value; }

}
