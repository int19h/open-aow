// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Layout;

public class LayoutVideoPlayer : VideoPlayer
{
    public LayoutVideoPlayer(Component owner) : base(owner)
    {
    }

    public VideoPlayerLoopMode Loopmode { get; set; }
    public StreamSoundEvent OnStreamSound { get; set; }
    public Set<VideoPlayerOptions_Element> Options { get; set; }

    public enum VideoPlayerLoopMode
    {
        None, LoopLongest, LoopVideo, LoopSound, LoopIndependent
    }

    public delegate void StreamSoundEvent(StreamSoundMode Mode, object Data, int DataSize);
}
