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

    public Window Window;
    public Button ExitButton;
    public Label SendingGameLabel;
    public Gauge ProgressBar;
    public Label RecipientLabel;
    public Panel SendPanel;
    public Panel FailedPanel;
    public MailMessage MailMessage;
    public SmtpSender SmtpSender;

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
        SendingGameLabel.Text = sendingEmailGame;
        FailedPanel.Visible = false;
        SendPanel.Visible = true;
        _progress = 0;
        ExitButton.Visible = false;
    }

    // AoWSMTPDlg.pas:145
    public void StartSending()
    {
        /* TODO: Implement Classes.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWSMTPDlg.pas:161
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWSMTPDlg.pas:166
    public void ExitBtnClick()
    {
        Window.Visible = false;
    }

    // AoWSMTPDlg.pas:171
    public void SmtpSenderProgress(int progress, int total)
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
    public void SmtpSenderError(ref string error)
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
                SendingGameLabel.Text = gameSuccessfullySent;
                ExitButton.Visible = true;
            }
            else
            {
                var panel = FailedPanel;
                if (!panel.Visible)
                {
                    panel.Visible = true;
                    SendPanel.Visible = false;
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
        dialog._event = eventValue;
        var smtpEventLog = dialog._event;
        smtpEventLog.AddRef();

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.RecipientFormat), [eventValue.ToAddress]);
        dialog.RecipientLabel.Text = format;
        try
        {
            dialog.SmtpSender.Message = dialog.MailMessage;
            var smtpServer = GameTables.GameEngine.Registry.GetSmtpServer();
            dialog.SmtpSender.Host = smtpServer;
            dialog.SmtpSender.WinsockLoaded = true;
            MailMessage tmsMessage = dialog.MailMessage;
            tmsMessage.Encode = false;
            tmsMessage.IncludeHeader = false;
            tmsMessage.Header.From = eventValue.FromAddress;
            tmsMessage.Recipients.Clear();
            dialog.MailMessage.Recipients.AddAddress(eventValue.ToAddress, "");

            dialog.MailMessage.Subject = eventValue.Subject;
            dialog.MailMessage.SetBody(eventValue.Message);
            dialog.MailMessage.Attachments.Clear();
            dialog.MailMessage.Attachments.AddFile(eventValue.FileName);
            dialog.StartSending();
            dialog.Window.Visible = true;
        }
        catch
        {
            dialog._event.Success = false;
            dialog._finished = true;
            dialog.Window.Visible = true;
        }
    }

}
