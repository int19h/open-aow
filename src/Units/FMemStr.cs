// Translated from FMemStr.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine.Storage;

// { The game's FMemStr unit (Enginep.dpl), written for Delphi 3.02 and compared with the original code
//   by tools/match.py. TFastMemoryStream is Delphi 3's TMemoryStream with a direct Position field, a
//   32/128-byte capacity granularity and one-, two- and four-byte fast paths in Read and Write. }
// FMemory, FSize, FPosition, FCapacity
[LegacyObject]
public partial class FastMemoryStream : Stream
{
    internal byte[] FMemory = [];
    internal int FSize;
    internal int FPosition;
    internal int FCapacity;

    public FastMemoryStream()
    {
        CaptureAddress.Assign(this);
    }

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => true;

    public override long Length => FSize;

    public override long Position
    {
        get => FPosition;
        set => FPosition = (int)value;
    }

    public override void Flush()
    {
    }

    // FMemStr.pas:125
    internal void SetCapacity(int NewCapacity)
    {
        if (NewCapacity > 0)
        {
            if (FCapacity > 32)
            {
                NewCapacity = (NewCapacity + 127) & ~127;
            }
            else
            {
                NewCapacity = (NewCapacity + 31) & ~31;
            }
        }

        if (NewCapacity != FCapacity)
        {
            Array.Resize(ref FMemory, NewCapacity);

            FCapacity = NewCapacity;
            if (FCapacity > FSize)
            {
                FSize = FCapacity;
            }
        }
    }

    // FMemStr.pas:152. A shorter length leaves the position at NewSize - 1.
    public override void SetLength(long value)
    {
        int NewSize = (int)value;
        if (NewSize < FPosition)
        {
            FPosition = NewSize - 1;
        }

        SetCapacity(NewSize);
        FSize = NewSize;
    }

    // FMemStr.pas:90
    protected override void Dispose(bool disposing)
    {
        if (disposing) Clear();
        base.Dispose(disposing);
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (count < 0) return 0;
        return Read(buffer.AsSpan(offset, count));
    }

    // FMemStr.pas:43
    public override int Read(Span<byte> Buffer)
    {
        if (Buffer.IsEmpty) return 0;
        int Count = Buffer.Length;
        int Result = FSize - FPosition;
        if (Result > 0)
        {
            if (Result > Count)
            {
                Result = Count;
            }

            if (Count == 4)
            {
                int value__3 = MemoryMarshal.Read<int>(FMemory.AsSpan(FPosition, 4));
                MemoryMarshal.Write(Buffer, in value__3);
            }
            else
            {
                if (Count == 1)
                {
                    byte value__4 = MemoryMarshal.Read<byte>(FMemory.AsSpan(FPosition, 1));
                    MemoryMarshal.Write(Buffer, in value__4);
                }
                else
                {
                    if (Count == 2)
                    {
                        ushort value__5 = MemoryMarshal.Read<ushort>(FMemory.AsSpan(FPosition, 2));
                        MemoryMarshal.Write(Buffer, in value__5);
                    }
                    else
                    {
                        Move(FMemory.AsSpan(FPosition, Result), Buffer.Slice(0, Result), Result);
                    }
                }
            }

            FPosition += Result;
            return Result;
        }

        return 0;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (count < 0) return;
        Write(buffer.AsSpan(offset, count));
    }

    // FMemStr.pas:159. A non-empty write that stores nothing raises the stream write error.
    public override void Write(ReadOnlySpan<byte> Buffer)
    {
        int Count = Buffer.Length;
        int Pos;
        if (FPosition >= 0 && Count >= 0)
        {
            Pos = FPosition + Count;
            if (Pos > 0)
            {
                if (Pos > FSize)
                {
                    if (Pos > FCapacity)
                    {
                        SetCapacity(Pos);
                    }

                    FSize = Pos;
                }

                if (Count == 4)
                {
                    int value__6 = MemoryMarshal.Read<int>(Buffer);
                    MemoryMarshal.Write(FMemory.AsSpan(FPosition, 4), in value__6);
                }
                else
                {
                    if (Count == 1)
                    {
                        byte value__7 = MemoryMarshal.Read<byte>(Buffer);
                        MemoryMarshal.Write(FMemory.AsSpan(FPosition, 1), in value__7);
                    }
                    else
                    {
                        if (Count == 2)
                        {
                            ushort value__8 = MemoryMarshal.Read<ushort>(Buffer);
                            MemoryMarshal.Write(FMemory.AsSpan(FPosition, 2), in value__8);
                        }
                        else
                        {
                            if (Count != 0)
                                Move(Buffer.Slice(0, Count), FMemory.AsSpan(FPosition, Count), Count);
                        }
                    }
                }

                FPosition = Pos;
                return;
            }
        }

        if (Count != 0) throw new IOException("Stream write error");
    }

    // FMemStr.pas:63
    public override long Seek(long offset, SeekOrigin origin)
    {
        int Offset = (int)offset;
        switch (origin)
        {
            case SeekOrigin.Begin:
                FPosition = Offset;
                break;
            case SeekOrigin.Current:
                FPosition += Offset;
                break;
            case SeekOrigin.End:
                FPosition = FSize + Offset;
                break;
        }

        return FPosition;
    }

    // FMemStr.pas:96
    public void Clear()
    {
        SetCapacity(0);
        FSize = 0;
        FPosition = 0;
    }

    // FMemStr.pas:103
    public void LoadFromStream(Stream Stream)
    {
        int Count;
        Stream.Position = 0;
        Count = (int)Stream.Length;
        SetLength(Count);
        if (Count != 0)
        {
            Stream.ReadBuffer(FMemory.AsSpan(0, Count));
        }
    }

    // FMemStr.pas:113
    public void LoadFromFile(string FileName)
    {
        Stream Stream;
        Stream = OpenFile(FileName, 0);
        try
        {
            LoadFromStream(Stream);
        }
        finally
        {
            Stream?.Dispose();
        }
    }

    // FMemStr.pas:73
    public void SaveToStream(Stream Stream)
    {
        if (FSize != 0)
        {
            Stream.WriteBuffer(FMemory.AsSpan(0, FSize));
        }
    }

    // FMemStr.pas:78
    public void SaveToFile(string FileName)
    {
        Stream Stream;
        Stream = OpenFile(FileName, 65535);
        try
        {
            SaveToStream(Stream);
        }
        finally
        {
            Stream?.Dispose();
        }
    }

    public byte[] Memory { get => FMemory; }
    public int Capacity { get => FCapacity; set => SetCapacity(value); }
}
