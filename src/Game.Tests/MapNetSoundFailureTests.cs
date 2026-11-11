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

        internal readonly List<string> Events = [];

        public override int AddChild(EngineObject child)
        {
            Events.Add("insert");
            base.AddChild(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(EngineObject child)
        {
            Events.Add("remove");
            int index = Members.IndexOf(child);
            base.RemoveChild(child);
            return index;
        }
    }

    private sealed class FailingSample : Sample
    {
        private FailingSample(ConstructionContext context, SampleData data) : base(context, data)
        {
            Assert.Equal(2, data.RefCount);
            Assert.Equal(1, AudioHost.SampleList.Count);
            throw new InvalidDataException("sample");
        }

        internal static new FailingSample Create(SampleData data) =>
            ConstructionContext.Create(data, static (context, value) => new FailingSample(context, value));
    }

    private sealed class Container(HSMap map) : MapContainer(map)
    {
        internal readonly List<string> Events = [];

        public override int AddChild(EngineObject child)
        {
            Events.Add("insert");
            return base.AddChild(child);
        }

        public override int RemoveChild(EngineObject child)
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

    private sealed class ObservingFiles(IFileSystem inner, string name) : IFileSystem
    {
        internal SampleData? Published;

        public Stream Open(string path, int mode)
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
        var owner = new SoundOwner();
        var level = new MapLevel(owner);
        Assert.Same(level, Assert.Single(owner.Members));
        Assert.Same(owner, level.Owner);
        Assert.Equal(0, level.Count);
        Assert.Equal(["insert"], owner.Events);
        level.Dispose();
        Assert.Empty(owner.Members);
        Assert.Equal(["insert", "remove"], owner.Events);
    }

    [Fact]
    public void PlainWorldAndCombatLevelsKeepTheRegisteredTypeAndOwner()
    {
        foreach (var descriptor in new[] { EngineClass.Of<AoWMapLevel>(), EngineClass.Of<CombatMapLevel>() })
        {
            var owner = new SoundOwner();
            var level = Assert.IsAssignableFrom<MapLevel>(descriptor.Create(owner));
            Assert.IsType(descriptor.Type, level);
            Assert.Same(level, Assert.Single(owner.Members));
            Assert.Same(owner, level.Owner);
            Assert.Equal(0, level.Count);
            level.Dispose();
            Assert.Empty(owner.Members);
            Assert.Equal(["insert", "remove"], owner.Events);
        }
    }

    [Fact]
    public void FailedSampleRestoresMembershipAndDataReference()
    {
        var oldSamples = AudioHost.SampleList;
        var oldData = AudioHost.SampleDataList;
        AudioHost.SampleList = new SampleList();
        AudioHost.SampleDataList = new SampleDataList();
        try
        {
            var data = SampleData.Create("reference");
            Assert.Equal(1, data.RefCount);
            Assert.Throws<InvalidDataException>(() => new SampleClass("FailingSample", FailingSample.Create).Create(data));
            Assert.Empty(AudioHost.SampleList.List.Take(AudioHost.SampleList.Count));
            Assert.Equal(1, data.RefCount);
            Assert.Same(data, AudioHost.SampleDataList.Find("reference"));
            data.Release();
            Assert.Equal(0, AudioHost.SampleDataList.Count);
        }
        finally { AudioHost.SampleList = oldSamples; AudioHost.SampleDataList = oldData; }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingAndCorruptSdlWavesLeaveNoCacheEntryOnRetry(bool corrupt)
    {
        var oldSamples = AudioHost.SampleList;
        var oldData = AudioHost.SampleDataList;
        var oldSystems = AudioHost.SoundSystems;
        var oldPath = AudioHost.BaseSoundEnginePath;
        var oldFiles = Runtime.Files;
        AudioHost.SampleList = new SampleList();
        AudioHost.SampleDataList = new SampleDataList();
        AudioHost.SoundSystems = new SoundSystemList();
        string path = Path.Combine(AppContext.BaseDirectory, "9.3b-8-corrupt.wav");
        if (corrupt) File.WriteAllBytes(path, [1, 2, 3]);
        else path += ".missing";
        AudioHost.BaseSoundEnginePath = "";
        var files = new ObservingFiles(oldFiles, path);
        Runtime.Files = files;
        try
        {
            SdlSoundSystem.Install(mute: true, audioOutput: false);
            var meta = SdlSoundSystem.SampleData;
            SampleData? previous = null;
            for (int i = 0; i < 2; i++)
            {
                var error = Record.Exception(() => meta.Create(path));
                if (corrupt)
                {
                    Assert.IsType<InvalidDataException>(error);
                    Assert.Equal("Expected RIFF WAVE data.", error.Message);
                }
                else Assert.IsType<FileNotFoundException>(error);
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
            if (corrupt) File.Delete(path);
            AudioHost.SampleList = oldSamples;
            AudioHost.SampleDataList = oldData;
            AudioHost.SoundSystems = oldSystems;
            AudioHost.BaseSoundEnginePath = oldPath;
            Runtime.Files = oldFiles;
        }
    }
}
