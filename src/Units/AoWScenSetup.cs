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
public partial class ScenSetup : DataModule
{
    public ScenSetup(Component owner) : base(owner)
    { }

    public AOWMemo ChatMemo;
    public AOWPanel ChatPnl;
    public AOWVScrollBar ChatSB;
    public AOWEdit ChatEdit;
    public AOWPanel SidesPnl;
    public AOWPanel SettingsPnl;
    public AOWListBox PlayerList;
    public AOWPanel R1Pnl;
    public AOWPanel R5Pnl;
    public AOWPanel R9Pnl;
    public AOWPanel R2Pnl;
    public AOWPanel R6Pnl;
    public AOWPanel R10Pnl;
    public AOWPanel R3Pnl;
    public AOWPanel R7Pnl;
    public AOWPanel R11Pnl;
    public AOWPanel R4Pnl;
    public AOWPanel R8Pnl;
    public AOWPanel R12Pnl;
    public AOWButton ExitGame;
    public AOWButton PickScenarioBtn;
    public AOWButton StartBtn;
    public AOWLabel MiscLbl;
    public AOWPanel ScenSetupPnl;
    public AOWImage R1PlayerIcon;
    public AOWLabel R1Lbl;
    public AOWComboBox R1PlayerCB;
    public AOWPanel R1CenterPnl;
    public AOWLabel TurnLbl;
    public AOWLabel DurationLbl;
    public AOWLabel TurnsLbl;
    public AOWLabel TurnTimeLbl;
    public AOWHScrollBar TurnTimeSl;
    public AOWLabel TurnTimeVal;
    public AoWFrame DurFr;
    public AOWHScrollBar TurnsSl;
    public AOWLabel TurnsVal;
    public AOWComboBox PlayModeCB;
    public AOWVScrollBar TurnVSB;
    public AOWLabel R1InfoLbl;
    public AOWComboBox R1BehaviorCB;
    public AOWVScrollBar R1BehaviorSB;
    public AOWVScrollBar R1PlayerSB;
    public AOWPanel R3CenterPnl;
    public AOWLabel R3Lbl;
    public AOWComboBox R3BehaviorCB;
    public AOWVScrollBar R3BehaviorSB;
    public AOWImage R3PlayerIcon;
    public AOWLabel R3PlayerLbl;
    public AOWComboBox R3PlayerCB;
    public AOWVScrollBar R3PlayerSB;
    public AOWLabel R3InfoLbl;
    public AOWPanel R2CenterPnl;
    public AOWLabel R2Lbl;
    public AOWComboBox R2BehaviorCB;
    public AOWVScrollBar R2BehaviorSB;
    public AOWImage R2PlayerIcon;
    public AOWLabel R2InfoLbl;
    public AOWComboBox R2PlayerCB;
    public AOWVScrollBar R2PlayerSB;
    public AOWLabel R2PlayerLbl;
    public AOWLabel R4InfoLbl;
    public AOWLabel R4PlayerLbl;
    public AOWPanel R4CenterPnl;
    public AOWLabel R4Lbl;
    public AOWVScrollBar R4BehaviorSB;
    public AOWComboBox R4PlayerCB;
    public AOWVScrollBar R4PlayerSB;
    public AOWImage R4PlayerIcon;
    public AOWImage R5PlayerIcon;
    public AOWLabel R5Lbl;
    public AOWComboBox R5PlayerCB;
    public AOWPanel R5CenterPnl;
    public AOWComboBox R5BehaviorCB;
    public AOWVScrollBar R5BehaviorSB;
    public AOWVScrollBar R5PlayerSB;
    public AOWLabel R5InfoLbl;
    public AOWLabel R5PlayerLbl;
    public AOWPanel R6CenterPnl;
    public AOWLabel R6Lbl;
    public AOWComboBox R6BehaviorCB;
    public AOWVScrollBar R6BehaviorSB;
    public AOWImage R6PlayerIcon;
    public AOWLabel R6PlayerLbl;
    public AOWComboBox R6PlayerCB;
    public AOWVScrollBar R6PlayerSB;
    public AOWLabel R6InfoLbl;
    public AOWPanel R7CenterPnl;
    public AOWLabel R7Lbl;
    public AOWComboBox R7BehaviorCB;
    public AOWVScrollBar R7BehaviorSB;
    public AOWImage R7PlayerIcon;
    public AOWLabel R7PlayerLbl;
    public AOWComboBox R7PlayerCB;
    public AOWVScrollBar R7PlayerSB;
    public AOWLabel R7InfoLbl;
    public AOWLabel R8InfoLbl;
    public AOWLabel R8PlayerLbl;
    public AOWPanel R8CenterPnl;
    public AOWLabel R8Lbl;
    public AOWComboBox R8BehaviorCB;
    public AOWVScrollBar R8BehaviorSB;
    public AOWComboBox R8PlayerCB;
    public AOWVScrollBar R8PlayerSB;
    public AOWImage R8PlayerIcon;
    public AOWImage R9PlayerIcon;
    public AOWLabel R9Lbl;
    public AOWComboBox R9PlayerCB;
    public AOWPanel R9CenterPnl;
    public AOWComboBox R9BehaviorCB;
    public AOWVScrollBar R9BehaviorSB;
    public AOWVScrollBar R9PlayerSB;
    public AOWLabel R9InfoLbl;
    public AOWLabel R9PlayerLbl;
    public AOWPanel R10CenterPnl;
    public AOWLabel R10Lbl;
    public AOWComboBox R10BehaviorCB;
    public AOWVScrollBar R10BehaviorSB;
    public AOWImage R10PlayerIcon;
    public AOWLabel R10PlayerLbl;
    public AOWComboBox R10PlayerCB;
    public AOWVScrollBar s10sb2;
    public AOWLabel R10InfoLbl;
    public AOWPanel R11CenterPnl;
    public AOWLabel R11Lbl;
    public AOWComboBox R11BehaviorCB;
    public AOWVScrollBar R11BehaviorSB;
    public AOWImage R11PlayerIcon;
    public AOWLabel R11PlayerLbl;
    public AOWComboBox R11PlayerCB;
    public AOWVScrollBar R11PlayerSB;
    public AOWLabel R11InfoLbl;
    public AOWLabel R12InfoLbl;
    public AOWLabel R12PlayerLbl;
    public AOWPanel R12CenterPnl;
    public AOWLabel R12Lbl;
    public AOWComboBox R12BehaviorCB;
    public AOWVScrollBar R12BehaviorSB;
    public AOWComboBox R12PlayerCB;
    public AOWVScrollBar R12PlayerSB;
    public AOWImage R12PlayerIcon;
    public AOWComboBox PreferredRaceCB;
    public AOWVScrollBar PreferredRaceSB;
    public AOWLabel PreferredRaceLbl;
    public AoWFrame LFr;
    public AOWCheckBox CustomizeLeaders;
    public AOWLabel CustomizeLeadersLbl;
    public AOWButton EjectBtn;
    public AOWComboBox R4BehaviorCB;
    public AOWPanel R1FogPnl;
    public AOWLabel R1PlayerLbl;
    public AOWPanel R2FogPnl;
    public AOWPanel R3FogPnl;
    public AOWPanel R4FogPnl;
    public AOWPanel R8FogPnl;
    public AOWPanel R7FogPnl;
    public AOWPanel R6FogPnl;
    public AOWPanel R5FogPnl;
    public AOWPanel R12FogPnl;
    public AOWPanel R11FogPnl;
    public AOWPanel R10FogPnl;
    public AOWPanel R9FogPnl;
    public AOWButton PreferredRaceSort;
    public AOWButton PlayerListSort;
    public AOWPanel ConnectionPnl;
    public AOWListBox PreferredRaceList;
    public AOWButton AOWButton1;
    public AOWListBox PingList;
    public AOWVScrollBar PlayerListSB;
    public AOWPanel SettingsFogPnl;
    public AOWLabel ExplorationLbl;
    public AOWCheckBox Exploration;
    public AOWLabel ScenarioLbl;
    public AOWLabel HostNameLbl;
    public AOWLabel PlayerNameLbl;
    public AOWLabel FileNameLbl;
    public AOWButton MakeHostBtn;
    public AOWPanel RestartSettingsFogPnl;
    public AOWImage ChatFogPnl;
    public AOWCheckBox LeadersOnMapCB;
    public AOWLabel AOWLabel1;
    public AOWCheckBox AlliedVictoryCB;
    public AOWLabel AlliedVictoryLbl;
    public IvTranslator Translator;
    public AOWVScrollBar AOWVScrollBar1;
    public AOWComboBox TacticalCombatCB;
    public AOWLabel TacticalCombatLbl;
    public AOWCheckBox AlwaysViewTCCB;
    public AOWLabel AlwaysViewTCLbl;
    public int FValidateDelay;
    public SetupControl FSetupControl;
    public int FPingDelay;
    public bool FChatScrollPending;

