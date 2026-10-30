// Translated from AoWSWindow.pas.
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
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.Scanner;
using static Aow.Units.Scanner.Scanner;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AOWMWindow;
using static Aow.Units.AOWMWindow.AOWMWindow;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;

namespace Aow.Units.AoWSWindow
{
    public partial class TSWindow : Aow.Rtl.TDataModule
    {
        public TSWindow(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow ScannerWindow;
        public Aow.Units.AoWFrame.TAoWFrame ScannerFrame;
        public Aow.Units.AoWTabPanel.TAOWTabPanel ScannerTab;
        public Aow.Units.AoWCheckBox.TAOWCheckBox BuildingCheck;
        public Aow.Units.AoWCheckBox.TAOWCheckBox ArmyCheck;
        public Aow.Units.AoWCheckBox.TAOWCheckBox NaturalCheck;
        public Aow.Units.Scanner.TScanner Scanner;
        public Aow.Units.AoWImage.TAOWImage STitleIcon;
        public Aow.Units.AoWLabel.TAOWLabel STitleLabel;
        public Aow.Units.AoWButton.TAOWButton SMinIcon;
        public Aow.Units.AoWButton.TAOWButton Help;
        public Aow.Rtl.TIvTranslator Translator;
        // AoWSWindow.pas:51
        public void ScannerTabChange()
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                switch (ScannerTab.Selected)
                {
                    case 0:
                        Scanner.FUpdateStep = -1;
                        Aow.Units.AoWE.AoWE.AoWHSMap.ViewLevel(0);
                        break;
                    case 1:
                        Scanner.FUpdateStep = -1;
                        Aow.Units.AoWE.AoWE.AoWHSMap.ViewLevel(1);
                        break;
                    case 2:
                        Scanner.FUpdateStep = -1;
                        Aow.Units.AoWE.AoWE.AoWHSMap.ViewLevel(2);
                        break;
                }
            }
        }

        // AoWSWindow.pas:61
        public void ArmyCheckChange(Aow.Units.AoWCheckBox.TAOWCheckBox Sender)
        {
            Scanner.SetArmies(ArmyCheck.Checked);
            if (Sender.Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSWindow.pas:67
        public void BuildingCheckChange(Aow.Units.AoWCheckBox.TAOWCheckBox Sender)
        {
            Scanner.SetBuildings(BuildingCheck.Checked);
            if (Sender.Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSWindow.pas:73
        public void NaturalCheckChange(Aow.Units.AoWCheckBox.TAOWCheckBox Sender)
        {
            Scanner.SetNaturals(NaturalCheck.Checked);
            if (Sender.Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWSWindow.pas:79
        public void ScannerWindowResize()
        {
            this.ScannerWindow.Alignment.RightOffset = (this.ScannerWindow.FWinManager.WinWidth - this.ScannerWindow.WinLeft) - this.ScannerWindow.WinWidth;
            this.ScannerWindow.Alignment.TopOffset = this.ScannerWindow.WinTop;
        }

        // AoWSWindow.pas:88
        public void SMinIconClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            ScannerWindow.Visible = false;
        }

        // AoWSWindow.pas:94
        public void ScannerChangeTileBox()
        {
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX = (Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FTLeft + 6) * 33;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY = (Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.FTTop + 6) * 33;
            if (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer != -1)
            {
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTWidth(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneWidth / 32);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTHeight(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneHeight / 32);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTLeft((Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX / 32) - 6);
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetTTop((Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY / 32) - 6);
            }
        }

        // AoWSWindow.pas:107
        public void ScannerTabMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWSWindow.pas:110
        public void HelpClick()
        {
            Aow.Units.aowHelpWin.aowHelpWin.ShowHelp(501);
        }
    }

    public static partial class AoWSWindow
    {
        public static Aow.Units.AoWSWindow.TSWindow SWindow;
    }
}
