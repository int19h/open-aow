// Translated from AoWSMTPDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.E3IMSTypes;
using static Aow.Units.E3IMSTypes.E3IMSTypes;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWSMTPDlg
{
    // declarations of AoWSMTPDlg, from AoW.exe
    // Declaration-only field type from ims.dpl
    public partial class TmsMessage : Aow.Rtl.TComponent
    {
        public TmsMessage(TComponent owner) : base(owner)
        { }

        public Aow.Units.E3IMSTypes.TmsMessageHeader FHeader;
        public Aow.Rtl.AnsiString FSubject;
        public bool FIncludeHeader;
        public bool FEncode;
        public Aow.Units.E3IMSTypes.TmsAttList FAttachments;
        public Aow.Units.E3IMSTypes.TmsAddressList FRecipients;
        // AoWSMTPDlg.pas:20
        public void SetBody(Aow.Rtl.TStrings Value)
        {
            /* TODO: Implement TmsMessage.SetBody. */
            throw new System.NotImplementedException();
        }
    }

    // Declaration-only field type from ims.dpl
    public partial class TmsSMTP : Aow.Rtl.TComponent
    {
        public TmsSMTP(TComponent owner) : base(owner)
        { }

        public Aow.Rtl.AnsiString FHost;
        public Aow.Units.AoWSMTPDlg.TmsMessage FMessage;
        // AoWSMTPDlg.pas:29
        public void SetWinsockLoaded(bool Value)
        {
            /* TODO: Implement TmsSMTP.SetWinsockLoaded. */
            throw new System.NotImplementedException();
        }

        public bool WinsockLoaded { set => SetWinsockLoaded(value); }

        // AoWSMTPDlg.pas:30
        public void Send()
        {
            /* TODO: Implement TmsSMTP.Send. */
            throw new System.NotImplementedException();
        }
    }

    public partial class TIMSSendThread : Aow.Rtl.TThread
    {
        public bool FSuccess;
        // AoWSMTPDlg.pas:105
        protected internal override void Execute()
        {
            try
            {
                Aow.Units.AoWSMTPDlg.AoWSMTPDlg.SMTPDlg.IMSSMTP.Send();
                FSuccess = true;
            }
            catch (System.Exception)
            {
                FSuccess = false;
            }
        }

        // Classes.pas:771
        public TIMSSendThread(bool CreateSuspended) : base()
        { }
    }

    public partial class TSMTPDlg : Aow.Rtl.TDataModule
    {
        public TSMTPDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        public Aow.Units.AoWButton.TAOWButton ExitBtn;
        public Aow.Units.AoWLabel.TAOWLabel SendingGameLbl;
        public Aow.Units.AOWGauge.TAoWGauge ProgressBar;
        public Aow.Units.AoWLabel.TAOWLabel RecipientLbl;
        public Aow.Units.AoWPanel.TAOWPanel SendPnl;
        public Aow.Units.AoWPanel.TAOWPanel FailedPnl;
        public Aow.Units.AoWMemo.TAOWMemo FailedMemo;
        public Aow.Units.AoWButton.TAOWButton SettingsBtn;
        public Aow.Units.AoWButton.TAOWButton RetryBtn;
        public Aow.Units.AoWButton.TAOWButton ExitBtn2;
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWSMTPDlg.TmsMessage IMSMessage;
        public Aow.Units.AoWSMTPDlg.TmsSMTP IMSSMTP;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.AoWSMTPDlg.TIMSSendThread FThread;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        public Aow.Units.AoWE.TSMTPEventLog FEvent;
        public int FProgress;
        public bool FFinished;
        // AoWSMTPDlg.pas:115
        public void FinishDialog()
        {
            /* TODO: Implement Classes.Destroy in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AoWSMTPDlg.pas:134
        public void ResetProgress()
        {
            FFinished = false;
            ProgressBar.Position = 0;
            var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SendingEmailGameRStr);
            SendingGameLbl.Text = call__1;
            FailedPnl.Visible = false;
            SendPnl.Visible = true;
            FProgress = 0;
            ExitBtn.Visible = false;
        }

        // AoWSMTPDlg.pas:145
        public void StartSending()
        {
            /* TODO: Implement Classes.Create in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AoWSMTPDlg.pas:154
        public void IMSThreadTerminatedEvent(Aow.Rtl.TObject Sender)
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
        public void IMSSMTPProgress(Aow.Rtl.TObject Sender, int Progress, int Total)
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
        public void IMSSMTPError(Aow.Rtl.TObject Sender, ref Aow.Rtl.AnsiString Error)
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
                    var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GameSuccessfullySentRStr);
                    SendingGameLbl.Text = call__2;
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
            Aow.Units.E3Globals.E3Globals.ShowEmailSettings(Aow.Units.E3Globals.E3Globals.EmailSettingsDlg);
        }
    }

    public static partial class AoWSMTPDlg
    {
        public static Aow.Units.AoWSMTPDlg.TSMTPDlg SMTPDlg;
    }
}
