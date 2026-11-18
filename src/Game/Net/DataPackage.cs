// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

namespace Aow.Game.Net;

// FSender, FReceiver (player indices, stored with ids 1 and 2), FRefCount. The base of the
// packages that TPlayerCommunicator sends
public partial class DataPackage(CustomNode owner) : EngineObject(owner)
{
    internal int RefCount { get; private set; } = 1;

    // NetworkE.pas:592
    // { The original does not call the inherited ReadWrite. }
    public override void PersistFields(IPersister persister)
    {
        Sender = persister.Int32("Sender", value: Sender);
        Receiver = persister.Int32("Receiver", value: Receiver);
    }

    // NetworkE.pas:598
    public override void MainPersist(IPersister persister) => PersistFields(persister);

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
    public virtual string Name() => GetType().Name;

    public int Sender { get; set; }
    public int Receiver { get; set; }

}
