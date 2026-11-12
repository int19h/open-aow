using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using R = Aow.Rtl.RtlUnit;

namespace Aow.Rtl.Tests;

public sealed class EngineSupportTests
{
    private enum Flag : byte { First, Second, Third }

    private enum OffsetFlag : byte { First = 8, Second, Third }

    [InlineArray(4)] private struct FourBytes { private byte _first; }

    private sealed class Receiver { public void Event(object sender) { } }

    private sealed class EventContext { }

    private delegate void EventDone(EventContext context);

    [Fact]
    public void MethodPointerCastAdaptsTheContextAndPreservesMulticastOrder()
    {
        var context = new EventContext();
        var calls = new List<int>();
        EventDone first = value => { Assert.Same(context, value); calls.Add(1); };
        EventDone second = value => { Assert.Same(context, value); calls.Add(2); };
        var cast = R.RebindDelegate<NotifyEvent<object>>(first + second)!;
        cast(context);
        Assert.Equal(new[] { 1, 2 }, calls);
        Assert.Throws<InvalidCastException>(() => cast(new object()));
        EventDone nullable = value => Assert.Null(value);
        R.RebindDelegate<NotifyEvent<object>>(nullable)!(null!);
        Assert.Null(R.RebindDelegate<NotifyEvent<object>>(null));
        Assert.Same(cast, R.RebindDelegate<NotifyEvent<object>>(cast));
    }

    [Fact]
    public void PhysicalRecordsKeepDelphiOffsets()
    {
        Assert.Equal(8, Marshal.SizeOf<LargeInteger>());
        Assert.Equal(4, Marshal.OffsetOf<LargeInteger>(nameof(LargeInteger.HighPart)).ToInt32());
        LargeInteger value = new() { QuadPart = 0x12345678ABCDEF01 };
        Assert.Equal(0xABCDEF01u, value.LowPart);
        Assert.Equal(0x12345678, value.HighPart);
        Assert.Equal(16, Marshal.SizeOf<Rect>());
        Assert.Equal(12, Marshal.OffsetOf<Rect>(nameof(Rect.Bottom)).ToInt32());
    }

    [Fact]
    public void BufferHelpersPreservePhysicalAndStringBytes()
    {
        Assert.Equal(new byte[] { 0x78, 0x56 }, R.ValueBytes(0x12345678, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => R.ValueBytes(1, 5));
        string value = "\0\u00E9\u00FF";
        Assert.Equal(new byte[] { 0, 233, 255 }, Cp1252.Encode(value).ToArray());
        byte[] bytes = new byte[3];
        R.FillBytes(bytes, -1);
        Assert.Equal(new byte[] { 255, 255, 255 }, bytes);
        FourBytes left = default, right = default;
        left[3] = 7;
        Assert.False(R.ArrayEquals<FourBytes, byte>(left, right));
        right[3] = 7;
        Assert.True(R.ArrayEquals<FourBytes, byte>(left, right));
    }

    [Fact]
    public void NumericObjectSlotsRecoverValuesAndObjectIds()
    {
        Assert.Null(R.IntToObject(0));
        foreach (int value in new[] { 1, -1, 17, int.MinValue, int.MaxValue })
        {
            Assert.Same(R.IntToObject(value), R.IntToObject(value));
            Assert.Equal(value, R.ObjectToInt(R.IntToObject(value)));
        }
        var instance = new object();
        Assert.Equal(RuntimeHelpers.GetHashCode(instance) & int.MaxValue, R.ObjectToInt(instance));
        Assert.Equal(0, R.ObjectToInt(null));
    }

    [Fact]
    public void DelegateHalvesRemainDistinctFromFullDelegateEquality()
    {
        var a = new Receiver(); var b = new Receiver();
        NotifyEvent<object> first = a.Event, second = b.Event;
        Assert.Equal(R.MethodCode(first), R.MethodCode(second));
        Assert.NotSame(R.MethodTarget(first), R.MethodTarget(second));
        Assert.Null(R.MethodCode(null)); Assert.Null(R.MethodTarget(null));
    }

    [Fact]
    public void SetTransferUsesTheDelphiImageAndItsLowByte()
    {
        Set<Flag> value = default;
        R.RwSet(ref value, 1, 17, (bytes, size, id) =>
        {
            Assert.Equal(1, size); Assert.Equal(17, id); bytes[0] = 5;
        });
        Assert.True(value.Contains(Flag.First)); Assert.True(value.Contains(Flag.Third)); Assert.False(value.Contains(Flag.Second));
        Set<OffsetFlag> offset = default;
        R.RwSet(ref offset, 1, 0, (bytes, _, _) => bytes[0] = 5, 8);
        Assert.True(offset.Contains(OffsetFlag.First)); Assert.True(offset.Contains(OffsetFlag.Third));
    }

    [Fact]
    public void StringReadsCopyBeforeWritingAndKeepPartialTransfers()
    {
        var stream = new MemoryStream();
        stream.WriteBuffer(new byte[] { 0, 233 });
        stream.Position = 0;
        string value = "abcd", shared = value;
        Assert.Equal(2, R.ReadString(stream, ref value, 1, 2, false));
        Assert.Equal("abcd", shared);
        Assert.Equal("a\0\u00E9d", value);
        stream.Position = 0;
        Assert.Throws<EndOfStreamException>(() => R.ReadString(stream, ref value, 0, 4, true));
        Assert.Equal((byte)0, Cp1252.Byte(value[0])); Assert.Equal((byte)233, Cp1252.Byte(value[1]));
    }

    [Fact]
    public void ZlibAdaptersRoundTripAndLeaveTheBackingStreamOpen()
    {
        foreach (var level in new[] { CompressionLevel.NoCompression, CompressionLevel.Fastest, CompressionLevel.Optimal, CompressionLevel.SmallestSize })
        {
            byte[] expected = Enumerable.Range(0, 8193).Select(i => (byte)(i * 17)).ToArray();
            var backing = new MemoryStream();
            var compressor = new ZLibStream(backing, level, true);
            compressor.Write(expected);
            compressor.Dispose(); compressor.Dispose();
            Assert.True(backing.Length > 0);
            backing.Position = 0;
            var reader = new ZLibStream(backing, CompressionMode.Decompress, true);
            byte[] actual = new byte[expected.Length];
            reader.ReadExactly(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(0, reader.Read(actual));
            reader.Dispose();
            backing.Position = 0;
            Assert.True(backing.Read(actual.AsSpan(0, 1)) > 0);
        }
    }

    [Fact]
    public void CompressionLevelByteKeepsTheFormerEnumMapping()
    {
        Assert.Equal(CompressionLevel.NoCompression, ZLibLevels.ForByte(0));
        Assert.Equal(CompressionLevel.Fastest, ZLibLevels.ForByte(1));
        Assert.Equal(CompressionLevel.Optimal, ZLibLevels.ForByte(2));
        Assert.Equal(CompressionLevel.SmallestSize, ZLibLevels.ForByte(3));
        Assert.Equal(CompressionLevel.Optimal, ZLibLevels.ForByte(4));
        Assert.Equal(CompressionLevel.Optimal, ZLibLevels.ForByte(255));
    }

    [Fact]
    public void HostBoundaryHooksAreExplicitAndReplaceable()
    {
        var saved = R.MessageHandler;
        try
        {
            string shown = "";
            R.MessageHandler = message => shown = message;
            R.ShowMessage("message");
            Assert.Equal("message", shown);
        }
        finally { R.MessageHandler = saved; }
        Assert.True(R.GetHeapStatus().TotalAllocated >= 0);
    }
}
