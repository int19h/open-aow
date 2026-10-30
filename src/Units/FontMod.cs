// Translated from FontMod.pas.
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

namespace Aow.Units.FontMod
{
    // { TFontModule (instance size 116, VMT 0040380C) }
    public partial class TFontModule : Aow.Rtl.TDataModule
    {
        public TFontModule(TComponent owner) : base(owner)
        { }

        public Aow.Units.ImageLib.TImageLibraryFont Age8;
        // { +0044 }
        public Aow.Units.ImageLib.TImageLibraryFont Age10;
        // { +0048 }
        public Aow.Units.ImageLib.TImageLibraryFont Age16s;
        // { +004C }
        public Aow.Units.ImageLib.TImageLibraryFont Age10s;
        // { +0050 }
        public Aow.Units.ImageLib.TImageLibraryFont Age8s;
        // { +0054 }
        public Aow.Units.ImageLib.TImageLibraryFont Sessis14G;
        // { +0058 }
        public Aow.Units.ImageLib.TImageLibraryFont AoW15WhiteGrey;
        // { +005C }
        public Aow.Units.ImageLib.TImageLibraryFont AoW15BlackBrown;
        // { +0060 }
        public Aow.Units.ImageLib.TImageLibraryFont Age10LB;
        // { +0064 }
        public Aow.Units.ImageLib.TImageLibraryFont Age10b;
        // { +0068 }
        public Aow.Units.ImageLib.TImageLibraryFont Age10bs;
        // { +006C }
        public Aow.Units.ImageLib.TImageLibraryFont TBarNumber;
    }

    public static partial class FontMod
    {
    }
}
