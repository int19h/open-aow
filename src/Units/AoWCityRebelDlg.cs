// Translated from AoWCityRebelDlg.pas.
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
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWCityRebelDlg
{
    // completion callback. city-rebellion event
    public partial class TCityRebelDlg : Aow.Rtl.TDataModule
    {
        public TCityRebelDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow RebelWin;
        public Aow.Units.AoWLabel.TAOWLabel Title;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWButton.TAOWButton CenterBut;
        public Aow.Units.AoWButton.TAOWButton CloseBut;
        public Aow.Units.AoWImage.TAOWImage RebelBG;
        public Aow.Units.AoWLabel.TAOWLabel CityNameLabel;
        public Aow.Units.AoWLabel.TAOWLabel AOWLabel2;
        public Aow.Units.AoWLabel.TAOWLabel AOWLabel3;
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.EventLog.TExecuteDoneEvent FOnClose;
        public Aow.Units.City.TCityRebellionEventLog FEvent;
        // AoWCityRebelDlg.pas:49
        public void ClearEvent()
        {
            Aow.Units.City.TCityRebellionEventLog Event = default;
            Aow.Units.EventLog.TExecuteDoneEvent Notify = default;
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
        }

        // AoWCityRebelDlg.pas:60
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            RebelWin.Visible = false;
        }

        // AoWCityRebelDlg.pas:66
        public void CenterButClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            var cityRebellionEventLog = FEvent;
            Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(cityRebellionEventLog.FXhx, cityRebellionEventLog.FYhx, cityRebellionEventLog.FLhx);
        }

        // AoWCityRebelDlg.pas:72
        public void RebelWinHide()
        {
            ClearEvent();
        }

        // AoWCityRebelDlg.pas:77
        public void RebelWinKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if ((Key == 27) || (Key == 32))
            {
                RebelWin.Visible = false;
            }
        }
    }

    public static partial class AoWCityRebelDlg
    {
    }
}
