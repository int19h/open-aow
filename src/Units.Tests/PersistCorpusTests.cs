using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Units.Engine;
using Xunit.Abstractions;

namespace Aow.Units.Tests;

public sealed class CorpusFactAttribute : FactAttribute
{
    public CorpusFactAttribute()
    {
        if (PersistCorpusTests.SkipReason is string reason) Skip = reason;
    }
}

/// <summary>Every pilot instance in the installed corpus matches the converter after an engine rewrite.</summary>
[Collection("Engine")]
[EngineState]
public sealed class PersistCorpusTests
{
    private static readonly string[] Patterns =
        ["Scenario/Custom/*.hsm", "Scenario/Campaign/*.csm", "Scenario/Campaign/*.cam", "Release/*.ahl", "Release/*.hss", "TCMaps/*.hsm", "Save/*.asg"];

    private static readonly string[] Pilots =
    [
        "TEControlStyleObject", "TEResource", "TEResourceEditSettings", "TEnchantment", "TEventLog", "TGlobalEnchantment",
        "TItemGFX", "TMessageEventLog", "TAIBudget", "TAIBudgetList", "TAIBudgetManager", "TAISavingsBudget",
        "TAITransporterBudget", "TAICityFortifyBudget", "TAICityResearchBudget", "TAICityUpgradeBudget",
        "TCosmeticSurgeryEnchantment", "TFloodControl", "TFloodEnchantment", "TFrozenWaterHS",
        "TUnitEnchantment", "TUnitEnchantmentAbilityData",
    ];

    private readonly ITestOutputHelper output;

    public PersistCorpusTests(ITestOutputHelper output) => this.output = output;

    [CorpusFact]
    public void CorpusInstancesMatchTheConverter()
    {
        var schema = FormatSchema.Current;
        var factory = EnginePersistableFactory.Instance;
        var counts = Pilots.ToDictionary(name => name, _ => 0, StringComparer.Ordinal);
        var pilots = counts.Keys.ToHashSet(StringComparer.Ordinal);
        foreach (var relative in Files())
        {
            var path = Path.Combine(Root, relative);
            var bytes = File.ReadAllBytes(path);
            var found = GameFiles.Classify(bytes, Path.GetExtension(path));
            if (found.Format != DetectedFormat.Original || found.Profile is null || found.Kind is null) continue;
            var imported = GameFiles.Import(bytes, found.Profile, found.Kind, relative);
            var root = (JsonObject)imported.Document["root"]!;
            var cls = found.Profile.RootHasClass
                ? schema.Get(((JsonString)root["$class"]!).Value)
                : schema.Get(found.Profile.RootFamily);
            Walk(root, cls, found.Profile.Engine, relative, schema, factory, pilots, counts);
        }
        foreach (var name in Pilots)
        {
            string line = name + " " + counts[name];
            output.WriteLine(line);
            Console.WriteLine(line);
        }
        // Every install holds these, so a walker that finds nothing fails here.
        foreach (var name in (string[])["TAIBudgetManager", "TUnitEnchantmentAbilityData", "TEResourceEditSettings"])
            Assert.True(counts[name] > 0, name + " has no instance in the corpus.");
    }

    private static void Walk(JsonObject obj, SchemaClass cls, string engine, string file, FormatSchema schema, EnginePersistableFactory factory, HashSet<string> pilots, Dictionary<string, int> counts)
    {
        if (pilots.Contains(cls.Name))
        {
            counts[cls.Name]++;
            Prove(obj, cls, engine, file, schema, factory);
        }
        foreach (var prop in cls.Properties)
        {
            var member = obj.Find(prop.Name);
            if (member is null) continue;
            if (prop.IsDynamic)
            {
                if (member.Value is JsonArray array)
                    foreach (var item in array.Items) Visit(cls, prop, item, engine, file, schema, factory, pilots, counts);
                continue;
            }
            Visit(cls, prop, member.Value, engine, file, schema, factory, pilots, counts);
        }
    }

