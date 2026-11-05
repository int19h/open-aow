// Translated from Sound.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;

namespace Aow.Units.Sound
{
    public enum TSampleStatusElement : byte
    {
        ssRunning = 0
    }

    public enum TSoundOutputModeElement : byte
    {
        omMono = 0,
        omStereo = 1,
        om8bit = 2,
        om16bit = 3,
        omHighQuality = 4
    }

    public enum TSoundSystemStatusElement : byte
    {
        ssActivated = 0,
        ssDirectSoundEnabled = 1
    }

    public enum TSongPlayerStatusElement : byte
    {
        spPlaying = 0,
        spPaused = 1
    }

    public enum TSampleCallBackMode : byte
    {
        scRestartLoop = 0,
        scDone = 1
    }

    // FDelay (Update ticks before it plays), FSampleData, FCallBack, FStatus. One playing sound
    // effect, in SampleList
    public partial class TSample : TObject
    {
        public static SampleClass Descriptor { get; } = new("TSample", Create);

        internal int FDelay;
        internal TSampleData FSampleData;
        internal TSampleCallBack FCallBack;
        internal TSet<TSampleStatusElement> FStatus;
        // Sound.pas:746
        protected internal virtual bool GetPlaying() => FDelay != 0;

        // Sound.pas:81
        protected internal virtual int GetVolume()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:82
        protected internal virtual void SetVolume(int Value)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:83
        protected internal virtual bool GetLooped()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:84
        protected internal virtual void SetLooped(bool Value)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:85
        protected internal virtual TSamplePanning GetPanning()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:86
        protected internal virtual void SetPanning(TSamplePanning Value)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:761
        // { Called every tick (ProcessSound): a delayed sample counts down and plays, a finished one that
//   does not loop destroys itself. }
        public virtual void Update()
        {
            if (FStatus.Contains(TSampleStatusElement.ssRunning) && !GetPlaying())
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
                        Destroy();
                    }
                }
            }
        }

        // Sound.pas:727
        protected TSample(ConstructionContext context, TSampleData ASampleData)
        {
            context.Capture(this);
            Sound.SampleList.Add(this);
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
        public override void Destroy()
        {
            if (FCallBack != null)
            {
                FCallBack(this, TSampleCallBackMode.scDone);
            }

            if (FSampleData != null)
            {
                FSampleData.Release();
            }

            FSampleData = null;
            Sound.SampleList.Remove(this);
            base.Destroy();
        }

        // Sound.pas:774
        public void Execute()
        {
            if (!FStatus.Contains(TSampleStatusElement.ssRunning))
            {
                if (FDelay == 0)
                {
                    Play();
                }

                FStatus = FStatus + TSet<TSampleStatusElement>.Of(TSampleStatusElement.ssRunning);
            }
        }

        public TSampleData SampleData { get => FSampleData; }
        public int Delay { get => FDelay; set => FDelay = value; }
        public TSampleCallBack CallBack { get => FCallBack; set => FCallBack = value; }
        public TSet<TSampleStatusElement> Status { get => FStatus; }
        public bool Playing { get => GetPlaying(); }
        public int Volume { get => GetVolume(); set => SetVolume(value); }
        public bool Looped { get => GetLooped(); set => SetLooped(value); }
        public TSamplePanning Panning { get => GetPanning(); set => SetPanning(value); }

        // Sound.pas:89
        public static TSample Create(TSampleData data) =>
            ConstructionContext.Create(data, static (context, value) => new TSample(context, value));

    }

    // FFileName. What BackupSongStatus saves. the sound system adds its own position
    public partial class TSongStatus : TObject
    {
        protected internal AnsiString FFileName;
        public AnsiString FileName { get => FFileName; }
    }

    public delegate void TSampleCallBack(TSample Sample, TSampleCallBackMode Mode);
    public enum TSamplePanning : byte
    {
        spLeft = 0,
        spCenter = 1,
        spRight = 2,
        spSurround = 3
    }

    // { The settings a sound system is activated with (24 bytes): +0 OutputMode, +4 SampleRate, +8
