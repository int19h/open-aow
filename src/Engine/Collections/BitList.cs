#nullable disable
// Translated from Engine.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFormat;
using static Aow.Rtl.DelphiMemory;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Engine.Collections;

public partial class BitList : Persistent
{
    internal Memory<byte> List { get; set; } = Memory<byte>.Empty;

    private int _count;
    private int _capacity;
    // The default as its stored Boolean byte. A loaded byte other than 0 and 1 stays as it is: a
    // nonzero byte fills new bits, Get past the end reads its low bit, and the comparisons with an
    // item compare the whole byte, as the original's Boolean comparisons do.
    private byte _default;

    // Engine.pas:4743
    internal void SetCapacity(int newCapacity)
    {
        if (newCapacity < _count || newCapacity > EngineServices.MaxListSize)
        {
            Error("List capacity out of bounds (%d)", data: newCapacity);
        }

        newCapacity = (newCapacity + 31) & ~31;
        if (newCapacity != _capacity)
        {
            if (newCapacity == 0)
            {
                List = Memory<byte>.Empty;
            }
            else
            {
                byte[] resized = new byte[newCapacity >>> 3];
                List.Span[..Math.Min(List.Length, resized.Length)].CopyTo(resized);
                List = resized;
            }
            _capacity = newCapacity;
            for (int i = _count; i <= newCapacity - 1; i++)
            {
                Put(i, item: _default != 0);
            }
        }
    }

