// Translated from AoWSetup.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Session;

public partial class RemotePlayerList : ENode
{
    public RemotePlayerList(ECustomNode owner) : base(owner)
    { }

    // AoWSetup.pas:279
    public RemotePlayer GetPlayers(int Index) => (RemotePlayer)FChildren.List[Index];

    // AoWSetup.pas:284
    public int IndexOfRemoteID(int RemoteID)
    {
        int I;
        int Result = -1;
        I = 0;
        while (Result == -1 && I != FChildren.Count)
        {
            if (((RemotePlayer)FChildren.List[I]).FRemoteID == RemoteID)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

}
