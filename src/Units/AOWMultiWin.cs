// Translated from AOWMultiWin.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.AoWMessageDlg;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;
using Aow.Units.AoWSelFactionWin;
using static Aow.Units.AoWSelFactionWin.AoWSelFactionWin;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWScenSetup;
using static Aow.Units.AoWScenSetup.AoWScenSetup;
using Aow.Units.AoWPOP3Dlg;
using static Aow.Units.AoWPOP3Dlg.AoWPOP3Dlg;

namespace Aow.Units.AOWMultiWin
{
    // pending map, menu animation height
    public partial class TMultiWindow : Aow.Rtl.TDataModule
    {
        public TMultiWindow(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWPanel.TAOWPanel JoinPnl;
        public Aow.Units.AoWListBox.TAOWListBox GamesLB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar GLBvsc;
        public Aow.Units.AoWButton.TAOWButton JoinBtn;
        public Aow.Units.AoWPanel.TAOWPanel MultiPnl;
        public Aow.Units.AoWButton.TAOWButton BackBtn;
        public Aow.Units.AoWPanel.TAOWPanel GamePanel;
        public Aow.Units.AoWButton.TAOWButton TCPBtn;
        public Aow.Units.AoWButton.TAOWButton IPXBtn;
        public Aow.Units.AoWButton.TAOWButton SingleBtn;
        public Aow.Units.AoWButton.TAOWButton EmailBtn;
        public Aow.Units.AoWPanel.TAOWPanel TCPPnl;
        public Aow.Units.AoWLabel.TAOWLabel TCPNameLbl;
        public Aow.Units.AoWLabel.TAOWLabel Single;
        public Aow.Units.AoWLabel.TAOWLabel Address;
        public Aow.Units.AoWButton.TAOWButton CancelTCP;
        public Aow.Units.AoWButton.TAOWButton Host;
        public Aow.Units.AoWButton.TAOWButton Find;
        public Aow.Units.AoWLabel.TAOWLabel GameLbl;
        public Aow.Units.AoWEdit.TAOWEdit TCPName;
        public Aow.Units.AoWEdit.TAOWEdit IPEdit;
        public Aow.Units.AoWPanel.TAOWPanel LanPnl;
        public Aow.Units.AoWButton.TAOWButton LANFind;
        public Aow.Units.AoWLabel.TAOWLabel JoinLbl;
        public Aow.Units.AoWPanel.TAOWPanel BottomPnl;
        public Aow.Units.AoWButton.TAOWButton BackButn;
        public Aow.Units.AoWPanel.TAOWPanel MsgPnl;
        public Aow.Units.AoWButton.TAOWButton CancelOKBut;
        public Aow.Units.AoWMemo.TAOWMemo MessageMemo;
        public Aow.Units.AoWButton.TAOWButton LANCancel;
        public Aow.Units.AoWLabel.TAOWLabel AOWLabel1;
        public Aow.Units.AoWLabel.TAOWLabel LANLbl;
        public Aow.Units.AoWEdit.TAOWEdit IPXName;
        public Aow.Units.AoWButton.TAOWButton LANHost;
        public Aow.Units.AoWPanel.TAOWPanel PBEMPnl;
        public Aow.Units.AoWButton.TAOWButton CancelPBEMBtn;
        public Aow.Units.AoWButton.TAOWButton CheckEmailBtn;
        public Aow.Units.AoWButton.TAOWButton NewPBEMGameBtn;
        public Aow.Units.AoWButton.TAOWButton LoadPBEMGameBtn;
        public Aow.Units.AoWButton.TAOWButton EmailSettingsBtn;
        public Aow.Units.AoWButton.TAOWButton HeatNetBtn;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Rtl.AnsiString FPendingMap;
        public int FMenuHeight;
        // AOWMultiWin.pas:362
        public int ConnectProvider(Aow.Rtl.TConnectionType Kind)
        {
            /* TODO: Implement aowDPlayComp.ConnectionItems in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:385
        public bool ValidateEmail()
        {
            bool Result = default;
            var call__1 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetLocalEmailAddress();
            if (call__1 == ((AnsiString)""))
            {
                Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PleaseSetupEmailSettingsFirstRStr);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__2);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // AOWMultiWin.pas:103
        public void CloseBtnClick()
        {
            /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:142
        public void JoinBtnClick()
        {
            /* TODO: Implement aowDPlayComp.JoinSession in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:157
        public void GamesLBChange()
        {
            if (GamesLB.Selected > -1)
            {
                JoinBtn.Visible = true;
            }
            else
            {
                JoinBtn.Visible = false;
            }
        }

        // AOWMultiWin.pas:163
        public void BackBtnClick()
        {
            /* TODO: Implement aowDPlayComp.GetSessions in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:175
        public void TCPBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            GamePanel.Visible = false;
            TCPPnl.Visible = true;
        }

        // AOWMultiWin.pas:183
        public void IPXBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            GamePanel.Visible = false;
            LanPnl.Visible = true;
        }

        // AOWMultiWin.pas:191
        public void CancelTCPClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            GamePanel.Visible = true;
            TCPPnl.Visible = false;
            LanPnl.Visible = false;
        }

        // AOWMultiWin.pas:200
        public void FindClick()
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:215
        public void HostClick(Aow.Rtl.TObject Sender)
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:232
        public void IPXNameChange()
        {
            TCPName.Text = IPXName.Text;
        }

        // AOWMultiWin.pas:238
        public void TCPNameChange()
        {
            IPXName.Text = TCPName.Text;
        }

        // AOWMultiWin.pas:244
        public void LANFindClick()
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:259
        public void LANHostClick(Aow.Rtl.TObject Sender)
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWMultiWin.pas:276
        public void SingleBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            MultiPnl.Visible = false;
            Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.SetupHost((AnsiString)"", 0, 0);
            if (FPendingMap != ((AnsiString)""))
            {
                Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.hPickMap(FPendingMap);
            }

            Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.Execute(Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl);
        }

        // AOWMultiWin.pas:285
        public void ModemBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AOWMultiWin.pas:291
        public void DirectBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AOWMultiWin.pas:297
        public void EmailBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            GamePanel.Visible = false;
            PBEMPnl.Visible = true;
        }

        // AOWMultiWin.pas:305
        public void CancelOKButClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AOWMultiWin.pas:311
        public void CancelPBEMBtnClick()
        {
            GamePanel.Visible = true;
            PBEMPnl.Visible = false;
        }

        // AOWMultiWin.pas:318
        public void CheckEmailBtnClick()
        {
            if (ValidateEmail())
            {
                Aow.Units.E3Globals.E3Globals.POP3Dlg.ShowDialog();
            }
        }

        // AOWMultiWin.pas:323
        public void NewPBEMGameBtnClick()
        {
            if (ValidateEmail())
            {
                MultiPnl.Visible = false;
                Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.SetupHost((AnsiString)"", 2, 0);
                if (FPendingMap != ((AnsiString)""))
                {
                    Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl.hPickMap(FPendingMap);
                }

                Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.Execute(Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.SetupControl);
            }
        }

        // AOWMultiWin.pas:334
        public void LoadPBEMGameBtnClick()
        {
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 0;
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
            Aow.Rtl.Rtl.SetCurrentDir(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"EmailIn\\"));
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FindFiles();
        }

        // AOWMultiWin.pas:343
        public void EmailSettingsBtnClick()
        {
            Aow.Units.E3Globals.E3Globals.ShowEmailSettings(Aow.Units.E3Globals.E3Globals.EmailSettingsDlg);
        }

        // AOWMultiWin.pas:349
        public void HeatNetBtnClick()
        {
            /* TODO: Implement Forms.Handle in the platform or editor service. */
            throw new System.NotImplementedException();
        }
    }

    public static partial class AOWMultiWin
    {
        public static int SessionRefreshDelay;
    }
}
