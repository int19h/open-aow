// Translated from Sound.pas.
using System.Runtime.InteropServices;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Audio;

public enum SoundOutputModeElement : byte
{
    Mono = 0,
    Stereo = 1,
    Bits8 = 2,
    Bits16 = 3,
    HighQuality = 4
}

public enum SoundSystemStatusElement : byte
{
    Activated = 0,
    DirectSoundEnabled = 1
}

public enum SampleCallBackMode : byte
{
    RestartLoop = 0,
    Done = 1
}

[LegacyObject]
// FDelay (Update ticks before it plays), FSampleData, FCallBack, FStatus. One playing sound
// effect, in SampleList
public partial class Sample : IDisposable
{
    public static SampleClass Descriptor { get; } = new("Sample", Create);

    internal int FDelay;
    internal SampleData FSampleData;
    internal SampleCallBack FCallBack;
    internal Set<StatusElement> FStatus;

    // Sound.pas:746
    protected internal virtual bool GetPlaying() => FDelay != 0;

    // Sound.pas:81
    protected internal virtual int GetVolume()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:82
    protected internal virtual void SetVolume(int Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:83
    protected internal virtual bool GetLooped()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:84
    protected internal virtual void SetLooped(bool Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:85
    protected internal virtual SamplePanning GetPanning()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:86
    protected internal virtual void SetPanning(SamplePanning Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:761
    // { Called every tick (ProcessSound): a delayed sample counts down and plays, a finished one that
    //   does not loop destroys itself. }
    public virtual void Update()
    {
        if (FStatus.Contains(StatusElement.Running) && !GetPlaying())
        {
            if (FDelay != 0)
            {
                FDelay -= 1;
                if (FDelay == 0)
                {
                    Play();
                }
            }
            else
            {
                if (!GetLooped())
                {
                    Dispose();
                }
            }
        }
    }

    // Sound.pas:727
    protected Sample(ConstructionContext context, SampleData ASampleData)
    {
        CaptureAddress.Assign(this);
        context.Capture(this);
        Aow.Game.Audio.AudioHost.SampleList.Add(this);
        FSampleData = ASampleData;
        FSampleData.AddRef();
    }

    // Sound.pas:751
    public virtual void Play()
    {
    }

    // Sound.pas:755
    public virtual void Stop()
    {
    }

    // Sound.pas:735
    public virtual void Dispose()
    {
        if (FCallBack != null)
        {
            FCallBack(this, SampleCallBackMode.Done);
        }

        if (FSampleData != null)
        {
            FSampleData.Release();
        }

        FSampleData = null;
        Aow.Game.Audio.AudioHost.SampleList.Remove(this);
    }

    // Sound.pas:774
    public void Execute()
    {
        if (!FStatus.Contains(StatusElement.Running))
        {
            if (FDelay == 0)
            {
                Play();
            }

            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Running);
        }
    }

    public SampleData SampleData { get => FSampleData; }
    public int Delay { get => FDelay; set => FDelay = value; }
    public SampleCallBack CallBack { get => FCallBack; set => FCallBack = value; }
    public Set<StatusElement> Status { get => FStatus; }
    public bool Playing { get => GetPlaying(); }
    public int Volume { get => GetVolume(); set => SetVolume(value); }
    public bool Looped { get => GetLooped(); set => SetLooped(value); }
    public SamplePanning Panning { get => GetPanning(); set => SetPanning(value); }

    // Sound.pas:89
    public static Sample Create(SampleData data) =>
        ConstructionContext.Create(data, static (context, value) => new Sample(context, value));

    public enum StatusElement : byte
    {
        Running = 0
    }

}

[LegacyObject]
// FFileName. What BackupSongStatus saves. the sound system adds its own position
public partial class SongStatus
{
    protected internal string FFileName = "";

    public string FileName { get => FFileName; }

    public SongStatus()
    {
        CaptureAddress.Assign(this);
    }

    internal static bool RestoreSongStatus(SongStatus Status)
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.RestoreSongStatus(Status);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

}

public delegate void SampleCallBack(Sample Sample, SampleCallBackMode Mode);

public enum SamplePanning : byte
{
    Left = 0,
    Center = 1,
    Right = 2,
    Surround = 3
}

// { The settings a sound system is activated with (24 bytes): +0 OutputMode, +4 SampleRate, +8
//     MusicEnabled, +9 UseDirectSound, +$C Window (the DirectSound window; the active window when
//     0), +$10 SFXEnabled, +$14 SampleVoices. }
[StructLayout(LayoutKind.Explicit, Size = 24)]
public struct SoundSettings
{
    [FieldOffset(0)]
    public byte OutputMode__image;

    public Set<SoundOutputModeElement> OutputMode { get => Set<SoundOutputModeElement>.FromMask(OutputMode__image); set => OutputMode__image = (byte)value.Image; }

    [FieldOffset(4)]
    public int SampleRate;
    [FieldOffset(8)]
    public bool MusicEnabled;
    [FieldOffset(9)]
    public bool UseDirectSound;
    [FieldOffset(12)]
    public int Window;
    [FieldOffset(16)]
    public bool SFXEnabled;
    [FieldOffset(20)]
    public int SampleVoices;
}

[LegacyObject]
// FName, FRefCount. The sound data of one wave file, in SampleDataList while it is loaded.
// Create counts the first reference
public partial class SampleData : IDisposable
{
    public static SampleDataClass Descriptor { get; } = new("SampleData", Create, CreateEx);

    internal string FName = "";
    internal int FRefCount;

    // Sound.pas:674
    protected SampleData(ConstructionContext context, string Name)
    {
        CaptureAddress.Assign(this);
        context.Capture(this);
        FName = Name;
        Aow.Game.Audio.AudioHost.SampleDataList.Add(this);
        AddRef();
    }

    // Sound.pas:683
    // { Sound data from memory; it is not put in SampleDataList. }
    protected SampleData(ConstructionContext context, in WaveFormatEx Format, object Data, int Size)
    {
        CaptureAddress.Assign(this);
        context.Capture(this);
        FName = "";
        AddRef();
    }

    // Sound.pas:696
    public virtual void Release()
    {
        FRefCount -= 1;
        if (FRefCount == 0)
        {
            Dispose();
        }
    }

    // Sound.pas:703
    public virtual void AddRef()
    {
        FRefCount += 1;
    }

    // Sound.pas:690
    public virtual void Dispose()
    {
        Aow.Game.Audio.AudioHost.SampleDataList.Remove(this);
    }

    public string Name { get => FName; }
    public int RefCount { get => FRefCount; }

    // Sound.pas:60
    public static SampleData Create(string name) =>
        ConstructionContext.Create(name, static (context, value) => new SampleData(context, value));

    // Sound.pas:61
    public static SampleData CreateEx(in WaveFormatEx format, object data, int size) =>
        ConstructionContext.Create((format, data, size), static (context, value) =>
            new SampleData(context, value.format, value.data, value.size));

    public static Sample CreateSample(SampleData SampleData)
    {
        Sample Result;
        if (AudioHost.SoundSystem != null)
        {
            Result = Aow.Game.Audio.AudioHost.SoundSystem.SampleClass().Create(SampleData);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}

[LegacyObject]
// FRefCount, FStatus. The abstract sound system. GetName is a class function
// (RegisterSoundSystem calls it on the class)
public partial class SoundSystem : IDisposable
{
    protected internal int FRefCount;
    protected internal Set<SoundSystemStatusElement> FStatus;

    // Sound.pas:115
    protected internal virtual SampleClass SampleClass()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:116
    protected internal virtual SampleDataClass SampleDataClass()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:117
    protected internal virtual bool Activate(in SoundSettings Settings)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:118
    protected internal virtual bool Deactivate()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:119
    protected internal virtual void Process()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:120
    protected internal virtual int GetMusicVolume()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:121
    protected internal virtual int GetSFXVolume()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:122
    protected internal virtual bool GetPlayingSong()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:123
    protected internal virtual string GetPlayingSongFileName()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:124
    protected internal virtual bool GetSFXEnabled()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:125
    protected internal virtual bool GetMusicEnabled()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:1152
    public SoundSystem()
    {
        CaptureAddress.Assign(this);
        FRefCount = 1;
    }

    // Sound.pas:1165
    public virtual int Release()
    {
        FRefCount -= 1;
        int Result = FRefCount;
        if (FRefCount == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Sound.pas:1173
    public virtual int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Sound.pas:130
    public virtual bool SetSFXEnabled(bool Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:131
    public virtual bool SetMusicEnabled(bool Value)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:132
    public virtual bool SetSoundSettings(in SoundSettings Settings)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:133
    public virtual SoundSettings GetSoundSettings()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:134
    public virtual bool FadeMusicVolume(int Volume, int Speed)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:135
    public virtual bool SetMusicVolume(int Volume)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:136
    public virtual bool FadeSFXVolume(int Volume, int Speed)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:137
    public virtual bool SetSFXVolume(int Volume)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:138
    public virtual bool PlaySong(string FileName)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:139
    public virtual bool PlaySongEx(string FileName, int FadeOut, int FadeIn, bool Loop)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:141
    public virtual bool StopSong()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:142
    public virtual SongStatus BackupSongStatus()
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:143
    public virtual bool RestoreSongStatus(SongStatus Status)
    {
        throw AbstractErrorException.Create("Abstract Error");
    }

    // Sound.pas:1158
    public virtual void Dispose()
    {
        if (FStatus.Contains(SoundSystemStatusElement.Activated))
        {
            Deactivate();
        }
    }

    public Set<SoundSystemStatusElement> Status { get => FStatus; }
    public int MusicVolume { get => GetMusicVolume(); }
    public int SFXVolume { get => GetSFXVolume(); }
    public bool PlayingSong { get => GetPlayingSong(); }
    public string PlayingSongFileName { get => GetPlayingSongFileName(); }
    public bool SFXEnabled { get => GetSFXEnabled(); }
    public bool MusicEnabled { get => GetMusicEnabled(); }

}

// the wave files TSFXManager found
public partial class SFXList : StringList
{

}

// FSearchPath, FSFX. ExecutePathSearch lists every wave file under a directory (with its
// subdirectories) relative to the search path
public partial class SFXManager : Component
{
    internal string FSearchPath = "";
    internal SFXList FSFX;

    // Sound.pas:407
    public SFXManager(Component owner) : base(owner)
    {
        FSFX = new SFXList();
    }

    // Sound.pas:413
    public override void Dispose()
    {
        Clear();
        FSFX?.Dispose();
        base.Dispose();
    }

    // Sound.pas:420
    public void Clear()
    {
        FSFX.Clear();
    }

    // Sound.pas:425
    public void ExecutePathSearch(string Path)
    {
        string OldDir;
        void Search(string Dir)
        {
            SearchRec SR = default;
            StringList Dirs;
            bool Found;
            Found = FindFirst(FSearchPath + Dir + "*." + SFXExtension, 63 & ~16, ref SR) == 0;
            while (Found)
            {
                FSFX.Add(UpperCase(Dir + SR.Name));
                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            Dirs = new StringList();
            Found = FindFirst(Dir + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    Dirs.Add(Dir + UpperCase(SR.Name) + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
            {
                Search(Dirs[I]);
            }

            Dirs?.Dispose();
        }

        FSFX.Clear();
        FSearchPath = UpperCase(Path);
        if (FSearchPath[FSearchPath.Length - 1] != 92)
        {
            FSearchPath = FSearchPath + "\\";
        }

        OldDir = GetCurrentDir();
        SetCurrentDir(Path);
        Search("");
        SetCurrentDir(OldDir);
        FSearchPath = UpperCase(ExtractRelativePath(AudioHost.BaseSoundEnginePath, FSearchPath));
    }

    [Published]
    public string SearchPath { get => FSearchPath; set => FSearchPath = value; }

    [Published]
    public SFXList SFX { get => FSFX; }

    private static readonly string SFXExtension = "WAV";

}

// FSFXManager, FSubDirectory, FSkipDirectories ('.'-separated), FEmptySFXName
public partial class SFXManagerComboBox : ComboBox
{
    internal SFXManager FSFXManager;
    internal string FSubDirectory = "";
    internal string FSkipDirectories = "";
    internal string FEmptySFXName = "";

    // Sound.pas:472
    internal void SetSFXManager(SFXManager Value)
    {
        FSFXManager = Value;
        GenerateItems();
    }

    // Sound.pas:478
    internal void SetSubDirectory(string Value)
    {
        FSubDirectory = UpperCase(Value);
        if (!IsEmpty(Value) && FSubDirectory[FSubDirectory.Length - 1] != 92)
        {
            FSubDirectory = FSubDirectory + "\\";
        }
    }

    // Sound.pas:491
    internal string GetSFXName()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:499
    internal void SetSFXName(string Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:485

    public SFXManagerComboBox(Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:518
    // { Fills the items with the manager's files under SubDirectory (without the subdirectory), less
    //   those in SkipDirectories, after EmptySFXName. }
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    public string SFXName { get => GetSFXName(); set => SetSFXName(value); }

    [Published]
    public SFXManager SFXManager { get => FSFXManager; set => SetSFXManager(value); }

    [Published]
    public string SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

    [Published]
    public string SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

    [Published]
    public string EmptySFXName { get => FEmptySFXName; set => FEmptySFXName = value; }
}

// FSFXManager, FSubDirectory, FSkipDirectories, FEmptySFXName. The same as the combo box
public partial class SFXManagerListBox : ListBox
{
    internal SFXManager FSFXManager;
    internal string FSubDirectory = "";
    internal string FSkipDirectories = "";
    internal string FEmptySFXName = "";

    // Sound.pas:566
    internal void SetSFXManager(SFXManager Value)
    {
        FSFXManager = Value;
        GenerateItems();
    }

    // Sound.pas:572
    internal void SetSubDirectory(string Value)
    {
        FSubDirectory = UpperCase(Value);
        if (!IsEmpty(Value) && FSubDirectory[FSubDirectory.Length - 1] != 92)
        {
            FSubDirectory = FSubDirectory + "\\";
        }
    }

    // Sound.pas:586
    internal string GetSFXName()
    {
        /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:594
    internal void SetSFXName(string Value)
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:579

    public SFXManagerListBox(Component owner) : base(owner)
    {
        /* TODO: Implement StdCtrls.Create in the platform or editor service. */
        throw new NotImplementedException();
    }

    // Sound.pas:611
    public void GenerateItems()
    {
        /* TODO: Implement StdCtrls.Items in the platform or editor service. */
        throw new NotImplementedException();
    }

    public string SFXName { get => GetSFXName(); set => SetSFXName(value); }

    [Published]
    public SFXManager SFXManager { get => FSFXManager; set => SetSFXManager(value); }

    [Published]
    public string SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

    [Published]
    public string SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

    [Published]
    public string EmptySFXName { get => FEmptySFXName; set => FEmptySFXName = value; }
}

// FSongs, FSongNames (the titles, from the files). An engine object with a list of music files
public partial class Songlist : EObject
{
    // Sound.pas:801
    public new const int ClassID = 2162692;

    public Songlist(ECustomNode owner) : base(owner)
    {
        FSongs = new StringList();
        FSongs.OnChange = SongsChanged;
        FSongNames = new StringList();
    }

    internal StringList FSongs;
    internal StringList FSongNames;

    // Sound.pas:813
    internal void SetSongs(StringList Value)
    {
        FSongs.Assign(Value);
    }

    // Sound.pas:819
    // { Reads the song titles; a file that cannot be read is dropped from the list. }
    internal void SongsChanged(object Sender)
    {
        FSongNames.Clear();
        for (int I = FSongs.Count - 1; I >= 0; I--)
        {
            try
            {
                var songName = Aow.Game.Audio.AudioPlayback.GetSongName(FSongs[I]);
                FSongNames.Insert(0, songName);
            }
            catch
            {
                FSongs.Delete(I);
            }
        }
    }

    // Sound.pas:806
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            Stream.rwStrings(FSongs, 10);
        }
    }

    // Sound.pas:786

    // Sound.pas:794
    public override void Dispose()
    {
        FSongs?.Dispose();
        FSongNames?.Dispose();
        base.Dispose();
    }

    // Sound.pas:832
    public void Search(string Path, string Ext)
    {
        SearchRec SR = default;
        string FullPath;
        bool Found;
        if (Path[Path.Length - 1] != 92)
        {
            Path = Path + "\\";
        }

        FullPath = AudioHost.BaseSoundEnginePath + Path;
        FSongs.BeginUpdate();
        Found = FindFirst(FullPath + "*." + Ext, 63 & ~16, ref SR) == 0;
        while (Found)
        {
            FSongs.Add(UpperCase(Path + SR.Name));
            Found = FindNext(ref SR) == 0;
        }

        FindClose(ref SR);
        FSongs.EndUpdate();
    }

    public StringList Songs { get => FSongs; set => SetSongs(value); }
    public StringList SongNames { get => FSongNames; }

}

[LegacyObject]
// FCurrentSong, FStatus, FSongs, FSongNames, FCurrentIndex, FOnSongChange, FBackup (Pause),
// FPauseCount. Plays the songs in turn (Update starts the next one)
public partial class SongPlayer : IDisposable
{
    internal string FCurrentSong = "";
    internal Set<StatusElement> FStatus;
    internal StringList FSongs;
    internal StringList FSongNames;
    internal int FCurrentIndex;
    internal NotifyEvent<SongPlayer> FOnSongChange;
    internal SongStatus FBackup;
    internal int FPauseCount;

    // Sound.pas:870
    internal void SetSongs(StringList Value)
    {
        FSongs.Assign(Value);
    }

    // Sound.pas:875
    internal void SongsChanged(object Sender)
    {
        FSongNames.Clear();
        for (int I = FSongs.Count - 1; I >= 0; I--)
        {
            try
            {
                var songName = Aow.Game.Audio.AudioPlayback.GetSongName(FSongs[I]);
                FSongNames.Insert(0, songName);
            }
            catch
            {
                FSongs.Delete(I);
            }
        }

        if (FStatus.Contains(StatusElement.Playing))
        {
            if (FSongs.Count != 0)
            {
                if (FCurrentIndex != -1)
                {
                    var count = FSongs.Count;
                    if (FCurrentIndex >= count || FSongs[FCurrentIndex] != FCurrentSong)
                    {
                        FCurrentIndex = FSongs.IndexOf(FCurrentSong);
                        if (FCurrentIndex == -1)
                        {
                            FCurrentSong = "";
                        }
                    }
                }

                if (FCurrentIndex == -1 && !FStatus.Contains(StatusElement.Paused))
                {
                    Play(0);
                }
            }
            else
            {
                if (AudioHost.SoundSystem != null)
                {
                    Aow.Game.Audio.AudioHost.SoundSystem.StopSong();
                }

                FCurrentIndex = -1;
                FCurrentSong = "";
            }
        }
    }

    // Sound.pas:854
    public SongPlayer()
    {
        CaptureAddress.Assign(this);
        FSongs = new StringList();
        FSongs.OnChange = SongsChanged;
        FSongNames = new StringList();
        FCurrentIndex = -1;
    }

    // Sound.pas:863
    public virtual void Dispose()
    {
        FSongs?.Dispose();
        FSongNames?.Dispose();
    }

    // Sound.pas:909
    // { Called every tick (ProcessSound): starts the next song when the last one has ended. }
    public void Update()
    {
        if (AudioHost.SoundSystem != null && FStatus.Contains(StatusElement.Playing) && !FStatus.Contains(StatusElement.Paused) && !AudioHost.SoundSystem.PlayingSong && AudioHost.SoundSystem.MusicEnabled)
        {
            FCurrentIndex += 1;
            if (FSongs.Count <= FCurrentIndex)
            {
                FCurrentIndex = 0;
            }

            var count = FSongs.Count;
            if (FCurrentIndex < count)
            {
                Play(FCurrentIndex);
            }
            else
            {
                FCurrentIndex = -1;
            }
        }
    }

    // Sound.pas:924
    public bool PlayEx(int Index, int FadeOut, int FadeIn)
    {
        bool Result;
        if (AudioHost.SoundSystem != null && Index >= 0 && Index < FSongs.Count)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Playing);
            var item = FSongs[Index];
            Result = Aow.Game.Audio.AudioHost.SoundSystem.PlaySongEx(item, FadeOut, FadeIn, false);
            if (Result)
            {
                FCurrentIndex = Index;
                FCurrentSong = FSongs[Index];
                if (FOnSongChange != null)
                {
                    FOnSongChange(this);
                }
            }
            else
            {
                FCurrentSong = "";
                FCurrentIndex = -1;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Sound.pas:947
    public bool Play(int Index)
    {
        bool Result;
        if (AudioHost.SoundSystem != null && Index >= 0 && Index < FSongs.Count)
        {
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Playing);
            var item = FSongs[Index];
            Result = Aow.Game.Audio.AudioHost.SoundSystem.PlaySongEx(item, 2, 0, false);
            if (Result)
            {
                FCurrentIndex = Index;
                FCurrentSong = FSongs[Index];
                if (FOnSongChange != null)
                {
                    FOnSongChange(this);
                }
            }
            else
            {
                FCurrentSong = "";
                FCurrentIndex = -1;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Sound.pas:970
    public bool Stop(bool StopMusic)
    {
        bool Result;
        if (AudioHost.SoundSystem != null)
        {
            if (StopMusic)
            {
                Result = Aow.Game.Audio.AudioHost.SoundSystem.StopSong();
            }
            else
            {
                Result = false;
            }

            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Playing);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Sound.pas:985
    // { Pauses count; the first one saves the song position. }
    public bool Pause(bool StopMusic)
    {
        if (FPauseCount == 0)
        {
            if (FBackup != null)
            {
            }

            FBackup = Aow.Game.Audio.AudioPlayback.BackupSongStatus();
            FStatus = FStatus + Set<StatusElement>.Of(StatusElement.Paused);
            if (StopMusic)
            {
                Aow.Game.Audio.AudioHost.SoundSystem.StopSong();
            }
        }

        FPauseCount += 1;
        return true;
    }

    // Sound.pas:1000
    public bool Resume()
    {
        FPauseCount -= 1;
        if (FPauseCount == 0)
        {
            if (FBackup != null)
            {
                FCurrentIndex = FSongs.IndexOf(FBackup.FFileName);
                if (FCurrentIndex == -1)
                {
                    Play(0);
                }
                else
                {
                    Aow.Game.Audio.SongStatus.RestoreSongStatus(FBackup);
                }
                FBackup = null;
            }

            FStatus = FStatus - Set<StatusElement>.Of(StatusElement.Paused);
        }

        return true;
    }

    public string CurrentSong { get => FCurrentSong; }
    public Set<StatusElement> Status { get => FStatus; }
    public StringList Songs { get => FSongs; set => SetSongs(value); }
    public StringList SongNames { get => FSongNames; }
    public int CurrentIndex { get => FCurrentIndex; }
    public NotifyEvent<SongPlayer> OnSongChange { get => FOnSongChange; set => FOnSongChange = value; }

    public enum StatusElement : byte
    {
        Playing = 0, Paused = 1
    }

}

// FFileName. One wave file of a sequence
public partial class SFX : EObject
{
    public new const int ClassID = 2162689;

    public SFX(ECustomNode owner) : base(owner)
    { }

    internal string FFileName = "";

    // Sound.pas:1027
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(EngineServices.smText))
        {
            Stream.rwString(ref FFileName, 5);
        }
    }

    // Sound.pas:1034
    public Sample Play() => Aow.Game.Audio.AudioPlayback.PlaySample(FFileName);

    // Sound.pas:1039
    public Sample PlayEx(int Volume, bool Looped, int Delay, SamplePanning Panning, SampleCallBack CallBack) => Aow.Game.Audio.AudioPlayback.PlaySampleEx(FFileName, Volume, Looped, Delay, Panning, CallBack);

    public string FileName { get => FFileName; set => FFileName = value; }

}

// FID. A sound effect: its children are TSFX, one of them plays (at random)
public partial class SFXSequence : ENode
{
    public new const int ClassID = 2162690;

    public SFXSequence(ECustomNode owner) : base(owner)
    { }

    internal int FID;

    // Sound.pas:1057
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FID, 5);
    }

    // Sound.pas:1052
    public SFX GetSFX(int Index) => (SFX)this.FChildren.List[Index];

    // Sound.pas:1063
    public Sample Play()
    {
        Sample Result;
        if (this.FChildren.Count != 0)
        {
            var random = Random(this.FChildren.Count);
            Result = ((SFX)this.FChildren.List[random]).Play();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1071
    public Sample PlayEx(int Volume, bool Looped, int Delay, SamplePanning Panning, SampleCallBack CallBack)
    {
        Sample Result = default;
        if (this.FChildren.Count != 0)
        {
            var random = Random(this.FChildren.Count);
            Result = ((SFX)this.FChildren.List[random]).PlayEx(Volume, Looped, Delay, Panning, CallBack);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    public int ID { get => FID; set => FID = value; }

    public sealed class SFXAccessor(SFXSequence target)
    {
        public SFX this[int Index] { get => target.GetSFX(Index); }
    }

    public SFXAccessor SFX => new SFXAccessor(this);

}

// the sound effects (TSFXSequence children) by ID
public partial class SFXLibrary : ENode
{
    public new const int ClassID = 2162691;

    public SFXLibrary(ECustomNode owner) : base(owner)
    { }

    // Sound.pas:1088
    public SFXSequence GetSFXSequence(int Index)
    {
        SFXSequence Result;
        if (Index >= 0 && Index < this.FChildren.Count)
        {
            Result = (SFXSequence)this.FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1096
    public Sample Play(int ID)
    {
        Sample Result;
        SFXSequence S;
        S = Find(ID);
        if (S != null)
        {
            Result = S.Play();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1107
    public Sample PlayEx(int ID, int Volume, bool Looped, int Delay, SamplePanning Panning, SampleCallBack CallBack)
    {
        Sample Result = default;
        SFXSequence S;
        S = Find(ID);
        if (S != null)
        {
            Result = S.PlayEx(Volume, Looped, Delay, Panning, CallBack);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Sound.pas:1119
    public SFXSequence Find(int ID)
    {
        int I;
        I = 0;
        SFXSequence Result = null;
        while (I != this.FChildren.Count && Result == null)
        {
            if (((SFXSequence)this.FChildren.List[I]).FID == ID)
            {
                Result = (SFXSequence)this.FChildren.List[I];
            }

            I += 1;
        }

        return Result;
    }

    // Sound.pas:1134
    // { The first ID that two sequences share, -1 when the IDs are unique. }
    public int DoubleID()
    {
        int I;
        int ID;
        I = 0;
        int Result = -1;
        while (Result == -1 && I != Count)
        {
            ID = ((SFXSequence)this.FChildren.List[I]).FID;
            for (int J = I + 1, last = Count - 1; J <= last; J++)
            {
                if (ID == ((SFXSequence)this.FChildren.List[J]).FID)
                {
                    Result = ID;
                }
            }

            I += 1;
        }

        return Result;
    }

    public sealed class SFXSequencesAccessor(SFXLibrary target)
    {
        public SFXSequence this[int Index] { get => target.GetSFXSequence(Index); }
    }

    public SFXSequencesAccessor SFXSequences => new SFXSequencesAccessor(this);

}

// every TSample (ProcessSound updates them)
public partial class SampleList : DelphiList
{
    // Sound.pas:710
    public override void Dispose()
    {
        Clear();
        base.Dispose();
    }

    // Sound.pas:716
    public new void Clear()
    {
        for (int I = Count - 1; I >= 0; I--)
        {
            ((List[I] ?? throw new NullReferenceException()) as IDisposable)?.Dispose();
        }

        base.Clear();
    }

}

// the loaded TSampleData
public partial class SampleDataList : DelphiList
{
    // Sound.pas:659
    public SampleData Find(string Name)
    {
        SampleData Result;
        int I;
        I = 0;
        while (I != Count && CompareText(Name, ((SampleData)List[I]).FName) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = (SampleData)List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}

// { TENode keeps its child list private in Engine.pas: the sequences and the library reach it
//     through this copy of TENode's layout (+8 FChildren). The code is the same as with a
//     protected FChildren, which is probably what the original Engine unit had. }
public partial class ENodeAccess : ECustomNode
{
    public ENodeAccess(ECustomNode owner) : base(owner)
    { }

    internal EObjectList FChildren;
}

// the registered sound system classes
public partial class SoundSystemList : DelphiList
{
    // Sound.pas:1293
    public SoundSystemClass Find(string Name)
    {
        SoundSystemClass Result;
        int I;
        I = 0;
        while (I != Count && CompareText(Name, ((SoundSystemClass)List[I]).Name) != 0)
        {
            I += 1;
        }

        if (I != Count)
        {
            Result = (SoundSystemClass)List[I];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

}
