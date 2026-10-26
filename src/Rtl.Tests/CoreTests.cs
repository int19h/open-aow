using System.Runtime.InteropServices;
using Aow.Rtl.Tests.Generated;
using static Aow.Rtl.Rtl;
using DelphiException = Aow.Rtl.Exception;
using Assert = Xunit.Assert;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Aow.Rtl.Tests;

public sealed class CoreTests
{
    [Fact]
    public void MetaclassesAndGeneratedUnit()
    {
        var meta = TSample.Meta.Of<TSampleChild>();
        Assert.Same(meta, MetaRegistry.Of(typeof(TSampleChild)));
        Assert.Same(meta, TObject.Meta.Of<TSampleChild>());
        Assert.Equal(typeof(TSampleChild), meta.Type);
        Assert.Equal("TSampleChild", meta.ClassName);
        Assert.Equal("TSampleChild", TObject.ClassNameOf(typeof(TSampleChild)));
        Assert.Equal(12, meta.InstanceSize);
        Assert.True(meta.InheritsFrom(typeof(TSample)));
        Assert.False(meta.InheritsFrom(typeof(TFailingSample)));
        Assert.Same(TSample.Meta.Of<TSample>(), meta.ClassParent);
        var fresh = Assert.IsType<TSampleChild>(meta.NewInstance());
        Assert.Equal(0, fresh.Value);
        var next = meta.Create(3);
        Assert.Equal(fresh.InstanceId + 1, next.InstanceId);
        Assert.True(fresh.InstanceId >= 1);
        Assert.Equal(13, next.Value);
        Assert.Same(meta, next.ClassType);
        Assert.Equal(2, next.ClassType.ClassID());
        Assert.Throws<EAbstractError>(next.AbstractCall);
        Assert.Throws<InvalidOperationException>(() => new TObject().ClassType);
        Assert.Throws<InvalidOperationException>(() => TSample.Meta.Of<TSample>().ClassParent);
        Assert.Throws<InvalidOperationException>(() => MetaRegistry.Register(typeof(TSample), new TSample.Meta(), () => new TSample(), 0));
        Assert.Throws<InvalidOperationException>(() => MetaRegistry.Register(typeof(Unregistered), meta, () => new Unregistered(), 0));
        Assert.Throws<ArgumentException>(() => MetaRegistry.Register(typeof(string), new TObject.Meta(), () => new TObject(), 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => MetaRegistry.Register(typeof(Unregistered), new TObject.Meta(), () => new Unregistered(), -1));
        TObject? nil = null;
        nil.Free();
        next.Free();
        Assert.True(next.Destroyed);
        new TObject().Destroy();
        Assert.Throws<EInvalidOperation>(() => TSample.Meta.Of<TFailingSample>().Create(1));
        Assert.True(TFailingSample.LastInstance!.Destroyed);
        var result = GeneratedStyleSample.Run();
        Assert.Equal(20, result.Value);
        Assert.Equal((AnsiString)"abcde", result.Name);
        Assert.Equal(TSet<TFlag>.Range(TFlag.First, TFlag.Third), result.Flags);
        var copy = result;
        copy.Name = "x";
        copy.Flags = default;
        Assert.Equal((AnsiString)"abcde", result.Name);
        Assert.Equal(3, result.Flags.Count);
        Assert.Equal(12, Marshal.SizeOf<TSampleRecord>());
        var image = Bytes.Of(ref result);
        Assert.Equal(7, image[0]);
        Assert.Equal(new byte[] { 5, 97, 98, 99, 100, 101 }, image.Slice(1, 6).ToArray());
        Assert.Equal(6, Marshal.SizeOf<FixedArray_byte_6>());
    }

    private sealed class Unregistered : TObject;

    [Fact]
    public void StringsAreImmutableByteValues()
    {
        AnsiString empty = default;
        Assert.Equal(0, empty.Length);
        Assert.Equal("", empty.ToString());
        Assert.Equal((AnsiString)"", empty);
        Assert.Equal(empty.GetHashCode(), ((AnsiString)"").GetHashCode());
        Assert.Equal(empty, (AnsiString)(string?)null);
        Assert.Equal((byte)0, empty.AsPtr().Value);
        Assert.Equal(default, AnsiString.FromPChar(default));
        byte[] buffer = [0, 65, 128, 255];
        var value = AnsiString.FromBytes(buffer);
        buffer[1] = 99;
        Assert.Equal(65, value[2]);
        Assert.Equal((byte)128, ((AnsiString)"\x80")[1]);
        Assert.Equal((byte)128, ((AnsiString)"€")[1]);
        Assert.Equal((AnsiString)"?", (AnsiString)"中");
        Assert.Equal("\0A€ÿ", value.ToString());
        byte[] all = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
        var full = AnsiString.FromBytes(all);
        Assert.Equal(full, (AnsiString)full.ToString());
        var dict = new Dictionary<AnsiString, int> { [full] = 1 };
        Assert.Equal(1, dict[AnsiString.FromBytes(all)]);
        Assert.True(full.Equals((object)AnsiString.FromBytes(all)));
        Assert.False(full.Equals("x"));
        Assert.Equal(0, full.CompareTo(full));
        var pointer = full.AsPtr();
        pointer[1] = 10;
        Assert.Equal(1, full[2]);
        Assert.Equal(0, pointer[256]);
        Assert.Equal((AnsiString)"ab", AnsiString.FromPChar(((AnsiString)"ab\0cd").AsPtr()));
        var original = (AnsiString)"abc";
        var resized = original.Resized(5);
        Assert.Equal(AnsiString.FromBytes(97, 98, 99, 0, 0), resized);
        Assert.Equal((AnsiString)"abc", original);
        Assert.Equal((AnsiString)"aXc", original.WithChar(2, (byte)'X'));
        Assert.Throws<ERangeError>(() => original[0]);
        Assert.Throws<ERangeError>(() => original[4]);
        Assert.Throws<ERangeError>(() => original.WithChar(int.MinValue, 0));
        Assert.Throws<ERangeError>(() => original.Resized(-1));
        Assert.True((AnsiString)"a" <= "a");
        Assert.True((AnsiString)"a" >= "a");
        Assert.False((AnsiString)"a" < "a");
        Assert.False((AnsiString)"a" > "a");
        Assert.False((AnsiString)"a" != "a");
        Assert.Equal((AnsiString)"abcd", original + "d");
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
        Assert.Throws<EInvalidOperation>(() => pointer - Ptr<int>.Alloc(1));
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
        Assert.True(Ptr<int>.CompareMem(pointer, pointer.Realloc(5), 5));
        Assert.False(Ptr<int>.CompareMem(pointer, grown, 1));
        Ptr<int>.Copy(default, default, 0);
        Ptr<int>.Fill(default, -1, 0);
        Assert.True(Ptr<int>.CompareMem(default, default, 0));
        var box = pointer.Box();
        Assert.Equal(pointer, box.Pointer);
        Assert.Equal(pointer, Ptr<int>.Unbox((object)box));
        IntOrRef slot = box;
        Assert.Equal(pointer, Ptr<int>.Unbox(slot));
        Ptr<int>.Unbox(slot).Value = 42;
        Assert.Equal(42, pointer.Value);
        Assert.Throws<EInvalidCast>(() => Ptr<int>.Unbox((object?)null));
        Assert.Throws<EInvalidCast>(() => Ptr<byte>.Unbox(box));
        Assert.Throws<EInvalidCast>(() => Ptr<int>.Unbox((IntOrRef)3));
        string[] words = ["a", "b", "c"];
        var managed = Ptr<string>.To(words, 0);
        Ptr<string>.Copy(managed, managed + 1, 2);
        Assert.Equal(new[] { "a", "a", "b" }, words);
        Ptr<string>.Fill(managed + 1, 2, "z");
        Assert.Equal(new[] { "a", "z", "z" }, words);
        Assert.Throws<EInvalidOperation>(() => Ptr<string>.CompareMem(managed, managed, 1));
        var plusZero = Ptr<double>.To([0.0], 0);
        var minusZero = Ptr<double>.To([-0.0], 0);
        Assert.False(Ptr<double>.CompareMem(plusZero, minusZero, 1));
        Assert.True(Ptr<double>.CompareMem(Ptr<double>.To([double.NaN], 0), Ptr<double>.To([double.NaN], 0), 1));
        var cell = new TSampleRecord[1];
        var recordPointer = Ptr<TSampleRecord>.To(cell, 0);
        IntOrRef payload = recordPointer.Box();
        Ptr<TSampleRecord>.Unbox(payload).Value.Value = 91;
        Assert.Equal(91, cell[0].Value);
    }

    [Fact]
    public void IntOrRefAndVarRecKeepTagsAndIdentity()
    {
        IntOrRef zero = default;
        Assert.Null(zero.Ref);
        Assert.Equal(0, zero.Value);
        IntOrRef number = -123;
        Assert.Equal(-123, number.Value);
        Assert.Null(number.As<TObject>());
        var obj = new TObject();
        IntOrRef reference = obj;
        Assert.Same(obj, reference.Ref);
        Assert.Same(obj, reference.As<TObject>());
        Assert.Equal(obj.InstanceId, reference.Value);
        IntOrRef text = (AnsiString)"abc";
        Assert.Equal(obj.InstanceId + 1, text.Value);
        Assert.Equal((AnsiString)"abc", text.As<AnsiString>());
        var copy = text;
        Assert.Equal(text.Value, copy.Value);
        Assert.Equal(text.Value + 1, new TObject().InstanceId);
        IntOrRef nil = (TObject?)null;
        Assert.Null(nil.Ref);
        Assert.Equal(0, nil.Value);
        TVarRec integer = 4, chr = (byte)65, boolean = true, floating = 1.25;
        TVarRec str = (AnsiString)"hello", instance = obj, cls = TSample.Meta.Of<TSample>();
        Assert.Equal(VarKind.Integer, integer.Kind); Assert.Equal(4, integer.Int);
        Assert.Equal(VarKind.Char, chr.Kind); Assert.Equal(65, chr.Chr);
        TVarRec numericByte = (int)(byte)65;
        Assert.Equal(VarKind.Integer, numericByte.Kind); Assert.Equal(65, numericByte.Int);
        Assert.Equal(VarKind.Boolean, boolean.Kind); Assert.True(boolean.Bool);
        Assert.Equal(VarKind.Extended, floating.Kind); Assert.Equal(1.25, floating.Ext);
        Assert.Equal(VarKind.String, str.Kind); Assert.Equal((AnsiString)"hello", str.Str);
        Assert.Equal(VarKind.Object, instance.Kind); Assert.Same(obj, instance.Obj);
        Assert.Equal(VarKind.Class, cls.Kind); Assert.Same(TSample.Meta.Of<TSample>(), cls.Obj);
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
        Assert.Throws<ERangeError>(() => TSet<byte>.Of(64));
        Assert.Throws<ERangeError>(() => TSet<byte>.Range(0, 64));
        Assert.Throws<ERangeError>(() => default(TSet<byte>).With(255));
        Assert.Throws<ERangeError>(() => TSet<byte>.WithBounds(-1, 2));
        Assert.Throws<ERangeError>(() => TSet<byte>.WithBounds(0, 256));
        Assert.Throws<ERangeError>(() => TSet<byte>.WithBounds(9, 8));
        Assert.Throws<ERangeError>(() => TSet<byte>.WithBounds(8, 15).With(0));
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
        Assert.Throws<ERangeError>(() => shifted.CopyImageTo([]));
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
