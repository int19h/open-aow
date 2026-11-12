// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

public partial class EmailGameList : ENode
{
    public EmailGameList(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:2475
    public EmailGame GetEmailGames(int index) => (EmailGame)_children.List[index];

    // Diplomacy.pas:2478
    public int IndexOfUniqueLocalId(int id)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (((EmailGame)_children.List[result]).UniqueLocalId == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
