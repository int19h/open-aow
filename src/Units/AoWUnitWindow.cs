// Translated from AoWUnitWindow.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.E3Globals.E3Globals;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;

namespace Aow.Units.AoWUnitWindow
{
    // highlight. dragged item type. highlight step. blink state. blink delay. cursor image. ground
    // offset/count. show info
    public partial class TUnitWindow : TDataModule
    {
        public TUnitWindow(TComponent owner) : base(owner)
        { }

        public TAOWWindow UnitWin;
        public TAOWImage UnitFrameLeft;
        public TAOWTabPanel UnitTab;
        public TAOWButton Close;
        public TAOWLabel Title;
        public TAOWButton Help;
        public TAOWImage TitleIcon;
        public TAOWButton BCloseBtn;
        public TAOWImage CeltLineR;
        public TAOWMemo T2Memo;
        public TAOWImage ManaIcon;
        public TAOWLabel Mana;
        public TAOWListBox T1AbilityLB;
        public TAOWButton T1UseBut;
        public TAOWButton T1SortName;
        public TAOWButton T1SortType;
        public TAOWButton T1SortSource;
        public TAOWLabel T1UseMsg;
        public TAOWPanel T1Panel;
        public TAOWPanel T2Panel;
        public TAOWListBox T1TypeLB;
        public TAOWListBox T1SrcLB;
        public TAOWPanel T1ListPnl;
        public TAOWVScrollBar T1SBar;
        public TAOWVScrollBar T2SBar;
        public TAOWPanel T2Text;
        public TAOWPanel T2Leader;
        public TAOWButton Sel4;
        public TAOWPanel SelPnl;
        public TAOWButton Sel3;
        public TAOWButton Sel2;
        public TAOWButton Sel1;
        public TAOWButton Sel5;
        public TAOWButton Sel6;
        public TAOWButton Sel7;
        public TAOWImage AAlignment;
        public TAOWLabel AAlignmentLbl;
        public TAOWPanel T2Alignnl;
        public TAoWFrame AlFr;
        public TAOWImage TypeIcn;
        public TAOWLabel TypeV;
        public TAOWPanel T2TypePnl;
        public TAoWFrame TypeFr;
        public TAoWFrame LdrFr;
        public TAOWLabel SphereLbl;
        public TAOWPanel T3Panel;
        public TAOWCheckBox BP1;
        public TAOWLabel BPLbl1;
        public TAOWCheckBox BP2;
        public TAOWCheckBox BP3;
        public TAOWCheckBox BP4;
        public TAOWCheckBox BP5;
        public TAOWCheckBox BP6;
        public TAOWCheckBox BP7;
        public TAOWCheckBox BP8;
        public TAOWLabel BPLbl2;
        public TAOWLabel BodyLbl;
        public TAOWCheckBox Head;
        public TAOWCheckBox Torso;
        public TAOWCheckBox Attack;
        public TAOWCheckBox Defend;
        public TAOWCheckBox RingL;
        public TAOWCheckBox RingR;
        public TAOWCheckBox Gnd3;
        public TAOWCheckBox Gnd2;
        public TAOWCheckBox Gnd1;
        public TAOWCheckBox Gnd4;
        public TAOWCheckBox Gnd5;
        public TAOWLabel GndLbl;
        public TAOWImage SlotBack;
        public TAOWButton DisbandBtn;
        public TAOWPanel ItemUsePnl;
        public TAOWButton ItemUseBtn;
        public TAOWImage ItemPic;
        public TAOWPanel UnitMainPnl;
        public TAOWLabel UnitName;
        public TAOWImage UnitFrRight;
        public TAOWImage UAttackIcon;
        public TAOWImage UDefenseIcon;
        public TAOWImage UMovesIcon;
        public TAOWImage UDamageIcon;
        public TAOWImage UResistanceIcon;
        public TAOWImage UHitsIcon;
        public TAOWLabel UAttack;
        public TAOWLabel UDefense;
        public TAOWLabel UMoves;
        public TAOWLabel UDamage;
        public TAOWLabel UResistance;
        public TAOWLabel UHits;
        public TAOWPanel UStatPnl;
        public TAOWPanel UStatP1;
        public TAOWPanel UStatP2;
        public TAOWPanel UnitFr;
        public TAOWLabel Level;
        public TAOWImage UnitLine1;
        public TAOWImage UnitLine2;
        public TAOWImage FrLSh;
        public TAOWImage FrRSh;
        public TAOWPanel RightFramePnl;
        public TAOWLabel MoraleLbl;
        public TAOWImage UpkeepIcon;
        public TAOWLabel UpKeep;
        public TAoWFrame BCeltL;
        public TAOWImage CeltLineL;
        public TAOWButton GroundL;
        public TAOWButton GroundR;
        public TAOWImage RelationIcn;
        public TAOWLabel RelationV;
        public TAOWPanel T2RelPnl;
        public TAoWFrame RelFr;
        public TAOWImage RelRing;
        public TAOWLabel RelationLbl;
        public TAOWLabel AlignLbl;
        public TAOWLabel TypeLbl;
        public TAOWLabel XP;
        public TAOWButton PNext;
        public TAOWButton PPrev;
        public TIvTranslator Translator;
        public int FHighlight;
        public TItemType FItemType;
        public int FHighlightStep;
        public bool FItemVisible;
        public int FBlinkDelay;
        public ImageLib.TLibraryImage FCursorImage;
        public int FGroundOffset;
        public int FGroundCount;
        public TP2UnitShowInfo FShowInfo;
        // AoWUnitWindow.pas:985
        public void UpdateGround(int Offset)
        {
            TAbstractUnit U = default;
            Gnd1.Tag = -1;
            Gnd2.Tag = -1;
            Gnd3.Tag = -1;
            Gnd4.Tag = -1;
            Gnd5.Tag = -1;
            Gnd1.Checked = false;
            Gnd2.Checked = false;
            Gnd3.Checked = false;
            Gnd4.Checked = false;
            Gnd5.Checked = false;
            U = General.FSelectedUnit;
            if (U != null)
            {
                if (U is THero)
                {
                    if (((THero)U).GetItemsOnGround() != null)
                    {
                        var call__1 = ((THero)U).GetItemsOnGround();
                        var call__2 = call__1.Count;
                        FGroundCount = call__2;
                        FGroundOffset = Offset;
                        // { Original last-page clamp leaves only four items visible. }
                        if (FGroundCount < Offset + 5)
                        {
                            FGroundOffset = FGroundCount - 4;
                        }

                        if (FGroundOffset < 0)
                        {
                            FGroundOffset = 0;
                        }

                        GroundL.Visible = FGroundOffset > 0;
                        GroundR.Visible = FGroundOffset + 5 <= FGroundCount;
                        var call__3 = ((THero)U).GetItemsOnGround();
                        var call__4 = call__3.Count;
                        if (FGroundOffset < call__4)
                        {
                            var call__5 = ((THero)U).GetItemsOnGround();
                            var call__6 = call__5.GetItem(FGroundOffset);
                            Gnd1.Tag = call__6.FItemID;
                        }

                        var call__7 = ((THero)U).GetItemsOnGround();
                        var call__8 = call__7.Count;
                        if (FGroundOffset + 1 < call__8)
                        {
                            var call__9 = ((THero)U).GetItemsOnGround();
                            var call__10 = call__9.GetItem(FGroundOffset + 1);
                            Gnd2.Tag = call__10.FItemID;
                        }

                        var call__11 = ((THero)U).GetItemsOnGround();
                        var call__12 = call__11.Count;
                        if (FGroundOffset + 2 < call__12)
                        {
                            var call__13 = ((THero)U).GetItemsOnGround();
                            var call__14 = call__13.GetItem(FGroundOffset + 2);
                            Gnd3.Tag = call__14.FItemID;
                        }

                        var call__15 = ((THero)U).GetItemsOnGround();
                        var call__16 = call__15.Count;
                        if (FGroundOffset + 3 < call__16)
                        {
                            var call__17 = ((THero)U).GetItemsOnGround();
                            var call__18 = call__17.GetItem(FGroundOffset + 3);
                            Gnd4.Tag = call__18.FItemID;
                        }

                        var call__19 = ((THero)U).GetItemsOnGround();
                        var call__20 = call__19.Count;
                        if (FGroundOffset + 4 < call__20)
                        {
                            var call__21 = ((THero)U).GetItemsOnGround();
                            var call__22 = call__21.GetItem(FGroundOffset + 4);
                            Gnd5.Tag = call__22.FItemID;
                        }
                    }
                }
            }
        }

