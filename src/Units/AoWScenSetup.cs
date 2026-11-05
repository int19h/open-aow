// Translated from AoWScenSetup.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.Sound.Sound;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWSetup;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Platform;

namespace Aow.Units.AoWScenSetup
{
    // map-validation delay. setup control. ping delay. chat scroll pending
    public partial class TScenSetup : TDataModule
    {
        public TScenSetup(TComponent owner) : base(owner)
        { }

        public TAOWMemo ChatMemo;
        public TAOWPanel ChatPnl;
        public TAOWVScrollBar ChatSB;
        public TAOWEdit ChatEdit;
        public TAOWPanel SidesPnl;
        public TAOWPanel SettingsPnl;
        public TAOWListBox PlayerList;
        public TAOWPanel R1Pnl;
        public TAOWPanel R5Pnl;
        public TAOWPanel R9Pnl;
        public TAOWPanel R2Pnl;
        public TAOWPanel R6Pnl;
        public TAOWPanel R10Pnl;
        public TAOWPanel R3Pnl;
        public TAOWPanel R7Pnl;
        public TAOWPanel R11Pnl;
        public TAOWPanel R4Pnl;
        public TAOWPanel R8Pnl;
        public TAOWPanel R12Pnl;
        public TAOWButton ExitGame;
        public TAOWButton PickScenarioBtn;
        public TAOWButton StartBtn;
        public TAOWLabel MiscLbl;
        public TAOWPanel ScenSetupPnl;
        public TAOWImage R1PlayerIcon;
        public TAOWLabel R1Lbl;
        public TAOWComboBox R1PlayerCB;
        public TAOWPanel R1CenterPnl;
        public TAOWLabel TurnLbl;
        public TAOWLabel DurationLbl;
        public TAOWLabel TurnsLbl;
        public TAOWLabel TurnTimeLbl;
        public TAOWHScrollBar TurnTimeSl;
        public TAOWLabel TurnTimeVal;
        public TAoWFrame DurFr;
        public TAOWHScrollBar TurnsSl;
        public TAOWLabel TurnsVal;
        public TAOWComboBox PlayModeCB;
        public TAOWVScrollBar TurnVSB;
        public TAOWLabel R1InfoLbl;
        public TAOWComboBox R1BehaviorCB;
        public TAOWVScrollBar R1BehaviorSB;
        public TAOWVScrollBar R1PlayerSB;
        public TAOWPanel R3CenterPnl;
        public TAOWLabel R3Lbl;
        public TAOWComboBox R3BehaviorCB;
        public TAOWVScrollBar R3BehaviorSB;
        public TAOWImage R3PlayerIcon;
        public TAOWLabel R3PlayerLbl;
        public TAOWComboBox R3PlayerCB;
        public TAOWVScrollBar R3PlayerSB;
        public TAOWLabel R3InfoLbl;
        public TAOWPanel R2CenterPnl;
        public TAOWLabel R2Lbl;
        public TAOWComboBox R2BehaviorCB;
        public TAOWVScrollBar R2BehaviorSB;
        public TAOWImage R2PlayerIcon;
        public TAOWLabel R2InfoLbl;
        public TAOWComboBox R2PlayerCB;
        public TAOWVScrollBar R2PlayerSB;
        public TAOWLabel R2PlayerLbl;
        public TAOWLabel R4InfoLbl;
        public TAOWLabel R4PlayerLbl;
        public TAOWPanel R4CenterPnl;
        public TAOWLabel R4Lbl;
        public TAOWVScrollBar R4BehaviorSB;
        public TAOWComboBox R4PlayerCB;
        public TAOWVScrollBar R4PlayerSB;
        public TAOWImage R4PlayerIcon;
        public TAOWImage R5PlayerIcon;
        public TAOWLabel R5Lbl;
        public TAOWComboBox R5PlayerCB;
        public TAOWPanel R5CenterPnl;
        public TAOWComboBox R5BehaviorCB;
        public TAOWVScrollBar R5BehaviorSB;
        public TAOWVScrollBar R5PlayerSB;
        public TAOWLabel R5InfoLbl;
        public TAOWLabel R5PlayerLbl;
        public TAOWPanel R6CenterPnl;
        public TAOWLabel R6Lbl;
        public TAOWComboBox R6BehaviorCB;
        public TAOWVScrollBar R6BehaviorSB;
        public TAOWImage R6PlayerIcon;
        public TAOWLabel R6PlayerLbl;
        public TAOWComboBox R6PlayerCB;
        public TAOWVScrollBar R6PlayerSB;
        public TAOWLabel R6InfoLbl;
        public TAOWPanel R7CenterPnl;
        public TAOWLabel R7Lbl;
        public TAOWComboBox R7BehaviorCB;
        public TAOWVScrollBar R7BehaviorSB;
        public TAOWImage R7PlayerIcon;
        public TAOWLabel R7PlayerLbl;
        public TAOWComboBox R7PlayerCB;
        public TAOWVScrollBar R7PlayerSB;
        public TAOWLabel R7InfoLbl;
        public TAOWLabel R8InfoLbl;
        public TAOWLabel R8PlayerLbl;
        public TAOWPanel R8CenterPnl;
        public TAOWLabel R8Lbl;
        public TAOWComboBox R8BehaviorCB;
        public TAOWVScrollBar R8BehaviorSB;
        public TAOWComboBox R8PlayerCB;
        public TAOWVScrollBar R8PlayerSB;
        public TAOWImage R8PlayerIcon;
        public TAOWImage R9PlayerIcon;
        public TAOWLabel R9Lbl;
        public TAOWComboBox R9PlayerCB;
        public TAOWPanel R9CenterPnl;
        public TAOWComboBox R9BehaviorCB;
        public TAOWVScrollBar R9BehaviorSB;
        public TAOWVScrollBar R9PlayerSB;
        public TAOWLabel R9InfoLbl;
        public TAOWLabel R9PlayerLbl;
        public TAOWPanel R10CenterPnl;
        public TAOWLabel R10Lbl;
        public TAOWComboBox R10BehaviorCB;
        public TAOWVScrollBar R10BehaviorSB;
        public TAOWImage R10PlayerIcon;
        public TAOWLabel R10PlayerLbl;
        public TAOWComboBox R10PlayerCB;
        public TAOWVScrollBar s10sb2;
        public TAOWLabel R10InfoLbl;
        public TAOWPanel R11CenterPnl;
        public TAOWLabel R11Lbl;
        public TAOWComboBox R11BehaviorCB;
        public TAOWVScrollBar R11BehaviorSB;
        public TAOWImage R11PlayerIcon;
        public TAOWLabel R11PlayerLbl;
        public TAOWComboBox R11PlayerCB;
        public TAOWVScrollBar R11PlayerSB;
        public TAOWLabel R11InfoLbl;
        public TAOWLabel R12InfoLbl;
        public TAOWLabel R12PlayerLbl;
        public TAOWPanel R12CenterPnl;
        public TAOWLabel R12Lbl;
        public TAOWComboBox R12BehaviorCB;
        public TAOWVScrollBar R12BehaviorSB;
        public TAOWComboBox R12PlayerCB;
        public TAOWVScrollBar R12PlayerSB;
        public TAOWImage R12PlayerIcon;
        public TAOWComboBox PreferredRaceCB;
        public TAOWVScrollBar PreferredRaceSB;
        public TAOWLabel PreferredRaceLbl;
        public TAoWFrame LFr;
        public TAOWCheckBox CustomizeLeaders;
        public TAOWLabel CustomizeLeadersLbl;
        public TAOWButton EjectBtn;
        public TAOWComboBox R4BehaviorCB;
        public TAOWPanel R1FogPnl;
        public TAOWLabel R1PlayerLbl;
        public TAOWPanel R2FogPnl;
        public TAOWPanel R3FogPnl;
        public TAOWPanel R4FogPnl;
        public TAOWPanel R8FogPnl;
        public TAOWPanel R7FogPnl;
        public TAOWPanel R6FogPnl;
        public TAOWPanel R5FogPnl;
        public TAOWPanel R12FogPnl;
        public TAOWPanel R11FogPnl;
        public TAOWPanel R10FogPnl;
        public TAOWPanel R9FogPnl;
        public TAOWButton PreferredRaceSort;
        public TAOWButton PlayerListSort;
        public TAOWPanel ConnectionPnl;
        public TAOWListBox PreferredRaceList;
        public TAOWButton AOWButton1;
        public TAOWListBox PingList;
        public TAOWVScrollBar PlayerListSB;
        public TAOWPanel SettingsFogPnl;
        public TAOWLabel ExplorationLbl;
        public TAOWCheckBox Exploration;
        public TAOWLabel ScenarioLbl;
        public TAOWLabel HostNameLbl;
        public TAOWLabel PlayerNameLbl;
        public TAOWLabel FileNameLbl;
        public TAOWButton MakeHostBtn;
        public TAOWPanel RestartSettingsFogPnl;
        public TAOWImage ChatFogPnl;
        public TAOWCheckBox LeadersOnMapCB;
        public TAOWLabel AOWLabel1;
        public TAOWCheckBox AlliedVictoryCB;
        public TAOWLabel AlliedVictoryLbl;
        public TIvTranslator Translator;
        public TAOWVScrollBar AOWVScrollBar1;
        public TAOWComboBox TacticalCombatCB;
        public TAOWLabel TacticalCombatLbl;
        public TAOWCheckBox AlwaysViewTCCB;
        public TAOWLabel AlwaysViewTCLbl;
        public int FValidateDelay;
        public TSetupControl FSetupControl;
        public int FPingDelay;
        public bool FChatScrollPending;
        // AoWScenSetup.pas:526
        public void Execute(TSetupControl Control)
        {
            byte Availability;
            FSetupControl = Control;
            E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            ScenSetupPnl.Visible = true;
            ChatMemo.Items.Clear();
            ChatEdit.Text = "";
            FValidateDelay = 100;
            FPingDelay = 15;
            var remote = FSetupControl.GetRemote();
            ChatFogPnl.Visible = !remote;
            var fSetupControlRemote = FSetupControl.GetRemote();
            ChatMemo.FCaptureInput = fSetupControlRemote;
            var remote2 = FSetupControl.GetRemote();
            ChatEdit.FCaptureInput = remote2;
            if (FSetupControl.GetHost() && IsEmpty(FSetupControl.FSettings.FMapName))
            {
                PickMap();
            }

            if (FSetupControl.GetRemote())
            {
                TacticalCombatCB.Visible = true;
                TacticalCombatLbl.Visible = true;
                TacticalCombatCB.Items.Clear();
                for (int availabilityOrdinal = 0; availabilityOrdinal <= 2; availabilityOrdinal++)
                {
                    Availability = (byte)availabilityOrdinal;
                    var tacticalCombatAvailability = TranslateRStr(AoWE.AoWE.TTacticalCombatAvailabilityRStr[Availability]);
                    TacticalCombatCB.Items.AddObject(tacticalCombatAvailability, IntToObject(Availability));
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
                var classic = TranslateRStr(ClassicRStr);
                PlayModeCB.Items.AddObject(classic, IntToObject(1));
            }
            else
            {
                CustomizeLeadersLbl.TextColor = AlliedVictoryLbl.TextColor;
                CustomizeLeaders.FCaptureInput = true;
                PlayModeCB.Items.Clear();
                var simultaneous = TranslateRStr(SimultaneousRStr);
                PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
                var classic = TranslateRStr(ClassicRStr);
                PlayModeCB.Items.AddObject(classic, IntToObject(1));
            }

            bool condition = SongPlayer.CurrentIndex == -1;
            if (!condition)
            {
                var count = SongPlayer.Songs.Count;
                condition = SongPlayer.CurrentIndex >= count;
            }

            if (condition || SongPlayer.Songs[SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
            {
                SongPlayer.Songs.Clear();
                SongPlayer.Songs.Add("SONGS\\TITLE.IT");
                SongPlayer.Play(0);
            }

            UpdateScenarioSetup(this);
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
            FileDlgWindow.FMode = 2;
            FileDlgWindow.FileDialog.Visible = true;
        }

        // AoWScenSetup.pas:257
        public void ExitGameClick(TObject Sender)
        {
            /* TODO: Implement aowDPlayComp.CloseSession in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AoWScenSetup.pas:281
        public void ChatEditReturn()
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new NotImplementedException();
        }

        // AoWScenSetup.pas:295
        public void PlayerListDrawItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
        {
            int I;
            if (DrawItem < PlayerList.Items.Count)
            {
                var objectValue = PlayerList.Items.Objects[DrawItem];
                I = FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(ObjectToInt(objectValue));
                if (I != -1)
                {
                    if (FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == TMapValidationInfo.mviInvalid)
                    {
                        FontModule.Age8s.Color = 255;
                        FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, "X", SD);
                    }
                    else
                    {
                        if (FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == TMapValidationInfo.mviInvalidMap)
                        {
                            FontModule.Age8s.Color = 32768;
                            FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, "X", SD);
                        }
                    }
                }
            }
        }

        // AoWScenSetup.pas:358
        public void PickScenarioBtnClick()
        {
            General.PlayButtonSound(0);
            PickMap();
        }

        // AoWScenSetup.pas:364
        public void PreferredRaceCBChange()
        {
            if (PreferredRaceCB.Selected >= 0)
            {
                var objectValue = PreferredRaceCB.Items.Objects[PreferredRaceCB.Selected];
                FSetupControl.SetPreferredRace((byte)ObjectToInt(objectValue));
            }
        }

        // AoWScenSetup.pas:370
        public void StartBtnClick()
        {
            General.PlayButtonSound(0);
            if (FSetupControl.FSettings.FConnectionType == 2)
            {
                EmailGameSettingsDlg.Execute(FSetupControl);
            }
            else
            {
                GameHost.DeferredSetupAction = 1;
            }
        }

        // AoWScenSetup.pas:378
        public void EjectBtnClick()
        {
            General.PlayButtonSound(0);
            if (PlayerList.Selected > -1)
            {
                var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
                FSetupControl.hEjectPlayer(ObjectToInt(objectValue));
            }
        }

        // AoWScenSetup.pas:385
        public void MakeHostBtnClick()
        {
            General.PlayButtonSound(0);
            if (PlayerList.Selected > -1)
            {
                var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
                FSetupControl.hSetHostRemoteID(ObjectToInt(objectValue));
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
        public void PlayerListChange(TObject Sender)
        {
            SelectPlayer(((TAOWListBox)Sender).Selected);
        }

        // AoWScenSetup.pas:404
        public void TurnTimeSlChange()
        {
            FSetupControl.hSetTurnTimeLimitation(TurnTimeSl.Position);
            UpdateTurnTime();
        }

        // AoWScenSetup.pas:409
        public void TurnsCBChange(TObject Sender)
        {
            TurnsSlChange(this);
            if (((TAOWCheckBox)Sender).Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:415
        public void TurnsSlChange(TObject Sender)
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
            var objectValue = PlayModeCB.Items.Objects[PlayModeCB.Selected];
            FSetupControl.hSetPlayMode((byte)ObjectToInt(objectValue));
            E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:428
        public void RacePlayerCBChange(TObject Sender)
        {
            TSetupPlayerBehavior Behavior = default;
            TAOWComboBox Combo;
            Combo = (TAOWComboBox)Sender;
            var item = Combo.Items[Combo.Selected];
            if (FSetupControl.PlayerNameToPlayerID(item, ref Behavior))
            {
                var indexOfRace = FSetupControl.FSettings.IndexOfRace((byte)Combo.Tag);
                FSetupControl.hSetPlayerID(indexOfRace, Behavior, true);
            }

            E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:437
        public void RaceBehaviorCBChange(TAOWComboBox Sender)
        {
            TAOWComboBox Control;
            Control = Sender;
            var indexOfRace = FSetupControl.FSettings.IndexOfRace((byte)Control.Tag);
            FSetupControl.hSetPlayerBehavior(indexOfRace, (byte)Control.Selected);
            E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:444
        public void LeadersOnMapCBChange(TObject Sender)
        {
            FSetupControl.hSetLeadersOnMap(LeadersOnMapCB.Checked);
            if (((TAOWCheckBox)Sender).Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:450
        public void CustomizeLeadersChange(TObject Sender)
        {
            FSetupControl.hSetCustomizeLeaders(CustomizeLeaders.Checked);
            if (((TAOWCheckBox)Sender).Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:456
        public void AlliedVictoryCBChange(TObject Sender)
        {
            FSetupControl.hSetAlliedVictory(AlliedVictoryCB.Checked);
            if (((TAOWCheckBox)Sender).Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:462
        public void ExplorationChange(TObject Sender)
        {
            FSetupControl.hSetExplorationEnabled(Exploration.Checked);
            if (((TAOWCheckBox)Sender).Checked)
            {
                General.PlayButtonSound(1);
            }
            else
            {
                General.PlayButtonSound(2);
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
            General.PlayButtonSound(0);
        }

        // AoWScenSetup.pas:483
        public void ChatSBUpdate()
        {
            if (FChatScrollPending)
            {
                ChatSB.Position = ChatSB.Max + 1 - ChatSB.NumberVisible;
                FChatScrollPending = false;
            }
        }

        // AoWScenSetup.pas:492
        public void TacticalCombatCBChange()
        {
            var objectValue = TacticalCombatCB.Items.Objects[TacticalCombatCB.Selected];
            FSetupControl.hSetTacticalCombatAvailability((byte)ObjectToInt(objectValue));
        }

        // AoWScenSetup.pas:498
        public void UpdateTurnTime()
        {
            if (TurnTimeSl.Position == 0)
            {
                var noTimer = TranslateRStr(NoTimerRStr);
                TurnTimeVal.Text = LowerCase(noTimer);
            }
            else
            {
                var format = AoWE.AoWE.Format(TranslateRStr(XMinutesRStr), [IntToStr(TurnTimeSl.Position)]);
                TurnTimeVal.Text = format;
            }
        }

        // AoWScenSetup.pas:504
        public void UpdateTurnLimit()
        {
            if (TurnsSl.Position == 0)
            {
                var noLimit = TranslateRStr(NoLimitRStr);
                TurnsVal.Text = LowerCase(noLimit);
            }
            else
            {
                var format = AoWE.AoWE.Format(LowerCase(TranslateRStr(XTurnsRStr)), [IntToStr(TurnsSl.Position * 10)]);
                TurnsVal.Text = format;
            }
        }

        // AoWScenSetup.pas:510
        public void SelectPlayer(int Index)
        {
            if (Index >= 0 && Index < PlayerList.Items.Count)
            {
                PlayerList.Selected = Index;
                PingList.Selected = Index;
                PreferredRaceList.Selected = Index;
                bool condition = FSetupControl.GetHost() && ObjectToInt(PlayerList.Items.Objects[PlayerList.Selected]) != FSetupControl.FRemotePlayer.FRemoteID;
                EjectBtn.Visible = condition;
                var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
                var canSetHostRemoteID = FSetupControl.CanSetHostRemoteID(ObjectToInt(objectValue));
                MakeHostBtn.Visible = canSetHostRemoteID;
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
