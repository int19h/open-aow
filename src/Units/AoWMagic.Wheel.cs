namespace Aow.Units.AoWMagic;

public partial class TMagicWin
{
    public override void Loaded()
    {
        base.Loaded();
        if (PowerSourceList != null) PowerSourceList.WheelScrollBar = PowerSB;
        if (GlobalEnchantmentList != null) GlobalEnchantmentList.WheelScrollBar = GlobalEnchantmentSB;
        if (GlobalNameList != null) GlobalNameList.WheelScrollBar = GlobalEnchantmentSB;
        if (EnchantmentList != null) EnchantmentList.WheelScrollBar = EnchantmentSB;
        if (BindingList != null) BindingList.WheelScrollBar = EnchantmentSB;
    }
}
