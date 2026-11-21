#nullable disable warnings
// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// _currentSong, _status, _songs, _songNames, _currentIndex, OnSongChange, _backup (Pause),
// FPauseCount. Plays the songs in turn (Update starts the next one)
public sealed class SongPlayer : IDisposable
{
    private NotifyEvent<SongPlayer> _songChange;

    internal void SetSongChangeHandler(NotifyEvent<SongPlayer> handler) => _songChange = handler;

    private SongStatus? _backup;
    private int _pauseCount;

    // Sound.pas:870
    internal void SetSongs(DelphiStringList value) => Songs.Assign(value);

    // Sound.pas:875
    internal void SongsChanged(object sender)
    {
        SongNames.Clear();
        for (int i = Songs.Count - 1; i >= 0; i--)
        {
            try
            {
                string songName = AudioPlayback.GetSongName(Songs[i]);
                SongNames.Insert(0, value: songName);
            }
            catch
            {
                Songs.Delete(i);
            }
        }

        if (Status.Contains(Statuses.Playing))
        {
            if (Songs.Count != 0)
            {
                if (CurrentIndex != -1)
                {
                    int count = Songs.Count;
                    if (CurrentIndex >= count || Songs[CurrentIndex] != CurrentSong)
                    {
                        CurrentIndex = Songs.IndexOf(CurrentSong);
                        if (CurrentIndex == -1)
                        {
                            CurrentSong = "";
                        }
                    }
                }

                if (CurrentIndex == -1 && !Status.Contains(Statuses.Paused))
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
        Songs = new DelphiStringList
        {
            OnChange = SongsChanged
        };
        SongNames = [];
        CurrentIndex = -1;
    }

    // Sound.pas:863
    public void Dispose()
    {
        Songs?.Dispose();
        SongNames?.Dispose();
    }

    // Sound.pas:909
    // { Called every tick (ProcessSound): starts the next song when the last one has ended. }
    public void Update()
    {
        if (AudioHost.SoundSystem is not null && Status.Contains(Statuses.Playing) && !Status.Contains(Statuses.Paused) && !AudioHost.SoundSystem.PlayingSong && AudioHost.SoundSystem.MusicEnabled)
        {
            CurrentIndex += 1;
            if (Songs.Count <= CurrentIndex)
            {
                CurrentIndex = 0;
            }

            int count = Songs.Count;
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
        if (AudioHost.SoundSystem is not null && index >= 0 && index < Songs.Count)
        {
            Status |= Statuses.Playing;
            string item = Songs[index];
            result = AudioHost.SoundSystem.PlaySong(item, fadeOut, fadeIn, loop: false);
            if (result)
            {
                CurrentIndex = index;
                CurrentSong = Songs[index];
                _songChange?.Invoke(this);
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
        if (AudioHost.SoundSystem is not null && index >= 0 && index < Songs.Count)
        {
            Status |= Statuses.Playing;
            string item = Songs[index];
            result = AudioHost.SoundSystem.PlaySong(item, fadeOut: 2, fadeIn: 0, loop: false);
            if (result)
            {
                CurrentIndex = index;
                CurrentSong = Songs[index];
                _songChange?.Invoke(this);
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

            Status &= ~Statuses.Playing;
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
            Status |= Statuses.Paused;
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
                CurrentIndex = Songs.IndexOf(_backup.FileName);
                _ = CurrentIndex == -1 ? Play(0) : SongStatus.RestoreSongStatus(_backup);
                _backup = null;
            }

            Status &= ~Statuses.Paused;
        }

        return true;
    }

    public string CurrentSong { get; private set; } = "";
    public Statuses Status { get; private set; }
    public DelphiStringList Songs { get; }
    public DelphiStringList SongNames { get; }
    public int CurrentIndex { get; private set; }

    [Flags]
    public enum Statuses : byte
    {
        None = 0,
        Playing = 1 << 0,
        Paused = 1 << 1,
    }
}
