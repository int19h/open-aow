// Translated from Engine.pas.
using System.IO.Compression;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

public partial class CompressStream : MemoryStream
{

    // Engine.pas:1715
    public void LoadFromStream(Stream stream)
    {
        bool compressed = default;
        Span<byte> magic = stackalloc byte[4];
        ZLibStream D;
        byte[] buffer;
        int count;
        stream.ReadExactly(magic);
        stream.ReadExactly(Bytes.Of(ref compressed, 1));
        if (!magic.SequenceEqual(CompressMagic))
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
        stream.Write(CompressMagic);
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

    private static ReadOnlySpan<byte> CompressMagic => "CFS\0"u8;

    public enum Level : byte
    {
        None = 0, Fastest = 1, Default = 2, Max = 3
    }
}
