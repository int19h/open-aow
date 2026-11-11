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
    public VideoPlayer VideoPlayer;
    public IDirectSoundBuffer FSoundBuffer;
    public int FBufferSize;
    public IDirectSound FDirectSound;
    public int FStreamPosition;

    // AoWIntroScene.pas:173
    public void Finish()
    {
        ReleaseSound();
        Window.Visible = false;
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
    }

    // AoWIntroScene.pas:192
    public void WindowMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        Finish();
    }

    // AoWIntroScene.pas:197
    public void WindowKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Finish();
        }
    }
}
