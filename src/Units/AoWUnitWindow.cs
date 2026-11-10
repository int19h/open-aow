// Translated from AoWUnitWindow.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWUnitWindow.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
    public Aow.Game.Draw.Images.TLibraryImage FCursorImage;
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
        U = Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit;
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
        A = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
        {
            Enabled = A != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer && A.GetControlType(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit).Contains(TAbilityControlTypeElement.acAction);
        }
        else
        {
            bool condition = A != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null;
            if (condition)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[DrawItem];
                condition = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetControlType(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit).Contains(TAbilityControlTypeElement.acCombatAction);
            }

            bool test = condition && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints > 0;
            if (test)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[DrawItem];
                var combatMode = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit);
                test = !(TSet<TCombatMode>.Of(TCombatMode.cmShortRange, TCombatMode.cmMediumRange, TCombatMode.cmLongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[DrawItem]) == 52) || Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.GetMoves() == Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints;
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
    public void HelpClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWUnitWindow.pas:507
    public void T1AbilityLBChange(TAOWListBox Sender)
    {
        TAbility A;
        T1AbilityLB.Selected = Sender.Selected;
        T1TypeLB.Selected = Sender.Selected;
        T1SrcLB.Selected = Sender.Selected;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer && T1AbilityLB.Selected > -1)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                A = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
                if (A != null && A.GetEnabled(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit))
                {
                    if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
                    {
                        var controlType = A.GetControlType(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit);
                        T1UseBut.Visible = controlType.Contains(TAbilityControlTypeElement.acAction);
                    }
                    else
                    {
                        var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        bool condition = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit) != TCombatMode.cmNone;
                        if (condition)
                        {
                            var objectValue3 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                            condition = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue3)).GetCombatMode(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit) != TCombatMode.cmMelee;
                        }

                        T1UseBut.Visible = condition;
                    }

                    if (T1UseBut.Visible)
                    {
                        var name = A.GetName(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit);
                        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.UseXRStr), [name]);
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
            bool condition = T1AbilityLB.Selected > -1 && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null;
            if (condition)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                condition = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetControlType(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit).Contains(TAbilityControlTypeElement.acCombatAction);
            }

            bool test = condition && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedPlayer && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints > 0;
            if (test)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var combatMode = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetCombatMode(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit);
                test = !(TSet<TCombatMode>.Of(TCombatMode.cmShortRange, TCombatMode.cmMediumRange, TCombatMode.cmLongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[T1AbilityLB.Selected]) == 52) || Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit.GetMoves() == Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints;
            }

            if (test)
            {
                T1UseBut.Visible = true;
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var name = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetName(Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit);
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.UseXRStr), [name]);
                T1UseBut.Text = format;
            }
            else
            {
                T1UseBut.Visible = false;
            }
        }
    }

    // AoWUnitWindow.pas:562
    public void T1SbarChange()
    {
        T1AbilityLB.ListOffset = T1SBar.Position;
        T1TypeLB.ListOffset = T1SBar.Position;
    }

    // AoWUnitWindow.pas:568
    public void ItemRHMouseDown(TAOWCheckBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAOWCheckBox Check;
        Check = Sender;
        Aow.Game.Ui.Dialogs.TItemBanner.ItemID = Check.Tag;
    }

    // AoWUnitWindow.pas:575
    public void UnitWinResize()
    {
        this.UnitWin.Alignment.LeftOffset = this.UnitWin.WinLeft;
        this.UnitWin.Alignment.TopOffset = this.UnitWin.WinTop;
        this.UnitWin.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
        this.UnitWin.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
    }

    // AoWUnitWindow.pas:586
    public void UnitWindowCreate()
    {
        FItemType = (TItemType)6;
    }

    // AoWUnitWindow.pas:591
    public void UnitWinUpdate()
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

            if (Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null)
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
    public void T1SortNameClick(int Button)
    {
        TList Lists;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Lists = new TList();
        Lists.Add(T1AbilityLB);
        Lists.Add(T1TypeLB);
        Lists.Add(T1SrcLB);
        Aow.Game.Ui.Dialogs.DialogHosts.General.SortLists(ref Lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:631
    public void T1SortTypeClick(int Button)
    {
        TList Lists;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Lists = new TList();
        Lists.Add(T1TypeLB);
        Lists.Add(T1AbilityLB);
        Lists.Add(T1SrcLB);
        Aow.Game.Ui.Dialogs.DialogHosts.General.SortLists(ref Lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:642
    public void T1SortSourceClick(int Button)
    {
        TList Lists;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Lists = new TList();
        Lists.Add(T1SrcLB);
        Lists.Add(T1AbilityLB);
        Lists.Add(T1TypeLB);
        Aow.Game.Ui.Dialogs.DialogHosts.General.SortLists(ref Lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:653
    public void UseAbility()
    {
        TAbility A;
        string Reason = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
        A = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
        if (A != null && A.GetEnabled(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit))
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
            {
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
                {
                    if (A.Activate(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit, ref Reason))
                    {
                        UnitWin.Visible = false;
                    }
                    else
                    {
                        if (!IsEmpty(Reason))
                        {
                            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
                        }
                    }
                }
            }
            else
            {
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FCombatPlayer == Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedCombatPlayer)
                {
                    var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    if (Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.SelectAbility(ObjectToInt(objectValue2)) > -1)
                    {
                        UnitWin.Visible = false;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:676
    public void UseItemClick()
    {
        string Reason = "";
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit is THero)
        {
            if (((THero)Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit).CanUseItem(Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem.Tag, ref Reason))
            {
                ((THero)Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit).UseItem(Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem.Tag);
            }
            else
            {
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowWarning(Reason);
            }
        }
    }

    // AoWUnitWindow.pas:688
    public void UnitTabChange()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWUnitWindow.pas:693
    public void UnitWinMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Button == TMouseButton.mbRight && Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null)
        {
            Gnd1Change(Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem);
        }
    }

    // AoWUnitWindow.pas:714
    public void GroundRClick()
    {
        UpdateGround(FGroundOffset + 1);
    }

    // AoWUnitWindow.pas:719
    public void GroundLClick()
    {
        UpdateGround(FGroundOffset - 1);
    }

    // AoWUnitWindow.pas:724
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
            U = (THero)Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit;
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
                var checkBox = Component;
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
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit != null)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.GetFace() != null)
            {
                var image = Sender;
                R = Rect(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);

                Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.ShowFace(R, SurfDesc);
            }
        }
    }

    // AoWUnitWindow.pas:763
    public void FrRShPreDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit != null)
        {
            FShowInfo.Direction = 3;
            if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit is THero)
            {
                FShowInfo.Overlays = TSet<TUnitOverlayModesElement>.Of();
            }
            else
            {
                FShowInfo.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoRank);
            }

            var image = Sender;
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.ShowEx(image.WinLeft + 49, image.WinTop + 78, SurfDesc, FShowInfo);
        }
    }

    // AoWUnitWindow.pas:855
    public void Gnd3MouseDown(TAOWCheckBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit != null)
        {
            if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.FPlayer).GetBusy())
            {
                if (Button == TMouseButton.mbRight)
                {
                    if (Sender.Tag == -1)
                    {
                        Sender.PopupMenu = null;
                    }
                    else
                    {
                        Sender.PopupMenu = Aow.Game.Ui.Dialogs.DialogHosts.ItemBanner.IBannerPopup;
                    }

                    Aow.Game.Ui.Dialogs.TItemBanner.ItemID = Sender.Tag;
                }
                else
                {
                    if (Sender == Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem)
                    {
                        Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem.Checked = false;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:871
    public void HeadDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null && Sender != Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem && FItemType == 0)
        {
            var checkBox = Sender;
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:881
    public void TorsoDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null && Sender != Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem && FItemType == (TItemType)1)
        {
            var checkBox = Sender;
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:891
    public void AttackDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null && Sender != Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem && FItemType == (TItemType)2)
        {
            var checkBox = Sender;
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:901
    public void DefendDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null && Sender != Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem && FItemType == (TItemType)3)
        {
            var checkBox = Sender;
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:911
    public void RingLDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem != null && Sender != Aow.Game.Ui.Dialogs.TUnitWindow.SelectedItem && FItemType == (TItemType)4)
        {
            var checkBox = Sender;
            Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, Aow.Game.Draw.GfxState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:921
    public void DisbandBtnClick()
    {
        var disbandUnit = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TUnitWindow.DisbandUnitRStr);
        var confirm = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LobbyText.ConfirmRStr);
        Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.Confirm(confirm, disbandUnit, 7);
    }

    // AoWUnitWindow.pas:926
    public void T1AbilityLBMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility A;
        if (Button == TMouseButton.mbRight && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit != null && T1AbilityLB.Selected != -1)
        {
            var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
            A = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                Aow.Game.Ui.Dialogs.TInfoBanner.SetInfoBanner(Aow.Game.Ui.Dialogs.DialogHosts.InfoBanner, name, A.FDescription);
            }
        }
    }

    // AoWUnitWindow.pas:937
    public void UnitWinKeyDown(ref ushort Key, TSet<TShiftState> Shift)
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList != null)
        {
            I = Aow.Game.Ui.Dialogs.DialogHosts.General.IndexOfUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit);
            if (I > -1 && Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[0] > -1)
            {
                I -= 1;
                if (I < 0)
                {
                    I = 7;
                    while (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[I] == -1)
                    {
                        I -= 1;
                    }
                }

                var unit = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[I]);
                Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = unit;
                ShowUnit();
            }
        }
    }

    // AoWUnitWindow.pas:966
    public void PNextClick()
    {
        int I;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList != null)
        {
            I = Aow.Game.Ui.Dialogs.DialogHosts.General.IndexOfUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit);
            if (I > -1)
            {
                I += 1;
                if (I > 7)
                {
                    I = 0;
                }

                if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[I] == -1)
                {
                    I = 0;
                }

                var unit = Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList.GetUnit(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[I]);
                Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = unit;
                ShowUnit();
            }
        }
    }

    private static readonly string MoraleXRStr = "Morale: %s";
    private static readonly string UseXRStr = "Use %s";
    private static global::Aow.Game.Engine.Buffers.FixedArray_string_5 TMoraleRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_5.From(["Terrible", "Poor", "Okay", "Good", "High"]);
    private static global::Aow.Game.Ui.Controls.TAOWCheckBox SelectedItem;
    private static readonly string AbilitiesRStr = "Abilities";
    private static readonly string InfoRStr = "Info";
    private static readonly string NoneRStr = "None";
    private static readonly string CreatureRStr = "Creature";
    private static readonly string MachineRStr = "Machine";
    private static readonly string UnitRStr = "Unit";
    private static readonly string LeaderRStr = "Leader";
    private static readonly string HeroRStr = "Hero";
    private static readonly string NoLeaderRStr = "Hero has no Leader";
    private static readonly string LeaderSpheresRStr = "Leaders magic spheres";
    private static readonly string ItemsRStr = "Items";
    private static readonly string LevelRStr = "Level";
    private static readonly string ExpRStr = "Exp";
    private static readonly string DisbandUnitRStr = "Disband this unit?";

    private static int CompareUnitAbilities(string S1, string S2, object O1, object O2)
    {
        global::Aow.Game.Magic.Abilities.TAbility ability = global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(RtlUnit.ObjectToInt(O1));
        global::Aow.Game.Magic.Abilities.TAbility ability2 = global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(RtlUnit.ObjectToInt(O2));
        int num = ((ability.GetEnabled(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit) && ability.GetControlType(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit).Contains(global::Aow.Game.Magic.Abilities.TAbilityControlTypeElement.acAction)) ? 10 : 0);
        int num2 = ((ability2.GetEnabled(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit) && ability2.GetControlType(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit).Contains(global::Aow.Game.Magic.Abilities.TAbilityControlTypeElement.acAction)) ? 10 : 0);
        int num3 = num2 - num;
        if (num3 == 0)
        {
            num3 = RtlUnit.CompareStr(S1, S2);
        }
        return num3;
    }
}
