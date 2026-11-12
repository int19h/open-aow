// Translated from Sound.pas.

namespace Aow.Game.Audio;

// FFileName. What BackupSongStatus saves. the sound system adds its own position
public partial class SongStatus
{
    protected internal string _fileName = "";

    public string FileName { get => _fileName; }

    internal static bool RestoreSongStatus(SongStatus status)
    {
        bool result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.RestoreSongStatus(status);
        }
        else
        {
            result = false;
        }

        return result;
    }

}
