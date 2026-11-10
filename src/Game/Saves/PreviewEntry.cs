#nullable enable
using Aow.Formats;

namespace Aow.Game.Saves;

public sealed record PreviewEntry(byte[] Image, GameFileReport Report);
