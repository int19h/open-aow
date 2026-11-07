using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class HostTests
{
    [Fact]
    public void RandomTracingRecordsSeedWritesAndBothDrawsWithoutChangingTheStream()
    {
        int seed = RtlUnit.RandSeed;
        var previous = RtlUnit.RandomTrace;
        var events = new List<RandomTraceEvent>();
        try
        {
            RtlUnit.RandomTrace = events.Add;
            RtlUnit.RandSeed = 123456789;
            int integer = RtlUnit.Random(20);
            double fraction = RtlUnit.Random();
            int after = RtlUnit.RandSeed;
            RtlUnit.RandomTrace = null;
            RtlUnit.RandSeed = 123456789;
            Assert.Equal(integer, RtlUnit.Random(20));
            Assert.Equal(fraction, RtlUnit.Random());
            Assert.Equal(after, RtlUnit.RandSeed);
            Assert.Equal(3, events.Count);
            Assert.Equal("seed", events[0].Kind);
            Assert.Equal(123456789, events[0].After);
            Assert.Equal(new RandomTraceEvent("random", 20, integer, 123456789, events[1].After), events[1]);
            Assert.Equal(new RandomTraceEvent("random", null, fraction, events[1].After, after), events[2]);
        }
        finally { RtlUnit.RandomTrace = previous; RtlUnit.RandSeed = seed; }
    }

    [Fact]
    public void SeededRandomizeLeavesTheCurrentStreamAlone()
    {
        int seed = RtlUnit.RandSeed;
        bool suppressed = RtlUnit.SuppressRandomize;
        try
        {
            RtlUnit.RandSeed = 123456789;
            RtlUnit.SuppressRandomize = true;
            int first = RtlUnit.Random(1000);
            int afterFirst = RtlUnit.RandSeed;
            RtlUnit.Randomize();
            Assert.Equal(afterFirst, RtlUnit.RandSeed);
            int next = RtlUnit.Random(1000);
            RtlUnit.RandSeed = 123456789;
            Assert.Equal(first, RtlUnit.Random(1000));
            Assert.Equal(next, RtlUnit.Random(1000));
        }
        finally { RtlUnit.RandSeed = seed; RtlUnit.SuppressRandomize = suppressed; }
    }

    [Theory]
    [InlineData(4, 6)]
    [InlineData(0x104, 7)]
    [InlineData(0x203, 2)]
    [InlineData(0x102, 4)]
    public void DialogDefaultButtonUsesWindowsResultCodes(int flags, int expected)
    {
        var previous = RtlUnit.Dialogs;
        try
        {
            RtlUnit.Dialogs = new Defaults();
            Assert.Equal(expected, RtlUnit.MessageBox(0, "message", "caption", flags));
        }
        finally { RtlUnit.Dialogs = previous; }
    }

    private sealed class Defaults : IDialogService
    {
        public int Show(DialogRequest request) => request.DefaultResult;
    }
}
