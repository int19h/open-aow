#nullable disable
// Translated from AoWMessageDlg.pas.
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.UI.Dialogs;

// completion event, retained message event
public partial class MessageDialog : UIModule
{
    public MessageDialog(UIContext ui) : base(ui)
    { }

    public Window Window { get; private set; }
    public Label TitleLabel { get; private set; }
    public Panel YesNoPanel { get; private set; }
    public Panel OKPanel { get; private set; }
    public Panel OKCancelPanel { get; private set; }
    public Memo MessageMemo { get; private set; }
    public Panel YesNoCancelPanel { get; private set; }
    public Panel MemoPanel { get; private set; }
    public Panel HeaderMemoPanel { get; private set; }
    public Memo HeaderMemo { get; private set; }
    public Label HeaderLabel { get; private set; }

    private event ExecuteDoneEvent Done;
    private MessageEventLog _event;

    // AoWMessageDlg.pas:68
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        MessageEventLog log;
        Window.Visible = false;
        done = Done;
        log = _event;
        Done = null;
        _event = null;
        done?.Invoke(log);

        _ = (log?.Release());
    }

    // AoWMessageDlg.pas:83
    public void DlgHide() => FinishDialog();

    // AoWMessageDlg.pas:88
    public void YesBtnClick()
    {
        if (_event.Yes())
        {
            Window.Visible = false;
        }
    }

    // AoWMessageDlg.pas:93
    public void NoBtnClick()
    {
        if (_event.No())
        {
            Window.Visible = false;
        }
    }

    // AoWMessageDlg.pas:98
    public void OKBtnClick()
    {
        if (_event != null)
        {
            if (!_event.Ok())
            {
                return;
            }
        }

        Window.Visible = false;
    }

    // AoWMessageDlg.pas:105
    public void OKBtn2Click()
    {
        if (_event.Ok())
        {
            Window.Visible = false;
        }
    }

    // AoWMessageDlg.pas:110
    public void CancelBtnClick()
    {
        if (_event.Cancel())
        {
            Window.Visible = false;
        }
    }

    internal void ShowDemoMessage()
    {
        ResetMessageDialog(UI.MessageDialog);
        _ = UI.MessageDialog.MessageMemo.Items.Add("");
        string demoVersionMessage = UI.Engine.TranslateResourceString(GameText.DemoVersionMessageText);
        _ = UI.MessageDialog.MessageMemo.Items.Add(demoVersionMessage);
        UI.MessageDialog.OKPanel.Visible = true;
        UI.MessageDialog.Window.Visible = true;
    }

    internal static void ResetMessageDialog(MessageDialog dialog)
    {
        dialog.YesNoPanel.Visible = false;
        dialog.YesNoCancelPanel.Visible = false;
        dialog.OKPanel.Visible = false;
        dialog.OKCancelPanel.Visible = false;
        dialog.MessageMemo.Items.Clear();
        dialog.MemoPanel.Visible = true;
        dialog.HeaderMemoPanel.Visible = false;
        string message = dialog.UI.Engine.TranslateResourceString(GameText.MessageText);
        dialog.TitleLabel.Text = message;
    }

    internal static void ShowMessageEvent(MessageDialog dialog, MessageEventLog eventValue, ExecuteDoneEvent done)
    {
        MessageEventLog log;
        ResetMessageDialog(dialog);
        dialog.Done = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        log = dialog._event;
        dialog.TitleLabel.Text = log.Title;
        switch (log.Layout)
        {
            case MessageEventLogLayout.Normal:
                string text = log.Message.Text;
                dialog.MessageMemo.Items.Text = text;
                if (dialog.MessageMemo.Items.Text.Length < 80)
                {
                    dialog.MessageMemo.Items.Insert(0, "");
                }

                break;
            case MessageEventLogLayout.Header:
                dialog.MemoPanel.Visible = false;
                dialog.HeaderMemoPanel.Visible = true;
                dialog.HeaderLabel.Text = log.Header;
                string fMessageText = log.Message.Text;
                dialog.HeaderMemo.Items.Text = fMessageText;
                break;
        }

        switch (log.Buttons)
        {
            case MessageEventLogButtons.Ok:
                dialog.OKPanel.Visible = true;
                break;
            case MessageEventLogButtons.OkCancel:
                dialog.OKCancelPanel.Visible = true;
                break;
            case MessageEventLogButtons.YesNo:
                dialog.YesNoPanel.Visible = true;
                break;
            case MessageEventLogButtons.YesNoCancel:
                dialog.YesNoCancelPanel.Visible = true;
                break;
        }

        dialog.Window.Visible = true;
    }

}
