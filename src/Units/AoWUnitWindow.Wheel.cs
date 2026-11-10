namespace Aow.Game.Ui.Dialogs;

public partial class TUnitWindow
{
    public override void Loaded()
    {
        base.Loaded();
        if (T1AbilityLB != null) T1AbilityLB.WheelScrollBar = T1SBar;
        if (T1TypeLB != null) T1TypeLB.WheelScrollBar = T1SBar;
    }
}
