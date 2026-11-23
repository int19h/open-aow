#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Game.Audio;
using Aow.Rtl;

namespace Aow.Platform;

public sealed class SdlSoundSystem : SoundSystem
{
    private static bool s_muted = true, s_useOutput;
    private static SdlSoundSystem? s_active;
    private SoundSettings _settings;
    private SdlAudioOutput? _output;

    public AudioMixer Mixer { get; } = new();

    public enum SongState { Idle, Playing, WaitingForFadeIn }

    public SongState State { get; private set; }

    private int _volume = 127, _fadeIn = 2;
    private string _fileName = "";
    private bool _loop;

    public static void Install(bool mute = true, bool audioOutput = false)
    {
        s_muted = mute || Environment.GetEnvironmentVariable("AOW_MUTE") == "1";
        s_useOutput = audioOutput;
        if (AudioHost.SoundSystems.Find("SDL") is null)
        {
            NullSoundSystem.RegisterSoundSystem(Descriptor);
        }

        if (AudioHost.SoundSystems.Find("Galaxy") is null)
        {
            NullSoundSystem.RegisterSoundSystem(Galaxy);
        }
    }

    public static string Name => "SDL";
    public static SoundSystemClass Descriptor { get; } = new("SDL", static () => new SdlSoundSystem());
    public static SoundSystemClass Galaxy { get; } = new("Galaxy", static () => new SdlSoundSystem());
    public static SampleClass Samples { get; } = new("SdlSample", static data => SdlSample.Create(s_active!.Mixer, data));
    public static SampleDataClass SampleData { get; } = new("SdlSampleData", SdlSampleData.Create, createEx: SdlSampleData.Create);

    protected internal override SampleClass SampleClass() => Samples;
    protected internal override SampleDataClass SampleDataClass() => SampleData;

    protected internal override bool Activate(in SoundSettings value)
    {
        s_active = this;
        Status = SoundSystemStatuses.Activated;
        return SetSoundSettings(value);
    }

    protected internal override bool Deactivate()
    {
        _output?.Dispose();
        _output = null;
        Mixer.Dispose();
        State = SongState.Idle;
        Status = default;
        if (s_active == this)
        {
            s_active = null;
        }

        return true;
    }

    public override bool SetSoundSettings(in SoundSettings value)
    {
        if (value.SampleRate <= 0 || value.SampleVoices <= 0)
        {
            return false;
        }

        _output?.Dispose();
        _output = null;
        lock (Mixer.Sync)
        {
            _settings = value;
            Mixer.Rate = value.SampleRate;
            Mixer.VoiceLimit = value.SampleVoices;
            Mixer.MusicEnabled = value.MusicEnabled;
            Mixer.SfxEnabled = value.SfxEnabled;
        }
        if (s_useOutput)
        {
            try
            {
                _output = new(Mixer, settings: value, s_muted);
            }
            catch (Exception ex) when (ex is InvalidOperationException or DllNotFoundException or EntryPointNotFoundException)
            {
                HostLog.FallbackWithText(RtlHost.Logger, error: ex, text: "Audio output is silent", location: "SdlSoundSystem.SetSoundSettings");
            }
        }
        return true;
    }

    public override SoundSettings SoundSettings => _settings;
    public override bool MusicEnabled => _settings.MusicEnabled;
    public override bool SfxEnabled => _settings.SfxEnabled;

    public override bool SetMusicEnabled(bool value)
    {
        lock (Mixer.Sync)
        {
            _settings.MusicEnabled = Mixer.MusicEnabled = value;
            if (!value)
            {
                Mixer.ReplaceSong(null);
            }
        }
        return true;
    }

    public override bool SetSfxEnabled(bool value)
    {
        lock (Mixer.Sync)
        {
            _settings.SfxEnabled = Mixer.SfxEnabled = value;
        }

        return true;
    }

    private static int Scale(int value) => (int)Math.Round(Math.Clamp(value, 0, 100) * 127.0 / 100);
    public override bool SetMusicVolume(int value) => FadeMusicVolume(value, speed: 0);

