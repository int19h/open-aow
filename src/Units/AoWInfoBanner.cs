// Translated from AoWInfoBanner.pas.
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
using Aow.Units.AOWMenu;
using static Aow.Units.AOWMenu.AOWMenu;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;

namespace Aow.Units.AoWInfoBanner
{
    // { TInfoBanner (instance size 84, VMT 00406F44) }
    public partial class TInfoBanner : Aow.Rtl.TDataModule
    {
        public TInfoBanner(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWLabel.TAOWLabel HeaderLbl;
        // { +0044 }
        public Aow.Units.AoWMemo.TAOWMemo InfoMemo;
        // { +0048 }
        public Aow.Units.AOWMenu.TAOWMenu BannerPopup;
        // { +004C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar InfoMemoSB;
        // AoWInfoBanner.pas:35
        // { +0050 }
        public void ShowInfo(Aow.Rtl.AnsiString Header, Aow.Rtl.TStrings Description)
        {
            HeaderLbl.Text = Header;
            var call__1 = Description.Text;
            InfoMemo.Items.Text = call__1;
        }

        // AoWInfoBanner.pas:41
        // { 0040701C }
        public void BannerPopupMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (Button == Aow.Rtl.TMouseButton.mbRight)
            {
                BannerPopup.Visible = false;
            }
        }
    }

    public static partial class AoWInfoBanner
    {
        public static Aow.Units.AoWInfoBanner.TInfoBanner InfoBanner;
    }
}
