using Aow.Game.Draw;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class ScenarioSetup
{
    public void RacePnlDrawBack(Panel sender, LockedSurface surface, int Slot)
    {
        var panel = sender;
        DialogHosts.IntGfxMod.Shields[Slot].Show(panel.WinLeft + 5, panel.WinTop + 5, surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(in Rect drawRect, int drawItem, ref LockedSurface surface) { }
}
