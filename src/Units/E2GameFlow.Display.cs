using Aow.Units.AOWMWindow;
using Aow.Units.AoWTitleScene;
using Aow.Units.AoWVideoDlg;
using Aow.Units.AoWWinControl;
using Aow.Units.AoWWinMan;

namespace Aow.Units.E2GameFlow;

public static partial class E2GameFlowUnit
{
    public static void ShowTitleScene(TTitleScene scene, bool music) => scene.ShowScene(music);
    public static void ApplyVideoSettings(TVideoDialog dialog) => dialog.ApplyMode();
    public static bool HasModalWindow(TGeneral controller)
    {
        for (int i = controller.Manager.FChildren.Count - 1; i >= 0; i--)
            if (controller.Manager.FChildren[i] is TAOWWindowControl window &&
                window != AOWMWindowUnit.MWindow.MapWindow && window.FModal && window.Visible)
                return true;
        return false;
    }
}
