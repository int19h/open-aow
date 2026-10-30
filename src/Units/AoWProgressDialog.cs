// Translated from AoWProgressDialog.pas.
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
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;

namespace Aow.Units.AoWProgressDialog
{
    // { TProgressDialog (instance size 108, VMT 004131A0) }
    public partial class TProgressDialog : Aow.Rtl.TDataModule
    {
        public TProgressDialog(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWLabel.TAOWLabel StatusLbl;
        // { +0050 }
        public Aow.Units.AoWWindow.TAOWWindow ProgDlg;
        // { +0054 }
        public Aow.Units.AOWGauge.TAoWGauge Bar;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        // { +0064 }
        public Aow.Rtl.TIvTranslator Translator;
    }

    public static partial class AoWProgressDialog
    {
    }
}
