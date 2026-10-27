#nullable disable
using Aow.Rtl;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AoWWinMan;

public partial class TGeneral
{
    public void ManagerChangeCursor()
    {
        if (AoWUnit.AoWUnit.MainForm is { } form) form.FCursor = unchecked((short)Math.Abs(Manager.Cursor));
    }
    public void ManagerUpdate()
    {
        var unit = AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopup;
        if (unit.FShowPending && !unit.Visible)
        {
            unit.WinLeft = unit.FMenuLeft;
            unit.WinTop = unit.FMenuTop;
            unit.Visible = true;
        }
        var info = AoWInfoBanner.AoWInfoBanner.InfoBanner.BannerPopup;
        if (info.FShowPending && !info.Visible)
        {
            info.WinLeft = info.FMenuLeft;
            info.WinTop = info.FMenuTop;
            info.Visible = true;
        }
        AOWMWindow.AOWMWindow.MWindow.MapWindow.DoubleBuffered = Manager.FOverlayPriority > AOWMWindow.AOWMWindow.MWindow.MapWindow.Priority;
        if (AoWUnit.AoWUnit.MainForm is { FCursorChanged: true } form) Manager.AddDrawRect(ref form.FLastCursorRect, null);
    }
    public void MapProgress(Engine.TEObject sender, AnsiString message, int progression)
    {
        var progress = E2Globals.E2Globals.ProgressDialog;
        progress.StatusLbl.Text = message;
        progress.Bar.Position = progression;
        progress.ProgDlg.FSurfaceValid = false;
        var display = AoWUnit.AoWUnit.MainForm?.Display;
        if (display?.ActiveDC is { } dc)
        {
            Manager.DrawSurface(dc.GetSurface(1));
            dc.BltSurface(display.FSurfaceIndex);
        }
    }
}
