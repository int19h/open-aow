namespace Aow.Rtl;

public sealed class HaltException(int exitCode) : System.Exception
{
    public int ExitCode { get; } = exitCode;
}
