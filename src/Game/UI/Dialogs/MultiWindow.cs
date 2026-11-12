// Translated from AOWMultiWin.pas.
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// pending map, menu animation height
public partial class MultiWindow : UIModule
{
    public MultiWindow(UIContext ui) : base(ui)
    { }

    public Panel JoinPanel;
    public ListBox GamesList;
    public Button JoinButton;
    public Panel MultiPanel;
    public Panel GamePanel;
    public Button TcpButton;
    public Button IpxButton;
    public Panel TcpPanel;
    public Edit TcpNameEdit;
    public Edit IPAddressEdit;
    public Panel LanPanel;
    public Panel BottomPanel;
    public Edit IpxNameEdit;
    public Panel EmailPanel;
    private string _pendingMap = "";
    private int _menuHeight;

    // AOWMultiWin.pas:362
    public int ConnectProvider(ConnectionType kind)
    {
        int I = 0;
        int found = -1;
        int result = 0;
        while (I < UI.SetupEvents.DirectPlay.ConnectionItems.Count)
        {
            if (UI.SetupEvents.DirectPlay.ConnectionItems[I].FType == kind)
            {
                found = I;
                I = UI.SetupEvents.DirectPlay.ConnectionItems.Count;
            }

            I++;
        }

        if (found > -1)
        {
            UI.SetupEvents.DirectPlay.TCPPort = IPAddressEdit.Text;
            result = UI.SetupEvents.DirectPlay.Connect(found);
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
        if (IsEmpty(UI.Engine.Registry.GetLocalEmailAddress()))
        {
            MessageDialog.ResetMessageDialog(UI.MessageDialog);
            UI.MessageDialog.MessageMemo.Items.Add("");
            var pleaseSetupEmailSettingsFirst = UI.Engine.TranslateResourceString(s_pleaseSetupEmailSettingsFirstText);
            UI.MessageDialog.MessageMemo.Items.Add(pleaseSetupEmailSettingsFirst);
            UI.MessageDialog.OKPanel.Visible = true;
            UI.MessageDialog.Window.Visible = true;
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AOWMultiWin.pas:103
    public void BackButtonClick()
    {
        UI.General.PlayButtonSound(0);
        if (UI.SetupEvents.DirectPlay.Connected)
        {
            UI.SetupEvents.DirectPlay.DisConnect();
        }

        UI.TitleWindow.MainWindow.Visible = false;
        MultiPanel.Visible = false;
        WindowActions.ShowTitleScene(UI.TitleScene, true);
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
        if (GamesList.Selected > -1)
        {
            JoinButton.Visible = true;
        }
        else
        {
            JoinButton.Visible = false;
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
        UI.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        TcpPanel.Visible = true;
    }

    // AOWMultiWin.pas:183
    public void IPXBtnClick()
    {
        UI.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        LanPanel.Visible = true;
    }

    // AOWMultiWin.pas:191
    public void CancelTCPClick()
    {
        UI.General.PlayButtonSound(0);
        GamePanel.Visible = true;
        TcpPanel.Visible = false;
        LanPanel.Visible = false;
    }

    // AOWMultiWin.pas:200
    public void FindClick()
    {
        UI.General.PlayButtonSound(0);
        if (ConnectProvider(ConnectionType.TCP) == 0)
        {
            UI.SetupEvents.DirectPlay.PlayerName = TcpNameEdit.Text;
            TcpPanel.Visible = false;
            JoinPanel.Visible = true;
            GamesList.Items.Clear();
            GamesList.Sized = false;
        }
    }

    // AOWMultiWin.pas:215
    public void HostClick()
    {
        UI.General.PlayButtonSound(0);
        if (ConnectProvider(ConnectionType.TCP) == 0)
        {
            UI.SetupEvents.DirectPlay.PlayerName = TcpNameEdit.Text;
            UI.SetupEvents.DirectPlay.SessionName = AbilityRules.Format(UI.Engine.TranslateResourceString(s_playerGameFormat), [TcpNameEdit.Text]);
            if (UI.SetupEvents.DirectPlay.CreateSession() == 0)
            {
                MultiPanel.Visible = false;
                UI.SetupEvents.SetupControl.SetupHost(UI.SetupEvents.DirectPlay.PlayerName, 1, UI.SetupEvents.DirectPlay.PlayerID);
                if (!IsEmpty(_pendingMap))
                {
                    UI.SetupEvents.SetupControl.PickMap(_pendingMap);
                }

                UI.ScenarioSetup.Execute(UI.SetupEvents.SetupControl);
            }
        }
    }

    // AOWMultiWin.pas:232
    public void IPXNameChange()
    {
        TcpNameEdit.Text = IpxNameEdit.Text;
    }

    // AOWMultiWin.pas:238
    public void TCPNameChange()
    {
        IpxNameEdit.Text = TcpNameEdit.Text;
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
        UI.General.PlayButtonSound(0);
        MultiPanel.Visible = false;
        UI.SetupEvents.SetupControl.SetupHost("", 0, 0);
        if (!IsEmpty(_pendingMap))
        {
            UI.SetupEvents.SetupControl.PickMap(_pendingMap);
        }

        UI.ScenarioSetup.Execute(UI.SetupEvents.SetupControl);
    }

    // AOWMultiWin.pas:285
    public void ModemButtonClick()
    {
        UI.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:291
    public void DirectButtonClick()
    {
        UI.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:297
    public void EmailBtnClick()
    {
        UI.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        EmailPanel.Visible = true;
    }

    // AOWMultiWin.pas:305
    public void CancelOKButClick()
    {
        UI.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:311
    public void CancelPBEMBtnClick()
    {
        GamePanel.Visible = true;
        EmailPanel.Visible = false;
    }

    // AOWMultiWin.pas:318
    public void CheckEmailBtnClick()
    {
        if (ValidateEmail())
        {
            UI.Pop3Dialog.ShowDialog();
        }
    }

    // AOWMultiWin.pas:323
    public void NewPBEMGameBtnClick()
    {
        if (ValidateEmail())
        {
            MultiPanel.Visible = false;
            UI.SetupEvents.SetupControl.SetupHost("", 2, 0);
            if (!IsEmpty(_pendingMap))
            {
                UI.SetupEvents.SetupControl.PickMap(_pendingMap);
            }

            UI.ScenarioSetup.Execute(UI.SetupEvents.SetupControl);
        }
    }

    // AOWMultiWin.pas:334
    public void LoadPBEMGameBtnClick()
    {
        UI.FileDialogWindow.Mode = 0;
        UI.FileDialogWindow.Window.Visible = true;
        SetCurrentDir(UI.Engine.StartupDirectory + "EmailIn\\");
        UI.FileDialogWindow.FindFiles();
    }

    // AOWMultiWin.pas:343
    public void EmailSettingsBtnClick()
    {
        EmailSettingsDialog.ShowEmailSettings(UI.EmailSettingsDialog);
    }

    // AOWMultiWin.pas:349
    public void HeatNetBtnClick()
    {
        // The full version starts GOTOHEAT.EXE from the game folder and ends the game when it
        // starts. The installed game has no GOTOHEAT.EXE, so the original does nothing there.
        if (UI.Engine.Demo)
        {
            UI.MessageDialog.ShowDemoMessage();
        }
    }

    private static readonly string s_pleaseSetupEmailSettingsFirstText = "Please setup email settings first!";

    private static readonly string s_playerGameFormat = "%s's Game";

    internal const string NetworkPlayNotAvailableText = "Network play is not available in this version.";

    internal static void ShowMultiplayer(MultiWindow dialog, string name)
    {
        /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
        throw new NotImplementedException();
    }

}
