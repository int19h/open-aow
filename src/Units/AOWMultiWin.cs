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
// Translated from AOWMultiWin.pas.
using Aow.Rtl.Localize;
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
        while (I < Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.ConnectionItems.Count)
        {
            if (((TGDConnection)Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.ConnectionItems[I]).FType == Kind)
            {
                Found = I;
                I = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.ConnectionItems.Count;
            }

            I++;
        }

        if (Found > -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.TCPPort = IPEdit.Text;
            Result = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.Connect(Found);
            // The port's own message. The offline component never connects, and the caller
            // leaves the window as it is when the connection fails.
            if (Result != 0)
            {
                RtlUnit.ShowMessage(Aow.Game.Ui.Dialogs.TMultiWindow.NetworkPlayNotAvailableText);
            }
        }

        return Result;
    }

    // AOWMultiWin.pas:385
    public bool ValidateEmail()
    {
        bool Result;
        if (IsEmpty(Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetLocalEmailAddress()))
        {
            Aow.Game.Ui.Dialogs.TMessageDlg.ResetMessageDialog(Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add("");
            var pleaseSetupEmailSettingsFirst = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TMultiWindow.PleaseSetupEmailSettingsFirstRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.MessageMemo.Items.Add(pleaseSetupEmailSettingsFirst);
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.OKPnl.Visible = true;
            Aow.Game.Ui.Dialogs.DialogHosts.MessageDlg.Dlg.Visible = true;
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.Connected)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.DisConnect();
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        MultiPnl.Visible = false;
        Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, true);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        TCPPnl.Visible = true;
    }

    // AOWMultiWin.pas:183
    public void IPXBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        LanPnl.Visible = true;
    }

    // AOWMultiWin.pas:191
    public void CancelTCPClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = true;
        TCPPnl.Visible = false;
        LanPnl.Visible = false;
    }

    // AOWMultiWin.pas:200
    public void FindClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (ConnectProvider(TConnectionType.conTCP) == 0)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.PlayerName = TCPName.Text;
            Aow.Game.Ui.Dialogs.TMultiWindow.SessionRefreshDelay = 0;
            TCPPnl.Visible = false;
            JoinPnl.Visible = true;
            GamesLB.Items.Clear();
            GamesLB.FSized = false;
        }
    }

    // AOWMultiWin.pas:215
    public void HostClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (ConnectProvider(TConnectionType.conTCP) == 0)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.PlayerName = TCPName.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.SessionName = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TMultiWindow.XsGameRStr), [TCPName.Text]);
            if (Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.CreateSession() == 0)
            {
                MultiPnl.Visible = false;
                Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.SetupHost(Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.PlayerName, 1, Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.PlayerID);
                if (!IsEmpty(FPendingMap))
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.hPickMap(FPendingMap);
                }

                Aow.Game.Ui.Dialogs.DialogHosts.ScenSetup.Execute(Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        MultiPnl.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.SetupHost("", 0, 0);
        if (!IsEmpty(FPendingMap))
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.hPickMap(FPendingMap);
        }

        Aow.Game.Ui.Dialogs.DialogHosts.ScenSetup.Execute(Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl);
    }

    // AOWMultiWin.pas:285
    public void ModemBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:291
    public void DirectBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AOWMultiWin.pas:297
    public void EmailBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        GamePanel.Visible = false;
        PBEMPnl.Visible = true;
    }

    // AOWMultiWin.pas:305
    public void CancelOKButClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
            Aow.Game.Ui.Dialogs.DialogHosts.POP3Dlg.ShowDialog();
        }
    }

    // AOWMultiWin.pas:323
    public void NewPBEMGameBtnClick()
    {
        if (ValidateEmail())
        {
            MultiPnl.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.SetupHost("", 2, 0);
            if (!IsEmpty(FPendingMap))
            {
                Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl.hPickMap(FPendingMap);
            }

            Aow.Game.Ui.Dialogs.DialogHosts.ScenSetup.Execute(Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.SetupControl);
        }
    }

    // AOWMultiWin.pas:334
    public void LoadPBEMGameBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
        SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "EmailIn\\");
        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FindFiles();
    }

    // AOWMultiWin.pas:343
    public void EmailSettingsBtnClick()
    {
        Aow.Game.Ui.Dialogs.TEmailSettingsDlg.ShowEmailSettings(Aow.Game.Ui.Dialogs.DialogHosts.EmailSettingsDlg);
    }

    // AOWMultiWin.pas:349
    public void HeatNetBtnClick()
    {
        // The full version starts GOTOHEAT.EXE from the game folder and ends the game when it
        // starts. The installed game has no GOTOHEAT.EXE, so the original does nothing there.
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo)
        {
            Aow.Game.Ui.Dialogs.TMessageDlg.ShowDemoMessage();
        }
    }

    private static readonly string PleaseSetupEmailSettingsFirstRStr = "Please setup email settings first!";

    private static readonly string XsGameRStr = "%s's Game";

    internal static int SessionRefreshDelay;

    internal const string NetworkPlayNotAvailableText = "Network play is not available in this version.";

    internal static void ShowMultiplayer(TMultiWindow Dialog, string Name)
    {
        /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
        throw new NotImplementedException();
    }

}
