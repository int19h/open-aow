namespace Aow.Rtl;

public static partial class RtlUnit
{
    private static int Delimiter(string name, bool dot = false)
    {
        for (int i = name.Length; i > 0; i--)
            if (name[i - 1] is '\\' or ':' || (dot && name[i - 1] == '.')) return i;
        return 0;
    }

    public static string ExtractFileName(string? name)
    {
        name = RequireString(name);
        return name[Delimiter(name)..];
    }

    public static string ExtractFileDrive(string? name)
    {
        name = RequireString(name);
        if (name.Length >= 2 && name[1] == ':') return name[..2];
        if (name.Length < 2 || name[0] != '\\' || name[1] != '\\') return "";
        int i = 3, separators = 0;
        while (i < name.Length && separators < 2)
        {
            if (name[i - 1] == '\\') separators++;
            if (separators < 2) i++;
        }
        if (i <= name.Length && name[i - 1] == '\\') i--;
        return Copy(name, 1, i);
    }

    public static string ExtractFilePath(string? name)
    {
        name = RequireString(name);
        return name[..Delimiter(name)];
    }

    public static string ExtractFileDir(string? name)
    {
        name = RequireString(name);
        int i = Delimiter(name);
        if (i > 1 && name[i - 1] == '\\' && name[i - 2] is not ('\\' or ':')) i--;
        return name[..i];
    }

    public static string ExtractFileExt(string? name)
    {
        name = RequireString(name);
        int i = Delimiter(name, true);
        return i > 0 && name[i - 1] == '.' ? name[(i - 1)..] : "";
    }

    public static string ChangeFileExt(string? name, string? extension)
    {
        name = RequireString(name);
        int i = Delimiter(name, true);
        return (i > 0 && name[i - 1] == '.' ? name[..(i - 1)] : name) + RequireString(extension);
    }

    public static string ExpandFileName(string? name)
    {
        name = RequireString(name);
        return name.Length == 0 ? "" : Files.ExpandPath(name);
    }

    public static string ExtractRelativePath(string baseName, string destination)
        => Files.ExtractRelativePath(baseName, destination);
}
