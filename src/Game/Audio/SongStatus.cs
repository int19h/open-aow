// Translated from Sound.pas.

namespace Aow.Game.Audio;

// FFileName. What BackupSongStatus saves. the sound system adds its own position
public partial class SongStatus
{
    private string _fileName = "";

    public string FileName { get => _fileName; protected internal set => _fileName = value; }

    internal static bool RestoreSongStatus(SongStatus status)
    {
        bool result = AudioHost.SoundSystem != null && AudioHost.SoundSystem.RestoreSongStatus(status);
        return result;
    }

}
