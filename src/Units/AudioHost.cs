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

    public static global::Aow.Game.Audio.SongPlayer SongPlayer;

    public static global::Aow.Game.Audio.SoundSystem SoundSystem = default;

    internal static SoundSystemList SoundSystems = default;

    internal static global::Aow.Game.Audio.SampleDataList SampleDataList = default;

    internal static global::Aow.Game.Audio.SampleList SampleList = default;

    internal static void Initialize()
    {
        if (ToolServices != null)
        {
            BaseSoundEnginePath = ExtractFilePath(ToolServices.GetProjectName());
        }
        else
        {
            BaseSoundEnginePath = ExtractFilePath(global::Aow.Rtl.RtlUnit.Application.ExeName);
        }

        SampleList = new global::Aow.Game.Audio.SampleList();
        SampleDataList = new global::Aow.Game.Audio.SampleDataList();
        if (SoundSystems == null)
        {
            SoundSystems = new SoundSystemList();
        }

        SongPlayer = new global::Aow.Game.Audio.SongPlayer();
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
