// Translated from AOWMultiWin.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// pending map, menu animation height
public partial class MultiWindow : DataModule
{
    public MultiWindow(Component owner) : base(owner)
    { }

    public Panel JoinPnl;
    public ListBox GamesLB;
    public VerticalScrollBar GLBvsc;
    public Button JoinBtn;
    public Panel MultiPnl;
    public Button BackBtn;
    public Panel GamePanel;
    public Button TCPBtn;
    public Button IPXBtn;
    public Button SingleBtn;
    public Button EmailBtn;
    public Panel TCPPnl;
    public Label TCPNameLbl;
    public Label Single;
    public Label Address;
    public Button CancelTCP;
    public Button Host;
    public Button Find;
    public Label GameLbl;
    public Edit TCPName;
    public Edit IPEdit;
    public Panel LanPnl;
    public Button LANFind;
    public Label JoinLbl;
    public Panel BottomPnl;
    public Button BackButn;
    public Panel MsgPnl;
    public Button CancelOKBut;
    public Memo MessageMemo;
    public Button LANCancel;
    public Label Label1;
    public Label LANLbl;
    public Edit IPXName;
    public Button LANHost;
    public Panel PBEMPnl;
    public Button CancelPBEMBtn;
    public Button CheckEmailBtn;
    public Button NewPBEMGameBtn;
    public Button LoadPBEMGameBtn;
    public Button EmailSettingsBtn;
    public Button HeatNetBtn;
    public IvTranslator Translator;
    private string _pendingMap = "";
    private int _menuHeight;

    // AOWMultiWin.pas:362
    public int ConnectProvider(ConnectionType kind)
    {
        int I = 0;
        int found = -1;
        int result = 0;
        while (I < DialogHosts.SetupEvents.DPlay.ConnectionItems.Count)
        {
            if (((GDConnection)DialogHosts.SetupEvents.DPlay.ConnectionItems[I]).FType == kind)
            {
                found = I;
                I = DialogHosts.SetupEvents.DPlay.ConnectionItems.Count;
            }

            I++;
        }

        if (found > -1)
        {
            DialogHosts.SetupEvents.DPlay.TCPPort = IPEdit.Text;
            result = DialogHosts.SetupEvents.DPlay.Connect(found);
            // The port's own message. The offline component never connects, and the caller
            // leaves the window as it is when the connection fails.
            if (result != 0)
            {
                RtlUnit.ShowMessage(NetworkPlayNotAvailableText);
            }
        }

        return result;
    }

    // AOWMultiWin.pas:385
    public bool ValidateEmail()
    {
        bool result;
        if (IsEmpty(GameTables.GameEngine.Registry.GetLocalEmailAddress()))
        {
            MessageDialog.ResetMessageDialog(DialogHosts.MessageDialog);
            DialogHosts.MessageDialog.MessageMemo.Items.Add("");
            var pleaseSetupEmailSettingsFirst = AbilityRules.TranslateResourceString(s_pleaseSetupEmailSettingsFirstText);
            DialogHosts.MessageDialog.MessageMemo.Items.Add(pleaseSetupEmailSettingsFirst);
            DialogHosts.MessageDialog.OKPnl.Visible = true;
            DialogHosts.MessageDialog.Dlg.Visible = true;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AOWMultiWin.pas:103
    public void CloseBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.SetupEvents.DPlay.Connected)
        {
            DialogHosts.SetupEvents.DPlay.DisConnect();
        }

        DialogHosts.TitleWindow.TitleWin.Visible = false;
        MultiPnl.Visible = false;
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
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
        DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        TCPPnl.Visible = true;
    }

