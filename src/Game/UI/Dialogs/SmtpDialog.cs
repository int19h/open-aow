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
    public Image AOWImage1;
    public Image AOWImage2;
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
    public IMSSendThread FThread;
    public ExecuteDoneEvent FDone;
    public SmtpEventLog FEvent;
    public int FProgress;
    public bool FFinished;

    // AoWSMTPDlg.pas:115
    public void FinishDialog()
    {
        /* TODO: Implement Classes.Destroy in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWSMTPDlg.pas:134
    public void ResetProgress()
    {
        FFinished = false;
        ProgressBar.Position = 0;
        var sendingEmailGame = AbilityRules.TranslateRStr(SendingEmailGameRStr);
        SendingGameLbl.Text = sendingEmailGame;
        FailedPnl.Visible = false;
        SendPnl.Visible = true;
        FProgress = 0;
        ExitBtn.Visible = false;
    }

    // AoWSMTPDlg.pas:145
    public void StartSending()
    {
        /* TODO: Implement Classes.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWSMTPDlg.pas:154
    public void IMSThreadTerminatedEvent()
    {
        FEvent.FSuccess = FThread.FSuccess;
        FFinished = true;
        if (FEvent.FSuccess)
        {
            FProgress = 100;
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
    public void IMSSMTPProgress(int Progress, int Total)
    {
        if (Progress > FProgress)
        {
            FProgress = Progress;
        }
    }

    // AoWSMTPDlg.pas:176
    public void ProgressBarUpdate()
    {
        if (ProgressBar.Position != FProgress)
        {
            ProgressBar.Position = FProgress;
            ProgressBar.FDrawn = false;
        }
    }

    // AoWSMTPDlg.pas:185
    public void IMSSMTPError(ref string Error)
    {
        if (FEvent != null)
        {
            FEvent.FError = Error;
        }
    }

    // AoWSMTPDlg.pas:190
    public void DlgUpdate()
    {
        if (FFinished)
        {
            if (FEvent.FSuccess)
            {
                var gameSuccessfullySent = AbilityRules.TranslateRStr(GameSuccessfullySentRStr);
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
        EmailSettingsDialog.ShowEmailSettings(DialogHosts.EmailSettingsDlg);
    }

    private static readonly string GameSuccessfullySentRStr = "Game successfully sent";

    private static readonly string SendingEmailGameRStr = "Sending email game...";

    internal static void ShowSMTPEvent(SmtpDialog Dialog, SmtpEventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.ResetProgress();
        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        var smtpEventLog = Dialog.FEvent;
        smtpEventLog.AddRef();

        var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.RecipientXRStr), [Event.FToAddress]);
        Dialog.RecipientLbl.Text = format;
        try
        {
            Dialog.IMSSMTP.FMessage = Dialog.IMSMessage;
            var smtpServer = GameTables.AoWEngine.FRegistry.GetSMTPServer();
            Dialog.IMSSMTP.FHost = smtpServer;
            Dialog.IMSSMTP.WinsockLoaded = true;
            MailMessage tmsMessage = Dialog.IMSMessage;
            tmsMessage.FEncode = false;
            tmsMessage.FIncludeHeader = false;
            tmsMessage.FHeader.FFrom = Event.FFromAddress;
            tmsMessage.FRecipients.Clear();
            Dialog.IMSMessage.FRecipients.AddAddress(Event.FToAddress, "");

            Dialog.IMSMessage.FSubject = Event.FSubject;
            Dialog.IMSMessage.SetBody(Event.FMessage);
            Dialog.IMSMessage.FAttachments.Clear();
            Dialog.IMSMessage.FAttachments.AddFile(Event.FFileName);
            Dialog.StartSending();
            Dialog.Dlg.Visible = true;
        }
        catch
        {
            Dialog.FEvent.FSuccess = false;
            Dialog.FFinished = true;
            Dialog.Dlg.Visible = true;
        }
    }

}
