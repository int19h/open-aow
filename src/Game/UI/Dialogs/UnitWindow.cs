#nullable disable warnings
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

using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// highlight. dragged item type. highlight step. blink state. blink delay. cursor image. ground
// offset/count. show info
public sealed partial class UnitWindow(UIContext ui) : UIModule(ui)
{
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
    private LibraryImage? _cursorImage;
    private int _groundOffset;
    private int _groundCount;
    private UnitShowInfo _showInfo;

    // AoWUnitWindow.pas:985
    public void UpdateGround(int offset)
    {
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
        Troop? u = UI.General.SelectedUnit;
        if (u is not null)
        {
            if (u is Hero hero)
            {
                if (hero.GetItemsOnGround() is not null)
                {
                    _groundCount = hero.GetItemsOnGround().Count;
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
                    int count = hero.GetItemsOnGround().Count;
                    if (_groundOffset < count)
                    {
                        Item? item = hero.GetItemsOnGround().GetItem(_groundOffset);
                        Ground1CheckBox.Tag = item.ItemId;
                    }

                    int count2 = hero.GetItemsOnGround().Count;
                    if (_groundOffset + 1 < count2)
                    {
                        Item? item = hero.GetItemsOnGround().GetItem(_groundOffset + 1);
                        Ground2CheckBox.Tag = item.ItemId;
                    }

                    int count3 = hero.GetItemsOnGround().Count;
                    if (_groundOffset + 2 < count3)
                    {
                        Item? item = hero.GetItemsOnGround().GetItem(_groundOffset + 2);
                        Ground3CheckBox.Tag = item.ItemId;
                    }

                    int count4 = hero.GetItemsOnGround().Count;
                    if (_groundOffset + 3 < count4)
                    {
                        Item? item = hero.GetItemsOnGround().GetItem(_groundOffset + 3);
                        Ground4CheckBox.Tag = item.ItemId;
                    }

                    int count5 = hero.GetItemsOnGround().Count;
                    if (_groundOffset + 4 < count5)
                    {
                        Item? item = hero.GetItemsOnGround().GetItem(_groundOffset + 4);
                        Ground5CheckBox.Tag = item.ItemId;
                    }
                }
            }
        }
    }

    // AoWUnitWindow.pas:472
    public void PreDrawAbilityItem(UIComponent sender, in Rect drawRectangle, int drawItem, ref LockedSurface lockedSurface)
    {
        object? objectValue = AbilityAbilityList.Items.Objects[drawItem];
        Ability? a = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
        bool enabled;
        if (StrategicMap.Current.CombatMap is null)
        {
            enabled = a is not null && UI.General.SelectedUnit is not null && UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer && a.GetControlType(UI.General.SelectedUnit).Contains(AbilityControlTypes.Action);
        }
        else
        {
            bool condition = a is not null && UI.General.CombatUnit is not null;
            if (condition)
            {
                object? objectValue2 = AbilityAbilityList.Items.Objects[drawItem];
                condition = GamePack.Current.AbilityControl[ObjectToInt(objectValue2)].GetControlType(UI.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypes.CombatAction);
            }

            bool test = condition && UI.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                object? objectValue2 = AbilityAbilityList.Items.Objects[drawItem];
                CombatMode combatMode = GamePack.Current.AbilityControl[ObjectToInt(objectValue2)].GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                test = !(combatMode is >= CombatMode.ShortRange and <= CombatMode.LongRange || ObjectToInt(AbilityAbilityList.Items.Objects[drawItem]) == 52) || UI.General.CombatUnit.Unit.Troop.Moves == UI.General.CombatUnit.Unit.MovePoints;
            }

            enabled = test;
        }

