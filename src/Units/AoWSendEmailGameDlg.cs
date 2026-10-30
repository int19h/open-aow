// Translated from AoWSendEmailGameDlg.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWSMTPDlg;
using static Aow.Units.AoWSMTPDlg.AoWSMTPDlg;

namespace Aow.Units.AoWSendEmailGameDlg
{
    // callback, send event
    public partial class TSendEmailGameDlg : Aow.Rtl.TDataModule
    {
        public TSendEmailGameDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        public Aow.Units.AoWPanel.TAOWPanel OKCancelPnl;
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        public Aow.Units.AoWButton.TAOWButton ExitBtn;
        public Aow.Units.AoWEdit.TAOWEdit EmailAddressEdit;
        public Aow.Units.AoWLabel.TAOWLabel SeletModeInfoLbl;
        public Aow.Units.AoWCheckBox.TAOWCheckBox ManualChk;
        public Aow.Units.AoWCheckBox.TAOWCheckBox AutomatedChk;
        public Aow.Units.AoWLabel.TAOWLabel ManualLbl;
        public Aow.Units.AoWLabel.TAOWLabel AutomatedLbl;
        public Aow.Units.AoWLabel.TAOWLabel RecipientNameLbl;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWPanel.TAOWPanel ModePnl;
        public Aow.Units.AoWPanel.TAOWPanel ModePnlin;
        public Aow.Units.AoWPanel.TAOWPanel EditPnl;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        public Aow.Units.AoWE.TSendEmailGameEventLog FSendEvent;
        // AoWSendEmailGameDlg.pas:63
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TEventLog Event = default;
            Dlg.Visible = false;
            Done = FDone;
            Event = FSendEvent;
            FDone = null;
            FSendEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWSendEmailGameDlg.pas:77
        public void ShowSendDialog(Aow.Units.AoWE.TSendEmailGameEventLog Event, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            FDone = Done;
            ;
            FSendEvent = Aow.Rtl.Rtl.ReferenceFromId<Aow.Units.AoWE.TSendEmailGameEventLog>((int)(Aow.Rtl.Rtl.ObjectToInt(Event) << 0));
            FSendEvent.AddRef();
            var sendEmailGameEventLog = FSendEvent;
            var call__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FSendEvent.FSendPlayer);
            EmailAddressEdit.Text = call__2.FPBEMSettings.FEmailGame.FEmailAddress;
            // { Original reuses the resource-string temporary for the formatted text. }
            var call__3 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FSendEvent.FSendPlayer);
            var call__4 = call__3.GetNameEx();
            var call__5 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NextPlayerXRStr);
            var call__6 = Aow.Units.AoWE.AoWE.Format(call__5, [call__4]);
            RecipientNameLbl.Text = call__6;
            if (sendEmailGameEventLog.FMode == 2)
            {
                var call__7 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSMTPServer();
                AutomatedChk.Checked = call__7 != ((AnsiString)"");
                var call__8 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSMTPServer();
                ManualChk.Checked = call__8 == ((AnsiString)"");
            }
            else
            {
                AutomatedChk.Checked = sendEmailGameEventLog.FMode == 0;
                ManualChk.Checked = sendEmailGameEventLog.FMode == 1;
            }

            Dlg.Visible = true;
        }

        // AoWSendEmailGameDlg.pas:102
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWSendEmailGameDlg.pas:107
        public void OKBtnClick()
        {
            if (AutomatedChk.Checked)
            {
                FSendEvent.FMode = 0;
            }

            if (ManualChk.Checked)
            {
                FSendEvent.FMode = 1;
            }

            Dlg.Visible = false;
        }

        // AoWSendEmailGameDlg.pas:114
        public void ExitBtnClick()
        {
            FSendEvent.FMode = 2;
            Dlg.Visible = false;
        }

        // AoWSendEmailGameDlg.pas:120
        public void EmailAddressEditChange()
        {
            var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FSendEvent.FSendPlayer);
            call__9.FPBEMSettings.FEmailGame.FEmailAddress = EmailAddressEdit.Text;
        }

        // AoWSendEmailGameDlg.pas:125
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWSendEmailGameDlg
    {
    }
}
