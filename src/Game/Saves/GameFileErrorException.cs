#nullable enable

namespace Aow.Game.Saves;

/// <summary>A refused open or write at the file boundary. It carries the shared diagnostic.</summary>
public sealed class GameFileErrorException(FileLogRecord record) : Exception(record.ToString())
{
    public FileLogRecord Record { get; } = record;
}
