// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Events;

// message, help title, mode
public partial class HelpMessageEventLog : EventLog
{
    // EventLog.pas:860
    public HelpMessageEventLog(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Message = [];
        HelpFileName = AbilityRules.TranslateResourceString(GetEngine(), S: s_helpText);
        Mode = 0;
    }

    public DelphiStringList Message { get; }
    public string HelpFileName { get; init; } = "";
    public int Mode { get; }

    // EventLog.pas:868
    public override void Dispose() => base.Dispose();

    private static readonly string s_helpText = "Help";

}