    // Engine.pas:4758
    internal void SetCount(int newCount)
    {
        if (newCount is < 0 or > EngineServices.MaxListSize)
        {
            Error("List count out of bounds (%d)", data: newCount);
        }

        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > _count)
        {
            int oldCount = _count;
            _count = newCount;
            for (int i = oldCount, last = _count - 1; i <= last; i++)
            {
                Put(i, item: _default != 0);
            }
        }
        else
        {
            _count = newCount;
        }
    }

    // Engine.pas:4798
    internal string GetItemString()
    {
        string result = "";
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            if (DefaultByte(Get(i)) != _default)
            {
                result = $"{result}{IntToStr(i)},";
            }
        }

        return !IsEmpty(result) ? Copy(result, index: 1, count: result.Length - 1) : result;
    }

    // Engine.pas:4810
    internal void SetItemString(string S)
    {
        Clear();
        int p = Pos(",", haystack: S);
        while (!IsEmpty(S))
        {
            string item;
            if (p != 0)
            {
                item = Copy(S, index: 1, count: p - 1);
                S = DelphiStrings.Delete(S, index: 1, count: p);
            }
            else
            {
                item = S;
                S = "";
            }

            if (!IsEmpty(item))
            {
                Put(StrToInt(item), item: _default == 0);
            }

            p = Pos(",", haystack: S);
        }
    }

    // Engine.pas:4834
    internal int GetBitsSet()
    {
        int result = 0;
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            if (Get(i))
            {
                result += 1;
            }
        }

        return result;
    }

    // Engine.pas:4843
    internal int GetBitsCleared()
    {
        int result = 0;
        for (int i = 0, last = _count - 1; i <= last; i++)
        {
            if (!Get(i))
            {
                result += 1;
            }
        }

        return result;
    }

    // Engine.pas:4683
    // {Item}
    // {Item}
    protected internal virtual void Grow() => SetCapacity(_capacity + 32);

    // Engine.pas:4571
    public override void Dispose() => Clear();

    // Engine.pas:4576
    public override void Assign(Persistent source)
    {
        if (source is BitList bitList)
        {
            _count = bitList._count;
            SetCapacity(bitList._count);
            DelphiMemory.Move(bitList.List.Span[..(((_count + 31) >>> 5) * 4)], destination: List.Span[..(((_count + 31) >>> 5) * 4)], count: ((_count + 31) >>> 5) * 4);
            _default = bitList._default;
        }
        else
        {
            base.Assign(source);
        }
    }

    // Engine.pas:4590
    public int Add(bool item)
    {
        int result = _count;
        if (result == _capacity)
        {
            Grow();
        }

        Put(_count, item);
        return result;
    }

    // Engine.pas:4597
    public void Clear()
    {
        SetCount(0);
        SetCapacity(0);
    }

    // Engine.pas:4603
    public void Delete(int index)
    {
        if (index < 0 || index >= _count)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        _count -= 1;
        if (index < _count)
        {
            for (int i = index, last = _count - 1; i <= last; i++)
            {
                Put(i, item: Get(i + 1));
            }
        }
    }

    // Engine.pas:4624
    public void Exchange(int index1, int index2)
    {
        if (index1 < 0 || index1 >= _count)
        {
            Error("List index out of bounds (%d)", data: index1);
        }

        if (index2 < 0 || index2 >= _count)
        {
            Error("List index out of bounds (%d)", data: index2);
        }

        bool item = Get(index1);
        Put(index1, item: Get(index2));
        Put(index2, item);
    }

    // Engine.pas:4635
    public BitList Expand()
    {
        if (_count == _capacity)
        {
            Grow();
        }

        return this;
    }

    // Engine.pas:4641
    public bool First() => Get(0);

    // Engine.pas:4688
    public int IndexOf(bool item)
    {
        int result = 0;
        while (result < _count && item != Get(result))
        {
            result += 1;
        }

        return result == _count ? -1 : result;
    }

    // Engine.pas:4695
    public void Insert(int index, bool item)
    {
        if (index < 0 || index > _count)
        {
            Error("List index out of bounds (%d)", data: index);
        }

        if (_count == _capacity)
        {
            Grow();
        }

        if (index < _count)
        {
            _count += 1;
            // { sic: the original copies the same bit FCount - Index - 1 times instead of moving the tail }
            for (int i = _count - 2; i >= index; i--)
            {
                Put(index + 1, item: Get(index));
            }
        }
        else
        {
            _count += 1;
        }

        Put(index, item);
    }

    // Engine.pas:4712
    public bool Last() => Get(_count - 1);

    // Engine.pas:4717
    public void Move(int currentIndex, int newIndex)
    {
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", data: newIndex);
            }

            bool item = Get(currentIndex);
            Delete(currentIndex);
            Insert(newIndex, item);
        }
    }

    // Engine.pas:4730
    public int Remove(bool item)
    {
        int result = IndexOf(item);
        if (result != -1)
        {
            Delete(result);
        }

        return result;
    }

    // Engine.pas:4736
    public void Pack()
    {
        for (int i = _count - 1; i >= 0; i--)
        {
            if (DefaultByte(this[i]) == _default)
            {
                Delete(i);
            }
        }
    }

    // Engine.pas:4775
    public bool Compare(BitList list) => list._count == _count && CompareMem(List.Span[..((_count + 7) >>> 3)], list.List.Span[..((_count + 7) >>> 3)], count: (_count + 7) >>> 3);

    /// <summary>The image <c>TBitList.SaveToStream</c> (Engine.pas:4791) writes, the default as its stored byte.</summary>
    internal BitListImage SaveImage()
        => new(_default, _count, Bits: List.Span[..((_count + 7) >>> 3)].ToArray());

    /// <summary>
    /// <c>TBitList.LoadFromStream</c> (Engine.pas:4782) from a stored image. A default byte other
    /// than 0 and 1 stays that byte.
    /// </summary>
    internal void LoadImage(BitListImage image)
    {
        Clear();
        _default = image.Default;
        _count = image.Count;
        SetCapacity(_count);
        image.Bits.CopyTo(List.Span);
    }

    public int Capacity { get => _capacity; set => SetCapacity(value); }
    public int Count { get => _count; internal set => SetCount(value); }
    public bool Default { get => _default != 0; internal set => _default = DefaultByte(value); }

    public bool this[int index] { get => Get(index); set => Put(index, item: value); }

    public string ItemString => GetItemString();
    public int BitsSet => GetBitsSet();
    public int BitsCleared => GetBitsCleared();

    private static byte DefaultByte(bool item) => item ? (byte)1 : (byte)0;

    // Engine.pas:4613
    protected internal static void Error(string message, int data) => throw new ArgumentOutOfRangeException(paramName: null, message: Format(message, args: [data]));

}
