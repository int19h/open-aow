using Aow.Convert;
using Aow.Formats.Json;
using Aow.Formats.Original;

namespace Aow.Formats.Tests;

/// <summary>The data-only validator interface with a reference validator written as test data.</summary>
public sealed class ValidationTests : FormatsTest
{
    /// <summary>CurrentPlayer refers to a stored player slot (reference domain player-slot).</summary>
    private sealed class PlayerSlotValidator : IDocumentValidator
    {
        public string Name => "test-player-slot";

        public IEnumerable<FormatDiagnostic> Validate(ValidationContext context)
        {
            if (context.Root["CurrentPlayer"] is not JsonInteger current)
            {
                yield break;
            }

            JsonArray players = (JsonArray)((JsonObject)context.Root["Players"]!)["Children"]!;
            HashSet<long> slots = [.. players.Items.Select(p => ((JsonInteger)((JsonObject)p)["$slot"]!).Value)];
            if (!slots.Contains(current.Value))
            {
                yield return context.At(Codes.Reference,
                    message: $"Player slot {current.Value} is absent from root.Players.Children. Use an existing slot.",
                    value: current, path: DocumentNodePath.Root.Member("CurrentPlayer"), rejected: current.Value.ToString(System.Globalization.CultureInfo.InvariantCulture), constraint: "domain player-slot");
            }
        }
    }

    private static JsonObject Map()
    {
        byte[] bytes = File.ReadAllBytes(Path.Combine(Checker.GameRoot, "Scenario", "Custom", "Mountain Clash.hsm"));
        return GameFiles.Read(GameFiles.Write(GameFiles.Import(bytes, profile: FileProfile.Find("world-hsm")!, kind: "map").Document)).Document;
    }

    [GameFact]
    public void AValidDocumentHasNoFindings() => Assert.Empty(DocumentValidation.Run(Map(), validators: [new PlayerSlotValidator()]));

    [GameFact]
    public void ADanglingReferenceIsLocatedAndNamesItsDomain()
    {
        byte[] text = [.. GameFiles.Write(Map())];
        string edited = System.Text.Encoding.UTF8.GetString(text).Replace("\"$class\": \"StrategicMap\",", "\"$class\": \"StrategicMap\",\n    \"CurrentPlayer\": 17,", comparisonType: StringComparison.Ordinal);
        FormatDiagnostic finding = Assert.Single(DocumentValidation.Run(GameFiles.Read(System.Text.Encoding.UTF8.GetBytes(edited), fileName: "edited.hsm").Document, validators: [new PlayerSlotValidator()], file: "edited.hsm"));
        Assert.Equal(Codes.Reference, finding.Code);
        Assert.Equal("root.CurrentPlayer", finding.Path);
        Assert.Equal(12, finding.Span.Line);
        Assert.Equal(22, finding.Span.Column);
        Assert.Equal("domain player-slot", finding.Constraint);
        Assert.StartsWith("edited.hsm:12:22 E_REFERENCE root.CurrentPlayer\n", finding.ToString());
    }

    [GameFact]
    public void RemovingAReferencedChildIsRefusedAtTheReference()
    {
        JsonObject document = Map();
        JsonObject root = (JsonObject)document["root"]!;
        root.Members.Insert(1, item: new JsonMember("CurrentPlayer", new JsonInteger(1)));
        JsonArray players = (JsonArray)((JsonObject)root["Players"]!)["Children"]!;
        players.Items.RemoveAt(1);
        FormatDiagnostic finding = Assert.Single(DocumentValidation.Run(document, validators: [new PlayerSlotValidator()]));
        Assert.Equal("root.CurrentPlayer", finding.Path);
    }

    [GameFact]
    public void StructuralErrorsComeBeforeContentValidators()
    {
        JsonObject document = Map();
        _ = ((JsonObject)document["root"]!).Add("NoSuchMember", new JsonInteger(1));
        FormatDiagnostic finding = Assert.Single(DocumentValidation.Run(document, validators: [new PlayerSlotValidator()]));
        Assert.Equal(Codes.UnknownMember, finding.Code);
    }

    // Mountain Clash stores no prisoners army. A dungeon child on the map is the real member,
    // and the map stays structurally valid around it.
    [GameFact]
    public void PortOnlyItemsAreValidAndTheOriginalExportRefusesThem()
    {
        JsonObject document = Map();
        JsonArray children = (JsonArray)((JsonObject)document["root"]!)["Children"]!;
        long slot = 0;
        foreach (JsonValue item in children.Items)
        {
            if (item is JsonObject child && child["$slot"] is JsonInteger number && number.Value >= slot)
            {
                slot = number.Value + 1;
            }
        }

        JsonObject blade = new JsonObject()
            .Add("$class", new JsonString("Item"))
            .Add("$slot", new JsonInteger(0))
            .Add("Name", new JsonString("Blade"))
            .Add("ItemId", new JsonInteger(7));
        JsonObject prisoners = new JsonObject().Add("Items", new JsonObject().Add("Children", new JsonArray().Add(blade)));
        children.Items.Add(new JsonObject()
            .Add("$class", new JsonString("Dungeon"))
            .Add("$slot", new JsonInteger(slot))
            .Add("Prisoners", prisoners));
        Assert.Empty(DocumentValidation.Run(document, validators: []));
        ConversionException error = Assert.Throws<ConversionException>(() => GameFiles.Export(document));
        Assert.Equal(Codes.Unrepresentable, error.Diagnostic.Code);
        Assert.EndsWith(".Items", error.Diagnostic.Path);
        Assert.Contains("PrisonersArmy.Items", error.Diagnostic.Message);
        Assert.Equal("1073741824", error.Diagnostic.Value);
    }
}
