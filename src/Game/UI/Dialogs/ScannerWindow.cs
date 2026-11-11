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
                    Scanner.FUpdateStep = -1;
                    AoWHSMap.Current.ViewLevel(0);
                    break;
                case 1:
                    Scanner.FUpdateStep = -1;
                    AoWHSMap.Current.ViewLevel(1);
                    break;
                case 2:
                    Scanner.FUpdateStep = -1;
                    AoWHSMap.Current.ViewLevel(2);
                    break;
            }
        }
    }

    // AoWSWindow.pas:61
    public void ArmyCheckChange(CheckBox Sender)
    {
        Scanner.SetArmies(ArmyCheck.Checked);
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:67
    public void BuildingCheckChange(CheckBox Sender)
    {
        Scanner.SetBuildings(BuildingCheck.Checked);
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:73
    public void NaturalCheckChange(CheckBox Sender)
    {
        Scanner.SetNaturals(NaturalCheck.Checked);
        if (Sender.Checked)
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
        this.MainWindow.Alignment.RightOffset = this.MainWindow.FWinManager.WinWidth - this.MainWindow.WinLeft - this.MainWindow.WinWidth;
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
        DialogHosts.MWindow.MapViewer.SceneX = (DialogHosts.SWindow.Scanner.FTLeft + 6) * 33;
        DialogHosts.MWindow.MapViewer.SceneY = (DialogHosts.SWindow.Scanner.FTTop + 6) * 33;
        if (AoWHSMap.Current.FSeatedPlayer != -1)
        {
            DialogHosts.SWindow.Scanner.SetTWidth(DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            DialogHosts.SWindow.Scanner.SetTHeight(DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            DialogHosts.SWindow.Scanner.SetTLeft(DialogHosts.MWindow.MapViewer.SceneX / 32 - 6);
            DialogHosts.SWindow.Scanner.SetTTop(DialogHosts.MWindow.MapViewer.SceneY / 32 - 6);
        }
    }

    // AoWSWindow.pas:107
    public void ScannerTabMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWSWindow.pas:110
    public void HelpClick()
    {
        HelpWindow.ShowHelp(501);
    }
}
