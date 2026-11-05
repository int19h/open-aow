// Translated from AOWMultiWin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWEdit;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AOWMultiWin
{
    // pending map, menu animation height
    public partial class TMultiWindow : TDataModule
    {
        public TMultiWindow(TComponent owner) : base(owner)
        { }

        public TAOWPanel JoinPnl;
        public TAOWListBox GamesLB;
        public TAOWVScrollBar GLBvsc;
        public TAOWButton JoinBtn;
        public TAOWPanel MultiPnl;
        public TAOWButton BackBtn;
        public TAOWPanel GamePanel;
        public TAOWButton TCPBtn;
        public TAOWButton IPXBtn;
        public TAOWButton SingleBtn;
        public TAOWButton EmailBtn;
        public TAOWPanel TCPPnl;
        public TAOWLabel TCPNameLbl;
        public TAOWLabel Single;
        public TAOWLabel Address;
        public TAOWButton CancelTCP;
        public TAOWButton Host;
        public TAOWButton Find;
        public TAOWLabel GameLbl;
        public TAOWEdit TCPName;
        public TAOWEdit IPEdit;
        public TAOWPanel LanPnl;
        public TAOWButton LANFind;
        public TAOWLabel JoinLbl;
        public TAOWPanel BottomPnl;
        public TAOWButton BackButn;
        public TAOWPanel MsgPnl;
        public TAOWButton CancelOKBut;
        public TAOWMemo MessageMemo;
        public TAOWButton LANCancel;
        public TAOWLabel AOWLabel1;
        public TAOWLabel LANLbl;
        public TAOWEdit IPXName;
        public TAOWButton LANHost;
        public TAOWPanel PBEMPnl;
        public TAOWButton CancelPBEMBtn;
        public TAOWButton CheckEmailBtn;
        public TAOWButton NewPBEMGameBtn;
        public TAOWButton LoadPBEMGameBtn;
        public TAOWButton EmailSettingsBtn;
        public TAOWButton HeatNetBtn;
        public TIvTranslator Translator;
        public AnsiString FPendingMap;
        public int FMenuHeight;
        // AOWMultiWin.pas:362
        public int ConnectProvider(TConnectionType Kind)
        {
            /* TODO: Implement aowDPlayComp.ConnectionItems in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AOWMultiWin.pas:385
        public bool ValidateEmail()
        {
            bool Result = default;
            var call__1 = AoWEngine.FRegistry.GetLocalEmailAddress();
            if (call__1 == (AnsiString)"")
            {
                ResetMessageDialog(MessageDlg);
                MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__2 = TranslateRStr(PleaseSetupEmailSettingsFirstRStr);
                MessageDlg.MessageMemo.Items.Add(call__2);
                MessageDlg.OKPnl.Visible = true;
                MessageDlg.Dlg.Visible = true;
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
            throw new NotImplementedException();
        }

        // AOWMultiWin.pas:142
        public void JoinBtnClick()
        {
            /* TODO: Implement aowDPlayComp.JoinSession in the platform or editor service. */
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        // AOWMultiWin.pas:175
        public void TCPBtnClick()
        {
            General.PlayButtonSound(0);
            GamePanel.Visible = false;
            TCPPnl.Visible = true;
        }

        // AOWMultiWin.pas:183
        public void IPXBtnClick()
        {
            General.PlayButtonSound(0);
            GamePanel.Visible = false;
            LanPnl.Visible = true;
        }

        // AOWMultiWin.pas:191
        public void CancelTCPClick()
        {
            General.PlayButtonSound(0);
            GamePanel.Visible = true;
            TCPPnl.Visible = false;
            LanPnl.Visible = false;
        }

        // AOWMultiWin.pas:200
        public void FindClick()
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AOWMultiWin.pas:215
        public void HostClick(TObject Sender)
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        // AOWMultiWin.pas:259
        public void LANHostClick(TObject Sender)
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AOWMultiWin.pas:276
        public void SingleBtnClick()
        {
            General.PlayButtonSound(0);
            MultiPnl.Visible = false;
            SetupEvents.SetupControl.SetupHost((AnsiString)"", 0, 0);
            if (FPendingMap != (AnsiString)"")
            {
                SetupEvents.SetupControl.hPickMap(FPendingMap);
            }

            ScenSetup.Execute(SetupEvents.SetupControl);
        }

        // AOWMultiWin.pas:285
        public void ModemBtnClick()
        {
            General.PlayButtonSound(0);
        }

        // AOWMultiWin.pas:291
        public void DirectBtnClick()
        {
            General.PlayButtonSound(0);
        }

        // AOWMultiWin.pas:297
        public void EmailBtnClick()
        {
            General.PlayButtonSound(0);
            GamePanel.Visible = false;
            PBEMPnl.Visible = true;
        }

        // AOWMultiWin.pas:305
        public void CancelOKButClick()
        {
            General.PlayButtonSound(0);
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
                POP3Dlg.ShowDialog();
            }
        }

        // AOWMultiWin.pas:323
        public void NewPBEMGameBtnClick()
        {
            if (ValidateEmail())
            {
                MultiPnl.Visible = false;
                SetupEvents.SetupControl.SetupHost((AnsiString)"", 2, 0);
                if (FPendingMap != (AnsiString)"")
                {
                    SetupEvents.SetupControl.hPickMap(FPendingMap);
                }

                ScenSetup.Execute(SetupEvents.SetupControl);
            }
        }

        // AOWMultiWin.pas:334
        public void LoadPBEMGameBtnClick()
        {
            FileDlgWindow.FMode = 0;
            FileDlgWindow.FileDialog.Visible = true;
            SetCurrentDir(AoWEngine.StartupDirectory + (AnsiString)"EmailIn\\");
            FileDlgWindow.FindFiles();
        }

        // AOWMultiWin.pas:343
        public void EmailSettingsBtnClick()
        {
            ShowEmailSettings(EmailSettingsDlg);
        }

        // AOWMultiWin.pas:349
        public void HeatNetBtnClick()
        {
            /* TODO: Implement Forms.Handle in the platform or editor service. */
            throw new NotImplementedException();
        }
    }

    public static partial class AOWMultiWin
    {
        public static int SessionRefreshDelay;
    }
}
