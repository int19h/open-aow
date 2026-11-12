// Translated from AoWMessageDlg.pas.
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// completion event, retained message event
public partial class MessageDialog : DataModule
{
    public MessageDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label TitleLbl;
    public Panel YesNoPnl;
    public Button YesBtn;
    public Button NoBtn;
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Image Image1;
    public Image Image2;
    public Button OKBtn;
    public Panel OKPnl;
    public Panel OKCancelPnl;
    public Button OKBtn2;
    public Button CancelBtn;
    public Memo MessageMemo;
    public VerticalScrollBar MessageMemoSB;
    public Panel YesNoCancelPnl;
    public Button YesBtn2;
    public Button NoBtn2;
    public Button CancelBtn2;
    public Panel MemoPnl;
    public Panel HeaderMemoPnl;
    public Memo HeaderMemo;
    public VerticalScrollBar HeaderMemoSB;
    public Label HeaderLbl;
    public IvTranslator Translator;
    public ExecuteDoneEvent Done;
    public MessageEventLog Event;

    // AoWMessageDlg.pas:68
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        MessageEventLog log;
        Dlg.Visible = false;
        done = Done;
        log = Event;
        Done = null;
        Event = null;
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
        if (Event.Yes())
        {
            Dlg.Visible = false;
        }
    }

    // AoWMessageDlg.pas:93
    public void NoBtnClick()
    {
        if (Event.No())
        {
            Dlg.Visible = false;
        }
    }

    // AoWMessageDlg.pas:98
    public void OKBtnClick()
    {
        if (Event != null)
        {
            if (!Event.Ok())
            {
                return;
            }
        }

        Dlg.Visible = false;
    }

    // AoWMessageDlg.pas:105
    public void OKBtn2Click()
    {
        if (Event.Ok())
        {
            Dlg.Visible = false;
        }
    }

    // AoWMessageDlg.pas:110
    public void CancelBtnClick()
    {
        if (Event.Cancel())
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowDemoMessage()
    {
        ResetMessageDialog(DialogHosts.MessageDialog);
        DialogHosts.MessageDialog.MessageMemo.Items.Add("");
        var demoVersionMessage = AbilityRules.TranslateResourceString(GameText.s_demoVersionMessageText);
        DialogHosts.MessageDialog.MessageMemo.Items.Add(demoVersionMessage);
        DialogHosts.MessageDialog.OKPnl.Visible = true;
        DialogHosts.MessageDialog.Dlg.Visible = true;
    }

    internal static void ResetMessageDialog(MessageDialog dialog)
    {
        dialog.YesNoPnl.Visible = false;
        dialog.YesNoCancelPnl.Visible = false;
        dialog.OKPnl.Visible = false;
        dialog.OKCancelPnl.Visible = false;
        dialog.MessageMemo.Items.Clear();
        dialog.MemoPnl.Visible = true;
        dialog.HeaderMemoPnl.Visible = false;
        var message = AbilityRules.TranslateResourceString(GameText.s_messageText);
        dialog.TitleLbl.Text = message;
    }

    internal static void ShowMessageEvent(MessageDialog dialog, MessageEventLog eventValue, ExecuteDoneEvent done)
    {
        MessageEventLog log;
        ResetMessageDialog(dialog);
        dialog.Done = done;
        dialog.Event = eventValue;
        dialog.Event.AddRef();
        log = dialog.Event;
        dialog.TitleLbl.Text = log.Title;
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
                dialog.MemoPnl.Visible = false;
                dialog.HeaderMemoPnl.Visible = true;
                dialog.HeaderLbl.Text = log.Header;
                var fMessageText = log.Message.Text;
                dialog.HeaderMemo.Items.Text = fMessageText;
                break;
        }

        switch (log.Buttons)
        {
            case MessageEventLogButtons.Ok:
                dialog.OKPnl.Visible = true;
                break;
            case MessageEventLogButtons.OkCancel:
                dialog.OKCancelPnl.Visible = true;
                break;
            case MessageEventLogButtons.YesNo:
                dialog.YesNoPnl.Visible = true;
                break;
            case MessageEventLogButtons.YesNoCancel:
                dialog.YesNoCancelPnl.Visible = true;
                break;
        }

        dialog.Dlg.Visible = true;
    }

}
