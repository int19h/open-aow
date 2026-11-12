
using Aow.Formats;
using Aow.Formats.Json;

using Aow.Game.Saves;

using Aow.Platform;

namespace Aow.Game.Tests;

/// <summary>
/// Atomic replacement at the file boundary (own-formats.md 4.2): serialization and validation
/// finish before anything is written, a sibling temporary is written and closed, and only a
/// complete temporary replaces the old file. Every failure keeps the old file.
/// </summary>
// The file service and the file log are process globals, so the case runs in the engine collection.
[Collection("Engine")]
public sealed class AtomicReplacementTests : IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), "open-aow-replace-" + Guid.NewGuid().ToString("N"));
    private readonly IFileSystem _previous = Runtime.Files;
    private readonly Action<FileLogRecord>? _previousLog = FileBoundary.Log;
    private readonly List<FileLogRecord> _log = [];

    public AtomicReplacementTests()
    {
        Directory.CreateDirectory(Path.Combine(_directory, "root", "User"));
        Directory.CreateDirectory(Path.Combine(_directory, "user"));
        FileBoundary.Log = _log.Add;
    }

    public void Dispose()
    {
        Runtime.Files = _previous;
        FileBoundary.Log = _previousLog;
        Directory.Delete(_directory, true);
    }

    /// <summary>The engine image of an empty hero list: the stored image of one root property.</summary>
    private static byte[] HeroListImage(string name = "") => GameFiles.ExportImage((JsonObject)JsonReader.Parse(
        "{\"$format\": \"OpenAoW\", \"$version\": " + OwnFormat.CurrentVersion + ", \"$kind\": \"hero-list\", \"$container\": {\"profile\": \"hero-list\", \"compression\": \"none\"}, \"root\": {"
        + (name.Length == 0 ? "" : "\"LibraryName\": \"" + name + "\"") + "}}"));

    private string Old => Path.Combine(_directory, "user", "User", "list.ahl");
    private string Logical => Path.Combine(_directory, "root", "User", "list.ahl");
    private string Temporary => FileBoundary.TemporaryFor(Old);

    private LayeredFileSystem Layers(bool withOld = true)
    {
        if (withOld)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(Old)!);
            File.WriteAllBytes(Old, HeroListImage());
        }
        var files = new LayeredFileSystem(Path.Combine(_directory, "root"), Path.Combine(_directory, "user"));
        Runtime.Files = files;
        return files;
    }

    [Fact]
    public void OriginalUserSaveIsReplacedByItsDocument()
    {
        Layers();
        byte[] original = File.ReadAllBytes(Old);
        FileBoundary.Save(Logical, GameProfile.HeroList, original);
        byte[] written = File.ReadAllBytes(Old);
        Assert.True(GameFiles.IsJsonCandidate(written));
        Assert.False(File.Exists(Temporary));
        var reread = GameBoundary.PrepareLoad(written, Logical, GameProfile.HeroList, FormatMapping.Validators);
        Assert.Equal(original, reread.Bytes);
        var record = Assert.Single(_log);
        Assert.True(record.Succeeded);
        Assert.Equal("user", record.Layer);
        Assert.Equal("write", record.Report.Operation);
    }

    [Fact]
    public void SerializationFailureKeepsTheOldFileAndWritesNothing()
    {
        Layers();
        byte[] before = File.ReadAllBytes(Old);
        var error = Assert.Throws<GameFileErrorException>(() => FileBoundary.Save(Logical, GameProfile.HeroList, [1, 2, 3]));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
        Assert.False(error.Record.Succeeded);
    }

    [Fact]
    public void WriteFailureKeepsTheOldFileAndRemovesTheTemporary()
    {
        var layers = Layers();
        byte[] before = File.ReadAllBytes(Old);
        Runtime.Files = new FailingFiles(layers, failWrite: true);
        Assert.Throws<GameFileErrorException>(() => FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New")));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
        Assert.Equal(Codes.Output, Assert.Single(_log).Report.Error!.Code);
    }

    [Fact]
    public void ReplaceFailureKeepsTheOldFileAndRemovesTheTemporary()
    {
        var layers = Layers();
        byte[] before = File.ReadAllBytes(Old);
        Runtime.Files = new FailingFiles(layers, failReplace: true);
        Assert.Throws<GameFileErrorException>(() => FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New")));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
    }

    [Fact]
    public void ShippedFileIsShadowedNeverWritten()
    {
        Layers(withOld: false);
        string shipped = Path.Combine(_directory, "root", "User", "list.ahl");
        File.WriteAllBytes(shipped, HeroListImage());
        Runtime.Files = new LayeredFileSystem(Path.Combine(_directory, "root"), Path.Combine(_directory, "user"));
        FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New"));
        Assert.Equal(HeroListImage(), File.ReadAllBytes(shipped));
        Assert.True(GameFiles.IsJsonCandidate(File.ReadAllBytes(Old)));
        Assert.Equal("user", Runtime.Files.Layer(Logical));
    }

    [Fact]
    public void FileServiceWithoutReplacementRefusesTheSave()
    {
        var layers = Layers();
        byte[] before = File.ReadAllBytes(Old);
        Runtime.Files = new NoReplaceFiles(layers);
        Assert.Throws<GameFileErrorException>(() => FileBoundary.Save(Logical, GameProfile.HeroList, HeroListImage("New")));
        Assert.Equal(before, File.ReadAllBytes(Old));
        Assert.False(File.Exists(Temporary));
    }

    private sealed class FailingFiles(IFileSystem inner, bool failWrite = false, bool failReplace = false) : IFileSystem
    {
        public string CurrentDirectory => inner.CurrentDirectory;

        public string ExpandPath(string path) => inner.ExpandPath(path);

        public Stream Open(string path, int mode)
        {
            var stream = inner.Open(path, mode);
            return failWrite && mode == Runtime.fmCreate ? new BrokenStream(stream) : stream;
        }

        public bool FileExists(string path) => inner.FileExists(path);
        public bool DirectoryExists(string path) => inner.DirectoryExists(path);
        public bool DeleteFile(string path) => inner.DeleteFile(path);
        public bool RenameFile(string source, string target) => inner.RenameFile(source, target);
        public bool ReplaceFile(string source, string target) => !failReplace && inner.ReplaceFile(source, target);
        public string Layer(string path) => inner.Layer(path);
        public bool CreateDirectory(string path) => inner.CreateDirectory(path);
        public DateTime? LastWriteTime(string path) => inner.LastWriteTime(path);
        public IEnumerable<RtlFileEntry> Enumerate(string directory) => inner.Enumerate(directory);
    }

    // A file service written before the replacement primitive: the interface default refuses.
    private sealed class NoReplaceFiles(IFileSystem inner) : IFileSystem
    {
        public string CurrentDirectory => inner.CurrentDirectory;

        public string ExpandPath(string path) => inner.ExpandPath(path);
        public Stream Open(string path, int mode) => inner.Open(path, mode);
        public bool FileExists(string path) => inner.FileExists(path);
        public bool DirectoryExists(string path) => inner.DirectoryExists(path);
        public bool DeleteFile(string path) => inner.DeleteFile(path);
        public bool RenameFile(string source, string target) => inner.RenameFile(source, target);
        public bool CreateDirectory(string path) => inner.CreateDirectory(path);
        public DateTime? LastWriteTime(string path) => inner.LastWriteTime(path);
        public IEnumerable<RtlFileEntry> Enumerate(string directory) => inner.Enumerate(directory);
    }

    // The disk fills after the first byte.
    private sealed class BrokenStream(Stream inner) : Stream
    {
        public override bool CanRead => false;
        public override bool CanSeek => false;
        public override bool CanWrite => true;
        public override long Length => inner.Length;
        public override long Position { get => inner.Position; set => inner.Position = value; }

        public override void Flush() => inner.Flush();
        public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => inner.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count)
        {
            inner.Write(buffer, offset, Math.Min(count, 1));
            throw new IOException("There is not enough space on the disk.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) inner.Dispose();
            base.Dispose(disposing);
        }
    }
}
