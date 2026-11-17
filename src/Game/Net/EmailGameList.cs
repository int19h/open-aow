// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

public partial class EmailGameList : OwningNode, IChildList<EmailGame>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only an EmailGame, so the read never fails.
    public EmailGame this[int index] => GetChild<EmailGame>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is EmailGame;

    int IChildSink<EmailGame>.AddChild(EmailGame child) => AddChild(child);

    public EmailGameList(CustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:2475
    public EmailGame GetEmailGames(int index) => this[index];

    // Diplomacy.pas:2478
    public int IndexOfUniqueLocalId(int id)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (this[result].UniqueLocalId == id)
            {
                return result;
            }
        }

        return -1;
    }

    // Engine.pas:631

}
