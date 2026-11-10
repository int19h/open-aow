// Translated from AOWMultiWin.pas.
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
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
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
// Translated from AOWMultiWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// pending map, menu animation height
public partial class MultiWindow : DataModule
{
    public MultiWindow(Component owner) : base(owner)
    { }

    public AOWPanel JoinPnl;
    public AOWListBox GamesLB;
    public AOWVScrollBar GLBvsc;
    public AOWButton JoinBtn;
    public AOWPanel MultiPnl;
    public AOWButton BackBtn;
    public AOWPanel GamePanel;
    public AOWButton TCPBtn;
    public AOWButton IPXBtn;
    public AOWButton SingleBtn;
    public AOWButton EmailBtn;
    public AOWPanel TCPPnl;
    public AOWLabel TCPNameLbl;
    public AOWLabel Single;
    public AOWLabel Address;
    public AOWButton CancelTCP;
    public AOWButton Host;
    public AOWButton Find;
    public AOWLabel GameLbl;
    public AOWEdit TCPName;
    public AOWEdit IPEdit;
    public AOWPanel LanPnl;
    public AOWButton LANFind;
    public AOWLabel JoinLbl;
    public AOWPanel BottomPnl;
    public AOWButton BackButn;
    public AOWPanel MsgPnl;
    public AOWButton CancelOKBut;
    public AOWMemo MessageMemo;
    public AOWButton LANCancel;
    public AOWLabel AOWLabel1;
    public AOWLabel LANLbl;
    public AOWEdit IPXName;
    public AOWButton LANHost;
    public AOWPanel PBEMPnl;
    public AOWButton CancelPBEMBtn;
    public AOWButton CheckEmailBtn;
    public AOWButton NewPBEMGameBtn;
    public AOWButton LoadPBEMGameBtn;
    public AOWButton EmailSettingsBtn;
    public AOWButton HeatNetBtn;
    public IvTranslator Translator;
    public string FPendingMap = "";
    public int FMenuHeight;

    // AOWMultiWin.pas:362
    public int ConnectProvider(ConnectionType Kind)
    {
        int I = 0;
        int Found = -1;
        int Result = 0;
        while (I < DialogHosts.SetupEvents.DPlay.ConnectionItems.Count)
        {
            if (((GDConnection)DialogHosts.SetupEvents.DPlay.ConnectionItems[I]).FType == Kind)
            {
                Found = I;
                I = DialogHosts.SetupEvents.DPlay.ConnectionItems.Count;
            }

            I++;
        }

        if (Found > -1)
        {
            DialogHosts.SetupEvents.DPlay.TCPPort = IPEdit.Text;
            Result = DialogHosts.SetupEvents.DPlay.Connect(Found);
            // The port's own message. The offline component never connects, and the caller
            // leaves the window as it is when the connection fails.
            if (Result != 0)
            {
                RtlUnit.ShowMessage(NetworkPlayNotAvailableText);
            }
        }

        return Result;
    }

    // AOWMultiWin.pas:385
    public bool ValidateEmail()
    {
        bool Result;
        if (IsEmpty(GameTables.AoWEngine.FRegistry.GetLocalEmailAddress()))
        {
            MessageDlg.ResetMessageDialog(DialogHosts.MessageDlg);
            DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var pleaseSetupEmailSettingsFirst = AbilityRules.TranslateRStr(PleaseSetupEmailSettingsFirstRStr);
            DialogHosts.MessageDlg.MessageMemo.Items.Add(pleaseSetupEmailSettingsFirst);
            DialogHosts.MessageDlg.OKPnl.Visible = true;
            DialogHosts.MessageDlg.Dlg.Visible = true;
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
            GamesLB.FSized = false;
        }
    }

    // AOWMultiWin.pas:215
    public void HostClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (ConnectProvider(ConnectionType.TCP) == 0)
        {
            DialogHosts.SetupEvents.DPlay.PlayerName = TCPName.Text;
            DialogHosts.SetupEvents.DPlay.SessionName = AbilityRules.Format(AbilityRules.TranslateRStr(XsGameRStr), [TCPName.Text]);
            if (DialogHosts.SetupEvents.DPlay.CreateSession() == 0)
            {
                MultiPnl.Visible = false;
                DialogHosts.SetupEvents.SetupControl.SetupHost(DialogHosts.SetupEvents.DPlay.PlayerName, 1, DialogHosts.SetupEvents.DPlay.PlayerID);
                if (!IsEmpty(FPendingMap))
                {
                    DialogHosts.SetupEvents.SetupControl.hPickMap(FPendingMap);
                }

                DialogHosts.ScenSetup.Execute(DialogHosts.SetupEvents.SetupControl);
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
        if (!IsEmpty(FPendingMap))
        {
            DialogHosts.SetupEvents.SetupControl.hPickMap(FPendingMap);
        }

        DialogHosts.ScenSetup.Execute(DialogHosts.SetupEvents.SetupControl);
    }

    // AOWMultiWin.pas:285
    public void ModemBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:291
    public void DirectBtnClick()
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
            DialogHosts.POP3Dlg.ShowDialog();
        }
    }

    // AOWMultiWin.pas:323
    public void NewPBEMGameBtnClick()
    {
        if (ValidateEmail())
        {
            MultiPnl.Visible = false;
            DialogHosts.SetupEvents.SetupControl.SetupHost("", 2, 0);
            if (!IsEmpty(FPendingMap))
            {
                DialogHosts.SetupEvents.SetupControl.hPickMap(FPendingMap);
            }

            DialogHosts.ScenSetup.Execute(DialogHosts.SetupEvents.SetupControl);
        }
    }

    // AOWMultiWin.pas:334
    public void LoadPBEMGameBtnClick()
    {
        DialogHosts.FileDlgWindow.FMode = 0;
        DialogHosts.FileDlgWindow.FileDialog.Visible = true;
        SetCurrentDir(GameTables.AoWEngine.StartupDirectory + "EmailIn\\");
        DialogHosts.FileDlgWindow.FindFiles();
    }

    // AOWMultiWin.pas:343
    public void EmailSettingsBtnClick()
    {
        EmailSettingsDlg.ShowEmailSettings(DialogHosts.EmailSettingsDlg);
    }

    // AOWMultiWin.pas:349
    public void HeatNetBtnClick()
    {
        // The full version starts GOTOHEAT.EXE from the game folder and ends the game when it
        // starts. The installed game has no GOTOHEAT.EXE, so the original does nothing there.
        if (GameTables.AoWEngine.FDemo)
        {
            MessageDlg.ShowDemoMessage();
        }
    }

    private static readonly string PleaseSetupEmailSettingsFirstRStr = "Please setup email settings first!";

    private static readonly string XsGameRStr = "%s's Game";

    internal static int SessionRefreshDelay;

    internal const string NetworkPlayNotAvailableText = "Network play is not available in this version.";

    internal static void ShowMultiplayer(MultiWindow Dialog, string Name)
    {
        /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
        throw new NotImplementedException();
    }

}
