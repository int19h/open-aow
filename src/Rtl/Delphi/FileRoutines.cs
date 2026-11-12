namespace Aow.Rtl;

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
    public static int FileAge(string name) => Files.LastWriteTime(name) is { } time ? FileDate(time) : -1;
}
