// Translated from AoWTargetSelection.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWTargetSelection.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class TargetSelectionDlg : DataModule
{
    public TargetSelectionDlg(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AoWFrame CeltFrame;
    public AOWButton Close;
    public AOWLabel Title;
    public AOWImage SIcon;
    public AOWButton MainBtn;
    public AOWMemo DefaultMemo;
    public AOWPanel DefaultPnl;
    public AOWLabel HeaderLbl;
    public AOWPanel HeaderPnl;
    public AoWArnoLine BorderImage;
    public AOWPanel SpellPnl;
    public AOWMemo SpellMemo;
    public AOWLabel TargetRangeLabel;
    public AOWLabel TargetLbl;
    public IvTranslator Translator;

    // AoWTargetSelection.pas:45
    public void CloseClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.World.Map.AoWHSMap.Current.FTargetSelection.Deactivate();
    }

    // AoWTargetSelection.pas:51
    public void MainBtnClick()
    {
        CloseClick();
    }
}
