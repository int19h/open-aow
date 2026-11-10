namespace Aow.Rtl;

public sealed class Application
{
    public string ExeName { get; set; } = Environment.ProcessPath ?? "";
}

public static partial class RtlUnit
{
    public static global::Aow.Rtl.Application Application { get; } = new();
    public static Action<string> MessageHandler { get; set; } = message => throw new InvalidOperationException(message);

    public static void ShowMessage(string message) => MessageHandler(message);
}
