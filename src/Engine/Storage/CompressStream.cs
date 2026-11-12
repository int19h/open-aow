// Translated from Engine.pas.
using System.IO.Compression;

using Aow.Engine.Buffers;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

public partial class CompressStream : MemoryStream
{

    // Engine.pas:1715
    public void LoadFromStream(Stream stream)
    {
        bool compressed = default;
        ByteArray4 magic = default;
        ZLibStream D;
        byte[] buffer;
        int count;
        stream.ReadExactly(((Span<byte>)magic).Slice(0, 4));
        stream.ReadExactly(Bytes.Of(ref compressed, 1));
        if (!ArrayEquals<ByteArray4, byte>(magic, s_compressMagic))
        {
            compressed = false;
            stream.Seek(0, SeekOrigin.Begin);
        }

        if (compressed)
        {
            buffer = default;
            count = -1;
            Array.Resize(ref buffer, 8096);
            D = new ZLibStream(stream, CompressionMode.Decompress, true);
            try
            {
                while (count != 0)
                {
                    count = D.Read(buffer.AsSpan(0, 8096));
                    if (count != 0)
                    {
                        this.WriteBuffer(buffer.AsSpan(0, count));
                    }
                }
            }
            finally
            {
                Array.Resize(ref buffer, 0);
                D?.Dispose();
            }
        }
        else
        {
            this.CopyFrom(stream, (int)(stream.Length - stream.Position));
        }

        Position = 0;
    }

    // Engine.pas:1752
    public int SaveToStream(Stream stream, Level level)
    {
        ZLibStream C;
        int startSize;
        Position = 0;
        int result = 100;
        startSize = (int)stream.Length;
        stream.Write(((ReadOnlySpan<byte>)s_compressMagic).Slice(0, 4));
        stream.Write(Bytes.Of(ref level, 1));
        if (level != Level.None)
        {
            C = new ZLibStream(stream, ZLibLevels.ForByte((byte)level), true);
            try
            {
                C.CopyFrom(this, 0);
                result = Round((double)((double)1 - (double)(stream.Length - startSize) / Length) * 100);
            }
            finally
            {
                C?.Dispose();
            }
        }
        else
        {
            stream.CopyFrom(this, 0);
        }

        return result;
    }

    // Engine.pas:1776
    public void SaveToStream(Stream stream)
    {
        SaveToStream(stream, Level.Default);
    }

    // Engine.pas:1781
    public int SaveToFile(string fileName, Level level)
    {
        int result = default;
        Stream F;
        F = OpenFile(fileName, 65535);
        try
        {
            result = SaveToStream(F, level);
        }
        finally
        {
            F?.Dispose();
        }

        return result;
    }

    // Engine.pas:1793
    public void SaveToFile(string fileName)
    {
        Stream F;
        F = OpenFile(fileName, 65535);
        try
        {
            SaveToStream(F, Level.Default);
        }
        finally
        {
            F?.Dispose();
        }
    }

    // An engine image the file boundary prepared: the inflated payload, at position 0.
    public void LoadImage(byte[] image)
    {
        this.WriteBuffer(image);
        Position = 0;
    }

    // Engine.pas:1805
    public void LoadFromFile(string fileName)
    {
        Stream F;
        F = OpenFile(fileName, 0 | 32);
        try
        {
            LoadFromStream(F);
        }
        finally
        {
            F?.Dispose();
        }
    }

    private static ByteArray4 s_compressMagic = ByteArray4.From("CFS\0"u8);

    public enum Level : byte
    {
        None = 0, Fastest = 1, Default = 2, Max = 3
    }
}
