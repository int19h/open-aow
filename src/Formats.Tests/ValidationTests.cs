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
            if (context.Root["CurrentPlayer"] is not JsonInteger current) yield break;
            var players = (JsonArray)((JsonObject)context.Root["Players"]!)["Children"]!;
            var slots = players.Items.Select(p => ((JsonInteger)((JsonObject)p)["$slot"]!).Value).ToHashSet();
            if (!slots.Contains(current.Value))
                yield return context.At(Codes.Reference,
                    $"Player slot {current.Value} is absent from root.Players.Children. Use an existing slot.",
                    current, DocumentNodePath.Root.Member("CurrentPlayer"), current.Value.ToString(System.Globalization.CultureInfo.InvariantCulture), "domain player-slot");
        }
    }

    private static JsonObject Map()
    {
        var bytes = File.ReadAllBytes(Path.Combine(Checker.GameRoot, "Scenario", "Custom", "Mountain Clash.hsm"));
        var document = GameFiles.Import(bytes, FileProfile.Find("world-hsm")!, "map").Document;
        return GameFiles.Read(GameFiles.Write(document)).Document;
    }

    [GameFact]
    public void AValidDocumentHasNoFindings() => Assert.Empty(DocumentValidation.Run(Map(), [new PlayerSlotValidator()]));

    [GameFact]
    public void ADanglingReferenceIsLocatedAndNamesItsDomain()
    {
        var text = GameFiles.Write(Map()).AsSpan().ToArray();
        var edited = System.Text.Encoding.UTF8.GetString(text).Replace("\"$class\": \"StrategicMap\",", "\"$class\": \"StrategicMap\",\n    \"CurrentPlayer\": 17,", StringComparison.Ordinal);
        var document = GameFiles.Read(System.Text.Encoding.UTF8.GetBytes(edited), "edited.hsm").Document;
        var finding = Assert.Single(DocumentValidation.Run(document, [new PlayerSlotValidator()], "edited.hsm"));
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
        var document = Map();
        var root = (JsonObject)document["root"]!;
        root.Members.Insert(1, new JsonMember("CurrentPlayer", new JsonInteger(1)));
        var players = (JsonArray)((JsonObject)root["Players"]!)["Children"]!;
        players.Items.RemoveAt(1);
        var finding = Assert.Single(DocumentValidation.Run(document, [new PlayerSlotValidator()]));
        Assert.Equal("root.CurrentPlayer", finding.Path);
    }

    [GameFact]
    public void StructuralErrorsComeBeforeContentValidators()
    {
        var document = Map();
        ((JsonObject)document["root"]!).Add("NoSuchMember", new JsonInteger(1));
        var finding = Assert.Single(DocumentValidation.Run(document, [new PlayerSlotValidator()]));
        Assert.Equal(Codes.UnknownMember, finding.Code);
    }
}
