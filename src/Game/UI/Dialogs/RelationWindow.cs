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

    public Window RelWin;
    public Image RShield;
    public Image LShield;
    public Panel RelPnl;
    public Panel RIncomingPnl;
    public Image P1I;
    public Image P2I;
    public Image P3I;
    public Image P4I;
    public Image P5I;
    public Image P6I;
    public Image P7I;
    public Image P8I;
    public Image P9I;
    public Image P10I;
    public Image P11I;
    public Image P12I;
    public Image R1I;
    public Image R2I;
    public Image R3I;
    public Image R4I;
    public Image R5I;
    public Image R6I;
    public Image R7I;
    public Image R8I;
    public Image R9I;
    public Image R10I;
    public Image R11I;
    public Image R12I;
    public Image FaceShadow;
    public Memo MMemo;
    public Image MIcon;
    public Panel RMsgEditPnl;
    public Panel RSBribePnl;
    public Memo SMemo;
    public HorizontalScrollBar SBGoldSlid;
    public Panel ActionPnl;
    public Panel LInfoPlayerPnl;
    public Panel RNewActionPnl;
    public Label APlayer;
    public Panel LInfoRacePnl;
    public Label ARace2;
    public Image AAlignment;
    public Label AAlignmentLbl;
    public CheckBox P1CB;
    public CheckBox P2CB;
    public CheckBox P3CB;
    public CheckBox P4CB;
    public CheckBox P5CB;
    public CheckBox P6CB;
    public CheckBox P7CB;
    public CheckBox P8CB;
    public CheckBox P9CB;
    public CheckBox P10CB;
    public CheckBox P11CB;
    public CheckBox P12CB;
    public Image FaceFr;
    public CheckBox R1CB;
    public CheckBox R2CB;
    public CheckBox R3CB;
    public CheckBox R4CB;
    public CheckBox R5CB;
    public CheckBox R6CB;
    public CheckBox R7CB;
    public CheckBox R8CB;
    public CheckBox R9CB;
    public CheckBox R10CB;
    public CheckBox R11CB;
    public CheckBox R12CB;
    public Image PlyrGlow;
    public Label NoContact;
    public Button ASelect;
    public ListBox ActionsLB;
    public Image APlyrIcon;
    public Label APlyrType;
    public Image PlStatIcn;
    public Label PlStatLbl;
    public Label PlStatV;
    public Image RlRelIcn;
    public Label RlRelV;
    public Label MBGoldBribeV;
    public Label MBManaBribeV;
    public Label SBGoldV;
    public Label SBManaV;
    public HorizontalScrollBar SBManaSlid;
    public Image SBIcon;
    public Label BribeHeaderLbl;
    public Panel RMsgRecPnl;
    public Memo RMMemo;
    public Label RMHeaderLbl;
    public Panel IncommingInfoPnl;
    public Memo IIMemo;
    public Image IIIcon;
    public Label OfTheLbl;
    public Label PRaceLbl;

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
        PlyrGlow.Alignment.LeftOffset = indexOf * 51 - 37 + 51;
        PlyrGlow.Aligned = false;
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
        RelWin.Visible = true;
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
        RelPnl.Aligned = false;
        RelWin.Drawn = false;
        PlyrGlow.Alignment.TopOffset = 10;
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
        selected = ActionsLB.Selected;
        AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).ListDiplomaticActions(player, ref _actions);
        for (int I = 0, last = _actions.GetCount() - 1; I <= last; I++)
        {
            var name = _actions.GetDiplomaticActions(I).GetName(AoWHSMap.Current.SeatedPlayer);
            var diplomaticAction = _actions.GetDiplomaticActions(I);
            ActionsLB.Items.AddObject(name, diplomaticAction);
        }

        ActionsLB.Selected = selected;
        ActionsLBChange();
        ActionPnl.Visible = true;
    }

    // AoWRelations.pas:309
    public void ShowPlayerInformation(sbyte player)
    {
        DiplomaticRelation relation;
        InfoPlayer = player;
        InfoRace = -1;
        FaceShadow.Visible = true;
        NoContact.Visible = false;
        PlStatV.Visible = true;
        PlStatIcn.Visible = true;
        PlStatLbl.Visible = true;
        LInfoPlayerPnl.Visible = true;
        LInfoRacePnl.Visible = false;
        var playerPlayerRace = AoWHSMap.Current.Players.GetPlayers(player);
        LShield.CurrentImage = playerPlayerRace.PlayerRace + 26;
        LShield.Visible = true;
        RShield.CurrentImage = LShield.CurrentImage;
        RShield.Visible = true;
        var firstDaySeen = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen(player);
        NoContact.Visible = firstDaySeen == -1;
        APlayer.Visible = !NoContact.Visible;
        OfTheLbl.Visible = APlayer.Visible;
        PRaceLbl.Visible = APlayer.Visible;
        if (AoWHSMap.Current.Players.GetPlayers(player).GetFirstDaySeen(AoWHSMap.Current.SeatedPlayer) != -1)
        {
            if (player != AoWHSMap.Current.SeatedPlayer)
            {
                relation = AoWHSMap.Current.Players.GetDiplomaticRelation(player, AoWHSMap.Current.SeatedPlayer);
                var diplomaticRelation = AbilityRules.TranslateResourceString(GameTables.s_diplomaticRelationTexts[(int)relation]);
                PlStatV.Text = diplomaticRelation;
                if (relation != DiplomaticRelation.Unknown)
                {
                    PlStatIcn.CurrentImage = (int)relation + 8;
                }
                else
                {
                    PlStatIcn.CurrentImage = 50;
                }
            }
            else
            {
                PlStatIcn.Visible = false;
                PlStatLbl.Visible = false;
                PlStatV.Visible = false;
            }
        }
        else
        {
            PlStatIcn.Visible = false;
            var noContact = AbilityRules.TranslateResourceString(s_noContactText);
            PlStatV.Text = noContact;
        }

        var name = AoWHSMap.Current.Players.GetPlayers(player).GetName();
        APlayer.Text = name;
        OfTheLbl.Text = DialogHosts.PlayerInfoWindow.LeadL.Text;
        var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[playerLocal.PlayerRace]);
        PRaceLbl.Text = raceNamesPlural;
        var playerPlayerType = AoWHSMap.Current.Players.GetPlayers(player);
        if (Set<PlayerType>.Of(PlayerType.Human, PlayerType.Cpu1, PlayerType.Cpu2, PlayerType.Cpu3, PlayerType.Cpu4, PlayerType.Cpu5).Contains(playerPlayerType.PlayerType))
        {
            switch (AoWHSMap.Current.Players.GetPlayers(player).PlayerType)
            {
                case PlayerType.Cpu1:
                    APlyrIcon.CurrentImage = 1;
                    break;
                case PlayerType.Cpu2:
                    APlyrIcon.CurrentImage = 2;
                    break;
                case PlayerType.Cpu3:
                    APlyrIcon.CurrentImage = 3;
                    break;
                case PlayerType.Cpu4:
                    APlyrIcon.CurrentImage = 4;
                    break;
                case PlayerType.Cpu5:
                    APlyrIcon.CurrentImage = 5;
                    break;
                default:
                    APlyrIcon.CurrentImage = 0;
                    break;
            }

            APlyrIcon.Visible = true;
        }
        else
        {
            APlyrIcon.Visible = false;
        }

        var player2 = AoWHSMap.Current.Players.GetPlayers(player);
        var playerType = AbilityRules.TranslateResourceString(GameTables.s_playerTypeTexts[(int)player2.PlayerType]);
        APlyrType.Text = playerType;
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
                PlyrGlow.Alignment.TopOffset = check.Alignment.TopOffset;
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
                                    image.ToolTipText.Add(GameTables.s_diplomaticRelationTexts[(int)diplomaticRelation]);
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
        LShield.Visible = false;
        RShield.Visible = false;
        FaceShadow.Visible = true;
        ActionPnl.Visible = true;
        LInfoPlayerPnl.Visible = false;
        LInfoRacePnl.Visible = true;
        var playerRelation = AoWHSMap.Current.Races.GetRace((sbyte)race).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
        RlRelIcn.CurrentImage = (int)playerRelation + 11;
        var playerRelation2 = AoWHSMap.Current.Races.GetRace((sbyte)race).GetPlayerRelation(AoWHSMap.Current.SeatedPlayer);
        var status = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation2]);
        RlRelV.Text = status;
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[(sbyte)race]);
        ARace2.Text = raceNamesPlural;
        switch (race)
        {
            case >= 0 and <= 3:
                var neutral = AbilityRules.TranslateResourceString(GameTables.NeutralText);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [neutral]);
                AAlignmentLbl.Text = format;
                AAlignment.CurrentImage = 33;
                break;
            case >= 4 and <= 7:
                var good = AbilityRules.TranslateResourceString(s_goodText);
                var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [good]);
                AAlignmentLbl.Text = formattedText;
                AAlignment.CurrentImage = 31;
                break;
            case >= 8 and <= 11:
                var evil = AbilityRules.TranslateResourceString(s_evilText);
                var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [evil]);
                AAlignmentLbl.Text = format2;
                AAlignment.CurrentImage = 35;
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
        RNewActionPnl.Visible = true;
        RIncomingPnl.Visible = false;
        RMsgEditPnl.Visible = false;
        RSBribePnl.Visible = false;
        RMsgRecPnl.Visible = false;
        IncommingInfoPnl.Visible = false;
        _actions.Clear();
        ActionsLB.Items.Clear();
        SMemo.Items.Clear();
        MMemo.Items.Clear();
        ActionsLBChange();
    }

    // AoWRelations.pas:540
    public void RelWindowCreate()
    {
        _actions = new DiplomaticActionList(null);
        Races = new Int32List();
        Players = new Int32List();
        FImages =
        [
            P1I, P2I, P3I, P4I, P5I, P6I, P7I, P8I, P9I, P10I, P11I, P12I,
            R1I, R2I, R3I, R4I, R5I, R6I, R7I, R8I, R9I, R10I, R11I, R12I,
        ];
        FChecks =
        [
            P1CB, P2CB, P3CB, P4CB, P5CB, P6CB, P7CB, P8CB, P9CB, P10CB, P11CB, P12CB,
            R1CB, R2CB, R3CB, R4CB, R5CB, R6CB, R7CB, R8CB, R9CB, R10CB, R11CB, R12CB,
        ];
    }

    // AoWRelations.pas:596
    public void RelWindowDestroy()
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
        RelWin.Visible = false;
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

        RelPnl.Aligned = false;
        RelWin.Drawn = false;
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

        RelPnl.Aligned = false;
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
                DialogHosts.UIGraphics.RelWin[28].ShowClipped(component.WinLeft, component.WinTop, surface);
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
            DialogHosts.UIGraphics.RelWin[player.PlayerRace + 16].ShowClipped(component.WinLeft, component.WinTop, surface);
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
            if (action == null && RNewActionPnl.Visible && ActionsLB.Selected != -1)
            {
                action = (DiplomaticAction)ActionsLB.Items.Objects[ActionsLB.Selected];
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

                            DialogHosts.UIGraphics.RelWin[image].Show(component.WinLeft, component.WinTop + 8, surface);
                        }
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                {
                    var playerRelation = AoWHSMap.Current.Races.GetRace(SelectedRace).GetPlayerRelation((sbyte)player);
                    DialogHosts.UIGraphics.RelWin[(int)playerRelation + 11].ShowClipped(component.WinLeft, component.WinTop + 8, surface);
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
                DialogHosts.UIGraphics.RelWin[(int)playerRelation + 11].ShowClipped(S.WinLeft, S.WinTop + 15, surface);
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

        PlyrGlow.Drawn = false;
    }

    // AoWRelations.pas:763
    public void PlyrGlowDraw(LockedSurface surface)
    {
        DialogHosts.UIGraphics.RelWin[44].ShowBlended(PlyrGlow.WinLeft, PlyrGlow.WinTop, surface, GraphicsState.IntensityBlendTable.LookupTables[_glowIntensity]);
    }

    // AoWRelations.pas:768
    public void FaceFrPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(FaceFr.WinLeft + 1, FaceFr.WinTop, FaceFr.WinLeft + FaceFr.WinWidth, FaceFr.WinTop + FaceFr.WinHeight);
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
                AoWHSSet.Current.RaceResources.GetRaceResource(SelectedRace).GetFace().Show(FaceFr.WinLeft + 1, FaceFr.WinTop + 3, surface);
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
        SBGoldV.Text = IntToStr(SBGoldSlid.Position);
    }

    // AoWRelations.pas:800
    public void SBManaSlidChange()
    {
        SBManaV.Text = IntToStr(SBManaSlid.Position);
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
        if (ActionsLB.Selected != -1)
        {
            SetAction((DiplomaticAction)ActionsLB.Items.Objects[ActionsLB.Selected]);
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
                RNewActionPnl.Visible = false;
                SBIcon.Visible = true;
                RSBribePnl.Visible = true;
                SBGoldSlid.Position = 0;
                SBManaSlid.Position = 0;
                var playerGems = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
                SBGoldSlid.Max = playerGems.Gems;
                var playerMagicControl = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
                SBManaSlid.Max = playerMagicControl.MagicControl.Mana;
                SBManaSlidChange();
                SBGoldSlidChange();
                var name = _action.GetName(AoWHSMap.Current.SeatedPlayer);
                BribeHeaderLbl.Text = name;
                if (_action.Proposal.Icon >= 0)
                {
                    SBIcon.CurrentImage = _action.Proposal.Icon;
                }
                else
                {
                    SBIcon.Visible = false;
                }
            }
            else if (view == ProposalView.Message)
            {
                RNewActionPnl.Visible = false;
                RMsgEditPnl.Visible = true;
            }
        }
    }

    // AoWRelations.pas:851
    public void ActionsLBChange()
    {
        RelPnl.Drawn = false;
        if (ActionsLB.Selected != -1)
        {
            ASelect.ImageLibraryIndexDown = 37;
            ASelect.ImageLibraryIndexLit = 40;
            ASelect.ImageLibraryIndexUp = 34;
        }
        else
        {
            ASelect.ImageLibraryIndexDown = 46;
            ASelect.ImageLibraryIndexLit = 49;
            ASelect.ImageLibraryIndexUp = 43;
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
        RNewActionPnl.Visible = true;
        RSBribePnl.Visible = false;
        RMsgEditPnl.Visible = false;
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
    public void MsgSendActionBtn()
    {
        string reason = "";
        sbyte player;
        DiplomaticAction action;
        action = _action;
        if (action != null)
        {
            action.Proposal = action.Proposal.WithOffer(SBGoldSlid.Position, SBManaSlid.Position);
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
    public void ReplyBtnClick()
    {
        DiplomaticAction action;
        action = AoWHSMap.Current.Players.GetPlayers(_action.Target).CreateDiplomaticAction(new SendMessage(), _action.Source, -1);
        ClearActions();
        SetAction(action);
        RNewActionPnl.Visible = false;
        RMsgEditPnl.Visible = true;
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
            RelWin.Visible = false;
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
                dialog.RNewActionPnl.Visible = false;
                dialog.RMsgRecPnl.Visible = true;
                var text = proposal.MessageText;
                dialog.RMMemo.Items.Text = text;
                var name = AoWHSMap.Current.Players.GetPlayers(from).GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.MessageFormat), [name]);
                dialog.RMHeaderLbl.Text = format;
            }
            else if (view == ProposalView.Offer)
            {
                dialog.RNewActionPnl.Visible = false;
                if (dialog._action.Reaction == DiplomaticActionReaction.None)
                {
                    dialog.HighlightPlayer(dialog._action.Source);
                    dialog.ShowPlayerInformation(dialog._action.Source);
                    dialog.RIncomingPnl.Visible = true;
                    var (_, gold, mana) = proposal.ToFields();
                    dialog.MBGoldBribeV.Text = IntToStr(gold);
                    dialog.MBManaBribeV.Text = IntToStr(mana);
                    var text2 = proposal.MessageText;
                    dialog.MMemo.Items.Text = text2;
                    if (proposal.Icon >= 0)
                    {
                        dialog.MIcon.CurrentImage = proposal.Icon;
                    }
                }
                else
                {
                    if (AoWHSMap.Current.SeatedPlayer == dialog._action.Source && dialog._action.Reaction != (DiplomaticActionReaction)3)
                    {
                        dialog.HighlightPlayer(dialog._action.Target);
                        dialog.ShowPlayerInformation(dialog._action.Target);
                        dialog.RNewActionPnl.Visible = false;
                        dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            dialog.IIIcon.CurrentImage = proposal.Icon;
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
                        dialog.RNewActionPnl.Visible = false;
                        dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            dialog.IIIcon.CurrentImage = proposal.Icon;
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
                dialog.RNewActionPnl.Visible = false;
                dialog.IncommingInfoPnl.Visible = true;
                var text3 = proposal.MessageText;
                dialog.IIMemo.Items.Text = text3;
                if (proposal.Icon >= 0)
                {
                    dialog.IIIcon.CurrentImage = proposal.Icon;
                }
            }
        }
        else
        {
            dialog.FinishDialog();
        }
    }

}
