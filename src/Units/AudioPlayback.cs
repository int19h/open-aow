using System.Runtime.InteropServices;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public static class AudioPlayback
{
    public static void LockSampleData(string Name)
    {
        TSampleData D;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            D = Aow.Game.Audio.AudioHost.SampleDataList.Find(Name);
            if (D == null)
            {
                Aow.Game.Audio.AudioHost.SoundSystem.SampleDataClass().Create(Name);
            }
            else
            {
                D.AddRef();
            }
        }
    }

    public static void UnlockSampleData(string Name)
    {
        TSampleData D;
        D = Aow.Game.Audio.AudioHost.SampleDataList.Find(Name);
        if (D != null)
        {
            D.Release();
        }
    }

    public static int SamplesActive() => Aow.Game.Audio.AudioHost.SampleList.Count;

    public static int SamplesLoaded() => Aow.Game.Audio.AudioHost.SampleDataList.Count;

    public static bool InitializeSoundSystem(string Name, in TSoundSettings Settings)
    {
        SoundSystemClass C;
        bool Result = true;
        if (IsEmpty(Name))
        {
            if (Aow.Game.Audio.AudioHost.SoundSystems.Count != 0)
            {
                C = (SoundSystemClass)Aow.Game.Audio.AudioHost.SoundSystems[0];
            }
            else
            {
                C = null;
            }
        }
        else
        {
            C = Aow.Game.Audio.AudioHost.SoundSystems.Find(Name);
        }

        if (C != null)
        {
            if (Aow.Game.Audio.AudioHost.SoundSystem != null)
            {
                Aow.Game.Audio.AudioPlayback.ReleaseSoundSystem();
            }

            Aow.Game.Audio.AudioHost.SoundSystem = C.Create();
            if (!Aow.Game.Audio.AudioHost.SoundSystem.Activate(Settings))
            {
                Aow.Game.Audio.AudioHost.SoundSystem?.Dispose();
                Aow.Game.Audio.AudioHost.SoundSystem = null;
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
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            if (Aow.Game.Audio.AudioHost.SongPlayer != null)
            {
                Aow.Game.Audio.AudioHost.SongPlayer.Stop(true);
            }

            if (Aow.Game.Audio.AudioHost.SampleList != null)
            {
                Aow.Game.Audio.AudioHost.SampleList.Clear();
            }

            Aow.Game.Audio.AudioHost.SoundSystem.Release();
            Aow.Game.Audio.AudioHost.SoundSystem = null;
        }
    }

    internal static void FadeMusicVolume(int Volume, int Speed)
    {
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Aow.Game.Audio.AudioHost.SoundSystem.FadeMusicVolume(Volume, Speed);
        }
    }

    internal static void SetSFXVolume(int Volume)
    {
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Aow.Game.Audio.AudioHost.SoundSystem.SetSFXVolume(Volume);
        }
    }

    internal static int GetSFXVolume()
    {
        int Result;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.GetSFXVolume();
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
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.GetSFXEnabled();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    internal static TSoundSettings GetSoundSettings()
    {
        TSoundSettings Result;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.GetSoundSettings();
        }
        else
        {
            Result = default;
        }

        return Result;
    }

    internal static TSongStatus BackupSongStatus()
    {
        TSongStatus Result;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.BackupSongStatus();
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
        Stream = OpenFile(Aow.Game.Audio.AudioHost.BaseSoundEnginePath + FileName, 0 | 32);
        try
        {
            Ext = UpperCase(ExtractFileExt(FileName));
            Title = default;
            if (Ext == ".IT")
            {
                Stream.Seek(4, SeekOrigin.Begin);
                Stream.ReadBytes(((Span<byte>)Title).Slice(0, 28));
                Result = StrPasBuffer(Title);
            }
            else
            {
                if (Ext == ".S3M")
                {
                    Stream.ReadBytes(((Span<byte>)Title).Slice(0, 28));
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
        if (Aow.Game.Audio.AudioHost.SampleList != null)
        {
            for (int I = Aow.Game.Audio.AudioHost.SampleList.Count - 1; I >= 0; I--)
            {
                ((TSample)Aow.Game.Audio.AudioHost.SampleList.List[I]).Update();
            }
        }

        if (Aow.Game.Audio.AudioHost.SongPlayer != null)
        {
            Aow.Game.Audio.AudioHost.SongPlayer.Update();
        }

        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Aow.Game.Audio.AudioHost.SoundSystem.Process();
        }
    }

    internal static void SetMusicVolume(int Volume)
    {
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Aow.Game.Audio.AudioHost.SoundSystem.SetMusicVolume(Volume);
        }
    }

    internal static int GetMusicVolume()
    {
        int Result;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.GetMusicVolume();
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
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.GetMusicEnabled();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    internal static TSample PlaySample(string Name)
    {
        TSample Result;
        TSampleData D;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null && Aow.Game.Audio.AudioHost.SoundSystem.SFXEnabled)
        {
            try
            {
                D = Aow.Game.Audio.AudioHost.SampleDataList.Find(Name);
                if (D == null)
                {
                    D = Aow.Game.Audio.AudioHost.SoundSystem.SampleDataClass().Create(Name);
                    Result = Aow.Game.Audio.AudioHost.SoundSystem.SampleClass().Create(D);
                    D.Release();
                }
                else
                {
                    Result = Aow.Game.Audio.AudioHost.SoundSystem.SampleClass().Create(D);
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

    public static TSample PlaySampleEx(string Name, int Volume, bool Looped, int Delay, TSamplePanning Panning, TSampleCallBack CallBack)
    {
        TSample Result;
        TSampleData D;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null && Aow.Game.Audio.AudioHost.SoundSystem.SFXEnabled)
        {
            try
            {
                D = Aow.Game.Audio.AudioHost.SampleDataList.Find(Name);
                if (D == null)
                {
                    D = Aow.Game.Audio.AudioHost.SoundSystem.SampleDataClass().Create(Name);
                    Result = Aow.Game.Audio.AudioHost.SoundSystem.SampleClass().Create(D);
                    D.Release();
                }
                else
                {
                    Result = Aow.Game.Audio.AudioHost.SoundSystem.SampleClass().Create(D);
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
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.PlaySong(FileName);
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
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.PlaySongEx(FileName, FadeOut, FadeIn, Loop);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    internal static TSampleData CreateSampleData(string Name)
    {
        TSampleData Result;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.SampleDataClass().Create(Name);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public static TSampleData CreateSampleDataEx(ReadOnlySpan<TWaveFormatEx> Format, object Data, int Size)
    {
        TSampleData Result;
        if (Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.SampleDataClass().CreateEx(Format[0], Data, Size);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
