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
        Title = AbilityRules.TranslateResourceString(GameText.s_saveText);
        Mode = Set<SaveGameEventLogModeElement>.Of();
    }

    public Set<SaveGameEventLogModeElement> Mode;
    public string Directory = "";
    public string DefaultName = "";
    public string FileName = "";
    public string Title = "";

}
