#nullable disable warnings
// Translated from AoWItemsFoundDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// item offset, completion event, item event
public sealed partial class ItemsFoundDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label TitleLabel { get; private set; }
    public Panel PickupPanel { get; private set; }
    public Panel OKPanel { get; private set; }
    public Memo MessageMemo { get; private set; }
    public Button LeftButton { get; private set; }
    public Button RightButton { get; private set; }

    private int _itemOffset;
    private ExecuteDoneEvent? _done;
    private ItemExplorationSiteEventLog? _event;

    // AoWItemsFoundDlg.pas:80
    public Hero? GetHero()
    {
        if (_event is not null)
        {
            ArmyHexSprite army = StrategicMap.Current.MapContainer.GetFieldMapCoordinate(_event.Location).GetArmyHexSprite();
            if (army is not null && army.Army.Player == StrategicMap.Current.SeatedPlayer)
            {
                return army.Army.Hero(255, types: RosterTables.AllHeroTypes);
            }
        }

        return null;
    }

    // AoWItemsFoundDlg.pas:66
    public void Finish()
    {
        Window.Visible = false;
        ExecuteDoneEvent? done = _done;
        ItemExplorationSiteEventLog? eventValue = _event;
        _done = null;
        _event = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWItemsFoundDlg.pas:62
    public void Reset()
    {
        _itemOffset = 0;
        PickupPanel.Visible = false;
        OKPanel.Visible = false;
    }

    // AoWItemsFoundDlg.pas:63
    public void DlgHide() => Finish();

    // AoWItemsFoundDlg.pas:92
    public void CloseBtn2Click() => Window.Visible = false;

    // AoWItemsFoundDlg.pas:95
    public void Gnd3Draw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        ItemExplorationSiteEventLog? eventValue = _event;
        if (eventValue is not null)
        {
            if (slot + _itemOffset < eventValue.ItemIds.Count)
            {
                int itemId = _event.ItemIds[slot + _itemOffset];
                Item? item = StrategicMap.Current.ItemControl.FindItem(itemId);
                item?.Show(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, surface: lockedSurface);
            }
        }
    }

    // AoWItemsFoundDlg.pas:110
    public void PickupBtnClick()
    {
        Hero? hero = GetHero();
        if (hero is not null)
        {
            UI.General.SelectedUnit = hero;
            UI.UnitWindow.ShowUnit();
            UI.UnitWindow.UnitTabs.Selected = 2;
            if (StrategicMap.Current.GameType == GameType.Tutorial)
            {
                TutorialMessageEventLog eventValue = new(null, holder: UI.Engine)
                {
                    HelpFileName = UI.Engine.TranslateResourceString(GameText.TutorialText)
                };
                if (StrategicMap.Current.TextControl.FindId(55) is not null)
                {
                    TextControlItem? id = StrategicMap.Current.TextControl.FindId(55);
                    eventValue.Message.Assign(id.Text);
                    StrategicMap.Current.TriggerExecuteEventLog(eventValue, immediate: true, done: null);
                }

                _ = eventValue.Release();
            }
        }

        Window.Visible = false;
    }

    // AoWItemsFoundDlg.pas:136
    public void Gnd3MouseDown(CheckBox sender, MouseButton button, ShiftStates shift, int X, int Y, int slot)
    {
        CheckBox box = sender;
        if (button == MouseButton.Right)
        {
            box.PopupMenu = null;
            if (_itemOffset + slot < _event.ItemIds.Count)
            {
                int itemId = _event.ItemIds[_itemOffset + slot];
                Item? item = StrategicMap.Current.ItemControl.FindItem(itemId);
                if (item is not null)
                {
                    box.PopupMenu = UI.ItemBanner.BannerMenu;
                    ItemBanner.ItemId = item.ItemId;
                }
            }
        }
    }

    // AoWItemsFoundDlg.pas:155
    public void LeftBtnClick()
    {
        if (_itemOffset > 0)
        {
            _itemOffset -= 1;
            Window.IsDrawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:164
    public void RightBtnClick()
    {
        if (_itemOffset + 5 < _event.ItemIds.Count)
        {
            _itemOffset += 1;
            Window.IsDrawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:173
    public void DlgKeyDown(in ushort key, ShiftStates shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowItemsFound(ItemsFoundDialog dialog, ItemExplorationSiteEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.Reset();
        dialog._done = done;
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        ItemExplorationSiteEventLog log = dialog._event;
        dialog.TitleLabel.Text = log.Title;
        string text = log.Messages.Text;
        dialog.MessageMemo.Items.Text = text;
        dialog.LeftButton.Visible = log.ItemIds.Count > 5;
        dialog.RightButton.Visible = log.ItemIds.Count > 5;
        if (dialog.GetHero() is null)
        {
            dialog.OKPanel.Visible = true;
        }
        else
        {
            dialog.PickupPanel.Visible = true;
        }

        StrategicMap.Current.TutorialControl.ShowMessage(54, immediate: true, onceOnly: true);
        dialog.Window.IsDrawn = false;
        dialog.Window.Visible = true;
    }
}
