#nullable disable
using Aow.Rtl;

namespace Aow.Units.AoWUnitWindow;

public partial class TUnitWindow
{
    public void UnitWinEnter(TObject sender) => AoWUnit.AoWUnit.MainForm.FCursorImage = FCursorImage;
    public void UnitWinExit(TObject sender) => AoWUnit.AoWUnit.MainForm.FCursorImage = null;
    public void UnitWinShow(TObject sender)
    {
        FCursorImage = null;
        AoWUnit.AoWUnit.MainForm.FCursorImage = FCursorImage;
    }
}
