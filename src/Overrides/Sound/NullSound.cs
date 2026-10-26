using Aow.Rtl;

namespace Aow.Units.Sound;

public sealed class NullSoundSystem : TSoundSystem
{
    private TSoundSettings settings;
    private int musicVolume, sfxVolume;
    private bool musicEnabled, sfxEnabled;
    private AnsiString song;

    public static void Install()
    {
        MetaRegistry.Register(typeof(NullSoundSystem), new NullMeta(), () => new NullSoundSystem(), 12);
        MetaRegistry.Register(typeof(NullSample), new TSample.Meta(), () => new NullSample(), 24);
        Sound.RegisterSoundSystem((TSoundSystem.Meta)MetaRegistry.Of(typeof(NullSoundSystem)));
        Sound.InitializeSoundSystem("Galaxy", default);
    }

    private sealed class NullMeta : TSoundSystem.Meta
    {
        protected internal override AnsiString GetName() => "Galaxy";
    }
    protected internal override TSample.Meta SampleClass() => (TSample.Meta)MetaRegistry.Of(typeof(NullSample));
    protected internal override TSampleData.Meta SampleDataClass() => TSampleData.Meta.Of<TSampleData>();
    protected internal override bool Activate(in TSoundSettings value) { settings = value; FStatus = TSet<TSoundSystemStatusElement>.Of(TSoundSystemStatusElement.ssActivated); return true; }
    protected internal override bool Deactivate() { FStatus = default; return true; }
    protected internal override void Process()
    {
        for (int i = Sound.SampleList.Count - 1; i >= 0; i--) ((TSample)Sound.SampleList[i]).Update();
    }
    protected internal override int GetMusicVolume() => musicVolume;
    protected internal override int GetSFXVolume() => sfxVolume;
    protected internal override bool GetPlayingSong() => song.Length != 0;
    protected internal override AnsiString GetPlayingSongFileName() => song;
    protected internal override bool GetSFXEnabled() => sfxEnabled;
    protected internal override bool GetMusicEnabled() => musicEnabled;
    public override bool SetSFXEnabled(bool value) { sfxEnabled = value; return true; }
    public override bool SetMusicEnabled(bool value) { musicEnabled = value; return true; }
    public override bool SetSoundSettings(in TSoundSettings value) { settings = value; return true; }
    public override TSoundSettings GetSoundSettings() => settings;
    public override bool FadeMusicVolume(int volume, int speed) => SetMusicVolume(volume);
    public override bool SetMusicVolume(int volume) { musicVolume = volume; return true; }
    public override bool FadeSFXVolume(int volume, int speed) => SetSFXVolume(volume);
    public override bool SetSFXVolume(int volume) { sfxVolume = volume; return true; }
    public override bool PlaySong(AnsiString fileName) { song = fileName; return true; }
    public override bool PlaySongEx(AnsiString fileName, int fadeOut, int fadeIn, bool loop) => PlaySong(fileName);
    public override bool StopSong() { song = default; return true; }
    public override TSongStatus BackupSongStatus() => new() { FFileName = song };
    public override bool RestoreSongStatus(TSongStatus status) { song = status.FileName; return true; }
}

internal sealed class NullSample : TSample
{
    private int volume;
    private bool looped;
    private TSamplePanning panning;
    protected internal override bool GetPlaying() => false;
    protected internal override int GetVolume() => volume;
    protected internal override void SetVolume(int value) => volume = value;
    protected internal override bool GetLooped() => looped;
    protected internal override void SetLooped(bool value) => looped = value;
    protected internal override TSamplePanning GetPanning() => panning;
    protected internal override void SetPanning(TSamplePanning value) => panning = value;
}
