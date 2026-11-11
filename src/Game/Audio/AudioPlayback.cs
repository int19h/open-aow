
using Aow.Engine.Buffers;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public static class AudioPlayback
{
    public static void LockSampleData(string Name)
    {
        SampleData D;
        if (AudioHost.SoundSystem != null)
        {
            D = AudioHost.SampleDataList.Find(Name);
            if (D == null)
            {
                AudioHost.SoundSystem.SampleDataClass().Create(Name);
            }
            else
            {
                D.AddRef();
            }
        }
    }

    public static void UnlockSampleData(string Name)
    {
        SampleData D;
        D = AudioHost.SampleDataList.Find(Name);
        if (D != null)
        {
            D.Release();
        }
    }

    public static int SamplesActive() => AudioHost.SampleList.Count;

    public static int SamplesLoaded() => AudioHost.SampleDataList.Count;

    public static bool InitializeSoundSystem(string Name, in SoundSettings Settings)
    {
        SoundSystemClass C;
        bool Result = true;
        if (IsEmpty(Name))
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
            C = AudioHost.SoundSystems.Find(Name);
        }

        if (C != null)
        {
            if (AudioHost.SoundSystem != null)
            {
                ReleaseSoundSystem();
            }

            AudioHost.SoundSystem = C.Create();
            if (!AudioHost.SoundSystem.Activate(Settings))
            {
                AudioHost.SoundSystem?.Dispose();
                AudioHost.SoundSystem = null;
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
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

    internal static void FadeMusicVolume(int Volume, int Speed)
    {
        if (AudioHost.SoundSystem != null)
        {
            AudioHost.SoundSystem.FadeMusicVolume(Volume, Speed);
        }
    }

    internal static void SetSFXVolume(int Volume)
    {
        if (AudioHost.SoundSystem != null)
        {
            AudioHost.SoundSystem.SetSFXVolume(Volume);
        }
    }

    internal static int GetSFXVolume()
    {
        int Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.GetSFXVolume();
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    internal static bool GetSFXEnabled()
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.GetSFXEnabled();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    internal static SoundSettings GetSoundSettings()
    {
        SoundSettings Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.GetSoundSettings();
        }
        else
        {
            Result = default;
        }

        return Result;
    }

    internal static SongStatus BackupSongStatus()
    {
        SongStatus Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.BackupSongStatus();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    internal static string GetSongName(string FileName)
    {
        string Result = "";
        Stream Stream;
        string Ext;
        FixedArray_byte_28 Title;
        int P;
        Stream = OpenFile(AudioHost.BaseSoundEnginePath + FileName, 0 | 32);
        try
        {
            Ext = UpperCase(ExtractFileExt(FileName));
            Title = default;
            if (Ext == ".IT")
            {
                Stream.Seek(4, SeekOrigin.Begin);
                Stream.ReadExactly(((Span<byte>)Title).Slice(0, 28));
                Result = StrPasBuffer(Title);
            }
            else
            {
                if (Ext == ".S3M")
                {
                    Stream.ReadExactly(((Span<byte>)Title).Slice(0, 28));
                    Result = StrPasBuffer(Title);
                }
                else
                {
                    Result = ExtractFileName(FileName);
                    P = Pos(".", Result);
                    if (P != 0)
                    {
                        Result = Copy(Result, 1, P - 1);
                    }
                }
            }
        }
        finally
        {
            Stream?.Dispose();
        }

        return Result;
    }

    public static void ProcessSound()
    {
        if (AudioHost.SampleList != null)
        {
            for (int I = AudioHost.SampleList.Count - 1; I >= 0; I--)
            {
                ((Sample)AudioHost.SampleList.List[I]).Update();
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

    internal static void SetMusicVolume(int Volume)
    {
        if (AudioHost.SoundSystem != null)
        {
            AudioHost.SoundSystem.SetMusicVolume(Volume);
        }
    }

    internal static int GetMusicVolume()
    {
        int Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.GetMusicVolume();
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    internal static bool GetMusicEnabled()
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.GetMusicEnabled();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    internal static Sample PlaySample(string Name)
    {
        Sample Result;
        SampleData D;
        if (AudioHost.SoundSystem != null && AudioHost.SoundSystem.SFXEnabled)
        {
            try
            {
                D = AudioHost.SampleDataList.Find(Name);
                if (D == null)
                {
                    D = AudioHost.SoundSystem.SampleDataClass().Create(Name);
                    Result = AudioHost.SoundSystem.SampleClass().Create(D);
                    D.Release();
                }
                else
                {
                    Result = AudioHost.SoundSystem.SampleClass().Create(D);
                }

                Result.Execute();
            }
            catch
            {
                Result = null;
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public static Sample PlaySampleEx(string Name, int Volume, bool Looped, int Delay, SamplePanning Panning, SampleCallBack CallBack)
    {
        Sample Result;
        SampleData D;
        if (AudioHost.SoundSystem != null && AudioHost.SoundSystem.SFXEnabled)
        {
            try
            {
                D = AudioHost.SampleDataList.Find(Name);
                if (D == null)
                {
                    D = AudioHost.SoundSystem.SampleDataClass().Create(Name);
                    Result = AudioHost.SoundSystem.SampleClass().Create(D);
                    D.Release();
                }
                else
                {
                    Result = AudioHost.SoundSystem.SampleClass().Create(D);
                }

                Result.Looped = Looped;
                Result.FDelay = Delay;
                Result.Panning = Panning;
                Result.FCallBack = CallBack;
                Result.Volume = Volume;
                Result.Execute();
            }
            catch
            {
                Result = null;
            }
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    internal static bool PlaySong(string FileName)
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.PlaySong(FileName);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    internal static bool PlaySongEx(string FileName, int FadeOut, int FadeIn, bool Loop)
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.PlaySongEx(FileName, FadeOut, FadeIn, Loop);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    internal static SampleData CreateSampleData(string Name)
    {
        SampleData Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.SampleDataClass().Create(Name);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public static SampleData CreateSampleDataEx(ReadOnlySpan<WaveFormatEx> Format, object Data, int Size)
    {
        SampleData Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = AudioHost.SoundSystem.SampleDataClass().CreateEx(Format[0], Data, Size);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
