using Aow.Game.Audio;
using Aow.Game.Engine;
using Aow.Game.Engine.Nodes;
using Aow.Game.World.Map;
using Aow.Platform;

namespace Aow.Units.Tests;

public sealed class MapNetSoundFailureTests
{
    private sealed class SoundOwner : EngineFixture.Owner
    {

        internal readonly List<string> Events = [];

        public override int AddChild(TEObject child)
        {
            Events.Add("insert");
            base.AddChild(child);
            return Members.Count - 1;
        }

        public override int RemoveChild(TEObject child)
        {
            Events.Add("remove");
            int index = Members.IndexOf(child);
            base.RemoveChild(child);
            return index;
        }
    }

    private sealed class FailingSample : Aow.Game.Audio.TSample
    {
        private FailingSample(ConstructionContext context, TSampleData data) : base(context, data)
        {
            Assert.Equal(2, data.RefCount);
            Assert.Equal(1, Aow.Game.Audio.AudioHost.SampleList.Count);
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

    private sealed class ObservingFiles(IFileSystem inner, string name) : IFileSystem
    {
        internal TSampleData? Published;

        public Stream Open(string path, int mode)
        {
            Published = Aow.Game.Audio.AudioHost.SampleDataList.Find(name);
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
        var level = new TMapLevel(owner);
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
        foreach (var descriptor in new[] { EClass.Of<Aow.Game.World.Map.TAoWMapLevel>(), EClass.Of<Aow.Game.Combat.TAoWCombatMapLevel>() })
        {
            var owner = new SoundOwner();
            var level = Assert.IsAssignableFrom<TMapLevel>(descriptor.Create(owner));
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
        var oldSamples = Aow.Game.Audio.AudioHost.SampleList;
        var oldData = Aow.Game.Audio.AudioHost.SampleDataList;
        Aow.Game.Audio.AudioHost.SampleList = new TSampleList();
        Aow.Game.Audio.AudioHost.SampleDataList = new TSampleDataList();
        try
        {
            var data = TSampleData.Create("reference");
            Assert.Equal(1, data.RefCount);
            Assert.Throws<InvalidDataException>(() => new SampleClass("FailingSample", FailingSample.Create).Create(data));
            Assert.Empty(Aow.Game.Audio.AudioHost.SampleList.List.Take(Aow.Game.Audio.AudioHost.SampleList.Count));
            Assert.Equal(1, data.RefCount);
            Assert.Same(data, Aow.Game.Audio.AudioHost.SampleDataList.Find("reference"));
            data.Release();
            Assert.Equal(0, Aow.Game.Audio.AudioHost.SampleDataList.Count);
        }
        finally { Aow.Game.Audio.AudioHost.SampleList = oldSamples; Aow.Game.Audio.AudioHost.SampleDataList = oldData; }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingAndCorruptSdlWavesLeaveNoCacheEntryOnRetry(bool corrupt)
    {
        var oldSamples = Aow.Game.Audio.AudioHost.SampleList;
        var oldData = Aow.Game.Audio.AudioHost.SampleDataList;
        var oldSystems = Aow.Game.Audio.AudioHost.SoundSystems;
        var oldPath = Aow.Game.Audio.AudioHost.BaseSoundEnginePath;
        var oldFiles = Runtime.Files;
        Aow.Game.Audio.AudioHost.SampleList = new TSampleList();
        Aow.Game.Audio.AudioHost.SampleDataList = new TSampleDataList();
        Aow.Game.Audio.AudioHost.SoundSystems = new TSoundSystemList();
        string path = Path.Combine(AppContext.BaseDirectory, "9.3b-8-corrupt.wav");
        if (corrupt) File.WriteAllBytes(path, [1, 2, 3]);
        else path += ".missing";
        Aow.Game.Audio.AudioHost.BaseSoundEnginePath = "";
        var files = new ObservingFiles(oldFiles, path);
        Runtime.Files = files;
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
                Assert.Null(Aow.Game.Audio.AudioHost.SampleDataList.Find(path));
                Assert.Equal(0, Aow.Game.Audio.AudioHost.SampleDataList.Count);
                Assert.Equal(0, Aow.Game.Audio.AudioHost.SampleList.Count);
                Assert.NotNull(files.Published);
                Assert.Equal(1, files.Published.RefCount);
                Assert.NotSame(previous, files.Published);
                previous = files.Published;
            }
        }
        finally
        {
            if (corrupt) File.Delete(path);
            Aow.Game.Audio.AudioHost.SampleList = oldSamples;
            Aow.Game.Audio.AudioHost.SampleDataList = oldData;
            Aow.Game.Audio.AudioHost.SoundSystems = oldSystems;
            Aow.Game.Audio.AudioHost.BaseSoundEnginePath = oldPath;
            Runtime.Files = oldFiles;
        }
    }
}
