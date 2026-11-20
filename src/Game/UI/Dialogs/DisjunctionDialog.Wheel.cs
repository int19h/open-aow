namespace Aow.Game.UI.Dialogs;

public sealed partial class DisjunctionDialog
{
    public override void Loaded()
    {
        base.Loaded();
        EnchantmentList?.WheelScrollBar = EnchantmentsScrollBar;
    }
}
