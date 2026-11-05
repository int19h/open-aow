// Translated from AoWTCScanner.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWWindow;
using Aow.Units.Scanner;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWTCScanner
{
    public partial class TTCScanner : TDataModule
    {
        public TTCScanner(TComponent owner) : base(owner)
        { }

        public TAOWWindow TCScannerWin;
        public TAoWFrame ScannerFr;
        public TScanner Scanner;
        public TAOWImage STitleIcon;
        public TAOWLabel STitleLabel;
        public TAOWButton SMinIcon;
        public TIvTranslator Translator;
        // AoWTCScanner.pas:38
        public void ScannerChangeTileBox()
        {
            AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX = (Scanner.FTLeft + 6) * 33;
            AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY = (Scanner.FTTop + 6) * 33;
            if (AoWHSMap.FSeatedPlayer != -1)
            {
                Scanner.SetTWidth(AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneWidth / 32);
                Scanner.SetTHeight(AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneHeight / 32);
                Scanner.SetTLeft(AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneX / 32 - 6);
                Scanner.SetTTop(AOWMWindow.AOWMWindow.MWindow.MapViewer.SceneY / 32 - 6);
            }
        }

        // AoWTCScanner.pas:51
        public void TCScannerWinDrag()
        {
            this.TCScannerWin.Alignment.RightOffset = this.TCScannerWin.WinManager.WinWidth - this.TCScannerWin.WinLeft - this.TCScannerWin.WinWidth;
            this.TCScannerWin.Alignment.TopOffset = this.TCScannerWin.WinTop;
        }

        // AoWTCScanner.pas:60
        public void SMinIconClick()
        {
            General.PlayButtonSound(0);
            TCScannerWin.Visible = false;
        }
    }

    public static partial class AoWTCScanner
    {
    }
}
