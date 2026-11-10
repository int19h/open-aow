// Translated from AoWSMTPDlg.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
// Translated from AoWSMTPDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Net
{

    // declarations of AoWSMTPDlg, from AoW.exe
    // Declaration-only field type from ims.dpl
    public partial class MailMessage : Component
    {
        public MailMessage(Component owner) : base(owner)
        { }

        public MailHeader FHeader;
        public string FSubject = "";
        public bool FIncludeHeader;
        public bool FEncode;
        public MailAttachmentList FAttachments;
        public MailAddressList FRecipients;

        // AoWSMTPDlg.pas:20
        public void SetBody(StringCollection Value)
        {
            /* TODO: Implement TmsMessage.SetBody. */
            throw new NotImplementedException();
        }
    }

    // Declaration-only field type from ims.dpl
    public partial class SmtpSender : Component
    {
        public SmtpSender(Component owner) : base(owner)
        { }

        public string FHost = "";
        public MailMessage FMessage;

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
    public partial class IMSSendThread
    {
        public bool FSuccess;

        // AoWSMTPDlg.pas:105
        protected internal void Execute()
        {
            try
            {
                DialogHosts.SMTPDlg.IMSSMTP.Send();
                FSuccess = true;
            }
            catch
            {
                FSuccess = false;
            }
        }

        // Classes.pas:771
        public IMSSendThread(bool CreateSuspended) : base()
        {
            CaptureAddress.Assign(this);
        }
    }

    public partial class SMTPDlg : DataModule
    {
        public SMTPDlg(Component owner) : base(owner)
        { }

        public AOWWindow Dlg;
        public AoWFrame BCeltL;
        public AoWFrame TCeltL;
        public AOWImage TCeltLineL;
        public AOWImage AOWImage1;
        public AOWImage AOWImage2;
        public AOWButton ExitBtn;
        public AOWLabel SendingGameLbl;
        public AoWGauge ProgressBar;
        public AOWLabel RecipientLbl;
        public AOWPanel SendPnl;
        public AOWPanel FailedPnl;
        public AOWMemo FailedMemo;
        public AOWButton SettingsBtn;
        public AOWButton RetryBtn;
        public AOWButton ExitBtn2;
        public AOWLabel TitleLbl;
        public AOWButton Close;
        public AOWImage TitleIcon;
        public MailMessage IMSMessage;
        public SmtpSender IMSSMTP;
        public IvTranslator Translator;
        public IMSSendThread FThread;
        public ExecuteDoneEvent FDone;
        public SMTPEventLog FEvent;
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
            EmailSettingsDlg.ShowEmailSettings(DialogHosts.EmailSettingsDlg);
        }

        private static readonly string GameSuccessfullySentRStr = "Game successfully sent";

        private static readonly string SendingEmailGameRStr = "Sending email game...";

        internal static void ShowSMTPEvent(SMTPDlg Dialog, SMTPEventLog Event, ExecuteDoneEvent Done)
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
}
