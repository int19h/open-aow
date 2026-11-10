// Translated from AoWScenSetup.pas.
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
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = true;
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
                var tacticalCombatAvailability = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.TTacticalCombatAvailabilityRStr[Availability]);
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
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.ClassicRStr);
            PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            CustomizeLeadersLbl.TextColor = AlliedVictoryLbl.TextColor;
            CustomizeLeaders.FCaptureInput = true;
            PlayModeCB.Items.Clear();
            var simultaneous = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.SimultaneousRStr);
            PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.ClassicRStr);
            PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }

        bool condition = Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex == -1;
        if (!condition)
        {
            var count = Aow.Game.Audio.AudioHost.SongPlayer.Songs.Count;
            condition = Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex >= count;
        }

        if (condition || Aow.Game.Audio.AudioHost.SongPlayer.Songs[Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
        {
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        }

        Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioSetup(this);
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
        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FMode = 2;
        Aow.Game.Ui.Dialogs.DialogHosts.FileDlgWindow.FileDialog.Visible = true;
    }

    // AoWScenSetup.pas:257
    public void ExitGameClick()
    {
        int I;
        TSetupSettings Settings;
        try
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
            if (FSetupControl.GetRemote())
            {
                if (FSetupControl.GetHost())
                {
                    Settings = FSetupControl.FSettings;
                    I = 0;
                    while (I != Settings.FRemotePlayers.Count && Settings.FRemotePlayers.GetPlayers(I).FRemoteID == Settings.FHostRemoteID)
                    {
                        I++;
                    }

                    if (I != Settings.FRemotePlayers.Count)
                    {
                        FSetupControl.hSetHostRemoteID(Settings.FRemotePlayers.GetPlayers(I).FRemoteID);
                    }
                }

                if (Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.Connected)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.CloseSession();
                }
            }

            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.DisConnect();
            CloseSetup();
            if (Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.FLobbyGame)
            {
                GameHost.MainForm.CloseRequested();
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.MultiWindow.Execute("");
            }
        }
        catch
        {
        }
    }

    // AoWScenSetup.pas:281
    public void ChatEditReturn()
    {
        /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AoWScenSetup.pas:295
    public void PlayerListDrawItem(in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
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
                    Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8s.Color = 255;
                    Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, "X", SD);
                }
                else
                {
                    if (FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == TMapValidationInfo.mviInvalidMap)
                    {
                        Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8s.Color = 32768;
                        Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, "X", SD);
                    }
                }
            }
        }
    }

    // AoWScenSetup.pas:358
    public void PickScenarioBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FSetupControl.FSettings.FConnectionType == 2)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.EmailGameSettingsDlg.Execute(FSetupControl);
        }
        else
        {
            GameHost.DeferredSetupAction = 1;
        }
    }

    // AoWScenSetup.pas:378
    public void EjectBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (PlayerList.Selected > -1)
        {
            var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            FSetupControl.hEjectPlayer(ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:385
    public void MakeHostBtnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
    public void PlayerListChange(TAOWListBox Sender)
    {
        SelectPlayer(Sender.Selected);
    }

    // AoWScenSetup.pas:404
    public void TurnTimeSlChange()
    {
        FSetupControl.hSetTurnTimeLimitation(TurnTimeSl.Position);
        UpdateTurnTime();
    }

    // AoWScenSetup.pas:409
    public void TurnsCBChange(TAOWCheckBox Sender)
    {
        TurnsSlChange();
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:415
    public void TurnsSlChange()
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
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWScenSetup.pas:428
    public void RacePlayerCBChange(TAOWComboBox Sender, int Slot)
    {
        TSetupPlayerBehavior Behavior = default;
        TAOWComboBox Combo;
        Combo = Sender;
        var item = Combo.Items[Combo.Selected];
        if (FSetupControl.PlayerNameToPlayerID(item, ref Behavior))
        {
            var indexOfRace = FSetupControl.FSettings.IndexOfRace((byte)Slot);
            FSetupControl.hSetPlayerID(indexOfRace, Behavior, true);
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWScenSetup.pas:437
    public void RaceBehaviorCBChange(TAOWComboBox Sender, int Slot)
    {
        TAOWComboBox Control;
        Control = Sender;
        var indexOfRace = FSetupControl.FSettings.IndexOfRace((byte)Slot);
        FSetupControl.hSetPlayerBehavior(indexOfRace, (byte)Control.Selected);
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWScenSetup.pas:444
    public void LeadersOnMapCBChange(TAOWCheckBox Sender)
    {
        FSetupControl.hSetLeadersOnMap(LeadersOnMapCB.Checked);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:450
    public void CustomizeLeadersChange(TAOWCheckBox Sender)
    {
        FSetupControl.hSetCustomizeLeaders(CustomizeLeaders.Checked);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:456
    public void AlliedVictoryCBChange(TAOWCheckBox Sender)
    {
        FSetupControl.hSetAlliedVictory(AlliedVictoryCB.Checked);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:462
    public void ExplorationChange(TAOWCheckBox Sender)
    {
        FSetupControl.hSetExplorationEnabled(Exploration.Checked);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
            var noTimer = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.NoTimerRStr);
            TurnTimeVal.Text = LowerCase(noTimer);
        }
        else
        {
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.XMinutesRStr), [IntToStr(TurnTimeSl.Position)]);
            TurnTimeVal.Text = format;
        }
    }

    // AoWScenSetup.pas:504
    public void UpdateTurnLimit()
    {
        if (TurnsSl.Position == 0)
        {
            var noLimit = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.NoLimitRStr);
            TurnsVal.Text = LowerCase(noLimit);
        }
        else
        {
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(LowerCase(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XTurnsRStr)), [IntToStr(TurnsSl.Position * 10)]);
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

    internal static FixedArray_string_3 TTacticalCombatAvailabilityRStr = FixedArray_string_3.From([]);

    internal static readonly string ClassicRStr = "Classic";

    private static readonly string NoLimitRStr = "No Limit";

    private static readonly string NoTimerRStr = "No Timer";

    internal static readonly string SimultaneousRStr = "Simultaneous";

    private static readonly string XMinutesRStr = "%s minutes";

    internal static void HideScenarioSetup(TScenSetup Setup)
    {
        Setup.ScenSetupPnl.Visible = false;
        Setup.FSetupControl = null;
    }

    internal static void InitializeScenarioSetup(TScenSetup Setup, TSetupControl Control)
    {
        byte B;
        Setup.FSetupControl = Control;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = true;
        Setup.ScenSetupPnl.Visible = true;
        Setup.ChatMemo.Items.Clear();
        Setup.ChatEdit.Text = "";
        Setup.FValidateDelay = 100;
        Setup.FPingDelay = 15;
        var remote = Setup.FSetupControl.GetRemote();
        Setup.ChatFogPnl.Visible = !remote;
        var fSetupControlRemote = Setup.FSetupControl.GetRemote();
        Setup.ChatMemo.CaptureInput = fSetupControlRemote;
        var remote2 = Setup.FSetupControl.GetRemote();
        Setup.ChatEdit.CaptureInput = remote2;
        if (Setup.FSetupControl.GetHost() && IsEmpty(Setup.FSetupControl.FSettings.FMapName))
        {
            Setup.PickMap();
        }

        if (Setup.FSetupControl.GetRemote())
        {
            Setup.TacticalCombatCB.Visible = true;
            Setup.TacticalCombatLbl.Visible = true;
            Setup.TacticalCombatCB.Items.Clear();
            for (int bOrdinal = 0; bOrdinal <= 2; bOrdinal++)
            {
                B = (byte)bOrdinal;
                var tacticalCombatAvailability = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.TTacticalCombatAvailabilityRStr[B]);
                Setup.TacticalCombatCB.Items.AddObject(tacticalCombatAvailability, IntToObject(B));
            }
        }
        else
        {
            Setup.TacticalCombatCB.Visible = false;
            Setup.TacticalCombatLbl.Visible = false;
        }

        if (Setup.FSetupControl.FSettings.FConnectionType == 2)
        {
            Setup.CustomizeLeadersLbl.TextColor = 8421504;
            Setup.CustomizeLeaders.CaptureInput = false;
            Setup.PlayModeCB.Items.Clear();
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.ClassicRStr);
            Setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            Setup.CustomizeLeadersLbl.TextColor = Setup.AlliedVictoryLbl.TextColor;
            Setup.CustomizeLeaders.CaptureInput = true;
            Setup.PlayModeCB.Items.Clear();
            var simultaneous = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.SimultaneousRStr);
            Setup.PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TScenSetup.ClassicRStr);
            Setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }

        bool condition = Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex != -1;
        if (condition)
        {
            var count = Aow.Game.Audio.AudioHost.SongPlayer.Songs.Count;
            condition = Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex < count;
        }

        if (!(condition && Aow.Game.Audio.AudioHost.SongPlayer.Songs[Aow.Game.Audio.AudioHost.SongPlayer.CurrentIndex] == "SONGS\\TITLE.IT"))
        {
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        }

        Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioSetup(Setup);
    }

    internal static void UpdateScenarioSetup(TScenSetup Setup)
    {
        int HostIndex;
        string HostName;
        string Connection;
        TRemotePlayer Player;
        TSetupControl Control;
        if (Setup.FSetupControl != null)
        {
            Control = Setup.FSetupControl;
            Setup.PlayerList.Items.Clear();
            Setup.PingList.Items.Clear();
            Setup.PreferredRaceList.Items.Clear();
            for (int I = 0, last = Control.FSettings.FRemotePlayers.Count - 1; I <= last; I++)
            {
                Player = Control.FSettings.FRemotePlayers.GetPlayers(I);
                Setup.PlayerList.Items.AddObject(Player.FName, IntToObject(Player.FRemoteID));
                if (Player.FPreferredRace != 255)
                {
                    var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[(sbyte)Player.FPreferredRace]);
                    Setup.PreferredRaceList.Items.Add(raceNamesPlural);
                }
                else
                {
                    var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.NoneRStr);
                    Setup.PreferredRaceList.Items.Add(none);
                }

                Setup.PingList.Items.AddObject("", IntToObject(Player.FRemoteID));
            }

            var count = Setup.PlayerList.Items.Count;
            Setup.PlayerListSB.Max = count - 1;
            Setup.SelectPlayer(Setup.PlayerList.Selected);
            Setup.CustomizeLeaders.Checked = Setup.FSetupControl.FSettings.FCustomizeLeaders;
            var indexOfObject = Setup.PlayModeCB.Items.IndexOfObject(IntToObject(Setup.FSetupControl.FSettings.FPlayMode));
            Setup.PlayModeCB.Selected = indexOfObject;
            Setup.Exploration.Checked = Setup.FSetupControl.FSettings.FExplorationEnabled;
            Setup.TurnTimeSl.Position = Setup.FSetupControl.FSettings.FTurnTime;
            if (Setup.FSetupControl.FSettings.FDayLimit != 0)
            {
                Setup.TurnsSl.Position = Setup.FSetupControl.FSettings.FDayLimit / 10;
            }
            else
            {
                Setup.TurnsSl.Position = 0;
            }

            Setup.UpdateTurnLimit();
            Setup.UpdateTurnTime();
            bool condition = Setup.FSetupControl.GetHost() && Setup.FSetupControl.GetPlayerSetupValid();
            Setup.StartBtn.Visible = condition;
            var host = Setup.FSetupControl.GetHost();
            Setup.PickScenarioBtn.Visible = host;
            var fSetupControlHost = Setup.FSetupControl.GetHost();
            Setup.SettingsFogPnl.Visible = !fSetupControlHost;
            Setup.RestartSettingsFogPnl.Visible = !Setup.SettingsFogPnl.Visible && Setup.FSetupControl.FSettings.FMapDay != 0;
            Setup.LeadersOnMapCB.Checked = Setup.FSetupControl.FSettings.FLeadersOnMap;
            Setup.AlliedVictoryCB.Checked = Setup.FSetupControl.FSettings.FAlliedVictory;
            var itemsIndexOfObject = Setup.TacticalCombatCB.Items.IndexOfObject(IntToObject(Setup.FSetupControl.FSettings.FTacticalCombatAvailability));
            Setup.TacticalCombatCB.Selected = itemsIndexOfObject;
            Setup.AlwaysViewTCCB.Visible = Setup.TacticalCombatCB.Visible && Setup.FSetupControl.FSettings.FTacticalCombatAvailability != 0;
            Setup.AlwaysViewTCCB.Checked = Setup.FSetupControl.FSettings.FAlwaysViewTC;
            Setup.AlwaysViewTCLbl.Visible = Setup.AlwaysViewTCCB.Visible;
            var remote = Setup.FSetupControl.GetRemote();
            Setup.PreferredRaceLbl.Visible = remote;
            var fSetupControlRemote = Setup.FSetupControl.GetRemote();
            Setup.PreferredRaceCB.Visible = fSetupControlRemote;
            if (Setup.PreferredRaceCB.Visible)
            {
                Setup.PreferredRaceCB.Items.Clear();
                var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.NoneRStr);
                Setup.PreferredRaceCB.Items.AddObject(none, IntToObject(-1));
                for (int I = 0, last = Setup.FSetupControl.FSettings.Count - 1; I <= last; I++)
                {
                    if (!Setup.FSetupControl.FSettings.GetSettings(I).FStatus.Contains(TPlayerSetupStatusElement.pssFixed))
                    {
                        var settingRace = Setup.FSetupControl.FSettings.GetSettings(I);
                        var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[settingRace.FRace]);
                        var setting = Setup.FSetupControl.FSettings.GetSettings(I);
                        Setup.PreferredRaceCB.Items.AddObject(raceNamesPlural, IntToObject(setting.FRace));
                    }
                }

                var aowComboBox = Setup.PreferredRaceCB;
                aowComboBox.NumberVisible = 5;
                aowComboBox.FSized = false;
                aowComboBox.Selected = aowComboBox.Items.IndexOfObject(IntToObject((sbyte)Setup.FSetupControl.FRemotePlayer.FPreferredRace));
            }

            var indexOfRace = Control.FSettings.IndexOfRace(0);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace), Setup.R1PlayerCB, Setup.R1BehaviorCB, Setup.R1PlayerIcon, Setup.R1PlayerLbl, Setup.R1InfoLbl, Setup.R1FogPnl);
            var fSettingsIndexOfRace = Control.FSettings.IndexOfRace(1);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(fSettingsIndexOfRace), Setup.R2PlayerCB, Setup.R2BehaviorCB, Setup.R2PlayerIcon, Setup.R2PlayerLbl, Setup.R2InfoLbl, Setup.R2FogPnl);
            var indexOfRace2 = Control.FSettings.IndexOfRace(2);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace2), Setup.R3PlayerCB, Setup.R3BehaviorCB, Setup.R3PlayerIcon, Setup.R3PlayerLbl, Setup.R3InfoLbl, Setup.R3FogPnl);
            var indexOfRace3 = Control.FSettings.IndexOfRace(3);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace3), Setup.R4PlayerCB, Setup.R4BehaviorCB, Setup.R4PlayerIcon, Setup.R4PlayerLbl, Setup.R4InfoLbl, Setup.R4FogPnl);
            var indexOfRace4 = Control.FSettings.IndexOfRace(4);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace4), Setup.R5PlayerCB, Setup.R5BehaviorCB, Setup.R5PlayerIcon, Setup.R5PlayerLbl, Setup.R5InfoLbl, Setup.R5FogPnl);
            var indexOfRace5 = Control.FSettings.IndexOfRace(5);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace5), Setup.R6PlayerCB, Setup.R6BehaviorCB, Setup.R6PlayerIcon, Setup.R6PlayerLbl, Setup.R6InfoLbl, Setup.R6FogPnl);
            var indexOfRace6 = Control.FSettings.IndexOfRace(6);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace6), Setup.R7PlayerCB, Setup.R7BehaviorCB, Setup.R7PlayerIcon, Setup.R7PlayerLbl, Setup.R7InfoLbl, Setup.R7FogPnl);
            var indexOfRace7 = Control.FSettings.IndexOfRace(7);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace7), Setup.R8PlayerCB, Setup.R8BehaviorCB, Setup.R8PlayerIcon, Setup.R8PlayerLbl, Setup.R8InfoLbl, Setup.R8FogPnl);
            var indexOfRace8 = Control.FSettings.IndexOfRace(8);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace8), Setup.R9PlayerCB, Setup.R9BehaviorCB, Setup.R9PlayerIcon, Setup.R9PlayerLbl, Setup.R9InfoLbl, Setup.R9FogPnl);
            var indexOfRace9 = Control.FSettings.IndexOfRace(9);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace9), Setup.R10PlayerCB, Setup.R10BehaviorCB, Setup.R10PlayerIcon, Setup.R10PlayerLbl, Setup.R10InfoLbl, Setup.R10FogPnl);
            var indexOfRace10 = Control.FSettings.IndexOfRace(10);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace10), Setup.R11PlayerCB, Setup.R11BehaviorCB, Setup.R11PlayerIcon, Setup.R11PlayerLbl, Setup.R11InfoLbl, Setup.R11FogPnl);
            var indexOfRace11 = Control.FSettings.IndexOfRace(11);
            Aow.Game.Ui.Dialogs.TScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace11), Setup.R12PlayerCB, Setup.R12BehaviorCB, Setup.R12PlayerIcon, Setup.R12PlayerLbl, Setup.R12InfoLbl, Setup.R12FogPnl);
            if (Setup.FSetupControl.GetRemote())
            {
                switch (Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.ConnectionType)
                {
                    case TConnectionType.conTCP:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TCPIPRStr);
                        break;
                    case TConnectionType.conIPX:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.IPXRStr);
                        break;
                    case TConnectionType.conModem:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ModemRStr);
                        break;
                    case TConnectionType.conSerial:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.SerialRStr);
                        break;
                    default:
                        Connection = "";
                        break;
                }

                var setupSettings = Setup.FSetupControl.FSettings;
                HostIndex = setupSettings.FRemotePlayers.IndexOfRemoteID(setupSettings.FHostRemoteID);

                if (HostIndex != -1)
                {
                    HostName = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(HostIndex).FName;
                }
                else
                {
                    HostName = "?";
                }

                Setup.HostNameLbl.Visible = true;
                Setup.FileNameLbl.Visible = true;
                Setup.PlayerNameLbl.Visible = true;
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XHostYRStr), [Connection, HostName]);
                Setup.HostNameLbl.Text = format;
                var aoWEFormat = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.PlayerNameXRStr), [Setup.FSetupControl.FRemotePlayer.FName]);
                Setup.PlayerNameLbl.Text = aoWEFormat;
                var format2 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FileNameXRStr), [Setup.FSetupControl.FSettings.FMapName]);
                Setup.FileNameLbl.Text = format2;
                if (Setup.FSetupControl.FRemotePlayer.FMapValidation == TMapValidationInfo.mviInvalid)
                {
                    Setup.FileNameLbl.TextColor = 255;
                }
                else
                {
                    Setup.FileNameLbl.TextColor = 10329501;
                }
            }
            else
            {
                Setup.HostNameLbl.Visible = false;
                Setup.FileNameLbl.Visible = false;
                Setup.PlayerNameLbl.Visible = false;
            }

            var format3 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ScenarioXRStr), [Setup.FSetupControl.FSettings.FFileName]);
            Setup.ScenarioLbl.Text = format3;
            Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.FDrawn = false;
        }
    }

    internal static void UpdateScenarioRow(TScenSetup Setup, TPlayerSetupSettings Settings, TAOWComboBox PlayerBox, TAOWComboBox BehaviorBox, TAOWImage Icon, TAOWLabel PlayerName, TAOWLabel Info, TAOWPanel Fog)
    {
        TSetupPlayerBehavior Behavior = default;
        byte B;
        int I;
        bool Visible;
        bool Found;
        if (Settings != null)
        {
            Visible = Setup.FSetupControl.GetHost() && !Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
            if (Visible != PlayerBox.Visible)
            {
                PlayerBox.Visible = Visible;
            }

            if (PlayerBox.Visible)
            {
                PlayerBox.Items.BeginUpdate();
                PlayerBox.Items.Clear();
                Setup.FSetupControl.ListAvailablePlayers(PlayerBox.Items);
                PlayerBox.Items.EndUpdate();
                Found = false;
                I = PlayerBox.Items.Count;
                while (!Found && I != 0)
                {
                    I -= 1;
                    var item = PlayerBox.Items[I];
                    if (Setup.FSetupControl.PlayerNameToPlayerID(item, ref Behavior))
                    {
                        Found = Settings.FBehavior.PlayerType == Behavior.PlayerType && Settings.FBehavior.Flags == Behavior.Flags;
                    }
                }

                if (Found)
                {
                    PlayerBox.Selected = I;
                }
                else
                {
                    PlayerBox.Selected = -1;
                }

                PlayerName.Visible = false;
            }
            else
            {
                PlayerName.Visible = true;
                if (Settings.FBehavior.PlayerType == 0 && Settings.FBehavior.Flags != 0)
                {
                    I = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                    if (I != -1)
                    {
                        PlayerName.Text = Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FName;
                    }
                    else
                    {
                        PlayerName.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.HumanRStr);
                    }
                }
                else
                {
                    PlayerName.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TPlayerTypeRStr[Settings.FBehavior.PlayerType]);
                }
            }

            Info.Text = "";
            Info.Visible = Setup.FSetupControl.GetRemote() || Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
            if (Info.Visible)
            {
                if (Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed))
                {
                    Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FixedRStr);
                }
                else
                {
                    if (Setup.FSetupControl.GetRemote() && Settings.FStatus.Contains(TPlayerSetupStatusElement.pssForced) && Settings.FBehavior.PlayerType == 0)
                    {
                        I = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                        if (I != -1)
                        {
                            // { Original compares race bytes, including the FF sentinel. }
                            if (Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FPreferredRace == (byte)Settings.FRace)
                            {
                                Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TakenRStr);
                            }
                            else
                            {
                                Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ForcedRStr);
                            }
                        }
                    }
                    else
                    {
                        if (Settings.FBehavior.Flags != 0)
                        {
                            Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TakenRStr);
                        }
                        else
                        {
                            if (TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType))
                            {
                                Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TPlayerBehaviorRStr[(int)(TPlayerBehavior)Settings.FBehaviorMode]);
                            }
                        }
                    }
                }
            }

            BehaviorBox.Visible = Setup.FSetupControl.GetHost() && TSet<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
            if (BehaviorBox.Visible)
            {
                BehaviorBox.Items.BeginUpdate();
                BehaviorBox.Items.Clear();
                for (int bOrdinal = 0; bOrdinal <= 5; bOrdinal++)
                {
                    B = (byte)bOrdinal;
                    var playerBehavior = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TPlayerBehaviorRStr[(int)(TPlayerBehavior)B]);
                    BehaviorBox.Items.AddObject(playerBehavior, IntToObject(B));
                }

                BehaviorBox.Items.EndUpdate();
                BehaviorBox.Selected = Settings.FBehaviorMode;
            }

            Icon.Visible = TSet<byte>.WithBounds(0, 255).WithRange(0, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
            if (Icon.Visible)
            {
                switch (Settings.FBehavior.PlayerType)
                {
                    case 1:
                        Icon.CurrentImage = 1;
                        break;
                    case 2:
                        Icon.CurrentImage = 2;
                        break;
                    case 3:
                        Icon.CurrentImage = 3;
                        break;
                    case 6:
                        Icon.CurrentImage = 4;
                        break;
                    case 7:
                        Icon.CurrentImage = 5;
                        break;
                    default:
                        Icon.CurrentImage = 0;
                        break;
                }
            }

            Fog.Visible = Settings.FStatus.Contains(TPlayerSetupStatusElement.pssFixed);
        }
        else
        {
            Icon.Visible = false;
            PlayerBox.Visible = false;
            BehaviorBox.Visible = false;
            PlayerName.Visible = false;
            Info.Visible = true;
            Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.UnavailableRStr);
            Fog.Visible = true;
        }
    }

}
