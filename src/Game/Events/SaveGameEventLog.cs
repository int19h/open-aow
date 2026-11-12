// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Events;

// directory, default name, file name, title
public partial class SaveGameEventLog : EventLog
{
    // EventLog.pas:874
    public SaveGameEventLog(ECustomNode owner) : base(owner)
    {
        Title = AbilityRules.TranslateResourceString(GameText.SaveText);
        Mode = Set<SaveGameEventLogModeElement>.Of();
    }

    public Set<SaveGameEventLogModeElement> Mode { get; }
    public string Directory { get; set; } = "";
    public string DefaultName { get; set; } = "";
    public string FileName { get; set; } = "";
    public string Title { get; set; } = "";

}
