// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// FCurrentSong, FStatus, FSongs, FSongNames, FCurrentIndex, FOnSongChange, FBackup (Pause),
// FPauseCount. Plays the songs in turn (Update starts the next one)
public partial class SongPlayer : IDisposable
{
    internal string FCurrentSong = "";
    internal Set<StatusElement> FStatus;
    internal StringList FSongs;
    internal StringList FSongNames;
    internal int FCurrentIndex;
    internal NotifyEvent<SongPlayer> FOnSongChange;
    internal SongStatus FBackup;
    internal int FPauseCount;

    // Sound.pas:870
    internal void SetSongs(StringList Value)
    {
        FSongs.Assign(Value);
    }

    // Sound.pas:875
    internal void SongsChanged(object Sender)
    {
        FSongNames.Clear();
        for (int I = FSongs.Count - 1; I >= 0; I--)
        {
            try
            {
                var songName = AudioPlayback.GetSongName(FSongs[I]);
                FSongNames.Insert(0, songName);
            }
            catch
            {
                FSongs.Delete(I);
            }
        }

        if (FStatus.Contains(StatusElement.Playing))
        {
            if (FSongs.Count != 0)
            {
                if (FCurrentIndex != -1)
                {
                    var count = FSongs.Count;
                    if (FCurrentIndex >= count || FSongs[FCurrentIndex] != FCurrentSong)
                    {
                        FCurrentIndex = FSongs.IndexOf(FCurrentSong);
                        if (FCurrentIndex == -1)
                        {
                            FCurrentSong = "";
                        }
                    }
                }

                if (FCurrentIndex == -1 && !FStatus.Contains(StatusElement.Paused))
                {
                    Play(0);
                }
            }
            else
            {
                if (AudioHost.SoundSystem != null)
                {
                    AudioHost.SoundSystem.StopSong();
                }

                FCurrentIndex = -1;
                FCurrentSong = "";
            }
        }
    }

    // Sound.pas:854
    public SongPlayer()
    {
        FSongs = new StringList();
        FSongs.OnChange = SongsChanged;
        FSongNames = new StringList();
        FCurrentIndex = -1;
    }

    // Sound.pas:863
    public virtual void Dispose()
    {
        FSongs?.Dispose();
        FSongNames?.Dispose();
    }

    // Sound.pas:909
    // { Called every tick (ProcessSound): starts the next song when the last one has ended. }
    public void Update()
    {
        if (AudioHost.SoundSystem != null && FStatus.Contains(StatusElement.Playing) && !FStatus.Contains(StatusElement.Paused) && !AudioHost.SoundSystem.PlayingSong && AudioHost.SoundSystem.MusicEnabled)
        {
            FCurrentIndex += 1;
            if (FSongs.Count <= FCurrentIndex)
            {
                FCurrentIndex = 0;
            }

            var count = FSongs.Count;
            if (FCurrentIndex < count)
            {
                Play(FCurrentIndex);
            }
            else
            {
                FCurrentIndex = -1;
            }
        }
    }

    // Sound.pas:924
    public bool PlayEx(int Index, int FadeOut, int FadeIn)
    {
        bool Result;
        if (AudioHost.SoundSystem != null && Index >= 0 && Index < FSongs.Count)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Playing);
            var item = FSongs[Index];
            Result = AudioHost.SoundSystem.PlaySongEx(item, FadeOut, FadeIn, false);
            if (Result)
            {
                FCurrentIndex = Index;
                FCurrentSong = FSongs[Index];
                if (FOnSongChange != null)
                {
                    FOnSongChange(this);
                }
            }
            else
            {
                FCurrentSong = "";
                FCurrentIndex = -1;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Sound.pas:947
    public bool Play(int Index)
    {
        bool Result;
        if (AudioHost.SoundSystem != null && Index >= 0 && Index < FSongs.Count)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Playing);
            var item = FSongs[Index];
            Result = AudioHost.SoundSystem.PlaySongEx(item, 2, 0, false);
            if (Result)
            {
                FCurrentIndex = Index;
                FCurrentSong = FSongs[Index];
                if (FOnSongChange != null)
                {
                    FOnSongChange(this);
                }
            }
            else
            {
                FCurrentSong = "";
                FCurrentIndex = -1;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Sound.pas:970
    public bool Stop(bool StopMusic)
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            if (StopMusic)
            {
                Result = AudioHost.SoundSystem.StopSong();
            }
            else
            {
                Result = false;
            }

            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Playing);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Sound.pas:985
    // { Pauses count; the first one saves the song position. }
    public bool Pause(bool StopMusic)
    {
        if (FPauseCount == 0)
        {
            if (FBackup != null)
            {
            }

            FBackup = AudioPlayback.BackupSongStatus();
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Paused);
            if (StopMusic)
            {
                AudioHost.SoundSystem.StopSong();
            }
        }

        FPauseCount += 1;
        return true;
    }

    // Sound.pas:1000
    public bool Resume()
    {
        FPauseCount -= 1;
        if (FPauseCount == 0)
        {
            if (FBackup != null)
            {
                FCurrentIndex = FSongs.IndexOf(FBackup.FFileName);
                if (FCurrentIndex == -1)
                {
                    Play(0);
                }
                else
                {
                    SongStatus.RestoreSongStatus(FBackup);
                }
                FBackup = null;
            }

            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Paused);
        }

        return true;
    }

    public string CurrentSong { get => FCurrentSong; }
    public Set<StatusElement> Status { get => FStatus; }
    public StringList Songs { get => FSongs; set => SetSongs(value); }
    public StringList SongNames { get => FSongNames; }
    public int CurrentIndex { get => FCurrentIndex; }
    public NotifyEvent<SongPlayer> OnSongChange { get => FOnSongChange; set => FOnSongChange = value; }

    public enum StatusElement : byte
    {
        Playing = 0, Paused = 1
    }

}
