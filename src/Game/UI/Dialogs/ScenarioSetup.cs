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
    public Button Button1;
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
    public Label Label1;
    public CheckBox AlliedVictoryCB;
    public Label AlliedVictoryLbl;
    public IvTranslator Translator;
    public VerticalScrollBar VScrollBar1;
    public ComboBox TacticalCombatCB;
    public Label TacticalCombatLbl;
    public CheckBox AlwaysViewTCCB;
    public Label AlwaysViewTCLbl;

    public int ValidateDelay { get; private set; }
    public SetupControl SetupControl { get; set; }
    public int PingDelay { get; private set; }
    public bool ChatScrollPending { get; set; }

    // AoWScenSetup.pas:526
    public void Execute(SetupControl control)
    {
        byte availability;
        SetupControl = control;
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        ScenSetupPnl.Visible = true;
        ChatMemo.Items.Clear();
        ChatEdit.Text = "";
        ValidateDelay = 100;
        PingDelay = 15;
        var remote = SetupControl.GetRemote();
        ChatFogPnl.Visible = !remote;
        var fSetupControlRemote = SetupControl.GetRemote();
        ChatMemo.CaptureInput = fSetupControlRemote;
        var remote2 = SetupControl.GetRemote();
        ChatEdit.CaptureInput = remote2;
        if (SetupControl.GetHost() && IsEmpty(SetupControl.Settings.MapName))
        {
            PickMap();
        }

        if (SetupControl.GetRemote())
        {
            TacticalCombatCB.Visible = true;
            TacticalCombatLbl.Visible = true;
            TacticalCombatCB.Items.Clear();
            for (int availabilityOrdinal = 0; availabilityOrdinal <= 2; availabilityOrdinal++)
            {
                availability = (byte)availabilityOrdinal;
                var tacticalBattleAvailability = AbilityRules.TranslateResourceString(s_tacticalCombatAvailabilityTexts[availability]);
                TacticalCombatCB.Items.AddObject(tacticalBattleAvailability, IntToObject(availability));
            }
        }
        else
        {
            TacticalCombatCB.Visible = false;
            TacticalCombatLbl.Visible = false;
        }

        if (SetupControl.Settings.ConnectionType == 2)
        {
            CustomizeLeadersLbl.TextColor = 8421504;
            CustomizeLeaders.CaptureInput = false;
            PlayModeCB.Items.Clear();
            var classic = AbilityRules.TranslateResourceString(s_classicText);
            PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            CustomizeLeadersLbl.TextColor = AlliedVictoryLbl.TextColor;
            CustomizeLeaders.CaptureInput = true;
            PlayModeCB.Items.Clear();
            var simultaneous = AbilityRules.TranslateResourceString(s_simultaneousText);
            PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = AbilityRules.TranslateResourceString(s_classicText);
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
        SetupControl = null;
    }

    // AoWScenSetup.pas:584
    public void PickMap()
    {
        DialogHosts.FileDialogWindow.Mode = 2;
        DialogHosts.FileDialogWindow.FileDialog.Visible = true;
    }

    // AoWScenSetup.pas:257
    public void ExitGameClick()
    {
        int I;
        SetupSettings settings;
        try
        {
            DialogHosts.General.PlayButtonSound(0);
            if (SetupControl.GetRemote())
            {
                if (SetupControl.GetHost())
                {
                    settings = SetupControl.Settings;
                    I = 0;
                    while (I != settings.RemotePlayers.Count && settings.RemotePlayers.GetPlayers(I).RemoteId == settings.HostRemoteId)
                    {
                        I++;
                    }

                    if (I != settings.RemotePlayers.Count)
                    {
                        SetupControl.SetHostRemoteId(settings.RemotePlayers.GetPlayers(I).RemoteId);
                    }
                }

                if (DialogHosts.SetupEvents.DPlay.Connected)
                {
                    DialogHosts.SetupEvents.DPlay.CloseSession();
                }
            }

            DialogHosts.SetupEvents.DPlay.DisConnect();
            CloseSetup();
            if (DialogHosts.SetupEvents.LobbyGame)
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
    public void PlayerListDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        int I;
        if (drawItem < PlayerList.Items.Count)
        {
            var objectValue = PlayerList.Items.Objects[drawItem];
            I = SetupControl.Settings.RemotePlayers.IndexOfRemoteId(ObjectToInt(objectValue));
            if (I != -1)
            {
                if (SetupControl.Settings.RemotePlayers.GetPlayers(I).MapValidation == MapValidationInfo.Invalid)
                {
                    DialogHosts.UIFonts.Age8s.Color = 255;
                    DialogHosts.UIFonts.Age8s.SetText(drawRectangle.Right - 8, drawRectangle.Top + 4, "X", lockedSurface);
                }
                else
                {
                    if (SetupControl.Settings.RemotePlayers.GetPlayers(I).MapValidation == MapValidationInfo.InvalidMap)
                    {
                        DialogHosts.UIFonts.Age8s.Color = 32768;
                        DialogHosts.UIFonts.Age8s.SetText(drawRectangle.Right - 8, drawRectangle.Top + 4, "X", lockedSurface);
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
            SetupControl.SetPreferredRace((byte)ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:370
    public void StartBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (SetupControl.Settings.ConnectionType == 2)
        {
            DialogHosts.EmailGameSettingsDialog.Execute(SetupControl);
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
            SetupControl.EjectPlayer(ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:385
    public void MakeHostBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (PlayerList.Selected > -1)
        {
            var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            SetupControl.SetHostRemoteId(ObjectToInt(objectValue));
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
    public void PlayerListChange(ListBox sender)
    {
        SelectPlayer(sender.Selected);
    }

    // AoWScenSetup.pas:404
    public void TurnTimeSlChange()
    {
        SetupControl.SetTurnTimeLimitation(TurnTimeSl.Position);
        UpdateTurnTime();
    }

    // AoWScenSetup.pas:409
    public void TurnsCheckBoxChange(CheckBox sender)
    {
        TurnsSlChange();
        if (sender.Checked)
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
            SetupControl.SetTurnLimitation(TurnsSl.Position);
        }
        else
        {
            SetupControl.SetTurnLimitation(TurnsSl.Position * 10);
        }

        UpdateTurnLimit();
    }

    // AoWScenSetup.pas:422
    public void PlayModeCBChange()
    {
        var objectValue = PlayModeCB.Items.Objects[PlayModeCB.Selected];
        SetupControl.SetPlayMode((byte)ObjectToInt(objectValue));
        DialogHosts.TitleWindow.TitleWin.Drawn = false;
    }

    // AoWScenSetup.pas:428
    public void RacePlayerCBChange(ComboBox sender, int slot)
    {
        SetupPlayerBehavior behavior = default;
        ComboBox combo;
        combo = sender;
        var item = combo.Items[combo.Selected];
        if (SetupControl.PlayerNameToPlayerId(item, ref behavior))
        {
            var indexOfRace = SetupControl.Settings.IndexOfRace((byte)slot);
            SetupControl.SetPlayerId(indexOfRace, behavior, true);
        }

        DialogHosts.TitleWindow.TitleWin.Drawn = false;
    }

    // AoWScenSetup.pas:437
    public void RaceBehaviorCBChange(ComboBox sender, int slot)
    {
        ComboBox control;
        control = sender;
        var indexOfRace = SetupControl.Settings.IndexOfRace((byte)slot);
        SetupControl.SetPlayerBehavior(indexOfRace, (byte)control.Selected);
        DialogHosts.TitleWindow.TitleWin.Drawn = false;
    }

    // AoWScenSetup.pas:444
    public void LeadersOnMapCBChange(CheckBox sender)
    {
        SetupControl.SetLeadersOnMap(LeadersOnMapCB.Checked);
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:450
    public void CustomizeLeadersChange(CheckBox sender)
    {
        SetupControl.SetCustomizeLeaders(CustomizeLeaders.Checked);
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:456
    public void AlliedVictoryCBChange(CheckBox sender)
    {
        SetupControl.SetAlliedVictory(AlliedVictoryCB.Checked);
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:462
    public void ExplorationChange(CheckBox sender)
    {
        SetupControl.SetExplorationEnabled(Exploration.Checked);
        if (sender.Checked)
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
        SetupControl.SetAlwaysViewTacticalBattle(AlwaysViewTCCB.Checked);
    }

    // AoWScenSetup.pas:473
    public void SettingsClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWScenSetup.pas:483
    public void ChatSBUpdate()
    {
        if (ChatScrollPending)
        {
            ChatSB.Position = ChatSB.Max + 1 - ChatSB.NumberVisible;
            ChatScrollPending = false;
        }
    }

    // AoWScenSetup.pas:492
    public void TacticalCombatCBChange()
    {
        var objectValue = TacticalCombatCB.Items.Objects[TacticalCombatCB.Selected];
        SetupControl.SetTacticalBattleAvailability((byte)ObjectToInt(objectValue));
    }

    // AoWScenSetup.pas:498
    public void UpdateTurnTime()
    {
        if (TurnTimeSl.Position == 0)
        {
            var noTimer = AbilityRules.TranslateResourceString(s_noTimerText);
            TurnTimeVal.Text = LowerCase(noTimer);
        }
        else
        {
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_minutesFormat), [IntToStr(TurnTimeSl.Position)]);
            TurnTimeVal.Text = format;
        }
    }

    // AoWScenSetup.pas:504
    public void UpdateTurnLimit()
    {
        if (TurnsSl.Position == 0)
        {
            var noLimit = AbilityRules.TranslateResourceString(s_noLimitText);
            TurnsVal.Text = LowerCase(noLimit);
        }
        else
        {
            var format = AbilityRules.Format(LowerCase(AbilityRules.TranslateResourceString(GameText.TurnsFormat)), [IntToStr(TurnsSl.Position * 10)]);
            TurnsVal.Text = format;
        }
    }

    // AoWScenSetup.pas:510
    public void SelectPlayer(int index)
    {
        if (index >= 0 && index < PlayerList.Items.Count)
        {
            PlayerList.Selected = index;
            PingList.Selected = index;
            PreferredRaceList.Selected = index;
            bool condition = SetupControl.GetHost() && ObjectToInt(PlayerList.Items.Objects[PlayerList.Selected]) != SetupControl.RemotePlayer.RemoteId;
            EjectBtn.Visible = condition;
            var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            var canSetHostRemoteId = SetupControl.CanSetHostRemoteId(ObjectToInt(objectValue));
            MakeHostBtn.Visible = canSetHostRemoteId;
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

    internal static StringArray3 s_tacticalCombatAvailabilityTexts = StringArray3.From([]);

    private static readonly string s_classicText = "Classic";

    private static readonly string s_noLimitText = "No Limit";

    private static readonly string s_noTimerText = "No Timer";

    private static readonly string s_simultaneousText = "Simultaneous";

    private static readonly string s_minutesFormat = "%s minutes";

    internal static void HideScenarioSetup(ScenarioSetup setup)
    {
        setup.ScenSetupPnl.Visible = false;
        setup.SetupControl = null;
    }

    internal static void InitializeScenarioSetup(ScenarioSetup setup, SetupControl control)
    {
        byte B;
        setup.SetupControl = control;
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        setup.ScenSetupPnl.Visible = true;
        setup.ChatMemo.Items.Clear();
        setup.ChatEdit.Text = "";
        setup.ValidateDelay = 100;
        setup.PingDelay = 15;
        var remote = setup.SetupControl.GetRemote();
        setup.ChatFogPnl.Visible = !remote;
        var fSetupControlRemote = setup.SetupControl.GetRemote();
        setup.ChatMemo.CaptureInput = fSetupControlRemote;
        var remote2 = setup.SetupControl.GetRemote();
        setup.ChatEdit.CaptureInput = remote2;
        if (setup.SetupControl.GetHost() && IsEmpty(setup.SetupControl.Settings.MapName))
        {
            setup.PickMap();
        }

        if (setup.SetupControl.GetRemote())
        {
            setup.TacticalCombatCB.Visible = true;
            setup.TacticalCombatLbl.Visible = true;
            setup.TacticalCombatCB.Items.Clear();
            for (int bOrdinal = 0; bOrdinal <= 2; bOrdinal++)
            {
                B = (byte)bOrdinal;
                var tacticalBattleAvailability = AbilityRules.TranslateResourceString(s_tacticalCombatAvailabilityTexts[B]);
                setup.TacticalCombatCB.Items.AddObject(tacticalBattleAvailability, IntToObject(B));
            }
        }
        else
        {
            setup.TacticalCombatCB.Visible = false;
            setup.TacticalCombatLbl.Visible = false;
        }

        if (setup.SetupControl.Settings.ConnectionType == 2)
        {
            setup.CustomizeLeadersLbl.TextColor = 8421504;
            setup.CustomizeLeaders.CaptureInput = false;
            setup.PlayModeCB.Items.Clear();
            var classic = AbilityRules.TranslateResourceString(s_classicText);
            setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            setup.CustomizeLeadersLbl.TextColor = setup.AlliedVictoryLbl.TextColor;
            setup.CustomizeLeaders.CaptureInput = true;
            setup.PlayModeCB.Items.Clear();
            var simultaneous = AbilityRules.TranslateResourceString(s_simultaneousText);
            setup.PlayModeCB.Items.AddObject(simultaneous, IntToObject(2));
            var classic = AbilityRules.TranslateResourceString(s_classicText);
            setup.PlayModeCB.Items.AddObject(classic, IntToObject(1));
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

        UpdateScenarioSetup(setup);
    }

    internal static void UpdateScenarioSetup(ScenarioSetup setup)
    {
        int hostIndex;
        string hostName;
        string connection;
        RemotePlayer player;
        SetupControl control;
        if (setup.SetupControl != null)
        {
            control = setup.SetupControl;
            setup.PlayerList.Items.Clear();
            setup.PingList.Items.Clear();
            setup.PreferredRaceList.Items.Clear();
            for (int I = 0, last = control.Settings.RemotePlayers.Count - 1; I <= last; I++)
            {
                player = control.Settings.RemotePlayers.GetPlayers(I);
                setup.PlayerList.Items.AddObject(player.Name, IntToObject(player.RemoteId));
                if (player.PreferredRace != 255)
                {
                    var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[(sbyte)player.PreferredRace]);
                    setup.PreferredRaceList.Items.Add(raceNamesPlural);
                }
                else
                {
                    var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
                    setup.PreferredRaceList.Items.Add(none);
                }

                setup.PingList.Items.AddObject("", IntToObject(player.RemoteId));
            }

            var count = setup.PlayerList.Items.Count;
            setup.PlayerListSB.Max = count - 1;
            setup.SelectPlayer(setup.PlayerList.Selected);
            setup.CustomizeLeaders.Checked = setup.SetupControl.Settings.CustomizeLeaders;
            var indexOfObject = setup.PlayModeCB.Items.IndexOfObject(IntToObject(setup.SetupControl.Settings.PlayMode));
            setup.PlayModeCB.Selected = indexOfObject;
            setup.Exploration.Checked = setup.SetupControl.Settings.ExplorationEnabled;
            setup.TurnTimeSl.Position = setup.SetupControl.Settings.TurnTime;
            if (setup.SetupControl.Settings.DayLimit != 0)
            {
                setup.TurnsSl.Position = setup.SetupControl.Settings.DayLimit / 10;
            }
            else
            {
                setup.TurnsSl.Position = 0;
            }

            setup.UpdateTurnLimit();
            setup.UpdateTurnTime();
            bool condition = setup.SetupControl.GetHost() && setup.SetupControl.GetPlayerSetupValid();
            setup.StartBtn.Visible = condition;
            var host = setup.SetupControl.GetHost();
            setup.PickScenarioBtn.Visible = host;
            var fSetupControlHost = setup.SetupControl.GetHost();
            setup.SettingsFogPnl.Visible = !fSetupControlHost;
            setup.RestartSettingsFogPnl.Visible = !setup.SettingsFogPnl.Visible && setup.SetupControl.Settings.MapDay != 0;
            setup.LeadersOnMapCB.Checked = setup.SetupControl.Settings.LeadersOnMap;
            setup.AlliedVictoryCB.Checked = setup.SetupControl.Settings.AlliedVictory;
            var itemsIndexOfObject = setup.TacticalCombatCB.Items.IndexOfObject(IntToObject(setup.SetupControl.Settings.TacticalBattleAvailability));
            setup.TacticalCombatCB.Selected = itemsIndexOfObject;
            setup.AlwaysViewTCCB.Visible = setup.TacticalCombatCB.Visible && setup.SetupControl.Settings.TacticalBattleAvailability != 0;
            setup.AlwaysViewTCCB.Checked = setup.SetupControl.Settings.AlwaysViewTacticalBattle;
            setup.AlwaysViewTCLbl.Visible = setup.AlwaysViewTCCB.Visible;
            var remote = setup.SetupControl.GetRemote();
            setup.PreferredRaceLbl.Visible = remote;
            var fSetupControlRemote = setup.SetupControl.GetRemote();
            setup.PreferredRaceCB.Visible = fSetupControlRemote;
            if (setup.PreferredRaceCB.Visible)
            {
                setup.PreferredRaceCB.Items.Clear();
                var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
                setup.PreferredRaceCB.Items.AddObject(none, IntToObject(-1));
                for (int I = 0, last = setup.SetupControl.Settings.Count - 1; I <= last; I++)
                {
                    if (!setup.SetupControl.Settings.GetSettings(I).Status.Contains(PlayerSetupStatusElement.Fixed))
                    {
                        var settingRace = setup.SetupControl.Settings.GetSettings(I);
                        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[settingRace.Race]);
                        var setting = setup.SetupControl.Settings.GetSettings(I);
                        setup.PreferredRaceCB.Items.AddObject(raceNamesPlural, IntToObject(setting.Race));
                    }
                }

                var comboBox = setup.PreferredRaceCB;
                comboBox.NumberVisible = 5;
                comboBox.Sized = false;
                comboBox.Selected = comboBox.Items.IndexOfObject(IntToObject((sbyte)setup.SetupControl.RemotePlayer.PreferredRace));
            }

            var indexOfRace = control.Settings.IndexOfRace(0);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace), setup.R1PlayerCB, setup.R1BehaviorCB, setup.R1PlayerIcon, setup.R1PlayerLbl, setup.R1InfoLbl, setup.R1FogPnl);
            var fSettingsIndexOfRace = control.Settings.IndexOfRace(1);
            UpdateScenarioRow(setup, control.Settings.GetSettings(fSettingsIndexOfRace), setup.R2PlayerCB, setup.R2BehaviorCB, setup.R2PlayerIcon, setup.R2PlayerLbl, setup.R2InfoLbl, setup.R2FogPnl);
            var indexOfRace2 = control.Settings.IndexOfRace(2);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace2), setup.R3PlayerCB, setup.R3BehaviorCB, setup.R3PlayerIcon, setup.R3PlayerLbl, setup.R3InfoLbl, setup.R3FogPnl);
            var indexOfRace3 = control.Settings.IndexOfRace(3);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace3), setup.R4PlayerCB, setup.R4BehaviorCB, setup.R4PlayerIcon, setup.R4PlayerLbl, setup.R4InfoLbl, setup.R4FogPnl);
            var indexOfRace4 = control.Settings.IndexOfRace(4);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace4), setup.R5PlayerCB, setup.R5BehaviorCB, setup.R5PlayerIcon, setup.R5PlayerLbl, setup.R5InfoLbl, setup.R5FogPnl);
            var indexOfRace5 = control.Settings.IndexOfRace(5);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace5), setup.R6PlayerCB, setup.R6BehaviorCB, setup.R6PlayerIcon, setup.R6PlayerLbl, setup.R6InfoLbl, setup.R6FogPnl);
            var indexOfRace6 = control.Settings.IndexOfRace(6);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace6), setup.R7PlayerCB, setup.R7BehaviorCB, setup.R7PlayerIcon, setup.R7PlayerLbl, setup.R7InfoLbl, setup.R7FogPnl);
            var indexOfRace7 = control.Settings.IndexOfRace(7);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace7), setup.R8PlayerCB, setup.R8BehaviorCB, setup.R8PlayerIcon, setup.R8PlayerLbl, setup.R8InfoLbl, setup.R8FogPnl);
            var indexOfRace8 = control.Settings.IndexOfRace(8);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace8), setup.R9PlayerCB, setup.R9BehaviorCB, setup.R9PlayerIcon, setup.R9PlayerLbl, setup.R9InfoLbl, setup.R9FogPnl);
            var indexOfRace9 = control.Settings.IndexOfRace(9);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace9), setup.R10PlayerCB, setup.R10BehaviorCB, setup.R10PlayerIcon, setup.R10PlayerLbl, setup.R10InfoLbl, setup.R10FogPnl);
            var indexOfRace10 = control.Settings.IndexOfRace(10);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace10), setup.R11PlayerCB, setup.R11BehaviorCB, setup.R11PlayerIcon, setup.R11PlayerLbl, setup.R11InfoLbl, setup.R11FogPnl);
            var indexOfRace11 = control.Settings.IndexOfRace(11);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace11), setup.R12PlayerCB, setup.R12BehaviorCB, setup.R12PlayerIcon, setup.R12PlayerLbl, setup.R12InfoLbl, setup.R12FogPnl);
            if (setup.SetupControl.GetRemote())
            {
                switch (DialogHosts.SetupEvents.DPlay.ConnectionType)
                {
                    case ConnectionType.TCP:
                        connection = AbilityRules.TranslateResourceString(GameText.TcpIpLabel);
                        break;
                    case ConnectionType.IPX:
                        connection = AbilityRules.TranslateResourceString(GameText.IpxText);
                        break;
                    case ConnectionType.Modem:
                        connection = AbilityRules.TranslateResourceString(GameText.ModemText);
                        break;
                    case ConnectionType.Serial:
                        connection = AbilityRules.TranslateResourceString(GameText.SerialText);
                        break;
                    default:
                        connection = "";
                        break;
                }

                var setupSettings = setup.SetupControl.Settings;
                hostIndex = setupSettings.RemotePlayers.IndexOfRemoteId(setupSettings.HostRemoteId);

                if (hostIndex != -1)
                {
                    hostName = setup.SetupControl.Settings.RemotePlayers.GetPlayers(hostIndex).Name;
                }
                else
                {
                    hostName = "?";
                }

                setup.HostNameLbl.Visible = true;
                setup.FileNameLbl.Visible = true;
                setup.PlayerNameLbl.Visible = true;
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.HostFormat), [connection, hostName]);
                setup.HostNameLbl.Text = format;
                var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.PlayerNameFormat), [setup.SetupControl.RemotePlayer.Name]);
                setup.PlayerNameLbl.Text = formattedText;
                var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.FileNameFormat), [setup.SetupControl.Settings.MapName]);
                setup.FileNameLbl.Text = format2;
                if (setup.SetupControl.RemotePlayer.MapValidation == MapValidationInfo.Invalid)
                {
                    setup.FileNameLbl.TextColor = 255;
                }
                else
                {
                    setup.FileNameLbl.TextColor = 10329501;
                }
            }
            else
            {
                setup.HostNameLbl.Visible = false;
                setup.FileNameLbl.Visible = false;
                setup.PlayerNameLbl.Visible = false;
            }

            var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.ScenarioFormat), [setup.SetupControl.Settings.FileName]);
            setup.ScenarioLbl.Text = format3;
            DialogHosts.TitleWindow.TitleWin.Drawn = false;
        }
    }

    internal static void UpdateScenarioRow(ScenarioSetup setup, PlayerSetupSettings settings, ComboBox playerBox, ComboBox behaviorBox, Image icon, Label playerName, Label info, Panel fog)
    {
        SetupPlayerBehavior behavior = default;
        byte B;
        int I;
        bool visible;
        bool found;
        if (settings != null)
        {
            visible = setup.SetupControl.GetHost() && !settings.Status.Contains(PlayerSetupStatusElement.Fixed);
            if (visible != playerBox.Visible)
            {
                playerBox.Visible = visible;
            }

            if (playerBox.Visible)
            {
                playerBox.Items.BeginUpdate();
                playerBox.Items.Clear();
                setup.SetupControl.ListAvailablePlayers(playerBox.Items);
                playerBox.Items.EndUpdate();
                found = false;
                I = playerBox.Items.Count;
                while (!found && I != 0)
                {
                    I -= 1;
                    var item = playerBox.Items[I];
                    if (setup.SetupControl.PlayerNameToPlayerId(item, ref behavior))
                    {
                        found = settings.Behavior.PlayerType == behavior.PlayerType && settings.Behavior.Flags == behavior.Flags;
                    }
                }

                if (found)
                {
                    playerBox.Selected = I;
                }
                else
                {
                    playerBox.Selected = -1;
                }

                playerName.Visible = false;
            }
            else
            {
                playerName.Visible = true;
                if (settings.Behavior.PlayerType == 0 && settings.Behavior.Flags != 0)
                {
                    I = setup.SetupControl.Settings.RemotePlayers.IndexOfRemoteId(settings.Behavior.Flags);
                    if (I != -1)
                    {
                        playerName.Text = setup.SetupControl.Settings.RemotePlayers.GetPlayers(I).Name;
                    }
                    else
                    {
                        playerName.Text = AbilityRules.TranslateResourceString(GameTables.HumanText);
                    }
                }
                else
                {
                    playerName.Text = AbilityRules.TranslateResourceString(GameTables.s_playerTypeTexts[settings.Behavior.PlayerType]);
                }
            }

            info.Text = "";
            info.Visible = setup.SetupControl.GetRemote() || settings.Status.Contains(PlayerSetupStatusElement.Fixed);
            if (info.Visible)
            {
                if (settings.Status.Contains(PlayerSetupStatusElement.Fixed))
                {
                    info.Text = AbilityRules.TranslateResourceString(GameText.FixedText);
                }
                else
                {
                    if (setup.SetupControl.GetRemote() && settings.Status.Contains(PlayerSetupStatusElement.Forced) && settings.Behavior.PlayerType == 0)
                    {
                        I = setup.SetupControl.Settings.RemotePlayers.IndexOfRemoteId(settings.Behavior.Flags);
                        if (I != -1)
                        {
                            // { Original compares race bytes, including the FF sentinel. }
                            if (setup.SetupControl.Settings.RemotePlayers.GetPlayers(I).PreferredRace == (byte)settings.Race)
                            {
                                info.Text = AbilityRules.TranslateResourceString(GameText.TakenText);
                            }
                            else
                            {
                                info.Text = AbilityRules.TranslateResourceString(GameText.ForcedText);
                            }
                        }
                    }
                    else
                    {
                        if (settings.Behavior.Flags != 0)
                        {
                            info.Text = AbilityRules.TranslateResourceString(GameText.TakenText);
                        }
                        else
                        {
                            if (Set<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(settings.Behavior.PlayerType))
                            {
                                info.Text = AbilityRules.TranslateResourceString(GameTables.s_playerBehaviorTexts[(int)(PlayerBehavior)settings.BehaviorMode]);
                            }
                        }
                    }
                }
            }

            behaviorBox.Visible = setup.SetupControl.GetHost() && Set<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(settings.Behavior.PlayerType);
            if (behaviorBox.Visible)
            {
                behaviorBox.Items.BeginUpdate();
                behaviorBox.Items.Clear();
                for (int bOrdinal = 0; bOrdinal <= 5; bOrdinal++)
                {
                    B = (byte)bOrdinal;
                    var playerBehavior = AbilityRules.TranslateResourceString(GameTables.s_playerBehaviorTexts[(int)(PlayerBehavior)B]);
                    behaviorBox.Items.AddObject(playerBehavior, IntToObject(B));
                }

                behaviorBox.Items.EndUpdate();
                behaviorBox.Selected = settings.BehaviorMode;
            }

            icon.Visible = Set<byte>.WithBounds(0, 255).WithRange(0, 3).WithRange(6, 7).Contains(settings.Behavior.PlayerType);
            if (icon.Visible)
            {
                switch (settings.Behavior.PlayerType)
                {
                    case 1:
                        icon.CurrentImage = 1;
                        break;
                    case 2:
                        icon.CurrentImage = 2;
                        break;
                    case 3:
                        icon.CurrentImage = 3;
                        break;
                    case 6:
                        icon.CurrentImage = 4;
                        break;
                    case 7:
                        icon.CurrentImage = 5;
                        break;
                    default:
                        icon.CurrentImage = 0;
                        break;
                }
            }

            fog.Visible = settings.Status.Contains(PlayerSetupStatusElement.Fixed);
        }
        else
        {
            icon.Visible = false;
            playerBox.Visible = false;
            behaviorBox.Visible = false;
            playerName.Visible = false;
            info.Visible = true;
            info.Text = AbilityRules.TranslateResourceString(GameText.UnavailableText);
            fog.Visible = true;
        }
    }

}
