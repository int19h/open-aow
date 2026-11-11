namespace Aow.Game.UI.Dialogs;

public partial class DisjunctionDialog
{
    public override void Loaded()
    {
        base.Loaded();
        if (EnchantmentList != null) EnchantmentList.WheelScrollBar = EnchantmentsScrollBar;
    }
}
