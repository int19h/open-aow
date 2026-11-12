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
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// diplomatic actions, player indices, race indices, controls
public partial class RelationWindow : DataModule
{
    public RelationWindow(Component owner) : base(owner)
    { }

    public Window MainWindow;
    public Image RightShield;
    public Image LeftShield;
    public Panel RelationPanel;
    public Panel IncomingPanel;
    public Image Player1Image;
    public Image Player2Image;
    public Image Player3Image;
    public Image Player4Image;
    public Image Player5Image;
    public Image Player6Image;
    public Image Player7Image;
    public Image Player8Image;
    public Image Player9Image;
    public Image Player10Image;
    public Image Player11Image;
    public Image Player12Image;
    public Image Race1Image;
    public Image Race2Image;
    public Image Race3Image;
    public Image Race4Image;
    public Image Race5Image;
    public Image Race6Image;
    public Image Race7Image;
    public Image Race8Image;
    public Image Race9Image;
    public Image Race10Image;
    public Image Race11Image;
    public Image Race12Image;
    public Image FaceShadow;
    public Memo IncomingMemo;
    public Image IncomingIcon;
    public Panel MessageEditPanel;
    public Panel SendBribePanel;
    public Memo SendMessageMemo;
    public HorizontalScrollBar BribeGoldSlider;
    public Panel ActionPanel;
    public Panel PlayerInfoPanel;
    public Panel NewActionPanel;
    public Label PlayerNameLabel;
    public Panel RaceInfoPanel;
    public Label RaceNameLabel;
    public Image AlignmentImage;
    public Label AlignmentLabel;
    public CheckBox Player1CheckBox;
    public CheckBox Player2CheckBox;
    public CheckBox Player3CheckBox;
    public CheckBox Player4CheckBox;
    public CheckBox Player5CheckBox;
    public CheckBox Player6CheckBox;
    public CheckBox Player7CheckBox;
    public CheckBox Player8CheckBox;
    public CheckBox Player9CheckBox;
    public CheckBox Player10CheckBox;
    public CheckBox Player11CheckBox;
    public CheckBox Player12CheckBox;
    public Image FaceFrame;
    public CheckBox Race1CheckBox;
    public CheckBox Race2CheckBox;
    public CheckBox Race3CheckBox;
    public CheckBox Race4CheckBox;
    public CheckBox Race5CheckBox;
    public CheckBox Race6CheckBox;
    public CheckBox Race7CheckBox;
    public CheckBox Race8CheckBox;
    public CheckBox Race9CheckBox;
    public CheckBox Race10CheckBox;
    public CheckBox Race11CheckBox;
    public CheckBox Race12CheckBox;
    public Image PlayerGlow;
    public Label NoContactLabel;
    public Button SelectActionButton;
    public ListBox<DiplomaticAction> ActionsList;
    public Image PlayerTypeIcon;
    public Label PlayerTypeLabel;
    public Image PlayerStatusIcon;
    public Label PlayerStatusLabel;
    public Label PlayerStatusValueLabel;
    public Image RaceRelationIcon;
    public Label RaceRelationValueLabel;
    public Label ReceivedGoldLabel;
    public Label ReceivedManaLabel;
    public Label BribeGoldValueLabel;
    public Label BribeManaValueLabel;
    public HorizontalScrollBar BribeManaSlider;
    public Image SendBribeIcon;
    public Label BribeHeaderLabel;
    public Panel ReceivedMessagePanel;
    public Memo ReceivedMessageMemo;
    public Label ReceivedMessageHeaderLabel;
    public Panel IncomingInfoPanel;
    public Memo IncomingInfoMemo;
    public Image IncomingInfoIcon;
    public Label OfTheLabel;
    public Label PlayerRaceLabel;

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

    private ExecuteDoneEvent _done;
    private DiplomaticActionEventLog _event;
    private int _glowIntensity;
    private bool _glowIncreasing;

    // Slots 0..23 are images and slots 24..47 are check boxes, in RelWindowCreate order.
    // An index outside 0..47 throws.
    private UIComponent ControlAt(int Index)
    {
        if (Index >= 0 && Index < 24)
        {
            return FImages[Index];
        }

        return FChecks[Index - 24];
    }

