#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Audio;
using Aow.Game.Combat;

using Aow.Game.World.Map;
using Aow.Platform;

namespace Aow.Game.Tests;

public sealed class MapNetSoundFailureTests
{
    private sealed class SoundOwner : EngineFixture.Owner
    {

        internal List<string> Events { get; } = [];

        public override int AddChild(EngineObject child)
        {
            Events.Add("insert");
            _ = base.AddChild(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
        {
            Events.Add("remove");
            int index = Members.IndexOf(child);
            _ = base.RemoveChild(child);
            return index;
        }
    }

    private sealed class FailingSample : Sample
    {
        private FailingSample(ConstructionContext context, SampleData data) : base(context, sampleData: data)
        {
            Assert.Equal(2, data.RefCount);
            Assert.Equal(1, AudioHost.SampleList.Count);
            throw new InvalidDataException("sample");
        }

        internal static new FailingSample Create(SampleData data)
            => ConstructionContext.Create(data, static (context, value) => new FailingSample(context, data: value));
    }

    private sealed class Container(GameMap map) : MapContainer(map)
    {
        private readonly List<string> _events = [];

        public override int AddChild(EngineObject child)
        {
            _events.Add("insert");
            return base.AddChild(child);
        }

        public override int RemoveChild(EngineObject child)
        {
            _events.Add("remove");
            return base.RemoveChild(child);
        }

        public override void Clear()
        {
            _events.Add($"clear:{Count}");
            base.Clear();
        }
    }

    private sealed class ObservingFiles(IFileSystem inner, string name) : IFileSystem
    {
        internal SampleData? Published { get; private set; }

        public Stream Open(string path, FileOpenMode mode)
        {
            Published = AudioHost.SampleDataList.Find(name);
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

    [Fact]
    public void PlainDynamicLevelRemainsPublishedUntilDestroy()
    {
        SoundOwner owner = new();
        // A level takes a container as its owner. This one passes membership on to the recorder.
        EngineFixture.ContainerOwner container = new(owner);
        MapLevel level = new(container);
        Assert.Same(level, Assert.Single(owner.Members));
        Assert.Same(container, level.Owner);
        Assert.Equal(0, level.Count);
        Assert.Equal(["insert"], owner.Events);
        level.Dispose();
        Assert.Empty(owner.Members);
        Assert.Equal(["insert", "remove"], owner.Events);
    }

    [Fact]
    public void PlainWorldAndCombatLevelsKeepTheRegisteredTypeAndOwner()
    {
        foreach (EngineClass? descriptor in new[] { EngineClass.Of<MapLevel>(), EngineClass.Of<CombatMapLevel>() })
        {
            SoundOwner owner = new();
            EngineFixture.ContainerOwner container = new(owner);
            MapLevel level = Assert.IsAssignableFrom<MapLevel>(descriptor.CreateInstance(container, holder: EngineFixture.Engine));
            Assert.IsType(descriptor.Type, @object: level);
            Assert.Same(level, Assert.Single(owner.Members));
            Assert.Same(container, level.Owner);
            Assert.Equal(0, level.Count);
            level.Dispose();
            Assert.Empty(owner.Members);
            Assert.Equal(["insert", "remove"], owner.Events);
        }
    }

    [Fact]
    public void FailedSampleRestoresMembershipAndDataReference()
    {
        SampleList oldSamples = AudioHost.SampleList;
        SampleDataList oldData = AudioHost.SampleDataList;
        AudioHost.SampleList = new SampleList();
        AudioHost.SampleDataList = new SampleDataList();
        try
        {
            SampleData data = SampleData.Create("reference");
            Assert.Equal(1, data.RefCount);
            _ = Assert.Throws<InvalidDataException>(() => new SampleClass("FailingSample", FailingSample.Create).CreateInstance(data));
            Assert.Empty(AudioHost.SampleList.List.Take(AudioHost.SampleList.Count));
            Assert.Equal(1, data.RefCount);
            Assert.Same(data, AudioHost.SampleDataList.Find("reference"));
            data.Release();
            Assert.Equal(0, AudioHost.SampleDataList.Count);
        }
        finally
        {
            AudioHost.SampleList = oldSamples;
            AudioHost.SampleDataList = oldData;
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingAndCorruptSdlWavesLeaveNoCacheEntryOnRetry(bool corrupt)
    {
        SampleList oldSamples = AudioHost.SampleList;
        SampleDataList oldData = AudioHost.SampleDataList;
        SoundSystemList oldSystems = AudioHost.SoundSystems;
        string oldPath = AudioHost.BaseSoundEnginePath;
        IFileSystem oldFiles = RtlHost.Files;
        AudioHost.SampleList = new SampleList();
        AudioHost.SampleDataList = new SampleDataList();
        AudioHost.SoundSystems = new SoundSystemList();
        string path = Path.Combine(AppContext.BaseDirectory, "9.3b-8-corrupt.wav");
        if (corrupt)
        {
            File.WriteAllBytes(path, bytes: [1, 2, 3]);
        }
        else
        {
            path += ".missing";
        }

        AudioHost.BaseSoundEnginePath = "";
        ObservingFiles files = new(oldFiles, name: path);
        RtlHost.Files = files;
        try
        {
            SdlSoundSystem.Install(mute: true, audioOutput: false);
            SampleDataClass meta = SdlSoundSystem.SampleData;
            SampleData? previous = null;
            for (int i = 0; i < 2; i++)
            {
                Exception error = Record.Exception(() => meta.CreateInstance(path));
                if (corrupt)
                {
                    _ = Assert.IsType<InvalidDataException>(error);
                    Assert.Equal("Expected RIFF WAVE data.", error.Message);
                }
                else
                {
                    _ = Assert.IsType<FileNotFoundException>(error);
                }

                Assert.Null(AudioHost.SampleDataList.Find(path));
                Assert.Equal(0, AudioHost.SampleDataList.Count);
                Assert.Equal(0, AudioHost.SampleList.Count);
                Assert.NotNull(files.Published);
                Assert.Equal(1, files.Published.RefCount);
                Assert.NotSame(previous, files.Published);
                previous = files.Published;
            }
        }
        finally
        {
            if (corrupt)
            {
                File.Delete(path);
            }

            AudioHost.SampleList = oldSamples;
            AudioHost.SampleDataList = oldData;
            AudioHost.SoundSystems = oldSystems;
            AudioHost.BaseSoundEnginePath = oldPath;
            RtlHost.Files = oldFiles;
        }
    }
}
