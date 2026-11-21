// Layout translated from recovered Delphi forms.
#nullable disable
using System.Diagnostics.CodeAnalysis;
using Aow.Rtl;

namespace Aow.Game.Layout;

public sealed class LayoutVideoPlayer(Component owner) : VideoPlayer(owner)
{
    public VideoPlayerLoopMode LoopMode { get; set; }

    // Nothing in the port raises OnStreamSound. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnStreamSound keeps this backing field until the port raises it.")]
    private StreamSoundEvent _onStreamSound;

    public event StreamSoundEvent OnStreamSound { add => _onStreamSound += value; remove => _onStreamSound -= value; }
    public VideoPlayerOptions Options { get; set; }

    public enum VideoPlayerLoopMode
    {
        None, LoopLongest, LoopVideo, LoopSound, LoopIndependent
    }

    public delegate void StreamSoundEvent(StreamSoundMode Mode, object Data, int DataSize);
}
