// Translated from AoWSMTPDlg.pas.
using Aow.Game.Events;
using Aow.Game.Net;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSMTPDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.E3GlobalsUnit;

namespace Aow.Game.Net
{

    // declarations of AoWSMTPDlg, from AoW.exe
    // Declaration-only field type from ims.dpl
    public partial class TmsMessage : TComponent
    {
        public TmsMessage(TComponent owner) : base(owner)
        { }

        public TmsMessageHeader FHeader;
        public string FSubject = "";
        public bool FIncludeHeader;
        public bool FEncode;
        public TmsAttList FAttachments;
        public TmsAddressList FRecipients;

        // AoWSMTPDlg.pas:20
        public void SetBody(TStrings Value)
        {
            /* TODO: Implement TmsMessage.SetBody. */
            throw new NotImplementedException();
        }
    }

    // Declaration-only field type from ims.dpl
    public partial class TmsSMTP : TComponent
    {
        public TmsSMTP(TComponent owner) : base(owner)
        { }

        public string FHost = "";
        public TmsMessage FMessage;

        // AoWSMTPDlg.pas:29
        public void SetWinsockLoaded(bool Value)
        {
            /* TODO: Implement TmsSMTP.SetWinsockLoaded. */
            throw new NotImplementedException();
        }

        public bool WinsockLoaded { set => SetWinsockLoaded(value); }

        // AoWSMTPDlg.pas:30
        public void Send()
        {
            /* TODO: Implement TmsSMTP.Send. */
            throw new NotImplementedException();
        }
    }
}

namespace Aow.Game.Ui.Dialogs
{

    [LegacyObject]
    public partial class TIMSSendThread
    {
        public bool FSuccess;

        // AoWSMTPDlg.pas:105
        protected internal void Execute()
        {
            try
            {
                AoWSMTPDlgUnit.SMTPDlg.IMSSMTP.Send();
                FSuccess = true;
            }
            catch
            {
                FSuccess = false;
            }
        }

        // Classes.pas:771
        public TIMSSendThread(bool CreateSuspended) : base()
        {
            CaptureAddress.Assign(this);
        }
    }

    public partial class TSMTPDlg : TDataModule
    {
        public TSMTPDlg(TComponent owner) : base(owner)
        { }

        public TAOWWindow Dlg;
        public TAoWFrame BCeltL;
        public TAoWFrame TCeltL;
        public TAOWImage TCeltLineL;
        public TAOWImage AOWImage1;
        public TAOWImage AOWImage2;
        public TAOWButton ExitBtn;
        public TAOWLabel SendingGameLbl;
        public TAoWGauge ProgressBar;
        public TAOWLabel RecipientLbl;
        public TAOWPanel SendPnl;
        public TAOWPanel FailedPnl;
        public TAOWMemo FailedMemo;
        public TAOWButton SettingsBtn;
        public TAOWButton RetryBtn;
        public TAOWButton ExitBtn2;
        public TAOWLabel TitleLbl;
        public TAOWButton Close;
        public TAOWImage TitleIcon;
        public TmsMessage IMSMessage;
        public TmsSMTP IMSSMTP;
        public TIvTranslator Translator;
        public TIMSSendThread FThread;
        public TExecuteDoneEvent FDone;
        public TSMTPEventLog FEvent;
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
            var sendingEmailGame = TranslateRStr(SendingEmailGameRStr);
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
                    var gameSuccessfullySent = TranslateRStr(GameSuccessfullySentRStr);
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
            ShowEmailSettings(EmailSettingsDlg);
        }
    }
}

namespace Aow.Game.Net
{

    public static partial class AoWSMTPDlgUnit
    {
        public static TSMTPDlg SMTPDlg;
    }
}
