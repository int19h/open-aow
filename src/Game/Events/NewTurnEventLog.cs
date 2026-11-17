// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Events;

// player, text, mode, dialog style
public partial class NewTurnEventLog(CustomNode owner) : EventLog(owner)
{
    public sbyte TurnPlayer { get; init; }
    public string Text { get; set; } = "";
    public NewTurnEventLogMode Mode { get; init; }
    public Set<NewTurnEventLogDialogStyleElement> DialogStyle { get; init; }
}

public enum NewTurnEventLogMode : byte
{
    Dialog = 0, Close = 1
}
