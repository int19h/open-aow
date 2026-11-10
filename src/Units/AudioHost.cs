using System.Runtime.InteropServices;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public static class AudioHost
{
    internal static string BaseSoundEnginePath = "";

    public static TSongPlayer SongPlayer;

    public static TSoundSystem SoundSystem = default;

    internal static TSoundSystemList SoundSystems = default;

    internal static TSampleDataList SampleDataList = default;

    internal static TSampleList SampleList = default;

    internal static void Initialize()
    {
        if (ToolServices != null)
        {
            BaseSoundEnginePath = ExtractFilePath(ToolServices.GetProjectName());
        }
        else
        {
            BaseSoundEnginePath = ExtractFilePath(Application.ExeName);
        }

        SampleList = new TSampleList();
        SampleDataList = new TSampleDataList();
        if (SoundSystems == null)
        {
            SoundSystems = new TSoundSystemList();
        }

        SongPlayer = new TSongPlayer();
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
