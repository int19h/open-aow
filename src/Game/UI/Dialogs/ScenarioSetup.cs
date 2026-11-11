// Translated from AoWScenSetup.pas.
using Aow.Engine.Buffers;

using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Host;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// map-validation delay. setup control. ping delay. chat scroll pending
public partial class ScenarioSetup : DataModule
{
    public ScenarioSetup(Component owner) : base(owner)
    { }

    public Memo ChatMemo;
    public Panel ChatPnl;
    public VerticalScrollBar ChatSB;
    public Edit ChatEdit;
    public Panel SidesPnl;
    public Panel SettingsPnl;
    public ListBox PlayerList;
    public Panel R1Pnl;
    public Panel R5Pnl;
    public Panel R9Pnl;
    public Panel R2Pnl;
    public Panel R6Pnl;
    public Panel R10Pnl;
    public Panel R3Pnl;
    public Panel R7Pnl;
    public Panel R11Pnl;
    public Panel R4Pnl;
    public Panel R8Pnl;
    public Panel R12Pnl;
    public Button ExitGame;
    public Button PickScenarioBtn;
    public Button StartBtn;
    public Label MiscLbl;
    public Panel ScenSetupPnl;
    public Image R1PlayerIcon;
    public Label R1Lbl;
    public ComboBox R1PlayerCB;
    public Panel R1CenterPnl;
    public Label TurnLbl;
    public Label DurationLbl;
    public Label TurnsLbl;
    public Label TurnTimeLbl;
    public HorizontalScrollBar TurnTimeSl;
    public Label TurnTimeVal;
    public Frame DurFr;
    public HorizontalScrollBar TurnsSl;
    public Label TurnsVal;
    public ComboBox PlayModeCB;
    public VerticalScrollBar TurnVSB;
    public Label R1InfoLbl;
    public ComboBox R1BehaviorCB;
    public VerticalScrollBar R1BehaviorSB;
    public VerticalScrollBar R1PlayerSB;
    public Panel R3CenterPnl;
    public Label R3Lbl;
    public ComboBox R3BehaviorCB;
    public VerticalScrollBar R3BehaviorSB;
    public Image R3PlayerIcon;
    public Label R3PlayerLbl;
    public ComboBox R3PlayerCB;
    public VerticalScrollBar R3PlayerSB;
    public Label R3InfoLbl;
    public Panel R2CenterPnl;
    public Label R2Lbl;
    public ComboBox R2BehaviorCB;
    public VerticalScrollBar R2BehaviorSB;
    public Image R2PlayerIcon;
    public Label R2InfoLbl;
    public ComboBox R2PlayerCB;
    public VerticalScrollBar R2PlayerSB;
    public Label R2PlayerLbl;
    public Label R4InfoLbl;
    public Label R4PlayerLbl;
    public Panel R4CenterPnl;
    public Label R4Lbl;
    public VerticalScrollBar R4BehaviorSB;
    public ComboBox R4PlayerCB;
    public VerticalScrollBar R4PlayerSB;
    public Image R4PlayerIcon;
    public Image R5PlayerIcon;
    public Label R5Lbl;
    public ComboBox R5PlayerCB;
    public Panel R5CenterPnl;
    public ComboBox R5BehaviorCB;
    public VerticalScrollBar R5BehaviorSB;
    public VerticalScrollBar R5PlayerSB;
    public Label R5InfoLbl;
    public Label R5PlayerLbl;
    public Panel R6CenterPnl;
    public Label R6Lbl;
    public ComboBox R6BehaviorCB;
    public VerticalScrollBar R6BehaviorSB;
    public Image R6PlayerIcon;
    public Label R6PlayerLbl;
    public ComboBox R6PlayerCB;
    public VerticalScrollBar R6PlayerSB;
    public Label R6InfoLbl;
    public Panel R7CenterPnl;
    public Label R7Lbl;
    public ComboBox R7BehaviorCB;
    public VerticalScrollBar R7BehaviorSB;
    public Image R7PlayerIcon;
    public Label R7PlayerLbl;
    public ComboBox R7PlayerCB;
    public VerticalScrollBar R7PlayerSB;
    public Label R7InfoLbl;
    public Label R8InfoLbl;
    public Label R8PlayerLbl;
    public Panel R8CenterPnl;
    public Label R8Lbl;
    public ComboBox R8BehaviorCB;
    public VerticalScrollBar R8BehaviorSB;
    public ComboBox R8PlayerCB;
    public VerticalScrollBar R8PlayerSB;
    public Image R8PlayerIcon;
    public Image R9PlayerIcon;
    public Label R9Lbl;
    public ComboBox R9PlayerCB;
    public Panel R9CenterPnl;
    public ComboBox R9BehaviorCB;
    public VerticalScrollBar R9BehaviorSB;
    public VerticalScrollBar R9PlayerSB;
    public Label R9InfoLbl;
    public Label R9PlayerLbl;
    public Panel R10CenterPnl;
    public Label R10Lbl;
    public ComboBox R10BehaviorCB;
    public VerticalScrollBar R10BehaviorSB;
    public Image R10PlayerIcon;
    public Label R10PlayerLbl;
    public ComboBox R10PlayerCB;
    public VerticalScrollBar s10sb2;
    public Label R10InfoLbl;
    public Panel R11CenterPnl;
    public Label R11Lbl;
    public ComboBox R11BehaviorCB;
    public VerticalScrollBar R11BehaviorSB;
    public Image R11PlayerIcon;
    public Label R11PlayerLbl;
    public ComboBox R11PlayerCB;
    public VerticalScrollBar R11PlayerSB;
    public Label R11InfoLbl;
    public Label R12InfoLbl;
    public Label R12PlayerLbl;
    public Panel R12CenterPnl;
    public Label R12Lbl;
    public ComboBox R12BehaviorCB;
    public VerticalScrollBar R12BehaviorSB;
    public ComboBox R12PlayerCB;
    public VerticalScrollBar R12PlayerSB;
    public Image R12PlayerIcon;
    public ComboBox PreferredRaceCB;
    public VerticalScrollBar PreferredRaceSB;
    public Label PreferredRaceLbl;
    public Frame LFr;
    public CheckBox CustomizeLeaders;
    public Label CustomizeLeadersLbl;
    public Button EjectBtn;
    public ComboBox R4BehaviorCB;
    public Panel R1FogPnl;
    public Label R1PlayerLbl;
    public Panel R2FogPnl;
    public Panel R3FogPnl;
    public Panel R4FogPnl;
    public Panel R8FogPnl;
    public Panel R7FogPnl;
    public Panel R6FogPnl;
    public Panel R5FogPnl;
    public Panel R12FogPnl;
    public Panel R11FogPnl;
    public Panel R10FogPnl;
    public Panel R9FogPnl;
    public Button PreferredRaceSort;
    public Button PlayerListSort;
    public Panel ConnectionPnl;
    public ListBox PreferredRaceList;
    public Button AOWButton1;
    public ListBox PingList;
    public VerticalScrollBar PlayerListSB;
    public Panel SettingsFogPnl;
    public Label ExplorationLbl;
    public CheckBox Exploration;
    public Label ScenarioLbl;
    public Label HostNameLbl;
    public Label PlayerNameLbl;
    public Label FileNameLbl;
    public Button MakeHostBtn;
    public Panel RestartSettingsFogPnl;
    public Image ChatFogPnl;
    public CheckBox LeadersOnMapCB;
    public Label AOWLabel1;
    public CheckBox AlliedVictoryCB;
    public Label AlliedVictoryLbl;
    public IvTranslator Translator;
    public VerticalScrollBar AOWVScrollBar1;
    public ComboBox TacticalCombatCB;
    public Label TacticalCombatLbl;
    public CheckBox AlwaysViewTCCB;
    public Label AlwaysViewTCLbl;
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
                var tacticalCombatAvailability = AbilityRules.TranslateRStr(TTacticalCombatAvailabilityRStr[Availability]);
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
            var classic = AbilityRules.TranslateRStr(ClassicRStr);
            PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            CustomizeLeadersLbl.TextColor = AlliedVictoryLbl.TextColor;
            CustomizeLeaders.FCaptureInput = true;
            PlayModeCB.Items.Clear();
            var simultaneous = AbilityRules.TranslateRStr(SimultaneousRStr);
            PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = AbilityRules.TranslateRStr(ClassicRStr);
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
            AudioHost.SongPlayer.Songs.Clear();
            AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            AudioHost.SongPlayer.Play(0);
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
            DialogHosts.General.PlayButtonSound(0);
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
                    DialogHosts.SetupEvents.DPlay.CloseSession();
                }
            }

            DialogHosts.SetupEvents.DPlay.DisConnect();
            CloseSetup();
            if (DialogHosts.SetupEvents.FLobbyGame)
            {
                GameHost.MainForm.CloseRequested();
            }
            else
            {
                DialogHosts.MultiWindow.Execute("");
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
    public void PlayerListDrawItem(in Rect DrawRect, int DrawItem, ref LockedSurface SD)
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
                    DialogHosts.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, "X", SD);
                }
                else
                {
                    if (FSetupControl.FSettings.FRemotePlayers.GetPlayers(I).FMapValidation == MapValidationInfo.InvalidMap)
                    {
                        DialogHosts.FontModule.Age8s.Color = 32768;
                        DialogHosts.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, "X", SD);
                    }
                }
            }
        }
    }

    // AoWScenSetup.pas:358
    public void PickScenarioBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
        if (FSetupControl.FSettings.FConnectionType == 2)
        {
            DialogHosts.EmailGameSettingsDlg.Execute(FSetupControl);
        }
        else
        {
            GameHost.DeferredSetupAction = 1;
        }
    }

    // AoWScenSetup.pas:378
    public void EjectBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (PlayerList.Selected > -1)
        {
            var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            FSetupControl.hEjectPlayer(ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:385
    public void MakeHostBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
    public void PlayerListChange(ListBox Sender)
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
    public void TurnsCBChange(CheckBox Sender)
    {
        TurnsSlChange();
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
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
    public void RacePlayerCBChange(ComboBox Sender, int Slot)
    {
        SetupPlayerBehavior Behavior = default;
        ComboBox Combo;
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
    public void RaceBehaviorCBChange(ComboBox Sender, int Slot)
    {
        ComboBox Control;
        Control = Sender;
        var indexOfRace = FSetupControl.FSettings.IndexOfRace((byte)Slot);
        FSetupControl.hSetPlayerBehavior(indexOfRace, (byte)Control.Selected);
        DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWScenSetup.pas:444
    public void LeadersOnMapCBChange(CheckBox Sender)
    {
        FSetupControl.hSetLeadersOnMap(LeadersOnMapCB.Checked);
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:450
    public void CustomizeLeadersChange(CheckBox Sender)
    {
        FSetupControl.hSetCustomizeLeaders(CustomizeLeaders.Checked);
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:456
    public void AlliedVictoryCBChange(CheckBox Sender)
    {
        FSetupControl.hSetAlliedVictory(AlliedVictoryCB.Checked);
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:462
    public void ExplorationChange(CheckBox Sender)
    {
        FSetupControl.hSetExplorationEnabled(Exploration.Checked);
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
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
        DialogHosts.General.PlayButtonSound(0);
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
            var noTimer = AbilityRules.TranslateRStr(NoTimerRStr);
            TurnTimeVal.Text = LowerCase(noTimer);
        }
        else
        {
            var format = AbilityRules.Format(AbilityRules.TranslateRStr(XMinutesRStr), [IntToStr(TurnTimeSl.Position)]);
            TurnTimeVal.Text = format;
        }
    }

    // AoWScenSetup.pas:504
    public void UpdateTurnLimit()
    {
        if (TurnsSl.Position == 0)
        {
            var noLimit = AbilityRules.TranslateRStr(NoLimitRStr);
            TurnsVal.Text = LowerCase(noLimit);
        }
        else
        {
            var format = AbilityRules.Format(LowerCase(AbilityRules.TranslateRStr(GameText.XTurnsRStr)), [IntToStr(TurnsSl.Position * 10)]);
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

    internal static StringArray3 TTacticalCombatAvailabilityRStr = StringArray3.From([]);

    internal static readonly string ClassicRStr = "Classic";

    private static readonly string NoLimitRStr = "No Limit";

    private static readonly string NoTimerRStr = "No Timer";

    internal static readonly string SimultaneousRStr = "Simultaneous";

    private static readonly string XMinutesRStr = "%s minutes";

    internal static void HideScenarioSetup(ScenarioSetup Setup)
    {
        Setup.ScenSetupPnl.Visible = false;
        Setup.FSetupControl = null;
    }

    internal static void InitializeScenarioSetup(ScenarioSetup Setup, SetupControl Control)
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
                var tacticalCombatAvailability = AbilityRules.TranslateRStr(TTacticalCombatAvailabilityRStr[B]);
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
            var classic = AbilityRules.TranslateRStr(ClassicRStr);
            Setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            Setup.CustomizeLeadersLbl.TextColor = Setup.AlliedVictoryLbl.TextColor;
            Setup.CustomizeLeaders.CaptureInput = true;
            Setup.PlayModeCB.Items.Clear();
            var simultaneous = AbilityRules.TranslateRStr(SimultaneousRStr);
            Setup.PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = AbilityRules.TranslateRStr(ClassicRStr);
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
            AudioHost.SongPlayer.Songs.Clear();
            AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            AudioHost.SongPlayer.Play(0);
        }

        UpdateScenarioSetup(Setup);
    }

    internal static void UpdateScenarioSetup(ScenarioSetup Setup)
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
                    var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[(sbyte)Player.FPreferredRace]);
                    Setup.PreferredRaceList.Items.Add(raceNamesPlural);
                }
                else
                {
                    var none = AbilityRules.TranslateRStr(GameTables.NoneRStr);
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
                var none = AbilityRules.TranslateRStr(GameTables.NoneRStr);
                Setup.PreferredRaceCB.Items.AddObject(none, IntToObject(-1));
                for (int I = 0, last = Setup.FSetupControl.FSettings.Count - 1; I <= last; I++)
                {
                    if (!Setup.FSetupControl.FSettings.GetSettings(I).FStatus.Contains(PlayerSetupStatusElement.Fixed))
                    {
                        var settingRace = Setup.FSetupControl.FSettings.GetSettings(I);
                        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[settingRace.FRace]);
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
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace), Setup.R1PlayerCB, Setup.R1BehaviorCB, Setup.R1PlayerIcon, Setup.R1PlayerLbl, Setup.R1InfoLbl, Setup.R1FogPnl);
            var fSettingsIndexOfRace = Control.FSettings.IndexOfRace(1);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(fSettingsIndexOfRace), Setup.R2PlayerCB, Setup.R2BehaviorCB, Setup.R2PlayerIcon, Setup.R2PlayerLbl, Setup.R2InfoLbl, Setup.R2FogPnl);
            var indexOfRace2 = Control.FSettings.IndexOfRace(2);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace2), Setup.R3PlayerCB, Setup.R3BehaviorCB, Setup.R3PlayerIcon, Setup.R3PlayerLbl, Setup.R3InfoLbl, Setup.R3FogPnl);
            var indexOfRace3 = Control.FSettings.IndexOfRace(3);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace3), Setup.R4PlayerCB, Setup.R4BehaviorCB, Setup.R4PlayerIcon, Setup.R4PlayerLbl, Setup.R4InfoLbl, Setup.R4FogPnl);
            var indexOfRace4 = Control.FSettings.IndexOfRace(4);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace4), Setup.R5PlayerCB, Setup.R5BehaviorCB, Setup.R5PlayerIcon, Setup.R5PlayerLbl, Setup.R5InfoLbl, Setup.R5FogPnl);
            var indexOfRace5 = Control.FSettings.IndexOfRace(5);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace5), Setup.R6PlayerCB, Setup.R6BehaviorCB, Setup.R6PlayerIcon, Setup.R6PlayerLbl, Setup.R6InfoLbl, Setup.R6FogPnl);
            var indexOfRace6 = Control.FSettings.IndexOfRace(6);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace6), Setup.R7PlayerCB, Setup.R7BehaviorCB, Setup.R7PlayerIcon, Setup.R7PlayerLbl, Setup.R7InfoLbl, Setup.R7FogPnl);
            var indexOfRace7 = Control.FSettings.IndexOfRace(7);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace7), Setup.R8PlayerCB, Setup.R8BehaviorCB, Setup.R8PlayerIcon, Setup.R8PlayerLbl, Setup.R8InfoLbl, Setup.R8FogPnl);
            var indexOfRace8 = Control.FSettings.IndexOfRace(8);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace8), Setup.R9PlayerCB, Setup.R9BehaviorCB, Setup.R9PlayerIcon, Setup.R9PlayerLbl, Setup.R9InfoLbl, Setup.R9FogPnl);
            var indexOfRace9 = Control.FSettings.IndexOfRace(9);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace9), Setup.R10PlayerCB, Setup.R10BehaviorCB, Setup.R10PlayerIcon, Setup.R10PlayerLbl, Setup.R10InfoLbl, Setup.R10FogPnl);
            var indexOfRace10 = Control.FSettings.IndexOfRace(10);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace10), Setup.R11PlayerCB, Setup.R11BehaviorCB, Setup.R11PlayerIcon, Setup.R11PlayerLbl, Setup.R11InfoLbl, Setup.R11FogPnl);
            var indexOfRace11 = Control.FSettings.IndexOfRace(11);
            UpdateScenarioRow(Setup, Control.FSettings.GetSettings(indexOfRace11), Setup.R12PlayerCB, Setup.R12BehaviorCB, Setup.R12PlayerIcon, Setup.R12PlayerLbl, Setup.R12InfoLbl, Setup.R12FogPnl);
            if (Setup.FSetupControl.GetRemote())
            {
                switch (DialogHosts.SetupEvents.DPlay.ConnectionType)
                {
                    case ConnectionType.TCP:
                        Connection = AbilityRules.TranslateRStr(GameText.TCPIPRStr);
                        break;
                    case ConnectionType.IPX:
                        Connection = AbilityRules.TranslateRStr(GameText.IPXRStr);
                        break;
                    case ConnectionType.Modem:
                        Connection = AbilityRules.TranslateRStr(GameText.ModemRStr);
                        break;
                    case ConnectionType.Serial:
                        Connection = AbilityRules.TranslateRStr(GameText.SerialRStr);
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
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XHostYRStr), [Connection, HostName]);
                Setup.HostNameLbl.Text = format;
                var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.PlayerNameXRStr), [Setup.FSetupControl.FRemotePlayer.FName]);
                Setup.PlayerNameLbl.Text = aoWEFormat;
                var format2 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.FileNameXRStr), [Setup.FSetupControl.FSettings.FMapName]);
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

            var format3 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.ScenarioXRStr), [Setup.FSetupControl.FSettings.FFileName]);
            Setup.ScenarioLbl.Text = format3;
            DialogHosts.TitleWindow.TitleWin.FDrawn = false;
        }
    }

    internal static void UpdateScenarioRow(ScenarioSetup Setup, PlayerSetupSettings Settings, ComboBox PlayerBox, ComboBox BehaviorBox, Image Icon, Label PlayerName, Label Info, Panel Fog)
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
                        PlayerName.Text = AbilityRules.TranslateRStr(GameTables.HumanRStr);
                    }
                }
                else
                {
                    PlayerName.Text = AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[Settings.FBehavior.PlayerType]);
                }
            }

            Info.Text = "";
            Info.Visible = Setup.FSetupControl.GetRemote() || Settings.FStatus.Contains(PlayerSetupStatusElement.Fixed);
            if (Info.Visible)
            {
                if (Settings.FStatus.Contains(PlayerSetupStatusElement.Fixed))
                {
                    Info.Text = AbilityRules.TranslateRStr(GameText.FixedRStr);
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
                                Info.Text = AbilityRules.TranslateRStr(GameText.TakenRStr);
                            }
                            else
                            {
                                Info.Text = AbilityRules.TranslateRStr(GameText.ForcedRStr);
                            }
                        }
                    }
                    else
                    {
                        if (Settings.FBehavior.Flags != 0)
                        {
                            Info.Text = AbilityRules.TranslateRStr(GameText.TakenRStr);
                        }
                        else
                        {
                            if (Set<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(Settings.FBehavior.PlayerType))
                            {
                                Info.Text = AbilityRules.TranslateRStr(GameTables.TPlayerBehaviorRStr[(int)(Player.Behavior)Settings.FBehaviorMode]);
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
                    var playerBehavior = AbilityRules.TranslateRStr(GameTables.TPlayerBehaviorRStr[(int)(Player.Behavior)B]);
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
            Info.Text = AbilityRules.TranslateRStr(GameText.UnavailableRStr);
            Fog.Visible = true;
        }
    }

}
