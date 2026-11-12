using System.Reflection;
using Aow.Convert;
using Aow.Formats.Json;
using Aow.Formats.Migrations;

namespace Aow.Formats.Tests;

/// <summary>
/// Historical golden documents (own-formats.md 2.6): a test chain from version 1 to 3 runs every
/// step, keeps provenance, and the result equals the golden current document.
/// </summary>
public sealed class MigrationTests : FormatsTest
{
    private static byte[] Golden(string name)
    {
        using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Aow.Formats.Tests.Golden." + name)!;
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    [Fact]
    public void TheHistoricalChainReachesTheGoldenDocument()
    {
        var read = GameFiles.Read(Golden("hero-list-v1.json"), "hero-list-v1.json", null, Fixtures.TestChain());
        Assert.Equal(1, read.DeclaredVersion);
        Assert.Equal(["test-1-rename-LibraryTitle", "test-2-drop-Obsolete"], read.Migrations);
        Assert.Equal(Golden("hero-list-v3.json"), JsonWriter.ToUtf8(read.Document));
    }

    [Fact]
    public void AnIntermediateVersionRunsOnlyTheRemainingSteps()
    {
        var read = GameFiles.Read(Golden("hero-list-v2.json"), "hero-list-v2.json", null, Fixtures.TestChain());
        Assert.Equal(["test-2-drop-Obsolete"], read.Migrations);
        Assert.Equal(Golden("hero-list-v3.json"), JsonWriter.ToUtf8(read.Document));
    }

    [Fact]
    public void ANewerVersionFailsWithBothNumbers()
    {
        var error = Assert.Throws<ConversionException>(() =>
            GameFiles.Read("{\"$format\": \"OpenAoW\", \"$version\": 4}"u8.ToArray(), "new.ahl", null, Fixtures.TestChain()));
        Assert.Equal(Codes.Version, error.Diagnostic.Code);
        Assert.Contains("version 4, newer than the supported version 3", error.Diagnostic.Message);
        Assert.Equal(1, error.Diagnostic.Span.Line);
    }

    [Fact]
    public void ValuesAMigrationSynthesizesNameIt()
    {
        var chain = new MigrationChain(2, [new Migration(1, "test-add-NextLibraryID", d =>
        {
            ((JsonObject)d["root"]!).Add("NextLibraryID", new JsonString("not an integer"));
            return d;
        })]);
        var read = GameFiles.Read(Golden("hero-list-v1.json"), "v1.json", null, chain);
        var root = (JsonObject)read.Document["root"]!;
        root.Remove("LibraryTitle");
        root.Remove("Obsolete");
        var document = read.Document;
        document.Find("$version")!.Value = new JsonInteger(OwnFormat.CurrentVersion);
        var error = Assert.Throws<ConversionException>(() => GameFiles.Export(document));
        Assert.Equal("test-add-NextLibraryID", error.Diagnostic.Migration);
        Assert.Equal("root.NextLibraryID", error.Diagnostic.Path);
    }

    [Fact]
    public void AChainNeedsEveryStep()
    {
        Assert.Throws<ArgumentException>(() => new MigrationChain(3, [new Migration(1, "only", d => d)]));
        Assert.Throws<ArgumentException>(() => new MigrationChain(2, [new Migration(1, "a", d => d), new Migration(1, "b", d => d)]));
    }

    [Fact]
    public void TheCurrentChainRewritesSymbolicValues()
    {
        Assert.Equal(5, MigrationChain.Default.Current);
        var read = GameFiles.Read(Golden("hero-list-v1.json"), null, null, MigrationChain.Default);
        Assert.Equal([SymbolicMigration.Name, RecordMigration.Name, ListMigration.Name, ResourceMigration.Name], read.Migrations);
        Assert.Equal(1, read.DeclaredVersion);
        Assert.Equal(5, ((JsonInteger)read.Document["$version"]!).Value);
    }
}
