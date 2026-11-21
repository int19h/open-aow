#nullable disable warnings
// Translated from AoWSMTPDlg.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.UI.Dialogs;

public sealed partial class SmtpDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Button ExitButton { get; private set; }
    public Label SendingGameLabel { get; private set; }
    public Gauge ProgressBar { get; private set; }
    public Label RecipientLabel { get; private set; }
    public Panel SendPanel { get; private set; }
    public Panel FailedPanel { get; private set; }
    public MailMessage MailMessage { get; private set; }
    public SmtpSender SmtpSender { get; private set; }

    private SmtpEventLog _event;
    private int _progress;
    private bool _finished;

    // AoWSMTPDlg.pas:115
    public void FinishDialog()
        /* TODO: Implement Classes.Destroy in the platform or editor service. */
        => throw new NotImplementedException();

    // AoWSMTPDlg.pas:134
    public void ResetProgress()
    {
        _finished = false;
        ProgressBar.Position = 0;
        string sendingEmailGame = UI.Engine.TranslateResourceString(s_sendingEmailGameText);
        SendingGameLabel.Text = sendingEmailGame;
        FailedPanel.Visible = false;
        SendPanel.Visible = true;
        _progress = 0;
        ExitButton.Visible = false;
    }

    // AoWSMTPDlg.pas:145
    public void StartSending()
        /* TODO: Implement Classes.Create in the platform or editor service. */
        => throw new NotImplementedException();

    // AoWSMTPDlg.pas:161
    public void DlgHide() => FinishDialog();

    // AoWSMTPDlg.pas:166
    public void ExitBtnClick() => Window.Visible = false;

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
            ProgressBar.IsDrawn = false;
        }
    }

    // AoWSMTPDlg.pas:185
    public void SmtpSenderError(string error) => _event?.Error = error;

    // AoWSMTPDlg.pas:190
    public void DlgUpdate()
    {
        if (_finished)
        {
            if (_event.Success)
            {
                string gameSuccessfullySent = UI.Engine.TranslateResourceString(s_gameSuccessfullySentText);
                SendingGameLabel.Text = gameSuccessfullySent;
                ExitButton.Visible = true;
            }
            else
            {
                Panel panel = FailedPanel;
                if (!panel.Visible)
                {
                    panel.Visible = true;
                    SendPanel.Visible = false;
                }
            }
        }
    }

    // AoWSMTPDlg.pas:207
    public void RetryBtnClick() => StartSending();

    // AoWSMTPDlg.pas:212
    public void SettingsBtnClick() => EmailSettingsDialog.ShowEmailSettings(UI.EmailSettingsDialog);

    private static readonly string s_gameSuccessfullySentText = "Game successfully sent";

    private static readonly string s_sendingEmailGameText = "Sending email game...";

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Event renderer callbacks share a uniform signature taking done.")]
    internal static void ShowSmtpEvent(SmtpDialog dialog, SmtpEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.ResetProgress();
        dialog._event = eventValue;
        SmtpEventLog smtpEventLog = dialog._event;
        _ = smtpEventLog.AddRef();

        string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.RecipientFormat), arguments: [eventValue.ToAddress]);
        dialog.RecipientLabel.Text = format;
        try
        {
            dialog.SmtpSender.Message = dialog.MailMessage;
            string smtpServer = dialog.UI.Engine.Registry.GetSmtpServer();
            dialog.SmtpSender.Host = smtpServer;
            dialog.SmtpSender.WinsockLoaded = true;
            MailMessage tmsMessage = dialog.MailMessage;
            tmsMessage.Encode = false;
            tmsMessage.IncludeHeader = false;
            tmsMessage.Header.From = eventValue.FromAddress;
            tmsMessage.Recipients.Clear();
            dialog.MailMessage.Recipients.AddAddress(eventValue.ToAddress, name: "");

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
