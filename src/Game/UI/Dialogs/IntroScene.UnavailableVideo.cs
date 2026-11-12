using Aow.Game.Draw;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// No video decoder is installed. This follows the original missing-codec path.
public partial class IntroScene
{
    public void InitializeSound() { }

    public void ReleaseSound() { }

    public void VideoPlayerStreamSound(StreamSoundMode mode, object data, int size) { }

    public void Execute() => Finish();

    public void WindowHide() { }

    public void WindowUpdate() => Finish();
    public void WindowDrawSurfaceEx(Surface surface, out bool handled) => handled = true;
}
