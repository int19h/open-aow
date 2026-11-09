using System.IO.Compression;

namespace Aow.Rtl;

// The byte values the engine stores beside a zlib payload.
internal static class ZLibLevels
{
    internal static CompressionLevel ForByte(byte level) => level switch
    {
        0 => CompressionLevel.NoCompression,
        1 => CompressionLevel.Fastest,
        3 => CompressionLevel.SmallestSize,
        _ => CompressionLevel.Optimal,
    };
}
