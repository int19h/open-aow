using System.ComponentModel;
using Aow.Engine;

using Aow.Game.Saves;
using Aow.Game.World;

namespace Aow.Game.Tests;

internal static class PortedExceptions
{
    // The construction sweeps caught the Delphi exception base. This is that set after the
    // stand-ins with a .NET counterpart were replaced. Runtime failures stay outside it.
    internal static bool Is(Exception error) =>
        error is not NullStringException
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
            or Win32Exception
            or AbstractErrorException
            or AssertionFailedException
            or AbortException
            or HaltException
            or GameFileErrorException
            or DoublePrioriyIDException
            or PlugInMissingException
            or MapObjectResourceMissingException);
}
