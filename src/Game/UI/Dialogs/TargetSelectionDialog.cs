using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class TargetSelectionDialog : UIModule
{
    public TargetSelectionDialog(UIContext ui) : base(ui)
    { }

    public Window Window { get; private set; }
    public Button MainButton { get; private set; }
    public Memo DefaultMemo { get; private set; }
    public Label HeaderLabel { get; private set; }
    public ArnoLine BorderLine { get; private set; }
    public Label TargetLabel { get; private set; }

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
