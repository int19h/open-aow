using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Audio;
using SDL;
using static SDL.SDL3;

namespace Aow.Platform;

public sealed unsafe class SdlAudioOutput : IDisposable
{
    private SDL_AudioStream* _stream;
    private GCHandle _handle;
    private readonly AudioMixer _mixer;
    private readonly int _channels, _bytesPerSample;
    private readonly float[] _pcm;
    private readonly byte[] _encoded;
    private bool _initialized;
    private int _callbackFailed;

    public long SubmittedFrames { get; private set; }
    public bool CallbackFailed => Volatile.Read(ref _callbackFailed) != 0;

    public SdlAudioOutput(AudioMixer mixer, in SoundSettings settings, bool muted)
    {
        this._mixer = mixer;
        _channels = settings.OutputMode.Contains(SoundOutputModeElement.Mono) ? 1 : 2;
        _bytesPerSample = settings.OutputMode.Contains(SoundOutputModeElement.Bits8) ? 1 : 2;
        _pcm = new float[Math.Max(1, settings.SampleRate / 20) * 2];
        _encoded = new byte[_pcm.Length / 2 * _channels * _bytesPerSample];
        try
        {
            if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_AUDIO)) throw new InvalidOperationException(SDL_GetError());
            _initialized = true;
            _handle = GCHandle.Alloc(this);
            SDL_SetHint("SDL_AUDIO_DEVICE_SAMPLE_FRAMES", Math.Max(1, settings.SampleRate / 20).ToString(System.Globalization.CultureInfo.InvariantCulture));
            SDL_AudioSpec spec = new()
            {
                freq = settings.SampleRate,
                channels = _channels,
                format = _bytesPerSample == 1 ? SDL_AudioFormat.SDL_AUDIO_U8 : SDL_AudioFormat.SDL_AUDIO_S16LE
            };
            _stream = SDL_OpenAudioDeviceStream(SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK, &spec, &Callback, GCHandle.ToIntPtr(_handle));
            if (_stream is null) throw new InvalidOperationException(SDL_GetError());
            // Gain is set while the device is still paused.
            if (!SDL_SetAudioStreamGain(_stream, muted ? 0 : 1) || !SDL_ResumeAudioStreamDevice(_stream))
                throw new InvalidOperationException(SDL_GetError());
        }
        catch { Dispose(); throw; }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Callback(nint user, SDL_AudioStream* stream, int additional, int total)
    {
        var output = (SdlAudioOutput)GCHandle.FromIntPtr(user).Target!;
        try
        {
            int frameBytes = output._channels * output._bytesPerSample;
            int frames = (Math.Max(0, additional) + frameBytes - 1) / frameBytes;
            while (frames > 0)
            {
                int count = Math.Min(frames, output._pcm.Length / 2);
                output._mixer.Render(output._pcm.AsSpan(0, count * 2));
                for (int i = 0; i < count; i++)
                    for (int channel = 0; channel < output._channels; channel++)
                    {
                        float value = output._channels == 1 ? (output._pcm[i * 2] + output._pcm[i * 2 + 1]) / 2 : output._pcm[i * 2 + channel];
                        int at = (i * output._channels + channel) * output._bytesPerSample;
                        if (output._bytesPerSample == 1) output._encoded[at] = (byte)Math.Clamp((int)Math.Round(value * 128 + 128), 0, 255);
                        else BinaryPrimitives.WriteInt16LittleEndian(output._encoded.AsSpan(at), (short)Math.Clamp((int)Math.Round(value * 32768), -32768, 32767));
                    }
                fixed (byte* data = output._encoded)
                    if (!SDL_PutAudioStreamData(stream, (nint)data, count * frameBytes)) throw new InvalidOperationException();
                output.SubmittedFrames += count;
                frames -= count;
            }
        }
        catch (System.Exception) { Volatile.Write(ref output._callbackFailed, 1); }
    }

    public void Dispose()
    {
        if (_stream is not null) { SDL_DestroyAudioStream(_stream); _stream = null; }
        if (_handle.IsAllocated) _handle.Free();
        if (_initialized) { SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_AUDIO); _initialized = false; }
    }
}
