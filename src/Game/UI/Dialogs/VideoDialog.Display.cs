#nullable disable
using Aow.Game.Host;

namespace Aow.Game.UI.Dialogs;

public partial class VideoDialog
{
    public void ApplyMode()
    {
        if (!ModeChanged || GameHost.MainForm is not { } form) return;
        bool full = form.Display.Host.Fullscreen;
        if (_fullScreen == ScreenMode.Windowed || !full) ChangeResolution(0, 0);
        else if (_fullScreen == ScreenMode.Title && UI.Engine.Registry.GetAllowResolutionSwitches()) ChangeResolution(_sceneWidth, _sceneHeight);
        else ChangeResolution(DisplayWidth, DisplayHeight);
        ModeChanged = false;
    }

    public void ChangeResolution(int width, int height)
    {
        var form = GameHost.MainForm;
        if (form is null) return;
        if (width == 0 || height == 0) (width, height) = form.Display.Host.ClientSize;
        form.Display.ResizeBackSurfaces(width, height);
    }

    public int CurrentMode()
    {
        if (GameHost.MainForm is not { } form || !form.Display.Host.Fullscreen) return 0;
        for (int i = 0; i < _displayModes.Count; i++)
            if (_displayModes.Items[i].Width == form.Display.BackSurfaceWidth && _displayModes.Items[i].Height == form.Display.BackSurfaceHeight) return i + 1;
        return 0;
    }
}
