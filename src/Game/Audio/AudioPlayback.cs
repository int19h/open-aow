#nullable disable

using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStreams;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.PCharStrings;

namespace Aow.Game.Audio;

public static class AudioPlayback
{
    public static void LockSampleData(string name)
    {
        if (AudioHost.SoundSystem is not null)
        {
            SampleData d = AudioHost.SampleDataList.Find(name);
            if (d is null)
            {
                _ = AudioHost.SoundSystem.SampleDataClass().CreateInstance(name);
            }
            else
            {
                d.AddRef();
            }
        }
    }

    public static void UnlockSampleData(string name)
    {
        SampleData d = AudioHost.SampleDataList.Find(name);
        d?.Release();
    }

    public static int SamplesActive() => AudioHost.SampleList.Count;

    public static int SamplesLoaded() => AudioHost.SampleDataList.Count;

    public static bool InitializeSoundSystem(string name, in SoundSettings settings)
    {
        SoundSystemClass c = IsEmpty(name) ? AudioHost.SoundSystems.Count != 0 ? AudioHost.SoundSystems[0] : null : AudioHost.SoundSystems.Find(name);

        if (c is not null)
        {
            if (AudioHost.SoundSystem is not null)
            {
                ReleaseSoundSystem();
            }

            AudioHost.SoundSystem = c.CreateInstance();
            if (!AudioHost.SoundSystem.Activate(settings))
            {
                AudioHost.SoundSystem?.Dispose();
                AudioHost.SoundSystem = null;
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public static void ReleaseSoundSystem()
    {
        if (AudioHost.SoundSystem is not null)
        {
            _ = (AudioHost.SongPlayer?.Stop(true));

            AudioHost.SampleList?.Clear();

            _ = AudioHost.SoundSystem.Release();
            AudioHost.SoundSystem = null;
        }
    }

    internal static void FadeMusicVolume(int volume, int speed) => AudioHost.SoundSystem?.FadeMusicVolume(volume, speed);

    internal static void SetSfxVolume(int volume) => AudioHost.SoundSystem?.SetSfxVolume(volume);

    internal static bool SfxEnabled => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.SfxEnabled;

    internal static SoundSettings SoundSettings => AudioHost.SoundSystem is not null ? AudioHost.SoundSystem.SoundSettings : default;

    internal static SongStatus BackupSongStatus() => AudioHost.SoundSystem?.BackupSongStatus();

    internal static string GetSongName(string fileName)
    {
        Span<byte> title = stackalloc byte[28];
        Stream stream = OpenFile(AudioHost.BaseSoundEnginePath + fileName, mode: FileOpenMode.Read | FileOpenMode.ShareDenyWrite);
        string result;
        try
        {
            string ext = UpperCase(ExtractFileExt(fileName));
            title.Clear();
            if (ext == ".IT")
            {
                _ = stream.Seek(4, origin: SeekOrigin.Begin);
                stream.ReadExactly(title);
                return StrPasBuffer(title);
            }
            else
            {
                if (ext == ".S3M")
                {
                    stream.ReadExactly(title);
                    return StrPasBuffer(title);
                }
                else
                {
                    result = ExtractFileName(fileName);
                    int p = Pos(".", haystack: result);
                    if (p != 0)
                    {
                        return Copy(result, index: 1, count: p - 1);
                    }
                }
            }
        }
        finally
        {
            stream?.Dispose();
        }

        return result;
    }

    public static void ProcessSound()
    {
        if (AudioHost.SampleList is not null)
        {
            for (int i = AudioHost.SampleList.Count - 1; i >= 0; i--)
            {
                AudioHost.SampleList.List[i].Update();
            }
        }

        AudioHost.SongPlayer?.Update();

        AudioHost.SoundSystem?.Process();
    }

    internal static void SetMusicVolume(int volume) => AudioHost.SoundSystem?.SetMusicVolume(volume);

    internal static int MusicVolume => AudioHost.SoundSystem is not null ? AudioHost.SoundSystem.MusicVolume : -1;

    internal static bool MusicEnabled => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.MusicEnabled;

    internal static Sample PlaySample(string name)
    {
        Sample result;
        if (AudioHost.SoundSystem is not null && AudioHost.SoundSystem.SfxEnabled)
        {
            try
            {
                SampleData d = AudioHost.SampleDataList.Find(name);
                if (d is null)
                {
                    d = AudioHost.SoundSystem.SampleDataClass().CreateInstance(name);
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(d);
                    d.Release();
                }
                else
                {
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(d);
                }

                result.Execute();
            }
            catch
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        return result;
    }

    public static Sample PlaySample(string name, int volume, bool looped, int delay, SamplePanning panning, Action<Sample, SampleCallbackMode> callback)
    {
        Sample result;
        if (AudioHost.SoundSystem is not null && AudioHost.SoundSystem.SfxEnabled)
        {
            try
            {
                SampleData d = AudioHost.SampleDataList.Find(name);
                if (d is null)
                {
                    d = AudioHost.SoundSystem.SampleDataClass().CreateInstance(name);
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(d);
                    d.Release();
                }
                else
                {
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(d);
                }

                result.Looped = looped;
                result.Delay = delay;
                result.Panning = panning;
                result.SetCallbackHandler(callback);
                result.Volume = volume;
                result.Execute();
            }
            catch
            {
                return null;
            }
        }
        else
        {
            return null;
        }

        return result;
    }

    internal static bool PlaySong(string fileName, int fadeOut, int fadeIn, bool loop) => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.PlaySong(fileName, fadeOut, fadeIn, loop);

    public static SampleData CreateSampleData(ReadOnlySpan<WaveFormat> format, object data, int size) => AudioHost.SoundSystem?.SampleDataClass().CreateInstance(format[0], data, size);

}
