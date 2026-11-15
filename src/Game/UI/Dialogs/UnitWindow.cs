// Translated from AoWUnitWindow.pas.
using System.Collections.Immutable;

using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// highlight. dragged item type. highlight step. blink state. blink delay. cursor image. ground
// offset/count. show info
public partial class UnitWindow : UIModule
{
    public UnitWindow(UIContext ui) : base(ui)
    { }

    public Window MainWindow { get; private set; }
    public TabPanel UnitTabs { get; private set; }
    public Button BottomCloseButton { get; private set; }
    public Memo InfoMemo { get; private set; }
    public Image ManaIcon { get; private set; }
    public Label ManaLabel { get; private set; }
    public ListBox AbilityAbilityList { get; private set; }
    public Button AbilityUseButton { get; private set; }
    public ListBox AbilityTypeList { get; private set; }
    public ListBox AbilitySourceList { get; private set; }
    public VerticalScrollBar AbilityScrollBar { get; private set; }
    public VerticalScrollBar InfoScrollBar { get; private set; }
    public Panel InfoTextPanel { get; private set; }
    public Panel InfoLeaderPanel { get; private set; }
    public Button Sphere4Button { get; private set; }
    public Panel SpherePanel { get; private set; }
    public Button Sphere3Button { get; private set; }
    public Button Sphere2Button { get; private set; }
    public Button Sphere1Button { get; private set; }
    public Button Sphere5Button { get; private set; }
    public Button Sphere6Button { get; private set; }
    public Button Sphere7Button { get; private set; }
    public Image AlignmentImage { get; private set; }
    public Label AlignmentLabel { get; private set; }
    public Image TypeIcon { get; private set; }
    public Label TypeValueLabel { get; private set; }
    public Label SphereLabel { get; private set; }
    public Panel InventoryPanel { get; private set; }
    public CheckBox Backpack1CheckBox { get; private set; }
    public CheckBox Backpack2CheckBox { get; private set; }
    public CheckBox Backpack3CheckBox { get; private set; }
    public CheckBox Backpack4CheckBox { get; private set; }
    public CheckBox Backpack5CheckBox { get; private set; }
    public CheckBox Backpack6CheckBox { get; private set; }
    public CheckBox Backpack7CheckBox { get; private set; }
    public CheckBox Backpack8CheckBox { get; private set; }
    public CheckBox HeadSlotCheckBox { get; private set; }
    public CheckBox TorsoSlotCheckBox { get; private set; }
    public CheckBox AttackSlotCheckBox { get; private set; }
    public CheckBox DefendSlotCheckBox { get; private set; }
    public CheckBox LeftRingCheckBox { get; private set; }
    public CheckBox RightRingCheckBox { get; private set; }
    public CheckBox Ground3CheckBox { get; private set; }
    public CheckBox Ground2CheckBox { get; private set; }
    public CheckBox Ground1CheckBox { get; private set; }
    public CheckBox Ground4CheckBox { get; private set; }
    public CheckBox Ground5CheckBox { get; private set; }
    public Button DisbandButton { get; private set; }
    public Button ItemUseButton { get; private set; }
    public Label UnitNameLabel { get; private set; }
    public Image UnitMovesIcon { get; private set; }
    public Label UnitAttackLabel { get; private set; }
    public Label UnitDefenseLabel { get; private set; }
    public Label UnitMovesLabel { get; private set; }
    public Label UnitDamageLabel { get; private set; }
    public Label UnitResistanceLabel { get; private set; }
    public Label UnitHitsLabel { get; private set; }
    public Label LevelLabel { get; private set; }
    public Label MoraleLabel { get; private set; }
    public Image UpkeepIcon { get; private set; }
    public Label UpkeepLabel { get; private set; }
    public Button GroundPreviousButton { get; private set; }
    public Button GroundNextButton { get; private set; }
    public Image RelationIcon { get; private set; }
    public Label RelationValueLabel { get; private set; }
    public Label ExperienceLabel { get; private set; }
    public Button NextUnitButton { get; private set; }
    public Button PreviousUnitButton { get; private set; }

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
        Troop u;
        Ground1CheckBox.Tag = -1;
        Ground2CheckBox.Tag = -1;
        Ground3CheckBox.Tag = -1;
        Ground4CheckBox.Tag = -1;
        Ground5CheckBox.Tag = -1;
        Ground1CheckBox.Checked = false;
        Ground2CheckBox.Checked = false;
        Ground3CheckBox.Checked = false;
        Ground4CheckBox.Checked = false;
        Ground5CheckBox.Checked = false;
        u = UI.General.SelectedUnit;
        if (u != null)
        {
            if (u is Hero)
            {
                if (((Hero)u).GetItemsOnGround() != null)
                {
                    _groundCount = ((Hero)u).GetItemsOnGround().Count;
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

                    GroundPreviousButton.Visible = _groundOffset > 0;
                    GroundNextButton.Visible = _groundOffset + 5 <= _groundCount;
                    var count = ((Hero)u).GetItemsOnGround().Count;
                    if (_groundOffset < count)
                    {
                        var item = ((Hero)u).GetItemsOnGround().GetItem(_groundOffset);
                        Ground1CheckBox.Tag = item.ItemId;
                    }

                    var count2 = ((Hero)u).GetItemsOnGround().Count;
                    if (_groundOffset + 1 < count2)
                    {
                        var item = ((Hero)u).GetItemsOnGround().GetItem(_groundOffset + 1);
                        Ground2CheckBox.Tag = item.ItemId;
                    }

                    var count3 = ((Hero)u).GetItemsOnGround().Count;
                    if (_groundOffset + 2 < count3)
                    {
                        var item = ((Hero)u).GetItemsOnGround().GetItem(_groundOffset + 2);
                        Ground3CheckBox.Tag = item.ItemId;
                    }

                    var count4 = ((Hero)u).GetItemsOnGround().Count;
                    if (_groundOffset + 3 < count4)
                    {
                        var item = ((Hero)u).GetItemsOnGround().GetItem(_groundOffset + 3);
                        Ground4CheckBox.Tag = item.ItemId;
                    }

                    var count5 = ((Hero)u).GetItemsOnGround().Count;
                    if (_groundOffset + 4 < count5)
                    {
                        var item = ((Hero)u).GetItemsOnGround().GetItem(_groundOffset + 4);
                        Ground5CheckBox.Tag = item.ItemId;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:472
    public void PreDrawAbilityItem(UIComponent sender, in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        bool enabled;
        Ability a;
        var objectValue = AbilityAbilityList.Items.Objects[drawItem];
        a = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
        if (StrategicMap.Current.CombatMap == null)
        {
            enabled = a != null && UI.General.SelectedUnit != null && UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer && a.GetControlType(UI.General.SelectedUnit).Contains(AbilityControlTypeElement.Action);
        }
        else
        {
            bool condition = a != null && UI.General.CombatUnit != null;
            if (condition)
            {
                var objectValue2 = AbilityAbilityList.Items.Objects[drawItem];
                condition = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetControlType(UI.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && UI.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                var objectValue2 = AbilityAbilityList.Items.Objects[drawItem];
                var combatMode = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(AbilityAbilityList.Items.Objects[drawItem]) == 52) || UI.General.CombatUnit.Unit.Troop.GetMoves() == UI.General.CombatUnit.Unit.MovePoints;
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
        UI.General.PlayButtonSound(0);
    }

    // AoWUnitWindow.pas:507
    public void T1AbilityLBChange(ListBox sender)
    {
        Ability a;
        AbilityAbilityList.Selected = sender.Selected;
        AbilityTypeList.Selected = sender.Selected;
        AbilitySourceList.Selected = sender.Selected;
        if (StrategicMap.Current.CombatMap == null)
        {
            if (UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer && AbilityAbilityList.Selected > -1)
            {
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                a = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
                if (a != null && a.GetEnabled(UI.General.SelectedUnit))
                {
                    if (StrategicMap.Current.CombatMap == null)
                    {
                        var controlType = a.GetControlType(UI.General.SelectedUnit);
                        AbilityUseButton.Visible = controlType.Contains(AbilityControlTypeElement.Action);
                    }
                    else
                    {
                        var objectValue2 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                        bool condition = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(UI.General.SelectedUnit) != CombatMode.None;
                        if (condition)
                        {
                            var objectValue3 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                            condition = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue3)).GetCombatMode(UI.General.SelectedUnit) != CombatMode.Melee;
                        }

                        AbilityUseButton.Visible = condition;
                    }

                    if (AbilityUseButton.Visible)
                    {
                        var name = a.GetName(UI.General.SelectedUnit);
                        var format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_useFormat), [name]);
                        AbilityUseButton.Text = format;
                    }
                }
                else
                {
                    AbilityUseButton.Visible = false;
                }
            }
            else
            {
                AbilityUseButton.Visible = false;
            }
        }
        else
        {
            bool condition = AbilityAbilityList.Selected > -1 && UI.General.CombatUnit != null;
            if (condition)
            {
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                condition = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetControlType(UI.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && UI.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                var combatMode = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected]) == 52) || UI.General.CombatUnit.Unit.Troop.GetMoves() == UI.General.CombatUnit.Unit.MovePoints;
            }

            if (test)
            {
                AbilityUseButton.Visible = true;
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                var name = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetName(UI.General.CombatUnit.Unit.Troop);
                var format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_useFormat), [name]);
                AbilityUseButton.Text = format;
            }
            else
            {
                AbilityUseButton.Visible = false;
            }
        }
    }

    // AoWUnitWindow.pas:562
    public void T1SbarChange()
    {
        AbilityAbilityList.ListOffset = AbilityScrollBar.Position;
        AbilityTypeList.ListOffset = AbilityScrollBar.Position;
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
        this.MainWindow.Alignment.LeftOffset = this.MainWindow.WinLeft;
        this.MainWindow.Alignment.TopOffset = this.MainWindow.WinTop;
        this.MainWindow.Alignment.AlignHeight = AlignHeight.Top;
        this.MainWindow.Alignment.AlignWidth = AlignWidth.Left;
    }

    // AoWUnitWindow.pas:586
    public void UnitWindowCreate()
    {
        _itemType = (ItemType)6;
    }

    // AoWUnitWindow.pas:591
    public void UnitWinUpdate()
    {
        if (MainWindow.Visible)
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
                    AttackSlotCheckBox.Drawn = false;
                    break;
                case ItemType.Defense:
                    DefendSlotCheckBox.Drawn = false;
                    break;
                case ItemType.Head:
                    HeadSlotCheckBox.Drawn = false;
                    break;
                case ItemType.Torso:
                    TorsoSlotCheckBox.Drawn = false;
                    break;
                case ItemType.Ring:
                    LeftRingCheckBox.Drawn = false;
                    RightRingCheckBox.Drawn = false;
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
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        lists.Add(AbilityAbilityList);
        lists.Add(AbilityTypeList);
        lists.Add(AbilitySourceList);
        UI.General.SortLists(ref lists, 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:631
    public void T1SortTypeClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        lists.Add(AbilityTypeList);
        lists.Add(AbilityAbilityList);
        lists.Add(AbilitySourceList);
        UI.General.SortLists(ref lists, 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:642
    public void T1SortSourceClick(int button)
    {
        DelphiList<ListBox> lists;
        UI.General.PlayButtonSound(0);
        lists = new DelphiList<ListBox>();
        lists.Add(AbilitySourceList);
        lists.Add(AbilityAbilityList);
        lists.Add(AbilityTypeList);
        UI.General.SortLists(ref lists, 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:653
    public void UseAbility()
    {
        Ability a;
        string reason = "";
        UI.General.PlayButtonSound(0);
        var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
        a = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
        if (a != null && a.GetEnabled(UI.General.SelectedUnit))
        {
            if (StrategicMap.Current.CombatMap == null)
            {
                if (UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer)
                {
                    if (a.Activate(UI.General.SelectedUnit, ref reason))
                    {
                        MainWindow.Visible = false;
                    }
                    else
                    {
                        if (!IsEmpty(reason))
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }
                    }
                }
            }
            else
            {
                if (UI.General.CombatUnit != null && UI.General.CombatUnit.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                {
                    var objectValue2 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                    if (UI.General.CombatUnit.SelectAbility(ObjectToInt(objectValue2)) > -1)
                    {
                        MainWindow.Visible = false;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:676
    public void UseItemClick()
    {
        string reason = "";
        UI.General.PlayButtonSound(0);
        if (s_selectedItem != null && UI.General.SelectedUnit is Hero)
        {
            if (((Hero)UI.General.SelectedUnit).CanUseItem(s_selectedItem.Tag, ref reason))
            {
                ((Hero)UI.General.SelectedUnit).UseItem(s_selectedItem.Tag);
            }
            else
            {
                StrategicMap.Current.ShowWarning(reason);
            }
        }
    }

    // AoWUnitWindow.pas:688
    public void UnitTabChange()
    {
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWUnitWindow.pas:730
    public void Gnd1Draw(UIComponent sender, LockedSurface lockedSurface)
    {
        Hero u;
        Item item;
        UIComponent component;
        component = sender;
        if (component.Tag != -1)
        {
            u = (Hero)UI.General.SelectedUnit;
            item = u.Inventory.Items.FindItem(component.Tag);
            if (item == null)
            {
                item = u.HeroItems.Items.FindItem(component.Tag);
            }

            if (item == null)
            {
                if (u.GetItemsOnGround() != null)
                {
                    item = u.GetItemsOnGround().FindItem(component.Tag);
                }
            }

            if (item != null)
            {
                var checkBox = component;
                if (item.GetImage() != null)
                {
                    item.GetImage().ShowClipped(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface);
                }
            }
        }
    }

    // AoWUnitWindow.pas:750
    public void FrLShPreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect r;
        if (UI.General.SelectedUnit != null)
        {
            if (UI.General.SelectedUnit.GetFace() != null)
            {
                var image = sender;
                r = new Rect(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);

                UI.General.SelectedUnit.ShowFace(r, lockedSurface);
            }
        }
    }

    // AoWUnitWindow.pas:763
    public void FrRShPreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        if (UI.General.SelectedUnit != null)
        {
            _showInfo.Direction = 3;
            if (UI.General.SelectedUnit is Hero)
            {
                _showInfo.Overlays = Set<UnitOverlayModesElement>.Of();
            }
            else
            {
                _showInfo.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
            }

            var image = sender;
            UI.General.SelectedUnit.Show(image.WinLeft + 49, image.WinTop + 78, lockedSurface, _showInfo);
        }
    }

    // AoWUnitWindow.pas:855
    public void Gnd3MouseDown(CheckBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (UI.General.SelectedUnit != null)
        {
            if (!StrategicMap.Current.Players.GetPlayers(UI.General.SelectedUnit.Player).GetBusy())
            {
                if (button == MouseButton.Right)
                {
                    if (sender.Tag == -1)
                    {
                        sender.PopupMenu = null;
                    }
                    else
                    {
                        sender.PopupMenu = UI.ItemBanner.BannerMenu;
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
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:881
    public void TorsoDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)1)
        {
            var checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:891
    public void AttackDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)2)
        {
            var checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:901
    public void DefendDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)3)
        {
            var checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:911
    public void RingLDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)4)
        {
            var checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:921
    public void DisbandBtnClick()
    {
        var disbandUnit = UI.Engine.TranslateResourceString(s_disbandUnitText);
        var confirm = UI.Engine.TranslateResourceString(LobbyText.ConfirmText);
        UI.ConfirmationDialog.Confirm(confirm, text: disbandUnit, onConfirmed: DisbandSelectedUnit);
    }

    // AOWDialogWin.pas:73, action 7.
    private void DisbandSelectedUnit()
    {
        string reason = "";
        if (UI.General.SelectedUnit != null)
        {
            if (UI.General.SelectedUnit.CanDisband(ref reason))
            {
                if (UI.General.SelectedUnit.Disband())
                {
                    UI.UnitWindow.MainWindow.Visible = false;
                }
            }
            else
            {
                StrategicMap.Current.ShowWarning(reason);
            }
        }
    }

    // AoWUnitWindow.pas:926
    public void T1AbilityLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability a;
        if (button == MouseButton.Right && UI.General.SelectedUnit != null && AbilityAbilityList.Selected != -1)
        {
            var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
            a = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (a != null)
            {
                InfoBanner.SetInfoBanner(caption: a.GetName(null), banner: UI.InfoBanner, description: a.Description);
            }
        }
    }

    // AoWUnitWindow.pas:937
    public void UnitWinKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 27:
                MainWindow.Visible = false;
                break;
        }
    }

    // AoWUnitWindow.pas:944
    public void PPrevClick()
    {
        int i;
        UI.General.PlayButtonSound(0);
        if (UI.General.UnitList != null)
        {
            i = UI.General.IndexOfUnit(UI.General.SelectedUnit);
            if (i > -1 && UI.General.SelectedUnits[0] > -1)
            {
                i -= 1;
                if (i < 0)
                {
                    i = 7;
                    while (UI.General.SelectedUnits[i] == -1)
                    {
                        i -= 1;
                    }
                }

                var troop = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[i]);
                UI.General.SelectedUnit = troop;
                ShowUnit();
            }
        }
    }

    // AoWUnitWindow.pas:966
    public void PNextClick()
    {
        int i;
        UI.General.PlayButtonSound(0);
        if (UI.General.UnitList != null)
        {
            i = UI.General.IndexOfUnit(UI.General.SelectedUnit);
            if (i > -1)
            {
                i += 1;
                if (i > 7)
                {
                    i = 0;
                }

                if (UI.General.SelectedUnits[i] == -1)
                {
                    i = 0;
                }

                var troop = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[i]);
                UI.General.SelectedUnit = troop;
                ShowUnit();
            }
        }
    }

    private static readonly string s_moraleFormat = "Morale: %s";
    private static readonly string s_useFormat = "Use %s";
    private static readonly ImmutableArray<string> s_moraleTexts = ["Terrible", "Poor", "Okay", "Good", "High"];
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

    private int CompareUnitAbilities(string s1, string s2, object o1, object o2)
    {
        Ability ability = GamePack.Current.AbilityControl.GetAbility(RtlUnit.ObjectToInt(o1));
        Ability ability2 = GamePack.Current.AbilityControl.GetAbility(RtlUnit.ObjectToInt(o2));
        int number = ((ability.GetEnabled(UI.General.SelectedUnit) && ability.GetControlType(UI.General.SelectedUnit).Contains(AbilityControlTypeElement.Action)) ? 10 : 0);
        int number2 = ((ability2.GetEnabled(UI.General.SelectedUnit) && ability2.GetControlType(UI.General.SelectedUnit).Contains(AbilityControlTypeElement.Action)) ? 10 : 0);
        int number3 = number2 - number;
        if (number3 == 0)
        {
            number3 = RtlUnit.CompareStr(s1, s2);
        }
        return number3;
    }
}
