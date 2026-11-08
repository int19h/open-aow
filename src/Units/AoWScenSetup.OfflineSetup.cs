using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWPanel;
using Aow.Units.GFXE;

namespace Aow.Units.AoWScenSetup;

public partial class TScenSetup
{
    public void RacePnlDrawBack(TAOWPanel sender, TSurfaceDesc surface, int Slot)
    {
        var panel = sender;
        aowIntGfxModUnit.IntGfxMod.Shields[Slot].Show(panel.WinLeft + 5, panel.WinTop + 5, surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(in TRect drawRect, int drawItem, ref TSurfaceDesc surface) { }
}
