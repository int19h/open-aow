using System.Globalization;
using System.Text.Json;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class GateSwitchesTests
{
    [Fact]
    public void TestProcessRunsWithInvariantGlobalization()
    {
        Assert.True(AppContext.TryGetSwitch(GateSwitches.InvariantSwitch, out bool on));
        Assert.True(on);
        Assert.True(GateSwitches.InvariantGlobalization);
        Assert.Null(GateSwitches.CultureError("test"));
        Assert.Equal("", CultureInfo.CurrentCulture.Name);
        Assert.Throws<CultureNotFoundException>(() => CultureInfo.GetCultureInfo("de-DE"));
    }

    [Fact]
    public void CensusCountsEachKindUnderItsTypeOrBase()
    {
        Assert.Equal(0, ExceptionCensus.KindOf(new NullReferenceException()));
        Assert.Equal(1, ExceptionCensus.KindOf(new IndexOutOfRangeException()));
        Assert.Equal(2, ExceptionCensus.KindOf(new ArgumentOutOfRangeException(paramName: null, "range")));
        Assert.Equal(3, ExceptionCensus.KindOf(new AccessViolationException("access")));
        Assert.Equal(4, ExceptionCensus.KindOf(new FormatException("convert")));
        Assert.Equal(5, ExceptionCensus.KindOf(new NullStringException()));
        Assert.Equal(2, ExceptionCensus.KindOf(new DerivedRange()));
        Assert.Equal(-1, ExceptionCensus.KindOf(new InvalidOperationException()));
    }

    [Fact]
    public void CensusAppendsOneJsonLinePerRun()
    {
        string directory = Path.Combine(Path.GetTempPath(), "aow-census-" + Guid.NewGuid().ToString("N"));
        string file = Path.Combine(directory, "nested", "census.jsonl");
        try
        {
            ExceptionCensus.Append(file, ExceptionCensus.Line("Game", ["--fixture", "a b"], [1, 2, 3, 4, 5, 0]));
            ExceptionCensus.Append(file, ExceptionCensus.Line("Headless", [], [0, 0, 0, 0, 0, 7]));
            string[] lines = File.ReadAllText(file).Split('\n');
            Assert.Equal(3, lines.Length);
            Assert.Equal("", lines[2]);
            using var first = JsonDocument.Parse(lines[0]);
            Assert.Equal("Game", first.RootElement.GetProperty("host").GetString());
            Assert.Equal(["--fixture", "a b"], first.RootElement.GetProperty("arguments").EnumerateArray().Select(a => a.GetString()!));
            Assert.Equal(ExceptionCensus.Kinds, first.RootElement.GetProperty("counts").EnumerateObject().Select(p => p.Name));
            Assert.Equal([1L, 2, 3, 4, 5, 0], first.RootElement.GetProperty("counts").EnumerateObject().Select(p => p.Value.GetInt64()));
            using var second = JsonDocument.Parse(lines[1]);
            Assert.Equal(7, second.RootElement.GetProperty("counts").GetProperty("NullStringException").GetInt64());
            Assert.Throws<ArgumentException>(() => ExceptionCensus.Line("Game", [], [0]));
        }
        finally
        {
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
        }
    }

    private sealed class DerivedRange() : ArgumentOutOfRangeException(paramName: null, "derived");
}
