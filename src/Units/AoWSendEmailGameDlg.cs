// Translated from AoWSendEmailGameDlg.pas.
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSendEmailGameDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// callback, send event
public partial class TSendEmailGameDlg : TDataModule
{
    public TSendEmailGameDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel TitleLbl;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAOWPanel OKCancelPnl;
    public TAOWButton OKBtn;
    public TAOWButton ExitBtn;
    public TAOWEdit EmailAddressEdit;
    public TAOWLabel SeletModeInfoLbl;
    public TAOWCheckBox ManualChk;
    public TAOWCheckBox AutomatedChk;
    public TAOWLabel ManualLbl;
    public TAOWLabel AutomatedLbl;
    public TAOWLabel RecipientNameLbl;
    public TAOWImage TitleIcon;
    public TAOWPanel ModePnl;
    public TAOWPanel ModePnlin;
    public TAOWPanel EditPnl;
    public TAOWButton Close;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TSendEmailGameEventLog FSendEvent;

    // AoWSendEmailGameDlg.pas:63
    public void FinishDialog()
    {
        TExecuteDoneEvent Done;
        TEventLog Event;
        Dlg.Visible = false;
        Done = FDone;
        Event = FSendEvent;
        FDone = null;
        FSendEvent = null;
        if (Done != null)
        {
            Done(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWSendEmailGameDlg.pas:77
    public void ShowSendDialog(TSendEmailGameEventLog Event, TExecuteDoneEvent Done)
    {
        FDone = Done;
        FSendEvent = Event;
        FSendEvent.AddRef();
        var sendEmailGameEventLog = FSendEvent;
        var player = AoWHSMap.FPlayers.GetPlayers(FSendEvent.FSendPlayer);
        EmailAddressEdit.Text = player.FPBEMSettings.FEmailGame.FEmailAddress;
        // { Original reuses the resource-string temporary for the formatted text. }
        var nameEx = AoWHSMap.FPlayers.GetPlayers(FSendEvent.FSendPlayer).GetNameEx();
        var format = AoWEUnit.Format(TranslateRStr(NextPlayerXRStr), [nameEx]);
        RecipientNameLbl.Text = format;
        if (sendEmailGameEventLog.FMode == 2)
        {
            var smtpServer = AoWEngine.FRegistry.GetSMTPServer();
            AutomatedChk.Checked = !IsEmpty(smtpServer);
            var fRegistrySMTPServer = AoWEngine.FRegistry.GetSMTPServer();
            ManualChk.Checked = IsEmpty(fRegistrySMTPServer);
        }
        else
        {
            AutomatedChk.Checked = sendEmailGameEventLog.FMode == 0;
            ManualChk.Checked = sendEmailGameEventLog.FMode == 1;
        }

        Dlg.Visible = true;
    }

    // AoWSendEmailGameDlg.pas:102
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWSendEmailGameDlg.pas:107
    public void OKBtnClick()
    {
        if (AutomatedChk.Checked)
        {
            FSendEvent.FMode = 0;
        }

        if (ManualChk.Checked)
        {
            FSendEvent.FMode = 1;
        }

        Dlg.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:114
    public void ExitBtnClick()
    {
        FSendEvent.FMode = 2;
        Dlg.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:120
    public void EmailAddressEditChange()
    {
        AoWHSMap.FPlayers.GetPlayers(FSendEvent.FSendPlayer).FPBEMSettings.FEmailGame.FEmailAddress = EmailAddressEdit.Text;
    }

    // AoWSendEmailGameDlg.pas:125
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWSendEmailGameDlgUnit
{
}
