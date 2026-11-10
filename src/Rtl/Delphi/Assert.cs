namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static void Assert(bool condition, string? message, string file, int line)
    {
        message = RequireString(message);
        if (!condition) throw new AssertionFailedException($"{(string.IsNullOrEmpty(message) ? "Assertion failed" : message)} ({file}, line {line})");
    }
}
