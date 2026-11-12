// Translated from AoWItemsFoundDlg.pas.
using System.Runtime.CompilerServices;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// item offset, completion event, item event
public partial class ItemsFoundDialog : DataModule
{
    public ItemsFoundDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label TitleLabel;
    public Panel PickupPanel;
    public Panel OKPanel;
    public Memo MessageMemo;
    public Button LeftButton;
    public Button RightButton;
    private int _itemOffset;
    private ExecuteDoneEvent _onDone;
    private ItemExplorationSiteEventLog _event;

    // AoWItemsFoundDlg.pas:80
    public Hero GetHero()
    {
        ArmyHexSprite army;
        Hero result = null;
        if (_event != null)
        {
            army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(Unsafe.As<MapCoordinate, int>(ref _event.Location))).GetArmyHexSprite();
            if (army != null && army.Army.Player == AoWHSMap.Current.SeatedPlayer)
            {
                result = army.Army.Hero(255, RosterTables.AllHeroTypes);
            }
        }

        return result;
    }

    // AoWItemsFoundDlg.pas:66
    public void Finish()
    {
        ExecuteDoneEvent done;
        ItemExplorationSiteEventLog eventValue;
        Window.Visible = false;
        done = _onDone;
        eventValue = _event;
        _onDone = null;
        _event = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // AoWItemsFoundDlg.pas:62
    public void Reset()
    {
        _itemOffset = 0;
        PickupPanel.Visible = false;
        OKPanel.Visible = false;
    }

    // AoWItemsFoundDlg.pas:63
    public void DlgHide()
    {
        Finish();
    }

    // AoWItemsFoundDlg.pas:92
    public void CloseBtn2Click()
    {
        Window.Visible = false;
    }

    // AoWItemsFoundDlg.pas:95
    public void Gnd3Draw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        ItemExplorationSiteEventLog eventValue;
        Item item;
        eventValue = _event;
        if (eventValue != null)
        {
            if (slot + _itemOffset < eventValue.ItemIds.Count)
            {
                var itemId = _event.ItemIds[slot + _itemOffset];
                item = AoWHSMap.Current.ItemControl.FindItem(itemId);
                if (item != null)
                {
                    item.Show(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, lockedSurface);
                }
            }
        }
    }

    // AoWItemsFoundDlg.pas:110
    public void PickupBtnClick()
    {
        Hero hero;
        TutorialMessageEventLog eventValue;
        hero = GetHero();
        if (hero != null)
        {
            DialogHosts.General.SelectedUnit = hero;
            DialogHosts.UnitWindow.ShowUnit();
            DialogHosts.UnitWindow.UnitTabs.Selected = 2;
            if (AoWHSMap.Current.GameType == GameType.Tutorial)
            {
                eventValue = new TutorialMessageEventLog(null);
                eventValue.HelpFileName = AbilityRules.TranslateResourceString(GameText.TutorialText);
                if (AoWHSMap.Current.TextControl.FindId(55) != null)
                {
                    var id = AoWHSMap.Current.TextControl.FindId(55);
                    eventValue.Message.Assign(id.Text);
                    AoWHSMap.Current.TriggerExecuteEventLog(eventValue, true, null);
                }

                eventValue.Release();
            }
        }

        Window.Visible = false;
    }

    // AoWItemsFoundDlg.pas:136
    public void Gnd3MouseDown(CheckBox sender, MouseButton button, Set<ShiftState> shift, int X, int Y, int slot)
    {
        Item item;
        CheckBox box;
        box = sender;
        if (button == MouseButton.Right)
        {
            box.PopupMenu = null;
            if (_itemOffset + slot < _event.ItemIds.Count)
            {
                var itemId = _event.ItemIds[_itemOffset + slot];
                item = AoWHSMap.Current.ItemControl.FindItem(itemId);
                if (item != null)
                {
                    box.PopupMenu = DialogHosts.ItemBanner.BannerMenu;
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
            Window.Drawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:164
    public void RightBtnClick()
    {
        if (_itemOffset + 5 < _event.ItemIds.Count)
        {
            _itemOffset += 1;
            Window.Drawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:173
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowItemsFound(ItemsFoundDialog dialog, ItemExplorationSiteEventLog eventValue, ExecuteDoneEvent done)
    {
        ItemExplorationSiteEventLog log;
        dialog.Reset();
        dialog._onDone = done;
        dialog._event = eventValue;
        dialog._event.AddRef();
        log = dialog._event;
        dialog.TitleLabel.Text = log.Title;
        var text = log.Messages.Text;
        dialog.MessageMemo.Items.Text = text;
        dialog.LeftButton.Visible = log.ItemIds.Count > 5;
        dialog.RightButton.Visible = log.ItemIds.Count > 5;
        if (dialog.GetHero() == null)
        {
            dialog.OKPanel.Visible = true;
        }
        else
        {
            dialog.PickupPanel.Visible = true;
        }

        AoWHSMap.Current.TutorialControl.ShowMessage(54, true, true);
        dialog.Window.Drawn = false;
        dialog.Window.Visible = true;
    }

}
