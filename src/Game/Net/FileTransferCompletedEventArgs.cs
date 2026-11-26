using Aow.Rtl;

namespace Aow.Game.Net;

/// <summary>A DirectPlay file transfer that completed (issue 326).</summary>
public sealed class FileTransferCompletedEventArgs : EventArgs
{
    public required int Id { get; init; }

    public required GDFile File { get; init; }
}
