using Aow.Platform;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.Sound;
using SoundApi = Aow.Units.Sound.Sound;

namespace Aow.Rtl.Tests;

public sealed class MapNetSoundFailureTests
{
    private sealed class Owner : TECustomNode
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<string> Events = [];
        public override int AddChild(TEObject child)
        {
            Events.Add("insert");
            Members.Add(child);
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove");
            int index = Members.IndexOf(child);
            Members.Remove(child);
            return index;
        }
    }

    private sealed class FailingLevel : TMapLevel
    {
        private FailingLevel(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            ((Owner)owner).Events.Add("fail");
            throw new InvalidDataException("level");
        }
        internal static new FailingLevel Create(TECustomNode owner) =>
            ConstructionContext.Create(owner, static (context, value) => new FailingLevel(context, value));
        public override void Destroy()
        {
            ((Owner)Owner).Events.Add("destroy");
            base.Destroy();
        }
    }

    private sealed class FailingSample : Aow.Units.Sound.TSample
    {
        private FailingSample(ConstructionContext context, TSampleData data) : base(context, data)
        {
            Assert.Equal(2, data.RefCount);
            Assert.Equal(1, SoundApi.SampleList.Count);
            throw new InvalidDataException("sample");
        }
        internal static new FailingSample Create(TSampleData data) =>
            ConstructionContext.Create(data, static (context, value) => new FailingSample(context, value));
    }

    private sealed class Container(THSMap map) : TMapContainer(map)
    {
        internal readonly List<string> Events = [];
        public override int AddChild(TEObject child)
        {
            Events.Add("insert");
            return base.AddChild(child);
        }
        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove");
            return base.RemoveChild(child);
        }
        public override void Clear()
        {
            Events.Add("clear:" + Count);
            base.Clear();
        }
    }

    private sealed class FailingPublishedLevel : TMapLevel
    {
        private FailingPublishedLevel(ConstructionContext context, TECustomNode owner) : base(context, owner)
        {
            ((Container)owner).Events.Add("fail");
            throw new InvalidDataException("published level");
        }
        internal static new FailingPublishedLevel Create(TECustomNode owner) =>
            ConstructionContext.Create(owner, static (context, value) => new FailingPublishedLevel(context, value));
        public override void Destroy()
        {
            var container = (Container)Owner;
            container.Events.Add("destroy:" + container.Count);
            base.Destroy();
        }
    }

    private sealed class ObservingFiles(IFileSystem inner, AnsiString name) : IFileSystem
    {
        internal TSampleData? Published;
        public Stream Open(string path, int mode)
        {
            Published = SoundApi.SampleDataList.Find(name);
            Assert.NotNull(Published);
            Assert.Equal(1, Published.RefCount);
            return inner.Open(path, mode);
        }
        public string CurrentDirectory => inner.CurrentDirectory;
        public string ExpandPath(string path) => inner.ExpandPath(path);
        public bool FileExists(string path) => inner.FileExists(path);
        public bool DirectoryExists(string path) => inner.DirectoryExists(path);
        public bool DeleteFile(string path) => inner.DeleteFile(path);
        public bool RenameFile(string source, string target) => inner.RenameFile(source, target);
        public bool CreateDirectory(string path) => inner.CreateDirectory(path);
        public DateTime? LastWriteTime(string path) => inner.LastWriteTime(path);
        public IEnumerable<RtlFileEntry> Enumerate(string directory) => inner.Enumerate(directory);
    }

    static MapNetSoundFailureTests()
    {
        EClass.Register(typeof(FailingLevel), TMapLevel.ClassID, static owner => FailingLevel.Create(owner), 124);
        MetaRegistry.Register(typeof(FailingLevel), new TMapLevel.Meta(), 124);
        EClass.Register(typeof(FailingPublishedLevel), TMapLevel.ClassID, static owner => FailingPublishedLevel.Create(owner), 124);
        MetaRegistry.Register(typeof(FailingPublishedLevel), new TMapLevel.Meta(), 124);
    }

    [Fact]
    public void FailedDynamicLevelDetachesBeforeCallerCleanup()
    {
        var owner = new Owner();
        var error = Assert.Throws<InvalidDataException>(() => TMapLevel.Meta.Of<FailingLevel>().Create(owner));
        Assert.Equal("level", error.Message);
        Assert.Empty(owner.Members);
        Assert.Equal(["insert", "fail", "destroy", "remove"], owner.Events);
    }

    [Fact]
    public void ContainerCatchSeesTheFailedLevelAlreadyDetached()
    {
        if (Aow.Units.Engine.Engine.Engines is null) Aow.Units.Engine.Engine.Initialize();
        var engine = new TEngine(null!);
        engine.Options -= TSet<TEngineModeElement>.Of(Aow.Units.Engine.Engine.eoVerifyClasses);
        engine.RegisterEClasses([EClass.Of<FailingPublishedLevel>()]);
        var map = new THSMap(null!);
        var container = new Container(map) { FEngine = engine };
        try
        {
            var error = Assert.Throws<InvalidDataException>(() => container.New(new TNewHSMapSettings()));
            Assert.Equal("published level", error.Message);
            Assert.Equal(0, container.Count);
            Assert.Equal(["clear:0", "insert", "fail", "destroy:1", "remove", "clear:0"], container.Events);
        }
        finally { container.Destroy(); map.Destroy(); engine.Destroy(); }
    }

    [Fact]
    public void FailedSampleRestoresMembershipAndDataReference()
    {
        var oldSamples = SoundApi.SampleList;
        var oldData = SoundApi.SampleDataList;
        SoundApi.SampleList = new TSampleList();
        SoundApi.SampleDataList = new TSampleDataList();
        try
        {
            var data = TSampleData.Create("reference");
            Assert.Equal(1, data.RefCount);
            Assert.Throws<InvalidDataException>(() => new SampleClass(FailingSample.Create).Create(data));
            Assert.Empty(SoundApi.SampleList.List.Take(SoundApi.SampleList.Count));
            Assert.Equal(1, data.RefCount);
            Assert.Same(data, SoundApi.SampleDataList.Find("reference"));
            data.Release();
            Assert.Equal(0, SoundApi.SampleDataList.Count);
        }
        finally { SoundApi.SampleList = oldSamples; SoundApi.SampleDataList = oldData; }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingAndCorruptSdlWavesLeaveNoCacheEntryOnRetry(bool corrupt)
    {
        var oldSamples = SoundApi.SampleList;
        var oldData = SoundApi.SampleDataList;
        var oldSystems = SoundApi.SoundSystems;
        var oldPath = SoundApi.BaseSoundEnginePath;
        var oldFiles = Rtl.Files;
        SoundApi.SampleList = new TSampleList();
        SoundApi.SampleDataList = new TSampleDataList();
        SoundApi.SoundSystems = new TSoundSystemList();
        string path = Path.Combine(AppContext.BaseDirectory, "9.3b-8-corrupt.wav");
        if (corrupt) File.WriteAllBytes(path, [1, 2, 3]);
        else path += ".missing";
        SoundApi.BaseSoundEnginePath = "";
        var files = new ObservingFiles(oldFiles, path);
        Rtl.Files = files;
        try
        {
            TSdlSoundSystem.Install(mute: true, audioOutput: false);
            var meta = TSdlSoundSystem.SampleData;
            TSampleData? previous = null;
            for (int i = 0; i < 2; i++)
            {
                var error = Record.Exception(() => meta.Create(path));
                if (corrupt)
                {
                    Assert.IsType<InvalidDataException>(error);
                    Assert.Equal("Expected RIFF WAVE data.", error.Message);
                }
                else Assert.IsType<FileNotFoundException>(error);
                Assert.Null(SoundApi.SampleDataList.Find(path));
                Assert.Equal(0, SoundApi.SampleDataList.Count);
                Assert.Equal(0, SoundApi.SampleList.Count);
                Assert.NotNull(files.Published);
                Assert.Equal(1, files.Published.RefCount);
                Assert.NotSame(previous, files.Published);
                previous = files.Published;
            }
        }
        finally
        {
            if (corrupt) File.Delete(path);
            SoundApi.SampleList = oldSamples;
            SoundApi.SampleDataList = oldData;
            SoundApi.SoundSystems = oldSystems;
            SoundApi.BaseSoundEnginePath = oldPath;
            Rtl.Files = oldFiles;
        }
    }
}
