// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Layout;

public class LayoutVideoPlayer : VideoPlayer
{
    public LayoutVideoPlayer(Component owner) : base(owner)
    {
    }

    public VideoPlayerLoopMode LoopMode { get; set; }
    public StreamSoundEvent OnStreamSound { get; set; }
    public Set<VideoPlayerOptionsElement> Options { get; set; }

    public enum VideoPlayerLoopMode
    {
        None, LoopLongest, LoopVideo, LoopSound, LoopIndependent
    }

    public delegate void StreamSoundEvent(StreamSoundMode Mode, object Data, int DataSize);
}
