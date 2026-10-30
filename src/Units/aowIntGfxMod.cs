// Translated from aowIntGfxMod.pas.
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
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;

namespace Aow.Units.aowIntGfxMod
{
    // { TIntGfxMod (instance size 252, VMT 004039E8) }
    public partial class TIntGfxMod : Aow.Rtl.TDataModule
    {
        public TIntGfxMod(TComponent owner) : base(owner)
        { }

        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericT;
        // { +0044 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericF;
        // { +0048 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL CeltDeco;
        // { +004C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Cursors;
        // { +0050 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanTiles;
        // { +0054 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanFog;
        // { +0058 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanHiLites;
        // { +005C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL PyreAnim;
        // { +0060 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL UnitBGs;
        // { +0064 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FCWin;
        // { +0068 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Menus;
        // { +006C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL DragonMenu;
        // { +0070 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanWin;
        // { +0074 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericI;
        // { +0078 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL UnitWin;
        // { +007C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL UnitIcons;
        // { +0080 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TurnButton;
        // { +0084 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TBWin;
        // { +0088 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL BookPL;
        // { +008C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL BookPR;
        // { +0090 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FactionWin;
        // { +0094 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL RelWin;
        // { +0098 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL WorldMap;
        // { +009C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL BookWin;
        // { +00A0 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TitleScreen;
        // { +00A4 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Shields;
        // { +00A8 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ProdWin;
        // { +00AC }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FireLg;
        // { +00B0 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL MiscArt;
        // { +00B4 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Units;
        // { +00B8 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL SoundWin;
        // { +00BC }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TitleIcons;
        // { +00C0 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL MoveIcons;
        // { +00C4 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL LBIcons;
        // { +00C8 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL EventIcons;
        // { +00CC }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ProdIcons;
        // { +00D0 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericB;
        // { +00D4 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericE;
        // { +00D8 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericS;
        // { +00DC }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL VictoryFrames;
        // { +00E0 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL EndScene;
        // { +00E4 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL PyreFrames;
        // { +00E8 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL CityWin;
        // { +00EC }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL CampaignTrans;
        // { +00F0 }
        public Aow.Units.ImageLib.TImageLibrary ScanObj;
        // { +00F4 }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL PlyrIcns;
    }

    public static partial class aowIntGfxMod
    {
        // { +00F8 }
        public static Aow.Units.aowIntGfxMod.TIntGfxMod IntGfxMod;
    }
}
