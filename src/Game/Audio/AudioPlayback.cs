
using Aow.Engine.Buffers;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public static class AudioPlayback
{
    public static void LockSampleData(string name)
    {
        SampleData D;
        if (AudioHost.SoundSystem != null)
        {
            D = AudioHost.SampleDataList.Find(name);
            if (D == null)
            {
                AudioHost.SoundSystem.SampleDataClass().CreateInstance(name);
            }
            else
            {
                D.AddRef();
            }
        }
    }

    public static void UnlockSampleData(string name)
    {
        SampleData D;
        D = AudioHost.SampleDataList.Find(name);
        if (D != null)
        {
            D.Release();
        }
    }

    public static int SamplesActive() => AudioHost.SampleList.Count;

    public static int SamplesLoaded() => AudioHost.SampleDataList.Count;

    public static bool InitializeSoundSystem(string name, in SoundSettings settings)
    {
        SoundSystemClass C;
        bool result = true;
        if (IsEmpty(name))
        {
            if (AudioHost.SoundSystems.Count != 0)
            {
                C = (SoundSystemClass)AudioHost.SoundSystems[0];
            }
            else
            {
                C = null;
            }
        }
        else
        {
            C = AudioHost.SoundSystems.Find(name);
        }

        if (C != null)
        {
            if (AudioHost.SoundSystem != null)
            {
                ReleaseSoundSystem();
            }

            AudioHost.SoundSystem = C.CreateInstance();
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
            if (AudioHost.SongPlayer != null)
            {
                AudioHost.SongPlayer.Stop(true);
            }

            if (AudioHost.SampleList != null)
            {
                AudioHost.SampleList.Clear();
            }

            AudioHost.SoundSystem.Release();
            AudioHost.SoundSystem = null;
        }
    }

    internal static void FadeMusicVolume(int volume, int speed)
    {
        if (AudioHost.SoundSystem != null)
        {
            AudioHost.SoundSystem.FadeMusicVolume(volume, speed);
        }
    }

    internal static void SetSfxVolume(int volume)
    {
        if (AudioHost.SoundSystem != null)
        {
            AudioHost.SoundSystem.SetSfxVolume(volume);
        }
    }

    internal static int GetSfxVolume()
    {
        int result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.GetSfxVolume();
        }
        else
        {
            result = -1;
        }

        return result;
    }

    internal static bool GetSfxEnabled()
    {
        bool result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.GetSfxEnabled();
        }
        else
        {
            result = false;
        }

        return result;
    }

    internal static SoundSettings GetSoundSettings()
    {
        SoundSettings result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.GetSoundSettings();
        }
        else
        {
            result = default;
        }

        return result;
    }

    internal static SongStatus BackupSongStatus()
    {
        SongStatus result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.BackupSongStatus();
        }
        else
        {
            result = null;
        }

        return result;
    }

    internal static string GetSongName(string fileName)
    {
        string result = "";
        Stream stream;
        string ext;
        ByteArray28 title;
        int P;
        stream = OpenFile(AudioHost.BaseSoundEnginePath + fileName, 0 | 32);
        try
        {
            ext = UpperCase(ExtractFileExt(fileName));
            title = default;
            if (ext == ".IT")
            {
                stream.Seek(4, SeekOrigin.Begin);
                stream.ReadExactly(((Span<byte>)title).Slice(0, 28));
                result = StrPasBuffer(title);
            }
            else
            {
                if (ext == ".S3M")
                {
                    stream.ReadExactly(((Span<byte>)title).Slice(0, 28));
                    result = StrPasBuffer(title);
                }
                else
                {
                    result = ExtractFileName(fileName);
                    P = Pos(".", result);
                    if (P != 0)
                    {
                        result = Copy(result, 1, P - 1);
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
            for (int I = AudioHost.SampleList.Count - 1; I >= 0; I--)
            {
                (AudioHost.SampleList.List[I]).Update();
            }
        }

        if (AudioHost.SongPlayer != null)
        {
            AudioHost.SongPlayer.Update();
        }

        if (AudioHost.SoundSystem != null)
        {
            AudioHost.SoundSystem.Process();
        }
    }

    internal static void SetMusicVolume(int volume)
    {
        if (AudioHost.SoundSystem != null)
        {
            AudioHost.SoundSystem.SetMusicVolume(volume);
        }
    }

    internal static int GetMusicVolume()
    {
        int result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.GetMusicVolume();
        }
        else
        {
            result = -1;
        }

        return result;
    }

    internal static bool GetMusicEnabled()
    {
        bool result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.GetMusicEnabled();
        }
        else
        {
            result = false;
        }

        return result;
    }

    internal static Sample PlaySample(string name)
    {
        Sample result;
        SampleData D;
        if (AudioHost.SoundSystem != null && AudioHost.SoundSystem.SfxEnabled)
        {
            try
            {
                D = AudioHost.SampleDataList.Find(name);
                if (D == null)
                {
                    D = AudioHost.SoundSystem.SampleDataClass().CreateInstance(name);
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(D);
                    D.Release();
                }
                else
                {
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(D);
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
        SampleData D;
        if (AudioHost.SoundSystem != null && AudioHost.SoundSystem.SfxEnabled)
        {
            try
            {
                D = AudioHost.SampleDataList.Find(name);
                if (D == null)
                {
                    D = AudioHost.SoundSystem.SampleDataClass().CreateInstance(name);
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(D);
                    D.Release();
                }
                else
                {
                    result = AudioHost.SoundSystem.SampleClass().CreateInstance(D);
                }

                result.Looped = looped;
                result._delay = delay;
                result.Panning = panning;
                result._callback = callback;
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
        bool result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.PlaySong(fileName);
        }
        else
        {
            result = false;
        }

        return result;
    }

    internal static bool PlaySong(string fileName, int fadeOut, int fadeIn, bool loop)
    {
        bool result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.PlaySong(fileName, fadeOut, fadeIn, loop);
        }
        else
        {
            result = false;
        }

        return result;
    }

    internal static SampleData CreateSampleData(string name)
    {
        SampleData result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.SampleDataClass().CreateInstance(name);
        }
        else
        {
            result = null;
        }

        return result;
    }

    public static SampleData CreateSampleData(ReadOnlySpan<WaveFormatEx> format, object data, int size)
    {
        SampleData result;
        if (AudioHost.SoundSystem != null)
        {
            result = AudioHost.SoundSystem.SampleDataClass().CreateInstance(format[0], data, size);
        }
        else
        {
            result = null;
        }

        return result;
    }

}
