// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Engine;

public partial class PropertyTable : IDisposable
{
    private Item[] _items = [];
    private int _capacity;
    private int _nextIndex;
    private int _maxId;
    private Settings[] _stack = [];
    private int _stackCount;
    private int _stackCapacity;

    // Engine.pas:2260
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity > 0)
        {
            newCapacity = (newCapacity + 31) & ~31;
        }

        if (newCapacity != _capacity)
        {
            Array.Resize(ref _items, newCapacity);
            _capacity = newCapacity;
            if (_capacity < Count)
            {
                Count = _capacity;
            }
        }
    }

    // Engine.pas:2253
    public virtual void Dispose()
    {
        SetCapacity(0);
        Array.Resize(ref _stack, 0);
    }

    // Engine.pas:2273
    public void Add(int id, int offset)
    {
        if (_capacity == Count)
        {
            SetCapacity(_capacity + 1);
        }

        _items[Count].Id = id;
        _items[Count].Offset = offset;
        Count += 1;
        if (id > _maxId)
        {
            _maxId = id;
        }
    }

    // Engine.pas:2284
    public void Clear()
    {
        _nextIndex = 0;
        _maxId = 0;
        SetCapacity(0);
    }

    // Engine.pas:2291
    public int FindOffset(int id)
    {
        int i = _nextIndex;
        int result;
        if (i >= Count || _items[i].Id != id)
        {
            i = First;
            result = -1;
            while (i < Count && _items[i].Id != id)
            {
                i += 1;
            }

            if (i != Count)
            {
                result = _items[i].Offset;
                _nextIndex = i + 1;
            }
        }
        else
        {
            result = _items[i].Offset;
            _nextIndex += 1;
        }

        return result;
    }

    // Engine.pas:2315
    public bool PropertyExist(int id)
    {
        if (id <= _maxId)
        {
            int i = First;
            while (i < Count && _items[i].Id != id)
            {
                i += 1;
            }

            return i != Count;
        }
        else
        {
            return false;
        }
    }

    // The smallest stored offset of the current table above an offset, or -1. A stored value runs
    // to the next larger offset, as the original backend bounds it.
    public int NextOffset(int offset)
    {
        int result = -1;
        for (int i = First; i < Count; i++)
        {
            if (_items[i].Offset > offset && (result == -1 || _items[i].Offset < result))
            {
                result = _items[i].Offset;
            }
        }

        return result;
    }

    // Engine.pas:2330
    public void BackupSettings()
    {
        if (_stackCount == _stackCapacity)
        {
            _stackCapacity += 16;
            Array.Resize(ref _stack, _stackCapacity);
        }

        _stack[_stackCount] = new Settings(First: First, NextIndex: _nextIndex, MaxId: _maxId);

        _stackCount += 1;
        _nextIndex = Count;
        First = Count;
        _maxId = 0;
    }

    // Engine.pas:2349
    public void RestoreSettings()
    {
        _stackCount -= 1;
        Count = First;
        (First, _nextIndex, _maxId) = _stack[_stackCount];
    }

    // Engine.pas:2361
    public void LoadFromStream(Stream stream)
    {
        int longCount = default;
        byte shortCount = default;
        stream.ReadExactly(Bytes.Of(ref shortCount, 1));
        if ((shortCount & 128) != 0)
        {
            shortCount = (byte)(shortCount & 127);
            stream.ReadExactly(Bytes.Of(ref longCount, 4));
        }
        else
        {
            longCount = 0;
        }

        if (shortCount != 0)
        {
            ushort pair = default;
            for (int ordinal = 0, last = shortCount - 1; ordinal <= last; ordinal++)
            {
                stream.ReadExactly(Bytes.Of(ref pair, 2));
                Add(pair & 255, (int)(((uint)pair) >> 8));
            }
        }

        if (longCount != 0)
        {
            int id = default;
            int offset = default;
            for (int ordinal = 0, last = longCount - 1; ordinal <= last; ordinal++)
            {
                stream.ReadExactly(Bytes.Of(ref id, 4));
                stream.ReadExactly(Bytes.Of(ref offset, 4));
                Add(id, offset);
            }
        }
    }

    // Engine.pas:2391
    public void SaveToStream(Stream stream)
    {
        int longCount = 0;
        byte shortCount = 0;
        for (int i = First, last = Count - 1; i <= last; i++)
        {
            if (_items[i].Id <= 127 && _items[i].Offset <= 255)
            {
                shortCount = (byte)(shortCount + 1);
            }
            else
            {
                longCount += 1;
            }
        }

        if (longCount != 0)
        {
            byte marker = (byte)(shortCount | 128);
            stream.Write(Bytes.Of(ref marker, 1));
            stream.Write(Bytes.Of(ref longCount, 4));
        }
        else
        {
            stream.Write(Bytes.Of(ref shortCount, 1));
        }

        if (shortCount != 0)
        {
            for (int i = First, last = Count - 1; i <= last; i++)
            {
                if (_items[i].Id <= 127 && _items[i].Offset <= 255)
                {
                    int pair = _items[i].Id + (_items[i].Offset << 8);
                    stream.Write(Bytes.Of(ref pair, 2));
                }
            }
        }

        if (longCount != 0)
        {
            for (int i = First, last = Count - 1; i <= last; i++)
            {
                if (_items[i].Id > 127 || _items[i].Offset > 255)
                {
                    stream.Write(Bytes.Of(ref _items[i].Id, 4));
                    stream.Write(Bytes.Of(ref _items[i].Offset, 4));
                }
            }
        }
    }

    public Item[] Items => _items;
    public int Count { get; private set; }
    public int First { get; private set; }

    [StoredLayout]
    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Item
    {
        [FieldOffset(0)]
        public int Id;
        [FieldOffset(4)]
        public int Offset;
    }

    // The table state that BackupSettings saves and RestoreSettings brings back.
    public readonly record struct Settings(int First, int NextIndex, int MaxId);
}