        // AoWUnitWindow.pas:472
        public void PreDrawAbilityItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
        {
            bool Enabled = default;
            TAbility A = default;
            var call__23 = T1AbilityLB.Items.Objects[DrawItem];
            var call__24 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__23));
            A = call__24;
            if (AoWHSMap.FCombatMap == null)
            {
                bool bool__25 = A != null && General.FSelectedUnit != null && General.FSelectedUnit.FPlayer == AoWHSMap.FSeatedPlayer;
                if (bool__25)
                {
                    var call__26 = A.GetControlType(General.FSelectedUnit);
                    bool__25 = call__26.Contains(TAbilityControlTypeElement.acAction);
                }

                Enabled = bool__25;
            }
            else
            {
                bool bool__27 = A != null && General.FCombatUnit != null;
                if (bool__27)
                {
                    var call__28 = T1AbilityLB.Items.Objects[DrawItem];
                    var call__29 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__28));
                    var call__30 = call__29.GetControlType(General.FCombatUnit.FUnit.FUnit);
                    bool__27 = call__30.Contains(TAbilityControlTypeElement.acCombatAction);
                }

                bool bool__31 = bool__27;
                if (bool__31)
                {
                    var call__32 = General.FCombatUnit.FUnit.GetPlayer();
                    bool__31 = call__32 == AoWCombatMap.FSeatedPlayer;
                }

                bool bool__33 = bool__31 && General.FCombatUnit.FUnit.FMovePoints > 0;
                if (bool__33)
                {
                    var call__34 = T1AbilityLB.Items.Objects[DrawItem];
                    var call__35 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__34));
                    var call__36 = call__35.GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                    bool bool__37 = TSet<TCombatMode>.Of(TCombatMode.cmShortRange, TCombatMode.cmMediumRange, TCombatMode.cmLongRange).Contains(call__36);
                    if (!bool__37)
                    {
                        var call__38 = T1AbilityLB.Items.Objects[DrawItem];
                        bool__37 = ObjectToInt(call__38) == 52;
                    }

                    bool bool__39 = !bool__37;
                    if (!bool__39)
                    {
                        var call__40 = General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__39 = call__40 == General.FCombatUnit.FUnit.FMovePoints;
                    }

                    bool__33 = bool__39;
                }

                Enabled = bool__33;
            }

            if (Enabled)
            {
                Sender.TextColor = 14679807;
            }
            else
            {
                Sender.TextColor = 9415072;
            }
        }

        // AoWUnitWindow.pas:502
        public void HelpClick(TObject Sender)
        {
            General.PlayButtonSound(0);
        }

        // AoWUnitWindow.pas:507
        public void T1AbilityLBChange(TObject Sender)
        {
            TAbility A = default;
            T1AbilityLB.Selected = ((TAOWListBox)Sender).Selected;
            T1TypeLB.Selected = ((TAOWListBox)Sender).Selected;
            T1SrcLB.Selected = ((TAOWListBox)Sender).Selected;
            if (AoWHSMap.FCombatMap == null)
            {
                if (General.FSelectedUnit.FPlayer == AoWHSMap.FSeatedPlayer && T1AbilityLB.Selected > -1)
                {
                    var call__41 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__42 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__41));
                    A = call__42;
                    bool bool__43 = A != null;
                    if (bool__43)
                    {
                        var call__44 = A.GetEnabled(General.FSelectedUnit);
                        bool__43 = call__44;
                    }

                    if (bool__43)
                    {
                        if (AoWHSMap.FCombatMap == null)
                        {
                            var call__45 = A.GetControlType(General.FSelectedUnit);
                            T1UseBut.Visible = call__45.Contains(TAbilityControlTypeElement.acAction);
                        }
                        else
                        {
                            var call__46 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                            var call__47 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__46));
                            var call__48 = call__47.GetCombatMode(General.FSelectedUnit);
                            bool bool__49 = call__48 != TCombatMode.cmNone;
                            if (bool__49)
                            {
                                var call__50 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                                var call__51 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__50));
                                var call__52 = call__51.GetCombatMode(General.FSelectedUnit);
                                bool__49 = call__52 != TCombatMode.cmMelee;
                            }

                            T1UseBut.Visible = bool__49;
                        }

                        if (T1UseBut.Visible)
                        {
                            var call__53 = A.GetName(General.FSelectedUnit);
                            var call__54 = TranslateRStr(UseXRStr);
                            var call__55 = AoWE.AoWE.Format(call__54, [call__53]);
                            T1UseBut.Text = call__55;
                        }
                    }
                    else
                    {
                        T1UseBut.Visible = false;
                    }
                }
                else
                {
                    T1UseBut.Visible = false;
                }
            }
            else
            {
                bool bool__56 = T1AbilityLB.Selected > -1 && General.FCombatUnit != null;
                if (bool__56)
                {
                    var call__57 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__58 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__57));
                    var call__59 = call__58.GetControlType(General.FCombatUnit.FUnit.FUnit);
                    bool__56 = call__59.Contains(TAbilityControlTypeElement.acCombatAction);
                }

                bool bool__60 = bool__56;
                if (bool__60)
                {
                    var call__61 = General.FCombatUnit.FUnit.GetPlayer();
                    bool__60 = call__61 == AoWCombatMap.FSeatedPlayer;
                }

                bool bool__62 = bool__60 && General.FCombatUnit.FUnit.FMovePoints > 0;
                if (bool__62)
                {
                    var call__63 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__64 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__63));
                    var call__65 = call__64.GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                    bool bool__66 = TSet<TCombatMode>.Of(TCombatMode.cmShortRange, TCombatMode.cmMediumRange, TCombatMode.cmLongRange).Contains(call__65);
                    if (!bool__66)
                    {
                        var call__67 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        bool__66 = ObjectToInt(call__67) == 52;
                    }

                    bool bool__68 = !bool__66;
                    if (!bool__68)
                    {
                        var call__69 = General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__68 = call__69 == General.FCombatUnit.FUnit.FMovePoints;
                    }

                    bool__62 = bool__68;
                }

                if (bool__62)
                {
                    T1UseBut.Visible = true;
                    var call__70 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    var call__71 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__70));
                    var call__72 = call__71.GetName(General.FCombatUnit.FUnit.FUnit);
                    var call__73 = TranslateRStr(UseXRStr);
                    var call__74 = AoWE.AoWE.Format(call__73, [call__72]);
                    T1UseBut.Text = call__74;
                }
                else
                {
                    T1UseBut.Visible = false;
                }
            }
        }

        // AoWUnitWindow.pas:562
        public void T1SbarChange(TObject Sender)
        {
            T1AbilityLB.ListOffset = T1SBar.Position;
            T1TypeLB.ListOffset = T1SBar.Position;
        }

        // AoWUnitWindow.pas:568
        public void ItemRHMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAOWCheckBox Check = default;
            Check = (TAOWCheckBox)Sender;
            AoWItemBanner.AoWItemBanner.ItemID = Check.Tag;
        }

        // AoWUnitWindow.pas:575
        public void UnitWinResize(TObject Sender)
        {
            this.UnitWin.Alignment.LeftOffset = this.UnitWin.WinLeft;
            this.UnitWin.Alignment.TopOffset = this.UnitWin.WinTop;
            this.UnitWin.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            this.UnitWin.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
        }

        // AoWUnitWindow.pas:586
        public void UnitWindowCreate(TObject Sender)
        {
            FItemType = (TItemType)6;
        }

        // AoWUnitWindow.pas:591
        public void UnitWinUpdate(TObject Sender)
        {
            if (UnitWin.Visible)
            {
                FHighlight += FHighlightStep;
                if (FHighlight > 100 || FHighlight < 50)
                {
                    FHighlightStep = -FHighlightStep;
                    FHighlight += FHighlightStep;
                }

                switch (FItemType)
                {
                    case TItemType.itAttack:
                        Attack.FDrawn = false;
                        break;
                    case TItemType.itDefense:
                        Defend.FDrawn = false;
                        break;
                    case TItemType.itHead:
                        Head.FDrawn = false;
                        break;
                    case TItemType.itTorso:
                        Torso.FDrawn = false;
                        break;
                    case TItemType.itRing:
                        RingL.FDrawn = false;
                        RingR.FDrawn = false;
                        break;
                }

                if (AoWUnitWindow.SelectedItem != null)
                {
                    FBlinkDelay -= 1;
                    if (FBlinkDelay < 0)
                    {
                        FBlinkDelay = 2;
                        FItemVisible = !FItemVisible;
                    }
                }
            }
        }

        // AoWUnitWindow.pas:620
        public void T1SortNameClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T1AbilityLB);
            Lists.Add(T1TypeLB);
            Lists.Add(T1SrcLB);
            General.SortLists(ref Lists, 0);
            T1UseBut.Visible = false;
        }

        // AoWUnitWindow.pas:631
        public void T1SortTypeClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T1TypeLB);
            Lists.Add(T1AbilityLB);
            Lists.Add(T1SrcLB);
            General.SortLists(ref Lists, 0);
            T1UseBut.Visible = false;
        }

        // AoWUnitWindow.pas:642
        public void T1SortSourceClick(TObject Sender, int Button)
        {
            TList Lists = default;
            General.PlayButtonSound(0);
            Lists = new TList();
            Lists.Add(T1SrcLB);
            Lists.Add(T1AbilityLB);
            Lists.Add(T1TypeLB);
            General.SortLists(ref Lists, 0);
            T1UseBut.Visible = false;
        }

        // AoWUnitWindow.pas:653
        public void UseAbility()
        {
            TAbility A = default;
            AnsiString Reason = default;
            General.PlayButtonSound(0);
            var call__76 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
            var call__77 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__76));
            A = call__77;
            bool bool__78 = A != null;
            if (bool__78)
            {
                var call__79 = A.GetEnabled(General.FSelectedUnit);
                bool__78 = call__79;
            }

            if (bool__78)
            {
                if (AoWHSMap.FCombatMap == null)
                {
                    if (General.FSelectedUnit.FPlayer == AoWHSMap.FSeatedPlayer)
                    {
                        var call__80 = A.Activate(General.FSelectedUnit, ref Reason);
                        if (call__80)
                        {
                            UnitWin.Visible = false;
                        }
                        else
                        {
                            if (Reason != (AnsiString)"")
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }
                        }
                    }
                }
                else
                {
                    if (General.FCombatUnit != null && General.FCombatUnit.FUnit.FCombatPlayer == AoWCombatMap.FSeatedCombatPlayer)
                    {
                        var call__81 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        var call__82 = General.FCombatUnit.SelectAbility(ObjectToInt(call__81));
                        if (call__82 > -1)
                        {
                            UnitWin.Visible = false;
                        }
                    }
                }
            }
        }

        // AoWUnitWindow.pas:676
        public void UseItemClick(TObject Sender)
        {
            AnsiString Reason = default;
            General.PlayButtonSound(0);
            if (AoWUnitWindow.SelectedItem != null && General.FSelectedUnit is THero)
            {
                var call__83 = ((THero)General.FSelectedUnit).CanUseItem(AoWUnitWindow.SelectedItem.Tag, ref Reason);
                if (call__83)
                {
                    ((THero)General.FSelectedUnit).UseItem(AoWUnitWindow.SelectedItem.Tag);
                }
                else
                {
                    AoWHSMap.ShowWarning(Reason);
                }
            }
        }

        // AoWUnitWindow.pas:688
        public void UnitTabChange(TObject Sender)
        {
            General.PlayButtonSound(0);
        }

        // AoWUnitWindow.pas:693
        public void UnitWinMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (Button == TMouseButton.mbRight && AoWUnitWindow.SelectedItem != null)
            {
                Gnd1Change(AoWUnitWindow.SelectedItem);
            }
        }

        // AoWUnitWindow.pas:714
        public void GroundRClick(TObject Sender)
        {
            UpdateGround(FGroundOffset + 1);
        }

        // AoWUnitWindow.pas:719
        public void GroundLClick(TObject Sender)
        {
            UpdateGround(FGroundOffset - 1);
        }

        // AoWUnitWindow.pas:724
        public void CloseClick(TObject Sender)
        {
            General.PlayButtonSound(0);
            UnitWin.Visible = false;
        }

        // AoWUnitWindow.pas:730
        public void Gnd1Draw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            THero U = default;
            TItem Item = default;
            TAoWComponent Component = default;
            Component = Sender;
            if (Component.Tag != -1)
            {
                U = (THero)General.FSelectedUnit;
                var call__84 = U.FInventory.FindItem(Component.Tag);
                Item = call__84;
                if (Item == null)
                {
                    var call__85 = U.FHeroItems.FindItem(Component.Tag);
                    Item = call__85;
                }

                if (Item == null)
                {
                    if (U.GetItemsOnGround() != null)
                    {
                        var call__86 = U.GetItemsOnGround();
                        var call__87 = call__86.FindItem(Component.Tag);
                        Item = call__87;
                    }
                }

                if (Item != null)
                {
                    var panel = T3Panel;
                    var checkBox = (TAOWCheckBox)Component;
                    if (Item.GetImage() != null)
                    {
                        var call__90 = Item.GetImage();
                        call__90.ShowClipped(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc);
                    }
                }
            }
        }

        // AoWUnitWindow.pas:750
        public void FrLShPreDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            TRect R = default;
            if (General.FSelectedUnit != null)
            {
                var call__91 = General.FSelectedUnit.GetFace();
                if (call__91 != null)
                {
                    var image = (TAOWImage)Sender;
                    var call__93 = Rect(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);
                    R = call__93;

                    General.FSelectedUnit.ShowFace(R, SurfDesc);
                }
            }
        }

        // AoWUnitWindow.pas:763
        public void FrRShPreDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            if (General.FSelectedUnit != null)
            {
                FShowInfo.Direction = 3;
                if (General.FSelectedUnit is THero)
                {
                    FShowInfo.Overlays = TSet<TUnitOverlayModesElement>.Of();
                }
                else
                {
                    FShowInfo.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoRank);
                }

                var image = (TAOWImage)Sender;
                General.FSelectedUnit.ShowEx(image.WinLeft + 49, image.WinTop + 78, SurfDesc, FShowInfo);
            }
        }

        // AoWUnitWindow.pas:855
        public void Gnd3MouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (General.FSelectedUnit != null)
            {
                var call__95 = AoWHSMap.FPlayers.GetPlayers(General.FSelectedUnit.FPlayer);
                var call__96 = call__95.GetBusy();
                if (!call__96)
                {
                    if (Button == TMouseButton.mbRight)
                    {
                        if (((TAOWCheckBox)Sender).Tag == -1)
                        {
                            ((TAOWCheckBox)Sender).PopupMenu = null;
                        }
                        else
                        {
                            ((TAOWCheckBox)Sender).PopupMenu = AoWItemBanner.AoWItemBanner.ItemBanner.IBannerPopup;
                        }

                        AoWItemBanner.AoWItemBanner.ItemID = ((TAOWCheckBox)Sender).Tag;
                    }
                    else
                    {
                        if (Sender == AoWUnitWindow.SelectedItem)
                        {
                            AoWUnitWindow.SelectedItem.Checked = false;
                        }
                    }
                }
            }
        }

        // AoWUnitWindow.pas:871
        public void HeadDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (AoWUnitWindow.SelectedItem != null && Sender != AoWUnitWindow.SelectedItem && FItemType == 0)
            {
                var checkBox = (TAOWCheckBox)Sender;
                var call__99 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__99.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:881
        public void TorsoDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (AoWUnitWindow.SelectedItem != null && Sender != AoWUnitWindow.SelectedItem && FItemType == (TItemType)1)
            {
                var checkBox = (TAOWCheckBox)Sender;
                var call__102 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__102.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:891
        public void AttackDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (AoWUnitWindow.SelectedItem != null && Sender != AoWUnitWindow.SelectedItem && FItemType == (TItemType)2)
            {
                var checkBox = (TAOWCheckBox)Sender;
                var call__105 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__105.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:901
        public void DefendDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (AoWUnitWindow.SelectedItem != null && Sender != AoWUnitWindow.SelectedItem && FItemType == (TItemType)3)
            {
                var checkBox = (TAOWCheckBox)Sender;
                var call__108 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__108.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:911
        public void RingLDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            Gnd1Draw(Sender, SurfDesc);
            if (AoWUnitWindow.SelectedItem != null && Sender != AoWUnitWindow.SelectedItem && FItemType == (TItemType)4)
            {
                var checkBox = (TAOWCheckBox)Sender;
                var call__111 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin[24];
                call__111.ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
            }
        }

        // AoWUnitWindow.pas:921
        public void DisbandBtnClick()
        {
            var call__112 = TranslateRStr(DisbandUnitRStr);
            var call__113 = TranslateRStr(E2Globals.E2Globals.ConfirmRStr);
            DialogWin.Confirm(call__113, call__112, 7);
        }

        // AoWUnitWindow.pas:926
        public void T1AbilityLBMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility A = default;
            if (Button == TMouseButton.mbRight && General.FSelectedUnit != null && T1AbilityLB.Selected != -1)
            {
                var call__114 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var call__115 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__114));
                A = call__115;
                if (A != null)
                {
                    var call__116 = A.GetName(null);
                    SetInfoBanner(InfoBanner, call__116, A.FDescription);
                }
            }
        }

        // AoWUnitWindow.pas:937
        public void UnitWinKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            switch (Key)
            {
                case 27:
                    UnitWin.Visible = false;
                    break;
            }
        }

        // AoWUnitWindow.pas:944
        public void PPrevClick()
        {
            int I = default;
            General.PlayButtonSound(0);
            if (General.FUnitList != null)
            {
                var call__117 = General.IndexOfUnit(General.FSelectedUnit);
                I = call__117;
                if (I > -1 && General.FSelectedUnits[0] > -1)
                {
                    I -= 1;
                    if (I < 0)
                    {
                        I = 7;
                        while (General.FSelectedUnits[I] == -1)
                        {
                            I -= 1;
                        }
                    }

                    var call__118 = General.FUnitList.GetUnit(General.FSelectedUnits[I]);
                    General.FSelectedUnit = call__118;
                    ShowUnit();
                }
            }
        }

        // AoWUnitWindow.pas:966
        public void PNextClick()
        {
            int I = default;
            General.PlayButtonSound(0);
            if (General.FUnitList != null)
            {
                var call__119 = General.IndexOfUnit(General.FSelectedUnit);
                I = call__119;
                if (I > -1)
                {
                    I += 1;
                    if (I > 7)
                    {
                        I = 0;
                    }

                    if (General.FSelectedUnits[I] == -1)
                    {
                        I = 0;
                    }

                    var call__120 = General.FUnitList.GetUnit(General.FSelectedUnits[I]);
                    General.FSelectedUnit = call__120;
                    ShowUnit();
                }
            }
        }
    }

    public static partial class AoWUnitWindow
    {
        public static TUnitWindow UnitWindow;
        public static TAOWCheckBox SelectedItem;
        public static readonly AnsiString AbilitiesRStr = (AnsiString)"Abilities";
        public static readonly AnsiString InfoRStr = (AnsiString)"Info";
        public static readonly AnsiString NoneRStr = (AnsiString)"None";
        public static readonly AnsiString CreatureRStr = (AnsiString)"Creature";
        public static readonly AnsiString MachineRStr = (AnsiString)"Machine";
        public static readonly AnsiString UnitRStr = (AnsiString)"Unit";
        public static readonly AnsiString LeaderRStr = (AnsiString)"Leader";
        public static readonly AnsiString HeroRStr = (AnsiString)"Hero";
        public static readonly AnsiString NoLeaderRStr = (AnsiString)"Hero has no Leader";
        public static readonly AnsiString LeaderSpheresRStr = (AnsiString)"Leaders magic spheres";
        public static readonly AnsiString ItemsRStr = (AnsiString)"Items";
        public static readonly AnsiString LevelRStr = (AnsiString)"Level";
        public static readonly AnsiString ExpRStr = (AnsiString)"Exp";
        // AoWUnitWindow.pas:220
        public static int CompareUnitAbilities(AnsiString S1, AnsiString S2, TObject O1, TObject O2)
        {
            int Result = default;
            TAbility A1 = default;
            TAbility A2 = default;
            int V1 = default;
            int V2 = default;
            var call__121 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(O1));
            A1 = call__121;
            var call__122 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(O2));
            A2 = call__122;
            var call__123 = A1.GetEnabled(General.FSelectedUnit);
            bool bool__124 = call__123;
            if (bool__124)
            {
                var call__125 = A1.GetControlType(General.FSelectedUnit);
                bool__124 = call__125.Contains(TAbilityControlTypeElement.acAction);
            }

            if (bool__124)
            {
                V1 = 10;
            }
            else
            {
                V1 = 0;
            }

            var call__126 = A2.GetEnabled(General.FSelectedUnit);
            bool bool__127 = call__126;
            if (bool__127)
            {
                var call__128 = A2.GetControlType(General.FSelectedUnit);
                bool__127 = call__128.Contains(TAbilityControlTypeElement.acAction);
            }

            if (bool__127)
            {
                V2 = 10;
            }
            else
            {
                V2 = 0;
            }

            Result = V2 - V1;
            if (Result == 0)
            {
                Result = CompareStr(S1, S2);
            }

            return Result;
        }
    }
}
