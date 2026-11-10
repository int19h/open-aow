using System.IO.Enumeration;
using Aow.Rtl.Host;

namespace Aow.Rtl;

public struct SearchRec
{
    private string name;

    public string Name { readonly get => name ?? ""; set => name = value; }

    public int Size, Attr, Time;
    internal RtlFileEntry[]? Entries;
    internal int Index;
}

public static partial class RtlUnit
{
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
    public static int FindFirst(string path, int attr, ref SearchRec search)
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
    public static int FindNext(ref SearchRec search)
    {
        if (search.Entries is null) return 6;
        if (++search.Index >= search.Entries.Length) return 18;
        var item = search.Entries[search.Index];
        search.Name = item.Name; search.Size = item.Size; search.Attr = item.Attr; search.Time = DateTimeToFileDate(ToDelphiDate(item.Modified));
        return 0;
    }

    public static void FindClose(ref SearchRec search) => search.Entries = null;
    public static int FileAge(string name) => Files.LastWriteTime(name) is { } time ? DateTimeToFileDate(ToDelphiDate(time)) : -1;
}
