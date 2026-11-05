// Translated from AoWIntroScene.pas.
using Aow.Rtl;
using Aow.Units.AowUserWindow;
using Aow.Units.E2DirectSound;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.E2GameFlow.E2GameFlow;

namespace Aow.Units.AoWIntroScene;

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
        ShowTitleScene(TitleScene, true);
    }

    // AoWIntroScene.pas:192
    public void WindowMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        Finish();
    }

    // AoWIntroScene.pas:197
    public void WindowKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27 || Key == 32)
        {
            Finish();
        }
    }
}

public static partial class AoWIntroScene
{
}
