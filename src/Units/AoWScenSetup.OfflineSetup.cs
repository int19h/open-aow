using Aow.Game.Draw;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class ScenSetup
{
    public void RacePnlDrawBack(AOWPanel sender, SurfaceDesc surface, int Slot)
    {
        var panel = sender;
        DialogHosts.IntGfxMod.Shields[Slot].Show(panel.WinLeft + 5, panel.WinTop + 5, surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(in Rect drawRect, int drawItem, ref SurfaceDesc surface) { }
}
