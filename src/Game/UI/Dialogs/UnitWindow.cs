// Translated from AoWUnitWindow.pas.
using Aow.Engine.Buffers;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// highlight. dragged item type. highlight step. blink state. blink delay. cursor image. ground
// offset/count. show info
public partial class UnitWindow : DataModule
{
    public UnitWindow(Component owner) : base(owner)
    { }

    public Window UnitWin;
    public Image UnitFrameLeft;
    public TabPanel UnitTab;
    public Button Close;
    public Label Title;
    public Button Help;
    public Image TitleIcon;
    public Button BCloseBtn;
    public Image CeltLineR;
    public Memo T2Memo;
    public Image ManaIcon;
    public Label Mana;
    public ListBox T1AbilityLB;
    public Button T1UseBut;
    public Button T1SortName;
    public Button T1SortType;
    public Button T1SortSource;
    public Label T1UseMsg;
    public Panel T1Panel;
    public Panel T2Panel;
    public ListBox T1TypeLB;
    public ListBox T1SrcLB;
    public Panel T1ListPnl;
    public VerticalScrollBar T1SBar;
    public VerticalScrollBar T2SBar;
    public Panel T2Text;
    public Panel T2Leader;
    public Button Sel4;
    public Panel SelPnl;
    public Button Sel3;
    public Button Sel2;
    public Button Sel1;
    public Button Sel5;
    public Button Sel6;
    public Button Sel7;
    public Image AAlignment;
    public Label AAlignmentLbl;
    public Panel T2Alignnl;
    public Frame AlFr;
    public Image TypeIcn;
    public Label TypeV;
    public Panel T2TypePnl;
    public Frame TypeFr;
    public Frame LdrFr;
    public Label SphereLbl;
    public Panel T3Panel;
    public CheckBox BP1;
    public Label BPLbl1;
    public CheckBox BP2;
    public CheckBox BP3;
    public CheckBox BP4;
    public CheckBox BP5;
    public CheckBox BP6;
    public CheckBox BP7;
    public CheckBox BP8;
    public Label BPLbl2;
    public Label BodyLbl;
    public CheckBox Head;
    public CheckBox Torso;
    public CheckBox Attack;
    public CheckBox Defend;
    public CheckBox RingL;
    public CheckBox RingR;
    public CheckBox Gnd3;
    public CheckBox Gnd2;
    public CheckBox Gnd1;
    public CheckBox Gnd4;
    public CheckBox Gnd5;
    public Label GndLbl;
    public Image SlotBack;
    public Button DisbandBtn;
    public Panel ItemUsePnl;
    public Button ItemUseBtn;
    public Image ItemPic;
    public Panel UnitMainPnl;
    public Label UnitName;
    public Image UnitFrRight;
    public Image UAttackIcon;
    public Image UDefenseIcon;
    public Image UMovesIcon;
    public Image UDamageIcon;
    public Image UResistanceIcon;
    public Image UHitsIcon;
    public Label UAttack;
    public Label UDefense;
    public Label UMoves;
    public Label UDamage;
    public Label UResistance;
    public Label UHits;
    public Panel UStatPnl;
    public Panel UStatP1;
    public Panel UStatP2;
    public Panel UnitFr;
    public Label Level;
    public Image UnitLine1;
    public Image UnitLine2;
    public Image FrLSh;
    public Image FrRSh;
    public Panel RightFramePnl;
    public Label MoraleLbl;
    public Image UpkeepIcon;
    public Label UpKeep;
    public Frame BCeltL;
    public Image CeltLineL;
    public Button GroundL;
    public Button GroundR;
    public Image RelationIcn;
    public Label RelationV;
    public Panel T2RelPnl;
    public Frame RelFr;
    public Image RelRing;
    public Label RelationLbl;
    public Label AlignLbl;
    public Label TypeLbl;
    public Label XP;
    public Button PNext;
    public Button PPrev;
    public IvTranslator Translator;
    public int FHighlight;
    public ItemType FItemType;
    public int FHighlightStep;
    public bool FItemVisible;
    public int FBlinkDelay;
    public LibraryImage FCursorImage;
    public int FGroundOffset;
    public int FGroundCount;
    public UnitShowInfo FShowInfo;

