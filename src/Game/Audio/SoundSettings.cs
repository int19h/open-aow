// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// { The settings a sound system is activated with (24 bytes): +0 OutputMode, +4 SampleRate, +8
//     MusicEnabled, +9 UseDirectSound, +$C Window (the DirectSound window; the active window when
//     0), +$10 SfxEnabled, +$14 SampleVoices. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct SoundSettings
{
    public Set<SoundOutputModeElement> OutputMode { get; set => field = Set<SoundOutputModeElement>.FromMask((byte)value.Image); }

    public int SampleRate;
    public bool MusicEnabled;
    public bool UseDirectSound;
    public int Window;
    public bool SfxEnabled;
    public int SampleVoices;
}
