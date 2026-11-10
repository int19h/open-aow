// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Events;

// message, help title, mode
public partial class HelpMessageEventLog : EventLog
{
    // EventLog.pas:860
    public HelpMessageEventLog(ECustomNode owner) : base(owner)
    {
        FMessage = new StringList();
        FHelpFileName = AbilityRules.TranslateRStr(HelpRStr);
        FMode = 0;
    }

    public StringList FMessage;
    public string FHelpFileName = "";
    public byte FMode;

    // EventLog.pas:868
    public override void Dispose()
    {
        FMessage?.Dispose();
        base.Dispose();
    }

    private static readonly string HelpRStr = "Help";

}
