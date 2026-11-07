using Aow.Rtl;
using Aow.Units.GFXE;

namespace Aow.Units.AoWIntroScene;

// No video decoder is installed. This follows the original missing-codec path.
public partial class TIntroScene
{
    public void InitializeSound() { }

    public void ReleaseSound() { }

    public void VideoPlayerStreamSound(TStreamSoundMode mode, object data, int size) { }

    public void Execute() => Finish();

    public void WindowHide() { }

    public void WindowUpdate() => Finish();
    public void WindowDrawSurfaceEx(TSurface surface, ref bool handled) => handled = true;
}
