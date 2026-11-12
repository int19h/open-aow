namespace Aow.Game.UI.Dialogs;

public partial class FileDialogWindow
{
    public override void Loaded()
    {
        base.Loaded();
        if (SaveList != null) SaveList.WheelScrollBar = SaveScrollBar;
    }
}
