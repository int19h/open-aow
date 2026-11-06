using System.Globalization;
using System.IO.Enumeration;

namespace Aow.Rtl;

public readonly record struct RtlFileEntry(string Name, int Size, int Attr, DateTime Modified);

// The host brief replaces these defaults through Rtl.Files and Rtl.Clock.
public interface IFileSystem
{
    string CurrentDirectory { get; }
    string VirtualDrive => Path.GetPathRoot(ExpandPath("."))?.TrimEnd('\\', '/') ?? "";
    string ExtractRelativePath(string baseFile, string destination)
    {
        string first = ExpandPath(baseFile), second = ExpandPath(destination);
        if (!string.Equals(Path.GetPathRoot(first), Path.GetPathRoot(second), StringComparison.OrdinalIgnoreCase)) return destination;
        return Path.GetRelativePath(Path.GetDirectoryName(first) ?? CurrentDirectory, second).Replace(Path.DirectorySeparatorChar, '\\');
    }
    bool SetCurrentDirectory(string path) => false;
    string ExpandPath(string path);
    Stream Open(string path, int mode);
    bool FileExists(string path);
    bool DirectoryExists(string path);
    bool DeleteFile(string path);
    bool RenameFile(string source, string target);
    // The replacement primitive of the game's file boundary (own-formats.md 4.2): moves a
    // finished, closed sibling temporary over the target in one step. A file service without it
    // refuses, so a save never truncates the old file first.
    bool ReplaceFile(string source, string target) => false;
    // The layer a path resolves to, for file logs only.
    string Layer(string path) => "file";
    bool CreateDirectory(string path);
    bool RemoveDirectory(string path) => false;
    int GetAttributes(string path) => -1;
    int SetAttributes(string path, int attributes) => -1;
    DateTime? LastWriteTime(string path);
    IEnumerable<RtlFileEntry> Enumerate(string directory);
}

public sealed class SystemIoFileSystem : IFileSystem
{
    private string? currentDirectory;
    public string CurrentDirectory => currentDirectory ?? Directory.GetCurrentDirectory();
    public bool SetCurrentDirectory(string path)
    {
        string resolved = Resolve(path);
        if (!Directory.Exists(resolved)) return false;
        currentDirectory = resolved;
        return true;
    }
    public string ExpandPath(string path) => Path.GetFullPath(path.Length == 0 ? "." : path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar), CurrentDirectory);
    private string Resolve(string path)
    {
        string full = ExpandPath(path);
        if (OperatingSystem.IsWindows()) return full;
        string current = Path.GetPathRoot(full)!;
        foreach (string part in full[current.Length..].Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
        {
            string? match = Directory.Exists(current) ? Directory.EnumerateFileSystemEntries(current).FirstOrDefault(p => string.Equals(Path.GetFileName(p), part, StringComparison.OrdinalIgnoreCase)) : null;
            current = match ?? Path.Combine(current, part);
        }
        return current;
    }
    public Stream Open(string path, int mode) => new FileStream(Resolve(path), mode == RtlUnit.fmCreate ? FileMode.Create : FileMode.Open,
        mode == RtlUnit.fmCreate ? FileAccess.ReadWrite : (mode & 3) switch { 0 => FileAccess.Read, 1 => FileAccess.Write, _ => FileAccess.ReadWrite },
        (mode & 0xF0) == RtlUnit.fmShareDenyWrite ? FileShare.Read : FileShare.ReadWrite);
    public bool FileExists(string path) => File.Exists(Resolve(path));
    public bool DirectoryExists(string path) => Directory.Exists(Resolve(path));
    private static bool Attempt(Action action) { try { action(); return true; } catch (System.Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException) { return false; } }
    public bool DeleteFile(string path) => FileExists(path) && Attempt(() => File.Delete(Resolve(path)));
    public bool RenameFile(string source, string target) => Attempt(() => File.Move(Resolve(source), Resolve(target)));
    public bool ReplaceFile(string source, string target) => FileExists(source) && Attempt(() => File.Move(Resolve(source), Resolve(target), true));
    public bool CreateDirectory(string path) => !DirectoryExists(path) && Directory.Exists(Path.GetDirectoryName(Resolve(path))) && Attempt(() => Directory.CreateDirectory(Resolve(path)));
    public bool RemoveDirectory(string path) => DirectoryExists(path) && Attempt(() => Directory.Delete(Resolve(path)));
    public int GetAttributes(string path)
    {
        try { return (int)File.GetAttributes(Resolve(path)); }
        catch (System.Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException) { return -1; }
    }
    public int SetAttributes(string path, int attributes) => Attempt(() => File.SetAttributes(Resolve(path), (FileAttributes)attributes)) ? 0 : -1;
    public DateTime? LastWriteTime(string path) => FileExists(path) ? RtlFileTime.LastWriteTime(Resolve(path)) : null;
    public IEnumerable<RtlFileEntry> Enumerate(string directory)
    {
        if (!DirectoryExists(directory)) yield break;
        foreach (string path in Directory.EnumerateFileSystemEntries(Resolve(directory)))
        {
            FileAttributes attr = File.GetAttributes(path);
            yield return new(Path.GetFileName(path), (attr & FileAttributes.Directory) != 0 ? 0 : unchecked((int)new FileInfo(path).Length), (int)attr, RtlFileTime.LastWriteTime(path));
        }
    }
}

