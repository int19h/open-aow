using Aow.Units.AoWComp;
using Aow.Units.GFXE;

namespace Aow.Units.AoWCastUnitSpellDlg;

public partial class TCastUnitSpellDlg
{
    // AoWCastUnitSpellDlg.pas:183. The sender is a unit checkbox.
    public void U1PreDraw(TAoWComponent sender, TSurfaceDesc surface) =>
        DrawUnit(sender.Tag, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref surface);
}
