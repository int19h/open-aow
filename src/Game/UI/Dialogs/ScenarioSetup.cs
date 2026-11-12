// Translated from AoWScenSetup.pas.
using System.Collections.Immutable;
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
public partial class ScenarioSetup : UIModule
{
    public ScenarioSetup(UIContext ui) : base(ui)
    { }

    public Memo ChatMemo;
    public VerticalScrollBar ChatScrollBar;
    public Edit ChatEdit;
    public ListBox PlayerList;
    public Button PickScenarioButton;
    public Button StartButton;
    public Panel ScenarioSetupPanel;
    public Image Side1PlayerIcon;
    public ComboBox Side1PlayerComboBox;
    public HorizontalScrollBar TurnTimeSlider;
    public Label TurnTimeValueLabel;
    public HorizontalScrollBar TurnsSlider;
    public Label TurnsValueLabel;
    public ComboBox PlayModeComboBox;
    public Label Side1InfoLabel;
    public ComboBox Side1BehaviorComboBox;
    public ComboBox Side3BehaviorComboBox;
    public Image Side3PlayerIcon;
    public Label Side3PlayerLabel;
    public ComboBox Side3PlayerComboBox;
    public Label Side3InfoLabel;
    public ComboBox Side2BehaviorComboBox;
    public Image Side2PlayerIcon;
    public Label Side2InfoLabel;
    public ComboBox Side2PlayerComboBox;
    public Label Side2PlayerLabel;
    public Label Side4InfoLabel;
    public Label Side4PlayerLabel;
    public ComboBox Side4PlayerComboBox;
    public Image Side4PlayerIcon;
    public Image Side5PlayerIcon;
    public ComboBox Side5PlayerComboBox;
    public ComboBox Side5BehaviorComboBox;
    public Label Side5InfoLabel;
    public Label Side5PlayerLabel;
    public ComboBox Side6BehaviorComboBox;
    public Image Side6PlayerIcon;
    public Label Side6PlayerLabel;
    public ComboBox Side6PlayerComboBox;
    public Label Side6InfoLabel;
    public ComboBox Side7BehaviorComboBox;
    public Image Side7PlayerIcon;
    public Label Side7PlayerLabel;
    public ComboBox Side7PlayerComboBox;
    public Label Side7InfoLabel;
    public Label Side8InfoLabel;
    public Label Side8PlayerLabel;
    public ComboBox Side8BehaviorComboBox;
    public ComboBox Side8PlayerComboBox;
    public Image Side8PlayerIcon;
    public Image Side9PlayerIcon;
    public ComboBox Side9PlayerComboBox;
    public ComboBox Side9BehaviorComboBox;
    public Label Side9InfoLabel;
    public Label Side9PlayerLabel;
    public ComboBox Side10BehaviorComboBox;
    public Image Side10PlayerIcon;
    public Label Side10PlayerLabel;
    public ComboBox Side10PlayerComboBox;
    public Label Side10InfoLabel;
    public ComboBox Side11BehaviorComboBox;
    public Image Side11PlayerIcon;
    public Label Side11PlayerLabel;
    public ComboBox Side11PlayerComboBox;
    public Label Side11InfoLabel;
    public Label Side12InfoLabel;
    public Label Side12PlayerLabel;
    public ComboBox Side12BehaviorComboBox;
    public ComboBox Side12PlayerComboBox;
    public Image Side12PlayerIcon;
    public ComboBox PreferredRaceComboBox;
    public Label PreferredRaceLabel;
    public CheckBox CustomizeLeadersCheckBox;
    public Label CustomizeLeadersLabel;
    public Button EjectButton;
    public ComboBox Side4BehaviorComboBox;
    public Panel Side1FogPanel;
    public Label Side1PlayerLabel;
    public Panel Side2FogPanel;
    public Panel Side3FogPanel;
    public Panel Side4FogPanel;
    public Panel Side8FogPanel;
    public Panel Side7FogPanel;
    public Panel Side6FogPanel;
    public Panel Side5FogPanel;
    public Panel Side12FogPanel;
    public Panel Side11FogPanel;
    public Panel Side10FogPanel;
    public Panel Side9FogPanel;
    public ListBox PreferredRaceList;
    public ListBox PingList;
    public VerticalScrollBar PlayerListScrollBar;
    public Panel SettingsFogPanel;
    public CheckBox ExplorationCheckBox;
    public Label ScenarioLabel;
    public Label HostNameLabel;
    public Label PlayerNameLabel;
    public Label FileNameLabel;
    public Button MakeHostButton;
    public Panel RestartSettingsFogPanel;
    public Image ChatFogImage;
    public CheckBox LeadersOnMapCheckBox;
    public CheckBox AlliedVictoryCheckBox;
    public Label AlliedVictoryLabel;
    public ComboBox TacticalCombatComboBox;
    public Label TacticalCombatLabel;
    public CheckBox AlwaysViewTacticalCombatCheckBox;
    public Label AlwaysViewTacticalCombatLabel;

