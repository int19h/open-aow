using System.Reflection;
using System.Runtime.InteropServices;

namespace Aow.Rtl.Tests;

public sealed class PointerViewTests
{
    [Fact]
    public void StringPointerConversionKeepsBytesAndTerminatesAtNul()
    {
        var text = "A\u00E9\0B";
        var pointer = Ptr<byte>.Unbox(text);
        Assert.Equal(new byte[] { 65, 233, 0, 66, 0 }, pointer.AsSpan(5).ToArray());
        Assert.Equal("A\u00E9", RtlUnit.StrPas(pointer));
        Assert.Throws<EInvalidCast>(() => Ptr<int>.Unbox(text));
    }

    [StructLayout(LayoutKind.Explicit, Size = 8)]
    private struct Pair
    {
        [FieldOffset(0)] public int First;
        [FieldOffset(4)] public int Second;
    }

    [Fact]
    public void ExplicitBoxViewsKeepTheOriginalStorageAndOffset()
    {
        var pair = Ptr<Pair>.Alloc(1);
        IntOrRef payload = pair.Box();
        var words = Ptr<int>.FromBox(payload.Ref);
        words[1] = 73;
        Assert.Equal(73, pair.Value.Second);
        var bytes = Ptr<byte>.FromBox((words + 1).Box());
        Assert.Equal(4, bytes.ByteIndex);
        Assert.Equal(pair.StorageIdentity, bytes.StorageIdentity);
        Assert.Throws<EInvalidCast>(() => Ptr<string>.FromBox(pair.Box()));
    }

    [Fact]
    public void ViewsAliasFieldsKeepOffsetsAndCopyOverlappingRanges()
    {
        var records = Ptr<Pair>.Alloc(2);
        var words = records.Reinterpret<int>();
        words.Value = 17;
        (words + 1).Value = 23;
        Assert.Equal(17, records.Value.First);
        Assert.Equal(23, records.Value.Second);
        Assert.Equal(4, (words + 1).ByteIndex - words.ByteIndex);
        Assert.Equal(records.Reinterpret<int>(4), words + 1);
        Ptr<int>.Copy(words, words + 1, 2);
        Assert.Equal(new[] { 17, 17, 23, 0 }, words.AsSpan(4).ToArray());
        var list = new TList();
        list.Add((words + 1).Box());
        Assert.Equal(0, list.IndexOf(records.Reinterpret<int>(4).Box()));
        Assert.Equal(-1, list.IndexOf(words.Box()));
        Assert.Throws<EInvalidCast>(() => Ptr<byte>.Unbox(words.Box()));
        Assert.Throws<ArgumentOutOfRangeException>(() => words.AsSpan(5));
        Assert.Throws<IndexOutOfRangeException>(() => words[4]);
        Assert.Throws<IndexOutOfRangeException>(() => words[-1]);
        Assert.True(Ptr<int>.CompareMem(words, words.Realloc(4), 4));
    }

    [Fact]
    public void ViewsRejectManagedReinterpretationAndKeepNil()
    {
        Assert.True(default(Ptr<int>).Reinterpret<byte>().IsNull);
        Assert.Throws<EInvalidCast>(() => Ptr<string>.Alloc(1).Reinterpret<int>());
        Assert.Throws<EInvalidCast>(() => Ptr<int>.Alloc(1).Reinterpret<string>());
    }

    [Fact]
    public void BoxUnboxAndOffsetChainsReadTheRightByteWithoutViews()
    {
        var bytes = Ptr<byte>.Alloc(2048);
        for (int i = 0; i < 2048; i++) bytes[i] = (byte)(i * 7);
        object boxed = bytes.Box();
        for (int i = 0; i < 1000; i++) boxed = (RtlUnit.BytePointer(boxed) + 1).Box();
        var end = RtlUnit.BytePointer(boxed);
        Assert.Equal(1000, end.ByteIndex);
        Assert.Equal(unchecked((byte)(1000 * 7)), end.Value);
        Assert.Same(bytes.Array, end.Array);
        Assert.Null(end.ViewState);
    }

    [Fact]
    public void ViewsOfViewsAddressTheOriginalStorage()
    {
        var bytes = Ptr<byte>.Alloc(2048);
        for (int i = 0; i < 2048; i++) bytes[i] = (byte)(i * 7);
        var words = bytes.Reinterpret<ushort>();
        for (int i = 0; i < 1000; i++)
        {
            var step = Ptr<byte>.FromBox(words.Box()) + 1;
            AssertFlat(step.ViewState);
            words = Ptr<ushort>.FromBox(step.Box());
            AssertFlat(words.ViewState);
        }
        Assert.Equal(1000, words.ByteIndex);
        Assert.Equal((ushort)(bytes[1000] | bytes[1001] << 8), words.Value);
        Assert.Same(bytes.StorageIdentity, words.StorageIdentity);
    }

    // A view captures the storage and a byte offset, never another pointer or view.
    private static void AssertFlat(object? state)
    {
        Assert.NotNull(state);
        foreach (var field in state.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            object? value = field.GetValue(state);
            Assert.False(value is Delegate or IPointerBox, field.Name);
        }
    }
}
