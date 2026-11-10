#nullable disable
using Aow.Game.Host;

namespace Aow.Game.Ui.Dialogs;

public partial class VideoDialog
{
    public void ApplyMode()
    {
        if (!FModeChanged || GameHost.MainForm is not { } form) return;
        bool full = form.Display.Host.Fullscreen;
        if (FFullScreen == ScreenMode.Windowed || !full) ChangeResolution(0, 0);
        else if (FFullScreen == ScreenMode.Title && Magic.Abilities.GameTables.AoWEngine.FRegistry.GetAllowResolutionSwitches()) ChangeResolution(FSceneWidth, FSceneHeight);
        else ChangeResolution(FDisplayWidth, FDisplayHeight);
        FModeChanged = false;
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
        for (int i = 0; i < FDisplayModes.Count; i++)
            if (FDisplayModes.Items[i].Width == form.Display.BackSurfaceWidth && FDisplayModes.Items[i].Height == form.Display.BackSurfaceHeight) return i + 1;
        return 0;
    }
}
