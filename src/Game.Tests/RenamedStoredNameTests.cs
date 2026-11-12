using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Schema;
using Aow.Game.AI.Groups;
using Aow.Rtl;

namespace Aow.Game.Tests;

// A document saved before issue 187 renamed members stores the old names. They still load.
public sealed class RenamedStoredNameTests
{
    [Fact]
    public void AnOldEnumMemberNameLoads()
    {
        Assert.True(NameTable.TryMember(typeof(GameSettings.CombatResolveMode), "AlwaysFC", out string member));
        Assert.Equal(nameof(GameSettings.CombatResolveMode.AlwaysFastBattle), member);
        Assert.Equal(nameof(GameSettings.CombatResolveMode.AlwaysFastBattle), NameTable.StoredMember(typeof(GameSettings.CombatResolveMode), "AlwaysFC"));
    }

    [Fact]
    public void AnEnumMovedOutOfItsClassLoadsByItsOldName()
    {
        Assert.Equal(nameof(AIMoveControlMode), NameTable.Stored(typeof(AIMoveControlMode)));
        Assert.Equal(typeof(AIMoveControlMode), NameTable.Find("AIMoveControl.Mode"));
    }

    [Fact]
    public void AnOldPropertyNameLoads()
    {
        var resource = FormatSchema.Current.Get("EngineResource");
        Assert.Equal("ResourceId", resource.PropertyByName("ResourceID")!.Name);
        Assert.Equal(["ResourceID"], FormatSchema.Current.OldNamesOf("ResourceId"));

        var root = new JsonObject().Add("$class", new JsonString("EngineResource")).Add("ResourceID", new JsonInteger(5));
        var document = new JsonObject().Add("$version", new JsonInteger(OwnFormat.CurrentVersion)).Add("root", root);
        MigrationChain.Default.Migrate(document, version: OwnFormat.CurrentVersion, applied: []);
        Assert.Null(root.Find("ResourceID"));
        Assert.Equal(5, ((JsonInteger)root["ResourceId"]!).Value);
    }
}
