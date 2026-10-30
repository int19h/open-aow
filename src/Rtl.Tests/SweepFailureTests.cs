using System.Text.Json;

namespace Aow.Rtl.Tests;

public sealed class SweepFailureTests
{
    public static IEnumerable<object[]> Cases => SweepFailureFixture.Names.SelectMany(name =>
        (name.Contains("MapLevel", StringComparison.Ordinal) ? new[] { true } : new[] { false, true })
        .Select(reject => new object[] { name, reject }));

    [Theory]
    [MemberData(nameof(Cases))]
    public void GuardedFailureMatchesOriginalCleanup(string name, bool reject)
    {
        using var expected = JsonDocument.Parse(typeof(SweepFailureTests).Assembly.GetManifestResourceStream("Aow.Rtl.Tests.SweepFailureBaseline.json")!);
        for (int route = 0; route < 4; route++)
        {
            string actual = SweepFailureFixture.Observe(name, reject, route);
            Assert.Equal(expected.RootElement.GetProperty(name + "." + reject).GetString(), actual);
            Assert.DoesNotContain("no failure", actual, StringComparison.Ordinal);
        }
        Assert.Empty(typeof(Aow.Units.Engine.TEObject).Assembly.GetType(name)!.GetConstructors());
    }
}
