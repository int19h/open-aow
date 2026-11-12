// Translated from AoWMessageDlg.pas.
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// completion event, retained message event
public partial class MessageDialog : UIModule
{
    public MessageDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Label TitleLabel;
    public Panel YesNoPanel;
    public Panel OKPanel;
    public Panel OKCancelPanel;
    public Memo MessageMemo;
    public Panel YesNoCancelPanel;
    public Panel MemoPanel;
    public Panel HeaderMemoPanel;
    public Memo HeaderMemo;
    public Label HeaderLabel;
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
        if (done != null)
        {
            done(log);
        }

        if (log != null)
        {
            log.Release();
        }
    }

    // AoWMessageDlg.pas:83
    public void DlgHide()
    {
        FinishDialog();
    }

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
        UI.MessageDialog.MessageMemo.Items.Add("");
        var demoVersionMessage = AbilityRules.TranslateResourceString(GameText.DemoVersionMessageText);
        UI.MessageDialog.MessageMemo.Items.Add(demoVersionMessage);
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
        var message = AbilityRules.TranslateResourceString(GameText.MessageText);
        dialog.TitleLabel.Text = message;
    }

    internal static void ShowMessageEvent(MessageDialog dialog, MessageEventLog eventValue, ExecuteDoneEvent done)
    {
        MessageEventLog log;
        ResetMessageDialog(dialog);
        dialog.Done = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        log = dialog._event;
        dialog.TitleLabel.Text = log.Title;
        switch (log.Layout)
        {
            case MessageEventLogLayout.Normal:
                var text = log.Message.Text;
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
                var fMessageText = log.Message.Text;
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
