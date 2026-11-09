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
        Assert.Throws<InvalidCastException>(() => Ptr<int>.Unbox(text));
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
        IntOrRef payload = new(new PtrBox<Pair>(pair));
        var words = ((IPointerBox)payload.Ref!).As<int>();
        words[1] = 73;
        Assert.Equal(73, pair.Value.Second);
        var bytes = ((IPointerBox)new PtrBox<int>(words + 1)).As<byte>();
        Assert.Equal(4, bytes.ByteIndex);
        Assert.Equal(pair.StorageIdentity, bytes.StorageIdentity);
        Assert.Throws<InvalidCastException>(() => ((IPointerBox)new PtrBox<Pair>(pair)).As<string>());
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
        list.Add(new PtrBox<int>(words + 1));
        Assert.Equal(0, list.IndexOf(new PtrBox<int>(records.Reinterpret<int>(4))));
        Assert.Equal(-1, list.IndexOf(new PtrBox<int>(words)));
        Assert.Throws<InvalidCastException>(() => Ptr<byte>.Unbox(new PtrBox<int>(words)));
        Assert.Throws<ArgumentOutOfRangeException>(() => words.AsSpan(5));
        Assert.Throws<IndexOutOfRangeException>(() => words[4]);
        Assert.Throws<IndexOutOfRangeException>(() => words[-1]);
    }

    [Fact]
    public void ViewsRejectManagedReinterpretationAndKeepNil()
    {
        Assert.True(default(Ptr<int>).Reinterpret<byte>().IsNull);
        Assert.Throws<InvalidCastException>(() => Ptr<string>.Alloc(1).Reinterpret<int>());
        Assert.Throws<InvalidCastException>(() => Ptr<int>.Alloc(1).Reinterpret<string>());
    }

    [Fact]
    public void BoxUnboxAndOffsetChainsReadTheRightByteWithoutViews()
    {
        var bytes = Ptr<byte>.Alloc(2048);
        for (int i = 0; i < 2048; i++) bytes[i] = (byte)(i * 7);
        object boxed = new PtrBox<byte>(bytes);
        for (int i = 0; i < 1000; i++) boxed = new PtrBox<byte>(Ptr<byte>.Unbox(boxed) + 1);
        var end = Ptr<byte>.Unbox(boxed);
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
            var step = ((IPointerBox)new PtrBox<ushort>(words)).As<byte>() + 1;
            AssertFlat(step.ViewState);
            words = ((IPointerBox)new PtrBox<byte>(step)).As<ushort>();
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
            if (field.Name == "captureOrdinal") continue;
            object? value = field.GetValue(state);
            Assert.False(value is Delegate or IPointerBox, field.Name);
        }
    }
}
