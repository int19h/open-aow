
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Session;

// The default message being written (Diplomacy.pas:1420): the text control, the source's
// alignment row and the plural race names of source, target and other player.
public sealed class DiplomaticMessageText(TextControl textControl, int alignment, string sourceRace, string targetRace, string otherRace, StringCollection text)
{
    // Text Number of the alignment row, the source's races first.
    public void Proposing(int Number, bool AboutOther = false)
    {
        textControl.RetrieveText(alignment + Number, text);
        text.Text = AboutOther
            ? AbilityRules.Format(text.Text, [sourceRace, targetRace, otherRace])
            : AbilityRules.Format(text.Text, [sourceRace, targetRace]);
    }

    // Text Number of the alignment row, the target's races first.
    public void Replying(int Number, bool AboutOther = false)
    {
        textControl.RetrieveText(alignment + Number, text);
        text.Text = AboutOther
            ? AbilityRules.Format(text.Text, [targetRace, sourceRace, otherRace])
            : AbilityRules.Format(text.Text, [targetRace, sourceRace]);
    }

    // The proposal's text while unanswered, the reply's once accepted or rejected, none once ignored.
    public void Exchange(DiplomaticActionReaction Reaction, int Proposed, int Accepted, int Rejected, bool AboutOther = false)
    {
        if (Reaction == DiplomaticActionReaction.None)
        {
            Proposing(Proposed, AboutOther);
        }
        else if (Reaction == DiplomaticActionReaction.Accept)
        {
            Replying(Accepted, AboutOther);
        }
        else if (Reaction == DiplomaticActionReaction.Reject)
        {
            Replying(Rejected, AboutOther);
        }
    }
}
