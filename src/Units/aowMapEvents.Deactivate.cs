using Aow.Platform;

namespace Aow.Units.aowMapEvents;

public partial class TMapEvents
{
    // aowMapEvents.pas:176. The offline transport has no connection to release.
    public void TheMapDeactivateHSM()
    {
        try
        {
            AoWWinMan.AoWWinMan.General.SaveLayout();
            AoWWinMan.AoWWinMan.General.HideWindows();
            AOWMWindow.AOWMWindow.MWindow.MapViewer.HSMap = null;
            if (aowSetupEvents.aowSetupEvents.SetupEvents.FLobbyGame)
                GameHost.MainForm.CloseRequested();
        }
        catch (System.Exception)
        {
            Rtl.Rtl.ShowMessage("Error during close");
        }
    }
}
