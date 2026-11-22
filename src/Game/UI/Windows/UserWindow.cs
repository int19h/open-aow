#nullable disable warnings
// Translated from AowUserWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// extended drawing event
public sealed class UserWindow(Component owner) : Window(owner)
{
    [Published]
    public event EventHandler<SurfaceDrawingEventArgs>? SurfaceDrawing;

    // AowUserWindow.pas:29
    public override void DrawSurface(Surface? surface)
    {
        bool handled = false;
        SurfaceDrawingEventArgs e = new()
        {
            Surface = surface
        };
        SurfaceDrawing?.Invoke(this, e);
        handled = e.Handled;
        if (!handled)
        {
            base.DrawSurface(surface);
        }
    }

    public delegate void DrawSurfaceExEvent(Surface? Surface, out bool Handled);
}
