#nullable disable
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class ScannerWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public TabPanel ScannerTabs { get; private set; }
    public CheckBox BuildingCheckBox { get; private set; }
    public CheckBox ArmyCheckBox { get; private set; }
    public CheckBox NaturalCheckBox { get; private set; }
    public Scanner Scanner { get; private set; }
    public Label TitleLabel { get; private set; }

    // AoWSWindow.pas:51
    public void ScannerTabChange()
    {
        if (StrategicMap.Current is not null)
        {
            switch (ScannerTabs.Selected)
            {
                case 0:
                    Scanner.UpdateStep = -1;
                    StrategicMap.Current.ViewLevel(0);
                    break;
                case 1:
                    Scanner.UpdateStep = -1;
                    StrategicMap.Current.ViewLevel(1);
                    break;
                case 2:
                    Scanner.UpdateStep = -1;
                    StrategicMap.Current.ViewLevel(2);
                    break;
                default:
                    break;
            }
        }
    }

    // AoWSWindow.pas:61
    public void ArmyCheckChange(CheckBox sender)
    {
        Scanner.SetArmies(ArmyCheckBox.Checked);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:67
    public void BuildingCheckChange(CheckBox sender)
    {
        Scanner.SetBuildings(BuildingCheckBox.Checked);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:73
    public void NaturalCheckChange(CheckBox sender)
    {
        Scanner.SetNaturals(NaturalCheckBox.Checked);
        if (sender.Checked)
        {
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:79
    public void ScannerWindowResize()
    {
        MainWindow.Alignment.RightOffset = MainWindow.WindowManager.WinWidth - MainWindow.WinLeft - MainWindow.WinWidth;
        MainWindow.Alignment.TopOffset = MainWindow.WinTop;
    }

    // AoWSWindow.pas:88
    public void SMinIconClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWSWindow.pas:94
    public void ScannerChangeTileBox()
    {
        UI.MapWindow.MapViewer.SceneX = (UI.ScannerWindow.Scanner.TLeft + 6) * 33;
        UI.MapWindow.MapViewer.SceneY = (UI.ScannerWindow.Scanner.TTop + 6) * 33;
        if (StrategicMap.Current.SeatedPlayer != -1)
        {
            UI.ScannerWindow.Scanner.SetTWidth(UI.MapWindow.MapViewer.SceneWidth / 32);
            UI.ScannerWindow.Scanner.SetTHeight(UI.MapWindow.MapViewer.SceneHeight / 32);
            UI.ScannerWindow.Scanner.SetTLeft((UI.MapWindow.MapViewer.SceneX / 32) - 6);
            UI.ScannerWindow.Scanner.SetTTop((UI.MapWindow.MapViewer.SceneY / 32) - 6);
        }
    }

    // AoWSWindow.pas:107
    public void ScannerTabMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y) => UI.General.PlayButtonSound(0);

    // AoWSWindow.pas:110
    public void HelpClick() => UI.HelpWindow.ShowHelp(501);
}
