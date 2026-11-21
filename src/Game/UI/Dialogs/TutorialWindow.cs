#nullable disable warnings
// Translated from AoWTutorialWin.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.UI.Dialogs;

// done callback, retained event
public sealed partial class TutorialWindow(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Memo MessageMemo { get; private set; }
    public Memo HeaderMemo { get; private set; }
    public VerticalScrollBar AbilityScrollBar { get; private set; }

    private ExecuteDoneEvent? _done;
    private EventLog? _event;

    // AoWTutorialWin.pas:51
    public void FinishDialog()
    {
        ExecuteDoneEvent? done = _done;
        EventLog? log = _event;
        _done = null;
        _event = null;
        done?.Invoke(log);

        _ = (log?.Release());
    }

    // AoWTutorialWin.pas:38
    public void DlgHide() => FinishDialog();

    // AoWTutorialWin.pas:44
    public void CloseClick() => Window.Visible = false;

    internal static void ShowTutorialMessage(TutorialWindow dialog)
    {
        if (dialog.MessageMemo.Items.Count > 0)
        {
            dialog.Window.Visible = true;
        }
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi signature keeps unused on purpose.")]
    internal static void ShowTutorialEvent(TutorialWindow dialog, TutorialMessageEventLog eventValue, bool unused, ExecuteDoneEvent done)
    {
        dialog.AbilityScrollBar.Position = 0;
        dialog.FinishDialog();
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        dialog._done = done;
        dialog.HeaderMemo.Items.Clear();
        if (eventValue.Message.Count > 0)
        {
            string message = eventValue.Message[0];
            _ = dialog.HeaderMemo.Items.Add(message);
        }

        dialog.MessageMemo.Items.Clear();
        dialog.MessageMemo.Items.AddStrings(eventValue.Message);
        if (dialog.MessageMemo.Items.Count > 0)
        {
            dialog.MessageMemo.Items.Delete(0);
        }

        if (dialog.MessageMemo.Items.Count > 0 && IsEmpty(dialog.MessageMemo.Items[0]))
        {
            dialog.MessageMemo.Items.Delete(0);
        }

        dialog.Window.Visible = true;
    }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi signature keeps unused on purpose.")]
    internal static void ShowFlagMessage(TutorialWindow dialog, FlagMessageEventLog eventValue, bool unused, ExecuteDoneEvent done)
    {
        dialog.AbilityScrollBar.Position = 0;
        dialog.FinishDialog();
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        dialog._done = done;
        dialog.HeaderMemo.Items.Clear();
        _ = dialog.HeaderMemo.Items.Add(eventValue.HelpFileName);
        dialog.MessageMemo.Items.Clear();
        dialog.MessageMemo.Items.AddStrings(eventValue.Message);
        dialog.Window.Visible = true;
    }
}
