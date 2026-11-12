// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Net;

namespace Aow.Game.Commands;

// FRefCount, FTokenCounter (stored with id 10. -1 until the executer numbers the event). The
// base of the game's token events
public partial class TokenEvent : EngineObject, IOwned<TokenEventReferenceList>, IOwnedSink<TokenEventReferenceList>
{
    // A reference list holds the token event and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the cast never fails.
    public new TokenEventReferenceList Owner { get => (TokenEventReferenceList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is TokenEventReferenceList;

    void IOwnedSink<TokenEventReferenceList>.SetOwner(TokenEventReferenceList owner) => SetOwner(owner);

    // NetworkE.pas:950
    public new const int ClassId = ClassIdTokenEvent;

    public TokenEvent(ECustomNode owner) : base(owner)
    {
        RefCount = 1;
        _tokenCounter = -1;
    }

    internal int RefCount { get; private set; }

    internal int _tokenCounter;

    // NetworkE.pas:969
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref _tokenCounter, 10);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        _tokenCounter = persister.Int32("TokenCounter", _tokenCounter);
    }

    // NetworkE.pas:943

    // NetworkE.pas:955
    public override int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // NetworkE.pas:961
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

    // NetworkE.pas:975
    public virtual ExecuteResult Execute(TokenExecutor tokenEvent) => ExecuteResult.Done;

    public int TokenCounter { get => _tokenCounter; set => _tokenCounter = value; }

    private const short ClassIdTokenEvent = 1361;

}
