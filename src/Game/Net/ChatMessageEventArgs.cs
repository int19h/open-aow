namespace Aow.Game.Net;

/// <summary>A chat message the DirectPlay session received (issue 326).</summary>
public sealed class ChatMessageEventArgs : EventArgs
{
    public required string Message { get; init; }

    public required int ToId { get; init; }

    public required int FromId { get; init; }
}
