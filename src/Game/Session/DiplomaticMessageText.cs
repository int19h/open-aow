
using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Session;

// The default message being written (Diplomacy.pas:1420): the text control, the source's
// alignment row and the plural race names of source, target and other player.
public sealed class DiplomaticMessageText(TextControl textControl, int alignment, string sourceRace, string targetRace, string otherRace, StringCollection text)
{
    // Text Number of the alignment row, the source's races first.
    public void Proposing(int number, bool aboutOther = false)
    {
        textControl.RetrieveText(alignment + number, text);
        text.Text = aboutOther
            ? AbilityRules.Format(text.Text, [sourceRace, targetRace, otherRace])
            : AbilityRules.Format(text.Text, [sourceRace, targetRace]);
    }

    // Text Number of the alignment row, the target's races first.
    public void Replying(int number, bool aboutOther = false)
    {
        textControl.RetrieveText(alignment + number, text);
        text.Text = aboutOther
            ? AbilityRules.Format(text.Text, [targetRace, sourceRace, otherRace])
            : AbilityRules.Format(text.Text, [targetRace, sourceRace]);
    }

    // The proposal's text while unanswered, the reply's once accepted or rejected, none once ignored.
    public void Exchange(DiplomaticActionReaction reaction, int proposed, int accepted, int rejected, bool aboutOther = false)
    {
        if (reaction == DiplomaticActionReaction.None)
        {
            Proposing(proposed, aboutOther);
        }
        else if (reaction == DiplomaticActionReaction.Accept)
        {
            Replying(accepted, aboutOther);
        }
        else if (reaction == DiplomaticActionReaction.Reject)
        {
            Replying(rejected, aboutOther);
        }
    }
}
