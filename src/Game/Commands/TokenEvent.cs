// Translated from NetworkE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;

namespace Aow.Game.Commands;

// FRefCount, FTokenCounter (stored with id 10. -1 until the executer numbers the event). The
// base of the game's token events
public partial class TokenEvent(CustomNode owner) : EngineObject(owner), IOwned<TokenEventReferenceList>, IOwnedSink<TokenEventReferenceList>
{
    // A reference list holds the token event and owns it only while the list saves or loads it, and
    // nothing else owns it. AcceptsOwner admits only that list, so the read never fails.
    public new TokenEventReferenceList Owner { get => GetOwner<TokenEventReferenceList>(); set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is TokenEventReferenceList;

    void IOwnedSink<TokenEventReferenceList>.SetOwner(TokenEventReferenceList owner) => SetOwner(owner);

    // NetworkE.pas:950
    public new const int ClassId = ClassIdTokenEvent;

    internal int RefCount { get; private set; } = 1;

    // NetworkE.pas:969
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        TokenCounter = persister.Int32("TokenCounter", value: TokenCounter);
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

    public virtual bool Validate() => true;

    public int TokenCounter { get; set; } = -1;

    private const short ClassIdTokenEvent = 1361;

}