    // AoWScenSetup.pas:526
    public void Execute(SetupControl Control)
    {
        byte Availability;
        FSetupControl = Control;
        DialogHosts.TitleWindow.TitleWin.Visible = true;
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
                var tacticalCombatAvailability = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TTacticalCombatAvailabilityRStr[Availability]);
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
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ClassicRStr);
            PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            CustomizeLeadersLbl.TextColor = AlliedVictoryLbl.TextColor;
            CustomizeLeaders.FCaptureInput = true;
            PlayModeCB.Items.Clear();
            var simultaneous = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(SimultaneousRStr);
            PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ClassicRStr);
            PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }

        bool condition = AudioHost.SongPlayer.CurrentIndex == -1;
        if (!condition)
        {
            var count = AudioHost.SongPlayer.Songs.Count;
            condition = AudioHost.SongPlayer.CurrentIndex >= count;
        }

        if (condition || AudioHost.SongPlayer.Songs[AudioHost.SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
        {
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        }

        Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioSetup(this);
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
        DialogHosts.FileDlgWindow.FMode = 2;
        DialogHosts.FileDlgWindow.FileDialog.Visible = true;
    }

    // AoWScenSetup.pas:257
    public void ExitGameClick()
    {
        int I;
        SetupSettings Settings;
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

                if (DialogHosts.SetupEvents.DPlay.Connected)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.CloseSession();
                }
            }

            Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.DisConnect();
            CloseSetup();
            if (DialogHosts.SetupEvents.FLobbyGame)
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
    public void PlayerListDrawItem(in Rect DrawRect, int DrawItem, ref SurfaceDesc SD)
    {
        int I;
        if (DrawItem < PlayerList.Items.Count)
        {
            var objectValue = PlayerList.Items.Objects[DrawItem];
            I = FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(ObjectToInt(objectValue));
            if (I != -1)
            {
                if (FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == MapValidationInfo.Invalid)
                {
                    DialogHosts.FontModule.Age8s.Color = 255;
                    Aow.Game.Ui.Dialogs.DialogHosts.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, "X", SD);
                }
                else
                {
                    if (FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == MapValidationInfo.InvalidMap)
                    {
                        DialogHosts.FontModule.Age8s.Color = 32768;
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
    public void PlayerListChange(AOWListBox Sender)
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
    public void TurnsCBChange(AOWCheckBox Sender)
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
        DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWScenSetup.pas:428
    public void RacePlayerCBChange(AOWComboBox Sender, int Slot)
    {
        SetupPlayerBehavior Behavior = default;
        AOWComboBox Combo;
        Combo = Sender;
        var item = Combo.Items[Combo.Selected];
        if (FSetupControl.PlayerNameToPlayerID(item, ref Behavior))
        {
            var indexOfRace = FSetupControl.FSettings.IndexOfRace((byte)Slot);
            FSetupControl.hSetPlayerID(indexOfRace, Behavior, true);
        }

        DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWScenSetup.pas:437
    public void RaceBehaviorCBChange(AOWComboBox Sender, int Slot)
    {
        AOWComboBox Control;
        Control = Sender;
        var indexOfRace = FSetupControl.FSettings.IndexOfRace((byte)Slot);
        FSetupControl.hSetPlayerBehavior(indexOfRace, (byte)Control.Selected);
        DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWScenSetup.pas:444
    public void LeadersOnMapCBChange(AOWCheckBox Sender)
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
    public void CustomizeLeadersChange(AOWCheckBox Sender)
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
    public void AlliedVictoryCBChange(AOWCheckBox Sender)
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
    public void ExplorationChange(AOWCheckBox Sender)
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
            var noTimer = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(NoTimerRStr);
            TurnTimeVal.Text = LowerCase(noTimer);
        }
        else
        {
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(XMinutesRStr), [IntToStr(TurnTimeSl.Position)]);
            TurnTimeVal.Text = format;
        }
    }

    // AoWScenSetup.pas:504
    public void UpdateTurnLimit()
    {
        if (TurnsSl.Position == 0)
        {
            var noLimit = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(NoLimitRStr);
            TurnsVal.Text = LowerCase(noLimit);
        }
        else
        {
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(LowerCase(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XTurnsRStr)), [IntToStr(TurnsSl.Position * 10)]);
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

    internal static void HideScenarioSetup(ScenSetup Setup)
    {
        Setup.ScenSetupPnl.Visible = false;
        Setup.FSetupControl = null;
    }

    internal static void InitializeScenarioSetup(ScenSetup Setup, SetupControl Control)
    {
        byte B;
        Setup.FSetupControl = Control;
        DialogHosts.TitleWindow.TitleWin.Visible = true;
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
                var tacticalCombatAvailability = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(TTacticalCombatAvailabilityRStr[B]);
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
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ClassicRStr);
            Setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            Setup.CustomizeLeadersLbl.TextColor = Setup.AlliedVictoryLbl.TextColor;
            Setup.CustomizeLeaders.CaptureInput = true;
            Setup.PlayModeCB.Items.Clear();
            var simultaneous = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(SimultaneousRStr);
            Setup.PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ClassicRStr);
            Setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }

        bool condition = AudioHost.SongPlayer.CurrentIndex != -1;
        if (condition)
        {
            var count = AudioHost.SongPlayer.Songs.Count;
            condition = AudioHost.SongPlayer.CurrentIndex < count;
        }

        if (!(condition && AudioHost.SongPlayer.Songs[AudioHost.SongPlayer.CurrentIndex] == "SONGS\\TITLE.IT"))
        {
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
            Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        }

        Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioSetup(Setup);
    }

    internal static void UpdateScenarioSetup(ScenSetup Setup)
    {
        int HostIndex;
        string HostName;
        string Connection;
        RemotePlayer Player;
        SetupControl Control;
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
                    var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[(sbyte)Player.FPreferredRace]);
                    Setup.PreferredRaceList.Items.Add(raceNamesPlural);
                }
                else
                {
                    var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.NoneRStr);
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
                var none = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.NoneRStr);
                Setup.PreferredRaceCB.Items.AddObject(none, IntToObject(-1));
                for (int I = 0, last = Setup.FSetupControl.FSettings.Count - 1; I <= last; I++)
                {
                    if (!Setup.FSetupControl.FSettings.GetSettings(I).FStatus.Contains(PlayerSetupStatusElement.Fixed))
                    {
                        var settingRace = Setup.FSetupControl.FSettings.GetSettings(I);
                        var raceNamesPlural = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[settingRace.FRace]);
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
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace), Setup.R1PlayerCB, Setup.R1BehaviorCB, Setup.R1PlayerIcon, Setup.R1PlayerLbl, Setup.R1InfoLbl, Setup.R1FogPnl);
            var fSettingsIndexOfRace = Control.FSettings.IndexOfRace(1);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(fSettingsIndexOfRace), Setup.R2PlayerCB, Setup.R2BehaviorCB, Setup.R2PlayerIcon, Setup.R2PlayerLbl, Setup.R2InfoLbl, Setup.R2FogPnl);
            var indexOfRace2 = Control.FSettings.IndexOfRace(2);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace2), Setup.R3PlayerCB, Setup.R3BehaviorCB, Setup.R3PlayerIcon, Setup.R3PlayerLbl, Setup.R3InfoLbl, Setup.R3FogPnl);
            var indexOfRace3 = Control.FSettings.IndexOfRace(3);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace3), Setup.R4PlayerCB, Setup.R4BehaviorCB, Setup.R4PlayerIcon, Setup.R4PlayerLbl, Setup.R4InfoLbl, Setup.R4FogPnl);
            var indexOfRace4 = Control.FSettings.IndexOfRace(4);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace4), Setup.R5PlayerCB, Setup.R5BehaviorCB, Setup.R5PlayerIcon, Setup.R5PlayerLbl, Setup.R5InfoLbl, Setup.R5FogPnl);
            var indexOfRace5 = Control.FSettings.IndexOfRace(5);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace5), Setup.R6PlayerCB, Setup.R6BehaviorCB, Setup.R6PlayerIcon, Setup.R6PlayerLbl, Setup.R6InfoLbl, Setup.R6FogPnl);
            var indexOfRace6 = Control.FSettings.IndexOfRace(6);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace6), Setup.R7PlayerCB, Setup.R7BehaviorCB, Setup.R7PlayerIcon, Setup.R7PlayerLbl, Setup.R7InfoLbl, Setup.R7FogPnl);
            var indexOfRace7 = Control.FSettings.IndexOfRace(7);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace7), Setup.R8PlayerCB, Setup.R8BehaviorCB, Setup.R8PlayerIcon, Setup.R8PlayerLbl, Setup.R8InfoLbl, Setup.R8FogPnl);
            var indexOfRace8 = Control.FSettings.IndexOfRace(8);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace8), Setup.R9PlayerCB, Setup.R9BehaviorCB, Setup.R9PlayerIcon, Setup.R9PlayerLbl, Setup.R9InfoLbl, Setup.R9FogPnl);
            var indexOfRace9 = Control.FSettings.IndexOfRace(9);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace9), Setup.R10PlayerCB, Setup.R10BehaviorCB, Setup.R10PlayerIcon, Setup.R10PlayerLbl, Setup.R10InfoLbl, Setup.R10FogPnl);
            var indexOfRace10 = Control.FSettings.IndexOfRace(10);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace10), Setup.R11PlayerCB, Setup.R11BehaviorCB, Setup.R11PlayerIcon, Setup.R11PlayerLbl, Setup.R11InfoLbl, Setup.R11FogPnl);
            var indexOfRace11 = Control.FSettings.IndexOfRace(11);
            Aow.Game.Ui.Dialogs.ScenSetup.UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace11), Setup.R12PlayerCB, Setup.R12BehaviorCB, Setup.R12PlayerIcon, Setup.R12PlayerLbl, Setup.R12InfoLbl, Setup.R12FogPnl);
            if (Setup.FSetupControl.GetRemote())
            {
                switch (DialogHosts.SetupEvents.DPlay.ConnectionType)
                {
                    case ConnectionType.TCP:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.TCPIPRStr);
                        break;
                    case ConnectionType.IPX:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.IPXRStr);
                        break;
                    case ConnectionType.Modem:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ModemRStr);
                        break;
                    case ConnectionType.Serial:
                        Connection = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.SerialRStr);
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
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.XHostYRStr), [Connection, HostName]);
                Setup.HostNameLbl.Text = format;
                var aoWEFormat = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.PlayerNameXRStr), [Setup.FSetupControl.FRemotePlayer.FName]);
                Setup.PlayerNameLbl.Text = aoWEFormat;
                var format2 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FileNameXRStr), [Setup.FSetupControl.FSettings.FMapName]);
                Setup.FileNameLbl.Text = format2;
                if (Setup.FSetupControl.FRemotePlayer.FMapValidation == MapValidationInfo.Invalid)
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

            var format3 = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ScenarioXRStr), [Setup.FSetupControl.FSettings.FFileName]);
            Setup.ScenarioLbl.Text = format3;
            DialogHosts.TitleWindow.TitleWin.FDrawn = false;
        }
    }

    internal static void UpdateScenarioRow(ScenSetup Setup, PlayerSetupSettings Settings, AOWComboBox PlayerBox, AOWComboBox BehaviorBox, AOWImage Icon, AOWLabel PlayerName, AOWLabel Info, AOWPanel Fog)
    {
        SetupPlayerBehavior Behavior = default;
        byte B;
        int I;
        bool Visible;
        bool Found;
        if (Settings != null)
        {
            Visible = Setup.FSetupControl.GetHost() && !Settings.FStatus.Contains(PlayerSetupStatusElement.Fixed);
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
                        PlayerName.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.HumanRStr);
                    }
                }
                else
                {
                    PlayerName.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[Settings.FBehavior.PlayerType]);
                }
            }

            Info.Text = "";
            Info.Visible = Setup.FSetupControl.GetRemote() || Settings.FStatus.Contains(PlayerSetupStatusElement.Fixed);
            if (Info.Visible)
            {
                if (Settings.FStatus.Contains(PlayerSetupStatusElement.Fixed))
                {
                    Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.FixedRStr);
                }
                else
                {
                    if (Setup.FSetupControl.GetRemote() && Settings.FStatus.Contains(PlayerSetupStatusElement.Forced) && Settings.FBehavior.PlayerType == 0)
                    {
                        I = Setup.FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Settings.FBehavior.Flags);
                        if (I != -1)
                        {
                            // { Original compares race bytes, including the FF sentinel. }
                            if (Setup.FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FPreferredRace == (byte)Settings.FRace)
                            {
                                Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.TakenRStr);
                            }
                            else
                            {
                                Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.ForcedRStr);
                            }
                        }
                    }
                    else
                    {
                        if (Settings.FBehavior.Flags != 0)
                        {
                            Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.TakenRStr);
                        }
                        else
                        {
                            if (Set<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType))
                            {
                                Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.TPlayerBehaviorRStr[(int)(Player.Behavior)Settings.FBehaviorMode]);
                            }
                        }
                    }
                }
            }

            BehaviorBox.Visible = Setup.FSetupControl.GetHost() && Set<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
            if (BehaviorBox.Visible)
            {
                BehaviorBox.Items.BeginUpdate();
                BehaviorBox.Items.Clear();
                for (int bOrdinal = 0; bOrdinal <= 5; bOrdinal++)
                {
                    B = (byte)bOrdinal;
                    var playerBehavior = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameTables.TPlayerBehaviorRStr[(int)(Player.Behavior)B]);
                    BehaviorBox.Items.AddObject(playerBehavior, IntToObject(B));
                }

                BehaviorBox.Items.EndUpdate();
                BehaviorBox.Selected = Settings.FBehaviorMode;
            }

            Icon.Visible = Set<byte>.WithBounds(0, 255).WithRange(0, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType);
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

            Fog.Visible = Settings.FStatus.Contains(PlayerSetupStatusElement.Fixed);
        }
        else
        {
            Icon.Visible = false;
            PlayerBox.Visible = false;
            BehaviorBox.Visible = false;
            PlayerName.Visible = false;
            Info.Visible = true;
            Info.Text = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.UnavailableRStr);
            Fog.Visible = true;
        }
    }

}
