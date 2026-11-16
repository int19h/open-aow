// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class RemotePlayerList : OwningNode, IChildList<RemotePlayer>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a RemotePlayer, so the cast never fails.
    public RemotePlayer this[int index] => GetChild<RemotePlayer>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is RemotePlayer;

    int IChildSink<RemotePlayer>.AddChild(RemotePlayer child) => AddChild(child);

    public RemotePlayerList(CustomNode owner) : base(owner)
    { }

    // AoWSetup.pas:279
    public RemotePlayer GetPlayers(int index) => this[index];

    // AoWSetup.pas:284
    public int IndexOfRemoteId(int remoteId)
    {
        int i;
        int result = -1;
        i = 0;
        while (result == -1 && i != Count)
        {
            if (this[i].RemoteId == remoteId)
            {
                result = i;
            }

            i += 1;
        }

        return result;
    }

}
