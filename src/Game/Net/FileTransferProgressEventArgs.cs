namespace Aow.Game.Net;

/// <summary>The progress of a DirectPlay file transfer (issue 326).</summary>
public sealed class FileTransferProgressEventArgs : EventArgs
{
    public required int Id { get; init; }

    public required int Position { get; init; }

    public required int Size { get; init; }
}
