namespace Aow.Game.UI.Dialogs;

public partial class FileDialogWindow
{
    public override void Loaded()
    {
        base.Loaded();
        SaveList?.WheelScrollBar = SaveScrollBar;
    }
}
