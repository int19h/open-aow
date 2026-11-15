using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Units;

namespace Aow.Game.Tests;

/// <summary>
/// PE-5-a. PrisonersArmy.Items is the first port-only member. The own document keeps a list that
/// holds an item, and the original image refuses it with the path. An empty list is absent.
/// </summary>
public sealed partial class PersistEquivalenceTests
{
    [Fact]
    public void PrisonersArmyItemsRoundTripThroughTheOwnDocumentAndTheOriginalImageRefusesThem()
    {
        WithDestroyingMap(_ =>
        {
            using PrisonersArmy army = new(null!) { Holder = EngineFixture.Engine };
            PlainItem(itemId: 7, name: "Blade").SetOwner(army.Items);
            SchemaClass classValue = s_schema.Get("PrisonersArmy");
            ObjectNode tree = TreeWriter.Write(army, factory: Factory);
            JsonObject document = DocumentBackend.Write(tree, classValue: classValue, schema: s_schema, engine: "game");
            JsonObject stored = Assert.Single(((JsonArray)((JsonObject)document["Items"]!)["Children"]!).Items.Cast<JsonObject>());
            Assert.Equal("Blade", ((JsonString)stored["Name"]!).Value);
            Assert.Equal(7L, ((JsonInteger)stored["ItemId"]!).Value);

            using PrisonersArmy loaded = new(null!) { Holder = EngineFixture.Engine };
            TreeReader.Read(DocumentBackend.Read(document, classValue: classValue, schema: s_schema, engine: "game"), objectValue: loaded, factory: Factory);
            Assert.Equal(1, loaded.Items.Count);
            Assert.Equal(7, loaded.Items[0].ItemId);
            Assert.Equal("Blade", loaded.Items[0].Name);

            ConversionException export = Assert.Throws<ConversionException>(() => OriginalBackend.Write(tree, classValue: classValue, schema: s_schema, engine: "game"));
            Assert.Equal(Codes.Unrepresentable, export.Diagnostic.Code);
            Assert.Equal("root.Items", export.Diagnostic.Path);
            Assert.Contains("PrisonersArmy.Items", export.Diagnostic.Message);

            ConversionException packet = Assert.Throws<ConversionException>(() => GameBoundary.WritePacket(tree, classId: 131606, className: "PrisonersArmy"));
            Assert.Equal(Codes.Unrepresentable, packet.Diagnostic.Code);
            Assert.Equal("root.Items", packet.Diagnostic.Path);

            using PrisonersArmy empty = new(null!) { Holder = EngineFixture.Engine };
            ObjectNode emptyTree = TreeWriter.Write(empty, factory: Factory);
            JsonObject emptyDocument = DocumentBackend.Write(emptyTree, classValue: classValue, schema: s_schema, engine: "game");
            Assert.Null(emptyDocument.Find("Items"));
            OriginalBackend.Write(emptyTree, classValue: classValue, schema: s_schema, engine: "game");
            GameBoundary.WritePacket(emptyTree, classId: 131606, className: "PrisonersArmy");

            using PrisonersArmy fromAbsence = new(null!) { Holder = EngineFixture.Engine };
            TreeReader.Read(DocumentBackend.Read(emptyDocument, classValue: classValue, schema: s_schema, engine: "game"), objectValue: fromAbsence, factory: Factory);
            Assert.Equal(0, fromAbsence.Items.Count);
        });
    }
}
