// Translated from AoWItemsFoundDlg.pas.
using System.Runtime.CompilerServices;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// item offset, completion event, item event
public partial class ItemsFoundDlg : DataModule
{
    public ItemsFoundDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWLabel TitleLbl;
    public AOWPanel PickupPnl;
    public AOWButton PickupBtn;
    public AOWButton CloseBtn;
    public AoWFrame BCeltL;
    public AoWFrame TCeltL;
    public AOWImage TCeltLineL;
    public AOWImage AOWImage1;
    public AOWImage AOWImage2;
    public AOWButton CloseBtn2;
    public AOWPanel OKPnl;
    public AOWMemo MessageMemo;
    public AOWVScrollBar MessageMemoSB;
    public AOWPanel MemoPnl;
    public IvTranslator Translator;
    public AOWCheckBox Gnd3;
    public AOWCheckBox Gnd2;
    public AOWCheckBox Gnd1;
    public AOWCheckBox Gnd4;
    public AOWCheckBox Gnd5;
    public AOWButton LeftBtn;
    public AOWButton RightBtn;
    public int FItemOffset;
    public ExecuteDoneEvent FOnDone;
    public ItemExplorationSiteEventLog FEvent;

    // AoWItemsFoundDlg.pas:80
    public Hero GetHero()
    {
        ArmyHS Army;
        Hero Result = null;
        if (FEvent != null)
        {
            Army = ((AoWMapField)Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetFieldXYL(Unsafe.As<XYL, int>(ref FEvent.FLocation))).GetArmyHS();
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
    public void Gnd3Draw(AOWCheckBox Sender, SurfaceDesc SD, int Slot)
    {
        ItemExplorationSiteEventLog Event;
        Item Item;
        Event = FEvent;
        if (Event != null)
        {
            if (Slot + FItemOffset < Event.FItemIDs.Count)
            {
                var itemID = FEvent.FItemIDs[Slot + FItemOffset];
                Item = Aow.Game.World.Map.AoWHSMap.Current.FItemControl.FindItem(itemID);
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
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
            DialogHosts.UnitWindow.UnitTab.Selected = 2;
            if (AoWHSMap.Current.FGameType == GameType.Tutorial)
            {
                Event = new TutorialMessageEventLog(null);
                Event.FHelpFileName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(GameText.TutorialRStr);
                if (Aow.Game.World.Map.AoWHSMap.Current.FTextControl.FindID(55) != null)
                {
                    var id = Aow.Game.World.Map.AoWHSMap.Current.FTextControl.FindID(55);
                    Event.FMessage.Assign(id.FText);
                    Aow.Game.World.Map.AoWHSMap.Current.TriggerExecuteEventLog(Event, true, null);
                }

                Event.Release();
            }
        }

        Dlg.Visible = false;
    }

    // AoWItemsFoundDlg.pas:136
    public void Gnd3MouseDown(AOWCheckBox Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y, int Slot)
    {
        Item Item;
        AOWCheckBox Box;
        Box = Sender;
        if (Button == MouseButton.Right)
        {
            Box.PopupMenu = null;
            if (FItemOffset + Slot < FEvent.FItemIDs.Count)
            {
                var itemID = FEvent.FItemIDs[FItemOffset + Slot];
                Item = Aow.Game.World.Map.AoWHSMap.Current.FItemControl.FindItem(itemID);
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

    internal static void ShowItemsFound(ItemsFoundDlg Dialog, ItemExplorationSiteEventLog Event, ExecuteDoneEvent Done)
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

        Aow.Game.World.Map.AoWHSMap.Current.FTutorialControl.ShowMessage(54, true, true);
        Dialog.Dlg.FDrawn = false;
        Dialog.Dlg.Visible = true;
    }

}
