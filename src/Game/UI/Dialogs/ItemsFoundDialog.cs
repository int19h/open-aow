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
    public Image AOWImage1;
    public Image AOWImage2;
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
    public int FItemOffset;
    public ExecuteDoneEvent FOnDone;
    public ItemExplorationSiteEventLog FEvent;

    // AoWItemsFoundDlg.pas:80
    public Hero GetHero()
    {
        ArmyHexSprite Army;
        Hero Result = null;
        if (FEvent != null)
        {
            Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<MapCoordinate, int>(ref FEvent.FLocation))).GetArmyHS();
            if (Army != null && Army.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
            {
                Result = Army.FArmy.Hero(255, RosterTables.AllHeroTypes);
            }
        }

        return Result;
    }

    // AoWItemsFoundDlg.pas:66
    public void Finish()
    {
        ExecuteDoneEvent Done;
        ItemExplorationSiteEventLog Event;
        Dlg.Visible = false;
        Done = FOnDone;
        Event = FEvent;
        FOnDone = null;
        FEvent = null;
        if (Done != null)
        {
            Done(Event);
        }

        if (Event != null)
        {
            Event.Release();
        }
    }

    // AoWItemsFoundDlg.pas:62
    public void Reset()
    {
        FItemOffset = 0;
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
    public void Gnd3Draw(CheckBox Sender, LockedSurface SD, int Slot)
    {
        ItemExplorationSiteEventLog Event;
        Item Item;
        Event = FEvent;
        if (Event != null)
        {
            if (Slot + FItemOffset < Event.FItemIDs.Count)
            {
                var itemID = FEvent.FItemIDs[Slot + FItemOffset];
                Item = AoWHSMap.Current.FItemControl.FindItem(itemID);
                if (Item != null)
                {
                    Item.Show(Dlg.WinLeft + Sender.WinLeft, Dlg.WinTop + Sender.WinTop, SD);
                }
            }
        }
    }

    // AoWItemsFoundDlg.pas:110
    public void PickupBtnClick()
    {
        Hero Hero;
        TutorialMessageEventLog Event;
        Hero = GetHero();
        if (Hero != null)
        {
            DialogHosts.General.FSelectedUnit = Hero;
            DialogHosts.UnitWindow.ShowUnit();
            DialogHosts.UnitWindow.UnitTab.Selected = 2;
            if (AoWHSMap.Current.FGameType == GameType.Tutorial)
            {
                Event = new TutorialMessageEventLog(null);
                Event.FHelpFileName = AbilityRules.TranslateRStr(GameText.TutorialRStr);
                if (AoWHSMap.Current.FTextControl.FindID(55) != null)
                {
                    var id = AoWHSMap.Current.FTextControl.FindID(55);
                    Event.FMessage.Assign(id.FText);
                    AoWHSMap.Current.TriggerExecuteEventLog(Event, true, null);
                }

                Event.Release();
            }
        }

        Dlg.Visible = false;
    }

    // AoWItemsFoundDlg.pas:136
    public void Gnd3MouseDown(CheckBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        Item Item;
        CheckBox Box;
        Box = Sender;
        if (Button == MouseButton.Right)
        {
            Box.PopupMenu = null;
            if (FItemOffset + Slot < FEvent.FItemIDs.Count)
            {
                var itemID = FEvent.FItemIDs[FItemOffset + Slot];
                Item = AoWHSMap.Current.FItemControl.FindItem(itemID);
                if (Item != null)
                {
                    Box.PopupMenu = DialogHosts.ItemBanner.IBannerPopup;
                    ItemBanner.ItemID = Item.FItemID;
                }
            }
        }
    }

    // AoWItemsFoundDlg.pas:155
    public void LeftBtnClick()
    {
        if (FItemOffset > 0)
        {
            FItemOffset -= 1;
            Dlg.FDrawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:164
    public void RightBtnClick()
    {
        if (FItemOffset + 5 < FEvent.FItemIDs.Count)
        {
            FItemOffset += 1;
            Dlg.FDrawn = false;
        }
    }

    // AoWItemsFoundDlg.pas:173
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowItemsFound(ItemsFoundDialog Dialog, ItemExplorationSiteEventLog Event, ExecuteDoneEvent Done)
    {
        ItemExplorationSiteEventLog Log;
        Dialog.Reset();
        Dialog.FOnDone = Done;
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Log = Dialog.FEvent;
        Dialog.TitleLbl.Text = Log.FTitle;
        var text = Log.FMessages.Text;
        Dialog.MessageMemo.Items.Text = text;
        Dialog.LeftBtn.Visible = Log.FItemIDs.Count > 5;
        Dialog.RightBtn.Visible = Log.FItemIDs.Count > 5;
        if (Dialog.GetHero() == null)
        {
            Dialog.OKPnl.Visible = true;
        }
        else
        {
            Dialog.PickupPnl.Visible = true;
        }

        AoWHSMap.Current.FTutorialControl.ShowMessage(54, true, true);
        Dialog.Dlg.FDrawn = false;
        Dialog.Dlg.Visible = true;
    }

}
