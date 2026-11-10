// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

// inherited message and title
public partial class TutorialMessageEventLog : HelpMessageEventLog
{
    // EventLog.pas:871
    public TutorialMessageEventLog(ECustomNode owner) : base(owner)
    {
        FHelpFileName = AbilityRules.TranslateRStr(GameText.TutorialRStr);
    }

}
