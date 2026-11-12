namespace Aow.Rtl.Host;

public readonly record struct RtlFileEntry(string Name, int Size, int Attribute, DateTime Modified)
{
    // The DOS date and time of Modified, the time SYSUTILS.PAS FindFirst reports.
    public int FileDate => RtlUnit.FileDate(Modified);
}

// The host brief replaces these defaults through Rtl.Files and Rtl.Clock.
public interface IFileSystem
{
    string CurrentDirectory { get; }
    string VirtualDrive => Path.GetPathRoot(ExpandPath("."))?.TrimEnd('\\', '/') ?? "";

    string ExtractRelativePath(string baseFile, string destination)
    {
        // SysUtils: an empty base has no drive and no path, so the destination comes back unchanged.
        if (baseFile.Length == 0) return destination;
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

public sealed class PhysicalFileSystem : IFileSystem
{
    private string? _currentDirectory;

    public string CurrentDirectory => _currentDirectory ?? Directory.GetCurrentDirectory();

    public bool SetCurrentDirectory(string path)
    {
        string resolved = Resolve(path);
        if (!Directory.Exists(resolved)) return false;
        _currentDirectory = resolved;
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
            FileAttributes attribute = File.GetAttributes(path);
            yield return new(Path.GetFileName(path), (attribute & FileAttributes.Directory) != 0 ? 0 : unchecked((int)new FileInfo(path).Length), (int)attribute, RtlFileTime.LastWriteTime(path));
        }
    }
}
