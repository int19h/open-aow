// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Session;

namespace Aow.Game.Events;

// player, text, mode, dialog style
public partial class NewTurnEventLog(CustomNode owner, EngineObject holder) : EventLog(owner, holder: holder)
{
    public PlayerId? TurnPlayer { get; init; } = PlayerId.Independent;
    public string Text { get; set; } = "";
    public NewTurnEventLogMode Mode { get; init; }
    public NewTurnEventLogDialogStyles DialogStyle { get; init; }
}

public enum NewTurnEventLogMode : byte
{
    Dialog = 0, Close = 1
}
