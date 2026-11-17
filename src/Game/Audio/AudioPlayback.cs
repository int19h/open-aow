#nullable disable

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public static class AudioPlayback
{
    public static void LockSampleData(string name)
    {
        SampleData d;
        if (AudioHost.SoundSystem != null)
        {
            d = AudioHost.SampleDataList.Find(name);
            if (d == null)
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
        SampleData d;
        d = AudioHost.SampleDataList.Find(name);
        d?.Release();
    }

    public static int SamplesActive() => AudioHost.SampleList.Count;

    public static int SamplesLoaded() => AudioHost.SampleDataList.Count;

    public static bool InitializeSoundSystem(string name, in SoundSettings settings)
    {
        SoundSystemClass c;
        bool result = true;
        c = IsEmpty(name) ? AudioHost.SoundSystems.Count != 0 ? AudioHost.SoundSystems[0] : null : AudioHost.SoundSystems.Find(name);

        if (c != null)
        {
            if (AudioHost.SoundSystem != null)
            {
                ReleaseSoundSystem();
            }

            AudioHost.SoundSystem = c.CreateInstance();
            if (!AudioHost.SoundSystem.Activate(settings))
            {
                AudioHost.SoundSystem?.Dispose();
                AudioHost.SoundSystem = null;
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }

    public static void ReleaseSoundSystem()
    {
        if (AudioHost.SoundSystem != null)
        {
            _ = (AudioHost.SongPlayer?.Stop(true));

            AudioHost.SampleList?.Clear();

            _ = AudioHost.SoundSystem.Release();
            AudioHost.SoundSystem = null;
        }
    }

    internal static void FadeMusicVolume(int volume, int speed) => AudioHost.SoundSystem?.FadeMusicVolume(volume, speed);

    internal static void SetSfxVolume(int volume) => AudioHost.SoundSystem?.SetSfxVolume(volume);

    internal static int GetSfxVolume()
    {
        int result = AudioHost.SoundSystem != null ? AudioHost.SoundSystem.GetSfxVolume() : -1;
        return result;
    }

    internal static bool GetSfxEnabled()
    {
        bool result = AudioHost.SoundSystem != null && AudioHost.SoundSystem.GetSfxEnabled();
        return result;
    }

    internal static SoundSettings GetSoundSettings()
    {
        SoundSettings result = AudioHost.SoundSystem != null ? AudioHost.SoundSystem.GetSoundSettings() : default;
        return result;
    }

    internal static SongStatus BackupSongStatus()
    {
        SongStatus result = AudioHost.SoundSystem?.BackupSongStatus();
        return result;
    }

    internal static string GetSongName(string fileName)
    {
        string result = "";
        Stream stream;
        string ext;
        Span<byte> title = stackalloc byte[28];
        int p;
        stream = OpenFile(AudioHost.BaseSoundEnginePath + fileName, 0 | 32);
        try
        {
            ext = UpperCase(ExtractFileExt(fileName));
            title.Clear();
            if (ext == ".IT")
            {
                _ = stream.Seek(4, SeekOrigin.Begin);
                stream.ReadExactly(title);
                result = StrPasBuffer(title);
            }
            else
            {
                if (ext == ".S3M")
                {
                    stream.ReadExactly(title);
                    result = StrPasBuffer(title);
                }
                else
                {
                    result = ExtractFileName(fileName);
                    p = Pos(".", result);
                    if (p != 0)
                    {
                        result = Copy(result, 1, p - 1);
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
        if (AudioHost.SampleList != null)
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

    internal static int GetMusicVolume()
    {
        int result = AudioHost.SoundSystem != null ? AudioHost.SoundSystem.GetMusicVolume() : -1;
        return result;
    }

    internal static bool GetMusicEnabled()
    {
        bool result = AudioHost.SoundSystem != null && AudioHost.SoundSystem.GetMusicEnabled();
        return result;
    }

    internal static Sample PlaySample(string name)
    {
        Sample result;
        SampleData d;
        if (AudioHost.SoundSystem != null && AudioHost.SoundSystem.SfxEnabled)
        {
            try
            {
                d = AudioHost.SampleDataList.Find(name);
                if (d == null)
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
                result = null;
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    public static Sample PlaySample(string name, int volume, bool looped, int delay, SamplePanning panning, SampleCallBack callback)
    {
        Sample result;
        SampleData d;
        if (AudioHost.SoundSystem != null && AudioHost.SoundSystem.SfxEnabled)
        {
            try
            {
                d = AudioHost.SampleDataList.Find(name);
                if (d == null)
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
                result = null;
            }
        }
        else
        {
            result = null;
        }

        return result;
    }

    internal static bool PlaySong(string fileName)
    {
        bool result = AudioHost.SoundSystem != null && AudioHost.SoundSystem.PlaySong(fileName);
        return result;
    }

    internal static bool PlaySong(string fileName, int fadeOut, int fadeIn, bool loop)
    {
        bool result = AudioHost.SoundSystem != null && AudioHost.SoundSystem.PlaySong(fileName, fadeOut, fadeIn, loop);
        return result;
    }

    internal static SampleData CreateSampleData(string name)
    {
        SampleData result = AudioHost.SoundSystem?.SampleDataClass().CreateInstance(name);
        return result;
    }

    public static SampleData CreateSampleData(ReadOnlySpan<WaveFormat> format, object data, int size)
    {
        SampleData result = AudioHost.SoundSystem?.SampleDataClass().CreateInstance(format[0], data, size);
        return result;
    }

}
