#nullable disable
using Aow.Rtl;

namespace Aow.Units.AoWUnitWindow;

public partial class TUnitWindow
{
    public void UnitWinEnter(TObject sender) => Aow.Platform.GameHost.MainForm.FCursorImage = FCursorImage;
    public void UnitWinExit(TObject sender) => Aow.Platform.GameHost.MainForm.FCursorImage = null;
    public void UnitWinShow(TObject sender)
    {
        FCursorImage = null;
        Aow.Platform.GameHost.MainForm.FCursorImage = FCursorImage;
    }
}
