// Translated from Sound.pas.

namespace Aow.Game.Audio;

[Flags]
public enum SoundOutputModes : byte
{
    None = 0,
    Mono = 1 << 0,
    Stereo = 1 << 1,
    Bits8 = 1 << 2,
    Bits16 = 1 << 3,
    HighQuality = 1 << 4,
}
