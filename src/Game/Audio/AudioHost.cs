
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public static class AudioHost
{
    internal static string BaseSoundEnginePath { get; set; } = "";

    public static SongPlayer SongPlayer { get; set; }

    public static SoundSystem SoundSystem { get; set; } = default;

    internal static SoundSystemList SoundSystems { get; set; } = default;

    internal static SampleDataList SampleDataList { get; set; } = default;

    internal static SampleList SampleList { get; set; } = default;

    internal static void Initialize()
    {
        if (ToolServices != null)
        {
            BaseSoundEnginePath = ExtractFilePath(ToolServices.GetProjectName());
        }
        else
        {
            BaseSoundEnginePath = ExtractFilePath(RtlUnit.Application.ExeName);
        }

        SampleList = new SampleList();
        SampleDataList = new SampleDataList();
        if (SoundSystems == null)
        {
            SoundSystems = new SoundSystemList();
        }

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
        if (SoundSystem != null)
        {
            SoundSystem.Release();
        }

        SoundSystems?.Dispose();
    }

}
