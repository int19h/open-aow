// Translated from Engine.pas.
using System.Runtime.InteropServices;

using Aow.Rtl;

namespace Aow.Engine;

public partial class PropertyTable : IDisposable
{
    private Item[] _items = [];
    private int _capacity;
    private int _count;
    private int _nextIndex;
    private int _maxId;
    private Settings[] _stack = [];
    private int _stackCount;
    private int _stackCapacity;
    private int _first;

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
            if (_capacity < _count)
            {
                _count = _capacity;
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
        if (_capacity == _count)
        {
            SetCapacity(_capacity + 1);
        }

        _items[_count].Id = id;
        _items[_count].Offset = offset;
        _count += 1;
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
        int result;
        int I;
        I = _nextIndex;
        if (I >= _count || _items[I].Id != id)
        {
            I = _first;
            result = -1;
            while (I < _count && _items[I].Id != id)
            {
                I += 1;
            }

            if (I != _count)
            {
                result = _items[I].Offset;
                _nextIndex = I + 1;
            }
        }
        else
        {
            result = _items[I].Offset;
            _nextIndex += 1;
        }

        return result;
    }

    // Engine.pas:2315
    public bool PropertyExist(int id)
    {
        bool result;
        int I;
        if (id <= _maxId)
        {
            I = _first;
            while (I < _count && _items[I].Id != id)
            {
                I += 1;
            }

            result = I != _count;
        }
        else
        {
            result = false;
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

        ref Settings propertySettings = ref _stack[_stackCount];
        propertySettings.First = _first;
        propertySettings.NextIndex = _nextIndex;
        propertySettings.MaxId = _maxId;

        _stackCount += 1;
        _nextIndex = _count;
        _first = _count;
        _maxId = 0;
    }

    // Engine.pas:2349
    public void RestoreSettings()
    {
        _stackCount -= 1;
        _count = _first;
        ref Settings propertySettings = ref _stack[_stackCount];
        _first = propertySettings.First;
        _nextIndex = propertySettings.NextIndex;
        _maxId = propertySettings.MaxId;
    }

    // Engine.pas:2361
    public void LoadFromStream(Stream stream)
    {
        int longCount = default;
        int id = default;
        int offset = default;
        byte shortCount = default;
        ushort pair = default;
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
            for (int ordinal = 0, last = shortCount - 1; ordinal <= last; ordinal++)
            {
                stream.ReadExactly(Bytes.Of(ref pair, 2));
                Add(pair & 255, (int)(((uint)pair) >> 8));
            }
        }

        if (longCount != 0)
        {
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
        int longCount;
        byte shortCount;
        byte marker;
        int pair = default;
        longCount = 0;
        shortCount = 0;
        for (int I = _first, last = _count - 1; I <= last; I++)
        {
            if (_items[I].Id <= 127 && _items[I].Offset <= 255)
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
            marker = (byte)(shortCount | 128);
            stream.Write(Bytes.Of(ref marker, 1));
            stream.Write(Bytes.Of(ref longCount, 4));
        }
        else
        {
            stream.Write(Bytes.Of(ref shortCount, 1));
        }

        if (shortCount != 0)
        {
            for (int I = _first, last = _count - 1; I <= last; I++)
            {
                if (_items[I].Id <= 127 && _items[I].Offset <= 255)
                {
                    pair = _items[I].Id + (_items[I].Offset << 8);
                    stream.Write(Bytes.Of(ref pair, 2));
                }
            }
        }

        if (longCount != 0)
        {
            for (int I = _first, last = _count - 1; I <= last; I++)
            {
                if (_items[I].Id > 127 || _items[I].Offset > 255)
                {
                    stream.Write(Bytes.Of(ref _items[I].Id, 4));
                    stream.Write(Bytes.Of(ref _items[I].Offset, 4));
                }
            }
        }
    }

    public Item[] Items { get => _items; }
    public int Count { get => _count; }
    public int First { get => _first; }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    public struct Item
    {
        [FieldOffset(0)]
        public int Id;
        [FieldOffset(4)]
        public int Offset;
    }

    [StructLayout(LayoutKind.Explicit, Size = 12)]
    public struct Settings
    {
        [FieldOffset(0)]
        public int First;
        [FieldOffset(4)]
        public int NextIndex;
        [FieldOffset(8)]
        public int MaxId;
    }
}
