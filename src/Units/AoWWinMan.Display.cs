#nullable disable
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.Engine;

namespace Aow.Units.AoWWinMan;

public partial class TGeneral
{
    public void ManagerChangeCursor()
    {
        if (GameHost.MainForm is { } form) form.FCursor = (short)Math.Abs(Manager.Cursor);
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
        if (GameHost.MainForm is { FCursorChanged: true } form)
        {
            var rect = form.FLastCursorRect;
            Manager.AddDrawRect(ref rect, null);
        }
    }
    public void MapProgress(TEObject sender, AnsiString message, int progression)
    {
        var progress = E2Globals.E2Globals.ProgressDialog;
        progress.StatusLbl.Text = message;
        progress.Bar.Position = progression;
        progress.ProgDlg.FSurfaceValid = false;
        var display = GameHost.MainForm?.Display;
        if (display?.ActiveDC is { } dc)
        {
            Manager.DrawSurface(dc.GetSurface(1));
            dc.BltSurface(display.FSurfaceIndex);
        }
    }
}
