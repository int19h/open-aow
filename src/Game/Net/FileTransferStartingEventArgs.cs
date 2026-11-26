namespace Aow.Game.Net;

/// <summary>A file transfer the DirectPlay session is about to start (issue 326).</summary>
public sealed class FileTransferStartingEventArgs : EventArgs
{
    public required int Id { get; init; }

    public required string Name { get; init; }

    public required int Size { get; init; }

    public required byte Response { get; init; }
}
