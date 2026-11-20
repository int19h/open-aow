#nullable disable
// Translated from AoWIntroScene.pas.
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// sound buffer, buffer size, DirectSound, stream position
public sealed partial class IntroScene(UIContext ui) : UIModule(ui)
{
    public UserWindow Window { get; private set; }

    // AoWIntroScene.pas:173
    public void Finish()
    {
        ReleaseSound();
        Window.Visible = false;
        WindowActions.ShowTitleScene(UI.TitleScene, music: true);
    }

    // AoWIntroScene.pas:192
    public void WindowMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y) => Finish();

    // AoWIntroScene.pas:197
    public void WindowKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key is 27 or 32)
        {
            Finish();
        }
    }
}
