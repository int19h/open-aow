// Translated from Engine.pas.
using System.IO.Compression;

using Aow.Engine.Buffers;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

public partial class CompressStream : MemoryStream
{

    // Engine.pas:1715
    public void LoadFromStream(Stream Stream)
    {
        bool Compressed = default;
        ByteArray4 Magic = default;
        ZLibStream D;
        byte[] Buffer;
        int Count;
        Stream.ReadExactly(((Span<byte>)Magic).Slice(0, 4));
        Stream.ReadExactly(Bytes.Of(ref Compressed, 1));
        if (!ArrayEquals<ByteArray4, byte>(Magic, CompressMagic))
        {
            Compressed = false;
            Stream.Seek(0, SeekOrigin.Begin);
        }

        if (Compressed)
        {
            Buffer = default;
            Count = -1;
            Array.Resize(ref Buffer, 8096);
            D = new ZLibStream(Stream, CompressionMode.Decompress, true);
            try
            {
                while (Count != 0)
                {
                    Count = D.Read(Buffer.AsSpan(0, 8096));
                    if (Count != 0)
                    {
                        this.WriteBuffer(Buffer.AsSpan(0, Count));
                    }
                }
            }
            finally
            {
                Array.Resize(ref Buffer, 0);
                D?.Dispose();
            }
        }
        else
        {
            this.CopyFrom(Stream, (int)(Stream.Length - Stream.Position));
        }

        Position = 0;
    }

    // Engine.pas:1752
    public int SaveToStreamEx(Stream Stream, Level Level)
    {
        ZLibStream C;
        int StartSize;
        Position = 0;
        int Result = 100;
        StartSize = (int)Stream.Length;
        Stream.Write(((ReadOnlySpan<byte>)CompressMagic).Slice(0, 4));
        Stream.Write(Bytes.Of(ref Level, 1));
        if (Level != Level.None)
        {
            C = new ZLibStream(Stream, ZLibLevels.ForByte((byte)Level), true);
            try
            {
                C.CopyFrom(this, 0);
                Result = Round((double)((double)1 - (double)(Stream.Length - StartSize) / Length) * 100);
            }
            finally
            {
                C?.Dispose();
            }
        }
        else
        {
            Stream.CopyFrom(this, 0);
        }

        return Result;
    }

    // Engine.pas:1776
    public void SaveToStream(Stream Stream)
    {
        SaveToStreamEx(Stream, Level.Default);
    }

    // Engine.pas:1781
    public int SaveToFileEx(string FileName, Level Level)
    {
        int Result = default;
        Stream F;
        F = OpenFile(FileName, 65535);
        try
        {
            Result = SaveToStreamEx(F, Level);
        }
        finally
        {
            F?.Dispose();
        }

        return Result;
    }

    // Engine.pas:1793
    public void SaveToFile(string FileName)
    {
        Stream F;
        F = OpenFile(FileName, 65535);
        try
        {
            SaveToStreamEx(F, Level.Default);
        }
        finally
        {
            F?.Dispose();
        }
    }

    // An engine image the file boundary prepared: the inflated payload, at position 0.
    public void LoadImage(byte[] Image)
    {
        this.WriteBuffer(Image);
        Position = 0;
    }

    // Engine.pas:1805
    public void LoadFromFile(string FileName)
    {
        Stream F;
        F = OpenFile(FileName, 0 | 32);
        try
        {
            LoadFromStream(F);
        }
        finally
        {
            F?.Dispose();
        }
    }

    private static ByteArray4 CompressMagic = ByteArray4.From("CFS\0"u8);

    public enum Level : byte
    {
        None = 0, Fastest = 1, Default = 2, Max = 3
    }
}
