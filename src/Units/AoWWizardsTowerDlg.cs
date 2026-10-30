// Translated from AoWWizardsTowerDlg.pas.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.WizardsTower;
using static Aow.Units.WizardsTower.WizardsTower;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
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
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWWizardsTowerDlg
{
    // { TWizardsTowerDlg (instance size $94, VMT 0044B6EC).
//     $88 completion callback; $90 tower-entry event. }
    public partial class TWizardsTowerDlg : Aow.Rtl.TDataModule
    {
        public TWizardsTowerDlg(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton BuyBtn;
        // { +0058 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel ResearchPnl;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel SpellName;
        // { +0064 }
        public Aow.Units.AoWImage.TAOWImage SpellIcn;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel SpellClass;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel SpellCost;
        // { +0070 }
        public Aow.Units.AoWMemo.TAOWMemo ResearchMemo;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel PriceLbl;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel OfferText;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel PicPnl;
        // { +0080 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0084 }
        public Aow.Units.EventLog.TExecuteDoneEvent FOnClose;
        // { $88 }
        public Aow.Units.WizardsTower.TEnterWizardsTowerEventLog FEvent;
        // AoWWizardsTowerDlg.pas:54
        // { $90 }
        public void ClearEvent()
        {
            Aow.Units.WizardsTower.TEnterWizardsTowerEventLog Event = default;
            Aow.Units.EventLog.TExecuteDoneEvent Notify = default;
            Dlg.Visible = false;
            Notify = FOnClose;
            Event = FEvent;
            FOnClose = null;
            FEvent = null;
            if (Notify != null)
            {
                Notify(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }

            Aow.Units.AoWE.AoWE.AoWHSMap.FTutorialControl.ShowMessage(69, true, true);
        }

        // AoWWizardsTowerDlg.pas:67
        public void CancelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWWizardsTowerDlg.pas:73
        // { draft }
        public void BuyBtnClick()
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var call__1 = FEvent.FTower.Buy(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), ref Reason);
            if (!call__1)
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
            }
            else
            {
                Dlg.Visible = false;
            }
        }

        // AoWWizardsTowerDlg.pas:82
        // { draft }
        public void DlgHide()
        {
            ClearEvent();
        }

        // AoWWizardsTowerDlg.pas:87
        // { draft }
        public void SpellIcnDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TSpell S = default;
            int X = default;
            int Y = default;
            var wizardsTower = FEvent.FTower;
            if (wizardsTower.FSpellID != 0)
            {
                var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(wizardsTower.FSpellID);
                S = call__3;
                if (S != null)
                {
                    var call__4 = S.FCastImages.GetImageSequence(10);
                    if (call__4 != null)
                    {
                        var image = (Aow.Units.AoWImage.TAOWImage)Sender;
                        X = image.AOWWindow.WinLeft;
                        Y = image.AOWWindow.WinTop;
                        S.FCastImages.Show(10, image.WinLeft + X, image.WinTop + Y, 0, SD);
                    }
                }
            }
        }

        // AoWWizardsTowerDlg.pas:101
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWWizardsTowerDlg
    {
    }
}
