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
        Message = new StringList();
        HelpFileName = AbilityRules.TranslateResourceString(s_helpText);
        Mode = 0;
    }

    public StringList Message { get; }
    public string HelpFileName { get; set; } = "";
    public byte Mode { get; }

    // EventLog.pas:868
    public override void Dispose()
    {
        Message?.Dispose();
        base.Dispose();
    }

    private static readonly string s_helpText = "Help";

}
