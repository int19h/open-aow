namespace Aow.Rtl;

public sealed class DelphiApplication
{
    public string ExeName { get; set; } = Environment.ProcessPath ?? "";
}

public static class LegacyApplication
{
    public static DelphiApplication Application { get; } = new();
    public static Action<string> MessageHandler { get; set; } = message => throw new InvalidOperationException(message);

    public static void ShowMessage(string message) => MessageHandler(message);
}
