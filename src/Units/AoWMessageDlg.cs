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
public partial class TMessageDlg : TDataModule
{
    public TMessageDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel TitleLbl;
    public TAOWPanel YesNoPnl;
    public TAOWButton YesBtn;
    public TAOWButton NoBtn;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAOWButton OKBtn;
    public TAOWPanel OKPnl;
    public TAOWPanel OKCancelPnl;
    public TAOWButton OKBtn2;
    public TAOWButton CancelBtn;
    public TAOWMemo MessageMemo;
    public TAOWVScrollBar MessageMemoSB;
    public TAOWPanel YesNoCancelPnl;
    public TAOWButton YesBtn2;
    public TAOWButton NoBtn2;
    public TAOWButton CancelBtn2;
    public TAOWPanel MemoPnl;
    public TAOWPanel HeaderMemoPnl;
    public TAOWMemo HeaderMemo;
    public TAOWVScrollBar HeaderMemoSB;
    public TAOWLabel HeaderLbl;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TMessageEventLog FEvent;

    // AoWMessageDlg.pas:68
    public void FinishDialog()
    {
        TExecuteDoneEvent Done;
        TMessageEventLog Log;
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
        Aow.Game.Ui.Dialogs.TMessageDlg.ResetMessageDialog(Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg);
        Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add("");
        var demoVersionMessage = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.DemoVersionMessageRStr);
        Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add(demoVersionMessage);
        Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.OKPnl.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.Dlg.Visible = true;
    }

    internal static void ResetMessageDialog(TMessageDlg Dialog)
    {
        Dialog.YesNoPnl.Visible = false;
        Dialog.YesNoCancelPnl.Visible = false;
        Dialog.OKPnl.Visible = false;
        Dialog.OKCancelPnl.Visible = false;
        Dialog.MessageMemo.Items.Clear();
        Dialog.MemoPnl.Visible = true;
        Dialog.HeaderMemoPnl.Visible = false;
        var message = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.MessageRStr);
        Dialog.TitleLbl.Text = message;
    }

    internal static void ShowMessageEvent(TMessageDlg Dialog, TMessageEventLog Event, TExecuteDoneEvent Done)
    {
        TMessageEventLog Log;
        Aow.Game.Ui.Dialogs.TMessageDlg.ResetMessageDialog(Dialog);
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        Dialog.TitleLbl.Text = Log.FTitle;
        switch (Log.FLayout)
        {
            case TMessageEventLogLayout.melNormal:
                var text = Log.FMessage.Text;
                Dialog.MessageMemo.Items.Text = text;
                if (Dialog.MessageMemo.Items.Text.Length < 80)
                {
                    Dialog.MessageMemo.Items.Insert(0, "");
                }

                break;
            case TMessageEventLogLayout.melHeader:
                Dialog.MemoPnl.Visible = false;
                Dialog.HeaderMemoPnl.Visible = true;
                Dialog.HeaderLbl.Text = Log.FHeader;
                var fMessageText = Log.FMessage.Text;
                Dialog.HeaderMemo.Items.Text = fMessageText;
                break;
        }

        switch (Log.FButtons)
        {
            case TMessageEventLogButtons.mebOk:
                Dialog.OKPnl.Visible = true;
                break;
            case TMessageEventLogButtons.mebOkCancel:
                Dialog.OKCancelPnl.Visible = true;
                break;
            case TMessageEventLogButtons.mebYesNo:
                Dialog.YesNoPnl.Visible = true;
                break;
            case TMessageEventLogButtons.mebYesNoCancel:
                Dialog.YesNoCancelPnl.Visible = true;
                break;
        }

        Dialog.Dlg.Visible = true;
    }

}
