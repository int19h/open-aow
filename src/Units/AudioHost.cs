using System.Runtime.InteropServices;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public static class AudioHost
{
    internal static string BaseSoundEnginePath = "";

    public static SongPlayer SongPlayer;

    public static SoundSystem SoundSystem = default;

    internal static SoundSystemList SoundSystems = default;

    internal static SampleDataList SampleDataList = default;

    internal static SampleList SampleList = default;

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
