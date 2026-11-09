namespace Aow.Rtl;

public sealed class TApplication
{
    public string ExeName { get; set; } = Environment.ProcessPath ?? "";
}

public static partial class RtlUnit
{
    public static TApplication Application { get; } = new();
    public static Action<string> MessageHandler { get; set; } = message => throw new InvalidOperationException(message);

    public static void ShowMessage(string message) => MessageHandler(message);
}
