using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class ScannerWindow : DataModule
{
    public ScannerWindow(Component owner) : base(owner)
    { }

    public Window MainWindow;
    public Frame ScannerFrame;
    public TabPanel ScannerTab;
    public CheckBox BuildingCheck;
    public CheckBox ArmyCheck;
    public CheckBox NaturalCheck;
    public Scanner Scanner;
    public Image STitleIcon;
    public Label STitleLabel;
    public Button SMinIcon;
    public Button Help;
    public IvTranslator Translator;

    // AoWSWindow.pas:51
    public void ScannerTabChange()
    {
        if (AoWHSMap.Current != null)
        {
            switch (ScannerTab.Selected)
            {
                case 0:
                    Scanner.UpdateStep = -1;
                    AoWHSMap.Current.ViewLevel(0);
                    break;
                case 1:
                    Scanner.UpdateStep = -1;
                    AoWHSMap.Current.ViewLevel(1);
                    break;
                case 2:
                    Scanner.UpdateStep = -1;
                    AoWHSMap.Current.ViewLevel(2);
                    break;
            }
        }
    }

    // AoWSWindow.pas:61
    public void ArmyCheckChange(CheckBox sender)
    {
        Scanner.SetArmies(ArmyCheck.Checked);
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:67
    public void BuildingCheckChange(CheckBox sender)
    {
        Scanner.SetBuildings(BuildingCheck.Checked);
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:73
    public void NaturalCheckChange(CheckBox sender)
    {
        Scanner.SetNaturals(NaturalCheck.Checked);
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:79
    public void ScannerWindowResize()
    {
        this.MainWindow.Alignment.RightOffset = this.MainWindow._windowManager.WinWidth - this.MainWindow.WinLeft - this.MainWindow.WinWidth;
        this.MainWindow.Alignment.TopOffset = this.MainWindow.WinTop;
    }

    // AoWSWindow.pas:88
    public void SMinIconClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AoWSWindow.pas:94
    public void ScannerChangeTileBox()
    {
        DialogHosts.MapWindow.MapViewer.SceneX = (DialogHosts.ScannerWindow.Scanner.TLeft + 6) * 33;
        DialogHosts.MapWindow.MapViewer.SceneY = (DialogHosts.ScannerWindow.Scanner.TTop + 6) * 33;
        if (AoWHSMap.Current.SeatedPlayer != -1)
        {
            DialogHosts.ScannerWindow.Scanner.SetTWidth(DialogHosts.MapWindow.MapViewer.SceneWidth / 32);
            DialogHosts.ScannerWindow.Scanner.SetTHeight(DialogHosts.MapWindow.MapViewer.SceneHeight / 32);
            DialogHosts.ScannerWindow.Scanner.SetTLeft(DialogHosts.MapWindow.MapViewer.SceneX / 32 - 6);
            DialogHosts.ScannerWindow.Scanner.SetTTop(DialogHosts.MapWindow.MapViewer.SceneY / 32 - 6);
        }
    }

    // AoWSWindow.pas:107
    public void ScannerTabMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWSWindow.pas:110
    public void HelpClick()
    {
        HelpWindow.ShowHelp(501);
    }
}
