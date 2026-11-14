using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Rtl.Localize;

namespace Aow.Game.Tests;

// The translate visit walks an object through its tree methods and rewrites only strings.
[Collection("Engine")]
[EngineState]
public sealed class TranslateVisitTests
{
    private sealed class StoredObject() : EngineObject(null!)
    {
        public string Name { get; set; } = "";

        public string Detail { get; set; } = "";

        public override void PersistFields(IPersister persister)
        {
            Name = persister.String("Name", Name);
            Detail = persister.String("Detail", Detail);
        }
    }

    [Fact]
    public void AVisitWithoutADictionaryLeavesTheStrings()
    {
        using var translator = new TranslateVisit(EngineFixture.Engine);
        using var value = new StoredObject { Name = "Oak", Detail = "Hill" };
        translator.Object(value, 4);
        Assert.Equal("Oak", value.Name);
        Assert.Equal("Hill", value.Detail);
    }

    [Fact]
    public void ATextVisitRewritesEachStringThroughTheDictionary()
    {
        using var translator = new TranslateTextStream(EngineFixture.Engine);
        using var dictionary = new IvDictionary();
        translator.FDictionary = dictionary;
        using var value = new StoredObject { Name = "Oak", Detail = "Hill" };
        translator.Object(value, 4);
        Assert.Equal("Oak", value.Name);
        Assert.Equal("Hill", value.Detail);
        Assert.Equal("Oak", dictionary.Translate("Oak"));
    }
}
