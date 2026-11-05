// Translated from AoWTargetSelection.pas.
using Aow.Rtl;
using Aow.Units.AoWArnoLine;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AoWTargetSelection;

public partial class TTargetSelectionDlg : TDataModule
{
    public TTargetSelectionDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAoWFrame CeltFrame;
    public TAOWButton Close;
    public TAOWLabel Title;
    public TAOWImage SIcon;
    public TAOWButton MainBtn;
    public TAOWMemo DefaultMemo;
    public TAOWPanel DefaultPnl;
    public TAOWLabel HeaderLbl;
    public TAOWPanel HeaderPnl;
    public TAoWArnoLine BorderImage;
    public TAOWPanel SpellPnl;
    public TAOWMemo SpellMemo;
    public TAOWLabel TargetRangeLabel;
    public TAOWLabel TargetLbl;
    public TIvTranslator Translator;
    // AoWTargetSelection.pas:45
    public void CloseClick()
    {
        General.PlayButtonSound(0);
        AoWHSMap.FTargetSelection.Deactivate();
    }

    // AoWTargetSelection.pas:51
    public void MainBtnClick()
    {
        CloseClick();
    }
}

public static partial class AoWTargetSelectionUnit
{
}
