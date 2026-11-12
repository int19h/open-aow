// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Events;

// player, text, mode, dialog style
public partial class NewTurnEventLog : EventLog
{
    public NewTurnEventLog(ECustomNode owner) : base(owner)
    { }

    public sbyte TurnPlayer;
    public string Text = "";
    public NewTurnEventLogMode Mode;
    public Set<NewTurnEventLogDialogStyleElement> DialogStyle;
}

public enum NewTurnEventLogMode : byte
{
    Dialog = 0, Close = 1
}
