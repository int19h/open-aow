#nullable disable
using Aow.Game.Draw;
using Aow.Game.Host;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class General
{
    public void ManagerChangeCursor()
    {
        if (GameHost.MainForm is { } form)
        {
            form.FCursor = (short)Math.Abs(Manager.Cursor);
        }
    }

    public void ManagerUpdate()
    {
        Menu unit = UI.UnitBanner.BannerMenu;
        if (unit.ShowPending && !unit.Visible)
        {
            unit.WinLeft = unit.MenuLeft;
            unit.WinTop = unit.MenuTop;
            unit.Visible = true;
        }
        Menu info = UI.InfoBanner.BannerMenu;
        if (info.ShowPending && !info.Visible)
        {
            info.WinLeft = info.MenuLeft;
            info.WinTop = info.MenuTop;
            info.Visible = true;
        }
        UI.MapWindow.MainWindow.DoubleBuffered = Manager.OverlayPriority > UI.MapWindow.MainWindow.Priority;
        if (GameHost.MainForm is { FCursorChanged: true } form)
        {
            Rect rectangle = form.FLastCursorRectangle;
            Manager.AddDrawRectangle(ref rectangle, sender: null);
        }
    }

    public void MapProgress(string message, int progression)
    {
        ProgressDialog progress = UI.ProgressDialog;
        progress.StatusLabel.Text = message;
        progress.ProgressBar.Position = progression;
        progress.Window.SurfaceValid = false;
        Display display = GameHost.MainForm?.Display;
        if (display?.ActiveDisplayController is { } displayController)
        {
            Manager.DrawSurface(displayController.GetSurface(1));
            _ = displayController.BlitSurface(display.SurfaceIndex);
        }
    }
}
