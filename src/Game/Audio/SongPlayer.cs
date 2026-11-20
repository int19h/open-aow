#nullable disable
// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// _currentSong, _status, _songs, _songNames, _currentIndex, OnSongChange, _backup (Pause),
// FPauseCount. Plays the songs in turn (Update starts the next one)
public partial class SongPlayer : IDisposable
{
    private readonly DelphiStringList _songs;

    public event NotifyEvent<SongPlayer> OnSongChange;

    internal void SetSongChangeHandler(NotifyEvent<SongPlayer> handler) => OnSongChange = handler;

    private SongStatus _backup;
    private int _pauseCount;

    // Sound.pas:870
    internal void SetSongs(DelphiStringList value) => _songs.Assign(value);

    // Sound.pas:875
    internal void SongsChanged(object sender)
    {
        SongNames.Clear();
        for (int i = _songs.Count - 1; i >= 0; i--)
        {
            try
            {
                string songName = AudioPlayback.GetSongName(_songs[i]);
                SongNames.Insert(0, songName);
            }
            catch
            {
                _songs.Delete(i);
            }
        }

        if (Status.Contains(StatusElement.Playing))
        {
            if (_songs.Count != 0)
            {
                if (CurrentIndex != -1)
                {
                    int count = _songs.Count;
                    if (CurrentIndex >= count || _songs[CurrentIndex] != CurrentSong)
                    {
                        CurrentIndex = _songs.IndexOf(CurrentSong);
                        if (CurrentIndex == -1)
                        {
                            CurrentSong = "";
                        }
                    }
                }

                if (CurrentIndex == -1 && !Status.Contains(StatusElement.Paused))
                {
                    _ = Play(0);
                }
            }
            else
            {
                _ = (AudioHost.SoundSystem?.StopSong());

                CurrentIndex = -1;
                CurrentSong = "";
            }
        }
    }

    // Sound.pas:854
    public SongPlayer()
    {
        _songs = new DelphiStringList
        {
            OnChange = SongsChanged
        };
        SongNames = new DelphiStringList();
        CurrentIndex = -1;
    }

    // Sound.pas:863
    public virtual void Dispose()
    {
        _songs?.Dispose();
        SongNames?.Dispose();
    }

    // Sound.pas:909
    // { Called every tick (ProcessSound): starts the next song when the last one has ended. }
    public void Update()
    {
        if (AudioHost.SoundSystem is not null && Status.Contains(StatusElement.Playing) && !Status.Contains(StatusElement.Paused) && !AudioHost.SoundSystem.PlayingSong && AudioHost.SoundSystem.MusicEnabled)
        {
            CurrentIndex += 1;
            if (_songs.Count <= CurrentIndex)
            {
                CurrentIndex = 0;
            }

            int count = _songs.Count;
            if (CurrentIndex < count)
            {
                _ = Play(CurrentIndex);
            }
            else
            {
                CurrentIndex = -1;
            }
        }
    }

    // Sound.pas:924
    public bool Play(int index, int fadeOut, int fadeIn)
    {
        bool result;
        if (AudioHost.SoundSystem is not null && index >= 0 && index < _songs.Count)
        {
            Status += Set<StatusElement>.Of(StatusElement.Playing);
            string item = _songs[index];
            result = AudioHost.SoundSystem.PlaySong(item, fadeOut, fadeIn, false);
            if (result)
            {
                CurrentIndex = index;
                CurrentSong = _songs[index];
                OnSongChange?.Invoke(this);
            }
            else
            {
                CurrentSong = "";
                CurrentIndex = -1;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Sound.pas:947
    public bool Play(int index)
    {
        bool result;
        if (AudioHost.SoundSystem is not null && index >= 0 && index < _songs.Count)
        {
            Status += Set<StatusElement>.Of(StatusElement.Playing);
            string item = _songs[index];
            result = AudioHost.SoundSystem.PlaySong(item, 2, 0, false);
            if (result)
            {
                CurrentIndex = index;
                CurrentSong = _songs[index];
                OnSongChange?.Invoke(this);
            }
            else
            {
                CurrentSong = "";
                CurrentIndex = -1;
            }
        }
        else
        {
            return false;
        }

        return result;
    }

    // Sound.pas:970
    public bool Stop(bool stopMusic)
    {
        bool result;
        if (AudioHost.SoundSystem is not null)
        {
            result = stopMusic && AudioHost.SoundSystem.StopSong();

            Status -= Set<StatusElement>.Of(StatusElement.Playing);
        }
        else
        {
            return false;
        }

        return result;
    }

    // Sound.pas:985
    // { Pauses count; the first one saves the song position. }
    public bool Pause(bool stopMusic)
    {
        if (_pauseCount == 0)
        {
            if (_backup is not null)
            {
            }

            _backup = AudioPlayback.BackupSongStatus();
            Status += Set<StatusElement>.Of(StatusElement.Paused);
            if (stopMusic)
            {
                _ = AudioHost.SoundSystem.StopSong();
            }
        }

        _pauseCount += 1;
        return true;
    }

    // Sound.pas:1000
    public bool Resume()
    {
        _pauseCount -= 1;
        if (_pauseCount == 0)
        {
            if (_backup is not null)
            {
                CurrentIndex = _songs.IndexOf(_backup.FileName);
                _ = CurrentIndex == -1 ? Play(0) : SongStatus.RestoreSongStatus(_backup);
                _backup = null;
            }

            Status -= Set<StatusElement>.Of(StatusElement.Paused);
        }

        return true;
    }

    public string CurrentSong { get; private set; } = "";
    public Set<StatusElement> Status { get; private set; }
    public DelphiStringList Songs { get => _songs; set => SetSongs(value); }
    public DelphiStringList SongNames { get; }
    public int CurrentIndex { get; private set; }

    public enum StatusElement : byte
    {
        Playing = 0, Paused = 1
    }

}
