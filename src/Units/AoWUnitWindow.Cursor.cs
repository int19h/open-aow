#nullable disable
using Aow.Platform;
using Aow.Rtl;

namespace Aow.Units.AoWUnitWindow;

public partial class TUnitWindow
{
    public void UnitWinEnter(TObject sender) => GameHost.MainForm.FCursorImage = FCursorImage;
    public void UnitWinExit(TObject sender) => GameHost.MainForm.FCursorImage = null;
    public void UnitWinShow(TObject sender)
    {
        FCursorImage = null;
        GameHost.MainForm.FCursorImage = FCursorImage;
    }
}
