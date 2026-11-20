namespace Aow.Game.UI.Dialogs;

public sealed partial class FileDialogWindow
{
    public override void Loaded()
    {
        base.Loaded();
        SaveList?.WheelScrollBar = SaveScrollBar;
    }
}