    public override bool FadeMusicVolume(int value, int speed)
    {
        lock (Mixer.Sync)
        {
            _volume = Scale(value);
            if (State != SongState.WaitingForFadeIn)
            {
                Mixer.MusicGain.Set(_volume, speed);
            }
        }
        return true;
    }

    public override int MusicVolume => (int)Math.Round(_volume * 100.0 / 127);

    public override bool SetSfxVolume(int value) => FadeSfxVolume(value, speed: 0);

    public override bool FadeSfxVolume(int value, int speed)
    {
        lock (Mixer.Sync)
        {
            Mixer.SfxGain.Set(Scale(value), speed);
        }

        return true;
    }

    // GalSndS.GetSFXVolume divides the 0..127 master by 32767.
    protected internal override int GetSfxVolume()
    {
        lock (Mixer.Sync)
        {
            return (int)Math.Round(Mixer.SfxGain.Value * 100.0 / 32767);
        }
    }

    protected internal override bool GetPlayingSong()
    {
        lock (Mixer.Sync)
        {
            return State == SongState.WaitingForFadeIn || (Mixer.Song is not null && !Mixer.SongEnded);
        }
    }

    protected internal override string GetPlayingSongFileName() => GetPlayingSong() ? _fileName : "";
    public override bool PlaySong(string name) => PlaySong(name, fadeOut: 0, newFadeIn: 0, repeat: false);

    public override bool PlaySong(string name, int fadeOut, int newFadeIn, bool repeat)
    {
        if (!_settings.MusicEnabled)
        {
            return false;
        }

        lock (Mixer.Sync)
        {
            if (fadeOut != 0 && GetPlayingSong())
            {
                _fileName = name;
                _loop = repeat;
                _fadeIn = newFadeIn;
                State = SongState.WaitingForFadeIn;
                Mixer.MusicGain.Set(0, speed: fadeOut);
                return true;
            }
            Mixer.ReplaceSong(null);
            State = SongState.Idle;
            try
            {
                using Stream stream = RtlHost.Files.Open(AudioHost.BaseSoundEnginePath + name, mode: FileOpenMode.Read);
                using MemoryStream data = new();
                stream.CopyTo(data);
                TrackerSong? song = TrackerSong.Load(data.GetBuffer().AsSpan(0, length: (int)data.Length), loop: repeat);
                if (song is null)
                {
                    return false;
                }

                Mixer.ReplaceSong(song);
                _fileName = name;
                _loop = repeat;
                _fadeIn = newFadeIn;
                State = SongState.Playing;
                if (newFadeIn != 0)
                {
                    Mixer.MusicGain.Set(0, speed: 0);
                }

                Mixer.MusicGain.Set(_volume, speed: newFadeIn);
                return true;
            }
            catch (IOException ex)
            {
                HostLog.FallbackWithText(RtlHost.Logger, error: ex, text: "Music is silent", location: "SdlSoundSystem.PlaySong");
                return false;
            }
        }
    }

    protected internal override void Process()
    {
        lock (Mixer.Sync)
        {
            if (State == SongState.WaitingForFadeIn && Mixer.MusicGain.Value == 0)
            {
                _ = PlaySong(_fileName, fadeOut: 0, newFadeIn: _fadeIn, repeat: _loop);
            }
        }

        if (_output?.CallbackFailed == true)
        {
            HostLog.Error(RtlHost.Logger, text: "Audio callback failed. Output stopped.");
            _output.Dispose();
            _output = null;
        }
    }

    public override bool StopSong()
    {
        lock (Mixer.Sync)
        {
            Mixer.ReplaceSong(null);
            State = SongState.Idle;
            _fileName = "";
            _loop = false;
            _fadeIn = 2;
        }
        return true;
    }

    public sealed class SavedSong : SongStatus
    {
        public int Order { get; init; }
        public bool Loop { get; init; }
        public int Volume { get; init; }

        public SavedSong(string name) => FileName = name;
    }

