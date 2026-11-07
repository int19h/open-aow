using Aow.Formats.Original;

namespace Aow.Formats.Tests;

/// <summary>
/// The extensions of the document kinds (own-formats.md 1.1 and 2.9): each kind has its original
/// extension and its own one, an "o" before the original, and an extension only suggests a kind.
/// </summary>
public sealed class DocumentKindTests : FormatsTest
{
    public static TheoryData<string, string, string> Extensions => new()
    {
        { "save", ".asg", ".oasg" },
        { "campaign-save", ".acg", ".oacg" },
        { "hero-list", ".ahl", ".oahl" },
        { "item-list", ".ail", ".oail" },
        { "map", ".hsm", ".ohsm" },
        { "campaign-map", ".csm", ".ocsm" },
        { "campaign", ".cam", ".ocam" },
        { "scenario-set", ".hss", ".ohss" },
    };

    [Theory]
    [MemberData(nameof(Extensions))]
    public void EachKindNamesItsOriginalAndOwnExtension(string name, string original, string own)
    {
        var kind = DocumentKind.Find(name)!;
        Assert.Equal(original, kind.OriginalExtension);
        Assert.Equal(own, kind.OwnExtension);
        Assert.Same(kind, DocumentKind.ForExtension(original));
        Assert.Same(kind, DocumentKind.ForExtension(own.ToUpperInvariant()));
        Assert.Same(kind, DocumentKind.ForOriginalExtension(original.ToUpperInvariant()));
        Assert.Null(DocumentKind.ForOriginalExtension(own));
        Assert.True(kind.IsOwnExtension(own));
        Assert.False(kind.IsOwnExtension(original));
        Assert.Equal(name, GameFiles.KindForExtension(original));
        Assert.Equal(name, GameFiles.KindForExtension(own));
    }

    [Fact]
    public void TheKindsAreTheProfileKinds()
    {
        Assert.Equal(DocumentKind.All.Select(k => k.Name), FileProfile.AllKinds);
        foreach (var profile in FileProfile.All)
            foreach (var kind in profile.Kinds) Assert.NotNull(DocumentKind.Find(kind));
        Assert.Null(GameFiles.KindForExtension(".pfs"));
        Assert.Null(DocumentKind.ForExtension(null));
    }

    [Fact]
    public void NamesChangeOnlyTheirExtension()
    {
        Assert.Equal(@"Save\Foo.oasg", DocumentKind.Save.OwnName(@"Save\Foo.asg"));
        Assert.Equal(@"Save\Foo.asg", DocumentKind.Save.OriginalName(@"Save\Foo.oasg"));
        Assert.Equal(@"Save\Foo.oasg", DocumentKind.Save.OwnName(@"Save\Foo"));
        Assert.Equal(@"a.b\Foo.oasg", DocumentKind.Save.OwnName(@"a.b\Foo"));
        Assert.Equal("dir/Keepers.ocam", DocumentKind.Campaign.OwnName("dir/Keepers.CAM"));
    }

    // Content decides. An own document classifies as own under an original extension, and an
    // original classifies by its own extension too.
    [Fact]
    public void ClassificationIgnoresTheFormOfTheExtension()
    {
        var own = "{\"$format\": \"OpenAoW\", \"$version\": 1, \"$kind\": \"hero-list\", \"$container\": {\"profile\": \"hero-list\", \"compression\": \"none\"}, \"root\": {}}"u8.ToArray();
        Assert.Equal(DetectedFormat.OwnJson, GameFiles.Classify(own, ".ahl").Format);
        Assert.Equal(DetectedFormat.OwnJson, GameFiles.Classify(own, ".oahl").Format);
        var original = GameFiles.Export(GameFiles.Read(own).Document).Bytes;
        Assert.Equal(DetectedFormat.Original, GameFiles.Classify(original, ".ahl").Format);
        Assert.Equal(DetectedFormat.Original, GameFiles.Classify(original, ".oahl").Format);
    }
}
