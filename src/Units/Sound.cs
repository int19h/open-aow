// Translated from Sound.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.EList;
using static Aow.Units.EList.EList;

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

    // { Sound.TSample (instance size 24): +4 FDelay (Update ticks before it plays), +8 FSampleData,
//     +$C FCallBack, +$14 FStatus. One playing sound effect, in SampleList. }
    public partial class TSample : Aow.Rtl.TObject
    {
        public static SampleClass Descriptor { get; } = new(Create);

        internal int FDelay;
        internal Aow.Units.Sound.TSampleData FSampleData;
        internal Aow.Units.Sound.TSampleCallBack FCallBack;
        internal TSet<Aow.Units.Sound.TSampleStatusElement> FStatus;
        // Sound.pas:746
        protected internal virtual bool GetPlaying()
        {
            bool Result = default;
            Result = FDelay != 0;
            return Result;
        }

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
        protected internal virtual Aow.Units.Sound.TSamplePanning GetPanning()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:86
        protected internal virtual void SetPanning(Aow.Units.Sound.TSamplePanning Value)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:761
        // { Called every tick (ProcessSound): a delayed sample counts down and plays, a finished one that
//   does not loop destroys itself. }
        public virtual void Update()
        {
            bool bool__1 = FStatus.Contains(Aow.Units.Sound.TSampleStatusElement.ssRunning);
            if (bool__1)
            {
                var call__2 = GetPlaying();
                bool__1 = !call__2;
            }

            if (bool__1)
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
        // { TSample }
        protected TSample(ConstructionContext context, TSampleData ASampleData)
        {
            context.Capture(this);
            Aow.Units.Sound.Sound.SampleList.Add(this);
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
                FCallBack(this, Aow.Units.Sound.TSampleCallBackMode.scDone);
            }

            if (FSampleData != null)
            {
                FSampleData.Release();
            }

            FSampleData = null;
            Aow.Units.Sound.Sound.SampleList.Remove(this);
            base.Destroy();
        }

        // Sound.pas:774
        public void Execute()
        {
            if (!FStatus.Contains(Aow.Units.Sound.TSampleStatusElement.ssRunning))
            {
                if (FDelay == 0)
                {
                    Play();
                }

                FStatus = FStatus + TSet<Aow.Units.Sound.TSampleStatusElement>.Of(Aow.Units.Sound.TSampleStatusElement.ssRunning);
            }
        }

        public Aow.Units.Sound.TSampleData SampleData { get => FSampleData; }
        public int Delay { get => FDelay; set => FDelay = value; }
        public Aow.Units.Sound.TSampleCallBack CallBack { get => FCallBack; set => FCallBack = value; }
        public TSet<Aow.Units.Sound.TSampleStatusElement> Status { get => FStatus; }
        public bool Playing { get => GetPlaying(); }
        public int Volume { get => GetVolume(); set => SetVolume(value); }
        public bool Looped { get => GetLooped(); set => SetLooped(value); }
        public Aow.Units.Sound.TSamplePanning Panning { get => GetPanning(); set => SetPanning(value); }

        // Sound.pas:89
        public static TSample Create(TSampleData data) =>
            ConstructionContext.Create(data, static (context, value) => new TSample(context, value));

    }

    // { Sound.TSongStatus (instance size 8): +4 FFileName. What BackupSongStatus saves; the sound
//     system adds its own position. }
    public partial class TSongStatus : Aow.Rtl.TObject
    {
        protected internal Aow.Rtl.AnsiString FFileName;
        public Aow.Rtl.AnsiString FileName { get => FFileName; }
    }

    public delegate void TSampleCallBack(Aow.Units.Sound.TSample Sample, Aow.Units.Sound.TSampleCallBackMode Mode);
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
        public TSet<Aow.Units.Sound.TSoundOutputModeElement> OutputMode { get => TSet<Aow.Units.Sound.TSoundOutputModeElement>.FromMask((ulong)OutputMode__image); set => OutputMode__image = (byte)value.Image; }

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

    // { Sound.TSampleData (instance size 12): +4 FName, +8 FRefCount. The sound data of one wave
//     file, in SampleDataList while it is loaded; Create counts the first reference. }
    public partial class TSampleData : Aow.Rtl.TObject
    {
        public static SampleDataClass Descriptor { get; } = new(Create, CreateEx);

        internal Aow.Rtl.AnsiString FName;
        internal int FRefCount;
        // Sound.pas:674
        // { TSampleData }
        protected TSampleData(ConstructionContext context, AnsiString Name)
        {
            context.Capture(this);
            FName = Name;
            Aow.Units.Sound.Sound.SampleDataList.Add(this);
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
            Aow.Units.Sound.Sound.SampleDataList.Remove(this);
            base.Destroy();
        }

        public Aow.Rtl.AnsiString Name { get => FName; }
        public int RefCount { get => FRefCount; }

        // Sound.pas:60
        public static TSampleData Create(AnsiString name) =>
            ConstructionContext.Create(name, static (context, value) => new TSampleData(context, value));

        // Sound.pas:61
        public static TSampleData CreateEx(in TWaveFormatEx format, object data, int size) =>
            ConstructionContext.Create((format, data, size), static (context, value) =>
                new TSampleData(context, value.format, value.data, value.size));

    }

    // { Sound.TSoundSystem (instance size 12): +4 FRefCount, +8 FStatus. The abstract sound system;
