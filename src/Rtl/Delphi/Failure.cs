namespace Aow.Rtl;

public static partial class RtlUnit
{
    public static void Halt(int exitCode = 0) => throw new EHalt(exitCode);
    public static void Abort() => throw EAbort.Create();

    public static T? As<T>(object? value) where T : class => value is null ? null : value as T
        ?? throw new EInvalidCast("Invalid class typecast");

    public static Exception Wrap(System.Exception value) => value switch
    {
        Exception existing => existing,
        NullReferenceException => new EAccessViolation(value.Message),
        DivideByZeroException => new EDivByZero(value.Message),
        IndexOutOfRangeException => new ERangeError(value.Message),
        InvalidCastException => new EInvalidCast(value.Message),
        _ => new Exception(value.Message)
    };
}
