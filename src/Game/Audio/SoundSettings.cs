// Translated from Sound.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Game.Audio;

// { The settings a sound system is activated with (24 bytes): +0 OutputMode, +4 SampleRate, +8
//     MusicEnabled, +9 UseDirectSound, +$C Window (the DirectSound window; the active window when
//     0), +$10 SFXEnabled, +$14 SampleVoices. }
[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct SoundSettings
{
    [FieldOffset(0)]
    public byte OutputMode__image;

    public Set<SoundOutputModeElement> OutputMode { get => Set<SoundOutputModeElement>.FromMask(OutputMode__image); set => OutputMode__image = (byte)value.Image; }

    [FieldOffset(4)]
    public int SampleRate;
    [FieldOffset(8)]
    public bool MusicEnabled;
    [FieldOffset(9)]
    public bool UseDirectSound;
    [FieldOffset(12)]
    public int Window;
    [FieldOffset(16)]
    public bool SFXEnabled;
    [FieldOffset(20)]
    public int SampleVoices;
}
