using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.AOWMWindow;

public partial class TMWindow
{
    // AOWMWindow.pas:141. Frame invalidation and measured FPS use the active host.
    public void MapWindowUpdate()
    {
        if (AoWHSMap is null) return;
        AoWHSMap.UpdateFPS(FrameFixtureInputs.MeasuredFrameRate ?? AoWUnit.AoWUnit.MainForm.Display.CurrentFrameRate);
        MapWindow.FDrawn &= MapWindow.FDoubleBuffered;
    }
}
