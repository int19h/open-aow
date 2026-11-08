#nullable disable
using Aow.Platform;
using Aow.Rtl;
using Aow.Units.AoWInfoBanner;
using Aow.Units.AOWMWindow;
using Aow.Units.AoWUnitBanner;
using Aow.Units.E2Globals;
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
        var unit = AoWUnitBannerUnit.UnitBanner.BannerPopup;
        if (unit.FShowPending && !unit.Visible)
        {
            unit.WinLeft = unit.FMenuLeft;
            unit.WinTop = unit.FMenuTop;
            unit.Visible = true;
        }
        var info = AoWInfoBannerUnit.InfoBanner.BannerPopup;
        if (info.FShowPending && !info.Visible)
        {
            info.WinLeft = info.FMenuLeft;
            info.WinTop = info.FMenuTop;
            info.Visible = true;
        }
        AOWMWindowUnit.MWindow.MapWindow.DoubleBuffered = Manager.FOverlayPriority > AOWMWindowUnit.MWindow.MapWindow.Priority;
        if (GameHost.MainForm is { FCursorChanged: true } form)
        {
            var rect = form.FLastCursorRect;
            Manager.AddDrawRect(ref rect, null);
        }
    }

    public void MapProgress(string message, int progression)
    {
        var progress = E2GlobalsUnit.ProgressDialog;
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
