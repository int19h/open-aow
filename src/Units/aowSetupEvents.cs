// Translated from aowSetupEvents.pas.
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
using Aow.Units.AoWSetup;
using static Aow.Units.AoWSetup.AoWSetup;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;
using Aow.Units.AoWScenSetup;
using static Aow.Units.AoWScenSetup.AoWScenSetup;
using Aow.Units.AoWSyncDlg;
using static Aow.Units.AoWSyncDlg.AoWSyncDlg;
using Aow.Units.AoWMessageDlg;
using static Aow.Units.AoWMessageDlg.AoWMessageDlg;

namespace Aow.Units.aowSetupEvents
{
    public partial class TSetupEvents : Aow.Rtl.TDataModule
    {
        public TSetupEvents(TComponent owner) : base(owner)
        { }

        public Aow.Rtl.TGDPlay DPlay;
        public Aow.Units.AoWSetup.TSetupControl SetupControl;
        public bool FLobbyGame;
        // aowSetupEvents.pas:61
        public void ClearChat()
        {
            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatMemo.Items.Clear();
            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatEdit.Text = (AnsiString)"";
        }

        // aowSetupEvents.pas:67
        public void UpdateChatPlayers()
        {
            int I = default;
            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.PlayerList.Items.Clear();
            var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AllPlayersRStr);
            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.PlayerList.Items.AddObject(call__1, null);
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                {
                    I = 1;
                    int end__2 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count - 1;
                    for (; I <= end__2; I += 1)
                    {
                        bool bool__3 = (I != Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        if (bool__3)
                        {
                            var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                            bool__3 = (call__4.FStatus != Aow.Units.AoWE.TPlayerStatus.psTerminated);
                        }

                        bool bool__5 = bool__3;
                        if (bool__5)
                        {
                            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.Players[I];
                            bool__5 = (call__6.ConnectionMode == Aow.Units.NetworkE.TConnectionMode.cmRemote);
                        }

                        if (bool__5)
                        {
                            var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.Players[I];
                            var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(I);
                            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.PlayerList.Items.AddObject(call__8.FLeader.FHeroName, Aow.Rtl.Rtl.IntToObject((int)call__7.SystemID));
                            Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.PlayerList.Selected = 0;
                        }
                    }
                }
            }
        }

        // aowSetupEvents.pas:84
        public void UpdatePlayerName(Aow.Rtl.AnsiString Name)
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:90
        public void SetupControlStart()
        {
            Aow.Units.E2Globals.E2Globals.SynchronizeDlg.Dlg.Visible = false;
            Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.CloseSetup();
            Aow.Units.AoWWinMan.AoWWinMan.StartGame(Aow.Units.AoWWinMan.AoWWinMan.General);
            if (DPlay.Connected)
            {
                ClearChat();
                UpdateChatPlayers();
            }
        }

