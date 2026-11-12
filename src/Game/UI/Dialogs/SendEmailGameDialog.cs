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
    public Image Image1;
    public Image Image2;
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
    public ExecuteDoneEvent Done;
    public SendEmailGameEventLog SendEvent;

    // AoWSendEmailGameDlg.pas:63
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        EventLog eventValue;
        Dlg.Visible = false;
        done = Done;
        eventValue = SendEvent;
        Done = null;
        SendEvent = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWSendEmailGameDlg.pas:77
    public void ShowSendDialog(SendEmailGameEventLog eventValue, ExecuteDoneEvent done)
    {
        Done = done;
        SendEvent = eventValue;
        SendEvent.AddRef();
        var sendEmailGameEventLog = SendEvent;
        var player = AoWHSMap.Current.Players.GetPlayers(SendEvent.SendPlayer);
        EmailAddressEdit.Text = player.PbemSettings.EmailGame.EmailAddress;
        // { Original reuses the resource-string temporary for the formatted text. }
        var nameEx = AoWHSMap.Current.Players.GetPlayers(SendEvent.SendPlayer).GetNameWithRace();
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_nextPlayerFormat), [nameEx]);
        RecipientNameLbl.Text = format;
        if (sendEmailGameEventLog.Mode == 2)
        {
            var smtpServer = GameTables.GameEngine.Registry.GetSmtpServer();
            AutomatedChk.Checked = !IsEmpty(smtpServer);
            var fRegistrySmtpServer = GameTables.GameEngine.Registry.GetSmtpServer();
            ManualChk.Checked = IsEmpty(fRegistrySmtpServer);
        }
        else
        {
            AutomatedChk.Checked = sendEmailGameEventLog.Mode == 0;
            ManualChk.Checked = sendEmailGameEventLog.Mode == 1;
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
            SendEvent.Mode = 0;
        }

        if (ManualChk.Checked)
        {
            SendEvent.Mode = 1;
        }

        Dlg.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:114
    public void ExitBtnClick()
    {
        SendEvent.Mode = 2;
        Dlg.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:120
    public void EmailAddressEditChange()
    {
        AoWHSMap.Current.Players.GetPlayers(SendEvent.SendPlayer).PbemSettings.EmailGame.EmailAddress = EmailAddressEdit.Text;
    }

    // AoWSendEmailGameDlg.pas:125
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Dlg.Visible = false;
        }
    }

    private static readonly string s_nextPlayerFormat = "Next Player: %s";
}