    // AoWUnitWindow.pas:985
    public void UpdateGround(int Offset)
    {
        Troop U;
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
        U = DialogHosts.General.FSelectedUnit;
        if (U != null)
        {
            if (U is Hero)
            {
                if (((Hero)U).GetItemsOnGround() != null)
                {
                    FGroundCount = ((Hero)U).GetItemsOnGround().Count;
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
                    var count = ((Hero)U).GetItemsOnGround().Count;
                    if (FGroundOffset < count)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(FGroundOffset);
                        Gnd1.Tag = item.FItemID;
                    }

                    var count2 = ((Hero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 1 < count2)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(FGroundOffset + 1);
                        Gnd2.Tag = item.FItemID;
                    }

                    var count3 = ((Hero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 2 < count3)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(FGroundOffset + 2);
                        Gnd3.Tag = item.FItemID;
                    }

                    var count4 = ((Hero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 3 < count4)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(FGroundOffset + 3);
                        Gnd4.Tag = item.FItemID;
                    }

                    var count5 = ((Hero)U).GetItemsOnGround().Count;
                    if (FGroundOffset + 4 < count5)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(FGroundOffset + 4);
                        Gnd5.Tag = item.FItemID;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:472
    public void PreDrawAbilityItem(UIComponent Sender, in Rect DrawRect, int DrawItem, ref LockedSurface SurfDesc)
    {
        bool Enabled;
        Ability A;
        var objectValue = T1AbilityLB.Items.Objects[DrawItem];
        A = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
        if (AoWHSMap.Current.FCombatMap == null)
        {
            Enabled = A != null && DialogHosts.General.FSelectedUnit != null && DialogHosts.General.FSelectedUnit.FPlayer == AoWHSMap.Current.FSeatedPlayer && A.GetControlType(DialogHosts.General.FSelectedUnit).Contains(AbilityControlTypeElement.Action);
        }
        else
        {
            bool condition = A != null && DialogHosts.General.FCombatUnit != null;
            if (condition)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[DrawItem];
                condition = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetControlType(DialogHosts.General.FCombatUnit.FUnit.FUnit).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == CombatMap.Current.FSeatedPlayer && DialogHosts.General.FCombatUnit.FUnit.FMovePoints > 0;
            if (test)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[DrawItem];
                var combatMode = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[DrawItem]) == 52) || DialogHosts.General.FCombatUnit.FUnit.FUnit.GetMoves() == DialogHosts.General.FCombatUnit.FUnit.FMovePoints;
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
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWUnitWindow.pas:507
    public void T1AbilityLBChange(ListBox Sender)
    {
        Ability A;
        T1AbilityLB.Selected = Sender.Selected;
        T1TypeLB.Selected = Sender.Selected;
        T1SrcLB.Selected = Sender.Selected;
        if (AoWHSMap.Current.FCombatMap == null)
        {
            if (DialogHosts.General.FSelectedUnit.FPlayer == AoWHSMap.Current.FSeatedPlayer && T1AbilityLB.Selected > -1)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                A = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
                if (A != null && A.GetEnabled(DialogHosts.General.FSelectedUnit))
                {
                    if (AoWHSMap.Current.FCombatMap == null)
                    {
                        var controlType = A.GetControlType(DialogHosts.General.FSelectedUnit);
                        T1UseBut.Visible = controlType.Contains(AbilityControlTypeElement.Action);
                    }
                    else
                    {
                        var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        bool condition = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(DialogHosts.General.FSelectedUnit) != CombatMode.None;
                        if (condition)
                        {
                            var objectValue3 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                            condition = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue3)).GetCombatMode(DialogHosts.General.FSelectedUnit) != CombatMode.Melee;
                        }

                        T1UseBut.Visible = condition;
                    }

                    if (T1UseBut.Visible)
                    {
                        var name = A.GetName(DialogHosts.General.FSelectedUnit);
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(UseXRStr), [name]);
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
            bool condition = T1AbilityLB.Selected > -1 && DialogHosts.General.FCombatUnit != null;
            if (condition)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                condition = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetControlType(DialogHosts.General.FCombatUnit.FUnit.FUnit).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && DialogHosts.General.FCombatUnit.FUnit.GetPlayer() == CombatMap.Current.FSeatedPlayer && DialogHosts.General.FCombatUnit.FUnit.FMovePoints > 0;
            if (test)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var combatMode = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetCombatMode(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[T1AbilityLB.Selected]) == 52) || DialogHosts.General.FCombatUnit.FUnit.FUnit.GetMoves() == DialogHosts.General.FCombatUnit.FUnit.FMovePoints;
            }

            if (test)
            {
                T1UseBut.Visible = true;
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var name = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue)).GetName(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(UseXRStr), [name]);
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
    public void ItemRHMouseDown(CheckBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        CheckBox Check;
        Check = Sender;
        ItemBanner.ItemID = Check.Tag;
    }

    // AoWUnitWindow.pas:575
    public void UnitWinResize()
    {
        this.UnitWin.Alignment.LeftOffset = this.UnitWin.WinLeft;
        this.UnitWin.Alignment.TopOffset = this.UnitWin.WinTop;
        this.UnitWin.Alignment.AlignHeight = AlignHeight.Top;
        this.UnitWin.Alignment.AlignWidth = AlignWidth.Left;
    }

    // AoWUnitWindow.pas:586
    public void UnitWindowCreate()
    {
        FItemType = (ItemType)6;
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
                case ItemType.Attack:
                    Attack.FDrawn = false;
                    break;
                case ItemType.Defense:
                    Defend.FDrawn = false;
                    break;
                case ItemType.Head:
                    Head.FDrawn = false;
                    break;
                case ItemType.Torso:
                    Torso.FDrawn = false;
                    break;
                case ItemType.Ring:
                    RingL.FDrawn = false;
                    RingR.FDrawn = false;
                    break;
            }

            if (SelectedItem != null)
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
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T1AbilityLB);
        Lists.Add(T1TypeLB);
        Lists.Add(T1SrcLB);
        DialogHosts.General.SortLists(ref Lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:631
    public void T1SortTypeClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T1TypeLB);
        Lists.Add(T1AbilityLB);
        Lists.Add(T1SrcLB);
        DialogHosts.General.SortLists(ref Lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:642
    public void T1SortSourceClick(int Button)
    {
        DelphiList Lists;
        DialogHosts.General.PlayButtonSound(0);
        Lists = new DelphiList();
        Lists.Add(T1SrcLB);
        Lists.Add(T1AbilityLB);
        Lists.Add(T1TypeLB);
        DialogHosts.General.SortLists(ref Lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:653
    public void UseAbility()
    {
        Ability A;
        string Reason = "";
        DialogHosts.General.PlayButtonSound(0);
        var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
        A = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
        if (A != null && A.GetEnabled(DialogHosts.General.FSelectedUnit))
        {
            if (AoWHSMap.Current.FCombatMap == null)
            {
                if (DialogHosts.General.FSelectedUnit.FPlayer == AoWHSMap.Current.FSeatedPlayer)
                {
                    if (A.Activate(DialogHosts.General.FSelectedUnit, ref Reason))
                    {
                        UnitWin.Visible = false;
                    }
                    else
                    {
                        if (!IsEmpty(Reason))
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }
                    }
                }
            }
            else
            {
                if (DialogHosts.General.FCombatUnit != null && DialogHosts.General.FCombatUnit.FUnit.FCombatPlayer == CombatMap.Current.FSeatedCombatPlayer)
                {
                    var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    if (DialogHosts.General.FCombatUnit.SelectAbility(ObjectToInt(objectValue2)) > -1)
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
        DialogHosts.General.PlayButtonSound(0);
        if (SelectedItem != null && DialogHosts.General.FSelectedUnit is Hero)
        {
            if (((Hero)DialogHosts.General.FSelectedUnit).CanUseItem(SelectedItem.Tag, ref Reason))
            {
                ((Hero)DialogHosts.General.FSelectedUnit).UseItem(SelectedItem.Tag);
            }
            else
            {
                AoWHSMap.Current.ShowWarning(Reason);
            }
        }
    }

    // AoWUnitWindow.pas:688
    public void UnitTabChange()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWUnitWindow.pas:693
    public void UnitWinMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Button == MouseButton.Right && SelectedItem != null)
        {
            Gnd1Change(SelectedItem);
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
        DialogHosts.General.PlayButtonSound(0);
        UnitWin.Visible = false;
    }

    // AoWUnitWindow.pas:730
    public void Gnd1Draw(UIComponent Sender, LockedSurface SurfDesc)
    {
        Hero U;
        Item Item;
        UIComponent Component;
        Component = Sender;
        if (Component.Tag != -1)
        {
            U = (Hero)DialogHosts.General.FSelectedUnit;
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
    public void FrLShPreDraw(UIComponent Sender, LockedSurface SurfDesc)
    {
        Rect R;
        if (DialogHosts.General.FSelectedUnit != null)
        {
            if (DialogHosts.General.FSelectedUnit.GetFace() != null)
            {
                var image = Sender;
                R = new Rect(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);

                DialogHosts.General.FSelectedUnit.ShowFace(R, SurfDesc);
            }
        }
    }

    // AoWUnitWindow.pas:763
    public void FrRShPreDraw(UIComponent Sender, LockedSurface SurfDesc)
    {
        if (DialogHosts.General.FSelectedUnit != null)
        {
            FShowInfo.Direction = 3;
            if (DialogHosts.General.FSelectedUnit is Hero)
            {
                FShowInfo.Overlays = Set<UnitOverlayModesElement>.Of();
            }
            else
            {
                FShowInfo.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
            }

            var image = Sender;
            DialogHosts.General.FSelectedUnit.ShowEx(image.WinLeft + 49, image.WinTop + 78, SurfDesc, FShowInfo);
        }
    }

    // AoWUnitWindow.pas:855
    public void Gnd3MouseDown(CheckBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (DialogHosts.General.FSelectedUnit != null)
        {
            if (!AoWHSMap.Current.FPlayers.GetPlayers(DialogHosts.General.FSelectedUnit.FPlayer).GetBusy())
            {
                if (Button == MouseButton.Right)
                {
                    if (Sender.Tag == -1)
                    {
                        Sender.PopupMenu = null;
                    }
                    else
                    {
                        Sender.PopupMenu = DialogHosts.ItemBanner.IBannerPopup;
                    }

                    ItemBanner.ItemID = Sender.Tag;
                }
                else
                {
                    if (Sender == SelectedItem)
                    {
                        SelectedItem.Checked = false;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:871
    public void HeadDraw(UIComponent Sender, LockedSurface SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (SelectedItem != null && Sender != SelectedItem && FItemType == 0)
        {
            var checkBox = Sender;
            DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, GraphicsState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:881
    public void TorsoDraw(UIComponent Sender, LockedSurface SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (SelectedItem != null && Sender != SelectedItem && FItemType == (ItemType)1)
        {
            var checkBox = Sender;
            DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, GraphicsState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:891
    public void AttackDraw(UIComponent Sender, LockedSurface SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (SelectedItem != null && Sender != SelectedItem && FItemType == (ItemType)2)
        {
            var checkBox = Sender;
            DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, GraphicsState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:901
    public void DefendDraw(UIComponent Sender, LockedSurface SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (SelectedItem != null && Sender != SelectedItem && FItemType == (ItemType)3)
        {
            var checkBox = Sender;
            DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, GraphicsState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:911
    public void RingLDraw(UIComponent Sender, LockedSurface SurfDesc)
    {
        Gnd1Draw(Sender, SurfDesc);
        if (SelectedItem != null && Sender != SelectedItem && FItemType == (ItemType)4)
        {
            var checkBox = Sender;
            DialogHosts.IntGfxMod.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, SurfDesc, GraphicsState.IntensityBlendTable.LookupTables[FHighlight]);
        }
    }

    // AoWUnitWindow.pas:921
    public void DisbandBtnClick()
    {
        var disbandUnit = AbilityRules.TranslateRStr(DisbandUnitRStr);
        var confirm = AbilityRules.TranslateRStr(LobbyText.ConfirmRStr);
        DialogHosts.DialogWin.Confirm(confirm, disbandUnit, 7);
    }

    // AoWUnitWindow.pas:926
    public void T1AbilityLBMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        Ability A;
        if (Button == MouseButton.Right && DialogHosts.General.FSelectedUnit != null && T1AbilityLB.Selected != -1)
        {
            var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
            A = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, A.FDescription);
            }
        }
    }

    // AoWUnitWindow.pas:937
    public void UnitWinKeyDown(ref ushort Key, Set<ShiftState> Shift)
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
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.FUnitList != null)
        {
            I = DialogHosts.General.IndexOfUnit(DialogHosts.General.FSelectedUnit);
            if (I > -1 && DialogHosts.General.FSelectedUnits[0] > -1)
            {
                I -= 1;
                if (I < 0)
                {
                    I = 7;
                    while (DialogHosts.General.FSelectedUnits[I] == -1)
                    {
                        I -= 1;
                    }
                }

                var unit = DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[I]);
                DialogHosts.General.FSelectedUnit = unit;
                ShowUnit();
            }
        }
    }

    // AoWUnitWindow.pas:966
    public void PNextClick()
    {
        int I;
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.FUnitList != null)
        {
            I = DialogHosts.General.IndexOfUnit(DialogHosts.General.FSelectedUnit);
            if (I > -1)
            {
                I += 1;
                if (I > 7)
                {
                    I = 0;
                }

                if (DialogHosts.General.FSelectedUnits[I] == -1)
                {
                    I = 0;
                }

                var unit = DialogHosts.General.FUnitList.GetUnit(DialogHosts.General.FSelectedUnits[I]);
                DialogHosts.General.FSelectedUnit = unit;
                ShowUnit();
            }
        }
    }

    private static readonly string MoraleXRStr = "Morale: %s";
    private static readonly string UseXRStr = "Use %s";
    private static StringArray5 TMoraleRStr = StringArray5.From(["Terrible", "Poor", "Okay", "Good", "High"]);
    private static CheckBox SelectedItem;
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
        Ability ability = AoWHSSet.Current.FAbilityControl.GetAbility(RtlUnit.ObjectToInt(O1));
        Ability ability2 = AoWHSSet.Current.FAbilityControl.GetAbility(RtlUnit.ObjectToInt(O2));
        int num = ((ability.GetEnabled(DialogHosts.General.FSelectedUnit) && ability.GetControlType(DialogHosts.General.FSelectedUnit).Contains(AbilityControlTypeElement.Action)) ? 10 : 0);
        int num2 = ((ability2.GetEnabled(DialogHosts.General.FSelectedUnit) && ability2.GetControlType(DialogHosts.General.FSelectedUnit).Contains(AbilityControlTypeElement.Action)) ? 10 : 0);
        int num3 = num2 - num;
        if (num3 == 0)
        {
            num3 = RtlUnit.CompareStr(S1, S2);
        }
        return num3;
    }
}
