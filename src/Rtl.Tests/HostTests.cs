using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class HostTests
{
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
