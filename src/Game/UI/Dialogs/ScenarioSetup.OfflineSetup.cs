#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Draw;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class ScenarioSetup
{
    public void SidePanelDrawBack(Panel sender, LockedSurface surface, int slot)
    {
        Panel panel = sender;
        UI.Graphics.Shields[slot].Show(panel.AbsoluteLeft + 5, panel.AbsoluteTop + 5, lockedSurface: surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(in Rect drawRectangle, int drawItem, ref readonly LockedSurface surface) { }
}
