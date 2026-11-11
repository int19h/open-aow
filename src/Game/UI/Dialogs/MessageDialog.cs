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
    public Image AOWImage1;
    public Image AOWImage2;
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
    public ExecuteDoneEvent FDone;
    public MessageEventLog FEvent;

    // AoWMessageDlg.pas:68
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        MessageEventLog Log;
        Dlg.Visible = false;
        Done = FDone;
        Log = FEvent;
        FDone = null;
        FEvent = null;
        if (Done != null)
        {
            Done(Log);
        }

        if (Log != null)
        {
            Log.Release();
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
        if (FEvent.Yes())
        {
            Dlg.Visible = false;
        }
    }

    // AoWMessageDlg.pas:93
    public void NoBtnClick()
    {
        if (FEvent.No())
        {
            Dlg.Visible = false;
        }
    }

    // AoWMessageDlg.pas:98
    public void OKBtnClick()
    {
        if (FEvent != null)
        {
            if (!FEvent.OK())
            {
                return;
            }
        }

        Dlg.Visible = false;
    }

    // AoWMessageDlg.pas:105
    public void OKBtn2Click()
    {
        if (FEvent.OK())
        {
            Dlg.Visible = false;
        }
    }

    // AoWMessageDlg.pas:110
    public void CancelBtnClick()
    {
        if (FEvent.Cancel())
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowDemoMessage()
    {
        ResetMessageDialog(DialogHosts.MessageDlg);
        DialogHosts.MessageDlg.MessageMemo.Items.Add("");
        var demoVersionMessage = AbilityRules.TranslateRStr(GameText.DemoVersionMessageRStr);
        DialogHosts.MessageDlg.MessageMemo.Items.Add(demoVersionMessage);
        DialogHosts.MessageDlg.OKPnl.Visible = true;
        DialogHosts.MessageDlg.Dlg.Visible = true;
    }

    internal static void ResetMessageDialog(MessageDialog Dialog)
    {
        Dialog.YesNoPnl.Visible = false;
        Dialog.YesNoCancelPnl.Visible = false;
        Dialog.OKPnl.Visible = false;
        Dialog.OKCancelPnl.Visible = false;
        Dialog.MessageMemo.Items.Clear();
        Dialog.MemoPnl.Visible = true;
        Dialog.HeaderMemoPnl.Visible = false;
        var message = AbilityRules.TranslateRStr(GameText.MessageRStr);
        Dialog.TitleLbl.Text = message;
    }

    internal static void ShowMessageEvent(MessageDialog Dialog, MessageEventLog Event, ExecuteDoneEvent Done)
    {
        MessageEventLog Log;
        ResetMessageDialog(Dialog);
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        Dialog.TitleLbl.Text = Log.FTitle;
        switch (Log.FLayout)
        {
            case MessageEventLog.Layout.Normal:
                var text = Log.FMessage.Text;
                Dialog.MessageMemo.Items.Text = text;
                if (Dialog.MessageMemo.Items.Text.Length < 80)
                {
                    Dialog.MessageMemo.Items.Insert(0, "");
                }

                break;
            case MessageEventLog.Layout.Header:
                Dialog.MemoPnl.Visible = false;
                Dialog.HeaderMemoPnl.Visible = true;
                Dialog.HeaderLbl.Text = Log.FHeader;
                var fMessageText = Log.FMessage.Text;
                Dialog.HeaderMemo.Items.Text = fMessageText;
                break;
        }

        switch (Log.FButtons)
        {
            case MessageEventLog.Buttons.Ok:
                Dialog.OKPnl.Visible = true;
                break;
            case MessageEventLog.Buttons.OkCancel:
                Dialog.OKCancelPnl.Visible = true;
                break;
            case MessageEventLog.Buttons.YesNo:
                Dialog.YesNoPnl.Visible = true;
                break;
            case MessageEventLog.Buttons.YesNoCancel:
                Dialog.YesNoCancelPnl.Visible = true;
                break;
        }

        Dialog.Dlg.Visible = true;
    }

}
