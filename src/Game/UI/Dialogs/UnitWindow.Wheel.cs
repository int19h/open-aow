namespace Aow.Game.UI.Dialogs;

public partial class UnitWindow
{
    public override void Loaded()
    {
        base.Loaded();
        if (AbilityAbilityList != null) AbilityAbilityList.WheelScrollBar = AbilityScrollBar;
        if (AbilityTypeList != null) AbilityTypeList.WheelScrollBar = AbilityScrollBar;
    }
}
