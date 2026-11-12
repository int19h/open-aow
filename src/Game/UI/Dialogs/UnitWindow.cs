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
    private int _highlight;
    private ItemType _itemType;
    private int _highlightStep;
    private bool _itemVisible;
    private int _blinkDelay;
    private LibraryImage _cursorImage;
    private int _groundOffset;
    private int _groundCount;
    private UnitShowInfo _showInfo;

    // AoWUnitWindow.pas:985
    public void UpdateGround(int offset)
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
        U = DialogHosts.General.SelectedUnit;
        if (U != null)
        {
            if (U is Hero)
            {
                if (((Hero)U).GetItemsOnGround() != null)
                {
                    _groundCount = ((Hero)U).GetItemsOnGround().Count;
                    _groundOffset = offset;
                    // { Original last-page clamp leaves only four items visible. }
                    if (_groundCount < offset + 5)
                    {
                        _groundOffset = _groundCount - 4;
                    }

                    if (_groundOffset < 0)
                    {
                        _groundOffset = 0;
                    }

                    GroundL.Visible = _groundOffset > 0;
                    GroundR.Visible = _groundOffset + 5 <= _groundCount;
                    var count = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset < count)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset);
                        Gnd1.Tag = item.ItemId;
                    }

                    var count2 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 1 < count2)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 1);
                        Gnd2.Tag = item.ItemId;
                    }

                    var count3 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 2 < count3)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 2);
                        Gnd3.Tag = item.ItemId;
                    }

                    var count4 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 3 < count4)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 3);
                        Gnd4.Tag = item.ItemId;
                    }

                    var count5 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 4 < count5)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 4);
                        Gnd5.Tag = item.ItemId;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:472
    public void PreDrawAbilityItem(UIComponent sender, in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        bool enabled;
        Ability A;
        var objectValue = T1AbilityLB.Items.Objects[drawItem];
        A = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
        if (AoWHSMap.Current.CombatMap == null)
        {
            enabled = A != null && DialogHosts.General.SelectedUnit != null && DialogHosts.General.SelectedUnit.Player == AoWHSMap.Current.SeatedPlayer && A.GetControlType(DialogHosts.General.SelectedUnit).Contains(AbilityControlTypeElement.Action);
        }
        else
        {
            bool condition = A != null && DialogHosts.General.CombatUnit != null;
            if (condition)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[drawItem];
                condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetControlType(DialogHosts.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && DialogHosts.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && DialogHosts.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                var objectValue2 = T1AbilityLB.Items.Objects[drawItem];
                var combatMode = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(DialogHosts.General.CombatUnit.Unit.Troop);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[drawItem]) == 52) || DialogHosts.General.CombatUnit.Unit.Troop.GetMoves() == DialogHosts.General.CombatUnit.Unit.MovePoints;
            }

            enabled = test;
        }

        if (enabled)
        {
            sender.TextColor = 14679807;
        }
        else
        {
            sender.TextColor = 9415072;
        }
    }

    // AoWUnitWindow.pas:502
    public void HelpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWUnitWindow.pas:507
    public void T1AbilityLBChange(ListBox sender)
    {
        Ability A;
        T1AbilityLB.Selected = sender.Selected;
        T1TypeLB.Selected = sender.Selected;
        T1SrcLB.Selected = sender.Selected;
        if (AoWHSMap.Current.CombatMap == null)
        {
            if (DialogHosts.General.SelectedUnit.Player == AoWHSMap.Current.SeatedPlayer && T1AbilityLB.Selected > -1)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                A = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
                if (A != null && A.GetEnabled(DialogHosts.General.SelectedUnit))
                {
                    if (AoWHSMap.Current.CombatMap == null)
                    {
                        var controlType = A.GetControlType(DialogHosts.General.SelectedUnit);
                        T1UseBut.Visible = controlType.Contains(AbilityControlTypeElement.Action);
                    }
                    else
                    {
                        var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                        bool condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(DialogHosts.General.SelectedUnit) != CombatMode.None;
                        if (condition)
                        {
                            var objectValue3 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                            condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue3)).GetCombatMode(DialogHosts.General.SelectedUnit) != CombatMode.Melee;
                        }

                        T1UseBut.Visible = condition;
                    }

                    if (T1UseBut.Visible)
                    {
                        var name = A.GetName(DialogHosts.General.SelectedUnit);
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_useFormat), [name]);
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
            bool condition = T1AbilityLB.Selected > -1 && DialogHosts.General.CombatUnit != null;
            if (condition)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetControlType(DialogHosts.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && DialogHosts.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && DialogHosts.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var combatMode = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetCombatMode(DialogHosts.General.CombatUnit.Unit.Troop);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(T1AbilityLB.Items.Objects[T1AbilityLB.Selected]) == 52) || DialogHosts.General.CombatUnit.Unit.Troop.GetMoves() == DialogHosts.General.CombatUnit.Unit.MovePoints;
            }

            if (test)
            {
                T1UseBut.Visible = true;
                var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                var name = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetName(DialogHosts.General.CombatUnit.Unit.Troop);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_useFormat), [name]);
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
    public void ItemRHMouseDown(CheckBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        CheckBox check;
        check = sender;
        ItemBanner.ItemId = check.Tag;
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
        _itemType = (ItemType)6;
    }

    // AoWUnitWindow.pas:591
    public void UnitWinUpdate()
    {
        if (UnitWin.Visible)
        {
            _highlight += _highlightStep;
            if (_highlight > 100 || _highlight < 50)
            {
                _highlightStep = -_highlightStep;
                _highlight += _highlightStep;
            }

            switch (_itemType)
            {
                case ItemType.Attack:
                    Attack.Drawn = false;
                    break;
                case ItemType.Defense:
                    Defend.Drawn = false;
                    break;
                case ItemType.Head:
                    Head.Drawn = false;
                    break;
                case ItemType.Torso:
                    Torso.Drawn = false;
                    break;
                case ItemType.Ring:
                    RingL.Drawn = false;
                    RingR.Drawn = false;
                    break;
            }

            if (s_selectedItem != null)
            {
                _blinkDelay -= 1;
                if (_blinkDelay < 0)
                {
                    _blinkDelay = 2;
                    _itemVisible = !_itemVisible;
                }
            }
        }
    }

    // AoWUnitWindow.pas:620
    public void T1SortNameClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T1AbilityLB);
        lists.Add(T1TypeLB);
        lists.Add(T1SrcLB);
        DialogHosts.General.SortLists(ref lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:631
    public void T1SortTypeClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T1TypeLB);
        lists.Add(T1AbilityLB);
        lists.Add(T1SrcLB);
        DialogHosts.General.SortLists(ref lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:642
    public void T1SortSourceClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(T1SrcLB);
        lists.Add(T1AbilityLB);
        lists.Add(T1TypeLB);
        DialogHosts.General.SortLists(ref lists, 0);
        T1UseBut.Visible = false;
    }

    // AoWUnitWindow.pas:653
    public void UseAbility()
    {
        Ability A;
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
        A = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
        if (A != null && A.GetEnabled(DialogHosts.General.SelectedUnit))
        {
            if (AoWHSMap.Current.CombatMap == null)
            {
                if (DialogHosts.General.SelectedUnit.Player == AoWHSMap.Current.SeatedPlayer)
                {
                    if (A.Activate(DialogHosts.General.SelectedUnit, ref reason))
                    {
                        UnitWin.Visible = false;
                    }
                    else
                    {
                        if (!IsEmpty(reason))
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }
                    }
                }
            }
            else
            {
                if (DialogHosts.General.CombatUnit != null && DialogHosts.General.CombatUnit.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                {
                    var objectValue2 = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
                    if (DialogHosts.General.CombatUnit.SelectAbility(ObjectToInt(objectValue2)) > -1)
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
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (s_selectedItem != null && DialogHosts.General.SelectedUnit is Hero)
        {
            if (((Hero)DialogHosts.General.SelectedUnit).CanUseItem(s_selectedItem.Tag, ref reason))
            {
                ((Hero)DialogHosts.General.SelectedUnit).UseItem(s_selectedItem.Tag);
            }
            else
            {
                AoWHSMap.Current.ShowWarning(reason);
            }
        }
    }

    // AoWUnitWindow.pas:688
    public void UnitTabChange()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWUnitWindow.pas:693
    public void UnitWinMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right && s_selectedItem != null)
        {
            Gnd1Change(s_selectedItem);
        }
    }

    // AoWUnitWindow.pas:714
    public void GroundRClick()
    {
        UpdateGround(_groundOffset + 1);
    }

    // AoWUnitWindow.pas:719
    public void GroundLClick()
    {
        UpdateGround(_groundOffset - 1);
    }

    // AoWUnitWindow.pas:724
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        UnitWin.Visible = false;
    }

    // AoWUnitWindow.pas:730
    public void Gnd1Draw(UIComponent sender, LockedSurface lockedSurface)
    {
        Hero U;
        Item item;
        UIComponent component;
        component = sender;
        if (component.Tag != -1)
        {
            U = (Hero)DialogHosts.General.SelectedUnit;
            item = U.Inventory.FindItem(component.Tag);
            if (item == null)
            {
                item = U.HeroItems.FindItem(component.Tag);
            }

            if (item == null)
            {
                if (U.GetItemsOnGround() != null)
                {
                    item = U.GetItemsOnGround().FindItem(component.Tag);
                }
            }

            if (item != null)
            {
                var checkBox = component;
                if (item.GetImage() != null)
                {
                    item.GetImage().ShowClipped(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, lockedSurface);
                }
            }
        }
    }

    // AoWUnitWindow.pas:750
    public void FrLShPreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect R;
        if (DialogHosts.General.SelectedUnit != null)
        {
            if (DialogHosts.General.SelectedUnit.GetFace() != null)
            {
                var image = sender;
                R = new Rect(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);

                DialogHosts.General.SelectedUnit.ShowFace(R, lockedSurface);
            }
        }
    }

    // AoWUnitWindow.pas:763
    public void FrRShPreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        if (DialogHosts.General.SelectedUnit != null)
        {
            _showInfo.Direction = 3;
            if (DialogHosts.General.SelectedUnit is Hero)
            {
                _showInfo.Overlays = Set<UnitOverlayModesElement>.Of();
            }
            else
            {
                _showInfo.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
            }

            var image = sender;
            DialogHosts.General.SelectedUnit.Show(image.WinLeft + 49, image.WinTop + 78, lockedSurface, _showInfo);
        }
    }

    // AoWUnitWindow.pas:855
    public void Gnd3MouseDown(CheckBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (DialogHosts.General.SelectedUnit != null)
        {
            if (!AoWHSMap.Current.Players.GetPlayers(DialogHosts.General.SelectedUnit.Player).GetBusy())
            {
                if (button == MouseButton.Right)
                {
                    if (sender.Tag == -1)
                    {
                        sender.PopupMenu = null;
                    }
                    else
                    {
                        sender.PopupMenu = DialogHosts.ItemBanner.IBannerPopup;
                    }

                    ItemBanner.ItemId = sender.Tag;
                }
                else
                {
                    if (sender == s_selectedItem)
                    {
                        s_selectedItem.Checked = false;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:871
    public void HeadDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == 0)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:881
    public void TorsoDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)1)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:891
    public void AttackDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)2)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:901
    public void DefendDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)3)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:911
    public void RingLDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)4)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWin[24].ShowBlended(T3Panel.WinLeft + checkBox.WinLeft, T3Panel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:921
    public void DisbandBtnClick()
    {
        var disbandUnit = AbilityRules.TranslateResourceString(s_disbandUnitText);
        var confirm = AbilityRules.TranslateResourceString(LobbyText.ConfirmText);
        DialogHosts.ConfirmationDialog.Confirm(confirm, disbandUnit, 7);
    }

    // AoWUnitWindow.pas:926
    public void T1AbilityLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability A;
        if (button == MouseButton.Right && DialogHosts.General.SelectedUnit != null && T1AbilityLB.Selected != -1)
        {
            var objectValue = T1AbilityLB.Items.Objects[T1AbilityLB.Selected];
            A = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, A.Description);
            }
        }
    }

    // AoWUnitWindow.pas:937
    public void UnitWinKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
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
        if (DialogHosts.General.UnitList != null)
        {
            I = DialogHosts.General.IndexOfUnit(DialogHosts.General.SelectedUnit);
            if (I > -1 && DialogHosts.General.SelectedUnits[0] > -1)
            {
                I -= 1;
                if (I < 0)
                {
                    I = 7;
                    while (DialogHosts.General.SelectedUnits[I] == -1)
                    {
                        I -= 1;
                    }
                }

                var troop = DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[I]);
                DialogHosts.General.SelectedUnit = troop;
                ShowUnit();
            }
        }
    }

    // AoWUnitWindow.pas:966
    public void PNextClick()
    {
        int I;
        DialogHosts.General.PlayButtonSound(0);
        if (DialogHosts.General.UnitList != null)
        {
            I = DialogHosts.General.IndexOfUnit(DialogHosts.General.SelectedUnit);
            if (I > -1)
            {
                I += 1;
                if (I > 7)
                {
                    I = 0;
                }

                if (DialogHosts.General.SelectedUnits[I] == -1)
                {
                    I = 0;
                }

                var troop = DialogHosts.General.UnitList.GetUnit(DialogHosts.General.SelectedUnits[I]);
                DialogHosts.General.SelectedUnit = troop;
                ShowUnit();
            }
        }
    }

    private static readonly string s_moraleFormat = "Morale: %s";
    private static readonly string s_useFormat = "Use %s";
    private static StringArray5 s_moraleTexts = StringArray5.From(["Terrible", "Poor", "Okay", "Good", "High"]);
    private static CheckBox s_selectedItem;
    private static readonly string s_abilitiesText = "Abilities";
    private static readonly string s_infoText = "Info";
    private static readonly string s_noneText = "None";
    private static readonly string s_creatureText = "Creature";
    private static readonly string s_machineText = "Machine";
    private static readonly string s_unitText = "Unit";
    private static readonly string s_leaderText = "Leader";
    private static readonly string s_heroText = "Hero";
    private static readonly string s_noLeaderText = "Hero has no Leader";
    private static readonly string s_leaderSpheresText = "Leaders magic spheres";
    private static readonly string s_itemsText = "Items";
    private static readonly string s_levelText = "Level";
    private static readonly string s_experienceText = "Exp";
    private static readonly string s_disbandUnitText = "Disband this unit?";

    private static int CompareUnitAbilities(string s1, string s2, object o1, object o2)
    {
        Ability ability = AoWHSSet.Current.AbilityControl.GetAbility(RtlUnit.ObjectToInt(o1));
        Ability ability2 = AoWHSSet.Current.AbilityControl.GetAbility(RtlUnit.ObjectToInt(o2));
        int number = ((ability.GetEnabled(DialogHosts.General.SelectedUnit) && ability.GetControlType(DialogHosts.General.SelectedUnit).Contains(AbilityControlTypeElement.Action)) ? 10 : 0);
        int number2 = ((ability2.GetEnabled(DialogHosts.General.SelectedUnit) && ability2.GetControlType(DialogHosts.General.SelectedUnit).Contains(AbilityControlTypeElement.Action)) ? 10 : 0);
        int number3 = number2 - number;
        if (number3 == 0)
        {
            number3 = RtlUnit.CompareStr(s1, s2);
        }
        return number3;
    }
}
