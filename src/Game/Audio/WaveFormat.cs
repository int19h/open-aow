namespace Aow.Game.Audio;

// The format of a WAVE sound: the fields of the RIFF "fmt " chunk that the decoder reads.
public readonly record struct WaveFormat(ushort FormatTag, ushort Channels, int SamplesPerSecond, ushort BlockAlign, ushort BitsPerSample);
