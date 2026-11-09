using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using R = Aow.Rtl.RtlUnit;

namespace Aow.Rtl.Tests;

public sealed class EngineSupportTests
{
    private enum Flag : byte { A, B, C }

    private enum OffsetFlag : byte { A = 8, B, C }

    [InlineArray(4)] private struct FourBytes { private byte first; }

    private sealed class Receiver { public void Event(TObject sender) { } }

    private sealed class EventContext : TObject { }

    private delegate void EventDone(EventContext context);

    [Fact]
    public void MethodPointerCastAdaptsTheContextAndPreservesMulticastOrder()
    {
        var context = new EventContext();
        var calls = new List<int>();
        EventDone first = value => { Assert.Same(context, value); calls.Add(1); };
        EventDone second = value => { Assert.Same(context, value); calls.Add(2); };
        var cast = R.RebindDelegate<TNotifyEvent<TObject>>(first + second)!;
        cast(context);
        Assert.Equal(new[] { 1, 2 }, calls);
        Assert.Throws<InvalidCastException>(() => cast(new TObject()));
        EventDone nullable = value => Assert.Null(value);
        R.RebindDelegate<TNotifyEvent<TObject>>(nullable)!(null!);
        Assert.Null(R.RebindDelegate<TNotifyEvent<TObject>>(null));
        Assert.Same(cast, R.RebindDelegate<TNotifyEvent<TObject>>(cast));
    }

    [Fact]
    public void PhysicalRecordsKeepDelphiOffsets()
    {
        Assert.Equal(8, Marshal.SizeOf<TLargeInteger>());
        Assert.Equal(4, Marshal.OffsetOf<TLargeInteger>(nameof(TLargeInteger.HighPart)).ToInt32());
        TLargeInteger value = new() { QuadPart = 0x12345678ABCDEF01 };
        Assert.Equal(0xABCDEF01u, value.LowPart);
        Assert.Equal(0x12345678, value.HighPart);
        Assert.Equal(16, Marshal.SizeOf<TRect>());
        Assert.Equal(12, Marshal.OffsetOf<TRect>(nameof(TRect.Bottom)).ToInt32());
        Assert.Equal(3, (int)TBltMode.bmCenter);
    }

    [Fact]
    public void BufferHelpersPreservePhysicalAndStringBytes()
    {
        Assert.Equal(new byte[] { 0x78, 0x56 }, R.ValueBytes(0x12345678, 2));
        Assert.Throws<ERangeError>(() => R.ValueBytes(1, 5));
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
        var instance = new TObject();
        Assert.Equal(RuntimeHelpers.GetHashCode(instance), R.ObjectToInt(instance));
        Assert.Equal(0, R.ObjectToInt(null));
    }

    [Fact]
    public void DelegateHalvesRemainDistinctFromFullDelegateEquality()
    {
        var a = new Receiver(); var b = new Receiver();
        TNotifyEvent<TObject> first = a.Event, second = b.Event;
        Assert.Equal(R.MethodCode(first), R.MethodCode(second));
        Assert.NotSame(R.MethodTarget(first), R.MethodTarget(second));
        Assert.Null(R.MethodCode(null)); Assert.Null(R.MethodTarget(null));
    }

    [Fact]
    public void SetTransferUsesTheDelphiImageAndItsLowByte()
    {
        TSet<Flag> value = default;
        R.RwSet(ref value, 1, 17, (bytes, size, id) =>
        {
            Assert.Equal(1, size); Assert.Equal(17, id); bytes[0] = 5;
        });
        Assert.True(value.Contains(Flag.A)); Assert.True(value.Contains(Flag.C)); Assert.False(value.Contains(Flag.B));
        TSet<OffsetFlag> offset = default;
        R.RwSet(ref offset, 1, 0, (bytes, _, _) => bytes[0] = 5, 8);
        Assert.True(offset.Contains(OffsetFlag.A)); Assert.True(offset.Contains(OffsetFlag.C));
    }

    [Fact]
    public void StringReadsCopyBeforeWritingAndKeepPartialTransfers()
    {
        var stream = new TMemoryStream();
        stream.WriteBuffer(new byte[] { 0, 233 });
        stream.Position = 0;
        string value = "abcd", shared = value;
        Assert.Equal(2, R.ReadString(stream, ref value, 1, 2, false));
        Assert.Equal("abcd", shared);
        Assert.Equal("a\0\u00E9d", value);
        stream.Position = 0;
        Assert.Throws<EReadError>(() => R.ReadString(stream, ref value, 0, 4, true));
        Assert.Equal((byte)0, Cp1252.Byte(value[0])); Assert.Equal((byte)233, Cp1252.Byte(value[1]));
    }

    [Fact]
    public void ZlibAdaptersRoundTripAndLeaveTheBackingStreamOpen()
    {
        foreach (var level in Enum.GetValues<TCompressionLevel>())
        {
            byte[] expected = Enumerable.Range(0, 8193).Select(i => (byte)(i * 17)).ToArray();
            var backing = new TMemoryStream();
            var compressor = new TCompressionStream(level, backing);
            Assert.Equal(expected.Length, compressor.Write(expected));
            Assert.Equal(expected.Length, compressor.Seek(0, 1));
            Assert.Throws<EStreamError>(() => compressor.Seek(1, 0));
            compressor.Destroy(); compressor.Destroy();
            Assert.True(backing.Size > 0);
            backing.Position = 0;
            var reader = new TDecompressionStream(backing);
            byte[] actual = new byte[expected.Length];
            Assert.Equal(expected.Length, reader.Read(actual));
            Assert.Equal(expected, actual);
            Assert.Equal(0, reader.Read(actual));
            reader.Destroy();
            backing.Position = 0;
            Assert.True(backing.Read(actual.AsSpan(0, 1)) > 0);
        }
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
