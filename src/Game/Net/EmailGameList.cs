// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

public partial class EmailGameList : ENode
{
    public EmailGameList(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:2475
    public EmailGame GetEmailGames(int Index) => (EmailGame)FChildren.List[Index];

    // Diplomacy.pas:2478
    public int IndexOfUniqueLocalID(int ID)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (((EmailGame)FChildren.List[Result]).FUniqueLocalID == ID)
            {
                return Result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
