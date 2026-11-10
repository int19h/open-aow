// Translated from Sound.pas.

namespace Aow.Game.Audio;

// FFileName. What BackupSongStatus saves. the sound system adds its own position
public partial class SongStatus
{
    protected internal string FFileName = "";

    public string FileName { get => FFileName; }

    internal static bool RestoreSongStatus(SongStatus Status)
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.RestoreSongStatus(Status);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

}
