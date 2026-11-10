namespace Aow.Game.Ui.Dialogs;

public partial class FileDlgWindow
{
    public override void Loaded()
    {
        base.Loaded();
        if (SaveLB != null) SaveLB.WheelScrollBar = SSBar;
    }
}
