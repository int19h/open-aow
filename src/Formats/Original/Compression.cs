using System.IO.Compression;

namespace Aow.Formats.Original;

/// <summary>The CFS container of maps and saves (persistence.md 1.1): CFS\0, a level byte, then
/// the payload, raw for level 0 and a zlib stream otherwise.</summary>
public static class Cfs
{
    public static ReadOnlySpan<byte> Magic => "CFS\0"u8;

    public static bool HasMagic(ReadOnlySpan<byte> file) => file.Length >= 5 && file[..4].SequenceEqual(Magic);

    /// <summary>Inflates a zlib stream, refusing more than the limit before allocating it.</summary>
    public static byte[] Inflate(ReadOnlySpan<byte> zlib, long limit, Func<string, Exception> fail)
    {
        using var input = new MemoryStream(zlib.ToArray(), false);
        using var z = new ZLibStream(input, CompressionMode.Decompress);
        using var output = new MemoryStream();
        var buffer = new byte[81920];
        try
        {
            int read;
            while ((read = z.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (output.Length + read > limit) throw fail($"The inflated payload exceeds {limit} bytes.");
                output.Write(buffer, 0, read);
            }
        }
        catch (InvalidDataException error)
        {
            throw fail("The compressed stream is corrupt: " + error.Message);
        }
        return output.ToArray();
    }

    /// <summary>Inflates at most count leading bytes.</summary>
    public static byte[] InflatePrefix(ReadOnlySpan<byte> zlib, int count)
    {
        using var input = new MemoryStream(zlib.ToArray(), false);
        using var z = new ZLibStream(input, CompressionMode.Decompress);
        var buffer = new byte[count];
        int total = 0, read;
        while (total < count && (read = z.Read(buffer, total, count - total)) > 0) total += read;
        return buffer[..total];
    }

    public static byte[] Deflate(ReadOnlySpan<byte> payload, int level)
    {
        using var output = new MemoryStream();
        using (var z = new ZLibStream(output, level switch
        {
            1 => CompressionLevel.Fastest,
            3 => CompressionLevel.SmallestSize,
            _ => CompressionLevel.Optimal,
        }, true))
            z.Write(payload);
        return output.ToArray();
    }

    /// <summary>The five-byte prefix and the payload, compressed for levels 1 to 3.</summary>
    public static byte[] Wrap(ReadOnlySpan<byte> payload, int level)
    {
        var body = level == 0 ? payload.ToArray() : Deflate(payload, level);
        var result = new byte[5 + body.Length];
        Magic.CopyTo(result);
        result[4] = (byte)level;
        body.CopyTo(result, 5);
        return result;
    }
}

/// <summary>The CRC-32 of the original (Engine.GetCRC32), the reflected 0xEDB88320 polynomial.</summary>
public static class Crc32
{
    private static readonly uint[] s_table = Make();

    private static uint[] Make()
    {
        var table = new uint[256];
        for (uint i = 0; i < 256; i++)
        {
            uint c = i;
            for (int k = 0; k < 8; k++) c = (c & 1) != 0 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
            table[i] = c;
        }
        return table;
    }

    public static uint Compute(ReadOnlySpan<byte> data)
    {
        uint crc = 0xFFFFFFFFu;
        foreach (byte b in data) crc = s_table[(b ^ crc) & 0xFF] ^ (crc >> 8);
        return crc ^ 0xFFFFFFFFu;
    }
}
