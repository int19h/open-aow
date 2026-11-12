namespace Aow.Game.UI.Dialogs;

public partial class MagicWindow
{
    public override void Loaded()
    {
        base.Loaded();
        if (PowerSourceList != null) PowerSourceList.WheelScrollBar = PowerScrollBar;
        if (GlobalEnchantmentList != null) GlobalEnchantmentList.WheelScrollBar = GlobalEnchantmentScrollBar;
        if (GlobalNameList != null) GlobalNameList.WheelScrollBar = GlobalEnchantmentScrollBar;
        if (EnchantmentList != null) EnchantmentList.WheelScrollBar = EnchantmentScrollBar;
        if (BindingList != null) BindingList.WheelScrollBar = EnchantmentScrollBar;
    }
}
