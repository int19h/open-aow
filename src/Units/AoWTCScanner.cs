// Translated from AoWTCScanner.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWTCScanner.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class TCScanner : DataModule
{
    public TCScanner(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWWindow TCScannerWin;
    public AoWFrame ScannerFr;
    public global::Aow.Game.Ui.Grids.Scanner Scanner;
    public AOWImage STitleIcon;
    public AOWLabel STitleLabel;
    public AOWButton SMinIcon;
    public IvTranslator Translator;

    // AoWTCScanner.pas:38
    public void ScannerChangeTileBox()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX = (Scanner.FTLeft + 6) * 33;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY = (Scanner.FTTop + 6) * 33;
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer != -1)
        {
            Scanner.SetTWidth(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            Scanner.SetTHeight(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            Scanner.SetTLeft(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX / 32 - 6);
            Scanner.SetTTop(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY / 32 - 6);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        TCScannerWin.Visible = false;
    }
}
