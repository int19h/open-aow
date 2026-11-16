namespace Aow.Game.UI.Dialogs;

public partial class MagicWindow
{
    public override void Loaded()
    {
        base.Loaded();
        PowerSourceList?.WheelScrollBar = PowerScrollBar;

        GlobalEnchantmentList?.WheelScrollBar = GlobalEnchantmentScrollBar;

        GlobalNameList?.WheelScrollBar = GlobalEnchantmentScrollBar;

        EnchantmentList?.WheelScrollBar = EnchantmentScrollBar;

        BindingList?.WheelScrollBar = EnchantmentScrollBar;
    }
}
