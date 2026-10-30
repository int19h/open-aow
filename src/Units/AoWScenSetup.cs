// Translated from AoWScenSetup.pas.
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
using Aow.Units.AoWSetup;
using static Aow.Units.AoWSetup.AoWSetup;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWComboBox;
using static Aow.Units.AoWComboBox.AoWComboBox;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
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
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.AoWEmailGameSettingsDlg;
using static Aow.Units.AoWEmailGameSettingsDlg.AoWEmailGameSettingsDlg;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;

namespace Aow.Units.AoWScenSetup
{
    // map-validation delay. setup control. ping delay. chat scroll pending
    public partial class TScenSetup : Aow.Rtl.TDataModule
    {
        public TScenSetup(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWMemo.TAOWMemo ChatMemo;
        public Aow.Units.AoWPanel.TAOWPanel ChatPnl;
        public Aow.Units.AoWVScroll.TAOWVScrollBar ChatSB;
        public Aow.Units.AoWEdit.TAOWEdit ChatEdit;
        public Aow.Units.AoWPanel.TAOWPanel SidesPnl;
        public Aow.Units.AoWPanel.TAOWPanel SettingsPnl;
        public Aow.Units.AoWListBox.TAOWListBox PlayerList;
        public Aow.Units.AoWPanel.TAOWPanel R1Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R5Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R9Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R2Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R6Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R10Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R3Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R7Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R11Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R4Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R8Pnl;
        public Aow.Units.AoWPanel.TAOWPanel R12Pnl;
        public Aow.Units.AoWButton.TAOWButton ExitGame;
        public Aow.Units.AoWButton.TAOWButton PickScenarioBtn;
        public Aow.Units.AoWButton.TAOWButton StartBtn;
        public Aow.Units.AoWLabel.TAOWLabel MiscLbl;
        public Aow.Units.AoWPanel.TAOWPanel ScenSetupPnl;
        public Aow.Units.AoWImage.TAOWImage R1PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R1Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R1PlayerCB;
        public Aow.Units.AoWPanel.TAOWPanel R1CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel TurnLbl;
        public Aow.Units.AoWLabel.TAOWLabel DurationLbl;
        public Aow.Units.AoWLabel.TAOWLabel TurnsLbl;
        public Aow.Units.AoWLabel.TAOWLabel TurnTimeLbl;
        public Aow.Units.AoWHScroll.TAOWHScrollBar TurnTimeSl;
        public Aow.Units.AoWLabel.TAOWLabel TurnTimeVal;
        public Aow.Units.AoWFrame.TAoWFrame DurFr;
        public Aow.Units.AoWHScroll.TAOWHScrollBar TurnsSl;
        public Aow.Units.AoWLabel.TAOWLabel TurnsVal;
        public Aow.Units.AoWComboBox.TAOWComboBox PlayModeCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar TurnVSB;
        public Aow.Units.AoWLabel.TAOWLabel R1InfoLbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R1BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R1BehaviorSB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R1PlayerSB;
        public Aow.Units.AoWPanel.TAOWPanel R3CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R3Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R3BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R3BehaviorSB;
        public Aow.Units.AoWImage.TAOWImage R3PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R3PlayerLbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R3PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R3PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel R3InfoLbl;
        public Aow.Units.AoWPanel.TAOWPanel R2CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R2Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R2BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R2BehaviorSB;
        public Aow.Units.AoWImage.TAOWImage R2PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R2InfoLbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R2PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R2PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel R2PlayerLbl;
        public Aow.Units.AoWLabel.TAOWLabel R4InfoLbl;
        public Aow.Units.AoWLabel.TAOWLabel R4PlayerLbl;
        public Aow.Units.AoWPanel.TAOWPanel R4CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R4Lbl;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R4BehaviorSB;
        public Aow.Units.AoWComboBox.TAOWComboBox R4PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R4PlayerSB;
        public Aow.Units.AoWImage.TAOWImage R4PlayerIcon;
        public Aow.Units.AoWImage.TAOWImage R5PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R5Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R5PlayerCB;
        public Aow.Units.AoWPanel.TAOWPanel R5CenterPnl;
        public Aow.Units.AoWComboBox.TAOWComboBox R5BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R5BehaviorSB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R5PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel R5InfoLbl;
        public Aow.Units.AoWLabel.TAOWLabel R5PlayerLbl;
        public Aow.Units.AoWPanel.TAOWPanel R6CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R6Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R6BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R6BehaviorSB;
        public Aow.Units.AoWImage.TAOWImage R6PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R6PlayerLbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R6PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R6PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel R6InfoLbl;
        public Aow.Units.AoWPanel.TAOWPanel R7CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R7Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R7BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R7BehaviorSB;
        public Aow.Units.AoWImage.TAOWImage R7PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R7PlayerLbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R7PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R7PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel R7InfoLbl;
        public Aow.Units.AoWLabel.TAOWLabel R8InfoLbl;
        public Aow.Units.AoWLabel.TAOWLabel R8PlayerLbl;
        public Aow.Units.AoWPanel.TAOWPanel R8CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R8Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R8BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R8BehaviorSB;
        public Aow.Units.AoWComboBox.TAOWComboBox R8PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R8PlayerSB;
        public Aow.Units.AoWImage.TAOWImage R8PlayerIcon;
        public Aow.Units.AoWImage.TAOWImage R9PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R9Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R9PlayerCB;
        public Aow.Units.AoWPanel.TAOWPanel R9CenterPnl;
        public Aow.Units.AoWComboBox.TAOWComboBox R9BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R9BehaviorSB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R9PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel R9InfoLbl;
        public Aow.Units.AoWLabel.TAOWLabel R9PlayerLbl;
        public Aow.Units.AoWPanel.TAOWPanel R10CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R10Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R10BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R10BehaviorSB;
        public Aow.Units.AoWImage.TAOWImage R10PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R10PlayerLbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R10PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar s10sb2;
        public Aow.Units.AoWLabel.TAOWLabel R10InfoLbl;
        public Aow.Units.AoWPanel.TAOWPanel R11CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R11Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R11BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R11BehaviorSB;
        public Aow.Units.AoWImage.TAOWImage R11PlayerIcon;
        public Aow.Units.AoWLabel.TAOWLabel R11PlayerLbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R11PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R11PlayerSB;
        public Aow.Units.AoWLabel.TAOWLabel R11InfoLbl;
        public Aow.Units.AoWLabel.TAOWLabel R12InfoLbl;
        public Aow.Units.AoWLabel.TAOWLabel R12PlayerLbl;
        public Aow.Units.AoWPanel.TAOWPanel R12CenterPnl;
        public Aow.Units.AoWLabel.TAOWLabel R12Lbl;
        public Aow.Units.AoWComboBox.TAOWComboBox R12BehaviorCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R12BehaviorSB;
        public Aow.Units.AoWComboBox.TAOWComboBox R12PlayerCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar R12PlayerSB;
        public Aow.Units.AoWImage.TAOWImage R12PlayerIcon;
        public Aow.Units.AoWComboBox.TAOWComboBox PreferredRaceCB;
        public Aow.Units.AoWVScroll.TAOWVScrollBar PreferredRaceSB;
        public Aow.Units.AoWLabel.TAOWLabel PreferredRaceLbl;
        public Aow.Units.AoWFrame.TAoWFrame LFr;
        public Aow.Units.AoWCheckBox.TAOWCheckBox CustomizeLeaders;
        public Aow.Units.AoWLabel.TAOWLabel CustomizeLeadersLbl;
        public Aow.Units.AoWButton.TAOWButton EjectBtn;
        public Aow.Units.AoWComboBox.TAOWComboBox R4BehaviorCB;
        public Aow.Units.AoWPanel.TAOWPanel R1FogPnl;
        public Aow.Units.AoWLabel.TAOWLabel R1PlayerLbl;
        public Aow.Units.AoWPanel.TAOWPanel R2FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R3FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R4FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R8FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R7FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R6FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R5FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R12FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R11FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R10FogPnl;
        public Aow.Units.AoWPanel.TAOWPanel R9FogPnl;
        public Aow.Units.AoWButton.TAOWButton PreferredRaceSort;
        public Aow.Units.AoWButton.TAOWButton PlayerListSort;
        public Aow.Units.AoWPanel.TAOWPanel ConnectionPnl;
        public Aow.Units.AoWListBox.TAOWListBox PreferredRaceList;
        public Aow.Units.AoWButton.TAOWButton AOWButton1;
        public Aow.Units.AoWListBox.TAOWListBox PingList;
        public Aow.Units.AoWVScroll.TAOWVScrollBar PlayerListSB;
        public Aow.Units.AoWPanel.TAOWPanel SettingsFogPnl;
        public Aow.Units.AoWLabel.TAOWLabel ExplorationLbl;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Exploration;
        public Aow.Units.AoWLabel.TAOWLabel ScenarioLbl;
        public Aow.Units.AoWLabel.TAOWLabel HostNameLbl;
        public Aow.Units.AoWLabel.TAOWLabel PlayerNameLbl;
        public Aow.Units.AoWLabel.TAOWLabel FileNameLbl;
        public Aow.Units.AoWButton.TAOWButton MakeHostBtn;
        public Aow.Units.AoWPanel.TAOWPanel RestartSettingsFogPnl;
        public Aow.Units.AoWImage.TAOWImage ChatFogPnl;
        public Aow.Units.AoWCheckBox.TAOWCheckBox LeadersOnMapCB;
        public Aow.Units.AoWLabel.TAOWLabel AOWLabel1;
        public Aow.Units.AoWCheckBox.TAOWCheckBox AlliedVictoryCB;
        public Aow.Units.AoWLabel.TAOWLabel AlliedVictoryLbl;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.AoWVScroll.TAOWVScrollBar AOWVScrollBar1;
        public Aow.Units.AoWComboBox.TAOWComboBox TacticalCombatCB;
        public Aow.Units.AoWLabel.TAOWLabel TacticalCombatLbl;
        public Aow.Units.AoWCheckBox.TAOWCheckBox AlwaysViewTCCB;
        public Aow.Units.AoWLabel.TAOWLabel AlwaysViewTCLbl;
        public int FValidateDelay;
        public Aow.Units.AoWSetup.TSetupControl FSetupControl;
        public int FPingDelay;
        public bool FChatScrollPending;
        // AoWScenSetup.pas:526
        public void Execute(Aow.Units.AoWSetup.TSetupControl Control)
        {
            byte Availability = default;
            FSetupControl = Control;
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            ScenSetupPnl.Visible = true;
            ChatMemo.Items.Clear();
            ChatEdit.Text = (AnsiString)"";
            FValidateDelay = 100;
            FPingDelay = 15;
            var call__1 = FSetupControl.GetRemote();
            ChatFogPnl.Visible = !call__1;
            var call__2 = FSetupControl.GetRemote();
            ChatMemo.FCaptureInput = call__2;
            var call__3 = FSetupControl.GetRemote();
            ChatEdit.FCaptureInput = call__3;
            var call__4 = FSetupControl.GetHost();
            if (call__4 && (FSetupControl.FSettings.FMapName == ((AnsiString)"")))
            {
                PickMap();
            }

            var call__5 = FSetupControl.GetRemote();
            if (call__5)
            {
                TacticalCombatCB.Visible = true;
                TacticalCombatLbl.Visible = true;
                TacticalCombatCB.Items.Clear();
                {
                    int first__9 = (int)0;
                    int end__6 = (int)2;
                    for (int ordinal__8 = first__9; ordinal__8 <= end__6; ordinal__8++)
                    {
                        Availability = (byte)ordinal__8;
                        var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TTacticalCombatAvailabilityRStr[Availability]);
                        TacticalCombatCB.Items.AddObject(call__7, Aow.Rtl.Rtl.IntToObject((int)Availability));
                    }
                }
            }
            else
            {
                TacticalCombatCB.Visible = false;
                TacticalCombatLbl.Visible = false;
            }

            if (FSetupControl.FSettings.FConnectionType == 2)
            {
                CustomizeLeadersLbl.TextColor = 8421504;
                CustomizeLeaders.FCaptureInput = false;
                PlayModeCB.Items.Clear();
                var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                PlayModeCB.Items.AddObject(call__10, Aow.Rtl.Rtl.IntToObject((int)1));
            }
            else
            {
                CustomizeLeadersLbl.TextColor = AlliedVictoryLbl.TextColor;
                CustomizeLeaders.FCaptureInput = true;
                PlayModeCB.Items.Clear();
                var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SimultaneousRStr);
                PlayModeCB.Items.AddObject(call__11, Aow.Rtl.Rtl.IntToObject((int)2));
                var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                PlayModeCB.Items.AddObject(call__12, Aow.Rtl.Rtl.IntToObject((int)1));
            }

