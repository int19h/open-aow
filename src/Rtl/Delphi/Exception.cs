namespace Aow.Rtl;

public sealed class EHalt(int exitCode) : System.Exception
{
    public int ExitCode { get; } = exitCode;
}
