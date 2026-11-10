#nullable disable
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class General
{
    public void ManagerChangeCursor()
    {
        if (GameHost.MainForm is { } form) form.FCursor = (short)Math.Abs(Manager.Cursor);
    }

    public void ManagerUpdate()
    {
        var unit = Aow.Game.Ui.Dialogs.DialogHosts.UnitBanner.BannerPopup;
        if (unit.FShowPending && !unit.Visible)
        {
            unit.WinLeft = unit.FMenuLeft;
            unit.WinTop = unit.FMenuTop;
            unit.Visible = true;
        }
        var info = Aow.Game.Ui.Dialogs.DialogHosts.InfoBanner.BannerPopup;
        if (info.FShowPending && !info.Visible)
        {
            info.WinLeft = info.FMenuLeft;
            info.WinTop = info.FMenuTop;
            info.Visible = true;
        }
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.DoubleBuffered = Manager.FOverlayPriority > Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Priority;
        if (GameHost.MainForm is { FCursorChanged: true } form)
        {
            var rect = form.FLastCursorRect;
            Manager.AddDrawRect(ref rect, null);
        }
    }

    public void MapProgress(string message, int progression)
    {
        var progress = Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog;
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
