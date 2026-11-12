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

    public Window MainWindow;
    public TabPanel UnitTabs;
    public Button BottomCloseButton;
    public Memo InfoMemo;
    public Image ManaIcon;
    public Label ManaLabel;
    public ListBox AbilityAbilityList;
    public Button AbilityUseButton;
    public ListBox AbilityTypeList;
    public ListBox AbilitySourceList;
    public VerticalScrollBar AbilityScrollBar;
    public VerticalScrollBar InfoScrollBar;
    public Panel InfoTextPanel;
    public Panel InfoLeaderPanel;
    public Button Sphere4Button;
    public Panel SpherePanel;
    public Button Sphere3Button;
    public Button Sphere2Button;
    public Button Sphere1Button;
    public Button Sphere5Button;
    public Button Sphere6Button;
    public Button Sphere7Button;
    public Image AlignmentImage;
    public Label AlignmentLabel;
    public Image TypeIcon;
    public Label TypeValueLabel;
    public Label SphereLabel;
    public Panel InventoryPanel;
    public CheckBox Backpack1CheckBox;
    public CheckBox Backpack2CheckBox;
    public CheckBox Backpack3CheckBox;
    public CheckBox Backpack4CheckBox;
    public CheckBox Backpack5CheckBox;
    public CheckBox Backpack6CheckBox;
    public CheckBox Backpack7CheckBox;
    public CheckBox Backpack8CheckBox;
    public CheckBox HeadSlotCheckBox;
    public CheckBox TorsoSlotCheckBox;
    public CheckBox AttackSlotCheckBox;
    public CheckBox DefendSlotCheckBox;
    public CheckBox LeftRingCheckBox;
    public CheckBox RightRingCheckBox;
    public CheckBox Ground3CheckBox;
    public CheckBox Ground2CheckBox;
    public CheckBox Ground1CheckBox;
    public CheckBox Ground4CheckBox;
    public CheckBox Ground5CheckBox;
    public Button DisbandButton;
    public Button ItemUseButton;
    public Label UnitNameLabel;
    public Image UnitMovesIcon;
    public Label UnitAttackLabel;
    public Label UnitDefenseLabel;
    public Label UnitMovesLabel;
    public Label UnitDamageLabel;
    public Label UnitResistanceLabel;
    public Label UnitHitsLabel;
    public Label LevelLabel;
    public Label MoraleLabel;
    public Image UpkeepIcon;
    public Label UpkeepLabel;
    public Button GroundPreviousButton;
    public Button GroundNextButton;
    public Image RelationIcon;
    public Label RelationValueLabel;
    public Label ExperienceLabel;
    public Button NextUnitButton;
    public Button PreviousUnitButton;
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

                    GroundPreviousButton.Visible = _groundOffset > 0;
                    GroundNextButton.Visible = _groundOffset + 5 <= _groundCount;
                    var count = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset < count)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset);
                        Ground1CheckBox.Tag = item.ItemId;
                    }

                    var count2 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 1 < count2)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 1);
                        Ground2CheckBox.Tag = item.ItemId;
                    }

                    var count3 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 2 < count3)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 2);
                        Ground3CheckBox.Tag = item.ItemId;
                    }

                    var count4 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 3 < count4)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 3);
                        Ground4CheckBox.Tag = item.ItemId;
                    }

                    var count5 = ((Hero)U).GetItemsOnGround().Count;
                    if (_groundOffset + 4 < count5)
                    {
                        var item = ((Hero)U).GetItemsOnGround().GetItem(_groundOffset + 4);
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
        Ability A;
        var objectValue = AbilityAbilityList.Items.Objects[drawItem];
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
                var objectValue2 = AbilityAbilityList.Items.Objects[drawItem];
                condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetControlType(DialogHosts.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && DialogHosts.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && DialogHosts.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                var objectValue2 = AbilityAbilityList.Items.Objects[drawItem];
                var combatMode = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(DialogHosts.General.CombatUnit.Unit.Troop);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(AbilityAbilityList.Items.Objects[drawItem]) == 52) || DialogHosts.General.CombatUnit.Unit.Troop.GetMoves() == DialogHosts.General.CombatUnit.Unit.MovePoints;
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
        AbilityAbilityList.Selected = sender.Selected;
        AbilityTypeList.Selected = sender.Selected;
        AbilitySourceList.Selected = sender.Selected;
        if (AoWHSMap.Current.CombatMap == null)
        {
            if (DialogHosts.General.SelectedUnit.Player == AoWHSMap.Current.SeatedPlayer && AbilityAbilityList.Selected > -1)
            {
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                A = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
                if (A != null && A.GetEnabled(DialogHosts.General.SelectedUnit))
                {
                    if (AoWHSMap.Current.CombatMap == null)
                    {
                        var controlType = A.GetControlType(DialogHosts.General.SelectedUnit);
                        AbilityUseButton.Visible = controlType.Contains(AbilityControlTypeElement.Action);
                    }
                    else
                    {
                        var objectValue2 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                        bool condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue2)).GetCombatMode(DialogHosts.General.SelectedUnit) != CombatMode.None;
                        if (condition)
                        {
                            var objectValue3 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                            condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue3)).GetCombatMode(DialogHosts.General.SelectedUnit) != CombatMode.Melee;
                        }

                        AbilityUseButton.Visible = condition;
                    }

                    if (AbilityUseButton.Visible)
                    {
                        var name = A.GetName(DialogHosts.General.SelectedUnit);
                        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_useFormat), [name]);
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
            bool condition = AbilityAbilityList.Selected > -1 && DialogHosts.General.CombatUnit != null;
            if (condition)
            {
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                condition = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetControlType(DialogHosts.General.CombatUnit.Unit.Troop).Contains(AbilityControlTypeElement.CombatAction);
            }

            bool test = condition && DialogHosts.General.CombatUnit.Unit.GetPlayer() == CombatMap.Current.SeatedPlayer && DialogHosts.General.CombatUnit.Unit.MovePoints > 0;
            if (test)
            {
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                var combatMode = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetCombatMode(DialogHosts.General.CombatUnit.Unit.Troop);
                test = !(Set<CombatMode>.Of(CombatMode.ShortRange, CombatMode.MediumRange, CombatMode.LongRange).Contains(combatMode) || ObjectToInt(AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected]) == 52) || DialogHosts.General.CombatUnit.Unit.Troop.GetMoves() == DialogHosts.General.CombatUnit.Unit.MovePoints;
            }

            if (test)
            {
                AbilityUseButton.Visible = true;
                var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                var name = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue)).GetName(DialogHosts.General.CombatUnit.Unit.Troop);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_useFormat), [name]);
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
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(AbilityAbilityList);
        lists.Add(AbilityTypeList);
        lists.Add(AbilitySourceList);
        DialogHosts.General.SortLists(ref lists, 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:631
    public void T1SortTypeClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(AbilityTypeList);
        lists.Add(AbilityAbilityList);
        lists.Add(AbilitySourceList);
        DialogHosts.General.SortLists(ref lists, 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:642
    public void T1SortSourceClick(int button)
    {
        DelphiList lists;
        DialogHosts.General.PlayButtonSound(0);
        lists = new DelphiList();
        lists.Add(AbilitySourceList);
        lists.Add(AbilityAbilityList);
        lists.Add(AbilityTypeList);
        DialogHosts.General.SortLists(ref lists, 0);
        AbilityUseButton.Visible = false;
    }

    // AoWUnitWindow.pas:653
    public void UseAbility()
    {
        Ability A;
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
        A = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
        if (A != null && A.GetEnabled(DialogHosts.General.SelectedUnit))
        {
            if (AoWHSMap.Current.CombatMap == null)
            {
                if (DialogHosts.General.SelectedUnit.Player == AoWHSMap.Current.SeatedPlayer)
                {
                    if (A.Activate(DialogHosts.General.SelectedUnit, ref reason))
                    {
                        MainWindow.Visible = false;
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
                    var objectValue2 = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
                    if (DialogHosts.General.CombatUnit.SelectAbility(ObjectToInt(objectValue2)) > -1)
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
        MainWindow.Visible = false;
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
                    item.GetImage().ShowClipped(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface);
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
                        sender.PopupMenu = DialogHosts.ItemBanner.BannerMenu;
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
            DialogHosts.UIGraphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:881
    public void TorsoDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)1)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:891
    public void AttackDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)2)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:901
    public void DefendDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)3)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
        }
    }

    // AoWUnitWindow.pas:911
    public void RingLDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Gnd1Draw(sender, lockedSurface);
        if (s_selectedItem != null && sender != s_selectedItem && _itemType == (ItemType)4)
        {
            var checkBox = sender;
            DialogHosts.UIGraphics.UnitWindow[24].ShowBlended(InventoryPanel.WinLeft + checkBox.WinLeft, InventoryPanel.WinTop + checkBox.WinTop, lockedSurface, GraphicsState.IntensityBlendTable.LookupTables[_highlight]);
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
        if (button == MouseButton.Right && DialogHosts.General.SelectedUnit != null && AbilityAbilityList.Selected != -1)
        {
            var objectValue = AbilityAbilityList.Items.Objects[AbilityAbilityList.Selected];
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
                MainWindow.Visible = false;
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
