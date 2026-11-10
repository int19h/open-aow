namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static void Halt(int exitCode = 0) => throw new HaltException(exitCode);
    public static void Abort() => throw AbortException.Create();

    public static T? As<T>(object? value) where T : class => value is null ? null : value as T
        ?? throw new InvalidCastException("Invalid class typecast");
}
