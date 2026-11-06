using System.Reflection;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Game = Aow.Units.AoWE.AoWEUnit;
using EngineUnit = Aow.Units.Engine.EngineUnit;
using Aow.Units.Sound;

namespace Aow.Units.Tests;

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
        TECustomNode argument = owned ? owner : null!;
        Type type = kind switch { 0 => typeof(TEControlStyleObject), 1 => typeof(TEResourceEditSettings), _ => typeof(TPadControl) };
        var meta = EClass.Of(type);
        if (kind == 2)
        {
            // The pad has the inherited ID 81, so isolate its class entry in this test engine.
            engine.UnregisterEClasses([EClass.Of<TEControlStyleObject>()]);
            engine.RegisterEClasses([EClass.Of(type)]);
        }
        var direct = kind switch
        {
            0 => new TEControlStyleObject(argument),
            1 => new TEResourceEditSettings(argument),
            _ => (TEControlStyleObject)new TPadControl(argument)
        };
        TEObject[] objects = [direct, meta.Create(argument), engine.CreateEObject(meta.ClassID, argument)];
        TEObject? copy = null;
        try
        {
            Assert.Equal(kind == 1 ? 1026 : 81, meta.ClassID);
            Assert.Equal(kind == 0 ? 12 : kind == 1 ? 16 : 20, meta.InstanceSize);
            Assert.Same(meta, Aow.Units.Engine.EClass.Of(type));
            foreach (var value in objects)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.EClass);
            }
            if (direct is TEResourceEditSettings settings)
                settings.Editors.AddObject("Editor", Runtime.IntToObject(17));
            direct.SetControlStyle(TSet<TEControlStyleElement>.Of(EngineUnit.csStored, EngineUnit.csShared));
            copy = direct.Copy(engine);
            Assert.IsType(type, copy);
            Assert.Null(copy.Owner);
            Assert.Equal(direct.GetControlStyle(), copy.GetControlStyle());
            if (copy is TEResourceEditSettings copied)
            {
                Assert.NotSame(((TEResourceEditSettings)direct).Editors, copied.Editors);
                Assert.Equal("Editor", copied.Editors[0]);
                Assert.Equal(17, Runtime.ObjectToInt(copied.Editors.Objects[0]));
            }
            if (direct is TPadControl pad)
            {
                Assert.NotNull(pad.FTerrainTypes);
                Assert.NotNull(pad.FPadResources);
            }
            Assert.Equal(owned ? 3 : 0, owner.Members.Count);
        }
        finally
        {
            copy?.Destroy();
            foreach (var value in objects) value.Destroy();
            engine.Destroy();
        }
        Assert.Empty(owner.Members);
    }

    private sealed class Converted : TEControlStyleObject
    {
        internal int Calls;
        internal int Marker;
        internal int ObservedMarker;
        internal TSet<TEControlStyleElement> ObservedStyle;
        internal Converted(TECustomNode owner) : base(owner) { Marker = 77; }
        public override void SetOwner(TECustomNode owner)
        {
            Calls++;
            ObservedMarker = Marker;
            ObservedStyle = GetControlStyle();
            base.SetOwner(owner);
        }
    }





    [Fact]
    public void NullFlagOwnerFailsBeforeAnUnacquiredLinkIsReleased()
    {
        using var fixture = new FlagFixture(loaded: true);
        Assert.Throws<NullReferenceException>(() => TFlagControl.Create(null!));
        Assert.Throws<NullReferenceException>(() => EClass.Of<TFlagControl>().Create(null!));
        Assert.Equal(0, fixture.Entry.RefCount);
        Assert.Empty(fixture.Owner.Members);
    }

    private static FieldInfo Field(Type type, string name) =>
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!;

    private sealed class FlagOwner : TAoWHSSet
    {
        public FlagOwner() : base(null!, null!) { }

        internal readonly List<TEObject> Members = [];
        public override int AddChild(TEObject child) { Members.Add(child); return Members.Count - 1; }
        public override int RemoveChild(TEObject child) { int index = Members.IndexOf(child); Members.Remove(child); return index; }
    }

    private sealed class FlagFixture : IDisposable
    {
        internal readonly FlagOwner Owner;
        internal readonly TImageLibraryManager Manager = new TImageLibraryManager(null!);
        internal readonly TILManagerObject Entry;
        private readonly TAoWHSSet previous = Game.AoWHSSet;
        private readonly IFileSystem previousFiles = Runtime.Files;
        private readonly string previousStartup = EngineFixture.Engine.StartupDirectory;
        private readonly string previousSoundPath = SoundUnit.BaseSoundEnginePath;
        private readonly string previousImagePath = ImageLibUnit.BaseImageLibraryPath;
        private readonly string previousExe = Runtime.Application.ExeName;
        private readonly string root;
        private readonly bool ownsRoot;
        internal readonly FlagFiles Files;

        internal FlagFixture(bool loaded, string? directory = null)
        {
            ownsRoot = directory is null;
            root = directory ?? Path.Combine(Path.GetTempPath(), "aow-cf4-flags-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(root);
            Directory.CreateDirectory(Path.Combine(root, "IMAGES"));
            Directory.CreateDirectory(Path.Combine(root, "SFX"));
            Files = new FlagFiles(root);
            Runtime.Files = Files;
            EngineFixture.Engine.SetStartupDirectory(root + Path.DirectorySeparatorChar);
            SoundUnit.BaseSoundEnginePath = root + Path.DirectorySeparatorChar;
            ImageLibUnit.BaseImageLibraryPath = root + Path.DirectorySeparatorChar;
            Runtime.Application.ExeName = Path.Combine(root, "fixture.exe");
            try
            {
                Owner = new FlagOwner();
                Field(typeof(THSSet), "FImageLibraryManager").SetValue(Owner, Manager);
                Field(typeof(TImageLibraryManager), "FSearchPath").SetValue(Manager, "9.3b-2-missing/");
                var disk = (TILManagerList)Field(typeof(TImageLibraryManager), "FDiskList").GetValue(Manager)!;
                if (directory is null) Entry = disk.AddILFile("Flags.ILB");
                else
                {
                    Manager.ExecutePathSearch(directory);
                    Entry = Assert.IsType<TILManagerObject>(disk.Objects[0]);
                }
                if (loaded)
                    Field(typeof(TILManagerObject), "FImageLibrary").SetValue(Entry, new TImageLibrary(null!));
                Game.AoWHSSet = Owner;
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
            try { foreach (var child in Owner.Members.ToArray()) child.Destroy(); }
            finally
            {
                Game.AoWHSSet = previous;
                try { Manager.Destroy(); }
                finally { RestoreFiles(); }
            }
        }

        private void RestoreFiles()
        {
            EngineFixture.Engine.SetStartupDirectory(previousStartup);
            SoundUnit.BaseSoundEnginePath = previousSoundPath;
            ImageLibUnit.BaseImageLibraryPath = previousImagePath;
            Runtime.Application.ExeName = previousExe;
            Runtime.Files = previousFiles;
            if (ownsRoot) Directory.Delete(root, recursive: true);
        }
    }

    private sealed class FlagFiles : IFileSystem
    {
        private readonly string root;
        private readonly SystemIoFileSystem files = new();
        internal readonly List<string> OpenedPaths = [];
        internal readonly List<string> EnumeratedDirectories = [];
        internal FlagFiles(string root)
        {
            this.root = Path.GetFullPath(root);
            Assert.True(files.SetCurrentDirectory(this.root));
        }
        private string Checked(string path)
        {
            string full = files.ExpandPath(path);
            Assert.True(full.Equals(root, StringComparison.OrdinalIgnoreCase) || full.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase), "Flag fixture escaped temporary root: " + full);
            return full;
        }
        public string CurrentDirectory => files.CurrentDirectory;
        public bool SetCurrentDirectory(string path) => files.SetCurrentDirectory(Checked(path));
        public string ExpandPath(string path) => Checked(path);
        public Stream Open(string path, int mode)
        {
            string full = Checked(path);
            OpenedPaths.Add(full);
            return files.Open(full, mode);
        }
        public bool FileExists(string path) => files.FileExists(Checked(path));
        public bool DirectoryExists(string path) => files.DirectoryExists(Checked(path));
        public bool DeleteFile(string path) => files.DeleteFile(Checked(path));
        public bool RenameFile(string source, string target) => files.RenameFile(Checked(source), Checked(target));
        public bool CreateDirectory(string path) => files.CreateDirectory(Checked(path));
        public bool RemoveDirectory(string path) => files.RemoveDirectory(Checked(path));
        public int GetAttributes(string path) => files.GetAttributes(Checked(path));
        public int SetAttributes(string path, int attributes) => files.SetAttributes(Checked(path), attributes);
        public DateTime? LastWriteTime(string path) => files.LastWriteTime(Checked(path));
        public IEnumerable<RtlFileEntry> Enumerate(string directory)
        {
            string full = Checked(directory);
            EnumeratedDirectories.Add(full);
            return files.Enumerate(full).ToArray();
        }
    }

    private static TFlagControl CreateFlag(TECustomNode owner, bool throughMeta) => throughMeta
        ? (TFlagControl)EClass.Of<TFlagControl>().Create(owner)
        : TFlagControl.Create(owner);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FlagLinkCountAndOwnerMatchTheLegacyFactory(bool throughMeta)
    {
        using var fixture = new FlagFixture(loaded: true);
        Assert.Equal(0, fixture.Entry.RefCount);
        var control = CreateFlag(fixture.Owner, throughMeta);
        Assert.IsType<TFlagControl>(control);
        Assert.Same(fixture.Owner, control.Owner);
        Assert.Same(fixture.Entry.ImageLibrary, control.FFlags);
        Assert.Equal(1, fixture.Entry.RefCount);
        Assert.Same(control, Assert.Single(fixture.Owner.Members));
        control.Destroy();
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
        var error = Assert.Throws<Aow.Rtl.Exception>(() => CreateFlag(fixture.Owner, throughMeta));
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
        var previousClasses = ImageLibUnit.LibraryImageClasses;
        var classes = imageClassesInitialized ? new TList() : null;
        Directory.CreateDirectory(directory);
        try
        {
            // The version-4 index is truncated, so ReadBuffer fails before any image-class lookup.
            TILBHeader header = new() { ID = "ILB", Version = 4, HeaderSize = 24, IndexEnd = 28, Size = 28 };
            string path = Path.Combine(directory, "FLAGS.ILB");
            File.WriteAllBytes(path, Bytes.Of(ref header, 24).ToArray());
            ImageLibUnit.LibraryImageClasses = classes!;
            using var fixture = new FlagFixture(loaded: false, directory);
            var library = new TImageLibrary(null!);
            try { Assert.Throws<EReadError>(() => library.LoadFromFile(path)); }
            finally { library.Destroy(); }
            fixture.Files.OpenedPaths.Clear();
            var error = Assert.Throws<Aow.Rtl.Exception>(() => TFlagControl.Create(fixture.Owner));
            Assert.Contains("Error loading:", error.Message, StringComparison.Ordinal);
            Assert.Equal(path.ToUpperInvariant(), Assert.Single(fixture.Files.OpenedPaths).ToUpperInvariant());
            Assert.Contains(fixture.Files.EnumeratedDirectories, entry => entry.Equals(directory, StringComparison.OrdinalIgnoreCase));
            Assert.Equal(0, fixture.Entry.RefCount);
            Assert.Empty(fixture.Owner.Members);
        }
        finally
        {
            ImageLibUnit.LibraryImageClasses = previousClasses;
            classes?.Destroy();
            Directory.Delete(directory, recursive: true);
        }
    }


    private sealed class ReadEngine : TEngine
    {
        internal ReadEngine() : base(null!) { }
        internal TEObject? Created;
        public override TEObject CreateEObject(int classId, TECustomNode owner)
        {
            Created = base.CreateEObject(classId, owner);
            return Created;
        }
    }

    // An exact-read stream makes a short binary read fail at the truncated payload.
    private sealed class TruncatedStream : TMemoryStream
    {
        public override int Read(Span<byte> buffer)
        {
            int count = base.Read(buffer);
            if (count != buffer.Length) throw new EReadError("Stream read error");
            return count;
        }
    }

    private static ReadEngine CreateEngine()
    {
        if (EngineUnit.Engines is null) EngineUnit.Initialize();
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
        var existing = new TEObject(owner);
        TEObject source = EClass.Of<TEControlStyleObject>().Create(null!);
        using var stream = new TruncatedStream();
        try
        {
            var writer = new TEWriteStorageStream(stream, engine, false);
            writer.rwCreateEObject(ref source, null!, 7);
            writer.Destroy();
            stream.Size--;
            stream.Position = 0;
            var reader = new TEReadStorageStream(stream, engine, false);
            string message = "";
            reader.OnError = (TEStorageStream _, string text, ref bool proceed) =>
            {
                message = text;
                proceed = !propagate;
            };
            TEObject result = null!;
            try
            {
                if (propagate)
                    Assert.Equal("Stream read error", Assert.Throws<EReadError>(() => reader.rwCreateEObject(ref result, owner, 7)).Message);
                else
                    reader.rwCreateEObject(ref result, owner, 7);
                Assert.Equal("Stream read error", message);
                Assert.Null(result);
                Assert.IsType<TEControlStyleObject>(engine.Created);
                Assert.Null(engine.Created!.Owner);
                Assert.Same(existing, Assert.Single(owner.Members));
                Assert.Equal(1, owner.Removals);
            }
            finally { reader.Destroy(); }
        }
        finally { source.Destroy(); existing.Destroy(); engine.Destroy(); }
    }
}
