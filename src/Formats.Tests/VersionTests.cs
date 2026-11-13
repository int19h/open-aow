using Aow.Formats.Json;

namespace Aow.Formats.Tests;

/// <summary>
/// Own formats are not stable yet (own-formats.md 2.6): only the current version reads. A document
/// of another version fails with both numbers, and an older one asks for a new conversion.
/// </summary>
public sealed class VersionTests : FormatsTest
{
    private static byte[] HeroList(long version) => JsonWriter.ToUtf8(new JsonObject()
        .Add("$format", new JsonString(OwnFormat.FormatName))
        .Add("$version", new JsonInteger(version))
        .Add("$kind", new JsonString("hero-list"))
        .Add("$container", new JsonObject().Add("profile", new JsonString("hero-list")).Add("compression", new JsonString("none")))
        .Add("root", new JsonObject().Add("LibraryName", new JsonString("Current"))));

    [Fact]
    public void TheCurrentVersionReads()
    {
        var read = GameFiles.Read(HeroList(OwnFormat.CurrentVersion), "current.json");
        Assert.Equal(OwnFormat.CurrentVersion, read.DeclaredVersion);
        Assert.Equal("Current", ((JsonString)((JsonObject)read.Document["root"]!)["LibraryName"]!).Value);
    }

    [Fact]
    public void ANewerVersionFailsWithBothNumbers()
    {
        var error = Assert.Throws<ConversionException>(() => GameFiles.Read(HeroList(OwnFormat.CurrentVersion + 1), "new.json"));
        Assert.Equal(Codes.Version, error.Diagnostic.Code);
        Assert.Contains($"version {OwnFormat.CurrentVersion + 1}, newer than the supported version {OwnFormat.CurrentVersion}", error.Diagnostic.Message);
        Assert.Equal("$version", error.Diagnostic.Path);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(OwnFormat.CurrentVersion - 1)]
    public void AnOlderVersionFailsAndAsksForANewConversion(long version)
    {
        var error = Assert.Throws<ConversionException>(() => GameFiles.Read(HeroList(version), "old.json"));
        Assert.Equal(Codes.Version, error.Diagnostic.Code);
        Assert.Contains($"version {version}, older than the supported version {OwnFormat.CurrentVersion}. Convert the original file again.", error.Diagnostic.Message);
        Assert.Equal(DetectedFormat.UnsupportedVersion, GameFiles.Classify(HeroList(version), ".ahl").Format);
    }
}
