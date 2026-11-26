namespace Aow.Game.Net;

/// <summary>An error the mail sender reports (issue 326).</summary>
public sealed class SmtpErrorEventArgs : EventArgs
{
    public required string Error { get; init; }
}
