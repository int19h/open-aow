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
    public Edit EmailAddressEdit;
    public CheckBox ManualChk;
    public CheckBox AutomatedChk;
    public Label RecipientNameLbl;
    private ExecuteDoneEvent _done;
    private SendEmailGameEventLog _sendEvent;

    // AoWSendEmailGameDlg.pas:63
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        EventLog eventValue;
        Dlg.Visible = false;
        done = _done;
        eventValue = _sendEvent;
        _done = null;
        _sendEvent = null;
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
        _done = done;
        _sendEvent = eventValue;
        _sendEvent.AddRef();
        var sendEmailGameEventLog = _sendEvent;
        var player = AoWHSMap.Current.Players.GetPlayers(_sendEvent.SendPlayer);
        EmailAddressEdit.Text = player.PbemSettings.EmailGame.EmailAddress;
        // { Original reuses the resource-string temporary for the formatted text. }
        var nameEx = AoWHSMap.Current.Players.GetPlayers(_sendEvent.SendPlayer).GetNameWithRace();
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
            _sendEvent.Mode = 0;
        }

        if (ManualChk.Checked)
        {
            _sendEvent.Mode = 1;
        }

        Dlg.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:114
    public void ExitBtnClick()
    {
        _sendEvent.Mode = 2;
        Dlg.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:120
    public void EmailAddressEditChange()
    {
        AoWHSMap.Current.Players.GetPlayers(_sendEvent.SendPlayer).PbemSettings.EmailGame.EmailAddress = EmailAddressEdit.Text;
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
