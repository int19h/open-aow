// Translated from Sound.pas.
using Aow.Rtl;

namespace Aow.Game.Audio;

// _currentSong, _status, _songs, _songNames, _currentIndex, _onSongChange, _backup (Pause),
// FPauseCount. Plays the songs in turn (Update starts the next one)
public partial class SongPlayer : IDisposable
{
    internal string _currentSong = "";
    internal Set<StatusElement> _status;
    internal StringList _songs;
    internal StringList _songNames;
    internal int _currentIndex;
    internal NotifyEvent<SongPlayer> _onSongChange;
    internal SongStatus _backup;
    internal int _pauseCount;

    // Sound.pas:870
    internal void SetSongs(StringList value)
    {
        _songs.Assign(value);
    }

    // Sound.pas:875
    internal void SongsChanged(object sender)
    {
        _songNames.Clear();
        for (int I = _songs.Count - 1; I >= 0; I--)
        {
            try
            {
                var songName = AudioPlayback.GetSongName(_songs[I]);
                _songNames.Insert(0, songName);
            }
            catch
            {
                _songs.Delete(I);
            }
        }

        if (_status.Contains(StatusElement.Playing))
        {
            if (_songs.Count != 0)
            {
                if (_currentIndex != -1)
                {
                    var count = _songs.Count;
                    if (_currentIndex >= count || _songs[_currentIndex] != _currentSong)
                    {
                        _currentIndex = _songs.IndexOf(_currentSong);
                        if (_currentIndex == -1)
                        {
                            _currentSong = "";
                        }
                    }
                }

                if (_currentIndex == -1 && !_status.Contains(StatusElement.Paused))
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

                _currentIndex = -1;
                _currentSong = "";
            }
        }
    }

    // Sound.pas:854
    public SongPlayer()
    {
        _songs = new StringList();
        _songs.OnChange = SongsChanged;
        _songNames = new StringList();
        _currentIndex = -1;
    }

    // Sound.pas:863
    public virtual void Dispose()
    {
        _songs?.Dispose();
        _songNames?.Dispose();
    }

    // Sound.pas:909
    // { Called every tick (ProcessSound): starts the next song when the last one has ended. }
    public void Update()
    {
        if (AudioHost.SoundSystem != null && _status.Contains(StatusElement.Playing) && !_status.Contains(StatusElement.Paused) && !AudioHost.SoundSystem.PlayingSong && AudioHost.SoundSystem.MusicEnabled)
        {
            _currentIndex += 1;
            if (_songs.Count <= _currentIndex)
            {
                _currentIndex = 0;
            }

            var count = _songs.Count;
            if (_currentIndex < count)
            {
                Play(_currentIndex);
            }
            else
            {
                _currentIndex = -1;
            }
        }
    }

    // Sound.pas:924
    public bool Play(int index, int fadeOut, int fadeIn)
    {
        bool result;
        if (AudioHost.SoundSystem != null && index >= 0 && index < _songs.Count)
        {
            _status = _status + Set<StatusElement>.Of(StatusElement.Playing);
            var item = _songs[index];
            result = AudioHost.SoundSystem.PlaySong(item, fadeOut, fadeIn, false);
            if (result)
            {
                _currentIndex = index;
                _currentSong = _songs[index];
                if (_onSongChange != null)
                {
                    _onSongChange(this);
                }
            }
            else
            {
                _currentSong = "";
                _currentIndex = -1;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Sound.pas:947
    public bool Play(int index)
    {
        bool result;
        if (AudioHost.SoundSystem != null && index >= 0 && index < _songs.Count)
        {
            _status = _status + Set<StatusElement>.Of(StatusElement.Playing);
            var item = _songs[index];
            result = AudioHost.SoundSystem.PlaySong(item, 2, 0, false);
            if (result)
            {
                _currentIndex = index;
                _currentSong = _songs[index];
                if (_onSongChange != null)
                {
                    _onSongChange(this);
                }
            }
            else
            {
                _currentSong = "";
                _currentIndex = -1;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Sound.pas:970
    public bool Stop(bool stopMusic)
    {
        bool result;
        if (AudioHost.SoundSystem != null)
        {
            if (stopMusic)
            {
                result = AudioHost.SoundSystem.StopSong();
            }
            else
            {
                result = false;
            }

            _status = _status - Set<StatusElement>.Of(StatusElement.Playing);
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Sound.pas:985
    // { Pauses count; the first one saves the song position. }
    public bool Pause(bool stopMusic)
    {
        if (_pauseCount == 0)
        {
            if (_backup != null)
            {
            }

            _backup = AudioPlayback.BackupSongStatus();
            _status = _status + Set<StatusElement>.Of(StatusElement.Paused);
            if (stopMusic)
            {
                AudioHost.SoundSystem.StopSong();
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
            if (_backup != null)
            {
                _currentIndex = _songs.IndexOf(_backup._fileName);
                if (_currentIndex == -1)
                {
                    Play(0);
                }
                else
                {
                    SongStatus.RestoreSongStatus(_backup);
                }
                _backup = null;
            }

            _status = _status - Set<StatusElement>.Of(StatusElement.Paused);
        }

        return true;
    }

    public string CurrentSong { get => _currentSong; }
    public Set<StatusElement> Status { get => _status; }
    public StringList Songs { get => _songs; set => SetSongs(value); }
    public StringList SongNames { get => _songNames; }
    public int CurrentIndex { get => _currentIndex; }
    public NotifyEvent<SongPlayer> OnSongChange { get => _onSongChange; set => _onSongChange = value; }

    public enum StatusElement : byte
    {
        Playing = 0, Paused = 1
    }

}
