namespace Aow.Game.Net;

/// <summary>The progress of the mail sender (issue 326).</summary>
public sealed class SmtpProgressEventArgs : EventArgs
{
    public required int Progress { get; init; }

    public required int Total { get; init; }
}
