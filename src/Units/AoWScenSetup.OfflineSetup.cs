using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWPanel;
using Aow.Units.GFXE;

namespace Aow.Units.AoWScenSetup;

public partial class TScenSetup
{
    public void RacePnlDrawBack(TAoWComponent sender, TSurfaceDesc surface)
    {
        var panel = (TAOWPanel)sender;
        aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields[sender.Tag].Show(panel.WinLeft + 5, panel.WinTop + 5, surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(TAoWComponent sender, in TRect drawRect, int drawItem, ref TSurfaceDesc surface) { }
}
