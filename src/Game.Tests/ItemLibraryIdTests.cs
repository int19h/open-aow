using Aow.Game.Magic.Abilities;
using Aow.Game.Units;

namespace Aow.Game.Tests;

// Issue 346: an item library numbers a new item by its library id, not by the world item id
// the original looked up among the library's items. Two library items never share a library id,
// and a library id that only matches the world item id of another library item stays as it is.
[Collection("Engine")]
[EngineState]
public sealed class ItemLibraryIdTests
{
    [Fact]
    public void AddChildRenumbersEachLaterItemWithTheSameLibraryId()
    {
        ItemLibrary library = new(null) { Holder = EngineFixture.Engine };
        Item firstItem = new(null) { Holder = EngineFixture.Engine, LibraryId = 7 };
        _ = library.AddChild(firstItem);
        Item secondItem = new(null) { Holder = EngineFixture.Engine, LibraryId = 7 };
        _ = library.AddChild(secondItem);
        Item thirdItem = new(null) { Holder = EngineFixture.Engine, LibraryId = 7 };
        _ = library.AddChild(thirdItem);

        Assert.Equal(7, firstItem.LibraryId);
        Assert.True(secondItem.LibraryId > 0);
        Assert.True(thirdItem.LibraryId > 0);
        Assert.Equal(3, new[] { firstItem.LibraryId, secondItem.LibraryId, thirdItem.LibraryId }.Distinct().Count());
    }

    [Fact]
    public void AddChildNumbersAnItemWithoutALibraryId()
    {
        ItemLibrary library = new(null) { Holder = EngineFixture.Engine };
        Item numberedItem = new(null) { Holder = EngineFixture.Engine, LibraryId = 7 };
        _ = library.AddChild(numberedItem);
        Item unnumberedItem = new(null) { Holder = EngineFixture.Engine, LibraryId = 0 };
        _ = library.AddChild(unnumberedItem);

        Assert.True(unnumberedItem.LibraryId > 0);
        Assert.NotEqual(numberedItem.LibraryId, unnumberedItem.LibraryId);
    }

    [Fact]
    public void AddChildKeepsALibraryIdThatOnlyMatchesAWorldItemId()
    {
        ItemLibrary library = new(null) { Holder = EngineFixture.Engine };
        Item worldIdItem = new(null) { Holder = EngineFixture.Engine, ItemId = 5, LibraryId = 3 };
        _ = library.AddChild(worldIdItem);
        Item libraryIdItem = new(null) { Holder = EngineFixture.Engine, LibraryId = 5 };
        _ = library.AddChild(libraryIdItem);

        Assert.Equal(5, libraryIdItem.LibraryId);
    }
}
