// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Net;

// FSender, FReceiver (player indices, stored with ids 1 and 2), FRefCount. The base of the
// packages that TPlayerCommunicator sends
public partial class DataPackage : EngineObject
{
    internal int FSender;
    internal int FReceiver;
    internal int FRefCount;

    // NetworkE.pas:592
    // { The original does not call the inherited ReadWrite. }
    public override void ReadWrite(EStorageStream Stream)
    {
        Stream.rwInteger(ref FSender, 1);
        Stream.rwInteger(ref FReceiver, 2);
    }

    // NetworkE.pas:598
    public override void MainReadWrite(EStorageStream Stream)
    {
        ReadWrite(Stream);
    }

    // NetworkE.pas:566
    public DataPackage(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
    }

    // NetworkE.pas:572
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // NetworkE.pas:578
    public override int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (Result == 0)
        {
            Dispose();
        }

        return Result;
    }

    // NetworkE.pas:586
    public virtual string Name() => this.GetType().Name;

    public int Sender { get => FSender; set => FSender = value; }
    public int Receiver { get => FReceiver; set => FReceiver = value; }

}
