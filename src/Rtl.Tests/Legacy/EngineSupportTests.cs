using System.Diagnostics.CodeAnalysis;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Rtl.Tests;

public sealed class EngineSupportTests
{
    private enum Flag : byte { First, Second, Third }

    private enum OffsetFlag : byte { First = 8, Second, Third }

    [InlineArray(4)] private struct FourBytes { private byte _first; }

    private sealed class Receiver
    {
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The NotifyEvent delegate signature requires sender.")]
        public void Event(object sender) { }
    }

    private sealed class EventContext { }

    private delegate void EventDone(EventContext context);

    [Fact]
    public void MethodPointerCastAdaptsTheContextAndPreservesMulticastOrder()
    {
        EventContext context = new();
        List<int> calls = [];
        EventDone first = value =>
        {
            Assert.Same(context, value);
            calls.Add(1);
        };
        EventDone second = value =>
        {
            Assert.Same(context, value);
            calls.Add(2);
        };
        NotifyEvent<object> cast = DelegateRoutines.RebindDelegate<NotifyEvent<object>>(first + second)!;
        cast(context);
        Assert.Equal(new[] { 1, 2 }, calls);
        _ = Assert.Throws<InvalidCastException>(() => cast(new object()));
        EventDone nullable = Assert.Null;
        DelegateRoutines.RebindDelegate<NotifyEvent<object>>(nullable)!(null!);
        Assert.Null(DelegateRoutines.RebindDelegate<NotifyEvent<object>>(null));
        Assert.Same(cast, DelegateRoutines.RebindDelegate<NotifyEvent<object>>(cast));
    }

    [Fact]
    public void PhysicalRecordsKeepDelphiOffsets()
    {
        Assert.Equal(16, Marshal.SizeOf<RectImage>());
        Assert.Equal(12, Marshal.OffsetOf<RectImage>(nameof(RectImage.Bottom)).ToInt32());
    }

    [Fact]
    public void BufferHelpersPreservePhysicalAndStringBytes()
    {
        Assert.Equal(new byte[] { 0x78, 0x56 }, Heap.ValueBytes(0x12345678, count: 2));
        _ = Assert.Throws<ArgumentOutOfRangeException>(() => Heap.ValueBytes(1, count: 5));
        Assert.Equal(new byte[] { 0, 233, 255 }, Cp1252.Encode("\0\u00E9\u00FF").ToArray());
        byte[] bytes = new byte[3];
        ((Span<byte>)bytes).Fill(unchecked((byte)-1));
        Assert.Equal(new byte[] { 255, 255, 255 }, bytes);
    }

    [Fact]
    public void NumericObjectSlotsRecoverValuesAndObjectIds()
    {
        Assert.Null(IntegerObjects.IntToObject(0));
        foreach (int value in new[] { 1, -1, 17, int.MinValue, int.MaxValue })
        {
            Assert.Same(IntegerObjects.IntToObject(value), IntegerObjects.IntToObject(value));
            Assert.Equal(value, IntegerObjects.ObjectToInt(IntegerObjects.IntToObject(value)));
        }
        object instance = new();
        Assert.Equal(RuntimeHelpers.GetHashCode(instance) & int.MaxValue, IntegerObjects.ObjectToInt(instance));
        Assert.Equal(0, IntegerObjects.ObjectToInt(null));
    }

    [Fact]
    public void DelegateHalvesRemainDistinctFromFullDelegateEquality()
    {
        Receiver a = new();
        Receiver b = new();
        NotifyEvent<object> first = a.Event, second = b.Event;
        Assert.Equal(((Delegate?)first)?.Method, ((Delegate?)second)?.Method);
        Assert.NotSame(((Delegate?)first)?.Target, ((Delegate?)second)?.Target);
        Assert.Null(((Delegate?)null)?.Method);
        Assert.Null(((Delegate?)null)?.Target);
    }

    [Fact]
    public void SetTransferUsesTheDelphiImageAndItsLowByte()
    {
        Set<Flag> value = default;
        value = SetImage.TransferSet(value, size: 1, id: 17, (bytes, size, id) =>
        {
            Assert.Equal(1, size);
            Assert.Equal(17, id);
            bytes[0] = 5;
        });
        Assert.True(value.Contains(Flag.First));
        Assert.True(value.Contains(Flag.Third));
        Assert.False(value.Contains(Flag.Second));
        Set<OffsetFlag> offset = default;
        offset = SetImage.TransferSet(offset, size: 1, id: 0, transfer: (bytes, _, _) => bytes[0] = 5, low: 8);
        Assert.True(offset.Contains(OffsetFlag.First));
        Assert.True(offset.Contains(OffsetFlag.Third));
    }

    [Fact]
    public void StringReadsCopyBeforeWritingAndKeepPartialTransfers()
    {
        MemoryStream stream = new();
        stream.WriteBuffer([0, 233]);
        stream.Position = 0;
        string value = "abcd", shared = value;
        Assert.Equal(2, StreamText.ReadString(stream, ref value, start: 1, count: 2, exact: false));
        Assert.Equal("abcd", shared);
        Assert.Equal("a\0\u00E9d", value);
        stream.Position = 0;
        _ = Assert.Throws<EndOfStreamException>(() => StreamText.ReadString(stream, ref value, start: 0, count: 4, exact: true));
        Assert.Equal((byte)0, Cp1252.Byte(value[0]));
        Assert.Equal((byte)233, Cp1252.Byte(value[1]));
    }

    [Fact]
    public void ZlibAdaptersRoundTripAndLeaveTheBackingStreamOpen()
    {
        foreach (CompressionLevel level in new[] { CompressionLevel.NoCompression, CompressionLevel.Fastest, CompressionLevel.Optimal, CompressionLevel.SmallestSize })
        {
            byte[] expected = [.. Enumerable.Range(0, count: 8193).Select(i => (byte)(i * 17))];
            MemoryStream backing = new();
            ZLibStream compressor = new(backing, compressionLevel: level, leaveOpen: true);
            compressor.Write(expected);
            compressor.Dispose();
            compressor.Dispose();
            Assert.True(backing.Length > 0);
            backing.Position = 0;
            ZLibStream reader = new(backing, mode: CompressionMode.Decompress, leaveOpen: true);
            byte[] actual = new byte[expected.Length];
            reader.ReadExactly(actual);
            Assert.Equal(expected, actual);
            Assert.Equal(0, reader.Read(actual));
            reader.Dispose();
            backing.Position = 0;
            Assert.True(backing.Read(actual.AsSpan(0, length: 1)) > 0);
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
        Action<string> saved = LegacyApplication.MessageHandler;
        try
        {
            string shown = "";
            LegacyApplication.MessageHandler = message => shown = message;
            LegacyApplication.ShowMessage("message");
            Assert.Equal("message", shown);
        }
        finally
        {
            LegacyApplication.MessageHandler = saved;
        }
        Assert.True(new HeapStatus(unchecked((int)GC.GetTotalMemory(false))).TotalAllocated >= 0);
    }
}
