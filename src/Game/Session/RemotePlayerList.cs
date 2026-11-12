// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class RemotePlayerList : ENode
{
    public RemotePlayerList(ECustomNode owner) : base(owner)
    { }

    // AoWSetup.pas:279
    public RemotePlayer GetPlayers(int index) => (RemotePlayer)_children.List[index];

    // AoWSetup.pas:284
    public int IndexOfRemoteId(int remoteId)
    {
        int I;
        int result = -1;
        I = 0;
        while (result == -1 && I != _children.Count)
        {
            if (((RemotePlayer)_children.List[I]).RemoteId == remoteId)
            {
                result = I;
            }

            I += 1;
        }

        return result;
    }

}
