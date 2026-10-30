// Translated from AoWTCScanner.pas.
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
using Aow.Units.Scanner;
using static Aow.Units.Scanner.Scanner;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;

namespace Aow.Units.AoWTCScanner
{
    // { TTCScanner (instance size 96, VMT 00450958) }
    public partial class TTCScanner : Aow.Rtl.TDataModule
    {
        public TTCScanner(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow TCScannerWin;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame ScannerFr;
        // { +0048 }
        public Aow.Units.Scanner.TScanner Scanner;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage STitleIcon;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel STitleLabel;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton SMinIcon;
        // { +0058 }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWTCScanner.pas:38
        // { draft }
        // { +005C }
        public void ScannerChangeTileBox()
        {
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX = (Scanner.FTLeft + 6) * 33;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY = (Scanner.FTTop + 6) * 33;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != -1)
            {
                Scanner.SetTWidth(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneWidth / 32);
                Scanner.SetTHeight(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneHeight / 32);
                Scanner.SetTLeft((Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX / 32) - 6);
                Scanner.SetTTop((Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY / 32) - 6);
            }
        }

        // AoWTCScanner.pas:51
        // { draft }
        public void TCScannerWinDrag()
        {
            this.TCScannerWin.Alignment.RightOffset = (this.TCScannerWin.WinManager.WinWidth - this.TCScannerWin.WinLeft) - this.TCScannerWin.WinWidth;
            this.TCScannerWin.Alignment.TopOffset = this.TCScannerWin.WinTop;
        }

        // AoWTCScanner.pas:60
        // { draft }
        public void SMinIconClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            TCScannerWin.Visible = false;
        }
    }

    public static partial class AoWTCScanner
    {
    }
}
