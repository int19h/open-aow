using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.AI.Groups;
using Aow.Rtl;

namespace Aow.Game.Tests;

// Own documents store the current C# names (issue 230). A member name from before a rename does not
// load: own formats are not stable yet, and a document of an older version is converted again.
public sealed class StoredNameTests
{
    [Fact]
    public void AnOldEnumMemberNameIsNotAMember()
    {
        Assert.False(NameTable.TryMember(typeof(GameSettings.CombatResolveMode), "AlwaysFC", out _));
        Assert.True(NameTable.TryMember(typeof(GameSettings.CombatResolveMode), nameof(GameSettings.CombatResolveMode.AlwaysFastBattle), out string member));
        Assert.Equal(nameof(GameSettings.CombatResolveMode.AlwaysFastBattle), member);
    }

    [Fact]
    public void AnEnumMovedOutOfItsClassIsFoundByItsEarlierName()
    {
        Assert.Equal(nameof(AIMoveControlMode), NameTable.Stored(typeof(AIMoveControlMode)));
        Assert.Equal(typeof(AIMoveControlMode), NameTable.Find("AIMoveControl.Mode"));
    }

    [Fact]
    public void AnOldPropertyNameIsAnUnknownMember()
    {
        var schema = FormatSchema.Current;
        var resource = schema.Get("EngineResource");
        Assert.Null(resource.PropertyByName("ResourceID"));
        Assert.Equal("ResourceId", resource.PropertyById(1)!.Name);
        var error = Assert.Throws<ConversionException>(() =>
            DocumentBackend.Read(new JsonObject().Add("ResourceID", new JsonInteger(5)), resource, schema, "game"));
        Assert.Equal(Codes.UnknownMember, error.Diagnostic.Code);
        Assert.Equal("root.ResourceID", error.Diagnostic.Path);
    }
}
