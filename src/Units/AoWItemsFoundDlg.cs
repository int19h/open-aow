// Translated from AoWItemsFoundDlg.pas.
using Aow.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.ItemExpS;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWItemBanner.AoWItemBanner;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;

namespace Aow.Units.AoWItemsFoundDlg
{
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
            THero Result = default;
            TArmyHS Army = default;
            Result = null;
            if (FEvent != null)
            {
                var call__1 = AoWHSMap.MapContainer.GetFieldXYL(Unsafe.As<TXYL, int>(ref FEvent.FLocation));
                var call__2 = ((TAoWMapField)call__1).GetArmyHS();
                Army = call__2;
                if (Army != null && Army.FArmy.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    var call__3 = Army.FArmy.Hero(255, AllHeroTypes);
                    Result = call__3;
                }
            }

            return Result;
        }

        // AoWItemsFoundDlg.pas:66
        public void Finish()
        {
            TExecuteDoneEvent Done = default;
            TItemExplorationSiteEventLog Event = default;
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
        public void Gnd3Draw(TObject Sender, TSurfaceDesc SD)
        {
            TItemExplorationSiteEventLog Event = default;
            TItem Item = default;
            Event = FEvent;
            if (Event != null)
            {
                if (((TComponent)Sender).Tag + FItemOffset < Event.FItemIDs.Count)
                {
                    var call__4 = FEvent.FItemIDs[((TComponent)Sender).Tag + FItemOffset];
                    var call__5 = AoWHSMap.FItemControl.FindItem(call__4);
                    Item = call__5;
                    if (Item != null)
                    {
                        Item.Show(Dlg.WinLeft + ((TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((TAoWComponent)Sender).WinTop, SD);
                    }
                }
            }
        }

        // AoWItemsFoundDlg.pas:110
        public void PickupBtnClick()
        {
            THero Hero = default;
            TTutorialMessageEventLog Event = default;
            Hero = GetHero();
            if (Hero != null)
            {
                General.FSelectedUnit = Hero;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected = 2;
                if (AoWHSMap.FGameType == TGameType.gtTutorial)
                {
                    Event = new TTutorialMessageEventLog(null);
                    var call__6 = TranslateRStr(TutorialRStr);
                    Event.FHelpFileName = call__6;
                    var call__7 = AoWHSMap.FTextControl.FindID(55);
                    if (call__7 != null)
                    {
                        var call__8 = AoWHSMap.FTextControl.FindID(55);
                        Event.FMessage.Assign(call__8.FText);
                        AoWHSMap.TriggerExecuteEventLog(Event, true, null);
                    }

                    Event.Release();
                }
            }

            Dlg.Visible = false;
        }

        // AoWItemsFoundDlg.pas:136
        public void Gnd3MouseDown(TAOWCheckBox Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TItem Item = default;
            TAOWCheckBox Box = default;
            Box = Sender;
            if (Button == TMouseButton.mbRight)
            {
                Box.PopupMenu = null;
                if (FItemOffset + Box.Tag < FEvent.FItemIDs.Count)
                {
                    var call__9 = FEvent.FItemIDs[FItemOffset + Box.Tag];
                    var call__10 = AoWHSMap.FItemControl.FindItem(call__9);
                    Item = call__10;
                    if (Item != null)
                    {
                        Box.PopupMenu = ItemBanner.IBannerPopup;
                        ItemID = Item.FItemID;
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
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWItemsFoundDlg
    {
    }
}
