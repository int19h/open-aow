using Aow.Formats;
using Aow.Formats.Json;

using Aow.Game.Saves;
using Aow.Game.Units;

namespace Aow.Game.Tests;

/// <summary>
/// The gate's save shadow (persistence-tree.md 12): the tree document of a saved root equals the
/// document the boundary imports from the engine image, and a difference names its document path.
/// A changed imported member stands for a value the engine write changed: a second engine write
/// agrees with the tree, so the result names the path as an engine write change.
/// </summary>
[Collection("Engine")]
public sealed class SaveShadowTests
{
    [Fact]
    public void TheTreeDocumentOfASavedHeroListEqualsTheImportedOne()
    {
        using var library = new HeroLibrary(null!) { Holder = EngineFixture.Engine };
        library.LibraryName = "Shadow";
        var document = Saved(library);
        Assert.Equal(default, SaveShadow.Check(GameProfile.HeroList, library, document));
    }

    [Fact]
    public void ADifferenceThatASecondEngineWriteSettlesNamesItsDocumentPath()
    {
        using var library = new HeroLibrary(null!) { Holder = EngineFixture.Engine };
        library.LibraryName = "Shadow";
        var document = Saved(library);
        ((JsonObject)document["root"]!).Find("LibraryName")!.Value = new JsonString("Other");
        var result = SaveShadow.Check(GameProfile.HeroList, library, document);
        Assert.Equal("root.LibraryName: imported \"Other\", tree \"Shadow\"", result.EngineWriteChange);
        Assert.Null(result.Difference);
    }

    private static JsonObject Saved(HeroLibrary library)
    {
        using var memory = new MemoryStream();
        EngineFixture.Engine.WriteEngineObject(memory, library, progress: null);
        var result = GameBoundary.PrepareSave(memory.ToArray(), fileName: null, use: GameProfile.HeroList, validators: FormatMapping.Validators);
        Assert.True(result.Succeeded, result.Report.Error?.ToString());
        return result.Document!;
    }
}
