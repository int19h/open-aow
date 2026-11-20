using Aow.Convert;
using Aow.Formats.Original;

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
        TheoryData<string> data = [];
        foreach (string pattern in s_patterns)
        {
            string directory = Path.Combine(Checker.GameRoot, Path.GetDirectoryName(pattern)!);
            if (!Directory.Exists(directory))
            {
                continue;
            }

            foreach (string? file in Directory.EnumerateFiles(directory, searchPattern: Path.GetFileName(pattern), enumerationOptions: new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive })
                .Order(StringComparer.Ordinal))
            {
                data.Add(Path.GetRelativePath(Checker.GameRoot, path: file).Replace('\\', '/'));
            }
        }
        return data;
    }

    [GameFact]
    public void TheGameRootHoldsTheShippedFiles()
    {
        int count = Files().Count;
        Assert.True(count >= 132, userMessage: $"{count} in-scope files under {Checker.GameRoot}");
    }

    [GameTheory]
    [MemberData(nameof(Files))]
    public void OriginalRoundTrips(string relative)
    {
        string path = Path.Combine(Checker.GameRoot, relative);
        byte[] bytes = File.ReadAllBytes(path);
        Classification c = GameFiles.Classify(bytes, extension: Path.GetExtension(path));
        Assert.Equal(DetectedFormat.Original, c.Format);
        ImportResult imported = GameFiles.Import(bytes, c.Profile!, c.Kind!, fileName: relative);
        byte[] text = GameFiles.Write(imported.Document);
        ExportResult exported = GameFiles.Export(GameFiles.Read(text, fileName: relative).Document, fileName: relative);
        OriginalComparisonResult comparison = GameFiles.CompareG1(bytes, exported: exported.Bytes, profile: c.Profile!, document: imported.Document, kind: c.Kind!);
        Assert.NotEqual(OriginalMatchKind.Mismatch, comparison.Kind);
        byte[] again = GameFiles.Write(GameFiles.Import(exported.Bytes, c.Profile!, c.Kind!, fileName: relative).Document);
        Assert.True(again.AsSpan().SequenceEqual(text), userMessage: "G2");
    }
}
