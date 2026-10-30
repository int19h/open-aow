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
    public partial class TIntGfxMod : Aow.Rtl.TDataModule
    {
        public TIntGfxMod(TComponent owner) : base(owner)
        { }

        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericT;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericF;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL CeltDeco;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Cursors;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanTiles;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanFog;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanHiLites;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL PyreAnim;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL UnitBGs;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FCWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Menus;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL DragonMenu;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ScanWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericI;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL UnitWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL UnitIcons;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TurnButton;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TBWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL BookPL;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL BookPR;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FactionWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL RelWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL WorldMap;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL BookWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TitleScreen;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Shields;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ProdWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FireLg;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL MiscArt;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL Units;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL SoundWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL TitleIcons;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL MoveIcons;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL LBIcons;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL EventIcons;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ProdIcons;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericB;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericE;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL GenericS;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL VictoryFrames;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL EndScene;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL PyreFrames;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL CityWin;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL CampaignTrans;
        public Aow.Units.ImageLib.TImageLibrary ScanObj;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL PlyrIcns;
    }

    public static partial class aowIntGfxMod
    {
        public static Aow.Units.aowIntGfxMod.TIntGfxMod IntGfxMod;
    }
}
