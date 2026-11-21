#nullable disable warnings

using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.LegacyApplication;

namespace Aow.Game.Audio;

public static class AudioHost
{
    internal static string BaseSoundEnginePath { get; set; } = "";

    public static SongPlayer? SongPlayer { get; set; }

    public static SoundSystem? SoundSystem { get; set; } = default;

    internal static SoundSystemList? SoundSystems { get; set; } = default;

    internal static SampleDataList? SampleDataList { get; set; } = default;

    internal static SampleList? SampleList { get; set; } = default;

    internal static void Initialize()
    {
        BaseSoundEnginePath = ExtractFilePath(Application.ExeName);

        SampleList = new SampleList();
        SampleDataList = new SampleDataList();
        SoundSystems ??= new SoundSystemList();

        SongPlayer = new SongPlayer();
    }

    internal static void Finalize()
    {
        SongPlayer?.Dispose();
        SongPlayer = null;
        SampleList?.Dispose();
        SampleList = null;
        if (SampleDataList.Count != 0)
        {
            ShowMessage("Inproper use of Sound Engine (SoundData still loaded)!");
        }

        SampleDataList?.Dispose();
        SampleDataList = null;
        _ = (SoundSystem?.Release());

        SoundSystems?.Dispose();
    }
}