    // AOWMultiWin.pas:183
    public void IPXBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        LanPnl.Visible = true;
    }

    // AOWMultiWin.pas:191
    public void CancelTCPClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = true;
        TCPPnl.Visible = false;
        LanPnl.Visible = false;
    }

    // AOWMultiWin.pas:200
    public void FindClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (ConnectProvider(ConnectionType.TCP) == 0)
        {
            DialogHosts.SetupEvents.DPlay.PlayerName = TCPName.Text;
            SessionRefreshDelay = 0;
            TCPPnl.Visible = false;
            JoinPnl.Visible = true;
            GamesLB.Items.Clear();
            GamesLB.Sized = false;
        }
    }

    // AOWMultiWin.pas:215
    public void HostClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (ConnectProvider(ConnectionType.TCP) == 0)
        {
            DialogHosts.SetupEvents.DPlay.PlayerName = TCPName.Text;
            DialogHosts.SetupEvents.DPlay.SessionName = AbilityRules.Format(AbilityRules.TranslateResourceString(s_playerGameFormat), [TCPName.Text]);
            if (DialogHosts.SetupEvents.DPlay.CreateSession() == 0)
            {
                MultiPnl.Visible = false;
                DialogHosts.SetupEvents.SetupControl.SetupHost(DialogHosts.SetupEvents.DPlay.PlayerName, 1, DialogHosts.SetupEvents.DPlay.PlayerID);
                if (!IsEmpty(_pendingMap))
                {
                    DialogHosts.SetupEvents.SetupControl.PickMap(_pendingMap);
                }

                DialogHosts.ScenarioSetup.Execute(DialogHosts.SetupEvents.SetupControl);
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
        DialogHosts.General.PlayButtonSound(0);
        MultiPnl.Visible = false;
        DialogHosts.SetupEvents.SetupControl.SetupHost("", 0, 0);
        if (!IsEmpty(_pendingMap))
        {
            DialogHosts.SetupEvents.SetupControl.PickMap(_pendingMap);
        }

        DialogHosts.ScenarioSetup.Execute(DialogHosts.SetupEvents.SetupControl);
    }

    // AOWMultiWin.pas:285
    public void ModemButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:291
    public void DirectButtonClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:297
    public void EmailBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        PBEMPnl.Visible = true;
    }

    // AOWMultiWin.pas:305
    public void CancelOKButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
            DialogHosts.Pop3Dialog.ShowDialog();
        }
    }

    // AOWMultiWin.pas:323
    public void NewPBEMGameBtnClick()
    {
        if (ValidateEmail())
        {
            MultiPnl.Visible = false;
            DialogHosts.SetupEvents.SetupControl.SetupHost("", 2, 0);
            if (!IsEmpty(_pendingMap))
            {
                DialogHosts.SetupEvents.SetupControl.PickMap(_pendingMap);
            }

            DialogHosts.ScenarioSetup.Execute(DialogHosts.SetupEvents.SetupControl);
        }
    }

    // AOWMultiWin.pas:334
    public void LoadPBEMGameBtnClick()
    {
        DialogHosts.FileDialogWindow.Mode = 0;
        DialogHosts.FileDialogWindow.FileDialog.Visible = true;
        SetCurrentDir(GameTables.GameEngine.StartupDirectory + "EmailIn\\");
        DialogHosts.FileDialogWindow.FindFiles();
    }

    // AOWMultiWin.pas:343
    public void EmailSettingsBtnClick()
    {
        EmailSettingsDialog.ShowEmailSettings(DialogHosts.EmailSettingsDialog);
    }

    // AOWMultiWin.pas:349
    public void HeatNetBtnClick()
    {
        // The full version starts GOTOHEAT.EXE from the game folder and ends the game when it
        // starts. The installed game has no GOTOHEAT.EXE, so the original does nothing there.
        if (GameTables.GameEngine.Demo)
        {
            MessageDialog.ShowDemoMessage();
        }
    }

    private static readonly string s_pleaseSetupEmailSettingsFirstText = "Please setup email settings first!";

    private static readonly string s_playerGameFormat = "%s's Game";

    internal static int SessionRefreshDelay { get; private set; }

    internal const string NetworkPlayNotAvailableText = "Network play is not available in this version.";

    internal static void ShowMultiplayer(MultiWindow dialog, string name)
    {
        /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
        throw new NotImplementedException();
    }

}
