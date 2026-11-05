using Aow.Convert;

namespace Aow.Formats.Tests;

/// <summary>Every generated fixture case of aowconv check, one test each.</summary>
public sealed class FixtureTests : FormatsTest
{
    public static TheoryData<string> Cases()
    {
        var data = new TheoryData<string>();
        foreach (var c in Fixtures.All()) data.Add(c.Group + "/" + c.Name);
        return data;
    }

    [Theory]
    [MemberData(nameof(Cases))]
    public void FixtureMatchesItsExpectedResult(string name)
    {
        var c = Fixtures.All().Single(f => f.Group + "/" + f.Name == name);
        var outcome = c.Run();
        Assert.True(outcome.Matched, outcome.Detail);
    }

    [Fact]
    public void CaseNamesAreUnique()
    {
        var names = Fixtures.All().Select(c => c.Group + "/" + c.Name).ToList();
        Assert.Equal(names.Count, names.Distinct(StringComparer.Ordinal).Count());
    }
}
