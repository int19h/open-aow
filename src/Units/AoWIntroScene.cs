// Translated from AoWIntroScene.pas.
using Aow.Game.Audio;
using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

// sound buffer, buffer size, DirectSound, stream position
public partial class TIntroScene : TDataModule
{
    public TIntroScene(TComponent owner) : base(owner)
    { }

    public TAoWUserWindow Window;
    public TVideoPlayer VideoPlayer;
    public IDirectSoundBuffer FSoundBuffer;
    public int FBufferSize;
    public IDirectSound FDirectSound;
    public int FStreamPosition;

    // AoWIntroScene.pas:173
    public void Finish()
    {
        ReleaseSound();
        Window.Visible = false;
        Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, true);
    }

    // AoWIntroScene.pas:192
    public void WindowMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        Finish();
    }

    // AoWIntroScene.pas:197
    public void WindowKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Finish();
        }
    }
}
