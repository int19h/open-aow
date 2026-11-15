#nullable disable

using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

namespace Aow.Game.UI.Dialogs;

public partial class CombatScanner : UIModule
{
    public CombatScanner(UIContext ui) : base(ui)
    { }

    public Window CombatScannerWindow { get; private set; }
    public Scanner Scanner { get; private set; }

    // AoWTCScanner.pas:38
    public void ScannerChangeTileBox()
    {
        UI.MapWindow.MapViewer.SceneX = (Scanner.TLeft + 6) * 33;
        UI.MapWindow.MapViewer.SceneY = (Scanner.TTop + 6) * 33;
        if (StrategicMap.Current.SeatedPlayer != -1)
        {
            Scanner.SetTWidth(UI.MapWindow.MapViewer.SceneWidth / 32);
            Scanner.SetTHeight(UI.MapWindow.MapViewer.SceneHeight / 32);
            Scanner.SetTLeft(UI.MapWindow.MapViewer.SceneX / 32 - 6);
            Scanner.SetTTop(UI.MapWindow.MapViewer.SceneY / 32 - 6);
        }
    }

    // AoWTCScanner.pas:51
    public void TCScannerWinDrag()
    {
        CombatScannerWindow.Alignment.RightOffset = CombatScannerWindow.WindowManager.WinWidth - CombatScannerWindow.WinLeft - CombatScannerWindow.WinWidth;
        CombatScannerWindow.Alignment.TopOffset = CombatScannerWindow.WinTop;
    }

    // AoWTCScanner.pas:60
    public void SMinIconClick()
    {
        UI.General.PlayButtonSound(0);
        CombatScannerWindow.Visible = false;
    }
}
