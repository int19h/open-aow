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
        FTitle = AbilityRules.TranslateRStr(GameText.SaveRStr);
        FMode = Set<SaveGameEventLogModeElement>.Of();
    }

    public Set<SaveGameEventLogModeElement> FMode;
    public string FDirectory = "";
    public string FDefaultName = "";
    public string FFileName = "";
    public string FTitle = "";

}
