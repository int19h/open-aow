using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
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
        "TDiplomaticRelationHistoryItem", "TChangeGlobalMagicTE", "TUnitSpellCA", "TGlobalMagicControl",
        "TSingleTargetCA", "TCityEnchantment", "THatredEnchantment", "THarmonyEnchantment",
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
            Assert.Equal(2, ((JsonInteger)imported.Document["$version"]!).Value);
            var migrated = GameFiles.Read(GameFiles.Write(Version1Document(bytes, found.Profile, found.Kind, relative)), relative).Document;
            EqualJson(imported.Document, migrated, relative + " migration");
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
        foreach (var name in (string[])["TAIBudgetManager", "TUnitEnchantmentAbilityData", "TEResourceEditSettings", "TGlobalMagicControl"])
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
                var tree = TreeWriter.Write(loaded, factory);
                var actual = DocumentBackend.Write(tree, cls, schema, engine);
                EqualJson(expected, actual, file + " " + cls.Name + " write");
                EqualBytes(rewritten, OriginalBackend.Write(tree, cls, schema, engine), file + " " + cls.Name + " original");
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
                var fromImage = (TEObject)factory.Create(cls.Name);
                try
                {
                    TreeReader.Read(OriginalBackend.Read(image, cls, schema, engine), fromImage, factory);
                    EqualBytes(Image(loaded), Image(fromImage), file + " " + cls.Name + " readwrite");
                }
                finally
                {
                    fromImage.Destroy();
                }
            }
            finally
            {
                loaded.Destroy();
            }
        }
        catch (InvalidOperationException error) when (IsTreeRefusal(error))
        {
            // A refusing class overrides ReadWrite with no PersistFields, or MainReadWrite with no
            // MainPersist. The write throws that class's refusal. The count stands.
        }
        catch (System.Exception error) when (error is not Xunit.Sdk.XunitException)
        {
            throw new InvalidOperationException(file + " " + cls.Name + " failed. " + error.Message, error);
        }
    }

    private static readonly Dictionary<string, Type> EngineTypes = BuildEngineTypes();

    private static Dictionary<string, Type> BuildEngineTypes()
    {
        var types = new Dictionary<string, Type>();
        foreach (var eclass in EClass.All)
        {
            if (eclass.Type.Assembly != typeof(TEObject).Assembly) continue;
            types.TryAdd(eclass.Type.Name, eclass.Type);
        }
        return types;
    }

    private static bool IsTreeRefusal(InvalidOperationException error)
    {
        string message = error.Message;
        const string prefix = "Class ";
        if (!message.StartsWith(prefix, StringComparison.Ordinal)) return false;
        int nameEnd = message.IndexOf(' ', prefix.Length);
        if (nameEnd < 0) return false;
        string name = message.Substring(prefix.Length, nameEnd - prefix.Length);
        return EngineTypes.TryGetValue(name, out var type) && TEObject.TreePersistenceRefusal(type) == message;
    }

    private static void Load(TEObject obj, byte[] image)
    {
        EngineFixture.WithRegisteredClasses(() =>
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
        });
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
        Assert.Fail(label + " at " + FirstDifference(expected, actual, ""));
    }

    private static string FirstDifference(JsonValue expected, JsonValue actual, string path)
    {
        if (expected is JsonObject left && actual is JsonObject right)
        {
            int count = Math.Max(left.Members.Count, right.Members.Count);
            for (int i = 0; i < count; i++)
            {
                if (i >= left.Members.Count) return Join(path, right.Members[i].Name) + " extra";
                if (i >= right.Members.Count) return Join(path, left.Members[i].Name) + " missing";
                var one = left.Members[i];
                var other = right.Members[i];
                if (!string.Equals(one.Name, other.Name, StringComparison.Ordinal))
                    return path + " member " + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + " " + one.Name + " / " + other.Name;
                string nested = FirstDifference(one.Value, other.Value, Join(path, one.Name));
                if (nested.Length != 0) return nested;
            }
            return "";
        }
        if (expected is JsonArray leftItems && actual is JsonArray rightItems)
        {
            int count = Math.Max(leftItems.Items.Count, rightItems.Items.Count);
            for (int i = 0; i < count; i++)
            {
                string at = path + "[" + i.ToString(System.Globalization.CultureInfo.InvariantCulture) + "]";
                if (i >= leftItems.Items.Count) return at + " extra";
                if (i >= rightItems.Items.Count) return at + " missing";
                string nested = FirstDifference(leftItems.Items[i], rightItems.Items[i], at);
                if (nested.Length != 0) return nested;
            }
            return "";
        }
        var expectedBytes = JsonWriter.ToUtf8(expected);
        var actualBytes = JsonWriter.ToUtf8(actual);
        if (expectedBytes.AsSpan().SequenceEqual(actualBytes)) return "";
        string text = Encoding.UTF8.GetString(actualBytes);
        if (text.Length > 80) text = text[..80];
        return path + " " + text;
    }

    private static string Join(string path, string name) => path.Length == 0 ? name : path + "." + name;

    private static void EqualBytes(byte[] expected, byte[] actual, string label)
    {
        if (expected.AsSpan().SequenceEqual(actual)) return;
        Assert.Fail(label + " at " + expected.AsSpan().CommonPrefixLength(actual));
    }

    private static JsonObject Version1Document(byte[] file, FileProfile profile, string kind, string relative)
    {
        var session = new ImportSession
        {
            Schema = SymbolicMigration.Version1,
            Limits = FormatLimits.Default,
            Counts = new LayoutCounts(),
            File = relative,
        };
        var (container, root) = profile.Import(file, session);
        return new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName))
            .Add("$version", new JsonInteger(1))
            .Add("$kind", new JsonString(kind))
            .Add("$container", container)
            .Add("root", root);
    }

    private static string Root => InstalledSave.Root;

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
