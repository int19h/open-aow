// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FRefCount, FStatus. The abstract sound system. GetName is a class function
// (RegisterSoundSystem calls it on the class)
public partial class SoundSystem : IDisposable
{
    protected internal int RefCount { get; private set; }

    private Set<SoundSystemStatusElement> _status;

    // Sound.pas:115
    protected internal virtual SampleClass SampleClass()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:116
    protected internal virtual SampleDataClass SampleDataClass()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:117
    protected internal virtual bool Activate(in SoundSettings settings)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:118
    protected internal virtual bool Deactivate()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:119
    protected internal virtual void Process()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:120
    protected internal virtual int GetMusicVolume()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:121
    protected internal virtual int GetSfxVolume()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:122
    protected internal virtual bool GetPlayingSong()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:123
    protected internal virtual string GetPlayingSongFileName()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:124
    protected internal virtual bool GetSfxEnabled()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:125
    protected internal virtual bool GetMusicEnabled()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:1152
    public SoundSystem()
    {
        RefCount = 1;
    }

    // Sound.pas:1165
    public virtual int Release()
    {
        RefCount -= 1;
        int result = RefCount;
        if (RefCount == 0)
        {
            Dispose();
        }

        return result;
    }

    // Sound.pas:1173
    public virtual int AddRef()
    {
        RefCount += 1;
        return RefCount;
    }

    // Sound.pas:130
    public virtual bool SetSfxEnabled(bool value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:131
    public virtual bool SetMusicEnabled(bool value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:132
    public virtual bool SetSoundSettings(in SoundSettings settings)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:133
    public virtual SoundSettings GetSoundSettings()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:134
    public virtual bool FadeMusicVolume(int volume, int speed)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:135
    public virtual bool SetMusicVolume(int volume)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:136
    public virtual bool FadeSfxVolume(int volume, int speed)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:137
    public virtual bool SetSfxVolume(int volume)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:138
    public virtual bool PlaySong(string fileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:139
    public virtual bool PlaySong(string fileName, int fadeOut, int fadeIn, bool loop)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:141
    public virtual bool StopSong()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:142
    public virtual SongStatus BackupSongStatus()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:143
    public virtual bool RestoreSongStatus(SongStatus status)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:1158
    public virtual void Dispose()
    {
        if (_status.Contains(SoundSystemStatusElement.Activated))
        {
            Deactivate();
        }
    }

    public Set<SoundSystemStatusElement> Status { get => _status; protected set => _status = value; }
    public int MusicVolume { get => GetMusicVolume(); }
    public int SfxVolume { get => GetSfxVolume(); }
    public bool PlayingSong { get => GetPlayingSong(); }
    public string PlayingSongFileName { get => GetPlayingSongFileName(); }
    public bool SfxEnabled { get => GetSfxEnabled(); }
    public bool MusicEnabled { get => GetMusicEnabled(); }

}
