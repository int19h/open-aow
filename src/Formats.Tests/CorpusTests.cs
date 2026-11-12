using Aow.Convert;

namespace Aow.Formats.Tests;

/// <summary>
/// G1, G2 and G-own over the installed game's in-scope files (AOW_GAME_ROOT). The oracle
/// archive is checked by aowconv check --expect over the pinned census.
/// </summary>
public sealed class CorpusTests : FormatsTest
{
    private static readonly string[] s_patterns =
        ["Scenario/Custom/*.hsm", "Scenario/Campaign/*.csm", "Scenario/Campaign/*.cam", "Release/*.ahl", "Release/*.hss", "TCMaps/*.hsm", "Save/*.asg"];

    public static TheoryData<string> Files()
    {
        var data = new TheoryData<string>();
        foreach (var pattern in s_patterns)
        {
            var directory = Path.Combine(Checker.GameRoot, Path.GetDirectoryName(pattern)!);
            if (!Directory.Exists(directory)) continue;
            foreach (var file in Directory.EnumerateFiles(directory, Path.GetFileName(pattern), new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive })
                .Order(StringComparer.Ordinal))
                data.Add(Path.GetRelativePath(Checker.GameRoot, file).Replace('\\', '/'));
        }
        return data;
    }

    [GameFact]
    public void TheGameRootHoldsTheShippedFiles()
    {
        int count = Files().Count;
        Assert.True(count >= 132, $"{count} in-scope files under {Checker.GameRoot}");
    }

    [GameTheory]
    [MemberData(nameof(Files))]
    public void OriginalRoundTrips(string relative)
    {
        var path = Path.Combine(Checker.GameRoot, relative);
        var bytes = File.ReadAllBytes(path);
        var c = GameFiles.Classify(bytes, Path.GetExtension(path));
        Assert.Equal(DetectedFormat.Original, c.Format);
        var imported = GameFiles.Import(bytes, c.Profile!, c.Kind!, relative);
        var text = GameFiles.Write(imported.Document);
        var exported = GameFiles.Export(GameFiles.Read(text, relative).Document, null, relative);
        Assert.True(c.Profile!.Comparable(bytes, FormatLimits.Default).AsSpan().SequenceEqual(c.Profile.Comparable(exported.Bytes, FormatLimits.Default)), "G1");
        var again = GameFiles.Write(GameFiles.Import(exported.Bytes, c.Profile, c.Kind!, relative).Document);
        Assert.True(again.AsSpan().SequenceEqual(text), "G2");
        Assert.Empty(exported.Invalidated);
    }
}
