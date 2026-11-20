#nullable disable
// Translated from AoWScenSetup.pas.
using System.Collections.Immutable;
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// map-validation delay. setup control. ping delay. chat scroll pending
public partial class ScenarioSetup(UIContext ui) : UIModule(ui)
{
    public Memo ChatMemo { get; private set; }
    public VerticalScrollBar ChatScrollBar { get; private set; }
    public Edit ChatEdit { get; private set; }
    public ListBox PlayerList { get; private set; }
    public Button PickScenarioButton { get; private set; }
    public Button StartButton { get; private set; }
    public Panel ScenarioSetupPanel { get; private set; }
    public Image Side1PlayerIcon { get; private set; }
    public ComboBox Side1PlayerComboBox { get; private set; }
    public HorizontalScrollBar TurnTimeSlider { get; private set; }
    public Label TurnTimeValueLabel { get; private set; }
    public HorizontalScrollBar TurnsSlider { get; private set; }
    public Label TurnsValueLabel { get; private set; }
    public ComboBox PlayModeComboBox { get; private set; }
    public Label Side1InfoLabel { get; private set; }
    public ComboBox Side1BehaviorComboBox { get; private set; }
    public ComboBox Side3BehaviorComboBox { get; private set; }
    public Image Side3PlayerIcon { get; private set; }
    public Label Side3PlayerLabel { get; private set; }
    public ComboBox Side3PlayerComboBox { get; private set; }
    public Label Side3InfoLabel { get; private set; }
    public ComboBox Side2BehaviorComboBox { get; private set; }
    public Image Side2PlayerIcon { get; private set; }
    public Label Side2InfoLabel { get; private set; }
    public ComboBox Side2PlayerComboBox { get; private set; }
    public Label Side2PlayerLabel { get; private set; }
    public Label Side4InfoLabel { get; private set; }
    public Label Side4PlayerLabel { get; private set; }
    public ComboBox Side4PlayerComboBox { get; private set; }
    public Image Side4PlayerIcon { get; private set; }
    public Image Side5PlayerIcon { get; private set; }
    public ComboBox Side5PlayerComboBox { get; private set; }
    public ComboBox Side5BehaviorComboBox { get; private set; }
    public Label Side5InfoLabel { get; private set; }
    public Label Side5PlayerLabel { get; private set; }
    public ComboBox Side6BehaviorComboBox { get; private set; }
    public Image Side6PlayerIcon { get; private set; }
    public Label Side6PlayerLabel { get; private set; }
    public ComboBox Side6PlayerComboBox { get; private set; }
    public Label Side6InfoLabel { get; private set; }
    public ComboBox Side7BehaviorComboBox { get; private set; }
    public Image Side7PlayerIcon { get; private set; }
    public Label Side7PlayerLabel { get; private set; }
    public ComboBox Side7PlayerComboBox { get; private set; }
    public Label Side7InfoLabel { get; private set; }
    public Label Side8InfoLabel { get; private set; }
    public Label Side8PlayerLabel { get; private set; }
    public ComboBox Side8BehaviorComboBox { get; private set; }
    public ComboBox Side8PlayerComboBox { get; private set; }
    public Image Side8PlayerIcon { get; private set; }
    public Image Side9PlayerIcon { get; private set; }
    public ComboBox Side9PlayerComboBox { get; private set; }
    public ComboBox Side9BehaviorComboBox { get; private set; }
    public Label Side9InfoLabel { get; private set; }
    public Label Side9PlayerLabel { get; private set; }
    public ComboBox Side10BehaviorComboBox { get; private set; }
    public Image Side10PlayerIcon { get; private set; }
    public Label Side10PlayerLabel { get; private set; }
    public ComboBox Side10PlayerComboBox { get; private set; }
    public Label Side10InfoLabel { get; private set; }
    public ComboBox Side11BehaviorComboBox { get; private set; }
    public Image Side11PlayerIcon { get; private set; }
    public Label Side11PlayerLabel { get; private set; }
    public ComboBox Side11PlayerComboBox { get; private set; }
    public Label Side11InfoLabel { get; private set; }
    public Label Side12InfoLabel { get; private set; }
    public Label Side12PlayerLabel { get; private set; }
    public ComboBox Side12BehaviorComboBox { get; private set; }
    public ComboBox Side12PlayerComboBox { get; private set; }
    public Image Side12PlayerIcon { get; private set; }
    public ComboBox PreferredRaceComboBox { get; private set; }
    public Label PreferredRaceLabel { get; private set; }
    public CheckBox CustomizeLeadersCheckBox { get; private set; }
    public Label CustomizeLeadersLabel { get; private set; }
    public Button EjectButton { get; private set; }
    public ComboBox Side4BehaviorComboBox { get; private set; }
    public Panel Side1FogPanel { get; private set; }
    public Label Side1PlayerLabel { get; private set; }
    public Panel Side2FogPanel { get; private set; }
    public Panel Side3FogPanel { get; private set; }
    public Panel Side4FogPanel { get; private set; }
    public Panel Side8FogPanel { get; private set; }
    public Panel Side7FogPanel { get; private set; }
    public Panel Side6FogPanel { get; private set; }
    public Panel Side5FogPanel { get; private set; }
    public Panel Side12FogPanel { get; private set; }
    public Panel Side11FogPanel { get; private set; }
    public Panel Side10FogPanel { get; private set; }
    public Panel Side9FogPanel { get; private set; }
    public ListBox PreferredRaceList { get; private set; }
    public ListBox PingList { get; private set; }
    public VerticalScrollBar PlayerListScrollBar { get; private set; }
    public Panel SettingsFogPanel { get; private set; }
    public CheckBox ExplorationCheckBox { get; private set; }
    public Label ScenarioLabel { get; private set; }
    public Label HostNameLabel { get; private set; }
    public Label PlayerNameLabel { get; private set; }
    public Label FileNameLabel { get; private set; }
    public Button MakeHostButton { get; private set; }
    public Panel RestartSettingsFogPanel { get; private set; }
    public Image ChatFogImage { get; private set; }
    public CheckBox LeadersOnMapCheckBox { get; private set; }
    public CheckBox AlliedVictoryCheckBox { get; private set; }
    public Label AlliedVictoryLabel { get; private set; }
    public ComboBox TacticalCombatComboBox { get; private set; }
    public Label TacticalCombatLabel { get; private set; }
    public CheckBox AlwaysViewTacticalCombatCheckBox { get; private set; }
    public Label AlwaysViewTacticalCombatLabel { get; private set; }

