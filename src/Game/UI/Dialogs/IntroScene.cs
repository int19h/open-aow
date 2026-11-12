// Translated from AoWIntroScene.pas.
using Aow.Game.Audio;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// sound buffer, buffer size, DirectSound, stream position
public partial class IntroScene : DataModule
{
    public IntroScene(Component owner) : base(owner)
    { }

    public UserWindow Window;

    public IDirectSoundBuffer SoundBuffer { get; }
    public int BufferSize { get; }
    public IDirectSound DirectSound { get; }
    public int StreamPosition { get; }

    // AoWIntroScene.pas:173
    public void Finish()
    {
        ReleaseSound();
        Window.Visible = false;
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
    }

    // AoWIntroScene.pas:192
    public void WindowMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Finish();
    }

    // AoWIntroScene.pas:197
    public void WindowKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27 || key == 32)
        {
            Finish();
        }
    }
}
