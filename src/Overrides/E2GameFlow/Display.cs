namespace Aow.Units.E2GameFlow;

public static partial class E2GameFlow
{
    public static void ShowTitleScene(AoWTitleScene.TTitleScene scene, bool music) => scene.ShowScene(music);
    public static void ApplyVideoSettings(AoWVideoDlg.TVideoDialog dialog) => dialog.ApplyMode();
}
