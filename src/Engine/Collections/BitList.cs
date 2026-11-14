// Translated from Engine.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

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
            Error("List capacity out of bounds (%d)", newCapacity);
        }

        newCapacity = (newCapacity + 31) & ~31;
        if (newCapacity != _capacity)
        {
            if (newCapacity == 0) List = Memory<byte>.Empty;
            else
            {
                var resized = new byte[(int)((uint)newCapacity >> 3)];
                List.Span[..Math.Min(List.Length, resized.Length)].CopyTo(resized);
                List = resized;
            }
            _capacity = newCapacity;
            for (int I = _count; I <= newCapacity - 1; I++)
            {
                Put(I, _default != 0);
            }
        }
    }

    // Engine.pas:4758
    internal void SetCount(int newCount)
    {
        int oldCount;
        if (newCount < 0 || newCount > EngineServices.MaxListSize)
        {
            Error("List count out of bounds (%d)", newCount);
        }

        if (newCount > _capacity)
        {
            SetCapacity(newCount);
        }

        if (newCount > _count)
        {
            oldCount = _count;
            _count = newCount;
            for (int I = oldCount, last = _count - 1; I <= last; I++)
            {
                Put(I, _default != 0);
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
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            if (DefaultByte(Get(I)) != _default)
            {
                result = result + IntToStr(I) + ",";
            }
        }

        if (!IsEmpty(result))
        {
            result = Copy(result, 1, result.Length - 1);
        }

        return result;
    }

    // Engine.pas:4810
    internal void SetItemString(string S)
    {
        string item;
        int P;
        Clear();
        P = Pos(",", S);
        while (!IsEmpty(S))
        {
            if (P != 0)
            {
                item = Copy(S, 1, P - 1);
                S = RtlUnit.Delete(S, 1, P);
            }
            else
            {
                item = S;
                S = "";
            }

            if (!IsEmpty(item))
            {
                Put(StrToInt(item), _default == 0);
            }

            P = Pos(",", S);
        }
    }

    // Engine.pas:4834
    internal int GetBitsSet()
    {
        int result = 0;
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            if (Get(I))
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
        for (int I = 0, last = _count - 1; I <= last; I++)
        {
            if (!Get(I))
            {
                result += 1;
            }
        }

        return result;
    }

    // Engine.pas:4683
    // {Item}
    // {Item}
    protected internal virtual void Grow()
    {
        SetCapacity(_capacity + 32);
    }

    // Engine.pas:4571
    public override void Dispose()
    {
        Clear();
    }

    // Engine.pas:4576
    public override void Assign(Persistent source)
    {
        if (source is BitList)
        {
            var bitList = (BitList)source;
            this._count = bitList._count;
            this.SetCapacity(bitList._count);
            RtlUnit.Move(bitList.List.Span.Slice(0, (int)(((uint)(this._count + 31)) >> 5) * 4), this.List.Span.Slice(0, (int)(((uint)(this._count + 31)) >> 5) * 4), (int)(((uint)(this._count + 31)) >> 5) * 4);
            this._default = bitList._default;
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
            Error("List index out of bounds (%d)", index);
        }

        _count -= 1;
        if (index < _count)
        {
            for (int I = index, last = _count - 1; I <= last; I++)
            {
                Put(I, Get(I + 1));
            }
        }
    }

    // Engine.pas:4624
    public void Exchange(int index1, int index2)
    {
        bool item;
        if (index1 < 0 || index1 >= _count)
        {
            Error("List index out of bounds (%d)", index1);
        }

        if (index2 < 0 || index2 >= _count)
        {
            Error("List index out of bounds (%d)", index2);
        }

        item = Get(index1);
        Put(index1, Get(index2));
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

        if (result == _count)
        {
            result = -1;
        }

        return result;
    }

    // Engine.pas:4695
    public void Insert(int index, bool item)
    {
        if (index < 0 || index > _count)
        {
            Error("List index out of bounds (%d)", index);
        }

        if (_count == _capacity)
        {
            Grow();
        }

        if (index < _count)
        {
            _count += 1;
            // { sic: the original copies the same bit FCount - Index - 1 times instead of moving the tail }
            for (int I = _count - 2; I >= index; I--)
            {
                Put(index + 1, Get(index));
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
        bool item;
        if (currentIndex != newIndex)
        {
            if (newIndex < 0 || newIndex >= _count)
            {
                Error("List index out of bounds (%d)", newIndex);
            }

            item = Get(currentIndex);
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
        for (int I = _count - 1; I >= 0; I--)
        {
            if (DefaultByte(this[I]) == _default)
            {
                Delete(I);
            }
        }
    }

    // Engine.pas:4775
    public bool Compare(BitList list) => list._count == _count && CompareMem(List.Span.Slice(0, (int)(((uint)(_count + 7)) >> 3)), list.List.Span.Slice(0, (int)(((uint)(_count + 7)) >> 3)), (int)(((uint)(_count + 7)) >> 3));

    /// <summary>The image <c>TBitList.SaveToStream</c> (Engine.pas:4791) writes, the default as its stored byte.</summary>
    internal BitListImage SaveImage() =>
        new(_default, _count, List.Span.Slice(0, (int)(((uint)(_count + 7)) >> 3)).ToArray());

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
    public int Count { get => _count; set => SetCount(value); }
    public bool Default { get => _default != 0; set => _default = DefaultByte(value); }

    public bool this[int index] { get => Get(index); set => Put(index, value); }

    public string ItemString { get => GetItemString(); set => SetItemString(value); }
    public int BitsSet { get => GetBitsSet(); }
    public int BitsCleared { get => GetBitsCleared(); }

    private static byte DefaultByte(bool item) => item ? (byte)1 : (byte)0;

    // Engine.pas:4613
    protected internal static void Error(string message, int data)
    {
        throw new ArgumentOutOfRangeException(paramName: null, RtlUnit.Format(message, [data]));
    }

}
