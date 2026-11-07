using Aow.Rtl;

namespace Aow.Platform;

/// <summary>The install is a snapshot. All mutations target the user layer.</summary>
public sealed class LayeredFileSystem : IFileSystem
{
    private readonly string root, user;
    private readonly Dictionary<string, string> rootTree = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, string[]> userListings = new(StringComparer.OrdinalIgnoreCase);
    private string current = "";
    private readonly string[] userOnly;

    public LayeredFileSystem(string gameRoot, string userDirectory, IEnumerable<string>? userOnlyDirectories = null)
    {
        userOnly = userOnlyDirectories?.Select(p => p.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar)).ToArray() ?? [];
        root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(gameRoot));
        user = Path.TrimEndingDirectorySeparator(Path.GetFullPath(userDirectory));
        if (!Directory.Exists(root)) throw new DirectoryNotFoundException(root);
        RejectLink(root);
        if (!Same(root, user) && (Within(root, user) || Within(user, root)))
            throw new ArgumentException("The two layers must be separate directories or the same directory.");
        Snapshot(root, "");
        EnsureDirectory(user);
    }

    public string GameRoot => root;
    public string UserDirectory => user;
    public string VirtualDrive => OperatingSystem.IsWindows() ? Path.GetPathRoot(root)![..2] : "C:";
    public string CurrentDirectory => PublicPath(current);

    private string PublicPath(string relative) => OperatingSystem.IsWindows() ? Path.Combine(root, relative) : VirtualDrive + "\\" + relative.Replace(Path.DirectorySeparatorChar, '\\');
    private static bool Same(string a, string b) => string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
    private static bool Within(string parent, string child) => child.StartsWith(Path.TrimEndingDirectorySeparator(parent) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase);

    private static void RejectLink(string path)
    {
        if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != 0)
            throw new UnauthorizedAccessException("Links are outside the virtual drive: " + path);
    }

    private static void EnsureDirectory(string path)
    {
        string? parent = Path.GetDirectoryName(path);
        if (parent is not null) EnsureDirectory(parent);
        if (Directory.Exists(path)) { RejectLink(path); return; }
        Directory.CreateDirectory(path);
    }

    private void Snapshot(string directory, string relative)
    {
        rootTree[relative] = directory;
        foreach (string entry in Directory.EnumerateFileSystemEntries(directory).Order(StringComparer.Ordinal))
        {
            if ((File.GetAttributes(entry) & FileAttributes.ReparsePoint) != 0) continue;
            string key = Path.Combine(relative, Path.GetFileName(entry));
            if (userOnly.Any(p => Same(p, key))) continue;
            if (!rootTree.TryAdd(key, entry)) throw new IOException("Ambiguous install name: " + key);
            if (Directory.Exists(entry)) Snapshot(entry, key);
        }
    }

    private string Relative(string path)
    {
        path = path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        if (Same(path, root)) return "";
        if (Within(root, path)) path = path[(root.Length + 1)..];
        else if (Same(path, user)) return "";
        else if (Within(user, path)) path = path[(user.Length + 1)..];
        else if (path.Length >= 2 && path[1] == ':')
        {
            if (!Same(path[..2], VirtualDrive)) throw new UnauthorizedAccessException("Unknown virtual drive.");
            path = path[2..].TrimStart(Path.DirectorySeparatorChar);
        }
        else if (Path.IsPathRooted(path))
        {
            path = path.TrimStart(Path.DirectorySeparatorChar);
        }
        else path = Path.Combine(current, path);
        var parts = new List<string>();
        foreach (string part in path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
        {
            if (part == ".") continue;
            if (part == "..") { if (parts.Count > 0) parts.RemoveAt(parts.Count - 1); continue; }
            if (part.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0 || part.Contains(':')) throw new ArgumentException("Invalid path component.");
            parts.Add(part);
        }
        return Path.Combine([.. parts]);
    }

    public string ExpandPath(string path) => PublicPath(Relative(path));

    public string ExtractRelativePath(string baseFile, string destination)
    {
        // SysUtils: an empty base has no drive and no path, so the destination comes back unchanged.
        if (baseFile.Length == 0) return destination;
        static bool DirectoryPath(string path) => path.EndsWith('\\') || path.EndsWith('/');
        string[] first = Relative(baseFile).Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        string[] second = Relative(destination).Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        int baseCount = DirectoryPath(baseFile) ? first.Length : Math.Max(0, first.Length - 1);
        int destinationCount = DirectoryPath(destination) ? second.Length : Math.Max(0, second.Length - 1);
        int common = 0;
        while (common < baseCount && common < destinationCount && Same(first[common], second[common])) common++;
        var parts = Enumerable.Repeat("..", baseCount - common).Concat(second.Skip(common).Take(destinationCount - common));
        if (!DirectoryPath(destination) && second.Length > 0) parts = parts.Append(second[^1]);
        string result = string.Join("\\", parts);
        return DirectoryPath(destination) && result.Length > 0 ? result + "\\" : result;
    }

    public bool SetCurrentDirectory(string path)
    {
        string relative = Relative(path);
        string? found = ReadLocation(relative);
        if (found is null || !Directory.Exists(found)) return false;
        current = relative;
        return true;
    }

    private string? UserLocation(string relative)
    {
        string location = user;
        foreach (string part in relative.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
        {
            if (!Directory.Exists(location)) return null;
            if (!userListings.TryGetValue(location, out var entries))
                userListings[location] = entries = Directory.GetFileSystemEntries(location).Order(StringComparer.Ordinal).ToArray();
            string? match = entries.FirstOrDefault(e => Same(Path.GetFileName(e), part));
            if (match is null) return null;
            RejectLink(match);
            location = match;
        }
        return location;
    }

    private string? ReadLocation(string relative)
    {
        string ancestor = relative;
        while (ancestor.Length > 0)
        {
            ancestor = Path.GetDirectoryName(ancestor) ?? "";
            string? overlay = UserLocation(ancestor);
            if (overlay is not null && !Directory.Exists(overlay)) return null;
        }
        return UserLocation(relative) ?? (Same(root, user) ? null : rootTree.GetValueOrDefault(relative));
    }

    private string WriteLocation(string relative)
    {
        string location = user;
        foreach (string part in relative.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
        {
            string? match = Directory.Exists(location) ? Directory.EnumerateFileSystemEntries(location).Order(StringComparer.Ordinal).FirstOrDefault(e => Same(Path.GetFileName(e), part)) : null;
            location = match ?? Path.Combine(location, part);
            if (match is not null) RejectLink(match);
        }
        return location;
    }

    public Stream Open(string path, int mode)
    {
        string relative = Relative(path);
        bool create = mode == RtlUnit.fmCreate;
        FileAccess access = create ? FileAccess.ReadWrite : (mode & 3) switch { 0 => FileAccess.Read, 1 => FileAccess.Write, 2 => FileAccess.ReadWrite, _ => throw new ArgumentException("Invalid open mode.") };
        string target;
        if (access == FileAccess.Read) target = ReadLocation(relative) ?? throw new FileNotFoundException(path);
        else
        {
            target = WriteLocation(relative);
            EnsureDirectory(Path.GetDirectoryName(target)!);
            if (!create && !File.Exists(target))
            {
                string source = ReadLocation(relative) ?? throw new FileNotFoundException(path);
                File.Copy(source, target);
                File.SetAttributes(target, File.GetAttributes(target) & ~FileAttributes.ReadOnly);
            }
            userListings.Clear();
        }
        return new FileStream(target, create ? FileMode.Create : FileMode.Open, access,
            (mode & 0xF0) == RtlUnit.fmShareDenyWrite ? FileShare.Read : FileShare.ReadWrite);
    }

    public bool FileExists(string path) => ReadLocation(Relative(path)) is { } p && File.Exists(p);
    public bool DirectoryExists(string path) => ReadLocation(Relative(path)) is { } p && Directory.Exists(p);

    private bool Mutate(Action action)
    {
        try { action(); return true; }
        catch (System.Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException) { return false; }
        finally { userListings.Clear(); }
    }

    public bool DeleteFile(string path) => Mutate(() =>
    {
        string target = WriteLocation(Relative(path));
        if (!File.Exists(target)) throw new FileNotFoundException(path);
        File.Delete(target);
    });

    public bool RenameFile(string source, string target) => Mutate(() =>
    {
        string destination = WriteLocation(Relative(target));
        EnsureDirectory(Path.GetDirectoryName(destination)!);
        File.Move(WriteLocation(Relative(source)), destination);
    });

    // Both paths resolve in the user layer, so the move stays on one volume and replaces the
    // user copy in one step. A shipped file is shadowed, never written.
    public bool ReplaceFile(string source, string target) => Mutate(() =>
    {
        string from = WriteLocation(Relative(source));
        if (!File.Exists(from)) throw new FileNotFoundException(source);
        string destination = WriteLocation(Relative(target));
        EnsureDirectory(Path.GetDirectoryName(destination)!);
        File.Move(from, destination, true);
    });

    public string Layer(string path)
    {
        string relative = Relative(path);
        string? found = ReadLocation(relative);
        if (found is null) return "none";
        return Same(root, user) ? "single" : Within(user, found) || Same(user, found) ? "user" : "root";
    }

    public bool CreateDirectory(string path) => !DirectoryExists(path) && Mutate(() => EnsureDirectory(WriteLocation(Relative(path))));

    public bool RemoveDirectory(string path) => Mutate(() =>
    {
        string relative = Relative(path);
        if (relative.Length == 0) throw new UnauthorizedAccessException("The virtual drive cannot be removed.");
        Directory.Delete(WriteLocation(relative));
    });

    public int GetAttributes(string path) => ReadLocation(Relative(path)) is { } p ? (int)File.GetAttributes(p) : -1;
    public int SetAttributes(string path, int attributes) => Mutate(() => File.SetAttributes(WriteLocation(Relative(path)), (FileAttributes)attributes)) ? 0 : -1;
    public DateTime? LastWriteTime(string path) => ReadLocation(Relative(path)) is { } p ? RtlFileTime.LastWriteTime(p) : null;

    public IEnumerable<RtlFileEntry> Enumerate(string directory)
    {
        string relative = Relative(directory);
        if (!DirectoryExists(directory)) return [];
        var merged = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var pair in Same(root, user) ? [] : rootTree)
            if (pair.Key.Length > 0 && Same(Path.GetDirectoryName(pair.Key) ?? "", relative)) merged[Path.GetFileName(pair.Key)] = pair.Value;
        if (UserLocation(relative) is { } overlay && Directory.Exists(overlay))
            foreach (string entry in Directory.EnumerateFileSystemEntries(overlay).Order(StringComparer.Ordinal))
            {
                if ((File.GetAttributes(entry) & FileAttributes.ReparsePoint) != 0) continue;
                merged[Path.GetFileName(entry)] = entry;
            }
        return merged.Values.Select(p => new RtlFileEntry(Path.GetFileName(p), Directory.Exists(p) ? 0 : unchecked((int)new FileInfo(p).Length), (int)File.GetAttributes(p), RtlFileTime.LastWriteTime(p)))
            .OrderBy(e => e.Name.ToUpperInvariant(), StringComparer.Ordinal).ThenBy(e => e.Name, StringComparer.Ordinal).ToArray();
    }
}
