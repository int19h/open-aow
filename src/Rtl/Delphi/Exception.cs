namespace Aow.Rtl;

public sealed class HaltException(int exitCode) : Exception
{
    public int ExitCode { get; } = exitCode;
}
