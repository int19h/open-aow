// Translated from AoWSWindow.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWWindow;
using Aow.Units.Scanner;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.AOWMWindow.AOWMWindowUnit;
using static Aow.Units.aowHelpWin.aowHelpWinUnit;

namespace Aow.Units.AoWSWindow;

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
        if (AoWHSMap != null)
        {
            switch (ScannerTab.Selected)
            {
                case 0:
                    Scanner.FUpdateStep = -1;
                    AoWHSMap.ViewLevel(0);
                    break;
                case 1:
                    Scanner.FUpdateStep = -1;
                    AoWHSMap.ViewLevel(1);
                    break;
                case 2:
                    Scanner.FUpdateStep = -1;
                    AoWHSMap.ViewLevel(2);
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
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:67
    public void BuildingCheckChange(TAOWCheckBox Sender)
    {
        Scanner.SetBuildings(BuildingCheck.Checked);
        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }
    }

    // AoWSWindow.pas:73
    public void NaturalCheckChange(TAOWCheckBox Sender)
    {
        Scanner.SetNaturals(NaturalCheck.Checked);
        if (Sender.Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
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
        General.PlayButtonSound(0);
        ScannerWindow.Visible = false;
    }

    // AoWSWindow.pas:94
    public void ScannerChangeTileBox()
    {
        MWindow.MapViewer.SceneX = (AoWSWindowUnit.SWindow.Scanner.FTLeft + 6) * 33;
        MWindow.MapViewer.SceneY = (AoWSWindowUnit.SWindow.Scanner.FTTop + 6) * 33;
        if (AoWHSMap.FSeatedPlayer != -1)
        {
            AoWSWindowUnit.SWindow.Scanner.SetTWidth(MWindow.MapViewer.SceneWidth / 32);
            AoWSWindowUnit.SWindow.Scanner.SetTHeight(MWindow.MapViewer.SceneHeight / 32);
            AoWSWindowUnit.SWindow.Scanner.SetTLeft(MWindow.MapViewer.SceneX / 32 - 6);
            AoWSWindowUnit.SWindow.Scanner.SetTTop(MWindow.MapViewer.SceneY / 32 - 6);
        }
    }

    // AoWSWindow.pas:107
    public void ScannerTabMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        General.PlayButtonSound(0);
    }

    // AoWSWindow.pas:110
    public void HelpClick()
    {
        ShowHelp(501);
    }
}

public static partial class AoWSWindowUnit
{
    public static TSWindow SWindow;
}