//     GetName is a class function (RegisterSoundSystem calls it on the class). }
    public partial class TSoundSystem : Aow.Rtl.TObject
    {
        protected internal int FRefCount;
        protected internal TSet<Aow.Units.Sound.TSoundSystemStatusElement> FStatus;
        // Sound.pas:115
        protected internal virtual Aow.Units.Sound.SampleClass SampleClass()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:116
        protected internal virtual Aow.Units.Sound.SampleDataClass SampleDataClass()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:117
        protected internal virtual bool Activate(in Aow.Units.Sound.TSoundSettings Settings)
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
        protected internal virtual Aow.Rtl.AnsiString GetPlayingSongFileName()
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
        // { TSoundSystem }
        public TSoundSystem()
        {
            FRefCount = 1;
        }

        // Sound.pas:1165
        public virtual int Release()
        {
            int Result = default;
            FRefCount -= 1;
            Result = FRefCount;
            if (FRefCount == 0)
            {
                Destroy();
            }

            return Result;
        }

        // Sound.pas:1173
        public virtual int AddRef()
        {
            int Result = default;
            FRefCount += 1;
            Result = FRefCount;
            return Result;
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
        public virtual bool SetSoundSettings(in Aow.Units.Sound.TSoundSettings Settings)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:133
        public virtual Aow.Units.Sound.TSoundSettings GetSoundSettings()
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
        public virtual bool PlaySong(Aow.Rtl.AnsiString FileName)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:139
        public virtual bool PlaySongEx(Aow.Rtl.AnsiString FileName, int FadeOut, int FadeIn, bool Loop)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:141
        public virtual bool StopSong()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:142
        public virtual Aow.Units.Sound.TSongStatus BackupSongStatus()
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:143
        public virtual bool RestoreSongStatus(Aow.Units.Sound.TSongStatus Status)
        {
            throw EAbstractError.Create("Abstract Error");
        }

        // Sound.pas:1158
        public override void Destroy()
        {
            if (FStatus.Contains(Aow.Units.Sound.TSoundSystemStatusElement.ssActivated))
            {
                Deactivate();
            }

            base.Destroy();
        }

        public TSet<Aow.Units.Sound.TSoundSystemStatusElement> Status { get => FStatus; }
        public int MusicVolume { get => GetMusicVolume(); }
        public int SFXVolume { get => GetSFXVolume(); }
        public bool PlayingSong { get => GetPlayingSong(); }
        public Aow.Rtl.AnsiString PlayingSongFileName { get => GetPlayingSongFileName(); }
        public bool SFXEnabled { get => GetSFXEnabled(); }
        public bool MusicEnabled { get => GetMusicEnabled(); }

    }

    // { Sound.TSFXList (instance size 44): the wave files TSFXManager found }
    public partial class TSFXList : Aow.Rtl.TStringList
    {

    }

    // { Sound.TSFXManager (instance size 44): +$24 FSearchPath, +$28 FSFX. ExecutePathSearch lists
//     every wave file under a directory (with its subdirectories) relative to the search path. }
    public partial class TSFXManager : Aow.Rtl.TComponent
    {
        internal Aow.Rtl.AnsiString FSearchPath;
        internal Aow.Units.Sound.TSFXList FSFX;
        // Sound.pas:407
        // { TSFXManager }
        public TSFXManager(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
            var call__3 = new Aow.Units.Sound.TSFXList();
            FSFX = call__3;
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
        public void ExecutePathSearch(Aow.Rtl.AnsiString Path)
        {
            Aow.Rtl.AnsiString OldDir = default;
            void Search(Aow.Rtl.AnsiString Dir)
            {
                Aow.Rtl.TSearchRec SR = default;
                Aow.Rtl.TStringList Dirs = default;
                bool Found = default;
                int I = default;
                var call__4 = Aow.Rtl.Rtl.FindFirst(((FSearchPath + Dir) + ((AnsiString)"*.")) + Aow.Units.Sound.Sound.SFXExtension, 63 & ~16, ref SR);
                Found = call__4 == 0;
                while (Found)
                {
                    FSFX.Add(Aow.Rtl.Rtl.UpperCase(Dir + SR.Name));
                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
                Dirs = new Aow.Rtl.TStringList();
                Found = Aow.Rtl.Rtl.FindFirst(Dir + ((AnsiString)"*.*"), 16, ref SR) == 0;
                while (Found)
                {
                    if ((((SR.Attr & 16) != 0) && (SR.Name != AnsiString.FromBytes(46))) && (SR.Name != ((AnsiString)"..")))
                    {
                        Dirs.Add((Dir + Aow.Rtl.Rtl.UpperCase(SR.Name)) + AnsiString.FromBytes(92));
                    }

                    Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
                }

                Aow.Rtl.Rtl.FindClose(ref SR);
                {
                    I = 0;
                    int end__5 = Dirs.Count - 1;
                    for (; I <= end__5; I += 1)
                    {
                        var call__6 = Dirs[I];
                        Search(call__6);
                    }
                }

                Dirs.Free();
            }

            FSFX.Clear();
            FSearchPath = Aow.Rtl.Rtl.UpperCase(Path);
            if (FSearchPath[FSearchPath.Length] != 92)
            {
                FSearchPath = FSearchPath + AnsiString.FromBytes(92);
            }

            OldDir = Aow.Rtl.Rtl.GetCurrentDir();
            Aow.Rtl.Rtl.SetCurrentDir(Path);
            Search((AnsiString)"");
            Aow.Rtl.Rtl.SetCurrentDir(OldDir);
            var call__7 = Aow.Rtl.Rtl.ExtractRelativePath(Aow.Units.Sound.Sound.BaseSoundEnginePath, FSearchPath);
            FSearchPath = Aow.Rtl.Rtl.UpperCase(call__7);
        }

        [Published]
        public Aow.Rtl.AnsiString SearchPath { get => FSearchPath; set => FSearchPath = value; }

        [Published]
        public Aow.Units.Sound.TSFXList SFX { get => FSFX; }

    }

    // { Sound.TSFXManagerComboBox (instance size 384): +$170 FSFXManager, +$174 FSubDirectory, +$178
//     FSkipDirectories (';'-separated), +$17C FEmptySFXName. }
    public partial class TSFXManagerComboBox : Aow.Rtl.TComboBox
    {
        internal Aow.Units.Sound.TSFXManager FSFXManager;
        internal Aow.Rtl.AnsiString FSubDirectory;
        internal Aow.Rtl.AnsiString FSkipDirectories;
        internal Aow.Rtl.AnsiString FEmptySFXName;
        // Sound.pas:472
        // { TSFXManagerComboBox }
        internal void SetSFXManager(Aow.Units.Sound.TSFXManager Value)
        {
            FSFXManager = Value;
            GenerateItems();
        }

        // Sound.pas:478
        internal void SetSubDirectory(Aow.Rtl.AnsiString Value)
        {
            FSubDirectory = Aow.Rtl.Rtl.UpperCase(Value);
            if ((Value != ((AnsiString)"")) && (FSubDirectory[FSubDirectory.Length] != 92))
            {
                FSubDirectory = FSubDirectory + AnsiString.FromBytes(92);
            }
        }

        // Sound.pas:491
        internal Aow.Rtl.AnsiString GetSFXName()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.ItemIndex */
            throw new System.NotImplementedException();
        }

        // Sound.pas:499
        internal void SetSFXName(Aow.Rtl.AnsiString Value)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // Sound.pas:485
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Create */
            throw new System.NotImplementedException();
        }

        // Sound.pas:518
        // { Fills the items with the manager's files under SubDirectory (without the subdirectory), less
//   those in SkipDirectories, after EmptySFXName. }
        public void GenerateItems()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        public Aow.Rtl.AnsiString SFXName { get => GetSFXName(); set => SetSFXName(value); }

        [Published]
        public Aow.Units.Sound.TSFXManager SFXManager { get => FSFXManager; set => SetSFXManager(value); }

        [Published]
        public Aow.Rtl.AnsiString SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

        [Published]
        public Aow.Rtl.AnsiString SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

        [Published]
        public Aow.Rtl.AnsiString EmptySFXName { get => FEmptySFXName; set => FEmptySFXName = value; }

        // Sound.pas:189
        public static Aow.Units.Sound.TSFXManagerComboBox Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.Sound.TSFXManagerComboBox o = new Aow.Units.Sound.TSFXManagerComboBox();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Sound.TSFXManagerListBox (instance size 356): +$154 FSFXManager, +$158 FSubDirectory, +$15C
//     FSkipDirectories, +$160 FEmptySFXName. The same as the combo box. }
    public partial class TSFXManagerListBox : Aow.Rtl.TListBox
    {
        internal Aow.Units.Sound.TSFXManager FSFXManager;
        internal Aow.Rtl.AnsiString FSubDirectory;
        internal Aow.Rtl.AnsiString FSkipDirectories;
        internal Aow.Rtl.AnsiString FEmptySFXName;
        // Sound.pas:566
        // { TSFXManagerListBox }
        internal void SetSFXManager(Aow.Units.Sound.TSFXManager Value)
        {
            FSFXManager = Value;
            GenerateItems();
        }

        // Sound.pas:572
        internal void SetSubDirectory(Aow.Rtl.AnsiString Value)
        {
            FSubDirectory = Aow.Rtl.Rtl.UpperCase(Value);
            if ((Value != ((AnsiString)"")) && (FSubDirectory[FSubDirectory.Length] != 92))
            {
                FSubDirectory = FSubDirectory + AnsiString.FromBytes(92);
            }
        }

        // Sound.pas:586
        internal Aow.Rtl.AnsiString GetSFXName()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.ItemIndex */
            throw new System.NotImplementedException();
        }

        // Sound.pas:594
        internal void SetSFXName(Aow.Rtl.AnsiString Value)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        // Sound.pas:579
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Create */
            throw new System.NotImplementedException();
        }

        // Sound.pas:611
        public void GenerateItems()
        {
            /* TODO(T-shim) Platform or editor boundary StdCtrls.Items */
            throw new System.NotImplementedException();
        }

        public Aow.Rtl.AnsiString SFXName { get => GetSFXName(); set => SetSFXName(value); }

        [Published]
        public Aow.Units.Sound.TSFXManager SFXManager { get => FSFXManager; set => SetSFXManager(value); }

        [Published]
        public Aow.Rtl.AnsiString SubDirectory { get => FSubDirectory; set => SetSubDirectory(value); }

        [Published]
        public Aow.Rtl.AnsiString SkipDirectories { get => FSkipDirectories; set => FSkipDirectories = value; }

        [Published]
        public Aow.Rtl.AnsiString EmptySFXName { get => FEmptySFXName; set => FEmptySFXName = value; }

        // Sound.pas:212
        public static Aow.Units.Sound.TSFXManagerListBox Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.Sound.TSFXManagerListBox o = new Aow.Units.Sound.TSFXManagerListBox();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { Sound.TSonglist (instance size 16): +8 FSongs, +$C FSongNames (the titles, from the
//     files). An engine object with a list of music files. }
    public partial class TSonglist : Aow.Units.Engine.TEObject
    {
        // Sound.pas:801
        public new const int ClassID = 2162692;

        // Sound.pas:786
        private void InitializeSonglist(Aow.Units.Engine.TECustomNode AOwner)
        {
            var call__11 = new Aow.Rtl.TStringList();
            FSongs = call__11;
            FSongs.OnChange = SongsChanged;
            var call__12 = new Aow.Rtl.TStringList();
            FSongNames = call__12;
        }

        // Raw allocation for TStartupSongList.
        protected TSonglist() { }

        public TSonglist(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner)
        {
            InitializeSonglist(AOwner);
        }

        internal Aow.Rtl.TStringList FSongs;
        internal Aow.Rtl.TStringList FSongNames;
        // Sound.pas:813
        internal void SetSongs(Aow.Rtl.TStringList Value)
        {
            FSongs.Assign(Value);
        }

        // Sound.pas:819
        // { Reads the song titles; a file that cannot be read is dropped from the list. }
        internal void SongsChanged(Aow.Rtl.TObject Sender)
        {
            int I = default;
            FSongNames.Clear();
            {
                I = FSongs.Count - 1;
                byte end__8 = 0;
                for (; I >= end__8; I -= 1)
                {
                    try
                    {
                        var call__9 = FSongs[I];
                        var call__10 = Aow.Units.Sound.Sound.GetSongName(call__9);
                        FSongNames.Insert(0, call__10);
                    }
                    catch (System.Exception)
                    {
                        FSongs.Delete(I);
                    }
                }
            }
        }

        // Sound.pas:806
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.Engine.smText))
            {
                Stream.rwStrings(FSongs, 10);
            }
        }

        // Sound.pas:786
        // { TSonglist }
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            InitializeSonglist(AOwner);
        }

        // Sound.pas:794
        public override void Destroy()
        {
            FSongs.Free();
            FSongNames.Free();
            base.Destroy();
        }

        // Sound.pas:832
        public void Search(Aow.Rtl.AnsiString Path, Aow.Rtl.AnsiString Ext)
        {
            Aow.Rtl.TSearchRec SR = default;
            Aow.Rtl.AnsiString FullPath = default;
            bool Found = default;
            if (Path[Path.Length] != 92)
            {
                Path = Path + AnsiString.FromBytes(92);
            }

            FullPath = Aow.Units.Sound.Sound.BaseSoundEnginePath + Path;
            FSongs.BeginUpdate();
            Found = Aow.Rtl.Rtl.FindFirst((FullPath + ((AnsiString)"*.")) + Ext, 63 & ~16, ref SR) == 0;
            while (Found)
            {
                FSongs.Add(Aow.Rtl.Rtl.UpperCase(Path + SR.Name));
                Found = Aow.Rtl.Rtl.FindNext(ref SR) == 0;
            }

            Aow.Rtl.Rtl.FindClose(ref SR);
            FSongs.EndUpdate();
        }

        public Aow.Rtl.TStringList Songs { get => FSongs; set => SetSongs(value); }
        public Aow.Rtl.TStringList SongNames { get => FSongNames; }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Sound.pas:801
            public override int ClassID() => Aow.Units.Sound.TSonglist.ClassID;

            // Sound.pas:243
            public new Aow.Units.Sound.TSonglist Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Sound.TSonglist)base.Create(AOwner);

            public static new Meta Of<T>()
                where T : Aow.Units.Sound.TSonglist => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { Sound.TSongPlayer (instance size 40): +4 FCurrentSong, +8 FStatus, +$C FSongs, +$10
//     FSongNames, +$14 FCurrentIndex, +$18 FOnSongChange, +$20 FBackup (Pause), +$24
//     FPauseCount. Plays the songs in turn (Update starts the next one). }
    public partial class TSongPlayer : Aow.Rtl.TObject
    {
        internal Aow.Rtl.AnsiString FCurrentSong;
        internal TSet<Aow.Units.Sound.TSongPlayerStatusElement> FStatus;
        internal Aow.Rtl.TStringList FSongs;
        internal Aow.Rtl.TStringList FSongNames;
        internal int FCurrentIndex;
        internal Aow.Rtl.TNotifyEvent FOnSongChange;
        internal Aow.Units.Sound.TSongStatus FBackup;
        internal int FPauseCount;
        // Sound.pas:870
        internal void SetSongs(Aow.Rtl.TStringList Value)
        {
            FSongs.Assign(Value);
        }

        // Sound.pas:875
        internal void SongsChanged(Aow.Rtl.TObject Sender)
        {
            int I = default;
            FSongNames.Clear();
            {
                I = FSongs.Count - 1;
                byte end__13 = 0;
                for (; I >= end__13; I -= 1)
                {
                    try
                    {
                        var call__14 = FSongs[I];
                        var call__15 = Aow.Units.Sound.Sound.GetSongName(call__14);
                        FSongNames.Insert(0, call__15);
                    }
                    catch (System.Exception)
                    {
                        FSongs.Delete(I);
                    }
                }
            }

            if (FStatus.Contains(Aow.Units.Sound.TSongPlayerStatusElement.spPlaying))
            {
                var call__16 = FSongs.Count;
                if (call__16 != 0)
                {
                    if (FCurrentIndex != -1)
                    {
                        var call__17 = FSongs.Count;
                        bool bool__18 = (FCurrentIndex >= call__17);
                        if (!bool__18)
                        {
                            var call__19 = FSongs[FCurrentIndex];
                            bool__18 = (call__19 != FCurrentSong);
                        }

                        if (bool__18)
                        {
                            var call__20 = FSongs.IndexOf(FCurrentSong);
                            FCurrentIndex = call__20;
                            if (FCurrentIndex == -1)
                            {
                                FCurrentSong = (AnsiString)"";
                            }
                        }
                    }

                    if ((FCurrentIndex == -1) && !FStatus.Contains(Aow.Units.Sound.TSongPlayerStatusElement.spPaused))
                    {
                        Play(0);
                    }
                }
                else
                {
                    if (Aow.Units.Sound.Sound.SoundSystem != null)
                    {
                        Aow.Units.Sound.Sound.SoundSystem.StopSong();
                    }

                    FCurrentIndex = -1;
                    FCurrentSong = (AnsiString)"";
                }
            }
        }

        // Sound.pas:854
        // { TSongPlayer }
        public TSongPlayer()
        {
            var call__21 = new Aow.Rtl.TStringList();
            FSongs = call__21;
            FSongs.OnChange = SongsChanged;
            var call__22 = new Aow.Rtl.TStringList();
            FSongNames = call__22;
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
            bool bool__23 = ((Aow.Units.Sound.Sound.SoundSystem != null) && FStatus.Contains(Aow.Units.Sound.TSongPlayerStatusElement.spPlaying)) && !FStatus.Contains(Aow.Units.Sound.TSongPlayerStatusElement.spPaused);
            if (bool__23)
            {
                var call__24 = Aow.Units.Sound.Sound.SoundSystem.PlayingSong;
                var call__25 = !call__24;
                bool__23 = call__25;
            }

            bool bool__26 = bool__23;
            if (bool__26)
            {
                var call__27 = Aow.Units.Sound.Sound.SoundSystem.MusicEnabled;
                bool__26 = call__27;
            }

            if (bool__26)
            {
                FCurrentIndex += 1;
                var call__28 = FSongs.Count;
                if (call__28 <= FCurrentIndex)
                {
                    FCurrentIndex = 0;
                }

                var call__29 = FSongs.Count;
                if (FCurrentIndex < call__29)
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
            bool Result = default;
            bool bool__30 = (Aow.Units.Sound.Sound.SoundSystem != null) && (Index >= 0);
            if (bool__30)
            {
                var call__31 = FSongs.Count;
                bool__30 = (Index < call__31);
            }

            if (bool__30)
            {
                FStatus = FStatus + TSet<Aow.Units.Sound.TSongPlayerStatusElement>.Of(Aow.Units.Sound.TSongPlayerStatusElement.spPlaying);
                var call__32 = FSongs[Index];
                var call__33 = Aow.Units.Sound.Sound.SoundSystem.PlaySongEx(call__32, FadeOut, FadeIn, false);
                Result = call__33;
                if (Result)
                {
                    FCurrentIndex = Index;
                    var call__34 = FSongs[Index];
                    FCurrentSong = call__34;
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
            bool Result = default;
            bool bool__35 = (Aow.Units.Sound.Sound.SoundSystem != null) && (Index >= 0);
            if (bool__35)
            {
                var call__36 = FSongs.Count;
                bool__35 = (Index < call__36);
            }

            if (bool__35)
            {
                FStatus = FStatus + TSet<Aow.Units.Sound.TSongPlayerStatusElement>.Of(Aow.Units.Sound.TSongPlayerStatusElement.spPlaying);
                var call__37 = FSongs[Index];
                var call__38 = Aow.Units.Sound.Sound.SoundSystem.PlaySongEx(call__37, 2, 0, false);
                Result = call__38;
                if (Result)
                {
                    FCurrentIndex = Index;
                    var call__39 = FSongs[Index];
                    FCurrentSong = call__39;
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
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                if (StopMusic)
                {
                    Result = Aow.Units.Sound.Sound.SoundSystem.StopSong();
                }
                else
                {
                    Result = false;
                }

                FStatus = FStatus - TSet<Aow.Units.Sound.TSongPlayerStatusElement>.Of(Aow.Units.Sound.TSongPlayerStatusElement.spPlaying);
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
            bool Result = default;
            if (FPauseCount == 0)
            {
                if (FBackup != null)
                {
                    FBackup.Free();
                }

                var call__40 = Aow.Units.Sound.Sound.BackupSongStatus();
                FBackup = call__40;
                FStatus = FStatus + TSet<Aow.Units.Sound.TSongPlayerStatusElement>.Of(Aow.Units.Sound.TSongPlayerStatusElement.spPaused);
                if (StopMusic)
                {
                    Aow.Units.Sound.Sound.SoundSystem.StopSong();
                }
            }

            FPauseCount += 1;
            Result = true;
            return Result;
        }

        // Sound.pas:1000
        public bool Resume()
        {
            bool Result = default;
            FPauseCount -= 1;
            if (FPauseCount == 0)
            {
                if (FBackup != null)
                {
                    var call__41 = FSongs.IndexOf(FBackup.FFileName);
                    FCurrentIndex = call__41;
                    if (FCurrentIndex == -1)
                    {
                        Play(0);
                    }
                    else
                    {
                        Aow.Units.Sound.Sound.RestoreSongStatus(FBackup);
                    }

                    FBackup.Free();
                    FBackup = null;
                }

                FStatus = FStatus - TSet<Aow.Units.Sound.TSongPlayerStatusElement>.Of(Aow.Units.Sound.TSongPlayerStatusElement.spPaused);
            }

            Result = true;
            return Result;
        }

        public Aow.Rtl.AnsiString CurrentSong { get => FCurrentSong; }
        public TSet<Aow.Units.Sound.TSongPlayerStatusElement> Status { get => FStatus; }
        public Aow.Rtl.TStringList Songs { get => FSongs; set => SetSongs(value); }
        public Aow.Rtl.TStringList SongNames { get => FSongNames; }
        public int CurrentIndex { get => FCurrentIndex; }
        public Aow.Rtl.TNotifyEvent OnSongChange { get => FOnSongChange; set => FOnSongChange = value; }

    }

    // { Sound.TSFX (instance size 12): +8 FFileName. One wave file of a sequence. }
    public partial class TSFX : Aow.Units.Engine.TEObject
    {
        public new const int ClassID = 2162689;

        public TSFX(TECustomNode owner) : base(owner) { }

        internal Aow.Rtl.AnsiString FFileName;
        // Sound.pas:1027
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            if (!Stream.StorageMode().Contains(Aow.Units.Engine.Engine.smText))
            {
                Stream.rwString(ref FFileName, 5);
            }
        }

        // Sound.pas:1034
        public Aow.Units.Sound.TSample Play()
        {
            Aow.Units.Sound.TSample Result = default;
            var call__42 = Aow.Units.Sound.Sound.PlaySample(FFileName);
            Result = call__42;
            return Result;
        }

        // Sound.pas:1039
        public Aow.Units.Sound.TSample PlayEx(int Volume, bool Looped, int Delay, Aow.Units.Sound.TSamplePanning Panning, Aow.Units.Sound.TSampleCallBack CallBack)
        {
            Aow.Units.Sound.TSample Result = default;
            var call__43 = Aow.Units.Sound.Sound.PlaySampleEx(FFileName, Volume, Looped, Delay, Panning, CallBack);
            Result = call__43;
            return Result;
        }

        public Aow.Rtl.AnsiString FileName { get => FFileName; set => FFileName = value; }

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // Sound.pas:1022
            // { TSFX }
            public override int ClassID() => Aow.Units.Sound.TSFX.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Sound.TSFX => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.Sound.TSFX Create(TECustomNode owner) => (Aow.Units.Sound.TSFX)base.Create(owner);
        }
    }

    // { Sound.TSFXSequence (instance size 16): +$C FID. A sound effect: its children are TSFX, one
//     of them plays (at random). }
    public partial class TSFXSequence : Aow.Units.Engine.TENode
    {
        public new const int ClassID = 2162690;

        public TSFXSequence(TECustomNode owner) : base(owner) { }

        internal int FID;
        // Sound.pas:1057
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FID, 5);
        }

        // Sound.pas:1052
        public Aow.Units.Sound.TSFX GetSFX(int Index)
        {
            Aow.Units.Sound.TSFX Result = default;
            Result = (Aow.Units.Sound.TSFX)this.FChildren.List[Index];
            return Result;
        }

        // Sound.pas:1063
        public Aow.Units.Sound.TSample Play()
        {
            Aow.Units.Sound.TSample Result = default;
            if (this.FChildren.Count != 0)
            {
                var call__44 = Aow.Rtl.Rtl.Random(this.FChildren.Count);
                var call__45 = ((Aow.Units.Sound.TSFX)this.FChildren.List[call__44]).Play();
                Result = call__45;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1071
        public Aow.Units.Sound.TSample PlayEx(int Volume, bool Looped, int Delay, Aow.Units.Sound.TSamplePanning Panning, Aow.Units.Sound.TSampleCallBack CallBack)
        {
            Aow.Units.Sound.TSample Result = default;
            if (this.FChildren.Count != 0)
            {
                var call__46 = Aow.Rtl.Rtl.Random(this.FChildren.Count);
                var call__47 = ((Aow.Units.Sound.TSFX)this.FChildren.List[call__46]).PlayEx(Volume, Looped, Delay, Panning, CallBack);
                Result = call__47;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        public int ID { get => FID; set => FID = value; }

        public sealed class SFXAccessor(Aow.Units.Sound.TSFXSequence target)
        {
            public Aow.Units.Sound.TSFX this[int Index] { get => target.GetSFX(Index); }
        }

        public SFXAccessor SFX => new SFXAccessor(this);

        public new partial class Meta : Aow.Units.Engine.TENode.Meta
        {
            // Sound.pas:1047
            // { TSFXSequence }
            public override int ClassID() => Aow.Units.Sound.TSFXSequence.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Sound.TSFXSequence => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:631
            public new Aow.Units.Sound.TSFXSequence Create(TECustomNode owner) => (Aow.Units.Sound.TSFXSequence)base.Create(owner);
        }
    }

    // { Sound.TSFXLibrary (instance size 12): the sound effects (TSFXSequence children) by ID. }
    public partial class TSFXLibrary : Aow.Units.Engine.TENode
    {
        public new const int ClassID = 2162691;

        public TSFXLibrary(Aow.Units.Engine.TECustomNode AOwner) : base(AOwner) { }

        // Sound.pas:1088
        public Aow.Units.Sound.TSFXSequence GetSFXSequence(int Index)
        {
            Aow.Units.Sound.TSFXSequence Result = default;
            if ((Index >= 0) && (Index < this.FChildren.Count))
            {
                Result = (Aow.Units.Sound.TSFXSequence)this.FChildren.List[Index];
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1096
        public Aow.Units.Sound.TSample Play(int ID)
        {
            Aow.Units.Sound.TSample Result = default;
            Aow.Units.Sound.TSFXSequence S = default;
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
        public Aow.Units.Sound.TSample PlayEx(int ID, int Volume, bool Looped, int Delay, Aow.Units.Sound.TSamplePanning Panning, Aow.Units.Sound.TSampleCallBack CallBack)
        {
            Aow.Units.Sound.TSample Result = default;
            Aow.Units.Sound.TSFXSequence S = default;
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
        public Aow.Units.Sound.TSFXSequence Find(int ID)
        {
            Aow.Units.Sound.TSFXSequence Result = default;
            int I = default;
            I = 0;
            Result = null;
            while ((I != this.FChildren.Count) && (Result == null))
            {
                if (((Aow.Units.Sound.TSFXSequence)this.FChildren.List[I]).FID == ID)
                {
                    Result = (Aow.Units.Sound.TSFXSequence)this.FChildren.List[I];
                }

                I += 1;
            }

            return Result;
        }

        // Sound.pas:1134
        // { The first ID that two sequences share, -1 when the IDs are unique. }
        public int DoubleID()
        {
            int Result = default;
            int I = default;
            int J = default;
            int ID = default;
            I = 0;
            Result = -1;
            while (true)
            {
                bool bool__48 = (Result == -1);
                if (bool__48)
                {
                    var call__49 = Count;
                    bool__48 = (I != call__49);
                }

                if (!bool__48)
                    break;
                {
                    ID = ((Aow.Units.Sound.TSFXSequence)this.FChildren.List[I]).FID;
                    {
                        J = I + 1;
                        int end__50 = Count - 1;
                        for (; J <= end__50; J += 1)
                        {
                            if (ID == ((Aow.Units.Sound.TSFXSequence)this.FChildren.List[J]).FID)
                            {
                                Result = ID;
                            }
                        }
                    }

                    I += 1;
                }
            }

            return Result;
        }

        public sealed class SFXSequencesAccessor(Aow.Units.Sound.TSFXLibrary target)
        {
            public Aow.Units.Sound.TSFXSequence this[int Index] { get => target.GetSFXSequence(Index); }
        }

        public SFXSequencesAccessor SFXSequences => new SFXSequencesAccessor(this);

        public new partial class Meta : Aow.Units.Engine.TENode.Meta
        {
            // Sound.pas:1083
            // { TSFXLibrary }
            public override int ClassID() => Aow.Units.Sound.TSFXLibrary.ClassID;

            public static new Meta Of<T>()
                where T : Aow.Units.Sound.TSFXLibrary => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:631
            public new Aow.Units.Sound.TSFXLibrary Create(Aow.Units.Engine.TECustomNode AOwner) => (Aow.Units.Sound.TSFXLibrary)base.Create(AOwner);
        }
    }

    // { Sound.TSampleList (instance size 16): every TSample (ProcessSound updates them) }
    public partial class TSampleList : Aow.Rtl.TList
    {
        // Sound.pas:710
        // { TSampleList }
        public override void Destroy()
        {
            Clear();
            base.Destroy();
        }

        // Sound.pas:716
        public new void Clear()
        {
            int I = default;
            {
                I = Count - 1;
                byte end__81 = 0;
                for (; I >= end__81; I -= 1)
                {
                    ((Aow.Units.Sound.TSample)List[I]).Destroy();
                }
            }

            base.Clear();
        }

    }

    // { Sound.TSampleDataList (instance size 16): the loaded TSampleData }
    public partial class TSampleDataList : Aow.Rtl.TList
    {
        // Sound.pas:659
        // { TSampleDataList }
        public Aow.Units.Sound.TSampleData Find(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.Sound.TSampleData Result = default;
            int I = default;
            I = 0;
            while ((I != Count) && (Aow.Rtl.Rtl.CompareText(Name, ((Aow.Units.Sound.TSampleData)List[I]).FName) != 0))
            {
                I += 1;
            }

            if (I != Count)
            {
                Result = (Aow.Units.Sound.TSampleData)List[I];
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
    public partial class TENodeAccess : Aow.Units.Engine.TECustomNode
    {
        internal Aow.Units.Engine.TEObjectList FChildren;
    }

    // { Sound.TSoundSystemList (instance size 16): the registered sound system classes }
    public partial class TSoundSystemList : Aow.Rtl.TList
    {
        // Sound.pas:1293
        // { TSoundSystemList }
        public Aow.Units.Sound.SoundSystemClass Find(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.Sound.SoundSystemClass Result = default;
            int I = default;
            I = 0;
            while (true)
            {
                bool bool__82 = (I != Count);
                if (bool__82)
                {
                    var call__83 = ((Aow.Units.Sound.SoundSystemClass)List[I]).Name;
                    bool__82 = (Aow.Rtl.Rtl.CompareText(Name, call__83) != 0);
                }

                if (!bool__82)
                    break;
                {
                    I += 1;
                }
            }

            if (I != Count)
            {
                Result = (Aow.Units.Sound.SoundSystemClass)List[I];
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
        public static Aow.Rtl.AnsiString BaseSoundEnginePath;
        public static Aow.Units.Sound.TSongPlayer SongPlayer;
        // Sound.pas:1182
        // { Unit routines }
        // { Loads the sound data (or adds a reference) so that it stays loaded between samples. }
        public static void LockSampleData(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.Sound.TSampleData D = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                D = Aow.Units.Sound.Sound.SampleDataList.Find(Name);
                if (D == null)
                {
                    var call__51 = Aow.Units.Sound.Sound.SoundSystem.SampleDataClass();
                    call__51.Create(Name);
                }
                else
                {
                    D.AddRef();
                }
            }
        }

        // Sound.pas:1196
        public static void UnlockSampleData(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.Sound.TSampleData D = default;
            D = Aow.Units.Sound.Sound.SampleDataList.Find(Name);
            if (D != null)
            {
                D.Release();
            }
        }

        // Sound.pas:1205
        public static Aow.Units.Sound.TSampleData CreateSampleData(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.Sound.TSampleData Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                var call__52 = Aow.Units.Sound.Sound.SoundSystem.SampleDataClass();
                var call__53 = call__52.Create(Name);
                Result = call__53;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1213
        public static Aow.Units.Sound.TSampleData CreateSampleDataEx(ReadOnlySpan<Aow.Rtl.TWaveFormatEx> Format, object Data, int Size)
        {
            Aow.Units.Sound.TSampleData Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                var call__54 = Aow.Units.Sound.Sound.SoundSystem.SampleDataClass();
                var call__55 = call__54.CreateEx(Format[0], Data, Size);
                Result = call__55;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1221
        public static Aow.Units.Sound.TSample CreateSample(Aow.Units.Sound.TSampleData SampleData)
        {
            Aow.Units.Sound.TSample Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                var call__56 = Aow.Units.Sound.Sound.SoundSystem.SampleClass();
                var call__57 = call__56.Create(SampleData);
                Result = call__57;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1229
        public static Aow.Units.Sound.TSample PlaySample(Aow.Rtl.AnsiString Name)
        {
            Aow.Units.Sound.TSample Result = default;
            Aow.Units.Sound.TSampleData D = default;
            bool bool__58 = (Aow.Units.Sound.Sound.SoundSystem != null);
            if (bool__58)
            {
                var call__59 = Aow.Units.Sound.Sound.SoundSystem.SFXEnabled;
                bool__58 = call__59;
            }

            if (bool__58)
            {
                try
                {
                    D = Aow.Units.Sound.Sound.SampleDataList.Find(Name);
                    if (D == null)
                    {
                        var call__60 = Aow.Units.Sound.Sound.SoundSystem.SampleDataClass();
                        var call__61 = call__60.Create(Name);
                        D = call__61;
                        var call__62 = Aow.Units.Sound.Sound.SoundSystem.SampleClass();
                        var call__63 = call__62.Create(D);
                        Result = call__63;
                        D.Release();
                    }
                    else
                    {
                        var call__64 = Aow.Units.Sound.Sound.SoundSystem.SampleClass();
                        var call__65 = call__64.Create(D);
                        Result = call__65;
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
        public static Aow.Units.Sound.TSample PlaySampleEx(Aow.Rtl.AnsiString Name, int Volume, bool Looped, int Delay, Aow.Units.Sound.TSamplePanning Panning, Aow.Units.Sound.TSampleCallBack CallBack)
        {
            Aow.Units.Sound.TSample Result = default;
            Aow.Units.Sound.TSampleData D = default;
            bool bool__66 = (Aow.Units.Sound.Sound.SoundSystem != null);
            if (bool__66)
            {
                var call__67 = Aow.Units.Sound.Sound.SoundSystem.SFXEnabled;
                bool__66 = call__67;
            }

            if (bool__66)
            {
                try
                {
                    D = Aow.Units.Sound.Sound.SampleDataList.Find(Name);
                    if (D == null)
                    {
                        var call__68 = Aow.Units.Sound.Sound.SoundSystem.SampleDataClass();
                        var call__69 = call__68.Create(Name);
                        D = call__69;
                        var call__70 = Aow.Units.Sound.Sound.SoundSystem.SampleClass();
                        var call__71 = call__70.Create(D);
                        Result = call__71;
                        D.Release();
                    }
                    else
                    {
                        var call__72 = Aow.Units.Sound.Sound.SoundSystem.SampleClass();
                        var call__73 = call__72.Create(D);
                        Result = call__73;
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
        public static int SamplesActive()
        {
            int Result = default;
            Result = Aow.Units.Sound.Sound.SampleList.Count;
            return Result;
        }

        // Sound.pas:1286
        public static int SamplesLoaded()
        {
            int Result = default;
            Result = Aow.Units.Sound.Sound.SampleDataList.Count;
            return Result;
        }

        // Sound.pas:1306
        public static void RegisterSoundSystem(Aow.Units.Sound.SoundSystemClass AClass)
        {
            if (Aow.Units.Sound.Sound.SoundSystems == null)
            {
                Aow.Units.Sound.Sound.SoundSystems = new Aow.Units.Sound.TSoundSystemList();
            }

            var call__74 = AClass.Name;
            var call__75 = Aow.Units.Sound.Sound.SoundSystems.Find(call__74);
            if (call__75 == null)
            {
                Aow.Units.Sound.Sound.SoundSystems.Add(AClass);
            }
            else
            {
                var call__76 = AClass.Name;
                var call__77 = Aow.Rtl.Exception.Create((((AnsiString)"Sound System with name: ") + call__76) + ((AnsiString)" already registered"));
                throw call__77;
            }
        }

        // Sound.pas:1317
        // { Creates and activates the named sound system (the first registered one when Name is empty). }
        public static bool InitializeSoundSystem(Aow.Rtl.AnsiString Name, in Aow.Units.Sound.TSoundSettings Settings)
        {
            bool Result = default;
            Aow.Units.Sound.SoundSystemClass C = default;
            Result = true;
            if (Name == ((AnsiString)""))
            {
                if (Aow.Units.Sound.Sound.SoundSystems.Count != 0)
                {
                    var call__78 = Aow.Units.Sound.Sound.SoundSystems[0];
                    C = (Aow.Units.Sound.SoundSystemClass)call__78;
                }
                else
                {
                    C = null;
                }
            }
            else
            {
                C = Aow.Units.Sound.Sound.SoundSystems.Find(Name);
            }

            if (C != null)
            {
                if (Aow.Units.Sound.Sound.SoundSystem != null)
                {
                    Aow.Units.Sound.Sound.ReleaseSoundSystem();
                }

                Aow.Units.Sound.Sound.SoundSystem = C.Create();
                if (!Aow.Units.Sound.Sound.SoundSystem.Activate(Settings))
                {
                    Aow.Units.Sound.Sound.SoundSystem.Free();
                    Aow.Units.Sound.Sound.SoundSystem = null;
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
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                if (Aow.Units.Sound.Sound.SongPlayer != null)
                {
                    Aow.Units.Sound.Sound.SongPlayer.Stop(true);
                }

                if (Aow.Units.Sound.Sound.SampleList != null)
                {
                    Aow.Units.Sound.Sound.SampleList.Clear();
                }

                Aow.Units.Sound.Sound.SoundSystem.Release();
                Aow.Units.Sound.Sound.SoundSystem = null;
            }
        }

        // Sound.pas:1360
        public static TSet<Aow.Units.Sound.TSoundSystemStatusElement> SoundSystemStatus()
        {
            TSet<Aow.Units.Sound.TSoundSystemStatusElement> Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.FStatus;
            }
            else
            {
                Result = TSet<Aow.Units.Sound.TSoundSystemStatusElement>.Of();
            }

            return Result;
        }

        // Sound.pas:1368
        public static void SetMusicVolume(int Volume)
        {
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Aow.Units.Sound.Sound.SoundSystem.SetMusicVolume(Volume);
            }
        }

        // Sound.pas:1374
        public static void FadeMusicVolume(int Volume, int Speed)
        {
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Aow.Units.Sound.Sound.SoundSystem.FadeMusicVolume(Volume, Speed);
            }
        }

        // Sound.pas:1380
        public static int GetMusicVolume()
        {
            int Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.GetMusicVolume();
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
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Aow.Units.Sound.Sound.SoundSystem.SetSFXVolume(Volume);
            }
        }

        // Sound.pas:1394
        public static int GetSFXVolume()
        {
            int Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.GetSFXVolume();
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
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.GetMusicEnabled();
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
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.SetMusicEnabled(Value);
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
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.GetSFXEnabled();
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
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.SetSFXEnabled(Value);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1434
        public static Aow.Units.Sound.TSoundSettings GetSoundSettings()
        {
            Aow.Units.Sound.TSoundSettings Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.GetSoundSettings();
            }
            else
            {
                Result = default;
            }

            return Result;
        }

        // Sound.pas:1442
        public static bool SetSoundSettings(in Aow.Units.Sound.TSoundSettings Settings)
        {
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.SetSoundSettings(Settings);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1450
        public static Aow.Units.Sound.TSongStatus BackupSongStatus()
        {
            Aow.Units.Sound.TSongStatus Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.BackupSongStatus();
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // Sound.pas:1458
        public static bool RestoreSongStatus(Aow.Units.Sound.TSongStatus Status)
        {
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.RestoreSongStatus(Status);
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
        public static Aow.Rtl.AnsiString GetSongName(Aow.Rtl.AnsiString FileName)
        {
            Aow.Rtl.AnsiString Result = default;
            Aow.Rtl.TFileStream Stream = default;
            Aow.Rtl.AnsiString Ext = default;
            Aow.Units.FixedArray_byte_28 Title = default;
            int P = default;
            var call__79 = new Aow.Rtl.TFileStream(Aow.Units.Sound.Sound.BaseSoundEnginePath + FileName, 0 | 32);
            Stream = call__79;
            try
            {
                Ext = Aow.Rtl.Rtl.UpperCase(Aow.Rtl.Rtl.ExtractFileExt(FileName));
                Title = default;
                if (Ext == ((AnsiString)".IT"))
                {
                    Stream.Seek(4, 0);
                    Stream.Read(((Span<byte>)Title).Slice(0, 28));
                    Result = Aow.Rtl.Rtl.StrPasBuffer((ReadOnlySpan<byte>)Title);
                }
                else
                {
                    if (Ext == ((AnsiString)".S3M"))
                    {
                        Stream.Read(((Span<byte>)Title).Slice(0, 28));
                        Result = Aow.Rtl.Rtl.StrPasBuffer((ReadOnlySpan<byte>)Title);
                    }
                    else
                    {
                        Result = Aow.Rtl.Rtl.ExtractFileName(FileName);
                        P = Aow.Rtl.Rtl.Pos(AnsiString.FromBytes(46), Result);
                        if (P != 0)
                        {
                            Result = Aow.Rtl.Rtl.Copy(Result, 1, P - 1);
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
        public static Aow.Rtl.AnsiString PlayingSongFileName()
        {
            Aow.Rtl.AnsiString Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.GetPlayingSongFileName();
            }
            else
            {
                Result = (AnsiString)"";
            }

            return Result;
        }

        // Sound.pas:1510
        public static bool PlaySong(Aow.Rtl.AnsiString FileName)
        {
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.PlaySong(FileName);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // Sound.pas:1518
        public static bool PlaySongEx(Aow.Rtl.AnsiString FileName, int FadeOut, int FadeIn, bool Loop)
        {
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.PlaySongEx(FileName, FadeOut, FadeIn, Loop);
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
            bool Result = default;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Result = Aow.Units.Sound.Sound.SoundSystem.StopSong();
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
            int I = default;
            if (Aow.Units.Sound.Sound.SampleList != null)
            {
                {
                    I = Aow.Units.Sound.Sound.SampleList.Count - 1;
                    byte end__80 = 0;
                    for (; I >= end__80; I -= 1)
                    {
                        ((Aow.Units.Sound.TSample)Aow.Units.Sound.Sound.SampleList.List[I]).Update();
                    }
                }
            }

            if (Aow.Units.Sound.Sound.SongPlayer != null)
            {
                Aow.Units.Sound.Sound.SongPlayer.Update();
            }

            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Aow.Units.Sound.Sound.SoundSystem.Process();
            }
        }

        // Sound.pas:1547
        public static void RegisterSoundEngineClasses(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.RegisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Sound.TSFXSequence>(), Aow.Units.Engine.EClass.Of<Aow.Units.Sound.TSFXLibrary>(), Aow.Units.Engine.EClass.Of<Aow.Units.Sound.TSFX>()]);
        }

        // Sound.pas:1552
        public static void UnregisterSoundEngineClasses(Aow.Units.Engine.TEngine AEngine)
        {
            AEngine.UnregisterEClasses([Aow.Units.Engine.EClass.Of<Aow.Units.Sound.TSFXSequence>(), Aow.Units.Engine.EClass.Of<Aow.Units.Sound.TSFXLibrary>(), Aow.Units.Engine.EClass.Of<Aow.Units.Sound.TSFX>()]);
        }

        // Sound.pas:1557
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"Sound", [typeof(Aow.Units.Sound.TSFXManager), typeof(Aow.Units.Sound.TSFXManagerComboBox), typeof(Aow.Units.Sound.TSFXManagerListBox)]);
        }

        public static readonly Aow.Rtl.AnsiString SFXExtension = (AnsiString)"WAV";
        public static Aow.Units.Sound.TSoundSystem SoundSystem = default;
        public static Aow.Units.Sound.TSoundSystemList SoundSystems = default;
        public static Aow.Units.Sound.TSampleDataList SampleDataList = default;
        public static Aow.Units.Sound.TSampleList SampleList = default;
        public static void Initialize()
        {
            if (Aow.Rtl.Rtl.ToolServices != null)
            {
                var call__84 = Aow.Rtl.Rtl.ToolServices.GetProjectName();
                var call__85 = Aow.Rtl.Rtl.ExtractFilePath(call__84);
                Aow.Units.Sound.Sound.BaseSoundEnginePath = call__85;
            }
            else
            {
                var call__86 = Aow.Rtl.Rtl.Application.ExeName;
                var call__87 = Aow.Rtl.Rtl.ExtractFilePath(call__86);
                Aow.Units.Sound.Sound.BaseSoundEnginePath = call__87;
            }

            Aow.Units.Sound.Sound.SampleList = new Aow.Units.Sound.TSampleList();
            Aow.Units.Sound.Sound.SampleDataList = new Aow.Units.Sound.TSampleDataList();
            if (Aow.Units.Sound.Sound.SoundSystems == null)
            {
                Aow.Units.Sound.Sound.SoundSystems = new Aow.Units.Sound.TSoundSystemList();
            }

            var call__88 = new Aow.Units.Sound.TSongPlayer();
            Aow.Units.Sound.Sound.SongPlayer = call__88;
        }

        public static void Finalize()
        {
            Aow.Units.Sound.Sound.SongPlayer.Free();
            Aow.Units.Sound.Sound.SongPlayer = null;
            Aow.Units.Sound.Sound.SampleList.Free();
            Aow.Units.Sound.Sound.SampleList = null;
            if (Aow.Units.Sound.Sound.SampleDataList.Count != 0)
            {
                Aow.Rtl.Rtl.ShowMessage((AnsiString)"Inproper use of Sound Engine (SoundData still loaded)!");
            }

            Aow.Units.Sound.Sound.SampleDataList.Free();
            Aow.Units.Sound.Sound.SampleDataList = null;
            if (Aow.Units.Sound.Sound.SoundSystem != null)
            {
                Aow.Units.Sound.Sound.SoundSystem.Release();
            }

            Aow.Units.Sound.Sound.SoundSystems.Free();
        }
    }
}
