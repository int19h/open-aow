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
public partial class TItemsFoundDlg : TDataModule
{
    public TItemsFoundDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel TitleLbl;
    public TAOWPanel PickupPnl;
    public TAOWButton PickupBtn;
    public TAOWButton CloseBtn;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWImage AOWImage1;
    public TAOWImage AOWImage2;
    public TAOWButton CloseBtn2;
    public TAOWPanel OKPnl;
    public TAOWMemo MessageMemo;
    public TAOWVScrollBar MessageMemoSB;
    public TAOWPanel MemoPnl;
    public TIvTranslator Translator;
    public TAOWCheckBox Gnd3;
    public TAOWCheckBox Gnd2;
    public TAOWCheckBox Gnd1;
    public TAOWCheckBox Gnd4;
    public TAOWCheckBox Gnd5;
    public TAOWButton LeftBtn;
    public TAOWButton RightBtn;
    public int FItemOffset;
    public TExecuteDoneEvent FOnDone;
    public TItemExplorationSiteEventLog FEvent;

    // AoWItemsFoundDlg.pas:80
    public THero GetHero()
    {
        TArmyHS Army;
        THero Result = null;
        if (FEvent != null)
        {
            Army = ((TAoWMapField)Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetFieldXYL(Unsafe.As<TXYL, int>(ref FEvent.FLocation))).GetArmyHS();
            if (Army != null && Army.FArmy.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer)
            {
                Result = Army.FArmy.Hero(255, Aow.Game.Magic.Abilities.RosterTables.AllHeroTypes);
            }
        }

        return Result;
    }

    // AoWItemsFoundDlg.pas:66
    public void Finish()
    {
        TExecuteDoneEvent Done;
        TItemExplorationSiteEventLog Event;
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
    public void Gnd3Draw(TAOWCheckBox Sender, TSurfaceDesc SD, int Slot)
    {
        TItemExplorationSiteEventLog Event;
        TItem Item;
        Event = FEvent;
        if (Event != null)
        {
            if (Slot + FItemOffset < Event.FItemIDs.Count)
            {
                var itemID = FEvent.FItemIDs[Slot + FItemOffset];
                Item = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FItemControl.FindItem(itemID);
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
        THero Hero;
        TTutorialMessageEventLog Event;
        Hero = GetHero();
        if (Hero != null)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = Hero;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitTab.Selected = 2;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGameType == TGameType.gtTutorial)
            {
                Event = new TTutorialMessageEventLog(null);
                Event.FHelpFileName = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TutorialRStr);
                if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTextControl.FindID(55) != null)
                {
                    var id = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTextControl.FindID(55);
                    Event.FMessage.Assign(id.FText);
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.TriggerExecuteEventLog(Event, true, null);
                }

                Event.Release();
            }
        }

        Dlg.Visible = false;
    }

    // AoWItemsFoundDlg.pas:136
    public void Gnd3MouseDown(TAOWCheckBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y, int Slot)
    {
        TItem Item;
        TAOWCheckBox Box;
        Box = Sender;
        if (Button == TMouseButton.mbRight)
        {
            Box.PopupMenu = null;
            if (FItemOffset + Slot < FEvent.FItemIDs.Count)
            {
                var itemID = FEvent.FItemIDs[FItemOffset + Slot];
                Item = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FItemControl.FindItem(itemID);
                if (Item != null)
                {
                    Box.PopupMenu = Aow.Game.Ui.Dialogs.DialogHosts.ItemBanner.IBannerPopup;
                    Aow.Game.Ui.Dialogs.TItemBanner.ItemID = Item.FItemID;
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
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowItemsFound(TItemsFoundDlg Dialog, TItemExplorationSiteEventLog Event, TExecuteDoneEvent Done)
    {
        TItemExplorationSiteEventLog Log;
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

        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTutorialControl.ShowMessage(54, true, true);
        Dialog.Dlg.FDrawn = false;
        Dialog.Dlg.Visible = true;
    }

}
