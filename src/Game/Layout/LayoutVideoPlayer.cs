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

    // Nothing in the port raises OnStreamSound. The bound handler is kept.
    private StreamSoundEvent _onStreamSound;

    public event StreamSoundEvent OnStreamSound { add => _onStreamSound += value; remove => _onStreamSound -= value; }
    public Set<VideoPlayerOptionsElement> Options { get; set; }

    public enum VideoPlayerLoopMode
    {
        None, LoopLongest, LoopVideo, LoopSound, LoopIndependent
    }

    public delegate void StreamSoundEvent(StreamSoundMode Mode, object Data, int DataSize);
}
