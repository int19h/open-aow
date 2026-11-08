namespace Aow.Units.AoWDisjunctionDlg;

public partial class TDisjunctionDlg
{
    public override void Loaded()
    {
        base.Loaded();
        if (EnchantmentList != null) EnchantmentList.WheelScrollBar = EnchantmentsScrollBar;
    }
}
