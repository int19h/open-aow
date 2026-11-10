// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Events;

// player, text, mode, dialog style
public partial class NewTurnEventLog : EventLog
{
    public NewTurnEventLog(ECustomNode owner) : base(owner)
    { }

    public sbyte FTurnPlayer;
    public string FText = "";
    public Mode FMode;
    public Set<NewTurnEventLogDialogStyleElement> FDialogStyle;

    public enum Mode : byte
    {
        Dialog = 0, Close = 1
    }
}
