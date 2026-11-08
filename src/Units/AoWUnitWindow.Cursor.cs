#nullable disable
using Aow.Platform;
using Aow.Rtl;

namespace Aow.Units.AoWUnitWindow;

public partial class TUnitWindow
{
    public void UnitWinEnter() => GameHost.MainForm.FCursorImage = FCursorImage;
    public void UnitWinExit() => GameHost.MainForm.FCursorImage = null;

    public void UnitWinShow()
    {
        FCursorImage = null;
        GameHost.MainForm.FCursorImage = FCursorImage;
    }
}
