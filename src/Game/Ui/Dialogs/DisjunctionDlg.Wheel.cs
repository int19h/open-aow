namespace Aow.Game.Ui.Dialogs;

public partial class DisjunctionDlg
{
    public override void Loaded()
    {
        base.Loaded();
        if (EnchantmentList != null) EnchantmentList.WheelScrollBar = EnchantmentsScrollBar;
    }
}
