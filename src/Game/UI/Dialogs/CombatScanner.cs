using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class CombatScanner : DataModule
{
    public CombatScanner(Component owner) : base(owner)
    { }

    public Window TCScannerWin;
    public Frame ScannerFr;
    public Scanner Scanner;
    public Image STitleIcon;
    public Label STitleLabel;
    public Button SMinIcon;
    public IvTranslator Translator;

    // AoWTCScanner.pas:38
    public void ScannerChangeTileBox()
    {
        DialogHosts.MWindow.MapViewer.SceneX = (Scanner.FTLeft + 6) * 33;
        DialogHosts.MWindow.MapViewer.SceneY = (Scanner.FTTop + 6) * 33;
        if (AoWHSMap.Current.FSeatedPlayer != -1)
        {
            Scanner.SetTWidth(DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            Scanner.SetTHeight(DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            Scanner.SetTLeft(DialogHosts.MWindow.MapViewer.SceneX / 32 - 6);
            Scanner.SetTTop(DialogHosts.MWindow.MapViewer.SceneY / 32 - 6);
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
        DialogHosts.General.PlayButtonSound(0);
        TCScannerWin.Visible = false;
    }
}
