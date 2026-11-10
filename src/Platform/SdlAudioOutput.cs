using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Game.Audio;
using SDL;
using static SDL.SDL3;

namespace Aow.Platform;

public sealed unsafe class SdlAudioOutput : IDisposable
{
    private SDL_AudioStream* stream;
    private GCHandle handle;
    private readonly AudioMixer mixer;
    private readonly int channels, bytesPerSample;
    private readonly float[] pcm;
    private readonly byte[] encoded;
    private bool initialized;
    private int callbackFailed;

    public long SubmittedFrames { get; private set; }
    public bool CallbackFailed => Volatile.Read(ref callbackFailed) != 0;

    public SdlAudioOutput(AudioMixer mixer, in TSoundSettings settings, bool muted)
    {
        this.mixer = mixer;
        channels = settings.OutputMode.Contains(TSoundOutputModeElement.omMono) ? 1 : 2;
        bytesPerSample = settings.OutputMode.Contains(TSoundOutputModeElement.om8bit) ? 1 : 2;
        pcm = new float[Math.Max(1, settings.SampleRate / 20) * 2];
        encoded = new byte[pcm.Length / 2 * channels * bytesPerSample];
        try
        {
            if (!SDL_InitSubSystem(SDL_InitFlags.SDL_INIT_AUDIO)) throw new InvalidOperationException(SDL_GetError());
            initialized = true;
            handle = GCHandle.Alloc(this);
            SDL_SetHint("SDL_AUDIO_DEVICE_SAMPLE_FRAMES", Math.Max(1, settings.SampleRate / 20).ToString(System.Globalization.CultureInfo.InvariantCulture));
            SDL_AudioSpec spec = new()
            {
                freq = settings.SampleRate,
                channels = channels,
                format = bytesPerSample == 1 ? SDL_AudioFormat.SDL_AUDIO_U8 : SDL_AudioFormat.SDL_AUDIO_S16LE
            };
            stream = SDL_OpenAudioDeviceStream(SDL_AUDIO_DEVICE_DEFAULT_PLAYBACK, &spec, &Callback, GCHandle.ToIntPtr(handle));
            if (stream is null) throw new InvalidOperationException(SDL_GetError());
            // Gain is set while the device is still paused.
            if (!SDL_SetAudioStreamGain(stream, muted ? 0 : 1) || !SDL_ResumeAudioStreamDevice(stream))
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
            int frameBytes = output.channels * output.bytesPerSample;
            int frames = (Math.Max(0, additional) + frameBytes - 1) / frameBytes;
            while (frames > 0)
            {
                int count = Math.Min(frames, output.pcm.Length / 2);
                output.mixer.Render(output.pcm.AsSpan(0, count * 2));
                for (int i = 0; i < count; i++)
                    for (int channel = 0; channel < output.channels; channel++)
                    {
                        float value = output.channels == 1 ? (output.pcm[i * 2] + output.pcm[i * 2 + 1]) / 2 : output.pcm[i * 2 + channel];
                        int at = (i * output.channels + channel) * output.bytesPerSample;
                        if (output.bytesPerSample == 1) output.encoded[at] = (byte)Math.Clamp((int)Math.Round(value * 128 + 128), 0, 255);
                        else BinaryPrimitives.WriteInt16LittleEndian(output.encoded.AsSpan(at), (short)Math.Clamp((int)Math.Round(value * 32768), -32768, 32767));
                    }
                fixed (byte* data = output.encoded)
                    if (!SDL_PutAudioStreamData(stream, (nint)data, count * frameBytes)) throw new InvalidOperationException();
                output.SubmittedFrames += count;
                frames -= count;
            }
        }
        catch (System.Exception) { Volatile.Write(ref output.callbackFailed, 1); }
    }

    public void Dispose()
    {
        if (stream is not null) { SDL_DestroyAudioStream(stream); stream = null; }
        if (handle.IsAllocated) handle.Free();
        if (initialized) { SDL_QuitSubSystem(SDL_InitFlags.SDL_INIT_AUDIO); initialized = false; }
    }
}
