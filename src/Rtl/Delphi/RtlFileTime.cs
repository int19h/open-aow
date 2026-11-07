using System.Runtime.InteropServices;

namespace Aow.Rtl;

public static class RtlFileTime
{
    // Delphi 3 FindMatchingFile uses FileTimeToLocalFileTime before DOS packing.
    public static DateTime LastWriteTime(string path)
    {
        // FileTimeToLocalFileTime applies the current bias, not the bias in force at the stamp.
        if (!OperatingSystem.IsWindows())
        {
            DateTime stamp = File.GetLastWriteTimeUtc(path);
            return DateTime.SpecifyKind(stamp + TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow), DateTimeKind.Unspecified);
        }
        long utc = File.GetLastWriteTimeUtc(path).ToFileTimeUtc();
        if (!FileTimeToLocalFileTime(in utc, out long local)) throw new IOException("File time conversion failed.");
        return DateTime.SpecifyKind(DateTime.FromFileTimeUtc(local), DateTimeKind.Unspecified);
    }

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FileTimeToLocalFileTime(in long fileTime, out long localFileTime);
}