    private static void Visit(SchemaClass cls, SchemaProperty prop, JsonValue value, string engine, string file, FormatSchema schema, EnginePersistableFactory factory, HashSet<string> pilots, Dictionary<string, int> counts)
    {
        if (prop.Codec is FixedObjectCodec fixedObject && value is JsonObject nested)
            Walk(nested, schema.Get(fixedObject.ClassName), engine, file, schema, factory, pilots, counts);
        else if (prop.Codec is CreatedObjectCodec && value is JsonObject created && created["$class"] is JsonString name)
            Walk(created, schema.Get(name.Value), engine, file, schema, factory, pilots, counts);
        else if (prop.Codec is ChildrenCodec && value is JsonArray children)
        {
            foreach (var item in children.Items)
                if (item is JsonObject child && child["$class"] is JsonString childName)
                    Walk(child, schema.Get(childName.Value), engine, file, schema, factory, pilots, counts);
        }
        else if (prop.Codec is MapFieldsCodec && value is JsonArray rows)
        {
            var field = schema.Get(cls.FieldClass ?? "TMapField");
            foreach (var row in rows.Items)
                if (row is JsonArray cells)
                    foreach (var cell in cells.Items)
                        if (cell is JsonObject fieldObject)
                            Walk(fieldObject, field, engine, file, schema, factory, pilots, counts);
        }
        else if (prop.Codec is CampaignMapCodec && value is JsonObject map && map["root"] is JsonObject campaignRoot)
        {
            var campaignClass = campaignRoot["$class"] is JsonString nestedName ? schema.Get(nestedName.Value) : schema.Get("TAoWHSMap");
            Walk(campaignRoot, campaignClass, engine, file, schema, factory, pilots, counts);
        }
    }

    private static void Prove(JsonObject obj, SchemaClass cls, string engine, string file, FormatSchema schema, EnginePersistableFactory factory)
    {
        try
        {
            var image = PilotImage.Encode(obj, cls.Name, engine);
            var loaded = (TEObject)factory.Create(cls.Name);
            try
            {
                Load(loaded, image);
                var rewritten = Image(loaded);
                var expected = PilotImage.Decode(rewritten, cls.Name, engine);
                var actual = DocumentBackend.Write(TreeWriter.Write(loaded, factory), cls, schema, engine);
                EqualJson(expected, actual, file + " " + cls.Name + " write");
                var again = (TEObject)factory.Create(cls.Name);
                try
                {
                    TreeReader.Read(DocumentBackend.Read(expected, cls, schema, engine), again, factory);
                    EqualBytes(PilotImage.Encode(expected, cls.Name, engine), Image(again), file + " " + cls.Name + " read");
                }
                finally
                {
                    again.Destroy();
                }
            }
            finally
            {
                loaded.Destroy();
            }
        }
        catch (System.Exception error) when (error is not Xunit.Sdk.XunitException)
        {
            throw new InvalidOperationException(file + " " + cls.Name + " failed. " + error.Message, error);
        }
    }

    private static void Load(TEObject obj, byte[] image)
    {
        using var memory = new TMemoryStream();
        memory.Write(image);
        memory.Position = 0;
        var reader = new TEReadStorageStream(memory, EngineFixture.Engine, false);
        try
        {
            obj.MainReadWrite(reader);
        }
        finally
        {
            reader.Destroy();
        }
    }

    private static byte[] Image(TEObject obj)
    {
        using var memory = new TMemoryStream();
        var writer = new TEWriteStorageStream(memory, EngineFixture.Engine, false);
        try
        {
            obj.MainReadWrite(writer);
        }
        finally
        {
            writer.Destroy();
        }
        return memory.Memory[..memory.Size];
    }

    private static void EqualJson(JsonObject expected, JsonObject actual, string label)
    {
        var left = JsonWriter.ToUtf8(expected);
        var right = JsonWriter.ToUtf8(actual);
        if (left.AsSpan().SequenceEqual(right)) return;
        string text = Encoding.UTF8.GetString(right);
        if (text.Length > 1500) text = text[..1500];
        Assert.Fail(label + "\n" + text);
    }

    private static void EqualBytes(byte[] expected, byte[] actual, string label)
    {
        if (expected.AsSpan().SequenceEqual(actual)) return;
        Assert.Fail(label + " at " + expected.AsSpan().CommonPrefixLength(actual));
    }

    private static string Root => Path.GetFullPath(Environment.GetEnvironmentVariable("AOW_GAME_ROOT") is { Length: > 0 } root
        ? root
        : @"D:\Games\GOG\Age of Wonders");

    internal static string? SkipReason =>
        File.Exists(Path.Combine(Root, "Release", "Release.hss")) && Directory.Exists(Path.Combine(Root, "Scenario", "Custom"))
            ? null
            : "The installed game is absent: " + Root + ". Set AOW_GAME_ROOT to the game installation.";

    private static IEnumerable<string> Files()
    {
        foreach (var pattern in Patterns)
        {
            var directory = Path.Combine(Root, Path.GetDirectoryName(pattern)!);
            if (!Directory.Exists(directory)) continue;
            foreach (var file in Directory.EnumerateFiles(directory, Path.GetFileName(pattern), new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive }))
                yield return Path.GetRelativePath(Root, file);
        }
    }
}
