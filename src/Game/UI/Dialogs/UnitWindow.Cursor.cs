#nullable disable
using Aow.Game.Host;

namespace Aow.Game.UI.Dialogs;

public partial class UnitWindow
{
    public void UnitWinEnter() => GameHost.MainForm.FCursorImage = _cursorImage;
    public void UnitWinExit() => GameHost.MainForm.FCursorImage = null;

    public void UnitWinShow()
    {
        _cursorImage = null;
        GameHost.MainForm.FCursorImage = _cursorImage;
    }
}
