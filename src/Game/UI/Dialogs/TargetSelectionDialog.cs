using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class TargetSelectionDialog : DataModule
{
    public TargetSelectionDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Button MainBtn;
    public Memo DefaultMemo;
    public Label HeaderLbl;
    public ArnoLine BorderImage;
    public Label TargetLbl;

    // AoWTargetSelection.pas:45
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        AoWHSMap.Current.TargetSelection.Deactivate();
    }

    // AoWTargetSelection.pas:51
    public void MainBtnClick()
    {
        CloseClick();
    }
}
