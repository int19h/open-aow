using Aow.Rtl;
using Aow.Units.Sound;
using SoundApi = Aow.Units.Sound.Sound;

namespace Aow.Platform;

public class TSdlSoundSystem : TSoundSystem
{
    private static bool muted = true, useOutput;
    private static bool registered;
    private static TSdlSoundSystem? active;
    private TSoundSettings settings;
    private SdlAudioOutput? output;
    public AudioMixer Mixer { get; } = new();
    public enum SongState { Idle, Playing, WaitingForFadeIn }
    public SongState State { get; private set; }
    private int volume = 127, fadeIn = 2;
    private AnsiString fileName;
    private bool loop;

    public static void Install(bool mute = true, bool audioOutput = false)
    {
        muted = mute || Environment.GetEnvironmentVariable("AOW_MUTE") == "1";
        useOutput = audioOutput;
        if (!registered)
        {
            MetaRegistry.Register(typeof(TSdlSoundSystem), new SdlMeta(), () => new TSdlSoundSystem(), 12);
            MetaRegistry.Register(typeof(GalaxyAlias), new GalaxyMeta(), () => new GalaxyAlias(), 12);
            MetaRegistry.Register(typeof(SdlSampleData), new TSampleData.Meta(), () => new SdlSampleData(), 12);
            MetaRegistry.Register(typeof(SdlSample), new TSample.Meta(), () => new SdlSample(active!.Mixer), 24);
            registered = true;
        }
        if (SoundApi.SoundSystems.Find("SDL") is null) SoundApi.RegisterSoundSystem(TSoundSystem.Meta.Of<TSdlSoundSystem>());
        if (SoundApi.SoundSystems.Find("Galaxy") is null) SoundApi.RegisterSoundSystem(TSoundSystem.Meta.Of<GalaxyAlias>());
    }
    private sealed class SdlMeta : TSoundSystem.Meta { protected internal override AnsiString GetName() => "SDL"; }
    private sealed class GalaxyMeta : TSoundSystem.Meta { protected internal override AnsiString GetName() => "Galaxy"; }
    private sealed class GalaxyAlias : TSdlSoundSystem { }
    protected internal override TSample.Meta SampleClass() => TSample.Meta.Of<SdlSample>();
    protected internal override TSampleData.Meta SampleDataClass() => TSampleData.Meta.Of<SdlSampleData>();
    protected internal override bool Activate(in TSoundSettings value)
    {
        active = this;
        FStatus = TSet<TSoundSystemStatusElement>.Of(TSoundSystemStatusElement.ssActivated);
        return SetSoundSettings(value);
    }
    protected internal override bool Deactivate()
    {
        output?.Dispose(); output = null;
        Mixer.Dispose();
        State = SongState.Idle;
        FStatus = default;
        if (active == this) active = null;
        return true;
    }
    public override bool SetSoundSettings(in TSoundSettings value)
    {
        if (value.SampleRate <= 0 || value.SampleVoices <= 0) return false;
        output?.Dispose(); output = null;
        lock (Mixer.Sync)
        {
            settings = value;
            Mixer.Rate = value.SampleRate;
            Mixer.VoiceLimit = value.SampleVoices;
            Mixer.MusicEnabled = value.MusicEnabled;
            Mixer.SfxEnabled = value.SFXEnabled;
        }
        if (useOutput)
        {
            try { output = new(Mixer, value, muted); }
            catch (System.Exception ex) when (ex is InvalidOperationException or DllNotFoundException or EntryPointNotFoundException)
            { Console.Error.WriteLine("Audio output is silent: " + ex.Message); }
        }
        return true;
    }
    public override TSoundSettings GetSoundSettings() => settings;
    protected internal override bool GetMusicEnabled() => settings.MusicEnabled;
    protected internal override bool GetSFXEnabled() => settings.SFXEnabled;
    public override bool SetMusicEnabled(bool value)
    {
        lock (Mixer.Sync)
        {
            settings.MusicEnabled = Mixer.MusicEnabled = value;
            if (!value) Mixer.ReplaceSong(null);
        }
        return true;
    }
    public override bool SetSFXEnabled(bool value) { lock (Mixer.Sync) settings.SFXEnabled = Mixer.SfxEnabled = value; return true; }
    private static int Scale(int value) => (int)Math.Round(Math.Clamp(value, 0, 100) * 127.0 / 100);
    public override bool SetMusicVolume(int value) => FadeMusicVolume(value, 0);
    public override bool FadeMusicVolume(int value, int speed)
    {
        lock (Mixer.Sync)
        {
            volume = Scale(value);
            if (State != SongState.WaitingForFadeIn) Mixer.MusicGain.Set(volume, speed);
        }
        return true;
    }
    protected internal override int GetMusicVolume() => (int)Math.Round(volume * 100.0 / 127);
    public override bool SetSFXVolume(int value) => FadeSFXVolume(value, 0);
    public override bool FadeSFXVolume(int value, int speed) { lock (Mixer.Sync) Mixer.SfxGain.Set(Scale(value), speed); return true; }
    // GalSndS.GetSFXVolume divides the 0..127 master by 32767.
    protected internal override int GetSFXVolume() { lock (Mixer.Sync) return (int)Math.Round(Mixer.SfxGain.Value * 100.0 / 32767); }
    protected internal override bool GetPlayingSong() { lock (Mixer.Sync) return State == SongState.WaitingForFadeIn || (Mixer.Song is not null && !Mixer.SongEnded); }
    protected internal override AnsiString GetPlayingSongFileName() => GetPlayingSong() ? fileName : "";
    public override bool PlaySong(AnsiString name) => PlaySongEx(name, 0, 0, false);
    public override bool PlaySongEx(AnsiString name, int fadeOut, int newFadeIn, bool repeat)
    {
        if (!settings.MusicEnabled) return false;
        lock (Mixer.Sync)
        {
            if (fadeOut != 0 && GetPlayingSong())
            {
                fileName = name; loop = repeat; fadeIn = newFadeIn;
                State = SongState.WaitingForFadeIn;
                Mixer.MusicGain.Set(0, fadeOut);
                return true;
            }
            Mixer.ReplaceSong(null);
            State = SongState.Idle;
            try
            {
                using var stream = Aow.Rtl.Rtl.Files.Open((SoundApi.BaseSoundEnginePath + name).ToString(), 0);
                using var data = new MemoryStream(); stream.CopyTo(data);
                var song = OpenMptModule.Load(data.GetBuffer().AsSpan(0, (int)data.Length), repeat);
                if (song is null) return false;
                Mixer.ReplaceSong(song);
                fileName = name; loop = repeat; fadeIn = newFadeIn;
                State = SongState.Playing;
                if (newFadeIn != 0) Mixer.MusicGain.Set(0, 0);
                Mixer.MusicGain.Set(volume, newFadeIn);
                return true;
            }
            catch (IOException ex) { Console.Error.WriteLine("Music is silent: " + ex.Message); return false; }
        }
    }
    protected internal override void Process()
    {
        lock (Mixer.Sync)
            if (State == SongState.WaitingForFadeIn && Mixer.MusicGain.Value == 0)
                PlaySongEx(fileName, 0, fadeIn, loop);
        if (output?.CallbackFailed == true) { Console.Error.WriteLine("Audio callback failed. Output stopped."); output.Dispose(); output = null; }
    }
    public override bool StopSong()
    {
        lock (Mixer.Sync) { Mixer.ReplaceSong(null); State = SongState.Idle; fileName = ""; loop = false; fadeIn = 2; }
        return true;
    }
    public sealed class SavedSong : TSongStatus
    {
        public int Order { get; init; }
        public bool Loop { get; init; }
        public int Volume { get; init; }
        public SavedSong(AnsiString name) => FFileName = name;
    }
    public override TSongStatus BackupSongStatus()
    {
        lock (Mixer.Sync) return GetPlayingSong() ? new SavedSong(fileName) { Order = Mixer.Song?.Order ?? 0, Loop = loop, Volume = volume } : null!;
    }
    public override bool RestoreSongStatus(TSongStatus status)
    {
        lock (Mixer.Sync)
        {
            // The pending restore path in GalSndS deliberately restarts at order zero without looping.
            if (GetPlayingSong())
            {
                fileName = status.FileName; loop = false; fadeIn = 3;
                State = SongState.WaitingForFadeIn; Mixer.MusicGain.Set(0, 2);
                return true;
            }
            bool result = PlaySong(status.FileName);
            if (result && status is SavedSong saved) Mixer.Song!.Order = saved.Order;
            Mixer.MusicGain.Set(0, 0); Mixer.MusicGain.Set(volume, 3);
            return result;
        }
    }
}

