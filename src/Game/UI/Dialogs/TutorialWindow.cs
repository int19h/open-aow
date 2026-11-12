// Translated from AoWTutorialWin.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// done callback, retained event
public partial class TutorialWindow : UIModule
{
    public TutorialWindow(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Memo MessageMemo;
    public Memo HeaderMemo;
    public VerticalScrollBar AbilityScrollBar;
    private event ExecuteDoneEvent Done;
    private EventLog _event;

    // AoWTutorialWin.pas:51
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        EventLog log;
        done = Done;
        log = _event;
        Done = null;
        _event = null;
        if (done != null)
        {
            done(log);
        }

        if (log != null)
        {
            log.Release();
        }
    }

    // AoWTutorialWin.pas:38
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWTutorialWin.pas:44
    public void CloseClick()
    {
        Window.Visible = false;
    }

    internal static void ShowTutorialMessage(TutorialWindow dialog)
    {
        if (dialog.MessageMemo.Items.Count > 0)
        {
            dialog.Window.Visible = true;
        }
    }

    internal static void ShowTutorialEvent(TutorialWindow dialog, TutorialMessageEventLog eventValue, bool unused, ExecuteDoneEvent done)
    {
        dialog.AbilityScrollBar.Position = 0;
        dialog.FinishDialog();
        dialog._event = eventValue;
        dialog._event.AddRef();
        dialog.Done = done;
        dialog.HeaderMemo.Items.Clear();
        if (eventValue.Message.Count > 0)
        {
            var message = eventValue.Message[0];
            dialog.HeaderMemo.Items.Add(message);
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

    internal static void ShowFlagMessage(TutorialWindow dialog, FlagMessageEventLog eventValue, bool unused, ExecuteDoneEvent done)
    {
        dialog.AbilityScrollBar.Position = 0;
        dialog.FinishDialog();
        dialog._event = eventValue;
        dialog._event.AddRef();
        dialog.Done = done;
        dialog.HeaderMemo.Items.Clear();
        dialog.HeaderMemo.Items.Add(eventValue.HelpFileName);
        dialog.MessageMemo.Items.Clear();
        dialog.MessageMemo.Items.AddStrings(eventValue.Message);
        dialog.Window.Visible = true;
    }

}
