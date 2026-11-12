using System.IO.Enumeration;

namespace Aow.Rtl.Host;

public static class FileSearch
{
    // The entries of a directory whose names match the wildcard pattern at the end of the path,
    // as SYSUTILS.PAS FindFirst and FindNext list them, in the specified deterministic order:
    // the "." and ".." entries included, an entry left out when it has a hidden, system, volume
    // or directory attribute that the attributes do not include, sorted by the upper-cased name.
    // A missing or unreadable directory lists nothing. The list is taken in full before the
    // caller sees the first entry.
    public static IReadOnlyList<RtlFileEntry> FindEntries(this IFileSystem files, string path, int attributes)
    {
        path = path.Replace('/', '\\');
        string directory = RtlUnit.ExtractFilePath(path);
        if (directory.Length == 0) directory = files.CurrentDirectory;
        string pattern = RtlUnit.ExtractFileName(path);
        try
        {
            if (!files.DirectoryExists(directory)) return [];
            var all = files.Enumerate(directory).ToList();
            all.Add(new(Name: ".", Size: 0, Attribute: RtlUnit.faDirectory, Modified: RtlUnit.Clock.Now));
            all.Add(new(Name: "..", Size: 0, Attribute: RtlUnit.faDirectory, Modified: RtlUnit.Clock.Now));
            string expression = FileSystemName.TranslateWin32Expression(pattern);
            return [.. all
                .Where(entry => FileSystemName.MatchesWin32Expression(expression, entry.Name, ignoreCase: true))
                .Where(entry => (entry.Attribute & ~attributes & 30) == 0)
                .OrderBy(entry => entry.Name.ToUpperInvariant(), StringComparer.Ordinal)];
        }
        catch (System.Exception error) when (error is DirectoryNotFoundException or UnauthorizedAccessException)
        {
            return [];
        }
    }
}
