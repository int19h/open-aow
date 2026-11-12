// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Events;

// player, text, mode, dialog style
public partial class NewTurnEventLog : EventLog
{
    public NewTurnEventLog(ECustomNode owner) : base(owner)
    { }

    public sbyte TurnPlayer { get; set; }
    public string Text { get; set; } = "";
    public NewTurnEventLogMode Mode { get; set; }
    public Set<NewTurnEventLogDialogStyleElement> DialogStyle { get; set; }
}

public enum NewTurnEventLogMode : byte
{
    Dialog = 0, Close = 1
}
