// Translated from AoWSendEmailGameDlg.pas.
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSendEmailGameDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// callback, send event
public partial class SendEmailGameDlg : DataModule
{
    public SendEmailGameDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel TitleLbl;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public AOWPanel OKCancelPnl;
    public AOWButton OKBtn;
    public AOWButton ExitBtn;
    public AOWEdit EmailAddressEdit;
    public AOWLabel SeletModeInfoLbl;
    public AOWCheckBox ManualChk;
    public AOWCheckBox AutomatedChk;
    public AOWLabel ManualLbl;
    public AOWLabel AutomatedLbl;
    public AOWLabel RecipientNameLbl;
    public AOWImage TitleIcon;
    public AOWPanel ModePnl;
    public AOWPanel ModePnlin;
    public AOWPanel EditPnl;
    public AOWButton Close;
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
        var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FSendEvent.FSendPlayer);
        EmailAddressEdit.Text = player.FPBEMSettings.FEmailGame.FEmailAddress;
        // { Original reuses the resource-string temporary for the formatted text. }
        var nameEx = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FSendEvent.FSendPlayer).GetNameEx();
        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(NextPlayerXRStr), [nameEx]);
        RecipientNameLbl.Text = format;
        if (sendEmailGameEventLog.FMode == 2)
        {
            var smtpServer = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSMTPServer();
            AutomatedChk.Checked = !IsEmpty(smtpServer);
            var fRegistrySMTPServer = Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetSMTPServer();
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
        Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(FSendEvent.FSendPlayer).FPBEMSettings.FEmailGame.FEmailAddress = EmailAddressEdit.Text;
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
