// Translated from AoWSendEmailGameDlg.pas.
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// callback, send event
public partial class SendEmailGameDialog : UIModule
{
    public SendEmailGameDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Edit EmailAddressEdit;
    public CheckBox ManualCheckBox;
    public CheckBox AutomatedCheckBox;
    public Label RecipientNameLabel;
    private event ExecuteDoneEvent Done;
    private SendEmailGameEventLog _sendEvent;

    // AoWSendEmailGameDlg.pas:63
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        EventLog eventValue;
        Window.Visible = false;
        done = Done;
        eventValue = _sendEvent;
        Done = null;
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
        Done = done;
        _sendEvent = eventValue;
        _sendEvent.AddRef();
        var sendEmailGameEventLog = _sendEvent;
        var player = StrategicMap.Current.Players.GetPlayers(_sendEvent.SendPlayer);
        EmailAddressEdit.Text = player.PbemSettings.EmailGame.EmailAddress;
        // { Original reuses the resource-string temporary for the formatted text. }
        var nameEx = StrategicMap.Current.Players.GetPlayers(_sendEvent.SendPlayer).GetNameWithRace();
        var format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_nextPlayerFormat), [nameEx]);
        RecipientNameLabel.Text = format;
        if (sendEmailGameEventLog.Mode == 2)
        {
            var smtpServer = UI.Engine.Registry.GetSmtpServer();
            AutomatedCheckBox.Checked = !IsEmpty(smtpServer);
            var fRegistrySmtpServer = UI.Engine.Registry.GetSmtpServer();
            ManualCheckBox.Checked = IsEmpty(fRegistrySmtpServer);
        }
        else
        {
            AutomatedCheckBox.Checked = sendEmailGameEventLog.Mode == 0;
            ManualCheckBox.Checked = sendEmailGameEventLog.Mode == 1;
        }

        Window.Visible = true;
    }

    // AoWSendEmailGameDlg.pas:102
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWSendEmailGameDlg.pas:107
    public void OKBtnClick()
    {
        if (AutomatedCheckBox.Checked)
        {
            _sendEvent.Mode = 0;
        }

        if (ManualCheckBox.Checked)
        {
            _sendEvent.Mode = 1;
        }

        Window.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:114
    public void ExitBtnClick()
    {
        _sendEvent.Mode = 2;
        Window.Visible = false;
    }

    // AoWSendEmailGameDlg.pas:120
    public void EmailAddressEditChange()
    {
        StrategicMap.Current.Players.GetPlayers(_sendEvent.SendPlayer).PbemSettings.EmailGame.EmailAddress = EmailAddressEdit.Text;
    }

    // AoWSendEmailGameDlg.pas:125
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    private static readonly string s_nextPlayerFormat = "Next Player: %s";
}
