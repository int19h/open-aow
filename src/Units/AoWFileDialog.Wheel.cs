namespace Aow.Units.AoWFileDialog;

public partial class TFileDlgWindow
{
    public override void Loaded()
    {
        base.Loaded();
        if (SaveLB != null) SaveLB.WheelScrollBar = SSBar;
    }
}
