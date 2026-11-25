// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Rtl;

namespace Aow.Game.Layout;

public sealed class LayoutVideoPlayer(Component owner) : VideoPlayer(owner)
{
    public VideoPlayerLoopMode LoopMode { get; set; }
    public VideoPlayerOptions Options { get; set; }

    public enum VideoPlayerLoopMode
    {
        None,
        LoopLongest,
        LoopVideo,
        LoopSound,
        LoopIndependent
    }
}