public interface IClock
{
    DateTime Now { get; }
    DateTime UtcNow => Now.ToUniversalTime();
    uint TickCount { get; }
    double ElapsedMilliseconds { get; }
    void WaitMilliseconds(int milliseconds);
    void WaitUntil(double deadline)
    {
        int remaining = (int)Math.Ceiling(deadline - ElapsedMilliseconds);
        if (remaining > 0) WaitMilliseconds(remaining);
    }
}
public sealed class SystemClock : IClock
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public uint TickCount => unchecked((uint)Environment.TickCount64);
    public double ElapsedMilliseconds => System.Diagnostics.Stopwatch.GetTimestamp() * 1000.0 / System.Diagnostics.Stopwatch.Frequency;
    public void WaitMilliseconds(int milliseconds)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(milliseconds);
        if (milliseconds == 0) Thread.Yield();
        else Thread.Sleep(milliseconds);
    }
}

public struct TSearchRec
{
    private string name;
    public string Name { readonly get => name ?? ""; set => name = value; }
    public int Size, Attr, Time;
    internal RtlFileEntry[]? Entries;
    internal int Index;
}

public static partial class RtlUnit
{
    public static IFileSystem Files { get; set; } = new SystemIoFileSystem();
    public static IClock Clock { get; set; } = new SystemClock();
    public const int faReadOnly = 1, faHidden = 2, faSysFile = 4, faVolumeID = 8, faDirectory = 16, faArchive = 32, faAnyFile = 63;
    public static string GetCurrentDir() => Files.CurrentDirectory;
    public static bool SetCurrentDir(string path) => Files.SetCurrentDirectory(path);
    public static bool FileExists(string name) => Files.FileExists(name);
    public static bool DirectoryExists(string name) => Files.DirectoryExists(name);
    public static bool DeleteFile(string name) => Files.DeleteFile(name);
    public static bool RenameFile(string source, string target) => Files.RenameFile(source, target);
    public static bool CreateDir(string name) => Files.CreateDirectory(name);
    public static bool RemoveDir(string name) => Files.RemoveDirectory(name);
    public static int FileGetAttr(string name) => Files.GetAttributes(name);
    public static int FileSetAttr(string name, int attributes) => Files.SetAttributes(name, attributes);
    // SYSUTILS.PAS FindFirst and FindMatchingFile, with the specified deterministic order.
    public static int FindFirst(string path, int attr, ref TSearchRec search)
    {
        search = default;
        path = path.Replace('/', '\\');
        string directory = ExtractFilePath(path);
        if (directory.Length == 0) directory = Files.CurrentDirectory;
        string pattern = ExtractFileName(path);
        try
        {
            if (!Files.DirectoryExists(directory)) return 3;
            var all = Files.Enumerate(directory).ToList();
            all.Add(new(".", 0, faDirectory, Clock.Now));
            all.Add(new("..", 0, faDirectory, Clock.Now));
            string expression = FileSystemName.TranslateWin32Expression(pattern);
            var matches = all.Where(e => FileSystemName.MatchesWin32Expression(expression, e.Name, true)).ToArray();
            if (matches.Length == 0) return 2;
            search.Entries = matches.Where(e => (e.Attr & ~attr & 30) == 0).OrderBy(e => e.Name.ToUpperInvariant(), StringComparer.Ordinal).ToArray();
            search.Index = -1;
            int result = FindNext(ref search);
            if (result != 0) FindClose(ref search);
            return result;
        }
        catch (DirectoryNotFoundException) { return 3; }
        catch (UnauthorizedAccessException) { return 5; }
    }
    // SYSUTILS.PAS FindNext and FindClose.
    public static int FindNext(ref TSearchRec search)
    {
        if (search.Entries is null) return 6;
        if (++search.Index >= search.Entries.Length) return 18;
        var item = search.Entries[search.Index];
        search.Name = item.Name; search.Size = item.Size; search.Attr = item.Attr; search.Time = DateTimeToFileDate(ToDelphiDate(item.Modified));
        return 0;
    }
    public static void FindClose(ref TSearchRec search) => search.Entries = null;
    public static int FileAge(string name) => Files.LastWriteTime(name) is { } time ? DateTimeToFileDate(ToDelphiDate(time)) : -1;
    // SYSUTILS.PAS FileDateToDateTime and DateTimeToFileDate use DOS two-second resolution.
    public static double FileDateToDateTime(int value) => EncodeDate((int)((uint)value >> 25) + 1980, (value >> 21) & 15, (value >> 16) & 31) + EncodeTime((value >> 11) & 31, (value >> 5) & 63, (value & 31) * 2, 0);
    public static int DateTimeToFileDate(double value)
    {
        DecodeDate(value, out int y, out int m, out int d); DecodeTime(value, out int h, out int n, out int s, out _);
        return y is < 1980 or > 2099 ? 0 : unchecked(((y - 1980) << 25) | (m << 21) | (d << 16) | (h << 11) | (n << 5) | (s >> 1));
    }
    private static readonly DateTime Epoch = new(1899, 12, 30);
    private static double ToDelphiDate(DateTime time)
    {
        double days = (time.Date - Epoch).TotalDays, fraction = time.TimeOfDay.TotalDays;
        return days < 0 ? days - fraction : days + fraction;
    }
    // SYSUTILS.PAS Now, Date and Time.
    public static double Now() => ToDelphiDate(Clock.Now);
    public static double Date() => (Clock.Now.Date - Epoch).TotalDays;
    public static double Time() => Clock.Now.TimeOfDay.TotalDays;
    public static uint GetTickCount() => Clock.TickCount;
    public static uint timeGetTime() => Clock.TickCount;
    // SYSUTILS.PAS EncodeDate and EncodeTime.
    public static double EncodeDate(int year, int month, int day)
    {
        try { return (new DateTime(year, month, day) - Epoch).TotalDays; }
        catch (ArgumentOutOfRangeException) { throw new EConvertError("Invalid argument to date encode"); }
    }
    public static double EncodeTime(int hour, int minute, int second, int millisecond)
    {
        if (hour is < 0 or >= 24 || minute is < 0 or >= 60 || second is < 0 or >= 60 || millisecond is < 0 or >= 1000) throw new EConvertError("Invalid argument to time encode");
        return (hour * 3600000 + minute * 60000 + second * 1000 + millisecond) / 86400000.0;
    }
    // SYSUTILS.PAS DateTimeToTimeStamp rounds to milliseconds and uses the absolute fraction.
    public static void DecodeTime(double value, out int hour, out int minute, out int second, out int millisecond)
    {
        long ticks = (long)Math.Round(value * 86400000, MidpointRounding.ToEven);
        int ms = (int)Math.Abs(ticks % 86400000);
        hour = ms / 3600000; minute = ms / 60000 % 60; second = ms / 1000 % 60; millisecond = ms % 1000;
    }
    public static void DecodeDate(double value, out int year, out int month, out int day)
    {
        long rounded = (long)Math.Round(value * 86400000, MidpointRounding.ToEven);
        long days = rounded / 86400000;
        if (days + (Epoch - DateTime.MinValue).Days < 0) { year = month = day = 0; return; }
        DateTime date = Epoch.AddDays(days); year = date.Year; month = date.Month; day = date.Day;
    }
    public static void DecodeTime(double value, ref ushort hour, ref ushort minute, ref ushort second, ref ushort millisecond)
    {
        DecodeTime(value, out int h, out int n, out int s, out int ms);
        hour = (ushort)h; minute = (ushort)n; second = (ushort)s; millisecond = (ushort)ms;
    }
    public static void DecodeDate(double value, ref ushort year, ref ushort month, ref ushort day)
    {
        DecodeDate(value, out int y, out int m, out int d);
        year = (ushort)y; month = (ushort)m; day = (ushort)d;
    }
    public static string DateTimeToStr(double value)
    {
        DecodeDate(value, out int y, out int m, out int d); DecodeTime(value, out int h, out int n, out int s, out int ms);
        return new DateTime(y, m, d, h, n, s, ms).ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
    }
}