//     MusicEnabled, +9 UseDirectSound, +$C Window (the DirectSound window; the active window when
//     0), +$10 SFXEnabled, +$14 SampleVoices. }
    [StructLayout(LayoutKind.Explicit, Size = 24)]
    public struct TSoundSettings
    {
        [FieldOffset(0)]
        public byte OutputMode__image;
        public TSet<TSoundOutputModeElement> OutputMode { get => TSet<TSoundOutputModeElement>.FromMask(OutputMode__image); set => OutputMode__image = (byte)value.Image; }

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

    // FName, FRefCount. The sound data of one wave file, in SampleDataList while it is loaded.
    // Create counts the first reference
    public partial class TSampleData : TObject
    {
        public static SampleDataClass Descriptor { get; } = new("TSampleData", Create, CreateEx);

        internal AnsiString FName;
        internal int FRefCount;
        // Sound.pas:674
        protected TSampleData(ConstructionContext context, AnsiString Name)
        {
            context.Capture(this);
            FName = Name;
            Sound.SampleDataList.Add(this);
            AddRef();
        }

        // Sound.pas:683
        // { Sound data from memory; it is not put in SampleDataList. }
        protected TSampleData(ConstructionContext context, in TWaveFormatEx Format, object Data, int Size)
        {
            context.Capture(this);
            FName = (AnsiString)"";
            AddRef();
        }

        // Sound.pas:696
        public virtual void Release()
        {
            FRefCount -= 1;
            if (FRefCount == 0)
            {
                Destroy();
            }
        }

        // Sound.pas:703
        public virtual void AddRef()
        {
            FRefCount += 1;
        }

        // Sound.pas:690
        public override void Destroy()
        {
            Sound.SampleDataList.Remove(this);
            base.Destroy();
        }

        public AnsiString Name { get => FName; }
        public int RefCount { get => FRefCount; }

        // Sound.pas:60
        public static TSampleData Create(AnsiString name) =>
            ConstructionContext.Create(name, static (context, value) => new TSampleData(context, value));

        // Sound.pas:61
        public static TSampleData CreateEx(in TWaveFormatEx format, object data, int size) =>
            ConstructionContext.Create((format, data, size), static (context, value) =>
                new TSampleData(context, value.format, value.data, value.size));

    }

    // FRefCount, FStatus. The abstract sound system. GetName is a class function
    // (RegisterSoundSystem calls it on the class)
    public partial class TSoundSystem : TObject
    {
        protected internal int FRefCount;
        protected internal TSet<TSoundSystemStatusElement> FStatus;
        // Sound.pas:115
        protected internal virtual SampleClass SampleClass()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:116
        protected internal virtual SampleDataClass SampleDataClass()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:117
        protected internal virtual bool Activate(in TSoundSettings Settings)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:118
        protected internal virtual bool Deactivate()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:119
        protected internal virtual void Process()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:120
        protected internal virtual int GetMusicVolume()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:121
        protected internal virtual int GetSFXVolume()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:122
        protected internal virtual bool GetPlayingSong()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:123
        protected internal virtual AnsiString GetPlayingSongFileName()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:124
        protected internal virtual bool GetSFXEnabled()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:125
        protected internal virtual bool GetMusicEnabled()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:1152
        public TSoundSystem()
        {
            FRefCount = 1;
        }

        // Sound.pas:1165
        public virtual int Release()
        {
            FRefCount -= 1;
            int Result = FRefCount;
            if (FRefCount == 0)
            {
                Destroy();
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
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:131
        public virtual bool SetMusicEnabled(bool Value)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:132
        public virtual bool SetSoundSettings(in TSoundSettings Settings)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:133
        public virtual TSoundSettings GetSoundSettings()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:134
        public virtual bool FadeMusicVolume(int Volume, int Speed)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:135
        public virtual bool SetMusicVolume(int Volume)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:136
        public virtual bool FadeSFXVolume(int Volume, int Speed)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:137
        public virtual bool SetSFXVolume(int Volume)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:138
        public virtual bool PlaySong(AnsiString FileName)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:139
        public virtual bool PlaySongEx(AnsiString FileName, int FadeOut, int FadeIn, bool Loop)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:141
        public virtual bool StopSong()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:142
        public virtual TSongStatus BackupSongStatus()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:143
        public virtual bool RestoreSongStatus(TSongStatus Status)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:1158
        public override void Destroy()
        {
            if (FStatus.Contains(TSoundSystemStatusElement.ssActivated))
            {
                Deactivate();
            }

            base.Destroy();
        }

        public TSet<TSoundSystemStatusElement> Status { get => FStatus; }
        public int MusicVolume { get => GetMusicVolume(); }
        public int SFXVolume { get => GetSFXVolume(); }
        public bool PlayingSong { get => GetPlayingSong(); }
        public AnsiString PlayingSongFileName { get => GetPlayingSongFileName(); }
        public bool SFXEnabled { get => GetSFXEnabled(); }
        public bool MusicEnabled { get => GetMusicEnabled(); }

    }

    // the wave files TSFXManager found
    public partial class TSFXList : TStringList
    {

    }

    // FSearchPath, FSFX. ExecutePathSearch lists every wave file under a directory (with its
    // subdirectories) relative to the search path
    public partial class TSFXManager : TComponent
    {
        internal AnsiString FSearchPath;
        internal TSFXList FSFX;
        // Sound.pas:407
        public TSFXManager(TComponent owner) : base(owner)
        {
            FSFX = new TSFXList();
        }

        // Sound.pas:413
        public override void Destroy()
        {
            Clear();
            FSFX.Free();
            base.Destroy();
        }

        // Sound.pas:420
        public void Clear()
        {
            FSFX.Clear();
        }

        // Sound.pas:425
        public void ExecutePathSearch(AnsiString Path)
        {
            AnsiString OldDir;
            void Search(AnsiString Dir)
            {
                TSearchRec SR = default;
                TStringList Dirs;
                bool Found;
                Found = FindFirst(FSearchPath + Dir + (AnsiString)"*." + Sound.SFXExtension, 63 & ~16, ref SR) == 0;
                while (Found)
                {
                    FSFX.Add(UpperCase(Dir + SR.Name));
                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                Dirs = new TStringList();
                Found = FindFirst(Dir + (AnsiString)"*.*", 16, ref SR) == 0;
                while (Found)
                {
                    if ((SR.Attr & 16) != 0 && SR.Name != AnsiString.FromBytes(46) && SR.Name != (AnsiString)"..")
                    {
                        Dirs.Add(Dir + UpperCase(SR.Name) + AnsiString.FromBytes(92));
                    }

                    Found = FindNext(ref SR) == 0;
                }

                FindClose(ref SR);
                for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
                {
                    Search(Dirs[I]);
                }

                Dirs.Free();
            }

            FSFX.Clear();
            FSearchPath = UpperCase(Path);
            if (FSearchPath[FSearchPath.Length] != 92)
            {
                FSearchPath = FSearchPath + AnsiString.FromBytes(92);
            }

            OldDir = GetCurrentDir();
            SetCurrentDir(Path);
            Search((AnsiString)"");
            SetCurrentDir(OldDir);
            FSearchPath = UpperCase(ExtractRelativePath(Sound.BaseSoundEnginePath, FSearchPath));
        }

        [Published]
        public AnsiString SearchPath { get => FSearchPath; set => FSearchPath = value; }

        [Published]
        public TSFXList SFX { get => FSFX; }

    }

    // FSFXManager, FSubDirectory, FSkipDirectories ('.'-separated), FEmptySFXName
    public partial class TSFXManagerComboBox : TComboBox
    {
        internal TSFXManager FSFXManager;
        internal AnsiString FSubDirectory;
        internal AnsiString FSkipDirectories;
        internal AnsiString FEmptySFXName;
        // Sound.pas:472
        internal void SetSFXManager(TSFXManager Value)
        {
            FSFXManager = Value;
            GenerateItems();
        }

        // Sound.pas:478
        internal void SetSubDirectory(AnsiString Value)
        {
            FSubDirectory = UpperCase(Value);
            if (Value != (AnsiString)"" && FSubDirectory[FSubDirectory.Length] != 92)
            {
                FSubDirectory = FSubDirectory + AnsiString.FromBytes(92);
            }
        }

        // Sound.pas:491
        internal AnsiString GetSFXName()
        {
            /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
            throw new NotImplementedException();
        }

        // Sound.pas:499
        internal void SetSFXName(AnsiString Value)
        {
            /* TODO: Implement StdCtrls.Items in the platform or editor service. */
            throw new NotImplementedException();
        }

        // Sound.pas:485

        public TSFXManagerComboBox(TComponent owner) : base(owner)
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

        public AnsiString SFXName { get => GetSFXName(); set => SetSFXName(value); }

        [Published]
        public TSFXManager SFXManager { get => FSFXManager; set => SetSFXManager(value); }

        [Published]
        public AnsiString SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

        [Published]
        public AnsiString SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

        [Published]
        public AnsiString EmptySFXName { get => FEmptySFXName; set => FEmptySFXName = value; }
    }

    // FSFXManager, FSubDirectory, FSkipDirectories, FEmptySFXName. The same as the combo box
    public partial class TSFXManagerListBox : TListBox
    {
        internal TSFXManager FSFXManager;
        internal AnsiString FSubDirectory;
        internal AnsiString FSkipDirectories;
        internal AnsiString FEmptySFXName;
        // Sound.pas:566
        internal void SetSFXManager(TSFXManager Value)
        {
            FSFXManager = Value;
            GenerateItems();
        }

        // Sound.pas:572
        internal void SetSubDirectory(AnsiString Value)
        {
            FSubDirectory = UpperCase(Value);
            if (Value != (AnsiString)"" && FSubDirectory[FSubDirectory.Length] != 92)
            {
                FSubDirectory = FSubDirectory + AnsiString.FromBytes(92);
            }
        }

        // Sound.pas:586
        internal AnsiString GetSFXName()
        {
            /* TODO: Implement StdCtrls.ItemIndex in the platform or editor service. */
            throw new NotImplementedException();
        }

        // Sound.pas:594
        internal void SetSFXName(AnsiString Value)
        {
            /* TODO: Implement StdCtrls.Items in the platform or editor service. */
            throw new NotImplementedException();
        }

        // Sound.pas:579

        public TSFXManagerListBox(TComponent owner) : base(owner)
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

        public AnsiString SFXName { get => GetSFXName(); set => SetSFXName(value); }

        [Published]
        public TSFXManager SFXManager { get => FSFXManager; set => SetSFXManager(value); }

        [Published]
        public AnsiString SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

        [Published]
        public AnsiString SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

        [Published]
        public AnsiString EmptySFXName { get => FEmptySFXName; set => FEmptySFXName = value; }
    }

    // FSongs, FSongNames (the titles, from the files). An engine object with a list of music files
    public partial class TSonglist : TEObject
    {
        // Sound.pas:801
        public new const int ClassID = 2162692;

        public TSonglist(TECustomNode owner) : base(owner)
        {
            FSongs = new TStringList();
            FSongs.OnChange = SongsChanged;
            FSongNames = new TStringList();
        }

        internal TStringList FSongs;
        internal TStringList FSongNames;
        // Sound.pas:813
        internal void SetSongs(TStringList Value)
        {
            FSongs.Assign(Value);
        }

        // Sound.pas:819
        // { Reads the song titles; a file that cannot be read is dropped from the list. }
        internal void SongsChanged(TObject Sender)
        {
            FSongNames.Clear();
            for (int I = FSongs.Count - 1; I >= 0; I--)
            {
                try
                {
                    var songName = Sound.GetSongName(FSongs[I]);
                    FSongNames.Insert(0, songName);
                }
                catch (System.Exception)
                {
                    FSongs.Delete(I);
                }
            }
        }

        // Sound.pas:806
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(smText))
            {
                Stream.rwStrings(FSongs, 10);
            }
        }

        // Sound.pas:786


        // Sound.pas:794
        public override void Destroy()
        {
            FSongs.Free();
            FSongNames.Free();
            base.Destroy();
        }

        // Sound.pas:832
        public void Search(AnsiString Path, AnsiString Ext)
        {
            TSearchRec SR = default;
            AnsiString FullPath;
            bool Found;
            if (Path[Path.Length] != 92)
            {
                Path = Path + AnsiString.FromBytes(92);
            }

            FullPath = Sound.BaseSoundEnginePath + Path;
            FSongs.BeginUpdate();
            Found = FindFirst(FullPath + (AnsiString)"*." + Ext, 63 & ~16, ref SR) == 0;
            while (Found)
            {
                FSongs.Add(UpperCase(Path + SR.Name));
                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            FSongs.EndUpdate();
        }

        public TStringList Songs { get => FSongs; set => SetSongs(value); }
        public TStringList SongNames { get => FSongNames; }

    }

    // FCurrentSong, FStatus, FSongs, FSongNames, FCurrentIndex, FOnSongChange, FBackup (Pause),
    // FPauseCount. Plays the songs in turn (Update starts the next one)
    public partial class TSongPlayer : TObject
    {
        internal AnsiString FCurrentSong;
        internal TSet<TSongPlayerStatusElement> FStatus;
        internal TStringList FSongs;
        internal TStringList FSongNames;
        internal int FCurrentIndex;
        internal TNotifyEvent FOnSongChange;
        internal TSongStatus FBackup;
        internal int FPauseCount;
        // Sound.pas:870
        internal void SetSongs(TStringList Value)
        {
            FSongs.Assign(Value);
        }

        // Sound.pas:875
        internal void SongsChanged(TObject Sender)
        {
            FSongNames.Clear();
            for (int I = FSongs.Count - 1; I >= 0; I--)
            {
                try
                {
                    var songName = Sound.GetSongName(FSongs[I]);
                    FSongNames.Insert(0, songName);
                }
                catch (System.Exception)
                {
                    FSongs.Delete(I);
                }
            }

            if (FStatus.Contains(TSongPlayerStatusElement.spPlaying))
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
                                FCurrentSong = (AnsiString)"";
                            }
                        }
                    }

                    if (FCurrentIndex == -1 && !FStatus.Contains(TSongPlayerStatusElement.spPaused))
                    {
                        Play(0);
                    }
                }
                else
                {
                    if (Sound.SoundSystem != null)
                    {
                        Sound.SoundSystem.StopSong();
                    }

                    FCurrentIndex = -1;
                    FCurrentSong = (AnsiString)"";
                }
            }
        }

        // Sound.pas:854
        public TSongPlayer()
        {
            FSongs = new TStringList();
            FSongs.OnChange = SongsChanged;
            FSongNames = new TStringList();
            FCurrentIndex = -1;
        }

        // Sound.pas:863
        public override void Destroy()
        {
            FSongs.Free();
            FSongNames.Free();
            base.Destroy();
        }

        // Sound.pas:909
        // { Called every tick (ProcessSound): starts the next song when the last one has ended. }
        public void Update()
        {
            if (Sound.SoundSystem != null && FStatus.Contains(TSongPlayerStatusElement.spPlaying) && !FStatus.Contains(TSongPlayerStatusElement.spPaused) && !Sound.SoundSystem.PlayingSong && Sound.SoundSystem.MusicEnabled)
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
            if (Sound.SoundSystem != null && Index >= 0 && Index < FSongs.Count)
            {
                FStatus = FStatus + TSet<TSongPlayerStatusElement>.Of(TSongPlayerStatusElement.spPlaying);
                var item = FSongs[Index];
                Result = Sound.SoundSystem.PlaySongEx(item, FadeOut, FadeIn, false);
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
                    FCurrentSong = (AnsiString)"";
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
            if (Sound.SoundSystem != null && Index >= 0 && Index < FSongs.Count)
            {
                FStatus = FStatus + TSet<TSongPlayerStatusElement>.Of(TSongPlayerStatusElement.spPlaying);
                var item = FSongs[Index];
                Result = Sound.SoundSystem.PlaySongEx(item, 2, 0, false);
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
                    FCurrentSong = (AnsiString)"";
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
            if (Sound.SoundSystem != null)
            {
                if (StopMusic)
                {
                    Result = Sound.SoundSystem.StopSong();
                }
                else
                {
                    Result = false;
                }

                FStatus = FStatus - TSet<TSongPlayerStatusElement>.Of(TSongPlayerStatusElement.spPlaying);
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
                    FBackup.Free();
                }

                FBackup = Sound.BackupSongStatus();
                FStatus = FStatus + TSet<TSongPlayerStatusElement>.Of(TSongPlayerStatusElement.spPaused);
                if (StopMusic)
                {
                    Sound.SoundSystem.StopSong();
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
                        Sound.RestoreSongStatus(FBackup);
                    }

                    FBackup.Free();
                    FBackup = null;
                }

                FStatus = FStatus - TSet<TSongPlayerStatusElement>.Of(TSongPlayerStatusElement.spPaused);
            }

            return true;
        }

        public AnsiString CurrentSong { get => FCurrentSong; }
        public TSet<TSongPlayerStatusElement> Status { get => FStatus; }
        public TStringList Songs { get => FSongs; set => SetSongs(value); }
        public TStringList SongNames { get => FSongNames; }
        public int CurrentIndex { get => FCurrentIndex; }
        public TNotifyEvent OnSongChange { get => FOnSongChange; set => FOnSongChange = value; }

    }

    // FFileName. One wave file of a sequence
    public partial class TSFX : TEObject
    {
        public new const int ClassID = 2162689;

        public TSFX(TECustomNode owner) : base(owner)
        { }

        internal AnsiString FFileName;
        // Sound.pas:1027
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(smText))
            {
                Stream.rwString(ref FFileName, 5);
            }
        }

        // Sound.pas:1034
        public TSample Play() => Sound.PlaySample(FFileName);

        // Sound.pas:1039
        public TSample PlayEx(int Volume, bool Looped, int Delay, TSamplePanning Panning, TSampleCallBack CallBack) => Sound.PlaySampleEx(FFileName, Volume, Looped, Delay, Panning, CallBack);

        public AnsiString FileName { get => FFileName; set => FFileName = value; }

    }

    // FID. A sound effect: its children are TSFX, one of them plays (at random)
    public partial class TSFXSequence : TENode
    {
        public new const int ClassID = 2162690;

        public TSFXSequence(TECustomNode owner) : base(owner)
        { }

        internal int FID;
        // Sound.pas:1057
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FID, 5);
        }

        // Sound.pas:1052
        public TSFX GetSFX(int Index) => (TSFX)this.FChildren.List[Index];

        // Sound.pas:1063
        public TSample Play()
        {
            TSample Result;
            if (this.FChildren.Count != 0)
            {
                var random = Random(this.FChildren.Count);
                Result = ((TSFX)this.FChildren.List[random]).Play();
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1071
        public TSample PlayEx(int Volume, bool Looped, int Delay, TSamplePanning Panning, TSampleCallBack CallBack)
        {
            TSample Result = default;
            if (this.FChildren.Count != 0)
            {
                var random = Random(this.FChildren.Count);
                Result = ((TSFX)this.FChildren.List[random]).PlayEx(Volume, Looped, Delay, Panning, CallBack);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        public int ID { get => FID; set => FID = value; }

        public sealed class SFXAccessor(TSFXSequence target)
        {
            public TSFX this[int Index] { get => target.GetSFX(Index); }
        }

        public SFXAccessor SFX => new SFXAccessor(this);

    }

    // the sound effects (TSFXSequence children) by ID
    public partial class TSFXLibrary : TENode
    {
        public new const int ClassID = 2162691;

        public TSFXLibrary(TECustomNode owner) : base(owner)
        { }

        // Sound.pas:1088
        public TSFXSequence GetSFXSequence(int Index)
        {
            TSFXSequence Result;
            if (Index >= 0 && Index < this.FChildren.Count)
            {
                Result = (TSFXSequence)this.FChildren.List[Index];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1096
        public TSample Play(int ID)
        {
            TSample Result;
            TSFXSequence S;
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
        public TSample PlayEx(int ID, int Volume, bool Looped, int Delay, TSamplePanning Panning, TSampleCallBack CallBack)
        {
            TSample Result = default;
            TSFXSequence S;
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
        public TSFXSequence Find(int ID)
        {
            int I;
            I = 0;
            TSFXSequence Result = null;
            while (I != this.FChildren.Count && Result == null)
            {
                if (((TSFXSequence)this.FChildren.List[I]).FID == ID)
                {
                    Result = (TSFXSequence)this.FChildren.List[I];
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
                ID = ((TSFXSequence)this.FChildren.List[I]).FID;
                for (int J = I + 1, last = Count - 1; J <= last; J++)
                {
                    if (ID == ((TSFXSequence)this.FChildren.List[J]).FID)
                    {
                        Result = ID;
                    }
                }

                I += 1;
            }

            return Result;
        }

        public sealed class SFXSequencesAccessor(TSFXLibrary target)
        {
            public TSFXSequence this[int Index] { get => target.GetSFXSequence(Index); }
        }

        public SFXSequencesAccessor SFXSequences => new SFXSequencesAccessor(this);

    }

    // every TSample (ProcessSound updates them)
    public partial class TSampleList : TList
    {
        // Sound.pas:710
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // Sound.pas:716
        public new void Clear()
        {
            for (int I = Count - 1; I >= 0; I--)
            {
                ((TSample)List[I]).Destroy();
            }

            base.Clear();
        }

    }

    // the loaded TSampleData
    public partial class TSampleDataList : TList
    {
        // Sound.pas:659
        public TSampleData Find(AnsiString Name)
        {
            TSampleData Result;
            int I;
            I = 0;
            while (I != Count && CompareText(Name, ((TSampleData)List[I]).FName) != 0)
            {
                I += 1;
            }

            if (I != Count)
            {
                Result = (TSampleData)List[I];
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
    public partial class TENodeAccess : TECustomNode
    {
        public TENodeAccess(TECustomNode owner) : base(owner)
        { }

        internal TEObjectList FChildren;
    }

    // the registered sound system classes
    public partial class TSoundSystemList : TList
    {
        // Sound.pas:1293
        public SoundSystemClass Find(AnsiString Name)
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

    public static partial class Sound
    {
        public static AnsiString BaseSoundEnginePath;
        public static TSongPlayer SongPlayer;
        // Sound.pas:1182
        // { Unit routines }
        // { Loads the sound data (or adds a reference) so that it stays loaded between samples. }
        public static void LockSampleData(AnsiString Name)
        {
            TSampleData D;
            if (SoundSystem != null)
            {
                D = SampleDataList.Find(Name);
                if (D == null)
                {
                    SoundSystem.SampleDataClass().Create(Name);
                }
                else
                {
                    D.AddRef();
                }
            }
        }

        // Sound.pas:1196
        public static void UnlockSampleData(AnsiString Name)
        {
            TSampleData D;
            D = SampleDataList.Find(Name);
            if (D != null)
            {
                D.Release();
            }
        }

        // Sound.pas:1205
        public static TSampleData CreateSampleData(AnsiString Name)
        {
            TSampleData Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.SampleDataClass().Create(Name);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1213
        public static TSampleData CreateSampleDataEx(ReadOnlySpan<TWaveFormatEx> Format, object Data, int Size)
        {
            TSampleData Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.SampleDataClass().CreateEx(Format[0], Data, Size);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1221
        public static TSample CreateSample(TSampleData SampleData)
        {
            TSample Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.SampleClass().Create(SampleData);
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1229
        public static TSample PlaySample(AnsiString Name)
        {
            TSample Result;
            TSampleData D;
            if (SoundSystem != null && SoundSystem.SFXEnabled)
            {
                try
                {
                    D = SampleDataList.Find(Name);
                    if (D == null)
                    {
                        D = SoundSystem.SampleDataClass().Create(Name);
                        Result = SoundSystem.SampleClass().Create(D);
                        D.Release();
                    }
                    else
                    {
                        Result = SoundSystem.SampleClass().Create(D);
                    }

                    Result.Execute();
                }
                catch (System.Exception)
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

        // Sound.pas:1252
        public static TSample PlaySampleEx(AnsiString Name, int Volume, bool Looped, int Delay, TSamplePanning Panning, TSampleCallBack CallBack)
        {
            TSample Result;
            TSampleData D;
            if (SoundSystem != null && SoundSystem.SFXEnabled)
            {
                try
                {
                    D = SampleDataList.Find(Name);
                    if (D == null)
                    {
                        D = SoundSystem.SampleDataClass().Create(Name);
                        Result = SoundSystem.SampleClass().Create(D);
                        D.Release();
                    }
                    else
                    {
                        Result = SoundSystem.SampleClass().Create(D);
                    }

                    Result.Looped = Looped;
                    Result.FDelay = Delay;
                    Result.Panning = Panning;
                    Result.FCallBack = CallBack;
                    Result.Volume = Volume;
                    Result.Execute();
                }
                catch (System.Exception)
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

        // Sound.pas:1281
        public static int SamplesActive() => SampleList.Count;

        // Sound.pas:1286
        public static int SamplesLoaded() => SampleDataList.Count;

        // Sound.pas:1306
        public static void RegisterSoundSystem(SoundSystemClass AClass)
        {
            if (SoundSystems == null)
            {
                SoundSystems = new TSoundSystemList();
            }

            var name = AClass.Name;
            if (SoundSystems.Find(name) == null)
            {
                SoundSystems.Add(AClass);
            }
            else
            {
                throw Exception.Create((AnsiString)"Sound System with name: " + AClass.Name + (AnsiString)" already registered");
            }
        }

        // Sound.pas:1317
        // { Creates and activates the named sound system (the first registered one when Name is empty). }
        public static bool InitializeSoundSystem(AnsiString Name, in TSoundSettings Settings)
        {
            SoundSystemClass C;
            bool Result = true;
            if (Name == (AnsiString)"")
            {
                if (SoundSystems.Count != 0)
                {
                    C = (SoundSystemClass)SoundSystems[0];
                }
                else
                {
                    C = null;
                }
            }
            else
            {
                C = SoundSystems.Find(Name);
            }

            if (C != null)
            {
                if (SoundSystem != null)
                {
                    ReleaseSoundSystem();
                }

                SoundSystem = C.Create();
                if (!SoundSystem.Activate(Settings))
                {
                    SoundSystem.Free();
                    SoundSystem = null;
                    Result = false;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1347
        public static void ReleaseSoundSystem()
        {
            if (SoundSystem != null)
            {
                if (SongPlayer != null)
                {
                    SongPlayer.Stop(true);
                }

                if (SampleList != null)
                {
                    SampleList.Clear();
                }

                SoundSystem.Release();
                SoundSystem = null;
            }
        }

        // Sound.pas:1360
        public static TSet<TSoundSystemStatusElement> SoundSystemStatus()
        {
            TSet<TSoundSystemStatusElement> Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.FStatus;
            }
            else
            {
                Result = TSet<TSoundSystemStatusElement>.Of();
            }

            return Result;
        }

        // Sound.pas:1368
        public static void SetMusicVolume(int Volume)
        {
            if (SoundSystem != null)
            {
                SoundSystem.SetMusicVolume(Volume);
            }
        }

        // Sound.pas:1374
        public static void FadeMusicVolume(int Volume, int Speed)
        {
            if (SoundSystem != null)
            {
                SoundSystem.FadeMusicVolume(Volume, Speed);
            }
        }

        // Sound.pas:1380
        public static int GetMusicVolume()
        {
            int Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.GetMusicVolume();
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Sound.pas:1388
        public static void SetSFXVolume(int Volume)
        {
            if (SoundSystem != null)
            {
                SoundSystem.SetSFXVolume(Volume);
            }
        }

        // Sound.pas:1394
        public static int GetSFXVolume()
        {
            int Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.GetSFXVolume();
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // Sound.pas:1402
        public static bool GetMusicEnabled()
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.GetMusicEnabled();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1410
        public static bool SetMusicEnabled(bool Value)
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.SetMusicEnabled(Value);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1418
        public static bool GetSFXEnabled()
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.GetSFXEnabled();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1426
        public static bool SetSFXEnabled(bool Value)
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.SetSFXEnabled(Value);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1434
        public static TSoundSettings GetSoundSettings()
        {
            TSoundSettings Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.GetSoundSettings();
            }
            else
            {
                Result = default;
            }

            return Result;
        }

        // Sound.pas:1442
        public static bool SetSoundSettings(in TSoundSettings Settings)
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.SetSoundSettings(Settings);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1450
        public static TSongStatus BackupSongStatus()
        {
            TSongStatus Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.BackupSongStatus();
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1458
        public static bool RestoreSongStatus(TSongStatus Status)
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.RestoreSongStatus(Status);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1468
        // { The title stored in an Impulse Tracker or Scream Tracker 3 module, else the file name without
//   the extension. }
        public static AnsiString GetSongName(AnsiString FileName)
        {
            AnsiString Result = default;
            TFileStream Stream;
            AnsiString Ext;
            FixedArray_byte_28 Title;
            int P;
            Stream = new TFileStream(BaseSoundEnginePath + FileName, 0 | 32);
            try
            {
                Ext = UpperCase(ExtractFileExt(FileName));
                Title = default;
                if (Ext == (AnsiString)".IT")
                {
                    Stream.Seek(4, 0);
                    Stream.Read(((Span<byte>)Title).Slice(0, 28));
                    Result = StrPasBuffer(Title);
                }
                else
                {
                    if (Ext == (AnsiString)".S3M")
                    {
                        Stream.Read(((Span<byte>)Title).Slice(0, 28));
                        Result = StrPasBuffer(Title);
                    }
                    else
                    {
                        Result = ExtractFileName(FileName);
                        P = Pos(AnsiString.FromBytes(46), Result);
                        if (P != 0)
                        {
                            Result = Copy(Result, 1, P - 1);
                        }
                    }
                }
            }
            finally
            {
                Stream.Free();
            }

            return Result;
        }

        // Sound.pas:1502
        public static AnsiString PlayingSongFileName()
        {
            AnsiString Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.GetPlayingSongFileName();
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // Sound.pas:1510
        public static bool PlaySong(AnsiString FileName)
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.PlaySong(FileName);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1518
        public static bool PlaySongEx(AnsiString FileName, int FadeOut, int FadeIn, bool Loop)
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.PlaySongEx(FileName, FadeOut, FadeIn, Loop);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1526
        public static bool StopSong()
        {
            bool Result;
            if (SoundSystem != null)
            {
                Result = SoundSystem.StopSong();
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1534
        public static void ProcessSound()
        {
            if (SampleList != null)
            {
                for (int I = SampleList.Count - 1; I >= 0; I--)
                {
                    ((TSample)SampleList.List[I]).Update();
                }
            }

            if (SongPlayer != null)
            {
                SongPlayer.Update();
            }

            if (SoundSystem != null)
            {
                SoundSystem.Process();
            }
        }

        // Sound.pas:1547
        public static void RegisterSoundEngineClasses(TEngine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<TSFXSequence>(), EClass.Of<TSFXLibrary>(), EClass.Of<TSFX>()]);
        }

        // Sound.pas:1552
        public static void UnregisterSoundEngineClasses(TEngine AEngine)
        {
            AEngine.UnregisterEClasses([EClass.Of<TSFXSequence>(), EClass.Of<TSFXLibrary>(), EClass.Of<TSFX>()]);
        }

        // Sound.pas:1557
        public static void Register()
        {
            RegisterComponents((AnsiString)"Sound", [typeof(TSFXManager), typeof(TSFXManagerComboBox), typeof(TSFXManagerListBox)]);
        }

        public static readonly AnsiString SFXExtension = (AnsiString)"WAV";
        public static TSoundSystem SoundSystem = default;
        public static TSoundSystemList SoundSystems = default;
        public static TSampleDataList SampleDataList = default;
        public static TSampleList SampleList = default;
        public static void Initialize()
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

        public static void Finalize()
        {
            SongPlayer.Free();
            SongPlayer = null;
            SampleList.Free();
            SampleList = null;
            if (SampleDataList.Count != 0)
            {
                ShowMessage((AnsiString)"Inproper use of Sound Engine (SoundData still loaded)!");
            }

            SampleDataList.Free();
            SampleDataList = null;
            if (SoundSystem != null)
            {
                SoundSystem.Release();
            }

            SoundSystems.Free();
        }
    }
}
