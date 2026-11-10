// Translated from AoWSWindow.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSWindow.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class SWindow : DataModule
{
    public SWindow(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWWindow ScannerWindow;
    public AoWFrame ScannerFrame;
    public AOWTabPanel ScannerTab;
    public AOWCheckBox BuildingCheck;
    public AOWCheckBox ArmyCheck;
    public AOWCheckBox NaturalCheck;
    public global::Aow.Game.Ui.Grids.Scanner Scanner;
    public AOWImage STitleIcon;
    public AOWLabel STitleLabel;
    public AOWButton SMinIcon;
    public AOWButton Help;
    public IvTranslator Translator;

    // AoWSWindow.pas:51
    public void ScannerTabChange()
    {
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
        {
            switch (ScannerTab.Selected)
            {
                case 0:
                    Scanner.FUpdateStep = -1;
                    Aow.Game.World.Map.AoWHSMap.Current.ViewLevel(0);
                    break;
                case 1:
                    Scanner.FUpdateStep = -1;
                    Aow.Game.World.Map.AoWHSMap.Current.ViewLevel(1);
                    break;
                case 2:
                    Scanner.FUpdateStep = -1;
                    Aow.Game.World.Map.AoWHSMap.Current.ViewLevel(2);
                    break;
            }
        }
    }

    // AoWSWindow.pas:61
    public void ArmyCheckChange(AOWCheckBox Sender)
    {
        Scanner.SetArmies(ArmyCheck.Checked);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:67
    public void BuildingCheckChange(AOWCheckBox Sender)
    {
        Scanner.SetBuildings(BuildingCheck.Checked);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:73
    public void NaturalCheckChange(AOWCheckBox Sender)
    {
        Scanner.SetNaturals(NaturalCheck.Checked);
        if (Sender.Checked)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:79
    public void ScannerWindowResize()
    {
        this.ScannerWindow.Alignment.RightOffset = this.ScannerWindow.FWinManager.WinWidth - this.ScannerWindow.WinLeft - this.ScannerWindow.WinWidth;
        this.ScannerWindow.Alignment.TopOffset = this.ScannerWindow.WinTop;
    }

    // AoWSWindow.pas:88
    public void SMinIconClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        ScannerWindow.Visible = false;
    }

    // AoWSWindow.pas:94
    public void ScannerChangeTileBox()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX = (Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FTLeft + 6) * 33;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY = (Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FTTop + 6) * 33;
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer != -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTWidth(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTHeight(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTLeft(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX / 32 - 6);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTTop(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY / 32 - 6);
        }
    }

    // AoWSWindow.pas:107
    public void ScannerTabMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWSWindow.pas:110
    public void HelpClick()
    {
        Aow.Game.Ui.Dialogs.HelpWindow.ShowHelp(501);
    }
}
