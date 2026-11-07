// Translated from AoWUnitWindow.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWItemBanner;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.E2Globals;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWInfoBanner.AoWInfoBannerUnit;
using static Aow.Units.AoWTC.AoWTCUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E2Globals.E2GlobalsUnit;
using static Aow.Units.E3Globals.E3GlobalsUnit;
using static Aow.Units.GFXE.GFXEUnit;

namespace Aow.Units.AoWUnitWindow;

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
        TAbstractUnit U;
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
                    FGroundCount = ((THero)U).GetItemsOnGround().Count;
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
                    var count = ((THero)U).GetItemsOnGround().Count;
                    if (FGroundOffset < count)
                    {
                        var item = ((THero)U).GetItemsOnGround().GetItem(FGroundOffset);
                        Gnd1.Tag = item.FItemID;
                    }

                    var count2 = ((THero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 1 < count2)
                    {
                        var item = ((THero)U).GetItemsOnGround().GetItem(FGroundOffset + 1);
                        Gnd2.Tag = item.FItemID;
                    }

                    var count3 = ((THero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 2 < count3)
                    {
                        var item = ((THero)U).GetItemsOnGround().GetItem(FGroundOffset + 2);
                        Gnd3.Tag = item.FItemID;
                    }

                    var count4 = ((THero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 3 < count4)
                    {
                        var item = ((THero)U).GetItemsOnGround().GetItem(FGroundOffset + 3);
                        Gnd4.Tag = item.FItemID;
                    }

                    var count5 = ((THero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 4 < count5)
                    {
                        var item = ((THero)U).GetItemsOnGround().GetItem(FGroundOffset + 4);
                        Gnd5.Tag = item.FItemID;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:472
    public void PreDrawAbilityItem(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc)
    {
        bool Enabled;
        TAbility A;
        var objectValue = T1AbilityLB.Items.Objects[DrawItem];
        A = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
        if (AoWHSMap.FCombatMap == null)
        {
            Enabled = A != null && General.FSelectedUnit != null && General.FSelectedUnit.FPlayer == AoWHSMap.FSeatedPlayer && A.GetControlType(General.FSelectedUnit).Contains(TAbilityControlTypeElement.acAction);
        }
        else
        {
            bool condition = A != null && General.FCombatUnit != null;
            if (condition)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[DrawItem];
                condition = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetControlType(General.FCombatUnit.FUnit.FUnit).Contains(TAbilityControlTypeElement.acCombatAction);
            }

            bool test = condition && General.FCombatUnit.FUnit.GetPlayer() == AoWCombatMap.FSeatedPlayer && General.FCombatUnit.FUnit.FMovePoints > 0;
            if (test)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[DrawItem];
                var combatMode = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                test = !(TSet<TCombatMode>.Of(TCombatMode.cmShortRange, TCombatMode.cmMediumRange, TCombatMode.cmLongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[DrawItem]) == 52) || General.FCombatUnit.FUnit.FUnit.GetMoves() == General.FCombatUnit.FUnit.FMovePoints;
            }

            Enabled = test;
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
        TAbility A;
        T1AbilityLB.Selected = ((TAOWListBox)Sender).Selected;
        T1TypeLB.Selected = ((TAOWListBox)Sender).Selected;
        T1SrcLB.Selected = ((TAOWListBox)Sender).Selected;
        if (AoWHSMap.FCombatMap == null)
        {
            if (General.FSelectedUnit.FPlayer == AoWHSMap.FSeatedPlayer && T1AbilityLB.Selected > -1)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                A = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
                if (A != null && A.GetEnabled(General.FSelectedUnit))
                {
                    if (AoWHSMap.FCombatMap == null)
                    {
                        var controlType = A.GetControlType(General.FSelectedUnit);
                        T1UseBut.Visible = controlType.Contains(TAbilityControlTypeElement.acAction);
                    }
                    else
                    {
                        var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        bool condition = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(General.FSelectedUnit) != TCombatMode.cmNone;
                        if (condition)
                        {
                            var objectValue3 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                            condition = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue3)).GetCombatMode(General.FSelectedUnit) != TCombatMode.cmMelee;
                        }

                        T1UseBut.Visible = condition;
                    }

                    if (T1UseBut.Visible)
                    {
                        var name = A.GetName(General.FSelectedUnit);
                        var format = AoWEUnit.Format(TranslateRStr(UseXRStr), [name]);
                        T1UseBut.Text = format;
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
            bool condition = T1AbilityLB.Selected > -1 && General.FCombatUnit != null;
            if (condition)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                condition = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetControlType(General.FCombatUnit.FUnit.FUnit).Contains(TAbilityControlTypeElement.acCombatAction);
            }

            bool test = condition && General.FCombatUnit.FUnit.GetPlayer() == AoWCombatMap.FSeatedPlayer && General.FCombatUnit.FUnit.FMovePoints > 0;
            if (test)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var combatMode = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                test = !(TSet<TCombatMode>.Of(TCombatMode.cmShortRange, TCombatMode.cmMediumRange, TCombatMode.cmLongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[T1AbilityLB.Selected]) == 52) || General.FCombatUnit.FUnit.FUnit.GetMoves() == General.FCombatUnit.FUnit.FMovePoints;
            }

            if (test)
            {
                T1UseBut.Visible = true;
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var name = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetName(General.FCombatUnit.FUnit.FUnit);
                var format = AoWEUnit.Format(TranslateRStr(UseXRStr), [name]);
                T1UseBut.Text = format;
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
        TAOWCheckBox Check;
        Check = (TAOWCheckBox)Sender;
        AoWItemBannerUnit.ItemID = Check.Tag;
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

            if (AoWUnitWindowUnit.SelectedItem != null)
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
        TList Lists;
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
        TList Lists;
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
        TList Lists;
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
        TAbility A;
        string Reason = "";
        General.PlayButtonSound(0);
        var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
        A = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
        if (A != null && A.GetEnabled(General.FSelectedUnit))
        {
            if (AoWHSMap.FCombatMap == null)
            {
                if (General.FSelectedUnit.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    if (A.Activate(General.FSelectedUnit, ref Reason))
                    {
                        UnitWin.Visible = false;
                    }
                    else
                    {
                        if (!IsEmpty(Reason))
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
                    var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    if (General.FCombatUnit.SelectAbility(ObjectToInt(objectValue2)) > -1)
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
        string Reason = "";
        General.PlayButtonSound(0);
        if (AoWUnitWindowUnit.SelectedItem != null && General.FSelectedUnit is THero)
        {
            if (((THero)General.FSelectedUnit).CanUseItem(AoWUnitWindowUnit.SelectedItem.Tag, ref Reason))
            {
                ((THero)General.FSelectedUnit).UseItem(AoWUnitWindowUnit.SelectedItem.Tag);
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
        if (Button == TMouseButton.mbRight && AoWUnitWindowUnit.SelectedItem != null)
        {
            Gnd1Change(AoWUnitWindowUnit.SelectedItem);
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
        THero U;
        TItem Item;
        TAoWComponent Component;
        Component = Sender;
        if (Component.Tag != -1)
        {
            U = (THero)General.FSelectedUnit;
            Item = U.FInventory.FindItem(Component.Tag);
            if (Item == null)
            {
                Item = U.FHeroItems.FindItem(Component.Tag);
            }

            if (Item == null)
            {
                if (U.GetItemsOnGround() != null)
                {
                    Item = U.GetItemsOnGround().FindItem(Component.Tag);
                }
            }

            if (Item != null)
            {
                var checkBox = (TAOWCheckBox)Component;
                if (Item.GetImage() != null)
                {
                    Item.GetImage().ShowClipped(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc);
                }
            }
        }
    }

    // AoWUnitWindow.pas:750
    public void FrLShPreDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        TRect R;
        if (General.FSelectedUnit != null)
        {
            if (General.FSelectedUnit.GetFace() != null)
            {
                var image = (TAOWImage)Sender;
                R = Rect(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);

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
            if (!AoWHSMap.FPlayers.GetPlayers(General.FSelectedUnit.FPlayer).GetBusy())
            {
                if (Button == TMouseButton.mbRight)
                {
                    if (((TAOWCheckBox)Sender).Tag == -1)
                    {
                        ((TAOWCheckBox)Sender).PopupMenu = null;
                    }
                    else
                    {
                        ((TAOWCheckBox)Sender).PopupMenu = AoWItemBannerUnit.ItemBanner.IBannerPopup;
                    }

                    AoWItemBannerUnit.ItemID = ((TAOWCheckBox)Sender).Tag;
                }
                else
                {
                    if (Sender == AoWUnitWindowUnit.SelectedItem)
                    {
                        AoWUnitWindowUnit.SelectedItem.Checked = false;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:871
    public void HeadDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (AoWUnitWindowUnit.SelectedItem != null && Sender != AoWUnitWindowUnit.SelectedItem && FItemType == 0)
        {
            var checkBox = (TAOWCheckBox)Sender;
            aowIntGfxModUnit.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:881
    public void TorsoDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (AoWUnitWindowUnit.SelectedItem != null && Sender != AoWUnitWindowUnit.SelectedItem && FItemType == (TItemType)1)
        {
            var checkBox = (TAOWCheckBox)Sender;
            aowIntGfxModUnit.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:891
    public void AttackDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (AoWUnitWindowUnit.SelectedItem != null && Sender != AoWUnitWindowUnit.SelectedItem && FItemType == (TItemType)2)
        {
            var checkBox = (TAOWCheckBox)Sender;
            aowIntGfxModUnit.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:901
    public void DefendDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (AoWUnitWindowUnit.SelectedItem != null && Sender != AoWUnitWindowUnit.SelectedItem && FItemType == (TItemType)3)
        {
            var checkBox = (TAOWCheckBox)Sender;
            aowIntGfxModUnit.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:911
    public void RingLDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (AoWUnitWindowUnit.SelectedItem != null && Sender != AoWUnitWindowUnit.SelectedItem && FItemType == (TItemType)4)
        {
            var checkBox = (TAOWCheckBox)Sender;
            aowIntGfxModUnit.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:921
    public void DisbandBtnClick()
    {
        var disbandUnit = TranslateRStr(DisbandUnitRStr);
        var confirm = TranslateRStr(E2GlobalsUnit.ConfirmRStr);
        DialogWin.Confirm(confirm, disbandUnit, 7);
    }

    // AoWUnitWindow.pas:926
    public void T1AbilityLBMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility A;
        if (Button == TMouseButton.mbRight && General.FSelectedUnit != null && T1AbilityLB.Selected != -1)
        {
            var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
            A = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                SetInfoBanner(InfoBanner, name, A.FDescription);
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
        int I;
        General.PlayButtonSound(0);
        if (General.FUnitList != null)
        {
            I = General.IndexOfUnit(General.FSelectedUnit);
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

                var unit = General.FUnitList.GetUnit(General.FSelectedUnits[I]);
                General.FSelectedUnit = unit;
                ShowUnit();
            }
        }
    }

    // AoWUnitWindow.pas:966
    public void PNextClick()
    {
        int I;
        General.PlayButtonSound(0);
        if (General.FUnitList != null)
        {
            I = General.IndexOfUnit(General.FSelectedUnit);
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

                var unit = General.FUnitList.GetUnit(General.FSelectedUnits[I]);
                General.FSelectedUnit = unit;
                ShowUnit();
            }
        }
    }
}

public static partial class AoWUnitWindowUnit
{
    public static TUnitWindow UnitWindow;
    public static TAOWCheckBox SelectedItem;
    public static readonly string AbilitiesRStr = "Abilities";
    public static readonly string InfoRStr = "Info";
    public static readonly string NoneRStr = "None";
    public static readonly string CreatureRStr = "Creature";
    public static readonly string MachineRStr = "Machine";
    public static readonly string UnitRStr = "Unit";
    public static readonly string LeaderRStr = "Leader";
    public static readonly string HeroRStr = "Hero";
    public static readonly string NoLeaderRStr = "Hero has no Leader";
    public static readonly string LeaderSpheresRStr = "Leaders magic spheres";
    public static readonly string ItemsRStr = "Items";
    public static readonly string LevelRStr = "Level";
    public static readonly string ExpRStr = "Exp";

    // AoWUnitWindow.pas:220
    public static int CompareUnitAbilities(string S1, string S2, TObject O1, TObject O2)
    {
        TAbility A1;
        TAbility A2;
        int V1;
        int V2;
        A1 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(O1));
        A2 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(O2));
        if (A1.GetEnabled(General.FSelectedUnit) && A1.GetControlType(General.FSelectedUnit).Contains(TAbilityControlTypeElement.acAction))
        {
            V1 = 10;
        }
        else
        {
            V1 = 0;
        }

        if (A2.GetEnabled(General.FSelectedUnit) && A2.GetControlType(General.FSelectedUnit).Contains(TAbilityControlTypeElement.acAction))
        {
            V2 = 10;
        }
        else
        {
            V2 = 0;
        }

        int Result = V2 - V1;
        if (Result == 0)
        {
            Result = CompareStr(S1, S2);
        }

        return Result;
    }
}
