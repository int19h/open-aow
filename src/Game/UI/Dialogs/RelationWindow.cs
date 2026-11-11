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
    public Button MExitBtn;
    public Panel RelPnl;
    public Panel RIncomingPnl;
    public Button MAcceptBtn;
    public Button MRejectBtn;
    public VerticalScrollBar MScroll;
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
    public Label MBGoldLbl;
    public Image MIcon;
    public Panel RMsgEditPnl;
    public Panel RSBribePnl;
    public VerticalScrollBar SScroll;
    public Memo SMemo;
    public HorizontalScrollBar SBGoldSlid;
    public Panel ActionPnl;
    public Panel LInfoPlayerPnl;
    public Panel RNewActionPnl;
    public Button AExit;
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
    public VerticalScrollBar ActionSB;
    public Panel LBActionsPnl;
    public Label ActionLbl;
    public Image PIRelIcn;
    public Label PIRelLbl;
    public Panel PIRelPnl;
    public Label PIRelV;
    public Image APlyrIcon;
    public Label APlyrType;
    public Image PlStatIcn;
    public Label PlStatLbl;
    public Panel PIStatPnl;
    public Label PlStatV;
    public Panel PTypePnl;
    public Panel PIFrame;
    public Panel RIFrame;
    public Panel RIRelPnl;
    public Panel RAlignPnl;
    public Image RlRelIcn;
    public Label RIRelLbl;
    public Label RlRelV;
    public Label MBGoldBribeV;
    public Image MBGoldIcn;
    public Panel MBGoldPnl;
    public Panel MBManaPnl;
    public Label MBManaLbl;
    public Label MBManaBribeV;
    public Image MBManaIcn;
    public Label MsgLbl;
    public Button MsgSendBtn;
    public Button MsgCancelBtn;
    public Frame RSBribe2;
    public Label SBGoldV;
    public Image SBGoldIcn;
    public Panel SBGold;
    public Panel SBMana;
    public Label SBManaV;
    public Image SBManaIcn;
    public Frame RSBSep1;
    public Label SBribeBtn;
    public HorizontalScrollBar SBManaSlid;
    public Image SBIcon;
    public Button RSBSendBtn;
    public Button RBSCanlBtn;
    public Image StatusGlow;
    public Label BribeHeaderLbl;
    public Panel RMsgRecPnl;
    public Memo RMMemo;
    public Label RMHeaderLbl;
    public Button RMReplyBtn;
    public Button RMCancelBtn;
    public Button RMExitBtn;
    public Panel IncommingInfoPnl;
    public Button IIOkBtn;
    public Button IIExitBtn;
    public VerticalScrollBar IRScroll;
    public Memo IIMemo;
    public Image IIIcon;
    public IvTranslator Translator;
    public Label OfTheLbl;
    public Label PRaceLbl;
    public int FSelectedIndex;
    public DiplomaticAction FAction;
    public DiplomaticActionList FActions;
    public sbyte FInfoPlayer;
    public sbyte FInfoRace;
    public sbyte FSelectedPlayer;
    public sbyte FSelectedRace;
    public Int32List FPlayers;
    public Int32List FRaces;
    public List<Image> FImages;
    public List<CheckBox> FChecks;
    public ExecuteDoneEvent FDone;
    public DiplomaticActionEventLog FEvent;
    public int FGlowIntensity;
    public bool FGlowIncreasing;

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
        FPlayers.Clear();
        for (int I = 1, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen((sbyte)I) != -1 && AoWHSMap.Current.FPlayers.GetPlayers(I).FStatus == Player.Status.Playing)
            {
                FPlayers.Add(I);
            }
        }

        for (int I = 1; I <= 12; I++)
        {
            var check = FChecks[I - 1];
            if (I <= FPlayers.Count)
            {
                check.Visible = true;
                check.ToolTipText.Clear();
                var player = FPlayers[I - 1];
                var name = AoWHSMap.Current.FPlayers.GetPlayers(player).GetName();
                check.ToolTipText.Add(name);
            }
            else
            {
                check.Visible = false;
            }
        }

        var indexOf = FPlayers.IndexOf(AoWHSMap.Current.FSeatedPlayer);
        PlyrGlow.Alignment.LeftOffset = indexOf * 51 - 37 + 51;
        PlyrGlow.FAligned = false;
    }

    // AoWRelations.pas:241
    public void ShowRelations(int Player)
    {
        int Index;
        ClearActions();
        ResetChecks();
        DialogHosts.General.Manager.OverlayPriority = 49;
        FRaces.Clear();
        for (int I = 0, last = AoWHSMap.Current.FRaces.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSMap.Current.FRaces.GetRace(I).FEnabled)
            {
                FRaces.Add(I);
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            var check = FChecks[I + 12];
            if (I < FRaces.Count)
            {
                check.Visible = true;
                check.ToolTipText.Clear();
                var race = FRaces[I];
                var fRacesRace = AoWHSMap.Current.FRaces.GetRace(race);
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[fRacesRace.FRace]);
                check.ToolTipText.Add(raceNamesPlural);
            }
            else
            {
                check.Visible = false;
            }
        }

        FillPlayers();
        FGlowIntensity = 50;
        FGlowIncreasing = false;
        RelWin.Visible = true;
        if (ControlAt(Player + 23) is CheckBox playerCheck)
        {
            playerCheck.Checked = true;
        }

        Index = FPlayers.IndexOf(AoWHSMap.Current.FSeatedPlayer);
        var seated = ControlAt(Index + 24);
        if (seated is CheckBox seatedCheck)
        {
            seatedCheck.Checked = true;
        }

        P1CBChange(seated, Index);
        AoWHSMap.Current.FTutorialControl.ShowMessage(65, true, true);
    }

    // AoWRelations.pas:270
    public void ResetChecks()
    {
        FSelectedIndex = -1;
        RelPnl.FAligned = false;
        RelWin.FDrawn = false;
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
    public virtual void ShowPlayerActions(sbyte Player)
    {
        int Selected;
        ClearActions();
        HighlightPlayer(Player);
        ShowPlayerInformation(Player);
        Selected = ActionsLB.Selected;
        AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).ListDiplomaticActions(Player, ref FActions);
        for (int I = 0, last = FActions.GetCount() - 1; I <= last; I++)
        {
            var name = FActions.GetDiplomaticActions(I).GetName(AoWHSMap.Current.FSeatedPlayer);
            var diplomaticAction = FActions.GetDiplomaticActions(I);
            ActionsLB.Items.AddObject(name, diplomaticAction);
        }

        ActionsLB.Selected = Selected;
        ActionsLBChange();
        ActionPnl.Visible = true;
    }

    // AoWRelations.pas:309
    public void ShowPlayerInformation(sbyte Player)
    {
        DiplomaticRelation Relation;
        FInfoPlayer = Player;
        FInfoRace = -1;
        FaceShadow.Visible = true;
        NoContact.Visible = false;
        PlStatV.Visible = true;
        PlStatIcn.Visible = true;
        PlStatLbl.Visible = true;
        LInfoPlayerPnl.Visible = true;
        LInfoRacePnl.Visible = false;
        var playerPlayerRace = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        LShield.CurrentImage = playerPlayerRace.FPlayerRace + 26;
        LShield.Visible = true;
        RShield.CurrentImage = LShield.CurrentImage;
        RShield.Visible = true;
        var firstDaySeen = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen(Player);
        NoContact.Visible = firstDaySeen == -1;
        APlayer.Visible = !NoContact.Visible;
        OfTheLbl.Visible = APlayer.Visible;
        PRaceLbl.Visible = APlayer.Visible;
        if (AoWHSMap.Current.FPlayers.GetPlayers(Player).GetFirstDaySeen(AoWHSMap.Current.FSeatedPlayer) != -1)
        {
            if (Player != AoWHSMap.Current.FSeatedPlayer)
            {
                Relation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Player, AoWHSMap.Current.FSeatedPlayer);
                var diplomaticRelation = AbilityRules.TranslateRStr(GameTables.TDiplomaticRelationRStr[(int)Relation]);
                PlStatV.Text = diplomaticRelation;
                if (Relation != DiplomaticRelation.Unknown)
                {
                    PlStatIcn.CurrentImage = (int)Relation + 8;
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
            var noContact = AbilityRules.TranslateRStr(NoContactRStr);
            PlStatV.Text = noContact;
        }

        var name = AoWHSMap.Current.FPlayers.GetPlayers(Player).GetName();
        APlayer.Text = name;
        OfTheLbl.Text = DialogHosts.PInfoW.LeadL.Text;
        var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[player.FPlayerRace]);
        PRaceLbl.Text = raceNamesPlural;
        var playerPlayerType = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        if (Set<PlayerType>.Of(PlayerType.Human, PlayerType.CPU1, PlayerType.CPU2, PlayerType.CPU3, PlayerType.CPU4, PlayerType.CPU5).Contains(playerPlayerType.FPlayerType))
        {
            switch (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType)
            {
                case PlayerType.CPU1:
                    APlyrIcon.CurrentImage = 1;
                    break;
                case PlayerType.CPU2:
                    APlyrIcon.CurrentImage = 2;
                    break;
                case PlayerType.CPU3:
                    APlyrIcon.CurrentImage = 3;
                    break;
                case PlayerType.CPU4:
                    APlyrIcon.CurrentImage = 4;
                    break;
                case PlayerType.CPU5:
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

        var player2 = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        var playerType = AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[(int)player2.FPlayerType]);
        APlyrType.Text = playerType;
    }

    // AoWRelations.pas:370
    public void HighlightPlayer(sbyte Player)
    {
        ResetChecks();
        FSelectedPlayer = Player;
        FSelectedRace = -1;
        FSelectedIndex = FPlayers.IndexOf(Player);
        if (FSelectedIndex != -1)
        {
            var check = FChecks[FSelectedIndex];
            check.Alignment.TopOffset = 20;
            check.Checked = true;
            if (AoWHSMap.Current.FSeatedPlayer == Player)
            {
                PlyrGlow.Alignment.TopOffset = check.Alignment.TopOffset;
            }
        }

        for (int I = 0; I <= 11; I++)
        {
            var image = FImages[I];
            if (I < FPlayers.Count)
            {
                image.Visible = true;
                if (FPlayers[I] == Player)
                {
                    image.CurrentImage = 2;
                }
                else
                {
                    if (I > FRaces.Count)
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
                    if (FSelectedPlayer != -1)
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                        {
                            var player = FPlayers[I];
                            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                            {
                                var player2 = FPlayers[I];
                                if (AoWHSMap.Current.FPlayers.GetPlayers(FSelectedPlayer).GetFirstDaySeen((sbyte)player2) != -1)
                                {
                                    var player3 = FPlayers[I];
                                    var diplomaticRelation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FSelectedPlayer, (sbyte)player3);
                                    image.ToolTipText.Add(GameTables.TDiplomaticRelationRStr[(int)diplomaticRelation]);
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
            if (I < FRaces.Count)
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
                    if (FSelectedPlayer != -1)
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                        {
                            var race = FRaces[I];
                            var playerRelation = AoWHSMap.Current.FRaces.GetRace(race).GetPlayerRelation(FSelectedPlayer);
                            var status = AbilityRules.TranslateRStr(GameTables.TStatusRStr[(int)playerRelation]);
                            var race2 = FRaces[I];
                            var playerRelationValue = AoWHSMap.Current.FRaces.GetRace(race2).GetPlayerRelationValue(FSelectedPlayer);
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
    public void ShowRaceInformation(byte Race)
    {
        FInfoPlayer = -1;
        FInfoRace = (sbyte)Race;
        LShield.Visible = false;
        RShield.Visible = false;
        FaceShadow.Visible = true;
        ActionPnl.Visible = true;
        LInfoPlayerPnl.Visible = false;
        LInfoRacePnl.Visible = true;
        var playerRelation = AoWHSMap.Current.FRaces.GetRace((sbyte)Race).GetPlayerRelation(AoWHSMap.Current.FSeatedPlayer);
        RlRelIcn.CurrentImage = (int)playerRelation + 11;
        var playerRelation2 = AoWHSMap.Current.FRaces.GetRace((sbyte)Race).GetPlayerRelation(AoWHSMap.Current.FSeatedPlayer);
        var status = AbilityRules.TranslateRStr(GameTables.TStatusRStr[(int)playerRelation2]);
        RlRelV.Text = status;
        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[(sbyte)Race]);
        ARace2.Text = raceNamesPlural;
        switch (Race)
        {
            case >= 0 and <= 3:
                var neutral = AbilityRules.TranslateRStr(GameTables.NeutralRStr);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.AlignmentXRStr), [neutral]);
                AAlignmentLbl.Text = format;
                AAlignment.CurrentImage = 33;
                break;
            case >= 4 and <= 7:
                var good = AbilityRules.TranslateRStr(GoodRStr);
                var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.AlignmentXRStr), [good]);
                AAlignmentLbl.Text = aoWEFormat;
                AAlignment.CurrentImage = 31;
                break;
            case >= 8 and <= 11:
                var evil = AbilityRules.TranslateRStr(EvilRStr);
                var format2 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.AlignmentXRStr), [evil]);
                AAlignmentLbl.Text = format2;
                AAlignment.CurrentImage = 35;
                break;
        }
    }

    // AoWRelations.pas:456
    public void HighlightRace(sbyte Race)
    {
        int Index;
        ResetChecks();
        FSelectedPlayer = -1;
        FSelectedRace = Race;
        Index = FRaces.IndexOf(Race);
        // A missing race yields -1, and check 11 is P12CB, the old slot 35.
        var check = FChecks[Index + 12];
        check.Alignment.TopOffset = 175;
        check.Checked = true;
        FSelectedIndex = 20 + Index;
        for (int I = 0; I <= 11; I++)
        {
            var image = FImages[I];
            if (I < FPlayers.Count)
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
                    var player = FPlayers[I];
                    if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen((sbyte)player) != -1)
                    {
                        var player2 = FPlayers[I];
                        var playerRelation = AoWHSMap.Current.FRaces.GetRace(FSelectedRace).GetPlayerRelation((sbyte)player2);
                        var status = AbilityRules.TranslateRStr(GameTables.TStatusRStr[(int)playerRelation]);
                        var player3 = FPlayers[I];
                        var playerRelationValue = AoWHSMap.Current.FRaces.GetRace(FSelectedRace).GetPlayerRelationValue((sbyte)player3);
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
    public void SetAction(DiplomaticAction Action)
    {
        if (Action != FAction)
        {
            if (FAction != null)
            {
                FAction.Release();
            }

            FAction = Action;
            if (FAction != null)
            {
                FAction.AddRef();
            }
        }
    }

    // AoWRelations.pas:500
    public void FinishEvent()
    {
        ExecuteDoneEvent Done;
        DiplomaticActionEventLog Pending;
        DiplomaticActionEventLog Event;
        Pending = FEvent;
        if (Pending != null)
        {
            Done = FDone;
            Event = Pending;
            FDone = null;
            FEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }
    }

    // AoWRelations.pas:516
    public void FinishDialog()
    {
        SetAction(null);
        DialogHosts.General.Manager.OverlayPriority = 0;
        FinishEvent();
        AoWHSMap.Current.FTutorialControl.ShowMessage(71, true, true);
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
        FActions.Clear();
        ActionsLB.Items.Clear();
        SMemo.Items.Clear();
        MMemo.Items.Clear();
        ActionsLBChange();
    }

    // AoWRelations.pas:540
    public void RelWindowCreate()
    {
        FActions = new DiplomaticActionList(null);
        FRaces = new Int32List();
        FPlayers = new Int32List();
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
        FActions?.Dispose();
        FImages?.Clear();
        FChecks?.Clear();
        FPlayers?.Dispose();
        FRaces?.Dispose();
    }

    // AoWRelations.pas:604
    public void ExitClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        RelWin.Visible = false;
    }

    // AoWRelations.pas:610
    public void P1CBChange(UIComponent Sender, int Slot)
    {
        // The original SetChecked call on an image is undefined, so skipped. An image has FChecked and no SetChecked. The action branch runs only for a check box.
        if (Sender.FChecked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }

        RelPnl.FAligned = false;
        RelWin.FDrawn = false;
        if (Sender.FChecked && Sender is CheckBox)
        {
            if (FAction == null)
            {
                ShowPlayerActions((sbyte)FPlayers[Slot]);
            }
            else
            {
                HighlightPlayer((sbyte)FPlayers[Slot]);
            }
        }
    }

    // AoWRelations.pas:623
    public void R1CBChange(CheckBox Sender, int Slot)
    {
        CheckBox Check;
        Check = Sender;
        if (Check.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }

        RelPnl.FAligned = false;
        if (FAction == null)
        {
            ClearActions();
            HighlightRace((sbyte)FRaces[Slot - 20]);
            ShowRaceInformation((byte)FRaces[Slot - 20]);
        }
        else
        {
            HighlightRace((sbyte)FRaces[Slot - 20]);
        }
    }

    // AoWRelations.pas:639
    public void P1CBPreDraw(CheckBox Sender, LockedSurface Surface, int Slot)
    {
        sbyte Player;
        Rect R;
        UIComponent Component;
        Component = Sender;
        if (Slot < FPlayers.Count)
        {
            Player = (sbyte)FPlayers[Slot];
            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen(Player) == -1)
            {
                DialogHosts.IntGfxMod.RelWin[28].ShowClipped(Component.WinLeft, Component.WinTop, Surface);
            }
            else
            {
                R = new Rect(Component.WinLeft + 1, Component.WinTop + 1, Component.WinLeft + Component.WinWidth - 1, Component.WinTop + Component.WinHeight - 1);
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).GetFace() != null)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(Player).GetFace().ShowStretched(R, Surface);
                }
            }

            var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
            DialogHosts.IntGfxMod.RelWin[player.FPlayerRace + 16].ShowClipped(Component.WinLeft, Component.WinTop, Surface);
        }
    }

    // AoWRelations.pas:659
    public void R1CBPreDraw(CheckBox Sender, LockedSurface Surface, int Slot)
    {
        UIComponent Control;
        sbyte Race;
        Rect R;
        DiplomaticAction Action;
        int Modifier;
        Control = Sender;
        if (Slot - 20 < FRaces.Count)
        {
            Race = (sbyte)FRaces[Slot - 20];
            R = new Rect(Control.WinLeft + 1, Control.WinTop + 1, Control.WinLeft + Control.WinWidth - 1, Control.WinTop + Control.WinHeight - 1);
            AoWHSSet.Current.FRaceResources.GetRaceResource(Race).GetFace().ShowStretched(R, Surface);
            Action = FAction;
            if (Action == null && RNewActionPnl.Visible && ActionsLB.Selected != -1)
            {
                Action = (DiplomaticAction)ActionsLB.Items.Objects[ActionsLB.Selected];
            }

            if (Action != null)
            {
                Modifier = Action.GetRelationModifier(AoWHSMap.Current.FSeatedPlayer, Race);
                if (Modifier != 0)
                {
                    DialogHosts.FontModule.Age10.Color = 16777215;
                    DialogHosts.FontModule.Age10.SetTextCentered(R.Left + (R.Right - R.Left) / 2, R.Top + 50, IntToStr(Modifier), Surface);
                }
            }
        }
    }

    // AoWRelations.pas:689
    public void P1IDraw(Image Sender, LockedSurface Surface, int Slot)
    {
        int Player;
        DiplomaticRelation Relation;
        int Image;
        var component = Sender;
        if (FPlayers.Count > Slot && Slot != FSelectedIndex)
        {
            Player = FPlayers[Slot];
            if (FSelectedPlayer != -1)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                {
                    if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen((sbyte)Player) != -1)
                    {
                        if (AoWHSMap.Current.FPlayers.GetPlayers(FSelectedPlayer).GetFirstDaySeen((sbyte)Player) != -1)
                        {
                            Relation = AoWHSMap.Current.FPlayers.GetDiplomaticRelation(FSelectedPlayer, (sbyte)Player);
                            if (Relation != DiplomaticRelation.Unknown)
                            {
                                Image = (int)Relation + 8;
                            }
                            else
                            {
                                Image = 50;
                            }

                            DialogHosts.IntGfxMod.RelWin[Image].Show(component.WinLeft, component.WinTop + 8, Surface);
                        }
                    }
                }
            }
            else
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen((sbyte)Player) != -1)
                {
                    var playerRelation = AoWHSMap.Current.FRaces.GetRace(FSelectedRace).GetPlayerRelation((sbyte)Player);
                    DialogHosts.IntGfxMod.RelWin[(int)playerRelation + 11].ShowClipped(component.WinLeft, component.WinTop + 8, Surface);
                }
            }
        }
    }

    // AoWRelations.pas:715
    public void R1IDraw(Image Sender, LockedSurface Surface, int Slot)
    {
        int RaceIndex;
        UIComponent S;
        S = Sender;
        if (FSelectedPlayer != -1 && Slot - 20 < FRaces.Count)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
            {
                RaceIndex = FRaces[Slot - 20];
                var playerRelation = AoWHSMap.Current.FRaces.GetRace(RaceIndex).GetPlayerRelation(FSelectedPlayer);
                DialogHosts.IntGfxMod.RelWin[(int)playerRelation + 11].ShowClipped(S.WinLeft, S.WinTop + 15, Surface);
            }
        }
    }

    // AoWRelations.pas:723
    public void MAcceptBtnClick()
    {
        string Reason = "";
        DiplomaticAction Action;
        DialogHosts.General.PlayButtonSound(0);
        Action = FAction;
        if (Action != null)
        {
            if (Action.Accept(ref Reason))
            {
                FinishEvent();
                ShowPlayerActions(FAction.FSource);
            }
            else
            {
                AoWHSMap.Current.ShowMessage(Reason);
            }
        }
    }

    // AoWRelations.pas:739
    public void MRejectBtnClick()
    {
        string Reason = "";
        DiplomaticAction Action;
        DialogHosts.General.PlayButtonSound(0);
        Action = FAction;
        if (Action != null)
        {
            if (Action.Reject(ref Reason))
            {
                FinishEvent();
                ShowPlayerActions(FAction.FSource);
            }
            else
            {
                AoWHSMap.Current.ShowMessage(Reason);
            }
        }
    }

    // AoWRelations.pas:755
    public void PlyrGlowUpdate()
    {
        if (FGlowIncreasing)
        {
            FGlowIntensity += 1;
        }
        else
        {
            FGlowIntensity -= 1;
        }

        if (FGlowIntensity == 20)
        {
            FGlowIncreasing = true;
        }
        else
        {
            if (FGlowIntensity == 50)
            {
                FGlowIncreasing = false;
            }
        }

        PlyrGlow.FDrawn = false;
    }

    // AoWRelations.pas:763
    public void PlyrGlowDraw(LockedSurface Surface)
    {
        DialogHosts.IntGfxMod.RelWin[44].ShowBlended(PlyrGlow.WinLeft, PlyrGlow.WinTop, Surface, GraphicsState.IntensityBlendTable.LookupTables[FGlowIntensity]);
    }

    // AoWRelations.pas:768
    public void FaceFrPreDraw(LockedSurface Surface)
    {
        Rect R;
        R = new Rect(FaceFr.WinLeft + 1, FaceFr.WinTop, FaceFr.WinLeft + FaceFr.WinWidth, FaceFr.WinTop + FaceFr.WinHeight);
        if (FAction == null)
        {
            if (FSelectedPlayer != -1)
            {
                if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).GetFirstDaySeen(FSelectedPlayer) != -1)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FSelectedPlayer).FLeader.ShowFace(R, Surface);
                }
            }
            else
            {
                AoWHSSet.Current.FRaceResources.GetRaceResource(FSelectedRace).GetFace().Show(FaceFr.WinLeft + 1, FaceFr.WinTop + 3, Surface);
            }
        }
        else
        {
            if (FEvent != null && FEvent.FAction.Proposal.SignedAsOtherPlayer)
            {
                AoWHSMap.Current.FPlayers.GetPlayers(FEvent.FAction.FOtherPlayer).FLeader.ShowFace(R, Surface);
            }
            else
            {
                if (FAction.FSource == AoWHSMap.Current.FSeatedPlayer)
                {
                    AoWHSMap.Current.FPlayers.GetPlayers(FAction.FTarget).FLeader.ShowFace(R, Surface);
                }
                else
                {
                    if (FAction.FReaction == DiplomaticActionReaction.None)
                    {
                        AoWHSMap.Current.FPlayers.GetPlayers(FAction.FSource).FLeader.ShowFace(R, Surface);
                    }
                    else
                    {
                        AoWHSMap.Current.FPlayers.GetPlayers(FAction.FTarget).FLeader.ShowFace(R, Surface);
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
        string Reason = "";
        if (ActionsLB.Selected != -1)
        {
            SetAction((DiplomaticAction)ActionsLB.Items.Objects[ActionsLB.Selected]);
            var view = FAction.Proposal.View;
            if (view == ProposalView.Notice || view == ProposalView.Threat)
            {
                if (!FAction.Send(ref Reason))
                {
                    AoWHSMap.Current.ShowMessage(Reason);
                }
            }
            else if (view == ProposalView.Offer)
            {
                RNewActionPnl.Visible = false;
                SBIcon.Visible = true;
                RSBribePnl.Visible = true;
                SBGoldSlid.Position = 0;
                SBManaSlid.Position = 0;
                var playerGems = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
                SBGoldSlid.Max = playerGems.FGems;
                var playerMagicControl = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
                SBManaSlid.Max = playerMagicControl.FMagicControl.FMana;
                SBManaSlidChange();
                SBGoldSlidChange();
                var name = FAction.GetName(AoWHSMap.Current.FSeatedPlayer);
                BribeHeaderLbl.Text = name;
                if (FAction.Proposal.Icon >= 0)
                {
                    SBIcon.CurrentImage = FAction.Proposal.Icon;
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
        RelPnl.FDrawn = false;
        if (ActionsLB.Selected != -1)
        {
            ASelect.ILIndexDown = 37;
            ASelect.ILIndexLit = 40;
            ASelect.ILIndexUp = 34;
        }
        else
        {
            ASelect.ILIndexDown = 46;
            ASelect.ILIndexLit = 49;
            ASelect.ILIndexUp = 43;
        }
    }

    // AoWRelations.pas:868
    public void CancelSelectedAction()
    {
        if (FEvent != null)
        {
            FinishEvent();
        }

        ShowPlayerActions(FAction.FTarget);
        RNewActionPnl.Visible = true;
        RSBribePnl.Visible = false;
        RMsgEditPnl.Visible = false;
    }

    // AoWRelations.pas:877
    public void ChangeActionMessage(Memo Sender)
    {
        if (FAction != null)
        {
            var text = Sender.Items.Text;
            FAction.Proposal = FAction.Proposal with { Message = DiplomaticProposal.Lines(text) };
        }
    }

    // AoWRelations.pas:882
    public void MsgSendActionBtn()
    {
        string Reason = "";
        sbyte Player;
        DiplomaticAction Action;
        Action = FAction;
        if (Action != null)
        {
            Action.Proposal = Action.Proposal.WithOffer(SBGoldSlid.Position, SBManaSlid.Position);
            if (Action.Send(ref Reason))
            {
                Player = FAction.FTarget;
                if (FEvent != null && FEvent.FAction.Proposal.SignedAsOtherPlayer)
                {
                    Player = FEvent.FAction.FOtherPlayer;
                }

                FinishEvent();
                ShowPlayerActions(Player);
            }
            else
            {
                AoWHSMap.Current.ShowMessage(Reason);
            }
        }
    }

    // AoWRelations.pas:901
    public void ReplyBtnClick()
    {
        DiplomaticAction Action;
        Action = AoWHSMap.Current.FPlayers.GetPlayers(FAction.FTarget).CreateDiplomaticAction(new SendMessage(), FAction.FSource, -1);
        ClearActions();
        SetAction(Action);
        RNewActionPnl.Visible = false;
        RMsgEditPnl.Visible = true;
    }

    // AoWRelations.pas:912
    public void RMCancelBtnClick()
    {
        if (FEvent != null)
        {
            FinishEvent();
        }

        ShowPlayerActions(FAction.FSource);
    }

    // AoWRelations.pas:918
    public void IIOkBtnClick()
    {
        FinishEvent();
        if (FAction.FSource == AoWHSMap.Current.FSeatedPlayer)
        {
            ShowPlayerActions(FAction.FTarget);
        }
        else
        {
            ShowPlayerActions(FAction.FSource);
        }
    }

    // AoWRelations.pas:925
    public void RelWinKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            RelWin.Visible = false;
        }
    }

    private static readonly string EvilRStr = "Evil";

    private static readonly string GoodRStr = "Good";

    private static readonly string NoContactRStr = "No Contact";

    internal static void ShowDiplomaticEvent(RelationWindow Dialog, DiplomaticActionEventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.ClearActions();
        Dialog.ResetChecks();
        // { Original reports its source filename, independent of this checkout. }
        if (Dialog.FEvent != null)
        {
            WindowActions.OriginalAssert("Assertion failure", "D:\\AoWDev\\AoWRelations.pas", 663);
        }

        Dialog.FDone = Done;
        Dialog.FEvent = Event;
        Event.AddRef();

        if (AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStatus == Player.Status.Playing)
        {
            Dialog.ShowRelations(Event.FAction.FTarget);
            Dialog.SetAction(Event.FAction);
            var proposal = Dialog.FAction.Proposal;
            var view = proposal.View;
            if (view == ProposalView.Message)
            {
                var from = proposal.SignedAsOtherPlayer ? Dialog.FAction.FOtherPlayer : Dialog.FAction.FSource;
                Dialog.HighlightPlayer(from);
                Dialog.ShowPlayerInformation(from);
                Dialog.RNewActionPnl.Visible = false;
                Dialog.RMsgRecPnl.Visible = true;
                var text = proposal.MessageText;
                Dialog.RMMemo.Items.Text = text;
                var name = AoWHSMap.Current.FPlayers.GetPlayers(from).GetName();
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.MessageFromXRStr), [name]);
                Dialog.RMHeaderLbl.Text = format;
            }
            else if (view == ProposalView.Offer)
            {
                Dialog.RNewActionPnl.Visible = false;
                if (Dialog.FAction.FReaction == DiplomaticActionReaction.None)
                {
                    Dialog.HighlightPlayer(Dialog.FAction.FSource);
                    Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                    Dialog.RIncomingPnl.Visible = true;
                    var (_, gold, mana) = proposal.ToFields();
                    Dialog.MBGoldBribeV.Text = IntToStr(gold);
                    Dialog.MBManaBribeV.Text = IntToStr(mana);
                    var text2 = proposal.MessageText;
                    Dialog.MMemo.Items.Text = text2;
                    if (proposal.Icon >= 0)
                    {
                        Dialog.MIcon.CurrentImage = proposal.Icon;
                    }
                }
                else
                {
                    if (AoWHSMap.Current.FSeatedPlayer == Dialog.FAction.FSource && Dialog.FAction.FReaction != (DiplomaticActionReaction)3)
                    {
                        Dialog.HighlightPlayer(Dialog.FAction.FTarget);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FTarget);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        Dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            Dialog.IIIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        Dialog.FinishEvent();
                        if (Dialog.FAction.FReaction == (DiplomaticActionReaction)3)
                        {
                            Dialog.ShowPlayerActions(Dialog.FAction.FTarget);
                        }
                        else
                        {
                            Dialog.ShowPlayerActions(Dialog.FAction.FSource);
                        }
                    }
                }
            }
            else if (view == ProposalView.Notice)
            {
                if (AoWHSMap.Current.FSeatedPlayer == Dialog.FAction.FSource)
                {
                    Dialog.FinishEvent();
                    Dialog.ShowPlayerActions(Dialog.FAction.FTarget);
                }
                else
                {
                    if (AoWHSMap.Current.FSeatedPlayer == Dialog.FAction.FTarget)
                    {
                        Dialog.HighlightPlayer(Dialog.FAction.FSource);
                        Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                        Dialog.RNewActionPnl.Visible = false;
                        Dialog.IncommingInfoPnl.Visible = true;
                        var text2 = proposal.MessageText;
                        Dialog.IIMemo.Items.Text = text2;
                        if (proposal.Icon >= 0)
                        {
                            Dialog.IIIcon.CurrentImage = proposal.Icon;
                        }
                    }
                    else
                    {
                        Dialog.FinishEvent();
                        Dialog.ShowPlayerActions(Dialog.FAction.FSource);
                    }
                }
            }
            else if (view == ProposalView.Threat)
            {
                Dialog.HighlightPlayer(Dialog.FAction.FSource);
                Dialog.ShowPlayerInformation(Dialog.FAction.FSource);
                Dialog.RNewActionPnl.Visible = false;
                Dialog.IncommingInfoPnl.Visible = true;
                var text3 = proposal.MessageText;
                Dialog.IIMemo.Items.Text = text3;
                if (proposal.Icon >= 0)
                {
                    Dialog.IIIcon.CurrentImage = proposal.Icon;
                }
            }
        }
        else
        {
            Dialog.FinishDialog();
        }
    }

}
