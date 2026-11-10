using Aow.Game.Audio;
using Aow.Rtl;

namespace Aow.Platform;

public class SdlSoundSystem : SoundSystem
{
    private static bool muted = true, useOutput;
    private static SdlSoundSystem? active;
    private SoundSettings settings;
    private SdlAudioOutput? output;

    public AudioMixer Mixer { get; } = new();

    public enum SongState { Idle, Playing, WaitingForFadeIn }

    public SongState State { get; private set; }

    private int volume = 127, fadeIn = 2;
    private string fileName = "";
    private bool loop;

    public static void Install(bool mute = true, bool audioOutput = false)
    {
        muted = mute || Environment.GetEnvironmentVariable("AOW_MUTE") == "1";
        useOutput = audioOutput;
        if (Aow.Game.Audio.AudioHost.SoundSystems.Find("SDL") is null) Aow.Game.Audio.NullSoundSystem.RegisterSoundSystem(Descriptor);
        if (Aow.Game.Audio.AudioHost.SoundSystems.Find("Galaxy") is null) Aow.Game.Audio.NullSoundSystem.RegisterSoundSystem(Galaxy);
    }

    public static string Name => "SDL";
    public static SoundSystemClass Descriptor { get; } = new("SDL", static () => new SdlSoundSystem());
    public static SoundSystemClass Galaxy { get; } = new("Galaxy", static () => new SdlSoundSystem());
    public static SampleClass Samples { get; } = new("SdlSample", static data => SdlSample.Create(active!.Mixer, data));
    public static SampleDataClass SampleData { get; } = new("SdlSampleData", SdlSampleData.Create, SdlSampleData.CreateEx);

    protected internal override SampleClass SampleClass() => Samples;
    protected internal override SampleDataClass SampleDataClass() => SampleData;

    protected internal override bool Activate(in SoundSettings value)
    {
        active = this;
        FStatus = Set<SoundSystemStatusElement>.Of(SoundSystemStatusElement.Activated);
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

    public override bool SetSoundSettings(in SoundSettings value)
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

    public override SoundSettings GetSoundSettings() => settings;
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

    protected internal override string GetPlayingSongFileName() => GetPlayingSong() ? fileName : "";
    public override bool PlaySong(string name) => PlaySongEx(name, 0, 0, false);

    public override bool PlaySongEx(string name, int fadeOut, int newFadeIn, bool repeat)
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
                using var stream = RtlUnit.Files.Open(AudioHost.BaseSoundEnginePath + name, 0);
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

    public sealed class SavedSong : SongStatus
    {
        public int Order { get; init; }
        public bool Loop { get; init; }
        public int Volume { get; init; }

        public SavedSong(string name) => FFileName = name;
    }

    public override SongStatus BackupSongStatus()
    {
        lock (Mixer.Sync) return GetPlayingSong() ? new SavedSong(fileName) { Order = Mixer.Song?.Order ?? 0, Loop = loop, Volume = volume } : null!;
    }

    public override bool RestoreSongStatus(SongStatus status)
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

internal class SdlSampleData : SampleData
{
    public WaveData Wave { get; private set; } = new(44100, 1, []);

    public static new SdlSampleData Create(string name) =>
        ConstructionContext.Create(name, static (context, value) => new SdlSampleData(context, value));

    public static new SdlSampleData CreateEx(in WaveFormatEx format, object data, int size) =>
        ConstructionContext.Create((format, data, size), static (context, value) =>
            new SdlSampleData(context, value.format, value.data, value.size));

    private protected SdlSampleData(ConstructionContext context, string name) : base(context, name)
    {
        using var stream = RtlUnit.Files.Open(AudioHost.BaseSoundEnginePath + name, 0);
        Wave = WaveData.Read(stream);
    }

    private protected SdlSampleData(ConstructionContext context, in WaveFormatEx format, object data, int size) : base(context, format, data, size)
    {
        Wave = WaveData.Decode(format, RtlUnit.BufferBytes(data)[..size]);
    }
}

internal class SdlSample : Sample
{
    private readonly AudioMixer mixer;

    private protected SdlSample(ConstructionContext context, AudioMixer mixer, SampleData data) : base(context, data)
    {
        this.mixer = mixer;
    }

    public static SdlSample Create(AudioMixer mixer, SampleData data) =>
        ConstructionContext.Create((mixer, data), static (context, value) => new SdlSample(context, value.mixer, value.data));

    private AudioMixer.Voice? voice;
    private int volume = -1;
    private bool loop;
    private SamplePanning pan = SamplePanning.Center;

    protected internal override bool GetPlaying() { lock (mixer.Sync) return voice?.Playing == true; }

    protected internal override int GetVolume() => volume;

    protected internal override void SetVolume(int value) { lock (mixer.Sync) { volume = value; Apply(); } }

    protected internal override bool GetLooped() => loop;

    protected internal override void SetLooped(bool value) { lock (mixer.Sync) { loop = value; Apply(); } }

    protected internal override SamplePanning GetPanning() => pan;

    protected internal override void SetPanning(SamplePanning value) { lock (mixer.Sync) { pan = value; Apply(); } }

    private void Apply()
    {
        if (voice is null) return;
        voice.Loop = loop;
        voice.Volume = volume == -1 ? 1 : (float)(Math.Round(Math.Clamp(volume, 0, 100) * 32767.0 / 100) / 32767);
        voice.Pan = pan switch { SamplePanning.Left => -1, SamplePanning.Right => 1, _ => 0 };
    }

    public override void Play() { lock (mixer.Sync) { if (voice?.Playing != true) { voice = mixer.Play(((SdlSampleData)SampleData).Wave); Apply(); } } }

    public override void Stop() { lock (mixer.Sync) { if (voice is not null) voice.Playing = false; voice = null; } }

    public override void Dispose() { Stop(); base.Dispose(); }
}
