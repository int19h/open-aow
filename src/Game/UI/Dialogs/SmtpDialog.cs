// Translated from AoWSMTPDlg.pas.
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class SmtpDialog : DataModule
{
    public SmtpDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Button ExitBtn;
    public Label SendingGameLbl;
    public Gauge ProgressBar;
    public Label RecipientLbl;
    public Panel SendPnl;
    public Panel FailedPnl;
    public MailMessage IMSMessage;
    public SmtpSender IMSSMTP;
    private IMSSendThread _thread;

    public ExecuteDoneEvent Done { get; private set; }

    private SmtpEventLog _event;
    private int _progress;
    private bool _finished;

    // AoWSMTPDlg.pas:115
    public void FinishDialog()
    {
        /* TODO: Implement Classes.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWSMTPDlg.pas:134
    public void ResetProgress()
    {
        _finished = false;
        ProgressBar.Position = 0;
        var sendingEmailGame = AbilityRules.TranslateResourceString(s_sendingEmailGameText);
        SendingGameLbl.Text = sendingEmailGame;
        FailedPnl.Visible = false;
        SendPnl.Visible = true;
        _progress = 0;
        ExitBtn.Visible = false;
    }

    // AoWSMTPDlg.pas:145
    public void StartSending()
    {
        /* TODO: Implement Classes.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWSMTPDlg.pas:154
    public void ImsThreadTerminatedEvent()
    {
        _event.Success = _thread.FSuccess;
        _finished = true;
        if (_event.Success)
        {
            _progress = 100;
        }
    }

    // AoWSMTPDlg.pas:161
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWSMTPDlg.pas:166
    public void ExitBtnClick()
    {
        Dlg.Visible = false;
    }

    // AoWSMTPDlg.pas:171
    public void IMSSMTPProgress(int progress, int total)
    {
        if (progress > _progress)
        {
            _progress = progress;
        }
    }

    // AoWSMTPDlg.pas:176
    public void ProgressBarUpdate()
    {
        if (ProgressBar.Position != _progress)
        {
            ProgressBar.Position = _progress;
            ProgressBar.Drawn = false;
        }
    }

    // AoWSMTPDlg.pas:185
    public void IMSSMTPError(ref string error)
    {
        if (_event != null)
        {
            _event.Error = error;
        }
    }

    // AoWSMTPDlg.pas:190
    public void DlgUpdate()
    {
        if (_finished)
        {
            if (_event.Success)
            {
                var gameSuccessfullySent = AbilityRules.TranslateResourceString(s_gameSuccessfullySentText);
                SendingGameLbl.Text = gameSuccessfullySent;
                ExitBtn.Visible = true;
            }
            else
            {
                var panel = FailedPnl;
                if (!panel.Visible)
                {
                    panel.Visible = true;
                    SendPnl.Visible = false;
                }
            }
        }
    }

    // AoWSMTPDlg.pas:207
    public void RetryBtnClick()
    {
        StartSending();
    }

    // AoWSMTPDlg.pas:212
    public void SettingsBtnClick()
    {
        EmailSettingsDialog.ShowEmailSettings(DialogHosts.EmailSettingsDialog);
    }

    private static readonly string s_gameSuccessfullySentText = "Game successfully sent";

    private static readonly string s_sendingEmailGameText = "Sending email game...";

    internal static void ShowSmtpEvent(SmtpDialog dialog, SmtpEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.ResetProgress();
        dialog.Done = done;
        dialog._event = eventValue;
        var smtpEventLog = dialog._event;
        smtpEventLog.AddRef();

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.RecipientFormat), [eventValue.ToAddress]);
        dialog.RecipientLbl.Text = format;
        try
        {
            dialog.IMSSMTP.Message = dialog.IMSMessage;
            var smtpServer = GameTables.GameEngine.Registry.GetSmtpServer();
            dialog.IMSSMTP.Host = smtpServer;
            dialog.IMSSMTP.WinsockLoaded = true;
            MailMessage tmsMessage = dialog.IMSMessage;
            tmsMessage.Encode = false;
            tmsMessage.IncludeHeader = false;
            tmsMessage.Header.From = eventValue.FromAddress;
            tmsMessage.Recipients.Clear();
            dialog.IMSMessage.Recipients.AddAddress(eventValue.ToAddress, "");

            dialog.IMSMessage.Subject = eventValue.Subject;
            dialog.IMSMessage.SetBody(eventValue.Message);
            dialog.IMSMessage.Attachments.Clear();
            dialog.IMSMessage.Attachments.AddFile(eventValue.FileName);
            dialog.StartSending();
            dialog.Dlg.Visible = true;
        }
        catch
        {
            dialog._event.Success = false;
            dialog._finished = true;
            dialog.Dlg.Visible = true;
        }
    }

}
