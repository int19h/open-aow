using System.Reflection;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

namespace Aow.Game.Tests;

/// <summary>
/// A call the product does not make. The inventory test asserts that this method is reported.
/// </summary>
public static class PersistenceInventoryPlant
{
    public static byte[] WriteOwnPath(ObjectNode node, SchemaClass classValue, FormatSchema schema) =>
        OriginalBackend.Write(node, classValue: classValue, schema: schema, engine: "game");
}

/// <summary>A public stored id the product tree does not declare. The inventory test asserts that this member is reported.</summary>
public sealed class PlantedStoredId
{
    public int Id { get; init; }
}

public sealed class PersistenceInventoryTests
{
    private static readonly Assembly[] Product =
    [
        typeof(OriginalBackend).Assembly,
        typeof(Engine.PersistSupport).Assembly,
        typeof(GameEngine).Assembly,
    ];

    [Fact]
    public void AllowedSitesNameAReason()
    {
        Assert.NotEmpty(PersistenceInventory.AllowedSites);
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (var site in PersistenceInventory.AllowedSites)
        {
            Assert.False(string.IsNullOrWhiteSpace(site.TypeName));
            Assert.False(string.IsNullOrWhiteSpace(site.MethodName));
            Assert.False(string.IsNullOrWhiteSpace(site.Reason));
            Assert.True(names.Add(site.TypeName + "." + site.MethodName));
        }
    }

    [Fact]
    public void ProductAssembliesStayInsideTheAllowedSites()
    {
        Assert.Contains(Product, static assembly => assembly.GetName().Name == "Aow.Formats");
        Assert.Contains(Product, static assembly => assembly == typeof(Engine.PersistSupport).Assembly);
        Assert.Contains(Product, static assembly => assembly == typeof(GameEngine).Assembly);
        var reaches = PersistenceInventory.BackendReaches(Product);
        var exposed = PersistenceInventory.ExposedMembers(Product);
        Assert.True(reaches.Count == 0, "Own-path reaches:\n" + string.Join("\n", reaches));
        Assert.True(exposed.Count == 0, "Exposed members:\n" + string.Join("\n", exposed));
    }

    [Fact]
    public void PlantedViolationsAreFound()
    {
        Assert.Equal(
            [new BackendReach("Aow.Game.Tests.PersistenceInventoryPlant.WriteOwnPath", "Aow.Formats.Tree.OriginalBackend.Write")],
            PersistenceInventory.BackendReaches(typeof(PersistenceInventoryPlant)));
        Assert.Equal(
            [new ExposedMember("Aow.Game.Tests.PlantedStoredId", "Id", "stored id", UsedBy: null)],
            PersistenceInventory.ExposedMembers(typeof(PlantedStoredId)));
    }
}
