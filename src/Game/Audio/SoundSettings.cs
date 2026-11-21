// Translated from Sound.pas.

namespace Aow.Game.Audio;

// { The settings a sound system is activated with (24 bytes): +0 OutputMode, +4 SampleRate, +8
//     MusicEnabled, +9 UseDirectSound, +$C Window (the DirectSound window; the active window when
//     0), +$10 SfxEnabled, +$14 SampleVoices. }
// Each set keeps the width of its Delphi set: a value is cut to that width when it is assigned.
public struct SoundSettings
{
    public SoundOutputModes OutputMode { get; set; }

    public int SampleRate { get; set; }
    public bool MusicEnabled { get; set; }
    public bool UseDirectSound { get; internal set; }
    public int Window { get; }
    public bool SfxEnabled { get; set; }
    public int SampleVoices { get; set; }
}
