using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class HostTests
{
    [Fact]
    public void RandomTracingRecordsSeedWritesAndBothDrawsWithoutChangingTheStream()
    {
        int seed = DelphiRandom.RandSeed;
        Action<RandomTraceEvent>? previous = DelphiRandom.RandomTrace;
        List<RandomTraceEvent> events = [];
        try
        {
            DelphiRandom.RandomTrace = events.Add;
            DelphiRandom.RandSeed = 123456789;
            int integer = DelphiRandom.Random(20);
            double fraction = DelphiRandom.Random();
            int after = DelphiRandom.RandSeed;
            DelphiRandom.RandomTrace = null;
            DelphiRandom.RandSeed = 123456789;
            Assert.Equal(integer, DelphiRandom.Random(20));
            Assert.Equal(fraction, DelphiRandom.Random());
            Assert.Equal(after, DelphiRandom.RandSeed);
            Assert.Equal(3, events.Count);
            Assert.Equal("seed", events[0].Kind);
            Assert.Equal(123456789, events[0].After);
            Assert.Equal(new RandomTraceEvent("random", 20, integer, 123456789, events[1].After), events[1]);
            Assert.Equal(new RandomTraceEvent("random", null, fraction, events[1].After, after), events[2]);
        }
        finally { DelphiRandom.RandomTrace = previous; DelphiRandom.RandSeed = seed; }
    }

    [Fact]
    public void SeededRandomizeLeavesTheCurrentStreamAlone()
    {
        int seed = DelphiRandom.RandSeed;
        bool suppressed = DelphiRandom.SuppressRandomize;
        try
        {
            DelphiRandom.RandSeed = 123456789;
            DelphiRandom.SuppressRandomize = true;
            int first = DelphiRandom.Random(1000);
            int afterFirst = DelphiRandom.RandSeed;
            DelphiRandom.Randomize();
            Assert.Equal(afterFirst, DelphiRandom.RandSeed);
            int next = DelphiRandom.Random(1000);
            DelphiRandom.RandSeed = 123456789;
            Assert.Equal(first, DelphiRandom.Random(1000));
            Assert.Equal(next, DelphiRandom.Random(1000));
        }
        finally { DelphiRandom.RandSeed = seed; DelphiRandom.SuppressRandomize = suppressed; }
    }

    [Theory]
    [InlineData(4, 6)]
    [InlineData(0x104, 7)]
    [InlineData(0x203, 2)]
    [InlineData(0x102, 4)]
    public void DialogDefaultButtonUsesWindowsResultCodes(int flags, int expected)
    {
        IDialogService? previous = RtlHost.Dialogs;
        try
        {
            RtlHost.Dialogs = new Defaults();
            Assert.Equal(expected, RtlHost.MessageBox(0, "message", "caption", flags));
        }
        finally { RtlHost.Dialogs = previous; }
    }

    private sealed class Defaults : IDialogService
    {
        public int Show(DialogRequest request) => request.DefaultResult;
    }
}
