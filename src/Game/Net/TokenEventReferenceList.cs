using Aow.Engine.Nodes;

using Aow.Game.Commands;

namespace Aow.Game.Net;

// The token events of a TokenEventList (its FList). The original stores a plain TEReferenceNode
// there, and this class registers under the same class id, so the stored bytes are the same.
public class TokenEventReferenceList(ECustomNode owner) : EReferenceNode(owner), IChildList<TokenEvent>
{
    // The token event in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a token event, so the cast never fails.
    public TokenEvent this[int index] => (TokenEvent)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is TokenEvent;

    int IChildSink<TokenEvent>.AddChild(TokenEvent child) => AddChild(child);
}