            bool bool__13 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex == -1);
            if (!bool__13)
            {
                var call__14 = Aow.Units.Sound.Sound.SongPlayer.Songs.Count;
                bool__13 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex >= call__14);
            }

            bool bool__15 = bool__13;
            if (!bool__15)
            {
                var call__16 = Aow.Units.Sound.Sound.SongPlayer.Songs[Aow.Units.Sound.Sound.SongPlayer.CurrentIndex];
                bool__15 = (call__16 != ((AnsiString)"SONGS\\TITLE.IT"));
            }

            if (bool__15)
            {
                Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
                Aow.Units.Sound.Sound.SongPlayer.Songs.Add((AnsiString)"SONGS\\TITLE.IT");
                Aow.Units.Sound.Sound.SongPlayer.Play(0);
            }

            Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioSetup(this);
        }

        // AoWScenSetup.pas:579
        public void CloseSetup()
        {
            ScenSetupPnl.Visible = false;
            FSetupControl = null;
        }

        // AoWScenSetup.pas:584
        public void PickMap()
        {
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 2;
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
        }

        // AoWScenSetup.pas:257
        public void ExitGameClick(Aow.Rtl.TObject Sender)
        {
            /* TODO: Implement aowDPlayComp.CloseSession in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AoWScenSetup.pas:281
        public void ChatEditReturn()
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AoWScenSetup.pas:295
        public void PlayerListDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int I = default;
            var call__17 = PlayerList.Items.Count;
            if (DrawItem < call__17)
            {
                var call__18 = PlayerList.Items.Objects[DrawItem];
                var call__19 = FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Aow.Rtl.Rtl.ObjectToInt(call__18));
                I = call__19;
                if (I != -1)
                {
                    var call__20 = FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                    if (call__20.FMapValidation == Aow.Units.AoWSetup.TMapValidationInfo.mviInvalid)
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.Color = 255;
                        Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, AnsiString.FromBytes(88), SD);
                    }
                    else
                    {
                        var call__21 = FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                        if (call__21.FMapValidation == Aow.Units.AoWSetup.TMapValidationInfo.mviInvalidMap)
                        {
                            Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.Color = 32768;
                            Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, AnsiString.FromBytes(88), SD);
                        }
                    }
                }
            }
        }

        // AoWScenSetup.pas:358
        public void PickScenarioBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            PickMap();
        }

        // AoWScenSetup.pas:364
        public void PreferredRaceCBChange()
        {
            if (PreferredRaceCB.Selected >= 0)
            {
                var call__22 = PreferredRaceCB.Items.Objects[PreferredRaceCB.Selected];
                FSetupControl.SetPreferredRace(unchecked((byte)Aow.Rtl.Rtl.ObjectToInt(call__22)));
            }
        }

        // AoWScenSetup.pas:370
        public void StartBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FSetupControl.FSettings.FConnectionType == 2)
            {
                Aow.Units.E3Globals.E3Globals.EmailGameSettingsDlg.Execute(FSetupControl);
            }
            else
            {
                Aow.Platform.GameHost.DeferredSetupAction = 1;
            }
        }

        // AoWScenSetup.pas:378
        public void EjectBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (PlayerList.Selected > -1)
            {
                var call__23 = PlayerList.Items.Objects[PlayerList.Selected];
                FSetupControl.hEjectPlayer(Aow.Rtl.Rtl.ObjectToInt(call__23));
            }
        }

        // AoWScenSetup.pas:385
        public void MakeHostBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (PlayerList.Selected > -1)
            {
                var call__24 = PlayerList.Items.Objects[PlayerList.Selected];
                FSetupControl.hSetHostRemoteID(Aow.Rtl.Rtl.ObjectToInt(call__24));
            }
        }

        // AoWScenSetup.pas:392
        public void PlayerListSBChange()
        {
            PlayerList.ListOffset = PlayerListSB.Position;
            PingList.ListOffset = PlayerListSB.Position;
            PreferredRaceList.ListOffset = PlayerListSB.Position;
        }

        // AoWScenSetup.pas:399
        public void PlayerListChange(Aow.Rtl.TObject Sender)
        {
            SelectPlayer(((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected);
        }

        // AoWScenSetup.pas:404
        public void TurnTimeSlChange()
        {
            FSetupControl.hSetTurnTimeLimitation(TurnTimeSl.Position);
            UpdateTurnTime();
        }

        // AoWScenSetup.pas:409
        public void TurnsCBChange(Aow.Rtl.TObject Sender)
        {
            TurnsSlChange(this);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:415
        public void TurnsSlChange(Aow.Rtl.TObject Sender)
        {
            if (TurnsSl.Position == 0)
            {
                FSetupControl.hSetTurnLimitation(TurnsSl.Position);
            }
            else
            {
                FSetupControl.hSetTurnLimitation(TurnsSl.Position * 10);
            }

            UpdateTurnLimit();
        }

        // AoWScenSetup.pas:422
        public void PlayModeCBChange()
        {
            var call__25 = PlayModeCB.Items.Objects[PlayModeCB.Selected];
            FSetupControl.hSetPlayMode(unchecked((byte)Aow.Rtl.Rtl.ObjectToInt(call__25)));
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:428
        public void RacePlayerCBChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWSetup.TSetupPlayerBehavior Behavior = default;
            Aow.Units.AoWComboBox.TAOWComboBox Combo = default;
            Combo = (Aow.Units.AoWComboBox.TAOWComboBox)Sender;
            var call__26 = Combo.Items[Combo.Selected];
            var call__27 = FSetupControl.PlayerNameToPlayerID(call__26, ref Behavior);
            if (call__27)
            {
                var call__28 = FSetupControl.FSettings.IndexOfRace(unchecked((byte)unchecked((byte)Combo.Tag)));
                FSetupControl.hSetPlayerID(call__28, Behavior, true);
            }

            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:437
        public void RaceBehaviorCBChange(Aow.Units.AoWComboBox.TAOWComboBox Sender)
        {
            Aow.Units.AoWComboBox.TAOWComboBox Control = default;
            Control = Sender;
            var call__29 = FSetupControl.FSettings.IndexOfRace(unchecked((byte)unchecked((byte)Control.Tag)));
            FSetupControl.hSetPlayerBehavior(call__29, unchecked((byte)Control.Selected));
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:444
        public void LeadersOnMapCBChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetLeadersOnMap(LeadersOnMapCB.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:450
        public void CustomizeLeadersChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetCustomizeLeaders(CustomizeLeaders.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:456
        public void AlliedVictoryCBChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetAlliedVictory(AlliedVictoryCB.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:462
        public void ExplorationChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetExplorationEnabled(Exploration.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:468
        public void AlwaysViewTCCBChange()
        {
            FSetupControl.hSetAlwaysViewTC(AlwaysViewTCCB.Checked);
        }

        // AoWScenSetup.pas:473
        public void SettingsClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWScenSetup.pas:483
        public void ChatSBUpdate()
        {
            if (FChatScrollPending)
            {
                ChatSB.Position = (ChatSB.Max + 1) - ChatSB.NumberVisible;
                FChatScrollPending = false;
            }
        }

        // AoWScenSetup.pas:492
        public void TacticalCombatCBChange()
        {
            var call__30 = TacticalCombatCB.Items.Objects[TacticalCombatCB.Selected];
            FSetupControl.hSetTacticalCombatAvailability(unchecked((byte)Aow.Rtl.Rtl.ObjectToInt(call__30)));
        }

        // AoWScenSetup.pas:498
        public void UpdateTurnTime()
        {
            if (TurnTimeSl.Position == 0)
            {
                var call__31 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoTimerRStr);
                TurnTimeVal.Text = Aow.Rtl.Rtl.LowerCase(call__31);
            }
            else
            {
                var call__32 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XMinutesRStr);
                var call__33 = Aow.Units.AoWE.AoWE.Format(call__32, [Aow.Rtl.Rtl.IntToStr(TurnTimeSl.Position)]);
                TurnTimeVal.Text = call__33;
            }
        }

        // AoWScenSetup.pas:504
        public void UpdateTurnLimit()
        {
            if (TurnsSl.Position == 0)
            {
                var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoLimitRStr);
                TurnsVal.Text = Aow.Rtl.Rtl.LowerCase(call__34);
            }
            else
            {
                var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XTurnsRStr);
                var call__36 = Aow.Units.AoWE.AoWE.Format(Aow.Rtl.Rtl.LowerCase(call__35), [Aow.Rtl.Rtl.IntToStr(TurnsSl.Position * 10)]);
                TurnsVal.Text = call__36;
            }
        }

        // AoWScenSetup.pas:510
        public void SelectPlayer(int Index)
        {
            bool bool__37 = (Index >= 0);
            if (bool__37)
            {
                var call__38 = PlayerList.Items.Count;
                bool__37 = (Index < call__38);
            }

            if (bool__37)
            {
                PlayerList.Selected = Index;
                PingList.Selected = Index;
                PreferredRaceList.Selected = Index;
                var call__39 = FSetupControl.GetHost();
                bool bool__40 = call__39;
                if (bool__40)
                {
                    var call__41 = PlayerList.Items.Objects[PlayerList.Selected];
                    bool__40 = (Aow.Rtl.Rtl.ObjectToInt(call__41) != FSetupControl.FRemotePlayer.FRemoteID);
                }

                EjectBtn.Visible = bool__40;
                var call__42 = PlayerList.Items.Objects[PlayerList.Selected];
                var call__43 = FSetupControl.CanSetHostRemoteID(Aow.Rtl.Rtl.ObjectToInt(call__42));
                MakeHostBtn.Visible = call__43;
            }
            else
            {
                PlayerList.Selected = -1;
                PingList.Selected = -1;
                PreferredRaceList.Selected = -1;
                EjectBtn.Visible = false;
                MakeHostBtn.Visible = false;
            }
        }
    }

    public static partial class AoWScenSetup
    {
    }
}
