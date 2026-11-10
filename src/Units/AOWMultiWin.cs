// Translated from AOWMultiWin.pas.
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AOWMultiWin.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWExeFormsUnit;
using static Aow.Game.Ui.Dialogs.AoWFileDialogUnit;
using static Aow.Game.Ui.Dialogs.AoWMessageDlgUnit;
using static Aow.Game.Ui.Dialogs.aowSetupEventsUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Game.Ui.Dialogs.E3GlobalsUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
    public string FPendingMap = "";
    public int FMenuHeight;

    // AOWMultiWin.pas:362
    public int ConnectProvider(TConnectionType Kind)
    {
        int I = 0;
        int Found = -1;
        int Result = 0;
        while (I < SetupEvents.DPlay.ConnectionItems.Count)
        {
            if (((TGDConnection)SetupEvents.DPlay.ConnectionItems[I]).FType == Kind)
            {
                Found = I;
                I = SetupEvents.DPlay.ConnectionItems.Count;
            }

            I++;
        }

        if (Found > -1)
        {
            SetupEvents.DPlay.TCPPort = IPEdit.Text;
            Result = SetupEvents.DPlay.Connect(Found);
            // The port's own message. The offline component never connects, and the caller
            // leaves the window as it is when the connection fails.
            if (Result != 0)
            {
                RtlUnit.ShowMessage(AOWMultiWinUnit.NetworkPlayNotAvailableText);
            }
        }

        return Result;
    }

    // AOWMultiWin.pas:385
    public bool ValidateEmail()
    {
        bool Result;
        if (IsEmpty(AoWEngine.FRegistry.GetLocalEmailAddress()))
        {
            ResetMessageDialog(MessageDlg);
            MessageDlg.MessageMemo.Items.Add("");
            var pleaseSetupEmailSettingsFirst = TranslateRStr(PleaseSetupEmailSettingsFirstRStr);
            MessageDlg.MessageMemo.Items.Add(pleaseSetupEmailSettingsFirst);
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
        General.PlayButtonSound(0);
        if (SetupEvents.DPlay.Connected)
        {
            SetupEvents.DPlay.DisConnect();
        }

        E2GlobalsUnit.TitleWindow.TitleWin.Visible = false;
        MultiPnl.Visible = false;
        E2GameFlowUnit.ShowTitleScene(E2GlobalsUnit.TitleScene, true);
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
        General.PlayButtonSound(0);
        if (ConnectProvider(TConnectionType.conTCP) == 0)
        {
            SetupEvents.DPlay.PlayerName = TCPName.Text;
            AOWMultiWinUnit.SessionRefreshDelay = 0;
            TCPPnl.Visible = false;
            JoinPnl.Visible = true;
            GamesLB.Items.Clear();
            GamesLB.FSized = false;
        }
    }

    // AOWMultiWin.pas:215
    public void HostClick()
    {
        General.PlayButtonSound(0);
        if (ConnectProvider(TConnectionType.conTCP) == 0)
        {
            SetupEvents.DPlay.PlayerName = TCPName.Text;
            SetupEvents.DPlay.SessionName = Aow.Game.Magic.Abilities.AoWEUnit.Format(TranslateRStr(XsGameRStr), [TCPName.Text]);
            if (SetupEvents.DPlay.CreateSession() == 0)
            {
                MultiPnl.Visible = false;
                SetupEvents.SetupControl.SetupHost(SetupEvents.DPlay.PlayerName, 1, SetupEvents.DPlay.PlayerID);
                if (!IsEmpty(FPendingMap))
                {
                    SetupEvents.SetupControl.hPickMap(FPendingMap);
                }

                ScenSetup.Execute(SetupEvents.SetupControl);
            }
        }
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
    public void LANHostClick()
    {
        /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AOWMultiWin.pas:276
    public void SingleBtnClick()
    {
        General.PlayButtonSound(0);
        MultiPnl.Visible = false;
        SetupEvents.SetupControl.SetupHost("", 0, 0);
        if (!IsEmpty(FPendingMap))
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
            SetupEvents.SetupControl.SetupHost("", 2, 0);
            if (!IsEmpty(FPendingMap))
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
        SetCurrentDir(AoWEngine.StartupDirectory + "EmailIn\\");
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
        // The full version starts GOTOHEAT.EXE from the game folder and ends the game when it
        // starts. The installed game has no GOTOHEAT.EXE, so the original does nothing there.
        if (AoWEngine.FDemo)
        {
            ShowDemoMessage();
        }
    }
}

public static partial class AOWMultiWinUnit
{
    public static int SessionRefreshDelay;
    // The port has no table for its own text, so the multi window holds its one message.
    public const string NetworkPlayNotAvailableText = "Network play is not available in this version.";
}
