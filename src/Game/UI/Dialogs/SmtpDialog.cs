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
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Image Image1;
    public Image Image2;
    public Button ExitBtn;
    public Label SendingGameLbl;
    public Gauge ProgressBar;
    public Label RecipientLbl;
    public Panel SendPnl;
    public Panel FailedPnl;
    public Memo FailedMemo;
    public Button SettingsBtn;
    public Button RetryBtn;
    public Button ExitBtn2;
    public Label TitleLbl;
    public Button Close;
    public Image TitleIcon;
    public MailMessage IMSMessage;
    public SmtpSender IMSSMTP;
    public IvTranslator Translator;
    public IMSSendThread Thread;
    public ExecuteDoneEvent Done;
    public SmtpEventLog Event;
    public int Progress;
    public bool Finished;

    // AoWSMTPDlg.pas:115
    public void FinishDialog()
    {
        /* TODO: Implement Classes.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWSMTPDlg.pas:134
    public void ResetProgress()
    {
        Finished = false;
        ProgressBar.Position = 0;
        var sendingEmailGame = AbilityRules.TranslateResourceString(s_sendingEmailGameText);
        SendingGameLbl.Text = sendingEmailGame;
        FailedPnl.Visible = false;
        SendPnl.Visible = true;
        Progress = 0;
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
        Event.Success = Thread.FSuccess;
        Finished = true;
        if (Event.Success)
        {
            Progress = 100;
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
        if (progress > Progress)
        {
            Progress = progress;
        }
    }

    // AoWSMTPDlg.pas:176
    public void ProgressBarUpdate()
    {
        if (ProgressBar.Position != Progress)
        {
            ProgressBar.Position = Progress;
            ProgressBar.Drawn = false;
        }
    }

    // AoWSMTPDlg.pas:185
    public void IMSSMTPError(ref string error)
    {
        if (Event != null)
        {
            Event.Error = error;
        }
    }

    // AoWSMTPDlg.pas:190
    public void DlgUpdate()
    {
        if (Finished)
        {
            if (Event.Success)
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
        dialog.Event = eventValue;
        var smtpEventLog = dialog.Event;
        smtpEventLog.AddRef();

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_recipientFormat), [eventValue.ToAddress]);
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
            dialog.Event.Success = false;
            dialog.Finished = true;
            dialog.Dlg.Visible = true;
        }
    }

}
