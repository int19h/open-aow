#nullable disable
// Translated from Engine.pas.
using System.IO.Compression;

using Aow.Rtl;

using static Aow.Rtl.DelphiRounding;
using static Aow.Rtl.DelphiStreams;

namespace Aow.Engine.Storage;

public class CompressStream : MemoryStream
{

    // Engine.pas:1715
    public void LoadFromStream(Stream stream)
    {
        bool compressed = default;
        Span<byte> magic = stackalloc byte[4];
        stream.ReadExactly(magic);
        stream.ReadExactly(Bytes.Of(ref compressed, 1));
        if (!magic.SequenceEqual(CompressMagic))
        {
            compressed = false;
            _ = stream.Seek(0, SeekOrigin.Begin);
        }

        if (compressed)
        {
            byte[] buffer = default;
            Array.Resize(ref buffer, 8096);
            ZLibStream d = new(stream, CompressionMode.Decompress, true);
            try
            {
                int count = -1;
                while (count != 0)
                {
                    count = d.Read(buffer.AsSpan(0, 8096));
                    if (count != 0)
                    {
                        this.WriteBuffer(buffer.AsSpan(0, count));
                    }
                }
            }
            finally
            {
                Array.Resize(ref buffer, 0);
                d?.Dispose();
            }
        }
        else
        {
            _ = this.CopyFrom(stream, (int)(stream.Length - stream.Position));
        }

        Position = 0;
    }

    // Engine.pas:1752
    public int SaveToStream(Stream stream, Level level)
    {
        Position = 0;
        int startSize = (int)stream.Length;
        stream.Write(CompressMagic);
        stream.Write(Bytes.Of(ref level, 1));
        if (level != Level.None)
        {
            ZLibStream c = new(stream, ZLibLevels.ForByte((byte)level), true);
            try
            {
                _ = c.CopyFrom(this, 0);
                return Round((double)(1 - ((double)(stream.Length - startSize) / Length)) * 100);
            }
            finally
            {
                c?.Dispose();
            }
        }
        else
        {
            _ = stream.CopyFrom(this, 0);
        }

        return 100;
    }

    // Engine.pas:1776
    public void SaveToStream(Stream stream) => SaveToStream(stream, Level.Default);

    // Engine.pas:1781
    public int SaveToFile(string fileName, Level level)
    {
        Stream f = OpenFile(fileName, 65535);
        try
        {
            return SaveToStream(f, level);
        }
        finally
        {
            f?.Dispose();
        }
    }

    // Engine.pas:1793
    public void SaveToFile(string fileName)
    {
        Stream f = OpenFile(fileName, 65535);
        try
        {
            _ = SaveToStream(f, Level.Default);
        }
        finally
        {
            f?.Dispose();
        }
    }

    // Engine.pas:1805
    public void LoadFromFile(string fileName)
    {
        Stream f = OpenFile(fileName, 0 | 32);
        try
        {
            LoadFromStream(f);
        }
        finally
        {
            f?.Dispose();
        }
    }

    private static ReadOnlySpan<byte> CompressMagic => "CFS\0"u8;

    public enum Level : byte
    {
        None = 0, Fastest = 1, Default = 2, Max = 3
    }
}
