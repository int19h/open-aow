using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class TargetSelectionDialog : UIModule
{
    public TargetSelectionDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public Button MainButton;
    public Memo DefaultMemo;
    public Label HeaderLabel;
    public ArnoLine BorderLine;
    public Label TargetLabel;

    // AoWTargetSelection.pas:45
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        StrategicMap.Current.TargetSelection.Deactivate();
    }

    // AoWTargetSelection.pas:51
    public void MainBtnClick()
    {
        CloseClick();
    }
}
