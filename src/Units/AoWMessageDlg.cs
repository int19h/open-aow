// Translated from AoWMessageDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWMessageDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// completion event, retained message event
public partial class MessageDlg : DataModule
{
    public MessageDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel TitleLbl;
    public AOWPanel YesNoPnl;
    public AOWButton YesBtn;
    public AOWButton NoBtn;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public AOWButton OKBtn;
    public AOWPanel OKPnl;
    public AOWPanel OKCancelPnl;
    public AOWButton OKBtn2;
    public AOWButton CancelBtn;
    public AOWMemo MessageMemo;
    public AOWVScrollBar MessageMemoSB;
    public AOWPanel YesNoCancelPnl;
    public AOWButton YesBtn2;
    public AOWButton NoBtn2;
    public AOWButton CancelBtn2;
    public AOWPanel MemoPnl;
    public AOWPanel HeaderMemoPnl;
    public AOWMemo HeaderMemo;
    public AOWVScrollBar HeaderMemoSB;
    public AOWLabel HeaderLbl;
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

    internal static void ResetMessageDialog(MessageDlg Dialog)
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

    internal static void ShowMessageEvent(MessageDlg Dialog, MessageEventLog Event, ExecuteDoneEvent Done)
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
