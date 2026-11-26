namespace Aow.Game.Net;

/// <summary>A player that left the DirectPlay session (issue 326).</summary>
public sealed class PlayerDestroyedEventArgs : EventArgs
{
    public required int Id { get; init; }

    public required int PlayerCount { get; init; }
}
