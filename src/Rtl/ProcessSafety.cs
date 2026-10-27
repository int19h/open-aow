using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl;

public static class ProcessSafety
{
#pragma warning disable CA2255
    [ModuleInitializer]
    public static void Initialize()
    {
        if (OperatingSystem.IsWindows()) SetErrorMode(0x8003);
    }
#pragma warning restore CA2255

    [DllImport("kernel32.dll")]
    private static extern uint SetErrorMode(uint mode);
}