    public SetupControl SetupControl { get; set; }
    public bool ChatScrollPending { get; set; }

    // AoWScenSetup.pas:526
    public void Execute(SetupControl control)
    {
        SetupControl = control;
        UI.TitleWindow.MainWindow.Visible = true;
        ScenarioSetupPanel.Visible = true;
        ChatMemo.Items.Clear();
        ChatEdit.Text = "";
        bool remote = SetupControl.GetRemote();
        ChatFogImage.Visible = !remote;
        bool fSetupControlRemote = SetupControl.GetRemote();
        ChatMemo.CaptureInput = fSetupControlRemote;
        bool remote2 = SetupControl.GetRemote();
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
                byte availability = (byte)availabilityOrdinal;
                string tacticalBattleAvailability = UI.Engine.TranslateResourceString(TacticalCombatAvailabilityTexts[availability]);
                _ = TacticalCombatComboBox.Items.AddObject(tacticalBattleAvailability, item: IntToObject(availability));
            }
        }
        else
        {
            TacticalCombatComboBox.Visible = false;
            TacticalCombatLabel.Visible = false;
        }

        if (SetupControl.Settings.ConnectionType == 2)
        {
            CustomizeLeadersLabel.TextColor = 0x80_80_80;
            CustomizeLeadersCheckBox.CaptureInput = false;
            PlayModeComboBox.Items.Clear();
            string classic = UI.Engine.TranslateResourceString(s_classicText);
            _ = PlayModeComboBox.Items.AddObject(classic, item: IntToObject(1));
        }
        else
        {
            CustomizeLeadersLabel.TextColor = AlliedVictoryLabel.TextColor;
            CustomizeLeadersCheckBox.CaptureInput = true;
            PlayModeComboBox.Items.Clear();
            string simultaneous = UI.Engine.TranslateResourceString(s_simultaneousText);
            _ = PlayModeComboBox.Items.AddObject(simultaneous, item: IntToObject(2));
            string classic = UI.Engine.TranslateResourceString(s_classicText);
            _ = PlayModeComboBox.Items.AddObject(classic, item: IntToObject(1));
        }

        bool condition = AudioHost.SongPlayer.CurrentIndex == -1;
        if (!condition)
        {
            int count = AudioHost.SongPlayer.Songs.Count;
            condition = AudioHost.SongPlayer.CurrentIndex >= count;
        }

        if (condition || AudioHost.SongPlayer.Songs[AudioHost.SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
        {
            AudioHost.SongPlayer.Songs.Clear();
            _ = AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            _ = AudioHost.SongPlayer.Play(0);
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
        try
        {
            UI.General.PlayButtonSound(0);
            if (SetupControl.GetRemote())
            {
                if (SetupControl.GetHost())
                {
                    SetupSettings settings = SetupControl.Settings;
                    int i = 0;
                    while (i != settings.RemotePlayers.Count && settings.RemotePlayers.GetPlayers(i).RemoteId == settings.HostRemoteId)
                    {
                        i++;
                    }

                    if (i != settings.RemotePlayers.Count)
                    {
                        SetupControl.SetHostRemoteId(settings.RemotePlayers.GetPlayers(i).RemoteId);
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
                _ = GameHost.MainForm.CloseRequested();
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
        /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
        => throw new NotImplementedException();

    // AoWScenSetup.pas:295
    public void PlayerListDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface lockedSurface)
    {
        if (drawItem < PlayerList.Items.Count)
        {
            object objectValue = PlayerList.Items.Objects[drawItem];
            int i = SetupControl.Settings.RemotePlayers.IndexOfRemoteId(ObjectToInt(objectValue));
            if (i != -1)
            {
                if (SetupControl.Settings.RemotePlayers.GetPlayers(i).MapValidation == MapValidationInfo.Invalid)
                {
                    UI.Fonts.Age8Shadow.Color = 0x00_00_FF;
                    UI.Fonts.Age8Shadow.SetText(drawRectangle.Right - 8, drawRectangle.Top + 4, text: "X", lockedSurface);
                }
                else
                {
                    if (SetupControl.Settings.RemotePlayers.GetPlayers(i).MapValidation == MapValidationInfo.InvalidMap)
                    {
                        UI.Fonts.Age8Shadow.Color = 0x00_80_00;
                        UI.Fonts.Age8Shadow.SetText(drawRectangle.Right - 8, drawRectangle.Top + 4, text: "X", lockedSurface);
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
            object objectValue = PreferredRaceComboBox.Items.Objects[PreferredRaceComboBox.Selected];
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
            object objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            SetupControl.EjectPlayer(ObjectToInt(objectValue));
        }
    }

    // AoWScenSetup.pas:385
    public void MakeHostBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (PlayerList.Selected > -1)
        {
            object objectValue = PlayerList.Items.Objects[PlayerList.Selected];
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
    public void PlayerListChange(ListBox sender) => SelectPlayer(sender.Selected);

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
        object objectValue = PlayModeComboBox.Items.Objects[PlayModeComboBox.Selected];
        SetupControl.SetPlayMode((byte)ObjectToInt(objectValue));
        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWScenSetup.pas:428
    public void SidePlayerComboBoxChange(ComboBox sender, int slot)
    {
        SetupPlayerBehavior behavior = default;
        ComboBox combo = sender;
        string item = combo.Items[combo.Selected];
        if (SetupControl.PlayerNameToPlayerId(item, ref behavior))
        {
            int indexOfRace = SetupControl.Settings.IndexOfRace((byte)slot);
            SetupControl.SetPlayerId(indexOfRace, behavior, forced: true);
        }

        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWScenSetup.pas:437
    public void SideBehaviorComboBoxChange(ComboBox sender, int slot)
    {
        ComboBox control = sender;
        int indexOfRace = SetupControl.Settings.IndexOfRace((byte)slot);
        SetupControl.SetPlayerBehavior(indexOfRace, value: (byte)control.Selected);
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
    public void AlwaysViewTCCBChange() => SetupControl.SetAlwaysViewTacticalBattle(AlwaysViewTacticalCombatCheckBox.Checked);

    // AoWScenSetup.pas:473
    public void SettingsClick() => UI.General.PlayButtonSound(0);

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
        object objectValue = TacticalCombatComboBox.Items.Objects[TacticalCombatComboBox.Selected];
        SetupControl.SetTacticalBattleAvailability((byte)ObjectToInt(objectValue));
    }

    // AoWScenSetup.pas:498
    public void UpdateTurnTime()
    {
        if (TurnTimeSlider.Position == 0)
        {
            string noTimer = UI.Engine.TranslateResourceString(s_noTimerText);
            TurnTimeValueLabel.Text = LowerCase(noTimer);
        }
        else
        {
            string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_minutesFormat), arguments: [IntToStr(TurnTimeSlider.Position)]);
            TurnTimeValueLabel.Text = format;
        }
    }

    // AoWScenSetup.pas:504
    public void UpdateTurnLimit()
    {
        if (TurnsSlider.Position == 0)
        {
            string noLimit = UI.Engine.TranslateResourceString(s_noLimitText);
            TurnsValueLabel.Text = LowerCase(noLimit);
        }
        else
        {
            string format = AbilityRules.Format(LowerCase(UI.Engine.TranslateResourceString(GameText.TurnsFormat)), arguments: [IntToStr(TurnsSlider.Position * 10)]);
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
            object objectValue = PlayerList.Items.Objects[PlayerList.Selected];
            bool canSetHostRemoteId = SetupControl.CanSetHostRemoteId(ObjectToInt(objectValue));
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
        setup.SetupControl = control;
        setup.UI.TitleWindow.MainWindow.Visible = true;
        setup.ScenarioSetupPanel.Visible = true;
        setup.ChatMemo.Items.Clear();
        setup.ChatEdit.Text = "";
        bool remote = setup.SetupControl.GetRemote();
        setup.ChatFogImage.Visible = !remote;
        bool fSetupControlRemote = setup.SetupControl.GetRemote();
        setup.ChatMemo.CaptureInput = fSetupControlRemote;
        bool remote2 = setup.SetupControl.GetRemote();
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
                byte b = (byte)bOrdinal;
                string tacticalBattleAvailability = setup.UI.Engine.TranslateResourceString(TacticalCombatAvailabilityTexts[b]);
                _ = setup.TacticalCombatComboBox.Items.AddObject(tacticalBattleAvailability, item: IntToObject(b));
            }
        }
        else
        {
            setup.TacticalCombatComboBox.Visible = false;
            setup.TacticalCombatLabel.Visible = false;
        }

        if (setup.SetupControl.Settings.ConnectionType == 2)
        {
            setup.CustomizeLeadersLabel.TextColor = 0x80_80_80;
            setup.CustomizeLeadersCheckBox.CaptureInput = false;
            setup.PlayModeComboBox.Items.Clear();
            string classic = setup.UI.Engine.TranslateResourceString(s_classicText);
            _ = setup.PlayModeComboBox.Items.AddObject(classic, item: IntToObject(1));
        }
        else
        {
            setup.CustomizeLeadersLabel.TextColor = setup.AlliedVictoryLabel.TextColor;
            setup.CustomizeLeadersCheckBox.CaptureInput = true;
            setup.PlayModeComboBox.Items.Clear();
            string simultaneous = setup.UI.Engine.TranslateResourceString(s_simultaneousText);
            _ = setup.PlayModeComboBox.Items.AddObject(simultaneous, item: IntToObject(2));
            string classic = setup.UI.Engine.TranslateResourceString(s_classicText);
            _ = setup.PlayModeComboBox.Items.AddObject(classic, item: IntToObject(1));
        }

        bool condition = AudioHost.SongPlayer.CurrentIndex != -1;
        if (condition)
        {
            int count = AudioHost.SongPlayer.Songs.Count;
            condition = AudioHost.SongPlayer.CurrentIndex < count;
        }

        if (!(condition && AudioHost.SongPlayer.Songs[AudioHost.SongPlayer.CurrentIndex] == "SONGS\\TITLE.IT"))
        {
            AudioHost.SongPlayer.Songs.Clear();
            _ = AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            _ = AudioHost.SongPlayer.Play(0);
        }

        UpdateScenarioSetup(setup);
    }

    internal static void UpdateScenarioSetup(ScenarioSetup setup)
    {
        if (setup.SetupControl is not null)
        {
            SetupControl control = setup.SetupControl;
            setup.PlayerList.Items.Clear();
            setup.PingList.Items.Clear();
            setup.PreferredRaceList.Items.Clear();
            for (int i = 0, last = control.Settings.RemotePlayers.Count - 1; i <= last; i++)
            {
                RemotePlayer player = control.Settings.RemotePlayers.GetPlayers(i);
                _ = setup.PlayerList.Items.AddObject(player.Name, item: IntToObject(player.RemoteId));
                if (player.PreferredRace != 255)
                {
                    string raceNamesPlural = setup.UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[(sbyte)player.PreferredRace]);
                    _ = setup.PreferredRaceList.Items.Add(raceNamesPlural);
                }
                else
                {
                    string none = setup.UI.Engine.TranslateResourceString(GameTables.NoneText);
                    _ = setup.PreferredRaceList.Items.Add(none);
                }

                _ = setup.PingList.Items.AddObject("", item: IntToObject(player.RemoteId));
            }

            int count = setup.PlayerList.Items.Count;
            setup.PlayerListScrollBar.Max = count - 1;
            setup.SelectPlayer(setup.PlayerList.Selected);
            setup.CustomizeLeadersCheckBox.Checked = setup.SetupControl.Settings.CustomizeLeaders;
            int indexOfObject = setup.PlayModeComboBox.Items.IndexOfObject(IntToObject(setup.SetupControl.Settings.PlayMode));
            setup.PlayModeComboBox.Selected = indexOfObject;
            setup.ExplorationCheckBox.Checked = setup.SetupControl.Settings.ExplorationEnabled;
            setup.TurnTimeSlider.Position = setup.SetupControl.Settings.TurnTime;
            setup.TurnsSlider.Position = setup.SetupControl.Settings.DayLimit != 0 ? setup.SetupControl.Settings.DayLimit / 10 : 0;

            setup.UpdateTurnLimit();
            setup.UpdateTurnTime();
            bool condition = setup.SetupControl.GetHost() && setup.SetupControl.GetPlayerSetupValid();
            setup.StartButton.Visible = condition;
            bool host = setup.SetupControl.GetHost();
            setup.PickScenarioButton.Visible = host;
            bool fSetupControlHost = setup.SetupControl.GetHost();
            setup.SettingsFogPanel.Visible = !fSetupControlHost;
            setup.RestartSettingsFogPanel.Visible = !setup.SettingsFogPanel.Visible && setup.SetupControl.Settings.MapDay != 0;
            setup.LeadersOnMapCheckBox.Checked = setup.SetupControl.Settings.LeadersOnMap;
            setup.AlliedVictoryCheckBox.Checked = setup.SetupControl.Settings.AlliedVictory;
            int itemsIndexOfObject = setup.TacticalCombatComboBox.Items.IndexOfObject(IntToObject(setup.SetupControl.Settings.TacticalBattleAvailability));
            setup.TacticalCombatComboBox.Selected = itemsIndexOfObject;
            setup.AlwaysViewTacticalCombatCheckBox.Visible = setup.TacticalCombatComboBox.Visible && setup.SetupControl.Settings.TacticalBattleAvailability != 0;
            setup.AlwaysViewTacticalCombatCheckBox.Checked = setup.SetupControl.Settings.AlwaysViewTacticalBattle;
            setup.AlwaysViewTacticalCombatLabel.Visible = setup.AlwaysViewTacticalCombatCheckBox.Visible;
            bool remote = setup.SetupControl.GetRemote();
            setup.PreferredRaceLabel.Visible = remote;
            bool fSetupControlRemote = setup.SetupControl.GetRemote();
            setup.PreferredRaceComboBox.Visible = fSetupControlRemote;
            if (setup.PreferredRaceComboBox.Visible)
            {
                setup.PreferredRaceComboBox.Items.Clear();
                string none = setup.UI.Engine.TranslateResourceString(GameTables.NoneText);
                _ = setup.PreferredRaceComboBox.Items.AddObject(none, item: IntToObject(-1));
                for (int i = 0, last = setup.SetupControl.Settings.Count - 1; i <= last; i++)
                {
                    if (!setup.SetupControl.Settings.GetSettings(i).Status.Contains(PlayerSetupStatusElement.Fixed))
                    {
                        PlayerSetupSettings settingRace = setup.SetupControl.Settings.GetSettings(i);
                        string raceNamesPlural = setup.UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[settingRace.Race]);
                        PlayerSetupSettings setting = setup.SetupControl.Settings.GetSettings(i);
                        _ = setup.PreferredRaceComboBox.Items.AddObject(raceNamesPlural, item: IntToObject(setting.Race));
                    }
                }

                ComboBox comboBox = setup.PreferredRaceComboBox;
                comboBox.NumberVisible = 5;
                comboBox.Sized = false;
                comboBox.Selected = comboBox.Items.IndexOfObject(IntToObject((sbyte)setup.SetupControl.RemotePlayer.PreferredRace));
            }

            int indexOfRace = control.Settings.IndexOfRace(0);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace), playerBox: setup.Side1PlayerComboBox, behaviorBox: setup.Side1BehaviorComboBox, icon: setup.Side1PlayerIcon, playerName: setup.Side1PlayerLabel, info: setup.Side1InfoLabel, fog: setup.Side1FogPanel);
            int fSettingsIndexOfRace = control.Settings.IndexOfRace(1);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(fSettingsIndexOfRace), playerBox: setup.Side2PlayerComboBox, behaviorBox: setup.Side2BehaviorComboBox, icon: setup.Side2PlayerIcon, playerName: setup.Side2PlayerLabel, info: setup.Side2InfoLabel, fog: setup.Side2FogPanel);
            int indexOfRace2 = control.Settings.IndexOfRace(2);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace2), playerBox: setup.Side3PlayerComboBox, behaviorBox: setup.Side3BehaviorComboBox, icon: setup.Side3PlayerIcon, playerName: setup.Side3PlayerLabel, info: setup.Side3InfoLabel, fog: setup.Side3FogPanel);
            int indexOfRace3 = control.Settings.IndexOfRace(3);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace3), playerBox: setup.Side4PlayerComboBox, behaviorBox: setup.Side4BehaviorComboBox, icon: setup.Side4PlayerIcon, playerName: setup.Side4PlayerLabel, info: setup.Side4InfoLabel, fog: setup.Side4FogPanel);
            int indexOfRace4 = control.Settings.IndexOfRace(4);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace4), playerBox: setup.Side5PlayerComboBox, behaviorBox: setup.Side5BehaviorComboBox, icon: setup.Side5PlayerIcon, playerName: setup.Side5PlayerLabel, info: setup.Side5InfoLabel, fog: setup.Side5FogPanel);
            int indexOfRace5 = control.Settings.IndexOfRace(5);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace5), playerBox: setup.Side6PlayerComboBox, behaviorBox: setup.Side6BehaviorComboBox, icon: setup.Side6PlayerIcon, playerName: setup.Side6PlayerLabel, info: setup.Side6InfoLabel, fog: setup.Side6FogPanel);
            int indexOfRace6 = control.Settings.IndexOfRace(6);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace6), playerBox: setup.Side7PlayerComboBox, behaviorBox: setup.Side7BehaviorComboBox, icon: setup.Side7PlayerIcon, playerName: setup.Side7PlayerLabel, info: setup.Side7InfoLabel, fog: setup.Side7FogPanel);
            int indexOfRace7 = control.Settings.IndexOfRace(7);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace7), playerBox: setup.Side8PlayerComboBox, behaviorBox: setup.Side8BehaviorComboBox, icon: setup.Side8PlayerIcon, playerName: setup.Side8PlayerLabel, info: setup.Side8InfoLabel, fog: setup.Side8FogPanel);
            int indexOfRace8 = control.Settings.IndexOfRace(8);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace8), playerBox: setup.Side9PlayerComboBox, behaviorBox: setup.Side9BehaviorComboBox, icon: setup.Side9PlayerIcon, playerName: setup.Side9PlayerLabel, info: setup.Side9InfoLabel, fog: setup.Side9FogPanel);
            int indexOfRace9 = control.Settings.IndexOfRace(9);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace9), playerBox: setup.Side10PlayerComboBox, behaviorBox: setup.Side10BehaviorComboBox, icon: setup.Side10PlayerIcon, playerName: setup.Side10PlayerLabel, info: setup.Side10InfoLabel, fog: setup.Side10FogPanel);
            int indexOfRace10 = control.Settings.IndexOfRace(10);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace10), playerBox: setup.Side11PlayerComboBox, behaviorBox: setup.Side11BehaviorComboBox, icon: setup.Side11PlayerIcon, playerName: setup.Side11PlayerLabel, info: setup.Side11InfoLabel, fog: setup.Side11FogPanel);
            int indexOfRace11 = control.Settings.IndexOfRace(11);
            UpdateScenarioRow(setup, settings: control.Settings.GetSettings(indexOfRace11), playerBox: setup.Side12PlayerComboBox, behaviorBox: setup.Side12BehaviorComboBox, icon: setup.Side12PlayerIcon, playerName: setup.Side12PlayerLabel, info: setup.Side12InfoLabel, fog: setup.Side12FogPanel);
            if (setup.SetupControl.GetRemote())
            {
                string connection = setup.UI.SetupEvents.DirectPlay.ConnectionType switch
                {
                    ConnectionType.TCP => setup.UI.Engine.TranslateResourceString(GameText.TcpIpLabel),
                    ConnectionType.IPX => setup.UI.Engine.TranslateResourceString(GameText.IpxText),
                    ConnectionType.Modem => setup.UI.Engine.TranslateResourceString(GameText.ModemText),
                    ConnectionType.Serial => setup.UI.Engine.TranslateResourceString(GameText.SerialText),
                    ConnectionType.None or ConnectionType.Unknown or _ => "",
                };
                SetupSettings setupSettings = setup.SetupControl.Settings;
                int hostIndex = setupSettings.RemotePlayers.IndexOfRemoteId(setupSettings.HostRemoteId);

                string hostName = hostIndex != -1 ? setup.SetupControl.Settings.RemotePlayers.GetPlayers(hostIndex).Name : "?";

                setup.HostNameLabel.Visible = true;
                setup.FileNameLabel.Visible = true;
                setup.PlayerNameLabel.Visible = true;
                string format = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.HostFormat), arguments: [connection, hostName]);
                setup.HostNameLabel.Text = format;
                string formattedText = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.PlayerNameFormat), arguments: [setup.SetupControl.RemotePlayer.Name]);
                setup.PlayerNameLabel.Text = formattedText;
                string format2 = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.FileNameFormat), arguments: [setup.SetupControl.Settings.MapName]);
                setup.FileNameLabel.Text = format2;
                setup.FileNameLabel.TextColor = setup.SetupControl.RemotePlayer.MapValidation == MapValidationInfo.Invalid ? 255 : 10_329_501;
            }
            else
            {
                setup.HostNameLabel.Visible = false;
                setup.FileNameLabel.Visible = false;
                setup.PlayerNameLabel.Visible = false;
            }

            string format3 = AbilityRules.Format(setup.UI.Engine.TranslateResourceString(GameText.ScenarioFormat), arguments: [setup.SetupControl.Settings.FileName]);
            setup.ScenarioLabel.Text = format3;
            setup.UI.TitleWindow.MainWindow.Drawn = false;
        }
    }

    internal static void UpdateScenarioRow(ScenarioSetup setup, PlayerSetupSettings settings, ComboBox playerBox, ComboBox behaviorBox, Image icon, Label playerName, Label info, Panel fog)
    {
        if (settings is not null)
        {
            bool visible = setup.SetupControl.GetHost() && !settings.Status.Contains(PlayerSetupStatusElement.Fixed);
            if (visible != playerBox.Visible)
            {
                playerBox.Visible = visible;
            }

            int i;
            if (playerBox.Visible)
            {
                playerBox.Items.BeginUpdate();
                playerBox.Items.Clear();
                setup.SetupControl.ListAvailablePlayers(playerBox.Items);
                playerBox.Items.EndUpdate();
                bool found = false;
                i = playerBox.Items.Count;
                SetupPlayerBehavior behavior = default;
                while (!found && i != 0)
                {
                    i -= 1;
                    string item = playerBox.Items[i];
                    if (setup.SetupControl.PlayerNameToPlayerId(item, ref behavior))
                    {
                        found = settings.Behavior.PlayerType == behavior.PlayerType && settings.Behavior.Flags == behavior.Flags;
                    }
                }

                playerBox.Selected = found ? i : -1;

                playerName.Visible = false;
            }
            else
            {
                playerName.Visible = true;
                if (settings.Behavior.PlayerType == 0 && settings.Behavior.Flags != 0)
                {
                    i = setup.SetupControl.Settings.RemotePlayers.IndexOfRemoteId(settings.Behavior.Flags);
                    playerName.Text = i != -1
                        ? setup.SetupControl.Settings.RemotePlayers.GetPlayers(i).Name
                        : setup.UI.Engine.TranslateResourceString(GameTables.HumanText);
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
                        i = setup.SetupControl.Settings.RemotePlayers.IndexOfRemoteId(settings.Behavior.Flags);
                        if (i != -1)
                        {
                            // { Original compares race bytes, including the FF sentinel. }
                            info.Text = setup.SetupControl.Settings.RemotePlayers.GetPlayers(i).PreferredRace == (byte)settings.Race
                                ? setup.UI.Engine.TranslateResourceString(GameText.TakenText)
                                : setup.UI.Engine.TranslateResourceString(GameText.ForcedText);
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
                    byte b = (byte)bOrdinal;
                    string playerBehavior = setup.UI.Engine.TranslateResourceString(GameTables.PlayerBehaviorTexts[(int)(PlayerBehavior)b]);
                    _ = behaviorBox.Items.AddObject(playerBehavior, item: IntToObject(b));
                }

                behaviorBox.Items.EndUpdate();
                behaviorBox.Selected = settings.BehaviorMode;
            }

            icon.Visible = Set<byte>.WithBounds(0, 255).WithRange(0, 3).WithRange(6, 7).Contains(settings.Behavior.PlayerType);
            if (icon.Visible)
            {
                icon.CurrentImage = settings.Behavior.PlayerType switch
                {
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    6 => 4,
                    7 => 5,
                    _ => 0,
                };
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