    public SetupControl SetupControl { get; set; }
    public bool ChatScrollPending { get; set; }

    // AoWScenSetup.pas:526
    public void Execute(SetupControl control)
    {
        byte availability;
        SetupControl = control;
        UI.TitleWindow.MainWindow.Visible = true;
        ScenarioSetupPanel.Visible = true;
        ChatMemo.Items.Clear();
        ChatEdit.Text = "";
        var remote = SetupControl.GetRemote();
        ChatFogImage.Visible = !remote;
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
            TacticalCombatComboBox.Visible = true;
            TacticalCombatLabel.Visible = true;
            TacticalCombatComboBox.Items.Clear();
            for (int availabilityOrdinal = 0; availabilityOrdinal <= 2; availabilityOrdinal++)
            {
                availability = (byte)availabilityOrdinal;
                var tacticalBattleAvailability = UI.Engine.TranslateResourceString(TacticalCombatAvailabilityTexts[availability]);
                TacticalCombatComboBox.Items.AddObject(tacticalBattleAvailability, IntToObject(availability));
            }
        }
        else
        {
            TacticalCombatComboBox.Visible = false;
            TacticalCombatLabel.Visible = false;
        }

        if (SetupControl.Settings.ConnectionType == 2)
        {
            CustomizeLeadersLabel.TextColor = 8421504;
            CustomizeLeadersCheckBox.CaptureInput = false;
            PlayModeComboBox.Items.Clear();
            var classic = UI.Engine.TranslateResourceString(s_classicText);
            PlayModeComboBox.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            CustomizeLeadersLabel.TextColor = AlliedVictoryLabel.TextColor;
            CustomizeLeadersCheckBox.CaptureInput = true;
            PlayModeComboBox.Items.Clear();
            var simultaneous = UI.Engine.TranslateResourceString(s_simultaneousText);
            PlayModeComboBox.Items.AddObject(simultaneous, IntToObject(2));
            var classic = UI.Engine.TranslateResourceString(s_classicText);
            PlayModeComboBox.Items.AddObject(classic, IntToObject(1));
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
        ScenarioSetupPanel.Visible = false;
        SetupControl = null;
    }

    // AoWScenSetup.pas:584
    public void PickMap()
    {
        UI.FileDialogWindow.Mode = 2;
        UI.FileDialogWindow.Window.Visible = true;
    }

