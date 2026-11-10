using Aow.Rtl;

namespace Aow.Game.Audio;

public sealed class NullSoundSystem : SoundSystem
{
    private SoundSettings settings;
    private int musicVolume, sfxVolume;
    private bool musicEnabled, sfxEnabled;
    private string song = "";

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

    protected internal override bool Activate(in SoundSettings value) { settings = value; FStatus = Set<SoundSystemStatusElement>.Of(SoundSystemStatusElement.Activated); return true; }

    protected internal override bool Deactivate() { FStatus = default; return true; }

    protected internal override void Process()
    {
        for (int i = AudioHost.SampleList.Count - 1; i >= 0; i--) ((Sample)AudioHost.SampleList[i]).Update();
    }

    protected internal override int GetMusicVolume() => musicVolume;
    protected internal override int GetSFXVolume() => sfxVolume;
    protected internal override bool GetPlayingSong() => song.Length != 0;
    protected internal override string GetPlayingSongFileName() => song;
    protected internal override bool GetSFXEnabled() => sfxEnabled;
    protected internal override bool GetMusicEnabled() => musicEnabled;

    public override bool SetSFXEnabled(bool value) { sfxEnabled = value; return true; }

    public override bool SetMusicEnabled(bool value) { musicEnabled = value; return true; }

    public override bool SetSoundSettings(in SoundSettings value) { settings = value; return true; }

    public override SoundSettings GetSoundSettings() => settings;
    public override bool FadeMusicVolume(int volume, int speed) => SetMusicVolume(volume);

    public override bool SetMusicVolume(int volume) { musicVolume = volume; return true; }

    public override bool FadeSFXVolume(int volume, int speed) => SetSFXVolume(volume);

    public override bool SetSFXVolume(int volume) { sfxVolume = volume; return true; }

    public override bool PlaySong(string fileName) { song = fileName; return true; }

    public override bool PlaySongEx(string fileName, int fadeOut, int fadeIn, bool loop) => PlaySong(fileName);

    public override bool StopSong() { song = ""; return true; }

    public override SongStatus BackupSongStatus() => new() { FFileName = song };

    public override bool RestoreSongStatus(SongStatus status) { song = status.FileName; return true; }

    internal static void RegisterSoundSystem(SoundSystemClass AClass)
    {
        if (AudioHost.SoundSystems == null)
        {
            AudioHost.SoundSystems = new SoundSystemList();
        }
        string name = AClass.Name;
        if (AudioHost.SoundSystems.Find(name) == null)
        {
            ((DelphiList)AudioHost.SoundSystems).Add((object)AClass);
            return;
        }
        throw new InvalidOperationException("Sound System with name: " + AClass.Name + " already registered");
    }
}
