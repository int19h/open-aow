namespace Aow.Game.Net;

/// <summary>A DirectPlay message that could not be sent (issue 326).</summary>
public sealed class MessageFailedEventArgs : EventArgs
{
    public required int Id { get; init; }

    public required int Result { get; init; }
}
