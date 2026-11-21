// Translated from EventLog.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Events;

// directory, default name, file name, title
public partial class SaveGameEventLog : EventLog
{
    // EventLog.pas:874
    public SaveGameEventLog(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        Title = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.SaveText);
        Mode = SaveGameEventLogModes.None;
    }

    public SaveGameEventLogModes Mode { get; }
    public string Directory { get; set; } = "";
    public string DefaultName { get; set; } = "";
    public string FileName { get; set; } = "";
    public string Title { get; init; } = "";

}
