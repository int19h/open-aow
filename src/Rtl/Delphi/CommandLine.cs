namespace Aow.Rtl;

public static partial class RtlUnit
{
    private static string[] arguments = Environment.GetCommandLineArgs().Select(s => s).ToArray();

    // Until IRtlHost is introduced, the host supplies argument zero and the arguments here.
    public static void SetCommandLine(params string[] values)
    {
        if (values.Length == 0) throw new ArgumentException("Argument zero is the executable name.", nameof(values));
        arguments = (string[])values.Clone();
    }

    public static int ParamCount() => arguments.Length - 1;
    public static string ParamStr(int index) => index >= 0 && index < arguments.Length ? arguments[index] : "";
}
