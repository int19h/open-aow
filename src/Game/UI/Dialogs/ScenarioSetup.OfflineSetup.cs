using Aow.Game.Draw;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class ScenarioSetup
{
    public void SidePanelDrawBack(Panel sender, LockedSurface surface, int slot)
    {
        var panel = sender;
        UI.Graphics.Shields[slot].Show(panel.WinLeft + 5, panel.WinTop + 5, surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(in Rect drawRectangle, int drawItem, ref LockedSurface surface) { }
}
