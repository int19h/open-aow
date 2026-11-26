namespace Aow.Game.Net;

/// <summary>A message the DirectPlay session received (issue 326).</summary>
public sealed class MessageReceivedEventArgs : EventArgs
{
    public required object Data { get; init; }

    public required int Size { get; init; }

    public required int ToId { get; init; }

    public required int FromId { get; init; }
}
