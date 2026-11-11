// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Net;

namespace Aow.Game.Commands;

// FRefCount, FTokenCounter (stored with id 10. -1 until the executer numbers the event). The
// base of the game's token events
public partial class TokenEvent : EngineObject
{
    // NetworkE.pas:950
    public new const int ClassID = CID_TokenEvent;

    public TokenEvent(ECustomNode owner) : base(owner)
    {
        FRefCount = 1;
        FTokenCounter = -1;
    }

    internal int FRefCount;
    internal int FTokenCounter;

    // NetworkE.pas:969
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FTokenCounter, 10);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int("TokenCounter", ref FTokenCounter);
    }

    // NetworkE.pas:943

    // NetworkE.pas:955
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // NetworkE.pas:961
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

    // NetworkE.pas:975
    public virtual ExecuteResult Execute(TokenExecutor TE) => ExecuteResult.Done;

    public int TokenCounter { get => FTokenCounter; set => FTokenCounter = value; }

    private const short CID_TokenEvent = 1361;

}