        sender.TextColor = enabled ? 14_679_807 : 9_415_072;
    }

    // AoWUnitWindow.pas:502
    public void HelpClick() => UI.General.PlayButtonSound(0);

    // AoWUnitWindow.pas:507
    public void T1AbilityLBChange(ListBox sender)
    {
        AbilityAbilityList.Selected = sender.Selected;
        AbilityTypeList.Selected = sender.Selected;
        AbilitySourceList.Selected = sender.Selected;
        if (StrategicMap.Current.CombatMap is null)
        {
            if (UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer && AbilityAbilityList.Selected > -1)
            {
                object? objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                Ability? a = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
                if (a is not null && a.GetEnabled(UI.General.SelectedUnit))
                {
                    if (StrategicMap.Current.CombatMap is null)
                    {
                        AbilityControlTypes controlType = a.GetControlType(UI.General.SelectedUnit);
                        AbilityUseButton.Visible = controlType.Contains(AbilityControlTypes.Action);
                    }
                    else
                    {
                        object? objectValue2 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                        bool condition = GamePack.Current.AbilityControl[ObjectToInt(objectValue2)].GetCombatMode(UI.General.SelectedUnit) != CombatMode.None;
                        if (condition)
                        {
                            object? objectValue3 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                            condition = GamePack.Current.AbilityControl[ObjectToInt(objectValue3)].GetCombatMode(UI.General.SelectedUnit) != CombatMode.Melee;
                        }

                        AbilityUseButton.Visible = condition;
                    }

                    if (AbilityUseButton.Visible)
                    {
                        string name = a.GetName(UI.General.SelectedUnit);
                        string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_useFormat), arguments: [name]);
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
            bool condition = AbilityAbilityList.Selected > -1 && UI.General.CombatUnit is not null;
            if (condition)
            {
                object? objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                condition = GamePack.Current.AbilityControl[ObjectToInt(objectValue)].GetControlType(UI.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypes.CombatAction);
            }

            bool test = condition && UI.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && UI.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                object? objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                CombatMode combatMode = GamePack.Current.AbilityControl[ObjectToInt(objectValue)].GetCombatMode(UI.General.CombatUnit.Unit.Troop);
                test = !(combatMode is >= CombatMode.ShortRange and <= CombatMode.LongRange || ObjectToInt(AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected]) == 52) || UI.General.CombatUnit.Unit.Troop.Moves == UI.General.CombatUnit.Unit.MovePoints;
            }

            if (test)
            {
                AbilityUseButton.Visible = true;
                object? objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                string name = GamePack.Current.AbilityControl[ObjectToInt(objectValue)].GetName(UI.General.CombatUnit.Unit.Troop);
                string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_useFormat), arguments: [name]);
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
    public void ItemRHMouseDown(CheckBox sender, MouseButton button, ShiftStates shift, int X, int Y)
    {
        CheckBox check = sender;
        ItemBanner.ItemId = check.Tag;
    }

    // AoWUnitWindow.pas:575
    public void UnitWinResize()
    {
        MainWindow.Alignment.LeftOffset = MainWindow.WinLeft;
        MainWindow.Alignment.TopOffset = MainWindow.WinTop;
        MainWindow.Alignment.AlignHeight = AlignHeight.Top;
        MainWindow.Alignment.AlignWidth = AlignWidth.Left;
    }

    // AoWUnitWindow.pas:586
    public void UnitWindowCreate() => _itemType = ItemType.Use;

    // AoWUnitWindow.pas:591
    public void UnitWinUpdate()
    {
        if (MainWindow.Visible)
        {
            _highlight += _highlightStep;
            if (_highlight is > 100 or < 50)
            {
                _highlightStep = -_highlightStep;
                _highlight += _highlightStep;
            }

            switch (_itemType)
            {
                case ItemType.Attack:
                    AttackSlotCheckBox.IsDrawn = false;
                    break;
                case ItemType.Defense:
                    DefendSlotCheckBox.IsDrawn = false;
                    break;
                case ItemType.Head:
                    HeadSlotCheckBox.IsDrawn = false;
                    break;
                case ItemType.Torso:
                    TorsoSlotCheckBox.IsDrawn = false;
                    break;
                case ItemType.Ring:
                    LeftRingCheckBox.IsDrawn = false;
                    RightRingCheckBox.IsDrawn = false;
                    break;
                case ItemType.Scroll:
                case ItemType.Use:
                default:
                    break;
            }

            if (s_selectedItem is not null)
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
        UI.General.PlayButtonSound(0);
        DelphiList<ListBox> lists = new();
        _ = lists.Add(AbilityAbilityList);
        _ = lists.Add(AbilityTypeList);
        _ = lists.Add(AbilitySourceList);
        UI.General.SortLists(lists, order: 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:631
    public void T1SortTypeClick(int button)
    {
        UI.General.PlayButtonSound(0);
        DelphiList<ListBox> lists = new();
        _ = lists.Add(AbilityTypeList);
        _ = lists.Add(AbilityAbilityList);
        _ = lists.Add(AbilitySourceList);
        UI.General.SortLists(lists, order: 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:642
    public void T1SortSourceClick(int button)
    {
        UI.General.PlayButtonSound(0);
        DelphiList<ListBox> lists = new();
        _ = lists.Add(AbilitySourceList);
        _ = lists.Add(AbilityAbilityList);
        _ = lists.Add(AbilityTypeList);
        UI.General.SortLists(lists, order: 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:653
    public void UseAbility()
    {
        UI.General.PlayButtonSound(0);
        object? objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
        Ability? a = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
        if (a is not null && a.GetEnabled(UI.General.SelectedUnit))
        {
            if (StrategicMap.Current.CombatMap is null)
            {
                if (UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer)
                {
                    string reason = "";
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
                if (UI.General.CombatUnit is not null && UI.General.CombatUnit.Unit.CombatPlayer == CombatMap.Current.SeatedCombatPlayer)
                {
                    object? objectValue2 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
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
        UI.General.PlayButtonSound(0);
        if (s_selectedItem is not null && UI.General.SelectedUnit is Hero hero)
        {
            string reason = "";
            if (hero.CanUseItem(s_selectedItem.Tag, ref reason))
            {
                _ = hero.UseItem(s_selectedItem.Tag);
            }
            else
            {
                StrategicMap.Current.ShowWarning(reason);
            }
        }
    }

    // AoWUnitWindow.pas:688
    public void UnitTabChange() => UI.General.PlayButtonSound(0);

    // AoWUnitWindow.pas:693
    public void UnitWinMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (button == MouseButton.Right && s_selectedItem is not null)
        {
            Gnd1Change(s_selectedItem);
        }
    }

    // AoWUnitWindow.pas:714
    public void GroundRClick() => UpdateGround(_groundOffset + 1);

    // AoWUnitWindow.pas:719
    public void GroundLClick() => UpdateGround(_groundOffset - 1);

    // AoWUnitWindow.pas:724
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWUnitWindow.pas:730
    public void Gnd1Draw(UIComponent sender, LockedSurface lockedSurface)
    {
        UIComponent component = sender;
        if (component.Tag != -1 && UI.General.SelectedUnit is Hero heroUnit)
        {
            Hero u = heroUnit;
            Item? item = u.Inventory.Items.FindItem(component.Tag) ?? u.HeroItems.Items.FindItem(component.Tag);
            if (item is null)
            {
                if (u.GetItemsOnGround() is not null)
                {
                    item = u.GetItemsOnGround().FindItem(component.Tag);
                }
            }

            if (item is not null)
            {
                UIComponent checkBox = component;
                item.GetImage()?.ShowClipped(InventoryPanel.AbsoluteLeft + checkBox.WinLeft, InventoryPanel.AbsoluteTop + checkBox.WinTop, lockedSurface);
            }
        }
    }

    // AoWUnitWindow.pas:750
    public void FrLShPreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        if (UI.General.SelectedUnit is not null)
        {
            if (UI.General.SelectedUnit.GetFace() is not null)
            {
                UIComponent image = sender;
                Rect r = new(image.WinLeft + 29, image.WinTop + 28, image.WinLeft + 132, image.WinTop + 156);

                UI.General.SelectedUnit.ShowFace(r, lockedSurface);
            }
        }
    }

    // AoWUnitWindow.pas:763
    public void FrRShPreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        if (UI.General.SelectedUnit is not null)
        {
            _showInfo.Direction = 3;
            _showInfo.Overlays = UI.General.SelectedUnit is Hero
                ? UnitOverlayModes.None
                : UnitOverlayModes.Rank;

            UIComponent image = sender;
            _ = UI.General.SelectedUnit.Show(image.WinLeft + 49, image.WinTop + 78, lockedSurface, info: _showInfo);
        }
    }

    // AoWUnitWindow.pas:855
    public void Gnd3MouseDown(CheckBox sender, MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (UI.General.SelectedUnit is not null)
        {
            if (!StrategicMap.Current.Players.GetPlayers(UI.General.SelectedUnit.Player).GetBusy())
            {
                if (button == MouseButton.Right)
                {
                    sender.PopupMenu = sender.Tag == -1 ? null : UI.ItemBanner.BannerMenu;

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
        if (s_selectedItem is not null && sender != s_selectedItem && _itemType == 0)
        {
            UIComponent checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.AbsoluteLeft + checkBox.WinLeft, InventoryPanel.AbsoluteTop + checkBox.WinTop, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:881
    public void TorsoDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem is not null && sender != s_selectedItem && _itemType == ItemType.Torso)
        {
            UIComponent checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.AbsoluteLeft + checkBox.WinLeft, InventoryPanel.AbsoluteTop + checkBox.WinTop, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:891
    public void AttackDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem is not null && sender != s_selectedItem && _itemType == ItemType.Attack)
        {
            UIComponent checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.AbsoluteLeft + checkBox.WinLeft, InventoryPanel.AbsoluteTop + checkBox.WinTop, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:901
    public void DefendDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem is not null && sender != s_selectedItem && _itemType == ItemType.Defense)
        {
            UIComponent checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.AbsoluteLeft + checkBox.WinLeft, InventoryPanel.AbsoluteTop + checkBox.WinTop, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:911
    public void RingLDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem is not null && sender != s_selectedItem && _itemType == ItemType.Ring)
        {
            UIComponent checkBox = sender;
            UI.Graphics.UnitWindow[24].ShowBlended(InventoryPanel.AbsoluteLeft + checkBox.WinLeft, InventoryPanel.AbsoluteTop + checkBox.WinTop, lockedSurface, table: GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:921
    public void DisbandBtnClick()
    {
        string disbandUnit = UI.Engine.TranslateResourceString(s_disbandUnitText);
        string confirm = UI.Engine.TranslateResourceString(LobbyText.ConfirmText);
        UI.ConfirmationDialog.Confirm(confirm, text: disbandUnit, onConfirmed: DisbandSelectedUnit);
    }

    // AOWDialogWin.pas:73, action 7.
    private void DisbandSelectedUnit()
    {
        if (UI.General.SelectedUnit is not null)
        {
            string reason = "";
            if (UI.General.SelectedUnit.CanDisband(reason))
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
    public void T1AbilityLBMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (button == MouseButton.Right && UI.General.SelectedUnit is not null && AbilityAbilityList.Selected != -1)
        {
            object? objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
            Ability? a = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
            if (a is not null)
            {
                InfoBanner.SetInfoBanner(caption: a.GetName(null), banner: UI.InfoBanner, description: a.Description);
            }
        }
    }

    // AoWUnitWindow.pas:937
    public void UnitWinKeyDown(in ushort key, ShiftStates shift)
    {
        if (key is 27)
        {
            MainWindow.Visible = false;
        }
    }

    // AoWUnitWindow.pas:944
    public void PPrevClick()
    {
        UI.General.PlayButtonSound(0);
        if (UI.General.UnitList is not null)
        {
            int i = UI.General.IndexOfUnit(UI.General.SelectedUnit);
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

                Troop? troop = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[i]);
                UI.General.SelectedUnit = troop;
                ShowUnit();
            }
        }
    }

    // AoWUnitWindow.pas:966
    public void PNextClick()
    {
        UI.General.PlayButtonSound(0);
        if (UI.General.UnitList is not null)
        {
            int i = UI.General.IndexOfUnit(UI.General.SelectedUnit);
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

                Troop? troop = UI.General.UnitList.Units.GetUnit(UI.General.SelectedUnits[i]);
                UI.General.SelectedUnit = troop;
                ShowUnit();
            }
        }
    }

    private static readonly string s_moraleFormat = "Morale: %s";
    private static readonly string s_useFormat = "Use %s";
    private static readonly ImmutableArray<string> s_moraleTexts = ["Terrible", "Poor", "Okay", "Good", "High"];
    private static CheckBox? s_selectedItem;
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

    private int CompareUnitAbilities(string s1, string s2, object? o1, object? o2)
    {
        Ability? ability = GamePack.Current.AbilityControl[ObjectToInt(o1)];
        Ability? ability2 = GamePack.Current.AbilityControl[ObjectToInt(o2)];
        int number = (ability.GetEnabled(UI.General.SelectedUnit) && ability.GetControlType(UI.General.SelectedUnit).Contains(AbilityControlTypes.Action)) ? 10 : 0;
        int number2 = (ability2.GetEnabled(UI.General.SelectedUnit) && ability2.GetControlType(UI.General.SelectedUnit).Contains(AbilityControlTypes.Action)) ? 10 : 0;
        int number3 = number2 - number;
        if (number3 == 0)
        {
            number3 = CompareStr(s1, s2);
        }
        return number3;
    }
}
