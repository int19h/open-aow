// Generated from AoWSMTPDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
    // { Draft declarations of AoWSMTPDlg, from AoW.exe.
//   Layouts and VMT order follow data/classes.json; guessed signatures are marked draft.
//   Stub bodies are placeholders for source recovery. }
    // { Declaration-only field type from ims.dpl (instance size 588). }
    public partial class TmsMessage : Aow.Rtl.TComponent
    {
        public Aow.Units.E3IMSTypes.TmsMessageHeader FHeader;
        // { $24 }
        public Aow.Rtl.AnsiString FSubject;
        // { $28 }
        public bool FIncludeHeader;
        public bool FEncode;
        // { $128/$129; 0044DBD5 }
        public Aow.Units.FixedArray_byte_10 FReserved12A;
        public Aow.Units.E3IMSTypes.TmsAttList FAttachments;
        // { $134 }
        public Aow.Units.FixedArray_byte_260 FReserved138;
        public Aow.Units.E3IMSTypes.TmsAddressList FRecipients;
        // { $23C }
        public Aow.Units.FixedArray_byte_12 FReserved240;
        // AoWSMTPDlg.pas:20
        public void SetBody(Aow.Rtl.TStrings Value)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    // { Declaration-only field type from ims.dpl (instance size 636). }
    public partial class TmsSMTP : Aow.Rtl.TComponent
    {
        public Aow.Rtl.AnsiString FHost;
        // { $24 }
        public Aow.Units.FixedArray_byte_584 FReserved28;
        public Aow.Units.AoWSMTPDlg.TmsMessage FMessage;
        // { $270 }
        public Aow.Units.FixedArray_byte_8 FReserved274;
        // AoWSMTPDlg.pas:29
        public void SetWinsockLoaded(bool Value)
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }

        public bool WinsockLoaded { set => SetWinsockLoaded(value); }

        // AoWSMTPDlg.pas:30
        // { 0044DBB9 }
        public void Send()
        {
            /* TODO(T-4.4) Routine body is absent */
            throw new System.NotImplementedException();
        }
    }

    // { TIMSSendThread (instance size 44, VMT 0044D5D8) }
    public partial class TIMSSendThread : Aow.Rtl.TThread
    {
        public bool FSuccess;
        // { $28 }
        public Aow.Units.FixedArray_byte_3 F_IMSSendThread_29;
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
        public static Aow.Units.AoWSMTPDlg.TIMSSendThread Create(bool CreateSuspended)
        {
            return new Aow.Units.AoWSMTPDlg.TIMSSendThread();
        }
    }

    // { TSMTPDlg (instance size 180, VMT 0044D630) }
    public partial class TSMTPDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        // { +0058 }
        public Aow.Units.AoWButton.TAOWButton ExitBtn;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel SendingGameLbl;
        // { +0060 }
        public Aow.Units.AOWGauge.TAoWGauge ProgressBar;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel RecipientLbl;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel SendPnl;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel FailedPnl;
        // { +0070 }
        public Aow.Units.AoWMemo.TAOWMemo FailedMemo;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton SettingsBtn;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton RetryBtn;
        // { +007C }
        public Aow.Units.AoWButton.TAOWButton ExitBtn2;
        // { +0080 }
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        // { +0084 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +008C }
        public Aow.Units.AoWSMTPDlg.TmsMessage IMSMessage;
        // { +0090 }
        public Aow.Units.AoWSMTPDlg.TmsSMTP IMSSMTP;
        // { +0094 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0098 }
        public Aow.Units.AoWSMTPDlg.TIMSSendThread FThread;
        // { $9C }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $A0 }
        public Aow.Units.AoWE.TSMTPEventLog FEvent;
        // { $A8 }
        public int FProgress;
        // { $AC }
        public bool FFinished;
        // { $B0 }
        public Aow.Units.FixedArray_byte_3 F_SMTPDlg_B1;
        // AoWSMTPDlg.pas:115
        public void FinishDialog()
        {
            /* TODO(T-shim) Platform or editor boundary Classes.Destroy */
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
            /* TODO(T-shim) Platform or editor boundary Classes.Create */
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
        // { draft }
        public void ExitBtnClick()
        {
            Dlg.Visible = false;
        }

        // AoWSMTPDlg.pas:171
        // { draft }
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
        // { draft }
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
                    {
                        var w__3 = FailedPnl;
                        if (!w__3.Visible)
                        {
                            w__3.Visible = true;
                            SendPnl.Visible = false;
                        }
                    }
                }
            }
        }

        // AoWSMTPDlg.pas:207
        // { draft }
        public void RetryBtnClick()
        {
            StartSending();
        }

        // AoWSMTPDlg.pas:212
        // { draft }
        public void SettingsBtnClick()
        {
            Aow.Units.E3Globals.E3Globals.ShowEmailSettings(Aow.Units.E3Globals.E3Globals.EmailSettingsDlg);
        }
    }

    public static partial class AoWSMTPDlg
    {
        // { draft }
        public static Aow.Units.AoWSMTPDlg.TSMTPDlg SMTPDlg;
    }
}