    public override SongStatus BackupSongStatus()
    {
        lock (Mixer.Sync)
        {
            return GetPlayingSong() ? new SavedSong(_fileName) { Order = Mixer.Song?.Order ?? 0, Loop = _loop, Volume = _volume } : null!;
        }
    }

    public override bool RestoreSongStatus(SongStatus status)
    {
        lock (Mixer.Sync)
        {
            // The pending restore path in GalSndS deliberately restarts at order zero without looping.
            if (GetPlayingSong())
            {
                _fileName = status.FileName;
                _loop = false;
                _fadeIn = 3;
                State = SongState.WaitingForFadeIn;
                Mixer.MusicGain.Set(0, speed: 2);
                return true;
            }
            bool result = PlaySong(status.FileName);
            if (result && status is SavedSong saved)
            {
                Mixer.Song!.Order = saved.Order;
            }

            Mixer.MusicGain.Set(0, speed: 0);
            Mixer.MusicGain.Set(_volume, speed: 3);
            return result;
        }
    }
}

internal class SdlSampleData : SampleData
{
    public WaveData Wave { get; } = new(44100, Channels: 1, Samples: []);

    public static new SdlSampleData Create(string name)
        => ConstructionContext.Create(name, static (context, value) => new SdlSampleData(context, name: value));

    public static new SdlSampleData Create(in WaveFormat format, object data, int size)
        => ConstructionContext.Create((format, data, size), static (context, value) =>
            new SdlSampleData(context, value.format, value.data, value.size));

    private protected SdlSampleData(ConstructionContext context, string name) : base(context, name)
    {
        using Stream stream = RtlHost.Files.Open(AudioHost.BaseSoundEnginePath + name, mode: FileOpenMode.Read);
        Wave = WaveData.Read(stream);
    }

    private protected SdlSampleData(ConstructionContext context, in WaveFormat format, object data, int size) : base(context, format, data, size) => Wave = WaveData.Decode(format, data: PointerMemory.BufferBytes(data)[..size]);
}

internal class SdlSample : Sample
{
    private readonly AudioMixer _mixer;

    private protected SdlSample(ConstructionContext context, AudioMixer mixer, SampleData data) : base(context, sampleData: data) => _mixer = mixer;

    public static SdlSample Create(AudioMixer mixer, SampleData data)
        => ConstructionContext.Create((mixer, data), static (context, value) => new SdlSample(context, value.mixer, value.data));

    private AudioMixer.Voice? _voice;
    private int _volume = -1;
    private bool _loop;
    private SamplePanning _pan = SamplePanning.Center;

    protected internal override bool GetPlaying()
    {
        lock (_mixer.Sync)
        {
            return _voice?.Playing == true;
        }
    }

    public override int Volume
    {
        get => _volume;
        set
        {
            lock (_mixer.Sync)
            {
                _volume = value;
                Apply();
            }
        }
    }

    public override bool Looped
    {
        get => _loop;
        set
        {
            lock (_mixer.Sync)
            {
                _loop = value;
                Apply();
            }
        }
    }

    public override SamplePanning Panning
    {
        get => _pan;
        set
        {
            lock (_mixer.Sync)
            {
                _pan = value;
                Apply();
            }
        }
    }

    private void Apply()
    {
        if (_voice is null)
        {
            return;
        }

        _voice.Loop = _loop;
        _voice.Volume = _volume == -1 ? 1 : (float)(Math.Round(Math.Clamp(_volume, 0, 100) * 32767.0 / 100) / 32767);
        _voice.Pan = _pan switch { SamplePanning.Left => -1, SamplePanning.Right => 1, SamplePanning.Center or SamplePanning.Surround or _ => 0 };
    }

    public override void Play()
    {
        lock (_mixer.Sync)
        {
            if (_voice?.Playing != true)
            {
                _voice = _mixer.Play(((SdlSampleData)SampleData).Wave);
                Apply();
            }
        }
    }

    public override void Stop()
    {
        lock (_mixer.Sync)
        {
            _voice?.Playing = false;
            _voice = null;
        }
    }

    public override void Dispose()
    {
        Stop();
        base.Dispose();
    }
}
