using System.Security.Cryptography;
using System.Text;
using Aow.Convert;

namespace Aow.Formats.Tests;

/// <summary>The pinned G1 expectation of aowconv check: drift and a changed census fail.</summary>
public sealed class ExpectationTests : FormatsTest, IDisposable
{
    private readonly string _directory = Path.Combine(Path.GetTempPath(), $"aowconv-expect-{Guid.NewGuid():N}");

    public ExpectationTests() => Directory.CreateDirectory(_directory);

    public void Dispose() => Directory.Delete(_directory, true);

    private string Write(string censusSha)
    {
        byte[] census = Encoding.UTF8.GetBytes("{\"corpus\": [], \"summary\": {\"unexplainedSites\": 0}}");
        File.WriteAllBytes(Path.Combine(_directory, "census.json"), census);
        string sha = censusSha == "actual" ? System.Convert.ToHexStringLower(SHA256.HashData(census)) : censusSha;
        string path = Path.Combine(_directory, "expect.json");
        File.WriteAllText(path, """
            {
              "format": "aowconv-check-expectation",
              "version": 2,
              "census": {"path": "census.json", "sha256": "SHA"},
              "g1": {
                "exact": 10,
                "masked": 2,
                "document": 1,
                "rules": {"file-crc": 4, "hss-header-tail": 35},
                "profiles": {
                  "scenario-set": {"exact": 0, "masked": 1, "document": 0},
                  "world-hsm": {"exact": 10, "masked": 1, "document": 1}
                }
              },
              "legacyRules": {"LEGACY_VALLEY01_SPRITE": {"occurrences": 1, "files": ["install:Scenario/Campaign/Valley01.CSM"]}}
            }
            """.Replace("SHA", sha, StringComparison.Ordinal));
        return path;
    }

    private static SortedDictionary<string, (int, List<string>)> Rules(params string[] files) =>
        new(StringComparer.Ordinal) { ["LEGACY_VALLEY01_SPRITE"] = (files.Length, files.ToList()) };

    [Fact]
    public void MatchingCountsHaveNoDrift()
    {
        (CensusFacts _, Expectation? expectation) = Expectation.Load(Write("actual"));
        List<string> problems = expectation!.Compare(
            exact: 10,
            masked: 2,
            document: 1,
            maskedRules: new(StringComparer.Ordinal) { ["file-crc"] = 4, ["hss-header-tail"] = 35 },
            profiles: new(StringComparer.Ordinal)
            {
                ["scenario-set"] = new(0, 1, 0),
                ["world-hsm"] = new(10, 1, 1),
            },
            legacyRules: Rules("install:Scenario/Campaign/Valley01.CSM"));
        Assert.Empty(problems);
    }

    [Fact]
    public void EveryDifferenceIsDrift()
    {
        (CensusFacts _, Expectation? expectation) = Expectation.Load(Write("actual"));
        List<string> problems = expectation!.Compare(
            exact: 9,
            masked: 3,
            document: 1,
            maskedRules: new(StringComparer.Ordinal) { ["file-crc"] = 4, ["hss-header-tail"] = 36 },
            profiles: new(StringComparer.Ordinal)
            {
                ["scenario-set"] = new(0, 1, 0),
                ["world-hsm"] = new(9, 2, 1),
            },
            legacyRules: Rules("install:Scenario/Campaign/Valley01.CSM", "install:Scenario/Campaign/Other.csm"));
        Assert.Contains(problems, p => p.StartsWith("G1 exact files", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("G1 masked files", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("masked rule hss-header-tail", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("profile world-hsm", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("legacy rule LEGACY_VALLEY01_SPRITE occurrences", StringComparison.Ordinal));
        Assert.Contains(problems, p => p.StartsWith("legacy rule LEGACY_VALLEY01_SPRITE files", StringComparison.Ordinal));
    }

    [Fact]
    public void AChangedCensusIsRefused()
    {
        ExpectationException error = Assert.Throws<ExpectationException>(() => Expectation.Load(Write(new string('0', 64))));
        Assert.Contains("pinned SHA-256", error.Message);
    }

    [Fact]
    public void ACensusAloneHasNoG1Pin()
    {
        _ = Write("actual");
        (CensusFacts? census, Expectation? expectation) = Expectation.Load(Path.Combine(_directory, "census.json"));
        Assert.Null(expectation);
        Assert.Empty(census.Rows);
    }
}
