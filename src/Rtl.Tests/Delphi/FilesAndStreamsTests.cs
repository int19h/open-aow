using System.Runtime.InteropServices;
using static Aow.Rtl.DelphiDateTime;
using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiStreams;
using static Aow.Rtl.RtlHost;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class FilesAndStreamsTests
{
    // SysUtils.ExtractRelativePath compares the drives of the two paths first. An empty base has
    // none, so the destination comes back unchanged on any drive and from any current directory.
    [Fact]
    public void AnEmptyBaseKeepsTheDestination()
    {
        IFileSystem files = new PhysicalFileSystem();
        string current = Directory.GetCurrentDirectory();
        string sameDrive = Path.Combine(Path.GetPathRoot(current)!, "Games", "x.ilb");
        Assert.Equal(sameDrive, files.ExtractRelativePath("", sameDrive));
        Assert.Equal(@"Images\x.ilb", files.ExtractRelativePath("", @"Images\x.ilb"));
        Assert.Equal(@"Images\x.ilb", files.ExtractRelativePath(Path.Combine(current, "base") + Path.DirectorySeparatorChar, Path.Combine(current, "base", "Images", "x.ilb")));
    }

    [Fact]
    public void FileOperationsUseReplaceableSystemIODefault()
    {
        using ProbeDirectory directory = new();
        IFileSystem saved = Files;
        try
        {
            Files = new PhysicalFileSystem();
            Assert.Equal(directory.Root, GetCurrentDir());
            Assert.False(FileExists("a")); Assert.False(DirectoryExists("a")); Assert.False(DeleteFile("a")); Assert.False(RenameFile("a", "b")); Assert.Equal(-1, FileAge("a"));
            Assert.False(CreateDir("absent/child")); Assert.True(CreateDir("a")); Assert.False(CreateDir("a")); Assert.True(DirectoryExists("a")); Assert.False(FileExists("a"));
            using (Stream f = OpenFile("a/test.bin", fmCreate))
            {
                f.WriteBuffer([1, 2, 3]);
            }

            Assert.True(FileExists("a\\test.bin")); Assert.True(FileExists("A/TEST.BIN")); Assert.False(DirectoryExists("a/test.bin"));
            DateTime modified = new(2024, 2, 29, 12, 34, 56); File.SetLastWriteTime("a/test.bin", modified);
            // The native file API uses the current Windows time-zone bias.
            DateTime nativeTime = OperatingSystem.IsWindows() ? modified.ToUniversalTime() + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now) : modified;
            Assert.Equal(EncodeDate(nativeTime.Year, nativeTime.Month, nativeTime.Day) + EncodeTime(nativeTime.Hour, nativeTime.Minute, nativeTime.Second, 0), FileDateToDateTime(FileAge("a/test.bin")));
            Assert.True(RenameFile("a/test.bin", "a/new.bin")); Assert.False(FileExists("a/test.bin"));
            using (Stream readwrite = OpenFile("a/new.bin", fmOpenReadWrite))
            {
                readwrite.Position = 1;
                readwrite.Write([4]);
                Assert.Equal(2L, readwrite.Position);
            }

            using (Stream write = OpenFile("a/new.bin", fmOpenWrite))
            {
                _ = Assert.Throws<NotSupportedException>(() => write.Read(new byte[1]));
            }

            Assert.True(DeleteFile("a/new.bin")); Assert.False(DeleteFile("a/new.bin"));
            Assert.Equal("Cannot open file missing", Assert.Throws<FileNotFoundException>(() => OpenFile("missing", fmOpenRead)).Message);
            Assert.Equal("Cannot create file absent/file", Assert.Throws<IOException>(() => OpenFile("absent/file", fmCreate)).Message);
            Assert.Empty(Files.FindEntries("absent\\*", attributes: faAnyFile));
            File.WriteAllBytes("a/hidden", []); File.SetAttributes("a/hidden", FileAttributes.Hidden);
            // Only Windows has a hidden attribute. On Linux a hidden file is one whose name starts with a dot.
            if (OperatingSystem.IsWindows())
            {
                Assert.Empty(Files.FindEntries("a\\hidden", attributes: 0));
                RtlFileEntry hidden = Assert.Single(Files.FindEntries("a/hidden", attributes: faHidden)); Assert.True((hidden.Attribute & faHidden) != 0);
            }
            Assert.Equal("C:\\", ExtractFileDir("C:\\file.ext")); Assert.Equal("\\", ExtractFileDir("\\file")); Assert.Equal("C:", ExtractFileDir("C:file"));
            Assert.Equal("\\\\server\\share\\", ExtractFilePath("\\\\server\\share\\file"));
        }
        finally { Files = saved; }
    }

    private sealed class MemoryFiles : IFileSystem
    {
        internal MemoryStream Opened { get; } = new();
        internal List<string> Calls { get; } = [];

        public string CurrentDirectory => "fake";

        public string ExpandPath(string path) { Calls.Add("expand " + path); return "fake/" + path; }

        public Stream Open(string path, int mode) { Calls.Add($"open {path} {mode}"); return Opened; }

        public bool FileExists(string path) { Calls.Add("file " + path); return true; }

        public bool DirectoryExists(string path) { Calls.Add("directory " + path); return true; }

        public bool DeleteFile(string path) { Calls.Add("delete " + path); return true; }

        public bool RenameFile(string source, string target) { Calls.Add("rename " + source + " " + target); return true; }

        public bool CreateDirectory(string path) { Calls.Add("create " + path); return true; }

        public DateTime? LastWriteTime(string path) => new DateTime(2020, 1, 2);

        public IEnumerable<RtlFileEntry> Enumerate(string directory)
        {
            Calls.Add("enumerate " + directory);
            return new[] { new RtlFileEntry("b", 3, 128, new DateTime(2020, 1, 2)), new RtlFileEntry("A", 1, 128, new DateTime(2020, 1, 2)) };
        }
    }

    [Fact]
    public void EveryFileEntryPointUsesTheInjectedFileSystem()
    {
        IFileSystem saved = Files; MemoryFiles files = new();
        try
        {
            Files = files;
            Assert.Equal("fake", GetCurrentDir()); Assert.Equal("fake/a", ExpandFileName("a"));
            Assert.True(FileExists("a")); Assert.True(DirectoryExists("a")); Assert.True(CreateDir("a")); Assert.True(DeleteFile("a")); Assert.True(RenameFile("a", "b"));
            using (Stream stream = OpenFile("virtual", fmCreate)) { stream.WriteBuffer([7]); Assert.Equal(new byte[] { 7 }, files.Opened.ToArray()); }
            Assert.Contains("open virtual 65535", files.Calls);
            IReadOnlyList<RtlFileEntry> entries = Files.FindEntries("*", attributes: 0);
            Assert.Equal(2, entries.Count);
            Assert.Equal("A", entries[0].Name); Assert.Equal(1, entries[0].Size); Assert.Equal(FileAge("A"), entries[0].FileDate);
            Assert.Equal("b", entries[1].Name);
            Assert.Contains("enumerate fake", files.Calls);
        }
        finally { Files = saved; }
    }

    [StoredLayout]
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    private struct Record
    {
        [FieldOffset(0)] public int Number;
        [FieldOffset(4)] public byte Tail;
    }

    private sealed class ShortStream : Stream
    {
        internal int Calls { get; private set; }

        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => 0;

        public override long Position { get => 0; set { } }

        public override void Flush() { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            Calls++;
            return 0;
        }

        public override int Read(Span<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                return 0;
            }

            Calls++;
            return 0;
        }

        public override void Write(byte[] buffer, int offset, int count) => Write(buffer.AsSpan(offset, count));

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            if (buffer.IsEmpty)
            {
                return;
            }

            Calls++;
            throw new IOException("Stream write error");
        }

        public override long Seek(long offset, SeekOrigin origin) => 0;

        public override void SetLength(long value) { }
    }

    [Fact]
    public void TypedStreamBuffersUseRecordBytesAndShortTransfersRaise()
    {
        // CLASSES.PAS:2792..2854. The translated forms preserve the struct's byte layout.
        using MemoryStream memory = new();
        Record record = new() { Number = 0x04030201, Tail = 5 };
        Assert.Equal(8, memory.Write(in record)); memory.WriteBuffer(in record); Assert.Equal(2, memory.Write(in record, 2)); memory.WriteBuffer(in record, 2);
        memory.Position = 0; Record restored = new(); Assert.Equal(8, memory.Read(ref restored)); Assert.Equal(record.Number, restored.Number); Assert.Equal(record.Tail, restored.Tail);
        restored = default; memory.ReadBuffer(ref restored); Assert.Equal(record.Number, restored.Number);
        restored = default; Assert.Equal(2, memory.Read(ref restored, 2)); Assert.Equal(0x0201, restored.Number); memory.ReadBuffer(ref restored, 2);
        _ = Assert.Throws<EndOfStreamException>(() => memory.ReadBuffer(ref restored));
        memory.Clear(); Ptr<byte> pointer = Ptr<byte>.To([1, 2, 3, 4], 1);
        Assert.Equal(2, memory.Write(pointer, 2)); memory.WriteBuffer(pointer, 2); memory.Position = 0; Assert.Equal(2, memory.Read(pointer, 2)); memory.ReadBuffer(pointer, 2);
        _ = Assert.Throws<EndOfStreamException>(() => memory.ReadBuffer(pointer, 1));
        Assert.Equal(0, memory.Write(pointer, -1)); Assert.Equal(0, memory.Read(pointer, -1));
        Assert.Equal(0, memory.Write(new byte[1], -1)); Assert.Equal(0, memory.Read(new byte[1], -1));
        memory.Clear(); Assert.Equal(1, memory.Write([9, 8], 1)); Assert.Equal(0, memory.Read(new byte[2], 1));
        memory.ReadBuffer(default, 0); memory.WriteBuffer(default, 0);
        using ShortStream shortStream = new();
        shortStream.ReadBuffer([]); shortStream.WriteBuffer([]); Assert.Equal(0, shortStream.Calls);
        _ = Assert.Throws<EndOfStreamException>(() => shortStream.ReadBuffer(new byte[1])); _ = Assert.Throws<IOException>(() => shortStream.WriteBuffer(new byte[1])); Assert.Equal(2, shortStream.Calls);
        _ = Assert.Throws<IOException>(() => shortStream.CopyFrom(memory, -1));
    }

    [Fact]
    public void MemoryLoadSaveCapacityAndFileFailuresFollowSource()
    {
        // CLASSES.PAS:2985..3134. LoadFromStream preserves the destination position.
        using ProbeDirectory directory = new();
        using MemoryStream source = new(); using MemoryStream destination = new();
        source.WriteBuffer([1, 2, 3, 4]); source.Position = 2; destination.SetLength(8); destination.Position = 3; destination.LoadFromStream(source);
        Assert.Equal(4L, source.Position); Assert.Equal(3L, destination.Position); Assert.Equal(4L, destination.Length);
        destination.SaveToFile("memory.bin"); Assert.Equal(3L, destination.Position); Assert.Equal(new byte[] { 1, 2, 3, 4 }, File.ReadAllBytes("memory.bin"));
        destination.Clear(); destination.LoadFromFile("memory.bin"); Assert.Equal(0L, destination.Position); Assert.Equal(4L, destination.Length);
        using MemoryStream copy = new(); destination.WriteTo(copy); Assert.Equal(4L, copy.Position); Assert.Equal(0L, destination.Position);
        source.Clear(); destination.LoadFromStream(source); Assert.Equal(0L, destination.Length); Assert.Equal(0L, destination.Position);
        using Stream file = OpenFile("memory.bin", fmOpenRead | fmShareDenyWrite);
        // .NET enforces share modes between opens only on Windows.
        if (OperatingSystem.IsWindows())
        {
            _ = Assert.Throws<IOException>(() => OpenFile("memory.bin", fmOpenWrite));
        }

        _ = Assert.Throws<NotSupportedException>(() => file.SetLength(2)); Assert.Equal(0L, file.Position); Assert.Equal(4L, file.Length);
    }
}
