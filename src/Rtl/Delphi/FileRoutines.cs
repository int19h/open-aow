using static Aow.Rtl.DelphiDateTime;
using static Aow.Rtl.RtlHost;

namespace Aow.Rtl;

public static class DelphiFiles
{
    public const int faReadOnly = 1, faHidden = 2, faDirectory = 16, faAnyFile = 63;

    public static int FileAge(string name) => Files.LastWriteTime(name) is { } time ? FileDate(time) : -1;
}
