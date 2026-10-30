// Translated from AoWItemsFoundDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.ItemExpS;
using static Aow.Units.ItemExpS.ItemExpS;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWItemBanner;
using static Aow.Units.AoWItemBanner.AoWItemBanner;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;

namespace Aow.Units.AoWItemsFoundDlg
{
    // item offset, completion event, item event
    public partial class TItemsFoundDlg : Aow.Rtl.TDataModule
    {
        public TItemsFoundDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        public Aow.Units.AoWPanel.TAOWPanel PickupPnl;
        public Aow.Units.AoWButton.TAOWButton PickupBtn;
        public Aow.Units.AoWButton.TAOWButton CloseBtn;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        public Aow.Units.AoWButton.TAOWButton CloseBtn2;
        public Aow.Units.AoWPanel.TAOWPanel OKPnl;
        public Aow.Units.AoWMemo.TAOWMemo MessageMemo;
        public Aow.Units.AoWVScroll.TAOWVScrollBar MessageMemoSB;
        public Aow.Units.AoWPanel.TAOWPanel MemoPnl;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd3;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd2;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd1;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd4;
        public Aow.Units.AoWCheckBox.TAOWCheckBox Gnd5;
        public Aow.Units.AoWButton.TAOWButton LeftBtn;
        public Aow.Units.AoWButton.TAOWButton RightBtn;
        public int FItemOffset;
        public Aow.Units.EventLog.TExecuteDoneEvent FOnDone;
        public Aow.Units.ItemExpS.TItemExplorationSiteEventLog FEvent;
        // AoWItemsFoundDlg.pas:80
        public Aow.Units.AoWE.THero GetHero()
        {
            Aow.Units.AoWE.THero Result = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Result = null;
            if (FEvent != null)
            {
                var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetFieldXYL(Unsafe.As<Aow.Units.HSEngine.TXYL, int>(ref FEvent.FLocation));
                var call__2 = ((Aow.Units.AoWE.TAoWMapField)call__1).GetArmyHS();
                Army = call__2;
                if ((Army != null) && (Army.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer))
                {
                    var call__3 = Army.FArmy.Hero(255, Aow.Units.E2CampaignSupport.E2CampaignSupport.AllHeroTypes);
                    Result = call__3;
                }
            }

            return Result;
        }

        // AoWItemsFoundDlg.pas:66
        public void Finish()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.ItemExpS.TItemExplorationSiteEventLog Event = default;
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
        public void Gnd3Draw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.ItemExpS.TItemExplorationSiteEventLog Event = default;
            Aow.Units.AoWE.TItem Item = default;
            Event = FEvent;
            if (Event != null)
            {
                if ((((Aow.Rtl.TComponent)Sender).Tag + FItemOffset) < Event.FItemIDs.Count)
                {
                    var call__4 = FEvent.FItemIDs[((Aow.Rtl.TComponent)Sender).Tag + FItemOffset];
                    var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.FindItem(call__4);
                    Item = call__5;
                    if (Item != null)
                    {
                        Item.Show(Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, SD);
                    }
                }
            }
        }

        // AoWItemsFoundDlg.pas:110
        public void PickupBtnClick()
        {
            Aow.Units.AoWE.THero Hero = default;
            Aow.Units.EventLog.TTutorialMessageEventLog Event = default;
            Hero = GetHero();
            if (Hero != null)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit = Hero;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitTab.Selected = 2;
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FGameType == Aow.Units.AoWE.TGameType.gtTutorial)
                {
                    Event = new Aow.Units.EventLog.TTutorialMessageEventLog(null);
                    var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TutorialRStr);
                    Event.FHelpFileName = call__6;
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FTextControl.FindID(55);
                    if (call__7 != null)
                    {
                        var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FTextControl.FindID(55);
                        Event.FMessage.Assign(call__8.FText);
                        Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(Event, true, null);
                    }

                    Event.Release();
                }
            }

            Dlg.Visible = false;
        }

        // AoWItemsFoundDlg.pas:136
        public void Gnd3MouseDown(Aow.Units.AoWCheckBox.TAOWCheckBox Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TItem Item = default;
            Aow.Units.AoWCheckBox.TAOWCheckBox Box = default;
            Box = Sender;
            if (Button == Aow.Rtl.TMouseButton.mbRight)
            {
                Box.PopupMenu = null;
                if ((FItemOffset + Box.Tag) < FEvent.FItemIDs.Count)
                {
                    var call__9 = FEvent.FItemIDs[FItemOffset + Box.Tag];
                    var call__10 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.FindItem(call__9);
                    Item = call__10;
                    if (Item != null)
                    {
                        Box.PopupMenu = Aow.Units.AoWItemBanner.AoWItemBanner.ItemBanner.IBannerPopup;
                        Aow.Units.AoWItemBanner.AoWItemBanner.ItemID = Item.FItemID;
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
            if ((FItemOffset + 5) < FEvent.FItemIDs.Count)
            {
                FItemOffset += 1;
                Dlg.FDrawn = false;
            }
        }

        // AoWItemsFoundDlg.pas:173
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
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
