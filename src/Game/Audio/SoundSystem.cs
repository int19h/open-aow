// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FRefCount, FStatus. The abstract sound system. GetName is a class function
// (RegisterSoundSystem calls it on the class)
public partial class SoundSystem : IDisposable
{
    protected internal int FRefCount;
    protected internal Set<SoundSystemStatusElement> FStatus;

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
    protected internal virtual bool Activate(in SoundSettings Settings)
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
    protected internal virtual int GetSFXVolume()
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
    protected internal virtual bool GetSFXEnabled()
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
        FRefCount = 1;
    }

    // Sound.pas:1165
    public virtual int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Sound.pas:1173
    public virtual int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Sound.pas:130
    public virtual bool SetSFXEnabled(bool Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:131
    public virtual bool SetMusicEnabled(bool Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:132
    public virtual bool SetSoundSettings(in SoundSettings Settings)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:133
    public virtual SoundSettings GetSoundSettings()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:134
    public virtual bool FadeMusicVolume(int Volume, int Speed)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:135
    public virtual bool SetMusicVolume(int Volume)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:136
    public virtual bool FadeSFXVolume(int Volume, int Speed)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:137
    public virtual bool SetSFXVolume(int Volume)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:138
    public virtual bool PlaySong(string FileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:139
    public virtual bool PlaySongEx(string FileName, int FadeOut, int FadeIn, bool Loop)
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
    public virtual bool RestoreSongStatus(SongStatus Status)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:1158
    public virtual void Dispose()
    {
        if (FStatus.Contains(SoundSystemStatusElement.Activated))
        {
            Deactivate();
        }
    }

    public Set<SoundSystemStatusElement> Status { get => FStatus; }
    public int MusicVolume { get => GetMusicVolume(); }
    public int SFXVolume { get => GetSFXVolume(); }
    public bool PlayingSong { get => GetPlayingSong(); }
    public string PlayingSongFileName { get => GetPlayingSongFileName(); }
    public bool SFXEnabled { get => GetSFXEnabled(); }
    public bool MusicEnabled { get => GetMusicEnabled(); }

}
