// Translated from AoWMessageDlg.pas.
using Aow.Game.Events;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWMessageDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWExeFormsUnit;

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
}

public static partial class AoWMessageDlgUnit
{
    // AoWMessageDlg.pas:116
    public static void ShowDemoMessage()
    {
        ResetMessageDialog(MessageDlg);
        MessageDlg.MessageMemo.Items.Add("");
        var demoVersionMessage = TranslateRStr(DemoVersionMessageRStr);
        MessageDlg.MessageMemo.Items.Add(demoVersionMessage);
        MessageDlg.OKPnl.Visible = true;
        MessageDlg.Dlg.Visible = true;
    }

    public static TMessageDlg MessageDlg;
}