    // AoWRelations.pas:221
    public void FillPlayers()
    {
        Players.Clear();
        for (int I = 1, last = AoWHSMap.Current.Players.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)I) != -1 && AoWHSMap.Current.Players.GetPlayers(I).Status == PlayerStatus.Playing)
            {
                Players.Add(I);
            }
        }

        for (int I = 1; I <= 12; I++)
        {
            var check = FChecks[I - 1];
            if (I <= Players.Count)
            {
                check.Visible = true;
                check.ToolTipText.Clear();
                var player = Players[I - 1];
                var name = AoWHSMap.Current.Players.GetPlayers(player).GetName();
                check.ToolTipText.Add(name);
            }
            else
            {
                check.Visible = false;
            }
        }

        var indexOf = Players.IndexOf(AoWHSMap.Current.SeatedPlayer);
        PlayerGlow.Alignment.LeftOffset = indexOf * 51 - 37 + 51;
        PlayerGlow.Aligned = false;
    }

    // AoWRelations.pas:241
    public void ShowRelations(int player)
    {
        int index;
        ClearActions();
        ResetChecks();
        DialogHosts.General.Manager.OverlayPriority = 49;
        Races.Clear();
        for (int I = 0, last = AoWHSMap.Current.Races.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.Races.GetRace(I).Enabled)
            {
                Races.Add(I);
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            var check = FChecks[I + 12];
            if (I < Races.Count)
            {
                check.Visible = true;
                check.ToolTipText.Clear();
                var race = Races[I];
                var fRacesRace = AoWHSMap.Current.Races.GetRace(race);
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[fRacesRace.RaceIndex]);
                check.ToolTipText.Add(raceNamesPlural);
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

        index = Players.IndexOf(AoWHSMap.Current.SeatedPlayer);
        var seated = ControlAt(index + 24);
        if (seated is CheckBox seatedCheck)
        {
            seatedCheck.Checked = true;
        }

        P1CBChange(seated, index);
        AoWHSMap.Current.TutorialControl.ShowMessage(65, true, true);
    }

    // AoWRelations.pas:270
    public void ResetChecks()
    {
        SelectedIndex = -1;
        RelationPanel.Aligned = false;
        MainWindow.Drawn = false;
        PlayerGlow.Alignment.TopOffset = 10;
        for (int I = 0; I <= 11; I++)
        {
            var check = FChecks[I];
            check.Checked = false;
            check.Alignment.TopOffset = 10;
        }

        for (int I = 12; I <= 23; I++)
        {
            var check = FChecks[I];
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
        AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).ListDiplomaticActions(player, ref _actions);
        for (int I = 0, last = _actions.GetCount() - 1; I <= last; I++)
        {
            var name = _actions.GetDiplomaticActions(I).GetName(AoWHSMap.Current.SeatedPlayer);
            var diplomaticAction = _actions.GetDiplomaticActions(I);
            ActionsList.Items.AddObject(name, diplomaticAction);
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
        var playerPlayerRace = AoWHSMap.Current.Players.GetPlayers(player);
        LeftShield.CurrentImage = playerPlayerRace.PlayerRace + 26;
        LeftShield.Visible = true;
        RightShield.CurrentImage = LeftShield.CurrentImage;
        RightShield.Visible = true;
        var firstDaySeen = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(player);
        NoContactLabel.Visible = firstDaySeen == -1;
        PlayerNameLabel.Visible = !NoContactLabel.Visible;
        OfTheLabel.Visible = PlayerNameLabel.Visible;
        PlayerRaceLabel.Visible = PlayerNameLabel.Visible;
        if (AoWHSMap.Current.Players.GetPlayers(player).GetFirstDaySeen(AoWHSMap.Current.SeatedPlayer) != -1)
        {
            if (player != AoWHSMap.Current.SeatedPlayer)
            {
                relation = AoWHSMap.Current.Players.GetDiplomaticRelation(player, AoWHSMap.Current.SeatedPlayer);
                var diplomaticRelation = AbilityRules.TranslateResourceString(GameTables.DiplomaticRelationTexts[(int)relation]);
                PlayerStatusValueLabel.Text = diplomaticRelation;
                if (relation != DiplomaticRelation.Unknown)
                {
                    PlayerStatusIcon.CurrentImage = (int)relation + 8;
                }
                else
                {
                    PlayerStatusIcon.CurrentImage = 50;
                }
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
            var noContact = AbilityRules.TranslateResourceString(s_noContactText);
            PlayerStatusValueLabel.Text = noContact;
        }

        var name = AoWHSMap.Current.Players.GetPlayers(player).GetName();
        PlayerNameLabel.Text = name;
        OfTheLabel.Text = DialogHosts.PlayerInfoWindow.OfTheLabel.Text;
        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[playerLocal.PlayerRace]);
        PlayerRaceLabel.Text = raceNamesPlural;
        var playerPlayerType = AoWHSMap.Current.Players.GetPlayers(player);
        if (Set<PlayerType>.Of(PlayerType.Human, PlayerType.Cpu1, PlayerType.Cpu2, PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(playerPlayerType.PlayerType))
        {
            switch (AoWHSMap.Current.Players.GetPlayers(player).PlayerType)
            {
                case PlayerType.Cpu1:
                    PlayerTypeIcon.CurrentImage = 1;
                    break;
                case PlayerType.Cpu2:
                    PlayerTypeIcon.CurrentImage = 2;
                    break;
                case PlayerType.Cpu3:
                    PlayerTypeIcon.CurrentImage = 3;
                    break;
                case PlayerType.Cpu4:
                    PlayerTypeIcon.CurrentImage = 4;
                    break;
                case PlayerType.Cpu5:
                    PlayerTypeIcon.CurrentImage = 5;
                    break;
                default:
                    PlayerTypeIcon.CurrentImage = 0;
                    break;
            }

            PlayerTypeIcon.Visible = true;
        }
        else
        {
            PlayerTypeIcon.Visible = false;
        }

        var player2 = AoWHSMap.Current.Players.GetPlayers(player);
        var playerType = AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[(int)player2.PlayerType]);
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
            var check = FChecks[SelectedIndex];
            check.Alignment.TopOffset = 20;
            check.Checked = true;
            if (AoWHSMap.Current.SeatedPlayer == player)
            {
                PlayerGlow.Alignment.TopOffset = check.Alignment.TopOffset;
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            var image = FImages[I];
            if (I < Players.Count)
            {
                image.Visible = true;
                if (Players[I] == player)
                {
                    image.CurrentImage = 2;
                }
                else
                {
                    if (I > Races.Count)
                    {
                        image.CurrentImage = 0;
                    }
                    else
                    {
                        image.CurrentImage = 1;
                    }
                }
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
                        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                        {
                            var playerLocal = Players[I];
                            if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)playerLocal) != -1)
                            {
                                var player2 = Players[I];
                                if (AoWHSMap.Current.Players.GetPlayers(SelectedPlayer).GetFirstDaySeen((sbyte)player2) != -1)
                                {
                                    var player3 = Players[I];
                                    var diplomaticRelation = AoWHSMap.Current.Players.GetDiplomaticRelation(SelectedPlayer, (sbyte)player3);
                                    image.ToolTipText.Add(GameTables.DiplomaticRelationTexts[(int)diplomaticRelation]);
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

        for (int I = 0; I <= 11; I++)
        {
            var image = FImages[I + 12];
            if (I < Races.Count)
            {
                image.CurrentImage = 4;
            }
            else
            {
                image.CurrentImage = 5;
            }

            switch (image.CurrentImage)
            {
                case 4:
                    image.ToolTipText.Clear();
                    if (SelectedPlayer != -1)
                    {
                        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                        {
                            var race = Races[I];
                            var playerRelation = AoWHSMap.Current.Races.GetRace(race).GetPlayerRelation(SelectedPlayer);
                            var status = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
                            var race2 = Races[I];
                            var playerRelationValue = AoWHSMap.Current.Races.GetRace(race2).GetPlayerRelationValue(SelectedPlayer);
                            image.ToolTipText.Add(status + " (" + IntToStr(playerRelationValue) + ")");
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
        var playerRelation = AoWHSMap.Current.Races.GetRace((sbyte)race).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
        RaceRelationIcon.CurrentImage = (int)playerRelation + 11;
        var playerRelation2 = AoWHSMap.Current.Races.GetRace((sbyte)race).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
        var status = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation2]);
        RaceRelationValueLabel.Text = status;
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[(sbyte)race]);
        RaceNameLabel.Text = raceNamesPlural;
        switch (race)
        {
            case >= 0 and <= 3:
                var neutral = AbilityRules.TranslateResourceString(GameTables.NeutralText);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [neutral]);
                AlignmentLabel.Text = format;
                AlignmentImage.CurrentImage = 33;
                break;
            case >= 4 and <= 7:
                var good = AbilityRules.TranslateResourceString(s_goodText);
                var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [good]);
                AlignmentLabel.Text = formattedText;
                AlignmentImage.CurrentImage = 31;
                break;
            case >= 8 and <= 11:
                var evil = AbilityRules.TranslateResourceString(s_evilText);
                var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [evil]);
                AlignmentLabel.Text = format2;
                AlignmentImage.CurrentImage = 35;
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
        var check = FChecks[index + 12];
        check.Alignment.TopOffset = 175;
        check.Checked = true;
        SelectedIndex = 20 + index;
        for (int I = 0; I <= 11; I++)
        {
            var image = FImages[I];
            if (I < Players.Count)
            {
                image.CurrentImage = 1;
            }
            else
            {
                image.CurrentImage = 2;
            }

            switch (image.CurrentImage)
            {
                case 1:
                    image.ToolTipText.Clear();
                    var player = Players[I];
                    if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                    {
                        var player2 = Players[I];
                        var playerRelation = AoWHSMap.Current.Races.GetRace(SelectedRace).GetPlayerRelation((sbyte)player2);
                        var status = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
                        var player3 = Players[I];
                        var playerRelationValue = AoWHSMap.Current.Races.GetRace(SelectedRace).GetPlayerRelationValue((sbyte)player3);
                        image.ToolTipText.Add(status + " (" + IntToStr(playerRelationValue) + ")");
                    }

                    break;
                default:
                    image.ToolTipText.Clear();
                    break;
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            var image = FImages[I + 12];
            image.CurrentImage = 5;
            image.ToolTipText.Clear();
        }
    }

    // AoWRelations.pas:490
    public void SetAction(DiplomaticAction action)
    {
        if (action != _action)
        {
            if (_action != null)
            {
                _action.Release();
            }

            _action = action;
            if (_action != null)
            {
                _action.AddRef();
            }
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
            done = _done;
            eventValue = pending;
            _done = null;
            _event = null;
            if (done != null)
            {
                done(eventValue);
            }

            if (eventValue != null)
            {
                eventValue.Release();
            }
        }
    }

    // AoWRelations.pas:516
    public void FinishDialog()
    {
        SetAction(null);
        DialogHosts.General.Manager.OverlayPriority = 0;
        FinishEvent();
        AoWHSMap.Current.TutorialControl.ShowMessage(71, true, true);
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
        _actions = new DiplomaticActionList(null);
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
        DialogHosts.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWRelations.pas:610
    public void P1CBChange(UIComponent sender, int slot)
    {
        // The original SetChecked call on an image is undefined, so skipped. An image has FChecked and no SetChecked. The action branch runs only for a check box.
        if (sender._checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }

        RelationPanel.Aligned = false;
        MainWindow.Drawn = false;
        if (sender._checked && sender is CheckBox)
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
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
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
        sbyte Player;
        Rect R;
        UIComponent component;
        component = sender;
        if (slot < Players.Count)
        {
            Player = (sbyte)Players[slot];
            if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(Player) == -1)
            {
                DialogHosts.UIGraphics.RelationWindow[28].ShowClipped(component.WinLeft, component.WinTop, surface);
            }
            else
            {
                R = new Rect(component.WinLeft + 1, component.WinTop + 1, component.WinLeft + component.WinWidth - 1, component.WinTop + component.WinHeight - 1);
                if (AoWHSMap.Current.Players.GetPlayers(Player).GetFace() != null)
                {
                    AoWHSMap.Current.Players.GetPlayers(Player).GetFace().ShowStretched(R, surface);
                }
            }

            var player = AoWHSMap.Current.Players.GetPlayers(Player);
            DialogHosts.UIGraphics.RelationWindow[player.PlayerRace + 16].ShowClipped(component.WinLeft, component.WinTop, surface);
        }
    }

    // AoWRelations.pas:659
    public void R1CBPreDraw(CheckBox sender, LockedSurface surface, int slot)
    {
        UIComponent control;
        sbyte race;
        Rect R;
        DiplomaticAction action;
        int modifier;
        control = sender;
        if (slot - 20 < Races.Count)
        {
            race = (sbyte)Races[slot - 20];
            R = new Rect(control.WinLeft + 1, control.WinTop + 1, control.WinLeft + control.WinWidth - 1, control.WinTop + control.WinHeight - 1);
            AoWHSSet.Current.RaceResources.GetRaceResource(race).GetFace().ShowStretched(R, surface);
            action = _action;
            if (action == null && NewActionPanel.Visible && ActionsList.Selected != -1)
            {
                action = ActionsList.Items.Objects[ActionsList.Selected];
            }

            if (action != null)
            {
                modifier = action.GetRelationModifier(AoWHSMap.Current.SeatedPlayer, race);
                if (modifier != 0)
                {
                    DialogHosts.UIFonts.Age10.Color = 16777215;
                    DialogHosts.UIFonts.Age10.SetTextCentered(R.Left + (R.Right - R.Left) / 2, R.Top + 50, IntToStr(modifier), surface);
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
        var component = sender;
        if (Players.Count > slot && slot != SelectedIndex)
        {
            player = Players[slot];
            if (SelectedPlayer != -1)
            {
                if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                {
                    if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                    {
                        if (AoWHSMap.Current.Players.GetPlayers(SelectedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                        {
                            relation = AoWHSMap.Current.Players.GetDiplomaticRelation(SelectedPlayer, (sbyte)player);
                            if (relation != DiplomaticRelation.Unknown)
                            {
                                image = (int)relation + 8;
                            }
                            else
                            {
                                image = 50;
                            }

                            DialogHosts.UIGraphics.RelationWindow[image].Show(component.WinLeft, component.WinTop + 8, surface);
                        }
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                {
                    var playerRelation = AoWHSMap.Current.Races.GetRace(SelectedRace).GetPlayerRelation((sbyte)player);
                    DialogHosts.UIGraphics.RelationWindow[(int)playerRelation + 11].ShowClipped(component.WinLeft, component.WinTop + 8, surface);
                }
            }
        }
    }

    // AoWRelations.pas:715
    public void R1IDraw(Image sender, LockedSurface surface, int slot)
    {
        int raceIndex;
        UIComponent S;
        S = sender;
        if (SelectedPlayer != -1 && slot - 20 < Races.Count)
        {
            if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
            {
                raceIndex = Races[slot - 20];
                var playerRelation = AoWHSMap.Current.Races.GetRace(raceIndex).GetPlayerRelation(SelectedPlayer);
                DialogHosts.UIGraphics.RelationWindow[(int)playerRelation + 11].ShowClipped(S.WinLeft, S.WinTop + 15, surface);
            }
        }
    }

    // AoWRelations.pas:723
    public void MAcceptBtnClick()
    {
        string reason = "";
        DiplomaticAction action;
        DialogHosts.General.PlayButtonSound(0);
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
                AoWHSMap.Current.ShowMessage(reason);
            }
        }
    }

    // AoWRelations.pas:739
    public void MRejectBtnClick()
    {
        string reason = "";
        DiplomaticAction action;
        DialogHosts.General.PlayButtonSound(0);
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
                AoWHSMap.Current.ShowMessage(reason);
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
    public void PlyrGlowDraw(LockedSurface surface)
    {
        DialogHosts.UIGraphics.RelationWindow[44].ShowBlended(PlayerGlow.WinLeft, PlayerGlow.WinTop, surface, GraphicsState.IntensityBlendTable.LookupTables[_glowIntensity]);
    }

    // AoWRelations.pas:768
    public void FaceFrPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(FaceFrame.WinLeft + 1, FaceFrame.WinTop, FaceFrame.WinLeft + FaceFrame.WinWidth, FaceFrame.WinTop + FaceFrame.WinHeight);
        if (_action == null)
        {
            if (SelectedPlayer != -1)
            {
                if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(SelectedPlayer) != -1)
                {
                    AoWHSMap.Current.Players.GetPlayers(SelectedPlayer).Leader.ShowFace(R, surface);
                }
            }
            else
            {
                AoWHSSet.Current.RaceResources.GetRaceResource(SelectedRace).GetFace().Show(FaceFrame.WinLeft + 1, FaceFrame.WinTop + 3, surface);
            }
        }
        else
        {
            if (_event != null && _event.Action.Proposal.SignedAsOtherPlayer)
            {
                AoWHSMap.Current.Players.GetPlayers(_event.Action.OtherPlayer).Leader.ShowFace(R, surface);
            }
            else
            {
                if (_action.Source == AoWHSMap.Current.SeatedPlayer)
                {
                    AoWHSMap.Current.Players.GetPlayers(_action.Target).Leader.ShowFace(R, surface);
                }
                else
                {
                    if (_action.Reaction == DiplomaticActionReaction.None)
                    {
                        AoWHSMap.Current.Players.GetPlayers(_action.Source).Leader.ShowFace(R, surface);
                    }
                    else
                    {
                        AoWHSMap.Current.Players.GetPlayers(_action.Target).Leader.ShowFace(R, surface);
                    }
                }
            }
        }
    }

    // AoWRelations.pas:794. The offer enters the proposal when the player sends it (MsgSendActionBtn).
    public void SBGoldSlidChange()
    {
        BribeGoldValueLabel.Text = IntToStr(BribeGoldSlider.Position);
    }

    // AoWRelations.pas:800
    public void SBManaSlidChange()
    {
        BribeManaValueLabel.Text = IntToStr(BribeManaSlider.Position);
    }

    // AoWRelations.pas:806
    public void RelWinHide()
    {
        FinishDialog();
    }

    // AoWRelations.pas:811
    public void ASelectClick()
    {
        string reason = "";
        if (ActionsList.Selected != -1)
        {
            SetAction(ActionsList.Items.Objects[ActionsList.Selected]);
            var view = _action.Proposal.View;
            if (view == ProposalView.Notice || view == ProposalView.Threat)
            {
                if (!_action.Send(ref reason))
                {
                    AoWHSMap.Current.ShowMessage(reason);
                }
            }
            else if (view == ProposalView.Offer)
            {
                NewActionPanel.Visible = false;
                SendBribeIcon.Visible = true;
                SendBribePanel.Visible = true;
                BribeGoldSlider.Position = 0;
                BribeManaSlider.Position = 0;
                var playerGems = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
                BribeGoldSlider.Max = playerGems.Gems;
                var playerMagicControl = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
                BribeManaSlider.Max = playerMagicControl.MagicControl.Mana;
                SBManaSlidChange();
                SBGoldSlidChange();
                var name = _action.GetName(AoWHSMap.Current.SeatedPlayer);
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
            var text = sender.Items.Text;
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
                AoWHSMap.Current.ShowMessage(reason);
            }
        }
    }

    // AoWRelations.pas:901
    public void ReplyButtonClick()
    {
        DiplomaticAction action;
        action = AoWHSMap.Current.Players.GetPlayers(_action.Target).CreateDiplomaticAction(new SendMessage(), _action.Source, -1);
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
        if (_action.Source == AoWHSMap.Current.SeatedPlayer)
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

        dialog._done = done;
        dialog._event = eventValue;
        eventValue.AddRef();

        if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Status == PlayerStatus.Playing)
        {
            dialog.ShowRelations(eventValue.Action.Target);
            dialog.SetAction(eventValue.Action);
            var proposal = dialog._action.Proposal;
            var view = proposal.View;
            if (view == ProposalView.Message)
            {
                var from = proposal.SignedAsOtherPlayer ? dialog._action.OtherPlayer : dialog._action.Source;
                dialog.HighlightPlayer(from);
                dialog.ShowPlayerInformation(from);
                dialog.NewActionPanel.Visible = false;
                dialog.ReceivedMessagePanel.Visible = true;
                var text = proposal.MessageText;
                dialog.ReceivedMessageMemo.Items.Text = text;
                var name = AoWHSMap.Current.Players.GetPlayers(from).GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.MessageFormat), [name]);
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
                    var (_, gold, mana) = proposal.ToFields();
                    dialog.ReceivedGoldLabel.Text = IntToStr(gold);
                    dialog.ReceivedManaLabel.Text = IntToStr(mana);
                    var text2 = proposal.MessageText;
                    dialog.IncomingMemo.Items.Text = text2;
                    if (proposal.Icon >= 0)
                    {
                        dialog.IncomingIcon.CurrentImage = proposal.Icon;
                    }
                }
                else
                {
                    if (AoWHSMap.Current.SeatedPlayer == dialog._action.Source && dialog._action.Reaction != (DiplomaticActionReaction)3)
                    {
                        dialog.HighlightPlayer(dialog._action.Target);
                        dialog.ShowPlayerInformation(dialog._action.Target);
                        dialog.NewActionPanel.Visible = false;
                        dialog.IncomingInfoPanel.Visible = true;
                        var text2 = proposal.MessageText;
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
                if (AoWHSMap.Current.SeatedPlayer == dialog._action.Source)
                {
                    dialog.FinishEvent();
                    dialog.ShowPlayerActions(dialog._action.Target);
                }
                else
                {
                    if (AoWHSMap.Current.SeatedPlayer == dialog._action.Target)
                    {
                        dialog.HighlightPlayer(dialog._action.Source);
                        dialog.ShowPlayerInformation(dialog._action.Source);
                        dialog.NewActionPanel.Visible = false;
                        dialog.IncomingInfoPanel.Visible = true;
                        var text2 = proposal.MessageText;
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
                var text3 = proposal.MessageText;
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
