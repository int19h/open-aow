#nullable disable
using Aow.Game.Host;

namespace Aow.Game.UI.Dialogs;

public partial class UnitWindow
{
    public void UnitWinEnter() => GameHost.MainForm.FCursorImage = FCursorImage;
    public void UnitWinExit() => GameHost.MainForm.FCursorImage = null;

    public void UnitWinShow()
    {
        FCursorImage = null;
        GameHost.MainForm.FCursorImage = FCursorImage;
    }
}
