using Aow.Game.Engine;
using Aow.Game.Saves;
using Aow.Game.World;

namespace Aow.Units.Tests;

internal static class PortedExceptions
{
    // The construction sweeps caught the Delphi exception base. This is that set after the
    // stand-ins with a .NET counterpart were replaced. Runtime failures stay outside it.
    internal static bool Is(System.Exception error) =>
        error is not Aow.Rtl.NullStringException
        && error is (
            ArgumentException
            or InvalidOperationException
            or FormatException
            or IOException
            or NotSupportedException
            or InvalidCastException
            or ArithmeticException
            or AccessViolationException
            or KeyNotFoundException
            or OutOfMemoryException
            or System.ComponentModel.Win32Exception
            or Aow.Rtl.EAbstractError
            or Aow.Rtl.EAssertionFailed
            or Aow.Rtl.EAbort
            or Aow.Rtl.EHalt
            or EGameFileError
            or EDoublePrioriyID
            or EPlugInMissing
            or EMapObjectResourceMissing);
}
