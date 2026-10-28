namespace Aow.Units.E2GameFlow;

public static partial class E2GameFlow
{
    public static void ShowTitleScene(AoWTitleScene.TTitleScene scene, bool music) => scene.ShowScene(music);
    public static void ApplyVideoSettings(AoWVideoDlg.TVideoDialog dialog) => dialog.ApplyMode();
    public static bool HasModalWindow(AoWWinMan.TGeneral controller)
    {
        for (int i = controller.Manager.FChildren.Count - 1; i >= 0; i--)
            if (controller.Manager.FChildren[i] is AoWWinControl.TAOWWindowControl window &&
                window != AOWMWindow.AOWMWindow.MWindow.MapWindow && window.FModal && window.Visible)
                return true;
        return false;
    }
}
