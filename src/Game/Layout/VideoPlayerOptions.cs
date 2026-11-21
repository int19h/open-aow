// Layout translated from recovered Delphi forms.
#nullable disable

namespace Aow.Game.Layout;

[Flags]
public enum VideoPlayerOptions : byte
{
    None = 0,
    UseDirectSound = 1 << 0,
    SoundEnabled = 1 << 1,
    VideoEnabled = 1 << 2,
}
