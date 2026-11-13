using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;
using Aow.Game.Draw.Images;

using Aow.Game.World;
using Aow.Game.World.Map;

namespace Aow.Game.Tests;

[Collection("Engine")]
[EngineState]
public sealed class ControlStyleConstructionTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(2, true)]
    public void PlainCreationRoutesKeepTheExactTypeAndOwner(int kind, bool owned)
    {
        var engine = CreateEngine();
        var owner = new EngineFixture.Owner();
        ECustomNode argument = owned ? owner : null!;
        Type type = kind switch { 0 => typeof(ControlStyleObject), 1 => typeof(ResourceEditSettings), _ => typeof(PadControl) };
        var meta = EngineClass.Of(type);
        if (kind == 2)
        {
            // The pad has the inherited ID 81, so isolate its class entry in this test engine.
            engine.UnregisterEngineClasses([EngineClass.Of<ControlStyleObject>()]);
            engine.RegisterEngineClasses([EngineClass.Of(type)]);
        }
        var direct = kind switch
        {
            0 => new ControlStyleObject(argument),
            1 => new ResourceEditSettings(argument),
            _ => (ControlStyleObject)new PadControl(argument)
        };
        EngineObject[] objects = [direct, meta.CreateInstance(argument, holder: EngineFixture.Engine), engine.CreateEngineObject(meta.ClassId, argument)];
        EngineObject? copy = null;
        try
        {
            Assert.Equal(kind == 1 ? 1026 : 81, meta.ClassId);
            Assert.Same(meta, EngineClass.Of(type));
            foreach (var value in objects)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EngineClass);
            }
            if (direct is ResourceEditSettings settings)
                settings.Editors.AddObject("Editor", Runtime.IntToObject(17));
            direct.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared));
            copy = direct.Copy(engine);
            Assert.IsType(type, copy);
            Assert.Null(copy.Owner);
            Assert.Equal(direct.GetControlStyle(), copy.GetControlStyle());
            if (copy is ResourceEditSettings copied)
            {
                Assert.NotSame(((ResourceEditSettings)direct).Editors, copied.Editors);
                Assert.Equal("Editor", copied.Editors[0]);
                Assert.Equal(17, Runtime.ObjectToInt(copied.Editors.Objects[0]));
            }
            if (direct is PadControl pad)
            {
                Assert.NotNull(pad.TerrainTypes);
                Assert.NotNull(pad.PadResources);
            }
            Assert.Equal(owned ? 3 : 0, owner.Members.Count);
        }
        finally
        {
            copy?.Dispose();
            foreach (var value in objects) value.Dispose();
            engine.Dispose();
        }
        Assert.Empty(owner.Members);
    }

    private sealed class Converted : ControlStyleObject
    {
        private int _calls;

        internal Converted(ECustomNode owner) : base(owner) { }

        public override void SetOwner(ECustomNode owner)
        {
            _calls++;
            GetControlStyle();
            base.SetOwner(owner);
        }
    }

    [Fact]
    public void NullFlagOwnerFailsBeforeAnUnacquiredLinkIsReleased()
    {
        using var fixture = new FlagFixture(loaded: true);
        Assert.Throws<NullReferenceException>(() => FlagControl.Create(null!));
        Assert.Throws<NullReferenceException>(() => EngineClass.Of<FlagControl>().CreateInstance(null!, holder: EngineFixture.Engine));
        Assert.Equal(0, fixture.Entry.RefCount);
        Assert.Empty(fixture.Owner.Members);
    }

    private static FieldInfo Field(Type type, string name) => MemberNames.Field(type, name)!;

    private sealed class FlagOwner : GamePack
    {
        public FlagOwner() : base(null!, null!, EngineFixture.Engine) { }

        internal List<EngineObject> Members { get; } = [];

        public override int AddChild(EngineObject child) { Members.Add(child); return Members.Count - 1; }

        public override int RemoveChild(EngineObject child) { int index = Members.IndexOf(child); Members.Remove(child); return index; }
    }

    private sealed class FlagFixture : IDisposable
    {
        internal FlagOwner Owner { get; }

        private readonly ImageLibraryManager _manager = new ImageLibraryManager(null!);

        internal ImageLibraryManagerObject Entry { get; }

        private readonly GamePack _previous = GamePack.Current;
        private readonly IFileSystem _previousFiles = Runtime.Files;
        private readonly string _previousStartup = EngineFixture.Engine.StartupDirectory;
        private readonly string _previousSoundPath = AudioHost.BaseSoundEnginePath;
        private readonly string _previousImagePath = ImageLibraries.BaseImageLibraryPath;
        private readonly string _previousExecutable = Runtime.Application.ExeName;
        private readonly string _root;
        private readonly bool _ownsRoot;

        internal FlagFiles Files { get; }

        internal FlagFixture(bool loaded, string? directory = null)
        {
            _ownsRoot = directory is null;
            _root = directory ?? Path.Combine(Path.GetTempPath(), "aow-cf4-flags-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_root);
            Directory.CreateDirectory(Path.Combine(_root, "IMAGES"));
            Directory.CreateDirectory(Path.Combine(_root, "SFX"));
            Files = new FlagFiles(_root);
            Runtime.Files = Files;
            EngineFixture.Engine.SetStartupDirectory(_root + Path.DirectorySeparatorChar);
            AudioHost.BaseSoundEnginePath = _root + Path.DirectorySeparatorChar;
            ImageLibraries.BaseImageLibraryPath = _root + Path.DirectorySeparatorChar;
            Runtime.Application.ExeName = Path.Combine(_root, "fixture.exe");
            try
            {
                Owner = new FlagOwner();
                Field(typeof(GamePack), "_imageLibraryManager").SetValue(Owner, _manager);
                Field(typeof(ImageLibraryManager), "_searchPath").SetValue(_manager, "9.3b-2-missing/");
                var disk = (ImageLibraryManagerList)Field(typeof(ImageLibraryManager), "_diskList").GetValue(_manager)!;
                if (directory is null) Entry = disk.AddImageLibraryFile("Flags.ILB");
                else
                {
                    _manager.ExecutePathSearch(directory);
                    Entry = Assert.IsType<ImageLibraryManagerObject>(disk.Objects[0]);
                }
                if (loaded)
                    Field(typeof(ImageLibraryManagerObject), "_imageLibrary").SetValue(Entry, new ImageLibrary(null!));
                GamePack.Current = Owner;
                Owner.Members.Clear();
            }
            catch
            {
                RestoreFiles();
                throw;
            }
        }

        public void Dispose()
        {
            try { foreach (var child in Owner.Members.ToArray()) child.Dispose(); }
            finally
            {
                GamePack.Current = _previous;
                try { _manager.Dispose(); }
                finally { RestoreFiles(); }
            }
        }

        private void RestoreFiles()
        {
            EngineFixture.Engine.SetStartupDirectory(_previousStartup);
            AudioHost.BaseSoundEnginePath = _previousSoundPath;
            ImageLibraries.BaseImageLibraryPath = _previousImagePath;
            Runtime.Application.ExeName = _previousExecutable;
            Runtime.Files = _previousFiles;
            if (_ownsRoot) Directory.Delete(_root, recursive: true);
        }
    }

    private sealed class FlagFiles : IFileSystem
    {
        private readonly string _root;
        private readonly PhysicalFileSystem _files = new();

        internal List<string> OpenedPaths { get; } = [];
        internal List<string> EnumeratedDirectories { get; } = [];

        internal FlagFiles(string root)
        {
            this._root = Path.GetFullPath(root);
            Assert.True(_files.SetCurrentDirectory(this._root));
        }

        private string Checked(string path)
        {
            string full = _files.ExpandPath(path);
            Assert.True(full.Equals(_root, StringComparison.OrdinalIgnoreCase) || full.StartsWith(_root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Flag fixture escaped temporary root: " + full);
            return full;
        }

        public string CurrentDirectory => _files.CurrentDirectory;

        public bool SetCurrentDirectory(string path) => _files.SetCurrentDirectory(Checked(path));
        public string ExpandPath(string path) => Checked(path);

        public Stream Open(string path, int mode)
        {
            string full = Checked(path);
            OpenedPaths.Add(full);
            return _files.Open(full, mode);
        }

        public bool FileExists(string path) => _files.FileExists(Checked(path));
        public bool DirectoryExists(string path) => _files.DirectoryExists(Checked(path));
        public bool DeleteFile(string path) => _files.DeleteFile(Checked(path));
        public bool RenameFile(string source, string target) => _files.RenameFile(Checked(source), Checked(target));
        public bool CreateDirectory(string path) => _files.CreateDirectory(Checked(path));
        public bool RemoveDirectory(string path) => _files.RemoveDirectory(Checked(path));
        public int GetAttributes(string path) => _files.GetAttributes(Checked(path));
        public int SetAttributes(string path, int attributes) => _files.SetAttributes(Checked(path), attributes);
        public DateTime? LastWriteTime(string path) => _files.LastWriteTime(Checked(path));

        public IEnumerable<RtlFileEntry> Enumerate(string directory)
        {
            string full = Checked(directory);
            EnumeratedDirectories.Add(full);
            return _files.Enumerate(full).ToArray();
        }
    }

    private static FlagControl CreateFlag(ECustomNode owner, bool throughMeta) => throughMeta
        ? (FlagControl)EngineClass.Of<FlagControl>().CreateInstance(owner, holder: EngineFixture.Engine)
        : FlagControl.Create(owner);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FlagLinkCountAndOwnerMatchTheLegacyFactory(bool throughMeta)
    {
        using var fixture = new FlagFixture(loaded: true);
        Assert.Equal(0, fixture.Entry.RefCount);
        var control = CreateFlag(fixture.Owner, throughMeta);
        Assert.IsType<FlagControl>(control);
        Assert.Same(fixture.Owner, control.Owner);
        Assert.Same(fixture.Entry.ImageLibrary, control.Flags);
        Assert.Equal(1, fixture.Entry.RefCount);
        Assert.Same(control, Assert.Single(fixture.Owner.Members));
        control.Dispose();
        Assert.Equal(0, fixture.Entry.RefCount);
        Assert.Null(control.Owner);
        Assert.Empty(fixture.Owner.Members);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FlagLoadFailureRestoresTheLinkCountAndOwner(bool throughMeta)
    {
        using var fixture = new FlagFixture(loaded: false);
        Assert.Equal(0, fixture.Entry.RefCount);
        var error = Assert.Throws<IOException>(() => CreateFlag(fixture.Owner, throughMeta));
        Assert.Equal("Error loading: 9.3b-2-missing/FLAGS.ILB", error.Message);
        Assert.Equal(0, fixture.Entry.RefCount);
        Assert.Empty(fixture.Owner.Members);
        Assert.NotNull(fixture.Entry.ImageLibrary);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CorruptFlagFileDiscoveredByImageSearchRestoresPublication(bool imageClassesInitialized)
    {
        string directory = Path.Combine(Path.GetTempPath(), "aow-cf4-flags-" + Guid.NewGuid().ToString("N"));
        var previousClasses = ImageLibraries.LibraryImageClasses;
        var classes = imageClassesInitialized ? new List<LibraryImageClass>() : null;
        Directory.CreateDirectory(directory);
        try
        {
            // The version-4 index is truncated, so ReadBuffer fails before any image-class lookup.
            ImageLibraryHeader header = new() { Id = "ILB", Version = 4, HeaderSize = 24, IndexEnd = 28, Size = 28 };
            string path = Path.Combine(directory, "FLAGS.ILB");
            File.WriteAllBytes(path, Bytes.Of(ref header, 24).ToArray());
            ImageLibraries.LibraryImageClasses = classes!;
            using var fixture = new FlagFixture(loaded: false, directory);
            using (var library = new ImageLibrary(null!)) { Assert.Throws<EndOfStreamException>(() => library.LoadFromFile(path)); }
            fixture.Files.OpenedPaths.Clear();
            var error = Assert.Throws<IOException>(() => FlagControl.Create(fixture.Owner));
            Assert.Contains("Error loading:", error.Message, StringComparison.Ordinal);
            Assert.Equal(path.ToUpperInvariant(), Assert.Single(fixture.Files.OpenedPaths).ToUpperInvariant());
            Assert.Contains(fixture.Files.EnumeratedDirectories, entry => entry.Equals(directory, StringComparison.OrdinalIgnoreCase));
            Assert.Equal(0, fixture.Entry.RefCount);
            Assert.Empty(fixture.Owner.Members);
        }
        finally
        {
            ImageLibraries.LibraryImageClasses = previousClasses;
            classes?.Clear();
            Directory.Delete(directory, recursive: true);
        }
    }

    private sealed class ReadEngine : GameEngine
    {
        internal ReadEngine() : base(null!) { }

        internal EngineObject? Created { get; private set; }

        public override EngineObject CreateEngineObject(int classId, ECustomNode owner)
        {
            Created = base.CreateEngineObject(classId, owner);
            return Created;
        }
    }

    // An exact-read stream makes a short binary read fail at the truncated payload.
    private sealed class TruncatedStream : MemoryStream
    {
        public override int Read(Span<byte> buffer)
        {
            int count = base.Read(buffer);
            if (count != buffer.Length) throw new EndOfStreamException("Stream read error");
            return count;
        }
    }

    private static ReadEngine CreateEngine()
    {
        var engine = new ReadEngine();
        return engine;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TruncatedBinaryReadDestroysOnlyTheNewObject(bool propagate)
    {
        var engine = CreateEngine();
        var owner = new EngineFixture.Owner();
        var existing = new EngineObject(owner);
        EngineObject source = EngineClass.Of<ControlStyleObject>().CreateInstance(null!, holder: EngineFixture.Engine);
        using var stream = new TruncatedStream();
        try
        {
            var writer = new StorageWriter(stream, engine, false);
            writer.Created(ref source, null!, 7);
            writer.Dispose();
            stream.SetLength(stream.Length - 1);
            stream.Position = 0;
            var reader = new StorageReader(stream, engine, false);
            string message = "";
            reader.OnError += (StorageStream _, string text, ref bool proceed) =>
            {
                message = text;
                proceed = !propagate;
            };
            EngineObject result = null!;
            try
            {
                if (propagate)
                    Assert.Equal("Stream read error", Assert.Throws<EndOfStreamException>(() => reader.Created(ref result, owner, 7)).Message);
                else
                    reader.Created(ref result, owner, 7);
                Assert.Equal("Stream read error", message);
                Assert.Null(result);
                Assert.IsType<ControlStyleObject>(engine.Created);
                Assert.Null(engine.Created!.Owner);
                Assert.Same(existing, Assert.Single(owner.Members));
                Assert.Equal(1, owner.Removals);
            }
            finally { reader.Dispose(); }
        }
        finally { source.Dispose(); existing.Dispose(); engine.Dispose(); }
    }

    // A child read that fails with an empty message is reported as the stream default.
    private sealed class EmptyMessageStream : MemoryStream
    {
        public override int Read(Span<byte> buffer)
        {
            int count = base.Read(buffer);
            if (count != buffer.Length) throw new EndOfStreamException("");
            return count;
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void EmptyChildReadUsesTheDefaultMessageAndHonorsContinuation(bool propagate)
    {
        var engine = CreateEngine();
        var owner = new EngineFixture.Owner();
        var existing = new EngineObject(owner);
        EngineObject source = EngineClass.Of<ControlStyleObject>().CreateInstance(null!, holder: EngineFixture.Engine);
        using var stream = new EmptyMessageStream();
        try
        {
            var writer = new StorageWriter(stream, engine, false);
            writer.Created(ref source, null!, 7);
            writer.Dispose();
            stream.SetLength(stream.Length - 1);
            stream.Position = 0;
            var reader = new StorageReader(stream, engine, false);
            string message = "";
            reader.OnError += (StorageStream _, string text, ref bool proceed) =>
            {
                message = text;
                proceed = !propagate;
            };
            EngineObject result = null!;
            try
            {
                if (propagate)
                    Assert.Equal("", Assert.Throws<EndOfStreamException>(() => reader.Created(ref result, owner, 7)).Message);
                else
                    reader.Created(ref result, owner, 7);
                Assert.Equal("Error Reading EObject", message);
                Assert.Null(result);
                Assert.IsType<ControlStyleObject>(engine.Created);
                Assert.Null(engine.Created!.Owner);
                Assert.Same(existing, Assert.Single(owner.Members));
                Assert.Equal(1, owner.Removals);
            }
            finally { reader.Dispose(); }
        }
        finally { source.Dispose(); existing.Dispose(); engine.Dispose(); }
    }
}
