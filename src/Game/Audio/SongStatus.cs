// Translated from Sound.pas.

namespace Aow.Game.Audio;

// FFileName. What BackupSongStatus saves. the sound system adds its own position
public class SongStatus
{
    public string FileName { get; protected internal init; } = "";

    internal static bool RestoreSongStatus(SongStatus status) => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.RestoreSongStatus(status);

}
