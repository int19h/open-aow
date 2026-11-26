// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Session;

public sealed class RemotePlayerList(CustomNode owner) : OwningNode(owner), IChildList<RemotePlayer>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a RemotePlayer, so the cast never fails.
    public RemotePlayer this[int index] => GetChild<RemotePlayer>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is RemotePlayer;

    int IChildSink<RemotePlayer>.AddChild(RemotePlayer child) => AddChild(child);

    // AoWSetup.pas:279
    public RemotePlayer GetPlayers(int index) => this[index];

    // AoWSetup.pas:284
    public int IndexOfRemoteId(int remoteId) => this.FindIndex(remoteId, match: static (sought, player) => player.RemoteId == sought);
}
