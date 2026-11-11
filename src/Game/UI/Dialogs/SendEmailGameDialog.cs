// Translated from AoWSendEmailGameDlg.pas.
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// callback, send event
public partial class SendEmailGameDialog : DataModule
{
    public SendEmailGameDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label TitleLbl;
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Image AOWImage1;
    public Image AOWImage2;
    public Panel OKCancelPnl;
    public Button OKBtn;
    public Button ExitBtn;
    public Edit EmailAddressEdit;
    public Label SeletModeInfoLbl;
    public CheckBox ManualChk;
    public CheckBox AutomatedChk;
    public Label ManualLbl;
    public Label AutomatedLbl;
    public Label RecipientNameLbl;
    public Image TitleIcon;
    public Panel ModePnl;
    public Panel ModePnlin;
    public Panel EditPnl;
    public Button Close;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public SendEmailGameEventLog FSendEvent;

    // AoWSendEmailGameDlg.pas:63
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        EventLog Event;
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
    public void ShowSendDialog(SendEmailGameEventLog Event, ExecuteDoneEvent Done)
    {
        FDone = Done;
        FSendEvent = Event;
        FSendEvent.AddRef();
        var sendEmailGameEventLog = FSendEvent;
        var player = AoWHSMap.Current.FPlayers.GetPlayers(FSendEvent.FSendPlayer);
        EmailAddressEdit.Text = player.FPBEMSettings.FEmailGame.FEmailAddress;
        // { Original reuses the resource-string temporary for the formatted text. }
        var nameEx = AoWHSMap.Current.FPlayers.GetPlayers(FSendEvent.FSendPlayer).GetNameEx();
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(NextPlayerXRStr), [nameEx]);
        RecipientNameLbl.Text = format;
        if (sendEmailGameEventLog.FMode == 2)
        {
            var smtpServer = GameTables.AoWEngine.FRegistry.GetSMTPServer();
            AutomatedChk.Checked = !IsEmpty(smtpServer);
            var fRegistrySMTPServer = GameTables.AoWEngine.FRegistry.GetSMTPServer();
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
        AoWHSMap.Current.FPlayers.GetPlayers(FSendEvent.FSendPlayer).FPBEMSettings.FEmailGame.FEmailAddress = EmailAddressEdit.Text;
    }

    // AoWSendEmailGameDlg.pas:125
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    private static readonly string NextPlayerXRStr = "Next Player: %s";
}
