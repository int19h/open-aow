// Translated from Sound.pas.

namespace Aow.Game.Audio;

// FFileName. What BackupSongStatus saves. the sound system adds its own position
public partial class SongStatus
{
    public string FileName { get; protected internal set; } = "";

    internal static bool RestoreSongStatus(SongStatus status)
    {
        bool result = AudioHost.SoundSystem != null && AudioHost.SoundSystem.RestoreSongStatus(status);
        return result;
    }

}
