using System.Runtime.InteropServices;
using static Aow.Rtl.Rtl;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class FilesAndStreamsTests
{
    [Fact]
    public void FileOperationsUseReplaceableSystemIoDefault()
    {
        using var directory = new ProbeDirectory();
        var saved = Files;
        try
        {
            Files = new SystemIoFileSystem();
            Assert.Equal((AnsiString)directory.Root, GetCurrentDir());
            Assert.False(FileExists("a")); Assert.False(DirectoryExists("a")); Assert.False(DeleteFile("a")); Assert.False(RenameFile("a", "b")); Assert.Equal(-1, FileAge("a"));
            Assert.False(CreateDir("absent/child")); Assert.True(CreateDir("a")); Assert.False(CreateDir("a")); Assert.True(DirectoryExists("a")); Assert.False(FileExists("a"));
            using (var f = new TFileStream("a/test.bin", fmCreate)) f.WriteBuffer(new byte[] { 1, 2, 3 });
            Assert.True(FileExists("a\\test.bin")); Assert.True(FileExists("A/TEST.BIN")); Assert.False(DirectoryExists("a/test.bin"));
            var modified = new DateTime(2024, 2, 29, 12, 34, 56); File.SetLastWriteTime("a/test.bin", modified);
            // The native file API uses the current Windows time-zone bias.
            DateTime nativeTime = OperatingSystem.IsWindows() ? modified.ToUniversalTime() + TimeZoneInfo.Local.GetUtcOffset(DateTime.Now) : modified;
            Assert.Equal(EncodeDate(nativeTime.Year, nativeTime.Month, nativeTime.Day) + EncodeTime(nativeTime.Hour, nativeTime.Minute, nativeTime.Second, 0), FileDateToDateTime(FileAge("a/test.bin")));
            Assert.True(RenameFile("a/test.bin", "a/new.bin")); Assert.False(FileExists("a/test.bin"));
            using (var readwrite = new TFileStream("a/new.bin", fmOpenReadWrite)) { readwrite.Position = 1; Assert.Equal(1, readwrite.Write(new byte[] { 4 })); }
            using (var write = new TFileStream("a/new.bin", fmOpenWrite)) Assert.Equal(0, write.Read(new byte[1]));
            Assert.True(DeleteFile("a/new.bin")); Assert.False(DeleteFile("a/new.bin"));
            Assert.Equal((AnsiString)"Cannot open file missing", Assert.Throws<EFOpenError>(() => new TFileStream("missing", fmOpenRead)).Message);
            Assert.Equal((AnsiString)"Cannot create file absent/file", Assert.Throws<EFCreateError>(() => new TFileStream("absent/file", fmCreate)).Message);
            TSearchRec search = default;
            Assert.Equal(3, FindFirst("absent\\*", faAnyFile, ref search));
            File.WriteAllBytes("a/hidden", []); File.SetAttributes("a/hidden", FileAttributes.Hidden);
            Assert.Equal(18, FindFirst("a\\hidden", 0, ref search)); FindClose(ref search);
            Assert.Equal(0, FindFirst("a/hidden", faHidden, ref search)); Assert.True((search.Attr & faHidden) != 0); FindClose(ref search); Assert.Equal(6, FindNext(ref search));
            Assert.Equal((AnsiString)"C:\\", ExtractFileDir("C:\\file.ext")); Assert.Equal((AnsiString)"\\", ExtractFileDir("\\file")); Assert.Equal((AnsiString)"C:", ExtractFileDir("C:file"));
            Assert.Equal((AnsiString)"\\\\server\\share\\", ExtractFilePath("\\\\server\\share\\file"));
        }
        finally { Files = saved; }
    }

    private sealed class MemoryFiles : IFileSystem
    {
        internal MemoryStream Opened = new();
        internal readonly List<string> Calls = [];
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
        var saved = Files; var files = new MemoryFiles();
        try
        {
            Files = files;
            Assert.Equal((AnsiString)"fake", GetCurrentDir()); Assert.Equal((AnsiString)"fake/a", ExpandFileName("a"));
            Assert.True(FileExists("a")); Assert.True(DirectoryExists("a")); Assert.True(CreateDir("a")); Assert.True(DeleteFile("a")); Assert.True(RenameFile("a", "b"));
            using (var stream = new TFileStream("virtual", fmCreate)) { stream.WriteBuffer(new byte[] { 7 }); Assert.Equal(new byte[] { 7 }, files.Opened.ToArray()); }
            Assert.Contains("open virtual 65535", files.Calls);
            TSearchRec search = default;
            Assert.Equal(0, FindFirst("*", 0, ref search)); Assert.Equal((AnsiString)"A", search.Name); Assert.Equal(1, search.Size); Assert.Equal(FileAge("A"), search.Time);
            Assert.Equal(0, FindNext(ref search)); Assert.Equal((AnsiString)"b", search.Name); Assert.Equal(18, FindNext(ref search)); FindClose(ref search);
            Assert.Contains("enumerate fake", files.Calls);
        }
        finally { Files = saved; }
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    private struct Record
    {
        [FieldOffset(0)] public int Number;
        [FieldOffset(4)] public byte Tail;
    }

    private sealed class ShortStream : TStream
    {
        internal int Calls;
        public override int Read(Span<byte> buffer) { Calls++; return 0; }
        public override int Write(ReadOnlySpan<byte> buffer) { Calls++; return 0; }
        public override int Seek(int offset, int origin) => 0;
    }

    [Fact]
    public void TypedStreamBuffersUseRecordBytesAndShortTransfersRaise()
    {
        // CLASSES.PAS:2792..2854. The generated forms preserve the struct's byte layout.
        using var memory = new TMemoryStream();
        var record = new Record { Number = 0x04030201, Tail = 5 };
        Assert.Equal(8, memory.Write(in record)); memory.WriteBuffer(in record); Assert.Equal(2, memory.Write(in record, 2)); memory.WriteBuffer(in record, 2);
        memory.Position = 0; var restored = new Record(); Assert.Equal(8, memory.Read(ref restored)); Assert.Equal(record.Number, restored.Number); Assert.Equal(record.Tail, restored.Tail);
        restored = default; memory.ReadBuffer(ref restored); Assert.Equal(record.Number, restored.Number);
        restored = default; Assert.Equal(2, memory.Read(ref restored, 2)); Assert.Equal(0x0201, restored.Number); memory.ReadBuffer(ref restored, 2);
        Assert.Throws<EReadError>(() => memory.ReadBuffer(ref restored));
        memory.Clear(); var pointer = Ptr<byte>.To(new byte[] { 1, 2, 3, 4 }, 1);
        Assert.Equal(2, memory.Write(pointer, 2)); memory.WriteBuffer(pointer, 2); memory.Position = 0; Assert.Equal(2, memory.Read(pointer, 2)); memory.ReadBuffer(pointer, 2);
        Assert.Throws<EReadError>(() => memory.ReadBuffer(pointer, 1));
        memory.Position = -1; Assert.Throws<EWriteError>(() => memory.WriteBuffer(pointer, 1)); Assert.Equal(0, memory.Write(pointer, -1)); Assert.Equal(0, memory.Read(pointer, -1));
        Assert.Equal(0, memory.Write(new byte[1], -1)); Assert.Equal(0, memory.Read(new byte[1], -1));
        memory.Clear(); Assert.Equal(1, memory.Write(new byte[] { 9, 8 }, 1)); Assert.Equal(0, memory.Read(new byte[2], 1));
        memory.ReadBuffer(default(Ptr<byte>), 0); memory.WriteBuffer(default(Ptr<byte>), 0);
        using var shortStream = new ShortStream();
        shortStream.ReadBuffer([]); shortStream.WriteBuffer([]); Assert.Equal(0, shortStream.Calls);
        Assert.Throws<EReadError>(() => shortStream.ReadBuffer(new byte[1])); Assert.Throws<EWriteError>(() => shortStream.WriteBuffer(new byte[1])); Assert.Equal(2, shortStream.Calls);
        shortStream.Size = 10; shortStream.SetSize(10); Assert.Equal(0, shortStream.Size);
        Assert.Throws<EStreamError>(() => shortStream.CopyFrom(memory, -1));
    }

    [Fact]
    public void MemoryLoadSaveCapacityAndFileFailuresFollowSource()
    {
        // CLASSES.PAS:2985..3134. LoadFromStream preserves the destination position.
        using var directory = new ProbeDirectory();
        using var source = new TMemoryStream(); using var destination = new TMemoryStream();
        source.WriteBuffer(new byte[] { 1, 2, 3, 4 }); source.Position = 2; destination.Size = 8; destination.Position = 3; destination.LoadFromStream(source);
        Assert.Equal(4, source.Position); Assert.Equal(3, destination.Position); Assert.Equal(4, destination.Size);
        destination.SaveToFile("memory.bin"); Assert.Equal(3, destination.Position); Assert.Equal(new byte[] { 1, 2, 3, 4 }, File.ReadAllBytes("memory.bin"));
        destination.Clear(); destination.LoadFromFile("memory.bin"); Assert.Equal(0, destination.Position); Assert.Equal(4, destination.Size);
        using var copy = new TMemoryStream(); destination.SaveToStream(copy); Assert.Equal(4, copy.Position); Assert.Equal(0, destination.Position);
        destination.Capacity = 8193; Assert.Equal(16384, destination.Capacity); Assert.Equal(4, destination.Size);
        Assert.Throws<EStreamError>(() => destination.Capacity = -1); Assert.Throws<EStreamError>(() => destination.SetSize(-1));
        source.Clear(); destination.LoadFromStream(source); Assert.Equal(0, destination.Size); Assert.Equal(0, destination.Capacity);
        using var file = new TFileStream("memory.bin", fmOpenRead | fmShareDenyWrite);
        Assert.Throws<EFOpenError>(() => new TFileStream("memory.bin", fmOpenWrite));
        Assert.Equal(5, Assert.Throws<EWin32Error>(() => file.Size = 2).ErrorCode); Assert.Equal(2, file.Position); Assert.Equal(4, file.Size);
    }
}
