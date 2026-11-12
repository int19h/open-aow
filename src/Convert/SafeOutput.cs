namespace Aow.Convert;

/// <summary>
/// Output rules of the CLI (own-formats.md 3.2): an existing destination is refused without
/// --overwrite, the input is never the output under any alias, directory trees do not overlap,
/// and a destination is replaced only after the complete serialization, through a sibling
/// temporary file.
/// </summary>
public static class SafeOutput
{
    private static readonly StringComparison s_pathComparison =
        OperatingSystem.IsWindows() || OperatingSystem.IsMacOS() ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

    /// <summary>The full path with every symbolic link along it resolved.</summary>
    public static string Resolve(string path)
    {
        string full = Path.GetFullPath(path);
        var root = Path.GetPathRoot(full) ?? "";
        string current = root;
        foreach (var part in full[root.Length..].Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries))
        {
            current = Path.Combine(current, part);
            FileSystemInfo info = Directory.Exists(current) ? new DirectoryInfo(current) : new FileInfo(current);
            if (info.Exists && info.LinkTarget is not null && info.ResolveLinkTarget(true) is { } target)
                current = target.FullName;
        }
        return current.TrimEnd(Path.DirectorySeparatorChar);
    }

    public static bool SamePath(string a, string b) => string.Equals(Resolve(a), Resolve(b), s_pathComparison);

    /// <summary>Whether one directory tree contains the other.</summary>
    public static bool Overlaps(string a, string b)
    {
        string x = Resolve(a) + Path.DirectorySeparatorChar, y = Resolve(b) + Path.DirectorySeparatorChar;
        return x.StartsWith(y, s_pathComparison) || y.StartsWith(x, s_pathComparison);
    }

    public static string? CheckFile(string input, string output, bool overwrite)
    {
        if (SamePath(input, output)) return $"The output {output} is the input file.";
        if (Directory.Exists(output)) return $"The output {output} is a directory.";
        if (File.Exists(output) && !overwrite) return $"The output {output} exists. Use --overwrite to replace it.";
        return null;
    }

    /// <summary>Writes a sibling temporary, then moves it over the destination in one step.</summary>
    public static void Replace(string output, byte[] bytes)
    {
        string full = Path.GetFullPath(output);
        string directory = Path.GetDirectoryName(full) ?? ".";
        Directory.CreateDirectory(directory);
        string temporary = Path.Combine(directory, "." + Path.GetFileName(full) + "." + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            using (var stream = new FileStream(temporary, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            {
                stream.Write(bytes);
                stream.Flush(true);
            }
            File.Move(temporary, full, true);
        }
        finally
        {
            if (File.Exists(temporary)) File.Delete(temporary);
        }
    }
}
