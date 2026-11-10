// Translated from AoWTargetSelection.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWTargetSelection.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTargetSelection.Deactivate();
    }

    // AoWTargetSelection.pas:51
    public void MainBtnClick()
    {
        CloseClick();
    }
}