        // aowSetupEvents.pas:102
        public void SetupControlSendMap(Aow.Units.AoWSetup.TSetupControl Control, object Data, int Size, int Recipient)
        {
            /* TODO: Implement aowDPlayComp.Players in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:111
        public void DPlayFileStartMessage(int ID, ref Aow.Rtl.AnsiString Name, int Size, ref byte Response)
        {
            Aow.Units.E2Globals.E2Globals.ProgressDialog.StatusLbl.Text = (((AnsiString)"0k of ") + Aow.Rtl.Rtl.IntToStr(Size / 1000)) + AnsiString.FromBytes(107);
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = (AnsiString)"Receiving File";
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Max = Size;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position = 0;
        }

        // aowSetupEvents.pas:119
        public void DPlayFileDoneMessage(int ID, Aow.Rtl.TGDFile FileData)
        {
            /* TODO: Implement aowDPlayComp.FData in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:125
        public void DPlayFileProgressMessage(int ID, int Position, int Size)
        {
            if (Position > Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position)
            {
                Aow.Units.E2Globals.E2Globals.ProgressDialog.StatusLbl.Text = ((Aow.Rtl.Rtl.IntToStr(Position / 1000) + ((AnsiString)"k of ")) + Aow.Rtl.Rtl.IntToStr(Size / 1000)) + AnsiString.FromBytes(107);
                Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position = Position;
            }
        }

        // aowSetupEvents.pas:134
        public void DPlayChatMessage(ref Aow.Rtl.AnsiString Message, ref int ToID, ref int FromID)
        {
            if (Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.ScenSetupPnl.Visible)
            {
                Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.ChatMemo.Items.Add(Message);
                Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.FChatScrollPending = true;
            }
            else
            {
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatMemo.Items.Add(Message);
                Aow.Units.AoWExeForms.AoWExeForms.TBWindow.FChatFlash = 136;
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.FScrollPending = true;
                Aow.Units.Sound.Sound.PlaySample((AnsiString)"SFX\\EVENTS\\CHAT.WAV");
            }
        }

        // aowSetupEvents.pas:150
        public void DPlayMessageTimeout(ref Aow.Rtl.TGDMessage Message)
        {
            /* TODO: Implement aowDPlayComp.FToID in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:170
        public void DPlayEjected()
        {
            if (Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.ScenSetupPnl.Visible)
            {
                Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.ExitGameClick(null);
                Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
                var call__9 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.WarningRStr);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.TitleLbl.Text = call__9;
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.YouWereEjectedByTheHostRStr);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__10);
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
            }
        }

        // aowSetupEvents.pas:184
        public void DPlayPingMessage(int PingMS, int Player)
        {
            if (Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.ScenSetupPnl.Visible)
            {
                Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.PingList.FDrawn = false;
            }
        }

        // aowSetupEvents.pas:189
        public void SetupControlCustomizeLeader(Aow.Units.AoWSetup.TSetupControl Control, Aow.Units.AoWSetup.TCustomizeLeaderControl Leader)
        {
            Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.ScenSetupPnl.Visible = false;
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.Execute(Leader);
        }

        // aowSetupEvents.pas:195
        public void SetupControlLoadMap(Aow.Units.AoWSetup.TSetupControl Control, Aow.Rtl.AnsiString FileName)
        {
            var call__11 = Aow.Rtl.Rtl.ExtractFileName(FileName);
            var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LoadingXRStr);
            var call__13 = Aow.Units.AoWE.AoWE.Format(call__12, [call__11]);
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = call__13;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.StatusLbl.Text = (AnsiString)"";
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position = 0;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Max = 100;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = true;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 5000;
            if (!Control.FSettings.FCustomizeLeaders)
            {
                Aow.Units.Sound.Sound.SongPlayer.Stop(false);
                Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
                Aow.Units.Sound.Sound.FadeMusicVolume(0, 1);
            }

            var call__14 = Control.FStream.Size;
            if (call__14 != 0)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.LoadHSMfromStream(Control.FStream, false, Aow.Units.AoWWinMan.AoWWinMan.General.MapProgress);
                var call__15 = Aow.Rtl.Rtl.ExtractFileName(FileName);
                Aow.Units.AoWE.AoWE.AoWHSMap.FileName = (Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory + ((AnsiString)"Scenario\\")) + call__15;
            }
            else
            {
                Aow.Units.AoWE.AoWE.AoWEngine.LoadHSM(FileName, false, Aow.Units.AoWWinMan.AoWWinMan.General.MapProgress);
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = false;
            Control.MapLoaded();
        }

        // aowSetupEvents.pas:220
        public bool SetupControlSendData(object Data, int Size, int Recipient)
        {
            /* TODO: Implement aowDPlayComp.SendMessage in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:225
        public void SetupControlSettingChanged()
        {
            Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioSetup(Aow.Units.AoWExeForms.AoWExeForms.ScenSetup);
        }

        // aowSetupEvents.pas:228
        public void SetupControlEjectPlayer(Aow.Units.AoWSetup.TSetupControl Control, int RemoteID)
        {
            /* TODO: Implement aowDPlayComp.EjectPlayer in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:231
        public void DPlayMessage(ref object Data, ref int Size, ref int ToID, ref int FromID)
        {
            if (SetupControl.FStatus.Contains(Aow.Units.AoWSetup.TSetupStatusElement.ssBusy))
            {
                SetupControl.DataReceived(Data, Size, FromID);
            }

            if ((Aow.Units.AoWE.AoWE.AoWHSMap != null) && Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msActivated))
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC is Aow.Units.NetworkE.TRemotePCC)
                {
                    ((Aow.Units.NetworkE.TRemotePCC)Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC).DataReceived(Data, Size, FromID);
                }
            }
        }

        // aowSetupEvents.pas:239
        public void SetupControlSynchronize(Aow.Units.AoWSetup.TSetupControl Control)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FAligned = false;
            Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.ScenSetupPnl.Visible = false;
            if (Control.GetRemote())
            {
                Aow.Units.E2Globals.E2Globals.SynchronizeDlg.Dlg.Visible = true;
            }
        }

        // aowSetupEvents.pas:249
        public void SetupControlIncompatibleVersion(Aow.Units.AoWSetup.TSetupControl Control, int Major, int Minor)
        {
            /* TODO: Implement aowDPlayComp.CloseSession in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:263
        public void DPlaySessionLost()
        {
            /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // aowSetupEvents.pas:282
        public void SetupControlSendingMaps()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 5000;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = true;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = (AnsiString)"Transferring Files...";
            Aow.Units.E2Globals.E2Globals.ProgressDialog.StatusLbl.Text = (AnsiString)"Please Wait";
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position = 0;
        }

        // aowSetupEvents.pas:291
        public void DPlayMessageError()
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.PlayerConnectionLost();
            }
        }

        // aowSetupEvents.pas:299
        public void DPlayDestroyPlayer(int ID, int NumPlayers)
        {
            sbyte Player = default;
            if (Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.FSetupControl != null)
            {
                var call__16 = Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.FSetupControl.GetHost();
                if (call__16)
                {
                    Aow.Units.AoWExeForms.AoWExeForms.ScenSetup.FSetupControl.hDisconnectPlayer(ID);
                }
            }

            if ((Aow.Units.AoWE.AoWE.AoWHSMap != null) && (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC != null))
            {
                var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.IndexOfSystemID(ID);
                Player = unchecked((sbyte)call__17);
                if (Player != -1)
                {
                    var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    if (call__18 != null)
                    {
                        var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                        if (call__19.FStatus == Aow.Units.AoWE.TPlayerStatus.psPlaying)
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.PlayerDisconnected(unchecked((sbyte)Player));
                        }

                        UpdateChatPlayers();
                    }
                }
            }
        }
    }

    public static partial class aowSetupEvents
    {
        public static Aow.Units.aowSetupEvents.TSetupEvents SetupEvents;
    }
}
