namespace Aow.Platform;

// The audio thread owns PCM positions and ramps. Game objects stay on the main thread.
public sealed class AudioMixer : IDisposable
{
    public object Sync { get; } = new();

    public sealed class Voice(WaveData wave)
    {
        public WaveData Wave { get; } = wave;

        public double Position;
        public float Volume = 1;
        public float Pan;
        public bool Loop, Playing = true;
    }

    public sealed class Ramp
    {
        public float Value { get; private set; } = 127;

        private float target, step;
        private int remaining;

        public void Set(float value, int speed)
        {
            target = value;
            remaining = Math.Clamp(speed, 0, 100);
            if (remaining == 0) Value = value;
            else step = (target - Value) / remaining;
        }

        public void Tick() { if (remaining > 0) { Value += step; if (--remaining == 0) Value = target; } }
    }

    private readonly List<Voice> voices = [];

    public Ramp MusicGain { get; } = new();
    public Ramp SfxGain { get; } = new();
    public int Rate { get; set; } = 44100;
    public int VoiceLimit { get; set; } = 16;
    public long RenderedFrames { get; private set; }
    public bool MusicEnabled { get; set; } = true;
    public bool SfxEnabled { get; set; } = true;
    public TrackerSong? Song { get; set; }
    public bool SongEnded { get; private set; }

    public void ReplaceSong(TrackerSong? song) { Song?.Dispose(); Song = song; SongEnded = false; }

    public Voice? Play(WaveData data)
    {
        voices.RemoveAll(v => !v.Playing);
        if (voices.Count >= VoiceLimit || data.Frames == 0) return null;
        var voice = new Voice(data);
        voices.Add(voice);
        return voice;
    }

    public void StopVoices() { foreach (var voice in voices) voice.Playing = false; voices.Clear(); }

    // One ramp step per 50 ms mixer block, independent of SDL callback size.
    private int blockRemaining;

    public void Render(Span<float> output)
    {
        if (output.Length % 2 != 0) throw new ArgumentException("Expected stereo output.");
        lock (Sync)
        {
            RenderedFrames += output.Length / 2;
            output.Clear();
            while (!output.IsEmpty)
            {
                if (blockRemaining == 0) { MusicGain.Tick(); SfxGain.Tick(); blockRemaining = Math.Max(1, Rate / 20); }
                int frames = Math.Min(blockRemaining, output.Length / 2);
                var block = output[..(frames * 2)];
                if (Song is not null && !SongEnded)
                {
                    int read = Song.Render(Rate, block);
                    if (read < frames) { block[(read * 2)..].Clear(); SongEnded = true; }
                }
                float music = MusicEnabled ? MusicGain.Value / 127 : 0;
                for (int i = 0; i < block.Length; i++) block[i] *= music;
                foreach (var voice in voices)
                {
                    if (!voice.Playing) continue;
                    var wave = voice.Wave;
                    double step = (double)wave.SampleRate / Rate;
                    float gain = SfxEnabled ? voice.Volume * SfxGain.Value / 127 : 0;
                    float left = gain * (voice.Pan > 0 ? 1 - voice.Pan : 1);
                    float right = gain * (voice.Pan < 0 ? 1 + voice.Pan : 1);
                    for (int frame = 0; frame < frames; frame++)
                    {
                        if (voice.Position >= wave.Frames)
                        {
                            if (voice.Loop) voice.Position %= wave.Frames;
                            else { voice.Playing = false; break; }
                        }
                        int a = (int)voice.Position;
                        int b = a + 1 < wave.Frames ? a + 1 : voice.Loop ? 0 : a;
                        float fraction = (float)(voice.Position - a);
                        float At(int channel) => wave.Samples[a * wave.Channels + channel] * (1 - fraction) + wave.Samples[b * wave.Channels + channel] * fraction;
                        block[frame * 2] += At(0) * left;
                        block[frame * 2 + 1] += At(wave.Channels - 1) * right;
                        voice.Position += step;
                    }
                    if (!voice.Loop && voice.Position >= wave.Frames) voice.Playing = false;
                }
                for (int i = 0; i < block.Length; i++) block[i] = Math.Clamp(block[i], -1, 1);
                blockRemaining -= frames;
                output = output[(frames * 2)..];
            }
        }
    }

    public void Dispose() { lock (Sync) { ReplaceSong(null); StopVoices(); } }
}
