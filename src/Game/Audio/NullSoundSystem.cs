using Aow.Rtl;

namespace Aow.Game.Audio;

public sealed class NullSoundSystem : SoundSystem
{
    private SoundSettings _settings;
    private int _musicVolume, _sfxVolume;
    private bool _musicEnabled, _sfxEnabled;
    private string _song = "";

    public static void Install()
    {
        RegisterSoundSystem(Descriptor);
        AudioPlayback.InitializeSoundSystem("Galaxy", default);
    }

    public static string Name => "Galaxy";
    public static SoundSystemClass Descriptor { get; } = new("Galaxy", static () => new NullSoundSystem());
    public static SampleClass Samples { get; } = new("NullSample", NullSample.Create);

    protected internal override SampleClass SampleClass() => Samples;
    protected internal override SampleDataClass SampleDataClass() => SampleData.Descriptor;

    protected internal override bool Activate(in SoundSettings value) { _settings = value; Status = Set<SoundSystemStatusElement>.Of(SoundSystemStatusElement.Activated); return true; }

    protected internal override bool Deactivate() { Status = default; return true; }

    protected internal override void Process()
    {
        for (int i = AudioHost.SampleList.Count - 1; i >= 0; i--) (AudioHost.SampleList[i]).Update();
    }

    protected internal override int GetMusicVolume() => _musicVolume;
    protected internal override int GetSfxVolume() => _sfxVolume;
    protected internal override bool GetPlayingSong() => _song.Length != 0;
    protected internal override string GetPlayingSongFileName() => _song;
    protected internal override bool GetSfxEnabled() => _sfxEnabled;
    protected internal override bool GetMusicEnabled() => _musicEnabled;

    public override bool SetSfxEnabled(bool value) { _sfxEnabled = value; return true; }

    public override bool SetMusicEnabled(bool value) { _musicEnabled = value; return true; }

    public override bool SetSoundSettings(in SoundSettings value) { _settings = value; return true; }

    public override SoundSettings GetSoundSettings() => _settings;
    public override bool FadeMusicVolume(int volume, int speed) => SetMusicVolume(volume);

    public override bool SetMusicVolume(int volume) { _musicVolume = volume; return true; }

    public override bool FadeSfxVolume(int volume, int speed) => SetSfxVolume(volume);

    public override bool SetSfxVolume(int volume) { _sfxVolume = volume; return true; }

    public override bool PlaySong(string fileName) { _song = fileName; return true; }

    public override bool PlaySong(string fileName, int fadeOut, int fadeIn, bool loop) => PlaySong(fileName);

    public override bool StopSong() { _song = ""; return true; }

    public override SongStatus BackupSongStatus() => new() { FileName = _song };

    public override bool RestoreSongStatus(SongStatus status) { _song = status.FileName; return true; }

    internal static void RegisterSoundSystem(SoundSystemClass classValue)
    {
        if (AudioHost.SoundSystems == null)
        {
            AudioHost.SoundSystems = new SoundSystemList();
        }
        string name = classValue.Name;
        if (AudioHost.SoundSystems.Find(name) == null)
        {
            AudioHost.SoundSystems.Add(classValue);
            return;
        }
        throw new InvalidOperationException("Sound System with name: " + classValue.Name + " already registered");
    }
}
