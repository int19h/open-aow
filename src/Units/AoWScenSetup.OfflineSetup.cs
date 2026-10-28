namespace Aow.Units.AoWScenSetup;

public partial class TScenSetup
{
    public void RacePnlDrawBack(Aow.Units.AoWComp.TAoWComponent sender, Aow.Units.GFXE.TSurfaceDesc surface)
    {
        var panel = (Aow.Units.AoWPanel.TAOWPanel)sender;
        Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields[sender.Tag].Show(panel.WinLeft + 5, panel.WinTop + 5, surface);
    }

    public void ScenSetupPnlUpdate() { }

    public void PingListDrawItem(Aow.Units.AoWComp.TAoWComponent sender, in Aow.Rtl.TRect drawRect, int drawItem, ref Aow.Units.GFXE.TSurfaceDesc surface) { }
}
