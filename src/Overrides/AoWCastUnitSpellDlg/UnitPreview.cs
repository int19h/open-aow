namespace Aow.Units.AoWCastUnitSpellDlg;

public partial class TCastUnitSpellDlg
{
    // AoWCastUnitSpellDlg.pas:183. The sender is a unit checkbox.
    public void U1PreDraw(Aow.Units.AoWComp.TAoWComponent sender, Aow.Units.GFXE.TSurfaceDesc surface) =>
        DrawUnit(sender.Tag, UnitPnl.WinLeft + sender.WinLeft, UnitPnl.WinTop + sender.WinTop, ref surface);
}
