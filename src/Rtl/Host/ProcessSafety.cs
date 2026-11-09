using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl.Host;

public static class ProcessSafety
{
#pragma warning disable CA2255
    [ModuleInitializer]
    public static void Initialize()
    {
        if (OperatingSystem.IsWindows()) SetErrorMode(0x8003);
        // The module initializer runs before any object. Fixture and smoke captures then share one construction order.
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 1; i < args.Length; i++)
        {
            if (args[i] is "--fixture" or "--smoke-test")
            {
                CaptureAddress.PinAllocationOrder();
                return;
            }
        }
    }

#pragma warning restore CA2255

    [DllImport("kernel32.dll")]
    private static extern uint SetErrorMode(uint mode);
}
