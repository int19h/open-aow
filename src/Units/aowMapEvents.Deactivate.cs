namespace Aow.Units.aowMapEvents;

public partial class TMapEvents
{
    // aowMapEvents.pas:176. The offline transport has no connection to release.
    public void TheMapDeactivateHSM()
    {
        try
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.SaveLayout();
            Aow.Units.AoWWinMan.AoWWinMan.General.HideWindows();
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapViewer.HSMap = null;
            if (Aow.Units.aowSetupEvents.aowSetupEvents.SetupEvents.FLobbyGame)
                Aow.Platform.GameHost.MainForm.CloseRequested();
        }
        catch (System.Exception)
        {
            Aow.Rtl.Rtl.ShowMessage("Error during close");
        }
    }
}
