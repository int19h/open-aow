using System.Numerics;
using System.Runtime.InteropServices;
using Aow.Rtl.Tests.Generated;
using static Aow.Rtl.DelphiRandom;
using static Aow.Rtl.DelphiStrings;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class CoreTests
{
    private const Flags AllFlags = Flags.First | Flags.Second | Flags.Third;

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
            _ = Random(10000);
            Randomize();
            Assert.Equal(first, Random(10000));
            RandomizeSeed = null;
            int retained = RandSeed;
            Randomize();
            Assert.Equal(retained, RandSeed);
        }
        finally
        {
            RandomizeSeed = null;
            SuppressRandomize = suppressed;
            RandSeed = previous;
        }
    }

    [Fact]
    public void ObjectsAndGeneratedUnit()
    {
        Assert.Equal(nameof(SampleChild), NameTable.Delphi(typeof(SampleChild)));
        Assert.Equal(nameof(SampleChild), NameTable.Stored(typeof(SampleChild)));
        Assert.Equal("TPersistent", NameTable.Delphi(typeof(Persistent)));
        Assert.Equal(nameof(Persistent), NameTable.Stored(typeof(Persistent)));
        SampleChild fresh = new(0);
        SampleChild next = new(3);
        Assert.NotSame(fresh, next);
        Assert.Equal(13, next.Value);
        Assert.Equal(2, next.ClassId);
        _ = Assert.Throws<AbstractErrorException>(next.AbstractCall);
        object? nil = null;
        (nil as IDisposable)?.Dispose();
        next.Dispose();
        Assert.True(next.Destroyed);
        _ = Assert.Throws<InvalidOperationException>(() => FailingSample.Create(1));
        Assert.True(FailingSample.LastInstance!.Destroyed);
        SampleRecord result = GeneratedStyleSample.Run();
        Assert.Equal(20, result.Value);
        Assert.Equal("abcde", result.Name);
        Assert.Equal(AllFlags, result.Flags);
        SampleRecord copy = result;
        copy.Name = "x";
        copy.Flags = default;
        Assert.Equal("abcde", result.Name);
        Assert.Equal(3, BitOperations.PopCount((uint)result.Flags));
        Assert.Equal(12, Marshal.SizeOf<SampleRecord>());
        Span<byte> image = Bytes.Of(ref result);
        Assert.Equal(7, image[0]);
        Assert.Equal(new byte[] { 5, 97, 98, 99, 100, 101 }, image.Slice(1, length: 6).ToArray());
        Assert.Equal(6, Marshal.SizeOf<ByteArray6>());
    }

    private sealed class Unregistered;

    [Fact]
    public void StringsAreImmutableValuesWithOneCharPerByte()
    {
        using StringMode mode = new(false);
        string? nil = null;
        Assert.Equal(0, RequireString(nil).Length);
        Assert.True(IsEmpty(nil));
        Assert.Equal((byte)0, Ptr<byte>.Unbox("").Value);
        Assert.Equal("", PCharStrings.FromPChar(default));
        byte[] buffer = [0, 65, 128, 255];
        string value = Cp1252.Decode(buffer);
        buffer[1] = 99;
        Assert.Equal('A', value[1]);
        Assert.Equal("\0A\u20ac\u00ff", value);
        Assert.Equal((byte)63, Cp1252.Byte('\u0080'));
        Assert.Equal((byte)128, Cp1252.Byte('\u20ac'));
        Assert.Equal("?", Cp1252.Normalize("\u4e2d"));
        byte[] all = [.. Enumerable.Range(0, count: 256).Select(i => (byte)i)];
        string full = Cp1252.Decode(all);
        Assert.Equal(256, full.Length);
        Assert.Equal(all, Cp1252.Encode(full));
        Dictionary<string, int> dictionary = new() { [full] = 1 };
        Assert.Equal(1, dictionary[Cp1252.Decode(all)]);
        Assert.Equal(0, Cp1252.Compare(full, Cp1252.Decode(all)));
        // A PChar of a string is a zero-terminated copy, so a write through it leaves the string.
        Ptr<byte> pointer = Ptr<byte>.Unbox(full);
        pointer[1] = 10;
        Assert.Equal('\u0001', full[1]);
        Assert.Equal(0, pointer[256]);
        Assert.Equal("ab", PCharStrings.FromPChar(Ptr<byte>.Unbox("ab\0cd")));
        string original = "abc";
        Assert.Equal("abc\0\0", Resized(original, length: 5));
        Assert.Equal("abc", original);
        Assert.Equal("aXc", WithChar(original, index: 2, character: 'X'));
        // The indexer and WithChar report a string index. A negative length is an argument.
        _ = Assert.Throws<IndexOutOfRangeException>(() => original[3]);
        _ = Assert.Throws<IndexOutOfRangeException>(() => original[-1]);
        _ = Assert.Throws<IndexOutOfRangeException>(() => WithChar(original, index: 0, character: 'X'));
        _ = Assert.Throws<IndexOutOfRangeException>(() => WithChar(original, index: 4, character: 'X'));
        _ = Assert.Throws<IndexOutOfRangeException>(() => WithChar(original, index: int.MinValue, character: '\0'));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Resized(original, length: -1));
        Assert.Equal(0, Cp1252.Compare("a", "a"));
        Assert.True(Cp1252.Compare("a", "b") < 0);
        Assert.True(Cp1252.Compare("\u20ac", "\u00c4") < 0);
        Assert.Equal("abcd", $"{original}d");
    }

    [Fact]
    public void PointersAliasCellsAndSupportManagedCopies()
    {
        Ptr<int> nil = default;
        Assert.True(nil.IsNull);
        Assert.True(nil == default);
        Assert.True(Ptr<int>.Alloc(0).IsNull);
        Ptr<int> pointer = Ptr<int>.Alloc(5);
        Assert.False(pointer.IsNull);
        Assert.Equal(0, pointer.Index);
        Assert.Equal(5, pointer.Array!.Length);
        pointer.Value = 1;
        pointer[1] = 2;
        pointer[2] = 3;
        Assert.Equal(3, pointer.Array[2]);
        Assert.Equal(2, (pointer + 1).Value);
        Assert.Equal(pointer, pointer + 2 - 2);
        Assert.Equal(2, pointer + 2 - pointer);
        Assert.True(pointer != pointer + 1);
        Assert.True(pointer.Equals((object)Ptr<int>.To(pointer.Array, index: 0)));
        Assert.False(pointer.Equals(0));
        Assert.Equal(pointer.GetHashCode(), Ptr<int>.To(pointer.Array, index: 0).GetHashCode());
        _ = Assert.Throws<InvalidOperationException>(() => pointer - Ptr<int>.Alloc(1));
        Ptr<int>.Copy(pointer, destination: pointer + 1, count: 3);
        Assert.Equal((int[])[1, 1, 2, 3, 0], pointer.Array);
        Ptr<int>.Copy(pointer + 1, destination: pointer, count: 3);
        Assert.Equal((int[])[1, 2, 3, 3, 0], pointer.Array);
        Ptr<int>.Fill(pointer + 3, count: 2, value: 8);
        Assert.Equal((int[])[1, 2, 3, 8, 8], pointer.Array);
        Ptr<int> grown = pointer.Realloc(7);
        Assert.Equal((int[])[1, 2, 3, 8, 8, 0, 0], grown.Array);
        grown.Value = 9;
        Assert.Equal(1, pointer.Value);
        Assert.Equal((int[])[2, 3], (pointer + 1).Realloc(2).Array);
        Assert.True(pointer.Realloc(0).IsNull);
        Assert.Equal(2, nil.Realloc(2).Array!.Length);
        Ptr<int>.Copy(default, destination: default, count: 0);
        Ptr<int>.Fill(default, count: -1, value: 0);
        PtrBox<int> box = new(pointer);
        Assert.Equal(pointer, box.Pointer);
        Assert.Equal(pointer, Ptr<int>.Unbox(box));
        object slot = box;
        Assert.Equal(pointer, Ptr<int>.Unbox(slot));
        Ptr<int>.Unbox(slot).Value = 42;
        Assert.Equal(42, pointer.Value);
        _ = Assert.Throws<InvalidCastException>(() => Ptr<int>.Unbox((object?)null));
        _ = Assert.Throws<InvalidCastException>(() => Ptr<byte>.Unbox(box));
        string[] words = ["a", "b", "c"];
        Ptr<string> managed = Ptr<string>.To(words, index: 0);
        Ptr<string>.Copy(managed, destination: managed + 1, count: 2);
        Assert.Equal(new[] { "a", "a", "b" }, words);
        Ptr<string>.Fill(managed + 1, count: 2, value: "z");
        Assert.Equal(new[] { "a", "z", "z" }, words);
        SampleRecord[] cell = new SampleRecord[1];
        object payload = new PtrBox<SampleRecord>(Ptr<SampleRecord>.To(cell, index: 0));
        Ptr<SampleRecord>.Unbox(payload).Value.Value = 91;
        Assert.Equal(91, cell[0].Value);
    }
}