    // AoWScenSetup.pas:257
    public void ExitGameClick()
    {
        int I;
        SetupSettings settings;
        try
        {
            UI.General.PlayButtonSound(0);
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

                if (UI.SetupEvents.DirectPlay.Connected)
                {
                    UI.SetupEvents.DirectPlay.CloseSession();
                }
            }

            UI.SetupEvents.DirectPlay.DisConnect();
            CloseSetup();
            if (UI.SetupEvents.LobbyGame)
            {
                GameHost.MainForm.CloseRequested();
            }
            else
            {
                UI.MultiWindow.Execute("");
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
                    UI.Fonts.Age8Shadow.Color = 255;
                    UI.Fonts.Age8Shadow.SetText(drawRectangle.Right - 8, drawRectangle.Top + 4, "X", lockedSurface);
                }
                else
                {
                    if (SetupControl.Settings.RemotePlayers.GetPlayers(I).MapValidation == MapValidationInfo.InvalidMap)
                    {
                        UI.Fonts.Age8Shadow.Color = 32768;
                        UI.Fonts.Age8Shadow.SetText(drawRectangle.Right - 8, drawRectangle.Top + 4, "X", lockedSurface);
                    }
                }
            }
        }
    }

    // AoWScenSetup.pas:358
    public void PickScenarioBtnClick()
    {
        UI.General.PlayButtonSound(0);
        PickMap();
    }

    // AoWScenSetup.pas:364
    public void PreferredRaceCBChange()
    {
        if (PreferredRaceComboBox.Selected >= 0)
        {
            var objectValue = PreferredRaceComboBox.Items.Objects[PreferredRaceComboBox.Selected];
            SetupControl.SetPreferredRace((byte)ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:370
    public void StartBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (SetupControl.Settings.ConnectionType == 2)
        {
            UI.EmailGameSettingsDialog.Execute(SetupControl);
        }
        else
        {
            GameHost.DeferredSetupAction = 1;
        }
    }

    // AoWScenSetup.pas:378
    public void EjectBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (PlayerList.Selected > -1)
        {
            var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            SetupControl.EjectPlayer(ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:385
    public void MakeHostBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (PlayerList.Selected > -1)
        {
            var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            SetupControl.SetHostRemoteId(ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:392
    public void PlayerListSBChange()
    {
        PlayerList.ListOffset = PlayerListScrollBar.Position;
        PingList.ListOffset = PlayerListScrollBar.Position;
        PreferredRaceList.ListOffset = PlayerListScrollBar.Position;
    }

    // AoWScenSetup.pas:399
    public void PlayerListChange(ListBox sender)
    {
        SelectPlayer(sender.Selected);
    }

    // AoWScenSetup.pas:404
    public void TurnTimeSlChange()
    {
        SetupControl.SetTurnTimeLimitation(TurnTimeSlider.Position);
        UpdateTurnTime();
    }

    // AoWScenSetup.pas:409
    public void TurnsCheckBoxChange(CheckBox sender)
    {
        TurnsSlChange();
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:415
    public void TurnsSlChange()
    {
        if (TurnsSlider.Position == 0)
        {
            SetupControl.SetTurnLimitation(TurnsSlider.Position);
        }
        else
        {
            SetupControl.SetTurnLimitation(TurnsSlider.Position * 10);
        }

        UpdateTurnLimit();
    }

    // AoWScenSetup.pas:422
    public void PlayModeCBChange()
    {
        var objectValue = PlayModeComboBox.Items.Objects[PlayModeComboBox.Selected];
        SetupControl.SetPlayMode((byte)ObjectToInt(objectValue));
        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWScenSetup.pas:428
    public void SidePlayerComboBoxChange(ComboBox sender, int slot)
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

        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWScenSetup.pas:437
    public void SideBehaviorComboBoxChange(ComboBox sender, int slot)
    {
        ComboBox control;
        control = sender;
        var indexOfRace = SetupControl.Settings.IndexOfRace((byte)slot);
        SetupControl.SetPlayerBehavior(indexOfRace, (byte)control.Selected);
        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWScenSetup.pas:444
    public void LeadersOnMapCBChange(CheckBox sender)
    {
        SetupControl.SetLeadersOnMap(LeadersOnMapCheckBox.Checked);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:450
    public void CustomizeLeadersChange(CheckBox sender)
    {
        SetupControl.SetCustomizeLeaders(CustomizeLeadersCheckBox.Checked);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:456
    public void AlliedVictoryCBChange(CheckBox sender)
    {
        SetupControl.SetAlliedVictory(AlliedVictoryCheckBox.Checked);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:462
    public void ExplorationChange(CheckBox sender)
    {
        SetupControl.SetExplorationEnabled(ExplorationCheckBox.Checked);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWScenSetup.pas:468
    public void AlwaysViewTCCBChange()
    {
        SetupControl.SetAlwaysViewTacticalBattle(AlwaysViewTacticalCombatCheckBox.Checked);
    }

    // AoWScenSetup.pas:473
    public void SettingsClick()
    {
        UI.General.PlayButtonSound(0);
    }

    // AoWScenSetup.pas:483
    public void ChatSBUpdate()
    {
        if (ChatScrollPending)
        {
            ChatScrollBar.Position = ChatScrollBar.Max + 1 - ChatScrollBar.NumberVisible;
            ChatScrollPending = false;
        }
    }

    // AoWScenSetup.pas:492
    public void TacticalCombatCBChange()
    {
        var objectValue = TacticalCombatComboBox.Items.Objects[TacticalCombatComboBox.Selected];
        SetupControl.SetTacticalBattleAvailability((byte)ObjectToInt(objectValue));
    }

    // AoWScenSetup.pas:498
    public void UpdateTurnTime()
    {
        if (TurnTimeSlider.Position == 0)
        {
            var noTimer = UI.Engine.TranslateResourceString(s_noTimerText);
            TurnTimeValueLabel.Text = LowerCase(noTimer);
        }
        else
        {
            var format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_minutesFormat), [IntToStr(TurnTimeSlider.Position)]);
            TurnTimeValueLabel.Text = format;
        }
    }

    // AoWScenSetup.pas:504
    public void UpdateTurnLimit()
    {
        if (TurnsSlider.Position == 0)
        {
            var noLimit = UI.Engine.TranslateResourceString(s_noLimitText);
            TurnsValueLabel.Text = LowerCase(noLimit);
        }
        else
        {
            var format = AbilityRules.Format(LowerCase(UI.Engine.TranslateResourceString(GameText.TurnsFormat)), [IntToStr(TurnsSlider.Position * 10)]);
            TurnsValueLabel.Text = format;
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
            EjectButton.Visible = condition;
            var objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            var canSetHostRemoteId = SetupControl.CanSetHostRemoteId(ObjectToInt(objectValue));
            MakeHostButton.Visible = canSetHostRemoteId;
        }
        else
        {
            PlayerList.Selected = -1;
            PingList.Selected = -1;
            PreferredRaceList.Selected = -1;
            EjectButton.Visible = false;
            MakeHostButton.Visible = false;
        }
    }

    internal static ImmutableArray<string> TacticalCombatAvailabilityTexts { get; } = ["", "", ""];

    private static readonly string s_classicText = "Classic";

    private static readonly string s_noLimitText = "No Limit";

    private static readonly string s_noTimerText = "No Timer";

    private static readonly string s_simultaneousText = "Simultaneous";

    private static readonly string s_minutesFormat = "%s minutes";

    internal static void HideScenarioSetup(ScenarioSetup setup)
    {
        setup.ScenarioSetupPanel.Visible = false;
        setup.SetupControl = null;
    }

    internal static void InitializeScenarioSetup(ScenarioSetup setup, SetupControl control)
    {
        byte B;
        setup.SetupControl = control;
        setup.UI.TitleWindow.MainWindow.Visible = true;
        setup.ScenarioSetupPanel.Visible = true;
        setup.ChatMemo.Items.Clear();
        setup.ChatEdit.Text = "";
        var remote = setup.SetupControl.GetRemote();
        setup.ChatFogImage.Visible = !remote;
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
            setup.TacticalCombatComboBox.Visible = true;
            setup.TacticalCombatLabel.Visible = true;
            setup.TacticalCombatComboBox.Items.Clear();
            for (int bOrdinal = 0; bOrdinal <= 2; bOrdinal++)
            {
                B = (byte)bOrdinal;
                var tacticalBattleAvailability = setup.UI.Engine.TranslateResourceString(TacticalCombatAvailabilityTexts[B]);
                setup.TacticalCombatComboBox.Items.AddObject(tacticalBattleAvailability, IntToObject(B));
            }
        }
        else
        {
            setup.TacticalCombatComboBox.Visible = false;
            setup.TacticalCombatLabel.Visible = false;
        }

        if (setup.SetupControl.Settings.ConnectionType == 2)
        {
            setup.CustomizeLeadersLabel.TextColor = 8421504;
            setup.CustomizeLeadersCheckBox.CaptureInput = false;
            setup.PlayModeComboBox.Items.Clear();
            var classic = setup.UI.Engine.TranslateResourceString(s_classicText);
            setup.PlayModeComboBox.Items.AddObject(classic, IntToObject(1));
        }
        else
        {
            setup.CustomizeLeadersLabel.TextColor = setup.AlliedVictoryLabel.TextColor;
            setup.CustomizeLeadersCheckBox.CaptureInput = true;
            setup.PlayModeComboBox.Items.Clear();
            var simultaneous = setup.UI.Engine.TranslateResourceString(s_simultaneousText);
            setup.PlayModeComboBox.Items.AddObject(simultaneous, IntToObject(2));
            var classic = setup.UI.Engine.TranslateResourceString(s_classicText);
            setup.PlayModeComboBox.Items.AddObject(classic, IntToObject(1));
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
                    var raceNamesPlural = setup.UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[(sbyte)player.PreferredRace]);
                    setup.PreferredRaceList.Items.Add(raceNamesPlural);
                }
                else
                {
                    var none = setup.UI.Engine.TranslateResourceString(GameTables.NoneText);
                    setup.PreferredRaceList.Items.Add(none);
                }

                setup.PingList.Items.AddObject("", IntToObject(player.RemoteId));
            }

            var count = setup.PlayerList.Items.Count;
            setup.PlayerListScrollBar.Max = count - 1;
            setup.SelectPlayer(setup.PlayerList.Selected);
            setup.CustomizeLeadersCheckBox.Checked = setup.SetupControl.Settings.CustomizeLeaders;
            var indexOfObject = setup.PlayModeComboBox.Items.IndexOfObject(IntToObject(setup.SetupControl.Settings.PlayMode));
            setup.PlayModeComboBox.Selected = indexOfObject;
            setup.ExplorationCheckBox.Checked = setup.SetupControl.Settings.ExplorationEnabled;
            setup.TurnTimeSlider.Position = setup.SetupControl.Settings.TurnTime;
            if (setup.SetupControl.Settings.DayLimit != 0)
            {
                setup.TurnsSlider.Position = setup.SetupControl.Settings.DayLimit / 10;
            }
            else
            {
                setup.TurnsSlider.Position = 0;
            }

            setup.UpdateTurnLimit();
            setup.UpdateTurnTime();
            bool condition = setup.SetupControl.GetHost() && setup.SetupControl.GetPlayerSetupValid();
            setup.StartButton.Visible = condition;
            var host = setup.SetupControl.GetHost();
            setup.PickScenarioButton.Visible = host;
            var fSetupControlHost = setup.SetupControl.GetHost();
            setup.SettingsFogPanel.Visible = !fSetupControlHost;
            setup.RestartSettingsFogPanel.Visible = !setup.SettingsFogPanel.Visible && setup.SetupControl.Settings.MapDay != 0;
            setup.LeadersOnMapCheckBox.Checked = setup.SetupControl.Settings.LeadersOnMap;
            setup.AlliedVictoryCheckBox.Checked = setup.SetupControl.Settings.AlliedVictory;
            var itemsIndexOfObject = setup.TacticalCombatComboBox.Items.IndexOfObject(IntToObject(setup.SetupControl.Settings.TacticalBattleAvailability));
            setup.TacticalCombatComboBox.Selected = itemsIndexOfObject;
            setup.AlwaysViewTacticalCombatCheckBox.Visible = setup.TacticalCombatComboBox.Visible && setup.SetupControl.Settings.TacticalBattleAvailability != 0;
            setup.AlwaysViewTacticalCombatCheckBox.Checked = setup.SetupControl.Settings.AlwaysViewTacticalBattle;
            setup.AlwaysViewTacticalCombatLabel.Visible = setup.AlwaysViewTacticalCombatCheckBox.Visible;
            var remote = setup.SetupControl.GetRemote();
            setup.PreferredRaceLabel.Visible = remote;
            var fSetupControlRemote = setup.SetupControl.GetRemote();
            setup.PreferredRaceComboBox.Visible = fSetupControlRemote;
            if (setup.PreferredRaceComboBox.Visible)
            {
                setup.PreferredRaceComboBox.Items.Clear();
                var none = setup.UI.Engine.TranslateResourceString(GameTables.NoneText);
                setup.PreferredRaceComboBox.Items.AddObject(none, IntToObject(-1));
                for (int I = 0, last = setup.SetupControl.Settings.Count - 1; I <= last; I++)
                {
                    if (!setup.SetupControl.Settings.GetSettings(I).Status.Contains(PlayerSetupStatusElement.Fixed))
                    {
                        var settingRace = setup.SetupControl.Settings.GetSettings(I);
                        var raceNamesPlural = setup.UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[settingRace.Race]);
                        var setting = setup.SetupControl.Settings.GetSettings(I);
                        setup.PreferredRaceComboBox.Items.AddObject(raceNamesPlural, IntToObject(setting.Race));
                    }
                }

                var comboBox = setup.PreferredRaceComboBox;
                comboBox.NumberVisible = 5;
                comboBox.Sized = false;
                comboBox.Selected = comboBox.Items.IndexOfObject(IntToObject((sbyte)setup.SetupControl.RemotePlayer.PreferredRace));
            }

            var indexOfRace = control.Settings.IndexOfRace(0);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace), setup.Side1PlayerComboBox, setup.Side1BehaviorComboBox, setup.Side1PlayerIcon, setup.Side1PlayerLabel, setup.Side1InfoLabel, setup.Side1FogPanel);
            var fSettingsIndexOfRace = control.Settings.IndexOfRace(1);
            UpdateScenarioRow(setup, control.Settings.GetSettings(fSettingsIndexOfRace), setup.Side2PlayerComboBox, setup.Side2BehaviorComboBox, setup.Side2PlayerIcon, setup.Side2PlayerLabel, setup.Side2InfoLabel, setup.Side2FogPanel);
            var indexOfRace2 = control.Settings.IndexOfRace(2);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace2), setup.Side3PlayerComboBox, setup.Side3BehaviorComboBox, setup.Side3PlayerIcon, setup.Side3PlayerLabel, setup.Side3InfoLabel, setup.Side3FogPanel);
            var indexOfRace3 = control.Settings.IndexOfRace(3);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace3), setup.Side4PlayerComboBox, setup.Side4BehaviorComboBox, setup.Side4PlayerIcon, setup.Side4PlayerLabel, setup.Side4InfoLabel, setup.Side4FogPanel);
            var indexOfRace4 = control.Settings.IndexOfRace(4);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace4), setup.Side5PlayerComboBox, setup.Side5BehaviorComboBox, setup.Side5PlayerIcon, setup.Side5PlayerLabel, setup.Side5InfoLabel, setup.Side5FogPanel);
            var indexOfRace5 = control.Settings.IndexOfRace(5);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace5), setup.Side6PlayerComboBox, setup.Side6BehaviorComboBox, setup.Side6PlayerIcon, setup.Side6PlayerLabel, setup.Side6InfoLabel, setup.Side6FogPanel);
            var indexOfRace6 = control.Settings.IndexOfRace(6);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace6), setup.Side7PlayerComboBox, setup.Side7BehaviorComboBox, setup.Side7PlayerIcon, setup.Side7PlayerLabel, setup.Side7InfoLabel, setup.Side7FogPanel);
            var indexOfRace7 = control.Settings.IndexOfRace(7);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace7), setup.Side8PlayerComboBox, setup.Side8BehaviorComboBox, setup.Side8PlayerIcon, setup.Side8PlayerLabel, setup.Side8InfoLabel, setup.Side8FogPanel);
            var indexOfRace8 = control.Settings.IndexOfRace(8);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace8), setup.Side9PlayerComboBox, setup.Side9BehaviorComboBox, setup.Side9PlayerIcon, setup.Side9PlayerLabel, setup.Side9InfoLabel, setup.Side9FogPanel);
            var indexOfRace9 = control.Settings.IndexOfRace(9);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace9), setup.Side10PlayerComboBox, setup.Side10BehaviorComboBox, setup.Side10PlayerIcon, setup.Side10PlayerLabel, setup.Side10InfoLabel, setup.Side10FogPanel);
            var indexOfRace10 = control.Settings.IndexOfRace(10);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace10), setup.Side11PlayerComboBox, setup.Side11BehaviorComboBox, setup.Side11PlayerIcon, setup.Side11PlayerLabel, setup.Side11InfoLabel, setup.Side11FogPanel);
            var indexOfRace11 = control.Settings.IndexOfRace(11);
            UpdateScenarioRow(setup, control.Settings.GetSettings(indexOfRace11), setup.Side12PlayerComboBox, setup.Side12BehaviorComboBox, setup.Side12PlayerIcon, setup.Side12PlayerLabel, setup.Side12InfoLabel, setup.Side12FogPanel);
            if (setup.SetupControl.GetRemote())
            {
                switch (setup.UI.SetupEvents.DirectPlay.ConnectionType)
                {
                    case ConnectionType.TCP:
                        connection = setup.UI.Engine.TranslateResourceString(GameText.TcpIpLabel);
                        break;
                    case ConnectionType.IPX:
                        connection = setup.UI.Engine.TranslateResourceString(GameText.IpxText);
                        break;
                    case ConnectionType.Modem:
                        connection = setup.UI.Engine.TranslateResourceString(GameText.ModemText);
                        break;
                    case ConnectionType.Serial:
                        connection = setup.UI.Engine.TranslateResourceString(GameText.SerialText);
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

                setup.HostNameLabel.Visible = true;
                setup.FileNameLabel.Visible = true;
                setup.PlayerNameLabel.Visible = true;
                var format = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.HostFormat), [connection, hostName]);
                setup.HostNameLabel.Text = format;
                var formattedText = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.PlayerNameFormat), [setup.SetupControl.RemotePlayer.Name]);
                setup.PlayerNameLabel.Text = formattedText;
                var format2 = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.FileNameFormat), [setup.SetupControl.Settings.MapName]);
                setup.FileNameLabel.Text = format2;
                if (setup.SetupControl.RemotePlayer.MapValidation == MapValidationInfo.Invalid)
                {
                    setup.FileNameLabel.TextColor = 255;
                }
                else
                {
                    setup.FileNameLabel.TextColor = 10329501;
                }
            }
            else
            {
                setup.HostNameLabel.Visible = false;
                setup.FileNameLabel.Visible = false;
                setup.PlayerNameLabel.Visible = false;
            }

            var format3 = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.ScenarioFormat), [setup.SetupControl.Settings.FileName]);
            setup.ScenarioLabel.Text = format3;
            setup.UI.TitleWindow.MainWindow.Drawn = false;
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
                        playerName.Text = setup.UI.Engine.TranslateResourceString(GameTables.HumanText);
                    }
                }
                else
                {
                    playerName.Text = setup.UI.Engine.TranslateResourceString(GameTables.PlayerTypeTexts[settings.Behavior.PlayerType]);
                }
            }

            info.Text = "";
            info.Visible = setup.SetupControl.GetRemote() || settings.Status.Contains(PlayerSetupStatusElement.Fixed);
            if (info.Visible)
            {
                if (settings.Status.Contains(PlayerSetupStatusElement.Fixed))
                {
                    info.Text = setup.UI.Engine.TranslateResourceString(GameText.FixedText);
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
                                info.Text = setup.UI.Engine.TranslateResourceString(GameText.TakenText);
                            }
                            else
                            {
                                info.Text = setup.UI.Engine.TranslateResourceString(GameText.ForcedText);
                            }
                        }
                    }
                    else
                    {
                        if (settings.Behavior.Flags != 0)
                        {
                            info.Text = setup.UI.Engine.TranslateResourceString(GameText.TakenText);
                        }
                        else
                        {
                            if (Set<byte>.WithBounds(0, 255).WithRange(1, 3).WithRange(6, 7).Contains(settings.Behavior.PlayerType))
                            {
                                info.Text = setup.UI.Engine.TranslateResourceString(GameTables.PlayerBehaviorTexts[(int)(PlayerBehavior)settings.BehaviorMode]);
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
                    var playerBehavior = setup.UI.Engine.TranslateResourceString(GameTables.PlayerBehaviorTexts[(int)(PlayerBehavior)B]);
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
            info.Text = setup.UI.Engine.TranslateResourceString(GameText.UnavailableText);
            fog.Visible = true;
        }
    }

}
