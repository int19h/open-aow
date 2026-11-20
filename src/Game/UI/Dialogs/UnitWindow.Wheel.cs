namespace Aow.Game.UI.Dialogs;

public sealed partial class UnitWindow
{
    public override void Loaded()
    {
        base.Loaded();
        AbilityAbilityList?.WheelScrollBar = AbilityScrollBar;

        AbilityTypeList?.WheelScrollBar = AbilityScrollBar;
    }
}
