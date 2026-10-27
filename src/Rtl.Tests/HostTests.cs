using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class HostTests
{
    [Fact]
    public void RandomTracingRecordsSeedWritesAndBothDrawsWithoutChangingTheStream()
    {
        int seed = Rtl.RandSeed;
        var previous = Rtl.RandomTrace;
        var events = new List<RandomTraceEvent>();
        try
        {
            Rtl.RandomTrace = events.Add;
            Rtl.RandSeed = 123456789;
            int integer = Rtl.Random(20);
            double fraction = Rtl.Random();
            int after = Rtl.RandSeed;
            Rtl.RandomTrace = null;
            Rtl.RandSeed = 123456789;
            Assert.Equal(integer, Rtl.Random(20));
            Assert.Equal(fraction, Rtl.Random());
            Assert.Equal(after, Rtl.RandSeed);
            Assert.Equal(3, events.Count);
            Assert.Equal("seed", events[0].Kind);
            Assert.Equal(123456789, events[0].After);
            Assert.Equal(new RandomTraceEvent("random", 20, integer, 123456789, events[1].After), events[1]);
            Assert.Equal(new RandomTraceEvent("random", null, fraction, events[1].After, after), events[2]);
        }
        finally { Rtl.RandomTrace = previous; Rtl.RandSeed = seed; }
    }

    [Fact]
    public void SeededRandomizeLeavesTheCurrentStreamAlone()
    {
        int seed = Rtl.RandSeed;
        bool suppressed = Rtl.SuppressRandomize;
        try
        {
            Rtl.RandSeed = 123456789;
            Rtl.SuppressRandomize = true;
            int first = Rtl.Random(1000);
            int afterFirst = Rtl.RandSeed;
            Rtl.Randomize();
            Assert.Equal(afterFirst, Rtl.RandSeed);
            int next = Rtl.Random(1000);
            Rtl.RandSeed = 123456789;
            Assert.Equal(first, Rtl.Random(1000));
            Assert.Equal(next, Rtl.Random(1000));
        }
        finally { Rtl.RandSeed = seed; Rtl.SuppressRandomize = suppressed; }
    }

    [Theory]
    [InlineData(4, 6)]
    [InlineData(0x104, 7)]
    [InlineData(0x203, 2)]
    [InlineData(0x102, 4)]
    public void DialogDefaultButtonUsesWindowsResultCodes(int flags, int expected)
    {
        var previous = Rtl.Dialogs;
        try
        {
            Rtl.Dialogs = new Defaults();
            Assert.Equal(expected, Rtl.MessageBox(0, "message", "caption", flags));
        }
        finally { Rtl.Dialogs = previous; }
    }

    private sealed class Defaults : IDialogService
    {
        public int Show(DialogRequest request) => request.DefaultResult;
    }
}
