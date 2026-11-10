using Aow.Game.Draw;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

public partial class TScenSetup
{
    public void RacePnlDrawBack(TAOWPanel sender, TSurfaceDesc surface, int Slot)
    {
        var panel = sender;
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.Shields[Slot].Show(panel.WinLeft + 5, panel.WinTop + 5, surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(in TRect drawRect, int drawItem, ref TSurfaceDesc surface) { }
}
