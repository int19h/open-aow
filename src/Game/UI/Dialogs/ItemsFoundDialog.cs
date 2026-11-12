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

    public Window Dlg;
    public Label TitleLbl;
    public Panel PickupPnl;
    public Button PickupBtn;
    public Button CloseBtn;
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Image Image1;
    public Image Image2;
    public Button CloseBtn2;
    public Panel OKPnl;
    public Memo MessageMemo;
    public VerticalScrollBar MessageMemoSB;
    public Panel MemoPnl;
    public IvTranslator Translator;
    public CheckBox Gnd3;
    public CheckBox Gnd2;
    public CheckBox Gnd1;
    public CheckBox Gnd4;
    public CheckBox Gnd5;
    public Button LeftBtn;
    public Button RightBtn;
    public int ItemOffset;
    public ExecuteDoneEvent OnDone;
    public ItemExplorationSiteEventLog Event;

    // AoWItemsFoundDlg.pas:80
    public Hero GetHero()
    {
        ArmyHexSprite army;
        Hero result = null;
        if (Event != null)
        {
            army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldMapCoordinate(Unsafe.As<MapCoordinate, int>(ref Event.Location))).GetArmyHexSprite();
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
        Dlg.Visible = false;
        done = OnDone;
        eventValue = Event;
        OnDone = null;
        Event = null;
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
        ItemOffset = 0;
        PickupPnl.Visible = false;
        OKPnl.Visible = false;
    }

    // AoWItemsFoundDlg.pas:63
    public void DlgHide()
    {
        Finish();
    }

    // AoWItemsFoundDlg.pas:92
    public void CloseBtn2Click()
    {
        Dlg.Visible = false;
    }

    // AoWItemsFoundDlg.pas:95
    public void Gnd3Draw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        ItemExplorationSiteEventLog eventValue;
        Item item;
        eventValue = Event;
        if (eventValue != null)
        {
            if (slot + ItemOffset < eventValue.ItemIds.Count)
            {
                var itemId = Event.ItemIds[slot + ItemOffset];
                item = AoWHSMap.Current.ItemControl.FindItem(itemId);
                if (item != null)
                {
                    item.Show(Dlg.WinLeft + sender.WinLeft, Dlg.WinTop + sender.WinTop, lockedSurface);
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
            DialogHosts.UnitWindow.UnitTab.Selected = 2;
            if (AoWHSMap.Current.GameType == GameType.Tutorial)
            {
                eventValue = new TutorialMessageEventLog(null);
                eventValue.HelpFileName = AbilityRules.TranslateResourceString(GameText.s_tutorialText);
                if (AoWHSMap.Current.TextControl.FindId(55) != null)
                {
                    var id = AoWHSMap.Current.TextControl.FindId(55);
                    eventValue.Message.Assign(id.Text);
                    AoWHSMap.Current.TriggerExecuteEventLog(eventValue, true, null);
                }

                eventValue.Release();
            }
        }

        Dlg.Visible = false;
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
            if (ItemOffset + slot < Event.ItemIds.Count)
            {
                var itemId = Event.ItemIds[ItemOffset + slot];
                item = AoWHSMap.Current.ItemControl.FindItem(itemId);
                if (item != null)
                {
                    box.PopupMenu = DialogHosts.ItemBanner.IBannerPopup;
                    ItemBanner.s_itemId = item.ItemId;
                }
            }
        }
    }

    // AoWItemsFoundDlg.pas:155
    public void LeftBtnClick()
    {
        if (ItemOffset > 0)
        {
            ItemOffset -= 1;
            Dlg.Drawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:164
    public void RightBtnClick()
    {
        if (ItemOffset + 5 < Event.ItemIds.Count)
        {
            ItemOffset += 1;
            Dlg.Drawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:173
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowItemsFound(ItemsFoundDialog dialog, ItemExplorationSiteEventLog eventValue, ExecuteDoneEvent done)
    {
        ItemExplorationSiteEventLog log;
        dialog.Reset();
        dialog.OnDone = done;
        dialog.Event = eventValue;
        dialog.Event.AddRef();
        log = dialog.Event;
        dialog.TitleLbl.Text = log.Title;
        var text = log.Messages.Text;
        dialog.MessageMemo.Items.Text = text;
        dialog.LeftBtn.Visible = log.ItemIds.Count > 5;
        dialog.RightBtn.Visible = log.ItemIds.Count > 5;
        if (dialog.GetHero() == null)
        {
            dialog.OKPnl.Visible = true;
        }
        else
        {
            dialog.PickupPnl.Visible = true;
        }

        AoWHSMap.Current.TutorialControl.ShowMessage(54, true, true);
        dialog.Dlg.Drawn = false;
        dialog.Dlg.Visible = true;
    }

}