internal sealed class SdlSampleData : TSampleData
{
    public WaveData Wave { get; private set; } = new(44100, 1, []);
    public override void Create__init(AnsiString name)
    {
        base.Create__init(name);
        using var stream = Aow.Rtl.Rtl.Files.Open((SoundApi.BaseSoundEnginePath + name).ToString(), 0);
        Wave = WaveData.Read(stream);
    }
    public override void CreateEx__init(in TWaveFormatEx format, object data, int size)
    {
        base.CreateEx__init(format, data, size);
        Wave = WaveData.Decode(format, Aow.Rtl.Rtl.BufferBytes(data)[..size]);
    }
}

internal sealed class SdlSample(AudioMixer mixer) : TSample
{
    private AudioMixer.Voice? voice;
    private int volume = -1;
    private bool loop;
    private TSamplePanning pan = TSamplePanning.spCenter;
    protected internal override bool GetPlaying() { lock (mixer.Sync) return voice?.Playing == true; }
    protected internal override int GetVolume() => volume;
    protected internal override void SetVolume(int value) { lock (mixer.Sync) { volume = value; Apply(); } }
    protected internal override bool GetLooped() => loop;
    protected internal override void SetLooped(bool value) { lock (mixer.Sync) { loop = value; Apply(); } }
    protected internal override TSamplePanning GetPanning() => pan;
    protected internal override void SetPanning(TSamplePanning value) { lock (mixer.Sync) { pan = value; Apply(); } }
    private void Apply()
    {
        if (voice is null) return;
        voice.Loop = loop;
        voice.Volume = volume == -1 ? 1 : (float)(Math.Round(Math.Clamp(volume, 0, 100) * 32767.0 / 100) / 32767);
        voice.Pan = pan switch { TSamplePanning.spLeft => -1, TSamplePanning.spRight => 1, _ => 0 };
    }
    public override void Play() { lock (mixer.Sync) { if (voice?.Playing != true) { voice = mixer.Play(((SdlSampleData)SampleData).Wave); Apply(); } } }
    public override void Stop() { lock (mixer.Sync) { if (voice is not null) voice.Playing = false; voice = null; } }
    public override void Destroy() { Stop(); base.Destroy(); }
}
