// Translated from FMemStr.pas.
using System.Runtime.InteropServices;
using Aow.Rtl;
using static Aow.Rtl.DelphiMemory;
using static Aow.Rtl.DelphiStreams;

namespace Aow.Engine.Storage;

// { The game's FMemStr unit (Enginep.dpl), written for Delphi 3.02 and compared with the original code
//   by tools/match.py. TFastMemoryStream is Delphi 3's TMemoryStream with a direct Position field, a
//   32/128-byte capacity granularity and one-, two- and four-byte fast paths in Read and Write. }
// _memory, _size, _position, _capacity
public class FastMemoryStream : Stream
{
    private byte[] _memory = [];
    private int _size;
    private int _position;
    private int _capacity;

    public override bool CanRead => true;

    public override bool CanSeek => true;

    public override bool CanWrite => true;

    public override long Length => _size;

    public override long Position
    {
        get => _position;
        set => _position = (int)value;
    }

    public override void Flush()
    {
    }

    // FMemStr.pas:125
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity > 0)
        {
            newCapacity = _capacity > 32 ? (newCapacity + 127) & ~0b0111_1111 : (newCapacity + 31) & ~0b0001_1111;
        }

        if (newCapacity != _capacity)
        {
            Array.Resize(ref _memory, newSize: newCapacity);

            _capacity = newCapacity;
            if (_capacity > _size)
            {
                _size = _capacity;
            }
        }
    }

    // FMemStr.pas:152. A shorter length leaves the position at NewSize - 1.
    public override void SetLength(long value)
    {
        int newSize = (int)value;
        if (newSize < _position)
        {
            _position = newSize - 1;
        }

        SetCapacity(newSize);
        _size = newSize;
    }

    // FMemStr.pas:90
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Clear();
        }

        base.Dispose(disposing);
    }

    public override int Read(byte[] buffer, int offset, int count) => count < 0 ? 0 : Read(buffer.AsSpan(offset, length: count));

    // FMemStr.pas:43
    public override int Read(Span<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return 0;
        }

        int count = buffer.Length;
        int result = _size - _position;
        if (result > 0)
        {
            if (result > count)
            {
                result = count;
            }

            if (count == 4)
            {
                int value3 = MemoryMarshal.Read<int>(_memory.AsSpan(_position, length: 4));
                MemoryMarshal.Write(buffer, value: in value3);
            }
            else
            {
                if (count == 1)
                {
                    byte value4 = MemoryMarshal.Read<byte>(_memory.AsSpan(_position, length: 1));
                    MemoryMarshal.Write(buffer, value: in value4);
                }
                else
                {
                    if (count == 2)
                    {
                        ushort value5 = MemoryMarshal.Read<ushort>(_memory.AsSpan(_position, length: 2));
                        MemoryMarshal.Write(buffer, value: in value5);
                    }
                    else
                    {
                        Move(_memory.AsSpan(_position, length: result), destination: buffer[..result], count: result);
                    }
                }
            }

            _position += result;
            return result;
        }

        return 0;
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        if (count < 0)
        {
            return;
        }

        Write(buffer.AsSpan(offset, length: count));
    }

    // FMemStr.pas:159. A non-empty write that stores nothing raises the stream write error.
    public override void Write(ReadOnlySpan<byte> buffer)
    {
        int count = buffer.Length;
        if (_position >= 0 && count >= 0)
        {
            int position = _position + count;
            if (position > 0)
            {
                if (position > _size)
                {
                    if (position > _capacity)
                    {
                        SetCapacity(position);
                    }

                    _size = position;
                }

                if (count == 4)
                {
                    int value6 = MemoryMarshal.Read<int>(buffer);
                    MemoryMarshal.Write(_memory.AsSpan(_position, length: 4), value: in value6);
                }
                else
                {
                    if (count == 1)
                    {
                        byte value7 = MemoryMarshal.Read<byte>(buffer);
                        MemoryMarshal.Write(_memory.AsSpan(_position, length: 1), value: in value7);
                    }
                    else
                    {
                        if (count == 2)
                        {
                            ushort value8 = MemoryMarshal.Read<ushort>(buffer);
                            MemoryMarshal.Write(_memory.AsSpan(_position, length: 2), value: in value8);
                        }
                        else
                        {
                            if (count != 0)
                            {
                                Move(buffer[..count], destination: _memory.AsSpan(_position, length: count), count);
                            }
                        }
                    }
                }

                _position = position;
                return;
            }
        }

        if (count != 0)
        {
            throw new IOException("Stream write error");
        }
    }

    // FMemStr.pas:63
    public override long Seek(long offset, SeekOrigin origin)
    {
        int offsetLocal = (int)offset;
        switch (origin)
        {
            case SeekOrigin.Begin:
                _position = offsetLocal;
                break;
            case SeekOrigin.Current:
                _position += offsetLocal;
                break;
            case SeekOrigin.End:
                _position = _size + offsetLocal;
                break;
            default:
                break;
        }

        return _position;
    }

    // FMemStr.pas:96
    public void Clear()
    {
        SetCapacity(0);
        _size = 0;
        _position = 0;
    }

    // FMemStr.pas:103
    public void LoadFromStream(Stream stream)
    {
        stream.Position = 0;
        int count = (int)stream.Length;
        SetLength(count);
        if (count != 0)
        {
            stream.ReadBuffer(_memory.AsSpan(0, length: count));
        }
    }

    // FMemStr.pas:113
    public void LoadFromFile(string fileName)
    {
        Stream stream = OpenFile(fileName, mode: FileOpenMode.Read);
        try
        {
            LoadFromStream(stream);
        }
        finally
        {
            stream?.Dispose();
        }
    }

    // FMemStr.pas:73
    public void SaveToStream(Stream stream)
    {
        if (_size != 0)
        {
            stream.WriteBuffer(_memory.AsSpan(0, length: _size));
        }
    }

    // FMemStr.pas:78
    public void SaveToFile(string fileName)
    {
        Stream stream = OpenFile(fileName, mode: FileOpenMode.Create);
        try
        {
            SaveToStream(stream);
        }
        finally
        {
            stream?.Dispose();
        }
    }

    public byte[] Memory => _memory;
    public int Capacity { get => _capacity; set => SetCapacity(value); }
}
