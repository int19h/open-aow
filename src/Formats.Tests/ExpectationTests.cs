using System.Security.Cryptography;
using System.Text;
using Aow.Convert;

namespace Aow.Formats.Tests;

/// <summary>The pinned layout expectation of aowconv check: drift and a changed census fail.</summary>
public sealed class ExpectationTests : FormatsTest, IDisposable
{
    private readonly string directory = Path.Combine(Path.GetTempPath(), "aowconv-expect-" + Guid.NewGuid().ToString("N"));

    public ExpectationTests() => Directory.CreateDirectory(directory);

    public void Dispose() => Directory.Delete(directory, true);

    private string Write(string censusSha)
    {
        var census = Encoding.UTF8.GetBytes("{\"corpus\": [], \"summary\": {\"unexplainedSites\": 0}}");
        File.WriteAllBytes(Path.Combine(directory, "census.json"), census);
        string sha = censusSha == "actual" ? System.Convert.ToHexStringLower(SHA256.HashData(census)) : censusSha;
        string path = Path.Combine(directory, "expect.json");
        File.WriteAllText(path, """
            {
              "format": "aowconv-check-expectation",
              "version": 1,
              "census": {"path": "census.json", "sha256": "SHA"},
              "layout": {"parts": 3, "files": 2, "categories": {"gaps": 1, "table-form-order": 2}, "profiles": {"world-hsm": {"parts": 3, "files": 2}}},
              "legacyRules": {"LEGACY_VALLEY01_SPRITE": {"occurrences": 1, "files": ["install:Scenario/Campaign/Valley01.CSM"]}},
              "caseResidues": 0
            }
            """.Replace("SHA", sha, StringComparison.Ordinal));
        return path;
    }

    private static SortedDictionary<string, (int, List<string>)> Rules(params string[] files) =>
        new(StringComparer.Ordinal) { ["LEGACY_VALLEY01_SPRITE"] = (files.Length, files.ToList()) };

    [Fact]
    public void MatchingCountsHaveNoDrift()
    {
        var (_, expectation) = Expectation.Load(Write("actual"));
        var problems = expectation!.Compare(3, 2,
            new(StringComparer.Ordinal) { ["gaps"] = 1, ["table-form-order"] = 2 },
            new(StringComparer.Ordinal) { ["world-hsm"] = (3, 2) },
            Rules("install:Scenario/Campaign/Valley01.CSM"), 0);
        Assert.Empty(problems);
    }

    [Fact]
    public void EveryDifferenceIsDrift()
    {
        var (_, expectation) = Expectation.Load(Write("actual"));
        var problems = expectation!.Compare(4, 2,
            new(StringComparer.Ordinal) { ["gaps"] = 2, ["table-form-order"] = 2 },
            new(StringComparer.Ordinal) { ["world-hsm"] = (4, 2), ["tactical-hsm"] = (0, 0) },
            Rules("install:Scenario/Campaign/Valley01.CSM", "install:Scenario/Campaign/Other.csm"), 1);
        Assert.Contains(problems, p => p.StartsWith("layout parts", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("category gaps", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("profile world-hsm", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("case residues", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("legacy rule LEGACY_VALLEY01_SPRITE occurrences", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("legacy rule LEGACY_VALLEY01_SPRITE files", StringComparison.Ordinal));
    }

    [Fact]
    public void AChangedCensusIsRefused()
    {
        var error = Assert.Throws<ExpectationException>(() => Expectation.Load(Write(new string('0', 64))));
        Assert.Contains("pinned SHA-256", error.Message);
    }

    [Fact]
    public void ACensusAloneHasNoLayoutPin()
    {
        Write("actual");
        var (census, expectation) = Expectation.Load(Path.Combine(directory, "census.json"));
        Assert.Null(expectation);
        Assert.Empty(census.Rows);
    }
}
