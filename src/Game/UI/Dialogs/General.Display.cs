#nullable disable
using Aow.Game.Host;

namespace Aow.Game.UI.Dialogs;

public partial class General
{
    public void ManagerChangeCursor()
    {
        if (GameHost.MainForm is { } form) form.FCursor = (short)Math.Abs(Manager.Cursor);
    }

    public void ManagerUpdate()
    {
        var unit = UI.UnitBanner.BannerMenu;
        if (unit.ShowPending && !unit.Visible)
        {
            unit.WinLeft = unit.MenuLeft;
            unit.WinTop = unit.MenuTop;
            unit.Visible = true;
        }
        var info = UI.InfoBanner.BannerMenu;
        if (info.ShowPending && !info.Visible)
        {
            info.WinLeft = info.MenuLeft;
            info.WinTop = info.MenuTop;
            info.Visible = true;
        }
        UI.MapWindow.MainWindow.DoubleBuffered = Manager.OverlayPriority > UI.MapWindow.MainWindow.Priority;
        if (GameHost.MainForm is { FCursorChanged: true } form)
        {
            var rectangle = form.FLastCursorRectangle;
            Manager.AddDrawRectangle(ref rectangle, null);
        }
    }

    public void MapProgress(string message, int progression)
    {
        var progress = UI.ProgressDialog;
        progress.StatusLabel.Text = message;
        progress.ProgressBar.Position = progression;
        progress.Window.SurfaceValid = false;
        var display = GameHost.MainForm?.Display;
        if (display?.ActiveDisplayController is { } displayController)
        {
            Manager.DrawSurface(displayController.GetSurface(1));
            displayController.BlitSurface(display.SurfaceIndex);
        }
    }
}
