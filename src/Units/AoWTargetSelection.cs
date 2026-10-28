// Translated from AoWTargetSelection.pas.
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
using Aow.Units.AoWArnoLine;
using static Aow.Units.AoWArnoLine.AoWArnoLine;
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWTargetSelection
{
    // { TTargetSelectionDlg (instance size 132, VMT 004445D8) }
    public partial class TTargetSelectionDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWFrame.TAoWFrame CeltFrame;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage SIcon;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton MainBtn;
        // { +0058 }
        public Aow.Units.AoWMemo.TAOWMemo DefaultMemo;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel DefaultPnl;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel HeaderLbl;
        // { +0064 }
        public Aow.Units.AoWPanel.TAOWPanel HeaderPnl;
        // { +0068 }
        public Aow.Units.AoWArnoLine.TAoWArnoLine BorderImage;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel SpellPnl;
        // { +0070 }
        public Aow.Units.AoWMemo.TAOWMemo SpellMemo;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel TargetRangeLabel;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel TargetLbl;
        // { +007C }
        public Aow.Rtl.TIvTranslator Translator;
        // AoWTargetSelection.pas:45
        // { +0080 }
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWE.AoWE.AoWHSMap.FTargetSelection.Deactivate();
        }

        // AoWTargetSelection.pas:51
        public void MainBtnClick()
        {
            CloseClick();
        }
    }

    public static partial class AoWTargetSelection
    {
    }
}
