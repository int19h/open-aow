#nullable disable
// Translated from AoWRelations.pas.
using Aow.Engine.Collections;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// diplomatic actions, player indices, race indices, controls
public partial class RelationWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; set; }
    public Image RightShield { get; private set; }
    public Image LeftShield { get; private set; }
    public Panel RelationPanel { get; set; }
    public Panel IncomingPanel { get; set; }
    public Image Player1Image { get; set; }
    public Image Player2Image { get; set; }
    public Image Player3Image { get; set; }
    public Image Player4Image { get; set; }
    public Image Player5Image { get; set; }
    public Image Player6Image { get; set; }
    public Image Player7Image { get; set; }
    public Image Player8Image { get; set; }
    public Image Player9Image { get; set; }
    public Image Player10Image { get; set; }
    public Image Player11Image { get; set; }
    public Image Player12Image { get; set; }
    public Image Race1Image { get; set; }
    public Image Race2Image { get; set; }
    public Image Race3Image { get; set; }
    public Image Race4Image { get; set; }
    public Image Race5Image { get; set; }
    public Image Race6Image { get; set; }
    public Image Race7Image { get; set; }
    public Image Race8Image { get; set; }
    public Image Race9Image { get; set; }
    public Image Race10Image { get; set; }
    public Image Race11Image { get; set; }
    public Image Race12Image { get; set; }
    public Image FaceShadow { get; private set; }
    public Memo IncomingMemo { get; set; }
    public Image IncomingIcon { get; private set; }
    public Panel MessageEditPanel { get; set; }
    public Panel SendBribePanel { get; set; }
    public Memo SendMessageMemo { get; set; }
    public HorizontalScrollBar BribeGoldSlider { get; set; }
    public Panel ActionPanel { get; private set; }
    public Panel PlayerInfoPanel { get; private set; }
    public Panel NewActionPanel { get; set; }
    public Label PlayerNameLabel { get; private set; }
    public Panel RaceInfoPanel { get; private set; }
    public Label RaceNameLabel { get; private set; }
    public Image AlignmentImage { get; private set; }
    public Label AlignmentLabel { get; private set; }
    public CheckBox Player1CheckBox { get; set; }
    public CheckBox Player2CheckBox { get; set; }
    public CheckBox Player3CheckBox { get; set; }
    public CheckBox Player4CheckBox { get; set; }
    public CheckBox Player5CheckBox { get; set; }
    public CheckBox Player6CheckBox { get; set; }
    public CheckBox Player7CheckBox { get; set; }
    public CheckBox Player8CheckBox { get; set; }
    public CheckBox Player9CheckBox { get; set; }
    public CheckBox Player10CheckBox { get; set; }
    public CheckBox Player11CheckBox { get; set; }
    public CheckBox Player12CheckBox { get; set; }
    public Image FaceFrame { get; private set; }
    public CheckBox Race1CheckBox { get; set; }
    public CheckBox Race2CheckBox { get; set; }
    public CheckBox Race3CheckBox { get; set; }
    public CheckBox Race4CheckBox { get; set; }
    public CheckBox Race5CheckBox { get; set; }
    public CheckBox Race6CheckBox { get; set; }
    public CheckBox Race7CheckBox { get; set; }
    public CheckBox Race8CheckBox { get; set; }
    public CheckBox Race9CheckBox { get; set; }
    public CheckBox Race10CheckBox { get; set; }
    public CheckBox Race11CheckBox { get; set; }
    public CheckBox Race12CheckBox { get; set; }
    public Image PlayerGlow { get; set; }
    public Label NoContactLabel { get; private set; }
    public Button SelectActionButton { get; set; }
    public ListBox<DiplomaticAction> ActionsList { get; set; }
    public Image PlayerTypeIcon { get; private set; }
    public Label PlayerTypeLabel { get; private set; }
    public Image PlayerStatusIcon { get; private set; }
    public Label PlayerStatusLabel { get; private set; }
    public Label PlayerStatusValueLabel { get; private set; }
    public Image RaceRelationIcon { get; private set; }
    public Label RaceRelationValueLabel { get; private set; }
    public Label ReceivedGoldLabel { get; private set; }
    public Label ReceivedManaLabel { get; private set; }
    public Label BribeGoldValueLabel { get; set; }
    public Label BribeManaValueLabel { get; private set; }
    public HorizontalScrollBar BribeManaSlider { get; private set; }
    public Image SendBribeIcon { get; private set; }
    public Label BribeHeaderLabel { get; private set; }
    public Panel ReceivedMessagePanel { get; set; }
    public Memo ReceivedMessageMemo { get; private set; }
    public Label ReceivedMessageHeaderLabel { get; private set; }
    public Panel IncomingInfoPanel { get; set; }
    public Memo IncomingInfoMemo { get; private set; }
    public Image IncomingInfoIcon { get; private set; }
    public Label OfTheLabel { get; private set; }
    public Label PlayerRaceLabel { get; private set; }

    public int SelectedIndex { get; private set; }

    private DiplomaticAction _action;
    private DiplomaticActionList _actions;

    public sbyte InfoPlayer { get; private set; }
    public sbyte InfoRace { get; private set; }
    public sbyte SelectedPlayer { get; private set; }
    public sbyte SelectedRace { get; private set; }
    public Int32List Players { get; private set; }
    public Int32List Races { get; private set; }
    public List<Image> FImages { get; private set; }
    public List<CheckBox> FChecks { get; private set; }

    private event ExecuteDoneEvent Done;
    private DiplomaticActionEventLog _event;
    private int _glowIntensity;
    private bool _glowIncreasing;

    // Slots 0..23 are images and slots 24..47 are check boxes, in RelWindowCreate order.
    // An index outside 0..47 throws.
    private UIComponent ControlAt(int Index) => Index is >= 0 and < 24 ? FImages[Index] : FChecks[Index - 24];

    // AoWRelations.pas:221
    public void FillPlayers()
    {
        Players.Clear();
        for (int i = 1, last = StrategicMap.Current.Players.GetCount() - 1; i <= last; i++)
        {
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)i) != -1 && StrategicMap.Current.Players.GetPlayers(i).Status == PlayerStatus.Playing)
            {
                _ = Players.Add(i);
            }
        }

        for (int i = 1; i <= 12; i++)
        {
            CheckBox check = FChecks[i - 1];
            if (i <= Players.Count)
            {
                check.Visible = true;
                check.ToolTipText.Clear();
                int player = Players[i - 1];
                string name = StrategicMap.Current.Players.GetPlayers(player).GetName();
                _ = check.ToolTipText.Add(name);
            }
            else
            {
                check.Visible = false;
            }
        }

        int indexOf = Players.IndexOf(StrategicMap.Current.SeatedPlayer);
        PlayerGlow.Alignment.LeftOffset = (indexOf * 51) - 37 + 51;
        PlayerGlow.Aligned = false;
    }

    // AoWRelations.pas:241
    public void ShowRelations(int player)
    {
        int index;
        ClearActions();
        ResetChecks();
        UI.General.Manager.OverlayPriority = 49;
        Races.Clear();
        for (int i = 0, last = StrategicMap.Current.Races.GetCount() - 1; i <= last; i++)
        {
            if (StrategicMap.Current.Races.GetRace(i).Enabled)
            {
                _ = Races.Add(i);
            }
        }

        for (int i = 0; i <= 11; i++)
        {
            CheckBox check = FChecks[i + 12];
            if (i < Races.Count)
            {
                check.Visible = true;
                check.ToolTipText.Clear();
                int race = Races[i];
                Race fRacesRace = StrategicMap.Current.Races.GetRace(race);
                string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[fRacesRace.RaceIndex]);
                _ = check.ToolTipText.Add(raceNamesPlural);
            }
            else
            {
                check.Visible = false;
            }
        }

        FillPlayers();
        _glowIntensity = 50;
        _glowIncreasing = false;
        MainWindow.Visible = true;
        if (ControlAt(player + 23) is CheckBox playerCheck)
        {
            playerCheck.Checked = true;
        }

        index = Players.IndexOf(StrategicMap.Current.SeatedPlayer);
        UIComponent seated = ControlAt(index + 24);
        if (seated is CheckBox seatedCheck)
        {
            seatedCheck.Checked = true;
        }

        P1CBChange(seated, index);
        StrategicMap.Current.TutorialControl.ShowMessage(65, true, true);
    }

    // AoWRelations.pas:270
    public void ResetChecks()
    {
        SelectedIndex = -1;
        RelationPanel.Aligned = false;
        MainWindow.Drawn = false;
        PlayerGlow.Alignment.TopOffset = 10;
        for (int i = 0; i <= 11; i++)
        {
            CheckBox check = FChecks[i];
            check.Checked = false;
            check.Alignment.TopOffset = 10;
        }

        for (int i = 12; i <= 23; i++)
        {
            CheckBox check = FChecks[i];
            check.Checked = false;
            check.Alignment.TopOffset = 185;
        }
    }

    // AoWRelations.pas:293
    public virtual void ShowPlayerActions(sbyte player)
    {
        int selected;
        ClearActions();
        HighlightPlayer(player);
        ShowPlayerInformation(player);
        selected = ActionsList.Selected;
        _ = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).ListDiplomaticActions(player, ref _actions);
        for (int i = 0, last = _actions.GetCount() - 1; i <= last; i++)
        {
            string name = _actions.GetDiplomaticActions(i).GetName(StrategicMap.Current.SeatedPlayer);
            DiplomaticAction diplomaticAction = _actions.GetDiplomaticActions(i);
            _ = ActionsList.Items.AddObject(name, diplomaticAction);
        }

        ActionsList.Selected = selected;
        ActionsLBChange();
        ActionPanel.Visible = true;
    }

    // AoWRelations.pas:309
    public void ShowPlayerInformation(sbyte player)
    {
        DiplomaticRelation relation;
        InfoPlayer = player;
        InfoRace = -1;
        FaceShadow.Visible = true;
        NoContactLabel.Visible = false;
        PlayerStatusValueLabel.Visible = true;
        PlayerStatusIcon.Visible = true;
        PlayerStatusLabel.Visible = true;
        PlayerInfoPanel.Visible = true;
        RaceInfoPanel.Visible = false;
        Player playerPlayerRace = StrategicMap.Current.Players.GetPlayers(player);
        LeftShield.CurrentImage = playerPlayerRace.PlayerRace + 26;
        LeftShield.Visible = true;
        RightShield.CurrentImage = LeftShield.CurrentImage;
        RightShield.Visible = true;
        int firstDaySeen = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen(player);
        NoContactLabel.Visible = firstDaySeen == -1;
        PlayerNameLabel.Visible = !NoContactLabel.Visible;
        OfTheLabel.Visible = PlayerNameLabel.Visible;
        PlayerRaceLabel.Visible = PlayerNameLabel.Visible;
        if (StrategicMap.Current.Players.GetPlayers(player).GetFirstDaySeen(StrategicMap.Current.SeatedPlayer) != -1)
        {
            if (player != StrategicMap.Current.SeatedPlayer)
            {
                relation = StrategicMap.Current.Players.GetDiplomaticRelation(player, StrategicMap.Current.SeatedPlayer);
                string diplomaticRelation = UI.Engine.TranslateResourceString(GameTables.DiplomaticRelationTexts[(int)relation]);
                PlayerStatusValueLabel.Text = diplomaticRelation;
                PlayerStatusIcon.CurrentImage = relation != DiplomaticRelation.Unknown ? (int)relation + 8 : 50;
            }
            else
            {
                PlayerStatusIcon.Visible = false;
                PlayerStatusLabel.Visible = false;
                PlayerStatusValueLabel.Visible = false;
            }
        }
        else
        {
            PlayerStatusIcon.Visible = false;
            string noContact = UI.Engine.TranslateResourceString(s_noContactText);
            PlayerStatusValueLabel.Text = noContact;
        }

        string name = StrategicMap.Current.Players.GetPlayers(player).GetName();
        PlayerNameLabel.Text = name;
        OfTheLabel.Text = UI.PlayerInfoWindow.OfTheLabel.Text;
        Player playerLocal = StrategicMap.Current.Players.GetPlayers(player);
        string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[playerLocal.PlayerRace]);
        PlayerRaceLabel.Text = raceNamesPlural;
        Player playerPlayerType = StrategicMap.Current.Players.GetPlayers(player);
        if (Set<PlayerType>.Of(PlayerType.Human, PlayerType.Cpu1, PlayerType.Cpu2, PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(playerPlayerType.PlayerType))
        {
            PlayerTypeIcon.CurrentImage = StrategicMap.Current.Players.GetPlayers(player).PlayerType switch
            {
                PlayerType.Cpu1 => 1,
                PlayerType.Cpu2 => 2,
                PlayerType.Cpu3 => 3,
                PlayerType.Cpu4 => 4,
                PlayerType.Cpu5 => 5,
                PlayerType.Human or PlayerType.Independent or PlayerType.Free or _ => 0,
            };
            PlayerTypeIcon.Visible = true;
        }
        else
        {
            PlayerTypeIcon.Visible = false;
        }

        Player player2 = StrategicMap.Current.Players.GetPlayers(player);
        string playerType = UI.Engine.TranslateResourceString(GameTables.PlayerTypeTexts[(int)player2.PlayerType]);
        PlayerTypeLabel.Text = playerType;
    }

    // AoWRelations.pas:370
    public void HighlightPlayer(sbyte player)
    {
        ResetChecks();
        SelectedPlayer = player;
        SelectedRace = -1;
        SelectedIndex = Players.IndexOf(player);
        if (SelectedIndex != -1)
        {
            CheckBox check = FChecks[SelectedIndex];
            check.Alignment.TopOffset = 20;
            check.Checked = true;
            if (StrategicMap.Current.SeatedPlayer == player)
            {
                PlayerGlow.Alignment.TopOffset = check.Alignment.TopOffset;
            }
        }

        for (int i = 0; i <= 11; i++)
        {
            Image image = FImages[i];
            if (i < Players.Count)
            {
                image.Visible = true;
                image.CurrentImage = Players[i] == player ? 2 : i > Races.Count ? 0 : 1;
            }
            else
            {
                image.CurrentImage = 2;
            }

            switch (image.CurrentImage)
            {
                case 1:
                    image.ToolTipText.Clear();
                    if (SelectedPlayer != -1)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                        {
                            int playerLocal = Players[i];
                            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)playerLocal) != -1)
                            {
                                int player2 = Players[i];
                                if (StrategicMap.Current.Players.GetPlayers(SelectedPlayer).GetFirstDaySeen((sbyte)player2) != -1)
                                {
                                    int player3 = Players[i];
                                    DiplomaticRelation diplomaticRelation = StrategicMap.Current.Players.GetDiplomaticRelation(SelectedPlayer, (sbyte)player3);
                                    _ = image.ToolTipText.Add(GameTables.DiplomaticRelationTexts[(int)diplomaticRelation]);
                                }
                            }
                        }
                    }

                    break;
                default:
                    image.ToolTipText.Clear();
                    break;
            }
        }

        for (int i = 0; i <= 11; i++)
        {
            Image image = FImages[i + 12];
            image.CurrentImage = i < Races.Count ? 4 : 5;

            switch (image.CurrentImage)
            {
                case 4:
                    image.ToolTipText.Clear();
                    if (SelectedPlayer != -1)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                        {
                            int race = Races[i];
                            Status playerRelation = StrategicMap.Current.Races.GetRace(race).GetPlayerRelation(SelectedPlayer);
                            string status = UI.Engine.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
                            int race2 = Races[i];
                            int playerRelationValue = StrategicMap.Current.Races.GetRace(race2).GetPlayerRelationValue(SelectedPlayer);
                            _ = image.ToolTipText.Add(status + " (" + IntToStr(playerRelationValue) + ")");
                        }
                    }

                    break;
                default:
                    image.ToolTipText.Clear();
                    break;
            }
        }
    }

    // AoWRelations.pas:427
    public void ShowRaceInformation(byte race)
    {
        InfoPlayer = -1;
        InfoRace = (sbyte)race;
        LeftShield.Visible = false;
        RightShield.Visible = false;
        FaceShadow.Visible = true;
        ActionPanel.Visible = true;
        PlayerInfoPanel.Visible = false;
        RaceInfoPanel.Visible = true;
        Status playerRelation = StrategicMap.Current.Races.GetRace((sbyte)race).GetPlayerRelation(StrategicMap.Current.SeatedPlayer);
        RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        Status playerRelation2 = StrategicMap.Current.Races.GetRace((sbyte)race).GetPlayerRelation(StrategicMap.Current.SeatedPlayer);
        string status = UI.Engine.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation2]);
        RaceRelationValueLabel.Text = status;
        string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[(sbyte)race]);
        RaceNameLabel.Text = raceNamesPlural;
        switch (race)
        {
            case >= 0 and <= 3:
                string neutral = UI.Engine.TranslateResourceString(GameTables.NeutralText);
                string format = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), [neutral]);
                AlignmentLabel.Text = format;
                AlignmentImage.CurrentImage = 33;
                break;
            case >= 4 and <= 7:
                string good = UI.Engine.TranslateResourceString(s_goodText);
                string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), [good]);
                AlignmentLabel.Text = formattedText;
                AlignmentImage.CurrentImage = 31;
                break;
            case >= 8 and <= 11:
                string evil = UI.Engine.TranslateResourceString(s_evilText);
                string format2 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), [evil]);
                AlignmentLabel.Text = format2;
                AlignmentImage.CurrentImage = 35;
                break;
            default:
                break;
        }
    }

    // AoWRelations.pas:456
    public void HighlightRace(sbyte race)
    {
        int index;
        ResetChecks();
        SelectedPlayer = -1;
        SelectedRace = race;
        index = Races.IndexOf(race);
        // A missing race yields -1, and check 11 is P12CB, the old slot 35.
        CheckBox check = FChecks[index + 12];
        check.Alignment.TopOffset = 175;
        check.Checked = true;
        SelectedIndex = 20 + index;
        for (int i = 0; i <= 11; i++)
        {
            Image image = FImages[i];
            image.CurrentImage = i < Players.Count ? 1 : 2;

            switch (image.CurrentImage)
            {
                case 1:
                    image.ToolTipText.Clear();
                    int player = Players[i];
                    if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                    {
                        int player2 = Players[i];
                        Status playerRelation = StrategicMap.Current.Races.GetRace(SelectedRace).GetPlayerRelation((sbyte)player2);
                        string status = UI.Engine.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
                        int player3 = Players[i];
                        int playerRelationValue = StrategicMap.Current.Races.GetRace(SelectedRace).GetPlayerRelationValue((sbyte)player3);
                        _ = image.ToolTipText.Add(status + " (" + IntToStr(playerRelationValue) + ")");
                    }

                    break;
                default:
                    image.ToolTipText.Clear();
                    break;
            }
        }

        for (int i = 0; i <= 11; i++)
        {
            Image image = FImages[i + 12];
            image.CurrentImage = 5;
            image.ToolTipText.Clear();
        }
    }

    // AoWRelations.pas:490
    public void SetAction(DiplomaticAction action)
    {
        if (action != _action)
        {
            _ = (_action?.Release());

            _action = action;
            _ = (_action?.AddRef());
        }
    }

    // AoWRelations.pas:500
    public void FinishEvent()
    {
        ExecuteDoneEvent done;
        DiplomaticActionEventLog pending;
        DiplomaticActionEventLog eventValue;
        pending = _event;
        if (pending != null)
        {
            done = Done;
            eventValue = pending;
            Done = null;
            _event = null;
            done?.Invoke(eventValue);

            _ = (eventValue?.Release());
        }
    }

    // AoWRelations.pas:516
    public void FinishDialog()
    {
        SetAction(null);
        UI.General.Manager.OverlayPriority = 0;
        FinishEvent();
        StrategicMap.Current.TutorialControl.ShowMessage(71, true, true);
    }

    // AoWRelations.pas:524
    public void ClearActions()
    {
        SetAction(null);
        NewActionPanel.Visible = true;
        IncomingPanel.Visible = false;
        MessageEditPanel.Visible = false;
        SendBribePanel.Visible = false;
        ReceivedMessagePanel.Visible = false;
        IncomingInfoPanel.Visible = false;
        _actions.Clear();
        ActionsList.Items.Clear();
        SendMessageMemo.Items.Clear();
        IncomingMemo.Items.Clear();
        ActionsLBChange();
    }

    // AoWRelations.pas:540
    public void RelationWindowCreate()
    {
        _actions = new DiplomaticActionList(null) { Holder = UI.Engine };
        Races = new Int32List();
        Players = new Int32List();
        FImages =
        [
            Player1Image, Player2Image, Player3Image, Player4Image, Player5Image, Player6Image, Player7Image, Player8Image, Player9Image, Player10Image, Player11Image, Player12Image,
            Race1Image, Race2Image, Race3Image, Race4Image, Race5Image, Race6Image, Race7Image, Race8Image, Race9Image, Race10Image, Race11Image, Race12Image,
        ];
        FChecks =
        [
            Player1CheckBox, Player2CheckBox, Player3CheckBox, Player4CheckBox, Player5CheckBox, Player6CheckBox, Player7CheckBox, Player8CheckBox, Player9CheckBox, Player10CheckBox, Player11CheckBox, Player12CheckBox,
            Race1CheckBox, Race2CheckBox, Race3CheckBox, Race4CheckBox, Race5CheckBox, Race6CheckBox, Race7CheckBox, Race8CheckBox, Race9CheckBox, Race10CheckBox, Race11CheckBox, Race12CheckBox,
        ];
    }

    // AoWRelations.pas:596
    public void RelationWindowDestroy()
    {
        _actions?.Dispose();
        FImages?.Clear();
        FChecks?.Clear();
        Players?.Dispose();
        Races?.Dispose();
    }

    // AoWRelations.pas:604
    public void ExitClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWRelations.pas:610
    public void P1CBChange(UIComponent sender, int slot)
    {
        // The original SetChecked call on an image is undefined, so skipped. An image has FChecked and no SetChecked. The action branch runs only for a check box.
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }

        RelationPanel.Aligned = false;
        MainWindow.Drawn = false;
        if (sender.Checked && sender is CheckBox)
        {
            if (_action == null)
            {
                ShowPlayerActions((sbyte)Players[slot]);
            }
            else
            {
                HighlightPlayer((sbyte)Players[slot]);
            }
        }
    }

    // AoWRelations.pas:623
    public void R1CBChange(CheckBox sender, int slot)
    {
        CheckBox check;
        check = sender;
        if (check.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }

        RelationPanel.Aligned = false;
        if (_action == null)
        {
            ClearActions();
            HighlightRace((sbyte)Races[slot - 20]);
            ShowRaceInformation((byte)Races[slot - 20]);
        }
        else
        {
            HighlightRace((sbyte)Races[slot - 20]);
        }
    }

    // AoWRelations.pas:639
    public void P1CBPreDraw(CheckBox sender, LockedSurface surface, int slot)
    {
        sbyte player2;
        Rect r;
        UIComponent component;
        component = sender;
        if (slot < Players.Count)
        {
            player2 = (sbyte)Players[slot];
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen(player2) == -1)
            {
                UI.Graphics.RelationWindow[28].ShowClipped(component.WinLeft, component.WinTop, surface);
            }
            else
            {
                r = new Rect(component.WinLeft + 1, component.WinTop + 1, component.WinLeft + component.WinWidth - 1, component.WinTop + component.WinHeight - 1);
                StrategicMap.Current.Players.GetPlayers(player2).GetFace()?.ShowStretched(r, surface);
            }

            Player player = StrategicMap.Current.Players.GetPlayers(player2);
            UI.Graphics.RelationWindow[player.PlayerRace + 16].ShowClipped(component.WinLeft, component.WinTop, surface);
        }
    }

    // AoWRelations.pas:659
    public void R1CBPreDraw(CheckBox sender, LockedSurface surface, int slot)
    {
        UIComponent control;
        sbyte race;
        Rect r;
        DiplomaticAction action;
        int modifier;
        control = sender;
        if (slot - 20 < Races.Count)
        {
            race = (sbyte)Races[slot - 20];
            r = new Rect(control.WinLeft + 1, control.WinTop + 1, control.WinLeft + control.WinWidth - 1, control.WinTop + control.WinHeight - 1);
            GamePack.Current.RaceResources.GetRaceResource(race).GetFace().ShowStretched(r, surface);
            action = _action;
            if (action == null && NewActionPanel.Visible && ActionsList.Selected != -1)
            {
                action = ActionsList.Items.Objects[ActionsList.Selected];
            }

            if (action != null)
            {
                modifier = action.GetRelationModifier(StrategicMap.Current.SeatedPlayer, race);
                if (modifier != 0)
                {
                    UI.Fonts.Age10.Color = 16777215;
                    UI.Fonts.Age10.SetTextCentered(r.Left + ((r.Right - r.Left) / 2), r.Top + 50, IntToStr(modifier), surface);
                }
            }
        }
    }

    // AoWRelations.pas:689
    public void P1IDraw(Image sender, LockedSurface surface, int slot)
    {
        int player;
        DiplomaticRelation relation;
        int image;
        Image component = sender;
        if (Players.Count > slot && slot != SelectedIndex)
        {
            player = Players[slot];
            if (SelectedPlayer != -1)
            {
                if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                {
                    if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                    {
                        if (StrategicMap.Current.Players.GetPlayers(SelectedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                        {
                            relation = StrategicMap.Current.Players.GetDiplomaticRelation(SelectedPlayer, (sbyte)player);
                            image = relation != DiplomaticRelation.Unknown ? (int)relation + 8 : 50;

                            UI.Graphics.RelationWindow[image].Show(component.WinLeft, component.WinTop + 8, surface);
                        }
                    }
                }
            }
            else
            {
                if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                {
                    Status playerRelation = StrategicMap.Current.Races.GetRace(SelectedRace).GetPlayerRelation((sbyte)player);
                    UI.Graphics.RelationWindow[(int)playerRelation + 11].ShowClipped(component.WinLeft, component.WinTop + 8, surface);
                }
            }
        }
    }

    // AoWRelations.pas:715
    public void R1IDraw(Image sender, LockedSurface surface, int slot)
    {
        int raceIndex;
        UIComponent s;
        s = sender;
        if (SelectedPlayer != -1 && slot - 20 < Races.Count)
        {
            if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
            {
                raceIndex = Races[slot - 20];
                Status playerRelation = StrategicMap.Current.Races.GetRace(raceIndex).GetPlayerRelation(SelectedPlayer);
                UI.Graphics.RelationWindow[(int)playerRelation + 11].ShowClipped(s.WinLeft, s.WinTop + 15, surface);
            }
        }
    }

    // AoWRelations.pas:723
    public void MAcceptBtnClick()
    {
        string reason = "";
        DiplomaticAction action;
        UI.General.PlayButtonSound(0);
        action = _action;
        if (action != null)
        {
            if (action.Accept(ref reason))
            {
                FinishEvent();
                ShowPlayerActions(_action.Source);
            }
            else
            {
                StrategicMap.Current.ShowMessage(reason);
            }
        }
    }

    // AoWRelations.pas:739
    public void MRejectBtnClick()
    {
        string reason = "";
        DiplomaticAction action;
        UI.General.PlayButtonSound(0);
        action = _action;
        if (action != null)
        {
            if (action.Reject(ref reason))
            {
                FinishEvent();
                ShowPlayerActions(_action.Source);
            }
            else
            {
                StrategicMap.Current.ShowMessage(reason);
            }
        }
    }

    // AoWRelations.pas:755
    public void PlyrGlowUpdate()
    {
        if (_glowIncreasing)
        {
            _glowIntensity += 1;
        }
        else
        {
            _glowIntensity -= 1;
        }

        if (_glowIntensity == 20)
        {
            _glowIncreasing = true;
        }
        else
        {
            if (_glowIntensity == 50)
            {
                _glowIncreasing = false;
            }
        }

        PlayerGlow.Drawn = false;
    }

    // AoWRelations.pas:763
    public void PlyrGlowDraw(LockedSurface surface) => UI.Graphics.RelationWindow[44].ShowBlended(PlayerGlow.WinLeft, PlayerGlow.WinTop, surface, GraphicsState.IntensityBlendTable.LookupTables[_glowIntensity]);

    // AoWRelations.pas:768
    public void FaceFrPreDraw(LockedSurface surface)
    {
        Rect r;
        r = new Rect(FaceFrame.WinLeft + 1, FaceFrame.WinTop, FaceFrame.WinLeft + FaceFrame.WinWidth, FaceFrame.WinTop + FaceFrame.WinHeight);
        if (_action == null)
        {
            if (SelectedPlayer != -1)
            {
                if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                {
                    StrategicMap.Current.Players.GetPlayers(SelectedPlayer).Leader.ShowFace(r, surface);
                }
            }
            else
            {
                GamePack.Current.RaceResources.GetRaceResource(SelectedRace).GetFace().Show(FaceFrame.WinLeft + 1, FaceFrame.WinTop + 3, surface);
            }
        }
        else
        {
            if (_event != null && _event.Action.Proposal.SignedAsOtherPlayer)
            {
                StrategicMap.Current.Players.GetPlayers(_event.Action.OtherPlayer).Leader.ShowFace(r, surface);
            }
            else
            {
                if (_action.Source == StrategicMap.Current.SeatedPlayer)
                {
                    StrategicMap.Current.Players.GetPlayers(_action.Target).Leader.ShowFace(r, surface);
                }
                else
                {
                    if (_action.Reaction == DiplomaticActionReaction.None)
                    {
                        StrategicMap.Current.Players.GetPlayers(_action.Source).Leader.ShowFace(r, surface);
                    }
                    else
                    {
                        StrategicMap.Current.Players.GetPlayers(_action.Target).Leader.ShowFace(r, surface);
                    }
                }
            }
        }
    }

    // AoWRelations.pas:794. The offer enters the proposal when the player sends it (MsgSendActionBtn).
    public void SBGoldSlidChange() => BribeGoldValueLabel.Text = IntToStr(BribeGoldSlider.Position);

    // AoWRelations.pas:800
    public void SBManaSlidChange() => BribeManaValueLabel.Text = IntToStr(BribeManaSlider.Position);

    // AoWRelations.pas:806
    public void RelWinHide() => FinishDialog();

    // AoWRelations.pas:811
    public void ASelectClick()
    {
        string reason = "";
        if (ActionsList.Selected != -1)
        {
            SetAction(ActionsList.Items.Objects[ActionsList.Selected]);
            ProposalView view = _action.Proposal.View;
            if (view == ProposalView.Notice || view == ProposalView.Threat)
            {
                if (!_action.Send(ref reason))
                {
                    StrategicMap.Current.ShowMessage(reason);
                }
            }
            else if (view == ProposalView.Offer)
            {
                NewActionPanel.Visible = false;
                SendBribeIcon.Visible = true;
                SendBribePanel.Visible = true;
                BribeGoldSlider.Position = 0;
                BribeManaSlider.Position = 0;
                Player playerGems = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
                BribeGoldSlider.Max = playerGems.Gems;
                Player playerMagicControl = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
                BribeManaSlider.Max = playerMagicControl.MagicControl.Mana;
                SBManaSlidChange();
                SBGoldSlidChange();
                string name = _action.GetName(StrategicMap.Current.SeatedPlayer);
                BribeHeaderLabel.Text = name;
                if (_action.Proposal.Icon >= 0)
                {
                    SendBribeIcon.CurrentImage = _action.Proposal.Icon;
                }
                else
                {
                    SendBribeIcon.Visible = false;
                }
            }
            else if (view == ProposalView.Message)
            {
                NewActionPanel.Visible = false;
                MessageEditPanel.Visible = true;
            }
        }
    }

    // AoWRelations.pas:851
    public void ActionsLBChange()
    {
        RelationPanel.Drawn = false;
        if (ActionsList.Selected != -1)
        {
            SelectActionButton.ImageLibraryIndexDown = 37;
            SelectActionButton.ImageLibraryIndexLit = 40;
            SelectActionButton.ImageLibraryIndexUp = 34;
        }
        else
        {
            SelectActionButton.ImageLibraryIndexDown = 46;
            SelectActionButton.ImageLibraryIndexLit = 49;
            SelectActionButton.ImageLibraryIndexUp = 43;
        }
    }

    // AoWRelations.pas:868
    public void CancelSelectedAction()
    {
        if (_event != null)
        {
            FinishEvent();
        }

        ShowPlayerActions(_action.Target);
        NewActionPanel.Visible = true;
        SendBribePanel.Visible = false;
        MessageEditPanel.Visible = false;
    }

    // AoWRelations.pas:877
    public void ChangeActionMessage(Memo sender)
    {
        if (_action != null)
        {
            string text = sender.Items.Text;
            _action.Proposal = _action.Proposal with { Message = DiplomaticProposal.Lines(text) };
        }
    }

    // AoWRelations.pas:882
    public void SendActionButtonClick()
    {
        string reason = "";
        sbyte player;
        DiplomaticAction action;
        action = _action;
        if (action != null)
        {
            action.Proposal = action.Proposal.WithOffer(BribeGoldSlider.Position, BribeManaSlider.Position);
            if (action.Send(ref reason))
            {
                player = _action.Target;
                if (_event != null && _event.Action.Proposal.SignedAsOtherPlayer)
                {
                    player = _event.Action.OtherPlayer;
                }

                FinishEvent();
                ShowPlayerActions(player);
            }
            else
            {
                StrategicMap.Current.ShowMessage(reason);
            }
        }
    }

    // AoWRelations.pas:901
    public void ReplyButtonClick()
    {
        DiplomaticAction action;
        action = StrategicMap.Current.Players.GetPlayers(_action.Target).CreateDiplomaticAction(new SendMessage(), _action.Source, -1);
        ClearActions();
        SetAction(action);
        NewActionPanel.Visible = false;
        MessageEditPanel.Visible = true;
    }

    // AoWRelations.pas:912
    public void RMCancelBtnClick()
    {
        if (_event != null)
        {
            FinishEvent();
        }

        ShowPlayerActions(_action.Source);
    }

    // AoWRelations.pas:918
    public void IIOkBtnClick()
    {
        FinishEvent();
        if (_action.Source == StrategicMap.Current.SeatedPlayer)
        {
            ShowPlayerActions(_action.Target);
        }
        else
        {
            ShowPlayerActions(_action.Source);
        }
    }

    // AoWRelations.pas:925
    public void RelWinKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            MainWindow.Visible = false;
        }
    }

    private static readonly string s_evilText = "Evil";

    private static readonly string s_goodText = "Good";

    private static readonly string s_noContactText = "No Contact";

    internal static void ShowDiplomaticEvent(RelationWindow dialog, DiplomaticActionEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.ClearActions();
        dialog.ResetChecks();
        // { Original reports its source filename, independent of this checkout. }
        if (dialog._event != null)
        {
            WindowActions.OriginalAssert("Assertion failure", "D:\\AoWDev\\AoWRelations.pas", 663);
        }

        dialog.Done = done;
        dialog._event = eventValue;
        _ = eventValue.AddRef();

        if (StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Status == PlayerStatus.Playing)
        {
            dialog.ShowRelations(eventValue.Action.Target);
            dialog.SetAction(eventValue.Action);
            DiplomaticProposal proposal = dialog._action.Proposal;
            ProposalView view = proposal.View;
            if (view == ProposalView.Message)
            {
                sbyte from = proposal.SignedAsOtherPlayer ? dialog._action.OtherPlayer : dialog._action.Source;
                dialog.HighlightPlayer(from);
                dialog.ShowPlayerInformation(from);
                dialog.NewActionPanel.Visible = false;
                dialog.ReceivedMessagePanel.Visible = true;
                string text = proposal.MessageText;
                dialog.ReceivedMessageMemo.Items.Text = text;
                string name = StrategicMap.Current.Players.GetPlayers(from).GetName();
                string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(GameText.MessageFormat), [name]);
                dialog.ReceivedMessageHeaderLabel.Text = format;
            }
            else if (view == ProposalView.Offer)
            {
                dialog.NewActionPanel.Visible = false;
                if (dialog._action.Reaction == DiplomaticActionReaction.None)
                {
                    dialog.HighlightPlayer(dialog._action.Source);
                    dialog.ShowPlayerInformation(dialog._action.Source);
                    dialog.IncomingPanel.Visible = true;
                    (_, int gold, int mana) = proposal.ToFields();
                    dialog.ReceivedGoldLabel.Text = IntToStr(gold);
                    dialog.ReceivedManaLabel.Text = IntToStr(mana);
                    string text2 = proposal.MessageText;
                    dialog.IncomingMemo.Items.Text = text2;
                    if (proposal.Icon >= 0)
                    {
                        dialog.IncomingIcon.CurrentImage = proposal.Icon;
                    }
                }
                else
                {
                    if (StrategicMap.Current.SeatedPlayer == dialog._action.Source && dialog._action.Reaction != (DiplomaticActionReaction)3)
                    {
                        dialog.HighlightPlayer(dialog._action.Target);
                        dialog.ShowPlayerInformation(dialog._action.Target);
                        dialog.NewActionPanel.Visible = false;
                        dialog.IncomingInfoPanel.Visible = true;
                        string text2 = proposal.MessageText;
                        dialog.IncomingInfoMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            dialog.IncomingInfoIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        dialog.FinishEvent();
                        if (dialog._action.Reaction == (DiplomaticActionReaction)3)
                        {
                            dialog.ShowPlayerActions(dialog._action.Target);
                        }
                        else
                        {
                            dialog.ShowPlayerActions(dialog._action.Source);
                        }
                    }
                }
            }
            else if (view == ProposalView.Notice)
            {
                if (StrategicMap.Current.SeatedPlayer == dialog._action.Source)
                {
                    dialog.FinishEvent();
                    dialog.ShowPlayerActions(dialog._action.Target);
                }
                else
                {
                    if (StrategicMap.Current.SeatedPlayer == dialog._action.Target)
                    {
                        dialog.HighlightPlayer(dialog._action.Source);
                        dialog.ShowPlayerInformation(dialog._action.Source);
                        dialog.NewActionPanel.Visible = false;
                        dialog.IncomingInfoPanel.Visible = true;
                        string text2 = proposal.MessageText;
                        dialog.IncomingInfoMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            dialog.IncomingInfoIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        dialog.FinishEvent();
                        dialog.ShowPlayerActions(dialog._action.Source);
                    }
                }
            }
            else if (view == ProposalView.Threat)
            {
                dialog.HighlightPlayer(dialog._action.Source);
                dialog.ShowPlayerInformation(dialog._action.Source);
                dialog.NewActionPanel.Visible = false;
                dialog.IncomingInfoPanel.Visible = true;
                string text3 = proposal.MessageText;
                dialog.IncomingInfoMemo.Items.Text = text3;
                if (proposal.Icon >= 0)
                {
                    dialog.IncomingInfoIcon.CurrentImage = proposal.Icon;
                }
            }
        }
        else
        {
            dialog.FinishDialog();
        }
    }

}
