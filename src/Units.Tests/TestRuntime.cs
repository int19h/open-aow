using System.Runtime.CompilerServices;

namespace Aow.Units.Tests;

internal static class TestRuntime
{
    [ModuleInitializer]
    internal static void Initialize()
    {
        ProcessSafety.Initialize();
        Runtime.StrictStrings = true;
    }
}
