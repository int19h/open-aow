// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class RemotePlayerList : ENode, IChildList<RemotePlayer>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a RemotePlayer, so the cast never fails.
    public RemotePlayer this[int index] => (RemotePlayer)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is RemotePlayer;

    int IChildSink<RemotePlayer>.AddChild(RemotePlayer child) => AddChild(child);

    public RemotePlayerList(ECustomNode owner) : base(owner)
    { }

    // AoWSetup.pas:279
    public RemotePlayer GetPlayers(int index) => this[index];

    // AoWSetup.pas:284
    public int IndexOfRemoteId(int remoteId)
    {
        int I;
        int result = -1;
        I = 0;
        while (result == -1 && I != Count)
        {
            if (this[I].RemoteId == remoteId)
            {
                result = I;
            }

            I += 1;
        }

        return result;
    }

}
