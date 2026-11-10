// Translated from AoWSWindow.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWSWindow.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class TSWindow : TDataModule
{
    public TSWindow(TComponent owner) : base(owner)
    { }

    public TAOWWindow ScannerWindow;
    public TAoWFrame ScannerFrame;
    public TAOWTabPanel ScannerTab;
    public TAOWCheckBox BuildingCheck;
    public TAOWCheckBox ArmyCheck;
    public TAOWCheckBox NaturalCheck;
    public TScanner Scanner;
    public TAOWImage STitleIcon;
    public TAOWLabel STitleLabel;
    public TAOWButton SMinIcon;
    public TAOWButton Help;
    public TIvTranslator Translator;

    // AoWSWindow.pas:51
    public void ScannerTabChange()
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            switch (ScannerTab.Selected)
            {
                case 0:
                    Scanner.FUpdateStep = -1;
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewLevel(0);
                    break;
                case 1:
                    Scanner.FUpdateStep = -1;
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewLevel(1);
                    break;
                case 2:
                    Scanner.FUpdateStep = -1;
                    Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ViewLevel(2);
                    break;
            }
        }
    }

    // AoWSWindow.pas:61
    public void ArmyCheckChange(TAOWCheckBox Sender)
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
    public void BuildingCheckChange(TAOWCheckBox Sender)
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
    public void NaturalCheckChange(TAOWCheckBox Sender)
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
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer != -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTWidth(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTHeight(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight / 32);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTLeft(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneX / 32 - 6);
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetTTop(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneY / 32 - 6);
        }
    }

    // AoWSWindow.pas:107
    public void ScannerTabMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
    }

    // AoWSWindow.pas:110
    public void HelpClick()
    {
        Aow.Game.Ui.Dialogs.THelpWindow.ShowHelp(501);
    }
}
