using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Rtl.Tests.Generated;
using static Aow.Rtl.RtlUnit;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class CoreTests
{
    [Fact]
    public void FixtureRandomizeResetsAfterDrawsAndLeavesNormalSuppressionIntact()
    {
        int previous = RandSeed;
        bool suppressed = SuppressRandomize;
        try
        {
            SuppressRandomize = true;
            RandomizeSeed = 123456789;
            Randomize();
            int first = Random(10000);
            Random(10000);
            Randomize();
            Assert.Equal(first, Random(10000));
            RandomizeSeed = null;
            int retained = RandSeed;
            Randomize();
            Assert.Equal(retained, RandSeed);
        }
        finally { RandomizeSeed = null; SuppressRandomize = suppressed; RandSeed = previous; }
    }

    [Fact]
    public void ObjectsAndGeneratedUnit()
    {
        Assert.Equal("TSampleChild", ClassNames.ClassNameOf(typeof(TSampleChild)));
        var fresh = new TSampleChild(0);
        var next = new TSampleChild(3);
        Assert.NotSame(fresh, next);
        Assert.Equal(13, next.Value);
        Assert.Equal(2, next.ClassID);
        Assert.Throws<EAbstractError>(next.AbstractCall);
        object? nil = null;
        (nil as IDisposable)?.Dispose();
        next.Dispose();
        Assert.True(next.Destroyed);
        Assert.Throws<InvalidOperationException>(() => TFailingSample.Create(1));
        Assert.True(TFailingSample.LastInstance!.Destroyed);
        var result = GeneratedStyleSample.Run();
        Assert.Equal(20, result.Value);
        Assert.Equal("abcde", result.Name);
        Assert.Equal(TSet<TFlag>.Range(TFlag.First, TFlag.Third), result.Flags);
        var copy = result;
        copy.Name = "x";
        copy.Flags = default;
        Assert.Equal("abcde", result.Name);
        Assert.Equal(3, result.Flags.Count);
        Assert.Equal(12, Marshal.SizeOf<TSampleRecord>());
        var image = Bytes.Of(ref result);
        Assert.Equal(7, image[0]);
        Assert.Equal(new byte[] { 5, 97, 98, 99, 100, 101 }, image.Slice(1, 6).ToArray());
        Assert.Equal(6, Marshal.SizeOf<FixedArray_byte_6>());
    }

    private sealed class Unregistered;

    [Fact]
    public void StringsAreImmutableValuesWithOneCharPerByte()
    {
        using var mode = new StringMode(false);
        string? nil = null;
        Assert.Equal(0, Length(nil));
        Assert.True(IsEmpty(nil));
        Assert.Equal((byte)0, Ptr<byte>.Unbox("").Value);
        Assert.Equal("", StrPas(default));
        byte[] buffer = [0, 65, 128, 255];
        string value = Cp1252.Decode(buffer);
        buffer[1] = 99;
        Assert.Equal('A', value[1]);
        Assert.Equal("\0A\u20ac\u00ff", value);
        Assert.Equal((byte)63, Cp1252.Byte('\u0080'));
        Assert.Equal((byte)128, Cp1252.Byte('\u20ac'));
        Assert.Equal("?", Cp1252.Normalize("\u4e2d"));
        byte[] all = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
        string full = Cp1252.Decode(all);
        Assert.Equal(256, full.Length);
        Assert.Equal(all, Cp1252.Encode(full));
        var dictionary = new Dictionary<string, int> { [full] = 1 };
        Assert.Equal(1, dictionary[Cp1252.Decode(all)]);
        Assert.Equal(0, Cp1252.Compare(full, Cp1252.Decode(all)));
        // A PChar of a string is a zero-terminated copy, so a write through it leaves the string.
        var pointer = Ptr<byte>.Unbox(full);
        pointer[1] = 10;
        Assert.Equal('\u0001', full[1]);
        Assert.Equal(0, pointer[256]);
        Assert.Equal("ab", StrPas(Ptr<byte>.Unbox("ab\0cd")));
        string original = "abc";
        Assert.Equal("abc\0\0", Resized(original, 5));
        Assert.Equal("abc", original);
        Assert.Equal("aXc", WithChar(original, 2, 'X'));
        // The indexer and WithChar report a string index. A negative length is an argument.
        Assert.Throws<IndexOutOfRangeException>(() => original[3]);
        Assert.Throws<IndexOutOfRangeException>(() => original[-1]);
        Assert.Throws<IndexOutOfRangeException>(() => WithChar(original, 0, 'X'));
        Assert.Throws<IndexOutOfRangeException>(() => WithChar(original, 4, 'X'));
        Assert.Throws<IndexOutOfRangeException>(() => WithChar(original, int.MinValue, '\0'));
        Assert.Throws<ArgumentOutOfRangeException>(() => Resized(original, -1));
        Assert.Equal(0, Cp1252.Compare("a", "a"));
        Assert.True(Cp1252.Compare("a", "b") < 0);
        Assert.True(Cp1252.Compare("\u20ac", "\u00c4") < 0);
        Assert.Equal("abcd", original + "d");
    }

    [Fact]
    public void PointersAliasCellsAndSupportManagedCopies()
    {
        Ptr<int> nil = default;
        Assert.True(nil.IsNull);
        Assert.True(nil == default);
        Assert.True(Ptr<int>.Alloc(0).IsNull);
        var pointer = Ptr<int>.Alloc(5);
        Assert.False(pointer.IsNull);
        Assert.Equal(0, pointer.Index);
        Assert.Equal(5, pointer.Array!.Length);
        pointer.Value = 1;
        pointer[1] = 2;
        pointer[2] = 3;
        Assert.Equal(3, pointer.Array[2]);
        Assert.Equal(2, (pointer + 1).Value);
        Assert.Equal(pointer, (pointer + 2) - 2);
        Assert.Equal(2, pointer + 2 - pointer);
        Assert.True(pointer != pointer + 1);
        Assert.True(pointer.Equals((object)Ptr<int>.To(pointer.Array, 0)));
        Assert.False(pointer.Equals(0));
        Assert.Equal(pointer.GetHashCode(), Ptr<int>.To(pointer.Array, 0).GetHashCode());
        Assert.Throws<InvalidOperationException>(() => pointer - Ptr<int>.Alloc(1));
        Ptr<int>.Copy(pointer, pointer + 1, 3);
        Assert.Equal(new[] { 1, 1, 2, 3, 0 }, pointer.Array);
        Ptr<int>.Copy(pointer + 1, pointer, 3);
        Assert.Equal(new[] { 1, 2, 3, 3, 0 }, pointer.Array);
        Ptr<int>.Fill(pointer + 3, 2, 8);
        Assert.Equal(new[] { 1, 2, 3, 8, 8 }, pointer.Array);
        var grown = pointer.Realloc(7);
        Assert.Equal(new[] { 1, 2, 3, 8, 8, 0, 0 }, grown.Array);
        grown.Value = 9;
        Assert.Equal(1, pointer.Value);
        Assert.Equal(new[] { 2, 3 }, (pointer + 1).Realloc(2).Array);
        Assert.True(pointer.Realloc(0).IsNull);
        Assert.Equal(2, nil.Realloc(2).Array!.Length);
        Ptr<int>.Copy(default, default, 0);
        Ptr<int>.Fill(default, -1, 0);
        var box = new PtrBox<int>(pointer);
        Assert.Equal(pointer, box.Pointer);
        Assert.Equal(pointer, Ptr<int>.Unbox((object)box));
        IntOrRef slot = new(box);
        Assert.Equal(pointer, Ptr<int>.Unbox(slot.Ref));
        Ptr<int>.Unbox(slot.Ref).Value = 42;
        Assert.Equal(42, pointer.Value);
        Assert.Throws<InvalidCastException>(() => Ptr<int>.Unbox((object?)null));
        Assert.Throws<InvalidCastException>(() => Ptr<byte>.Unbox(box));
        Assert.Throws<InvalidCastException>(() => Ptr<int>.Unbox(((IntOrRef)3).Ref));
        string[] words = ["a", "b", "c"];
        var managed = Ptr<string>.To(words, 0);
        Ptr<string>.Copy(managed, managed + 1, 2);
        Assert.Equal(new[] { "a", "a", "b" }, words);
        Ptr<string>.Fill(managed + 1, 2, "z");
        Assert.Equal(new[] { "a", "z", "z" }, words);
        var cell = new TSampleRecord[1];
        var recordPointer = Ptr<TSampleRecord>.To(cell, 0);
        IntOrRef payload = new(new PtrBox<TSampleRecord>(recordPointer));
        Ptr<TSampleRecord>.Unbox(payload.Ref).Value.Value = 91;
        Assert.Equal(91, cell[0].Value);
    }

    [Fact]
    public void IntOrRefKeepsTagsAndIdentity()
    {
        IntOrRef zero = default;
        Assert.Null(zero.Ref);
        Assert.Equal(0, zero.Value);
        IntOrRef number = -123;
        Assert.Equal(-123, number.Value);
        Assert.Null(number.As<object>());
        var obj = new object();
        IntOrRef reference = new(obj);
        Assert.Same(obj, reference.Ref);
        Assert.Same(obj, reference.As<object>());
        Assert.Equal(RuntimeHelpers.GetHashCode(obj) & int.MaxValue, reference.Value);
        string literal = "abc";
        IntOrRef text = literal;
        Assert.Equal(RuntimeHelpers.GetHashCode(literal), text.Value);
        Assert.Equal("abc", text.As<string>());
        var copy = text;
        Assert.Equal(text.Value, copy.Value);
        IntOrRef nil = new((object?)null);
        Assert.Null(nil.Ref);
        Assert.Equal(0, nil.Value);
    }

    private enum WideFlag : byte { Low = 0, High = 63 }

    private enum ShiftedFlag : byte { Low = 8, High = 15 }

    [Fact]
    public void SetBoundariesAndImages()
    {
        TSet<WideFlag> empty = default;
        Assert.True(empty.IsEmpty);
        Assert.Equal(0, empty.Count);
        var set = TSet<WideFlag>.Of(WideFlag.Low, WideFlag.High);
        Assert.Equal(0x8000000000000001UL, set.Mask);
        Assert.Equal(set, TSet<WideFlag>.FromMask(set.Mask));
        Assert.True(set.Equals((object)TSet<WideFlag>.FromMask(set.Mask)));
        Assert.False(set.Equals(3));
        Assert.Equal(set.GetHashCode(), TSet<WideFlag>.FromMask(set.Mask).GetHashCode());
        Assert.Equal(64, TSet<WideFlag>.Range(WideFlag.Low, WideFlag.High).Count);
        Assert.True(TSet<WideFlag>.Range(WideFlag.High, WideFlag.Low).IsEmpty);
        Assert.Throws<ArgumentOutOfRangeException>(() => TSet<byte>.Of(64));
        Assert.Throws<ArgumentOutOfRangeException>(() => TSet<byte>.Range(0, 64));
        Assert.Throws<ArgumentOutOfRangeException>(() => default(TSet<byte>).With(255));
        Assert.Throws<ArgumentOutOfRangeException>(() => TSet<byte>.WithBounds(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => TSet<byte>.WithBounds(0, 256));
        Assert.Throws<ArgumentOutOfRangeException>(() => TSet<byte>.WithBounds(9, 8));
        Assert.Throws<ArgumentOutOfRangeException>(() => TSet<byte>.WithBounds(8, 15).With(0));
        Assert.False(TSet<byte>.WithBounds(8, 15).Contains(0));
        Assert.Equal(4, TSet<byte>.WithBounds(20, 40).StorageSize);
        Assert.Equal(4, TSet<byte>.WithBounds(0, 23).StorageSize);
        var shifted = TSet<ShiftedFlag>.Of(ShiftedFlag.Low, ShiftedFlag.High);
        Assert.Equal(0x8100UL, shifted.Mask);
        Assert.Equal(129UL, shifted.Image);
        Assert.Equal(1, shifted.StorageSize);
        byte[] image = new byte[1];
        shifted.CopyImageTo(image);
        Assert.Equal(129, image[0]);
        Assert.Throws<ArgumentOutOfRangeException>(() => shifted.CopyImageTo([]));
        var large = TSet<byte>.WithBounds(0, 255).WithRange(0, 255);
        Assert.Equal(large, TSet<byte>.WithBounds(0, 255).Range((byte)0, (byte)255));
        Assert.Equal(256, large.Count);
        image = new byte[32];
        large.CopyImageTo(image);
        Assert.All(image, b => Assert.Equal(255, b));
        large = large.Without(64).Without(128).Without(192);
        large.CopyImageTo(image);
        Assert.Equal(254, image[8]); Assert.Equal(254, image[16]); Assert.Equal(254, image[24]);
        Assert.Equal(253, large.Count);
        var crossing = TSet<byte>.WithBounds(60, 80).With(60).With(64).With(79);
        image = new byte[crossing.StorageSize];
        crossing.CopyImageTo(image);
        Assert.Equal(new byte[] { 16, 1, 128, 0 }, image);
        Assert.Equal(0x800110UL, crossing.Image);
    }

    [Fact]
    public void AllEightBitSetPairsHaveBitwiseValueSemantics()
    {
        for (int a = 0; a < 256; a++)
        {
            var left = TSet<byte>.FromMask((ulong)a);
            var original = left;
            for (int b = 0; b < 256; b++)
            {
                var right = TSet<byte>.FromMask((ulong)b);
                Assert.Equal((ulong)(a | b), (left + right).Mask);
                Assert.Equal((ulong)(a & ~b), (left - right).Mask);
                Assert.Equal((ulong)(a & b), (left & right).Mask);
                Assert.Equal((a & ~b) == 0, left.IsSubsetOf(right));
                Assert.Equal(a == b, left == right);
                Assert.Equal(a != b, left != right);
            }
            Assert.Equal(original, left);
        }
    }
}
