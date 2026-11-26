namespace Aow.Game.Net;

/// <summary>The round trip a DirectPlay ping measured (issue 326).</summary>
public sealed class PingEventArgs : EventArgs
{
    public required int Milliseconds { get; init; }

    public required int Player { get; init; }
}
