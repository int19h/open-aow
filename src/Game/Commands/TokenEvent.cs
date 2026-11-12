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
    public new const int ClassId = ClassIdTokenEvent;

    public TokenEvent(ECustomNode owner) : base(owner)
    {
        _refCount = 1;
        _tokenCounter = -1;
    }

    internal int _refCount;
    internal int _tokenCounter;

    // NetworkE.pas:969
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref _tokenCounter, 10);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32("TokenCounter", ref _tokenCounter);
    }

    // NetworkE.pas:943

    // NetworkE.pas:955
    public override int AddRef()
    {
        _refCount += 1;
        return _refCount;
    }

    // NetworkE.pas:961
    public override int Release()
    {
        _refCount -= 1;
        int result = _refCount;
        if (result == 0)
        {
            Dispose();
        }

        return result;
    }

    // NetworkE.pas:975
    public virtual ExecuteResult Execute(TokenExecutor tokenEvent) => ExecuteResult.Done;

    public int TokenCounter { get => _tokenCounter; set => _tokenCounter = value; }

    private const short ClassIdTokenEvent = 1361;

}
