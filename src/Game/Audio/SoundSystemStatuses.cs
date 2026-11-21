// Translated from Sound.pas.

namespace Aow.Game.Audio;

[Flags]
public enum SoundSystemStatuses : byte
{
    None = 0,
    Activated = 1 << 0,
    DirectSoundEnabled = 1 << 1,
}
