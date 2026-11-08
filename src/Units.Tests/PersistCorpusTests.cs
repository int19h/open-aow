using System.Text;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Xunit.Abstractions;

namespace Aow.Units.Tests;

public sealed class CorpusFactAttribute : FactAttribute
{
    public CorpusFactAttribute()
    {
        if (PersistCorpusTests.SkipReason is string reason) Skip = reason;
    }
}

/// <summary>Every tree-persistable instance in the installed corpus matches the converter after an engine rewrite.</summary>
[Collection("Engine")]
[EngineState]
public sealed class PersistCorpusTests
{
    private static readonly string[] Patterns =
        ["Scenario/Custom/*.hsm", "Scenario/Campaign/*.csm", "Scenario/Campaign/*.cam", "Release/*.ahl", "Release/*.hss", "TCMaps/*.hsm", "Save/*.asg"];

    // These subclasses are in the installed scenarios and saves. TRaidAG, TScoutAG and the other
    // behavior groups are absent from those files. TAbilityList shares class id 80, so a created
    // object is not named that class, and no property fixes it. TSetupSettings is not a member of
    // a map, campaign, hero list or save. Storm and animation sprites are absent from these files.
    private static readonly string[] Required =
    [
        "TGuardAG", "TPassiveAG", "TFortifyAG", "TNormalAG", "TSkirmishAG",
        "TAIBudgetManager", "TEResourceEditSettings", "TGlobalMagicControl",
    ];

    private static readonly string[] Watched =
    [
        "TAbilityList", "TGuardAG", "TPassiveAG", "TFortifyAG", "TRaidAG", "TScoutAG", "TNormalAG", "TSkirmishAG",
        "TGuardAreaAG", "TPatrolAG", "TRefugeAG", "TDefenderAG", "TAggressorAG", "TExpanderAG", "TScorcherAG",
        "TSuicidalAG", "TTransportAG", "TAbstractBehaviorAG",
        "TStorm", "TIceStorm", "TDeathStorm", "TDivineStorm", "TFireStorm", "TLightningStorm", "TPestilenceStorm", "TBlastStorm",
        "TAnimation", "TBloodShrineAnimation", "TImageSequenceAnimation", "TSpringOfLifeAnimation", "TTwinkleAnimation",
        "TDamageCA", "TSelfDestructCA", "TTurnUndeadCA", "TRemotePlayer", "TCustomAbilityList", "TSetupSettings", "TAIGroup",
        "TAIBudgetManager", "TEResourceEditSettings", "TGlobalMagicControl",
    ];

    private readonly ITestOutputHelper output;

    // TArmy.Destroy notifies AoWHSMap unless that map is being destroyed. A blank instance has no map.
    private static TAoWHSMap? reaper;

    public PersistCorpusTests(ITestOutputHelper output) => this.output = output;

    [CorpusFact]
    public void CorpusInstancesMatchTheConverter()
    {
        var previous = AoWEUnit.AoWHSMap;
        var map = new TAoWHSMap(null!);
        map.FStatus = TSet<THSMapStatusElement>.Of(THSMapStatusElement.msDestroying);
        reaper = map;
        blocked.Clear();
        try
        {
            var schema = FormatSchema.Current;
            var factory = new EnginePersistableFactory(EngineFixture.Engine);
            var counts = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var relative in Files())
            {
                var path = Path.Combine(Root, relative);
                var bytes = File.ReadAllBytes(path);
                var found = GameFiles.Classify(bytes, Path.GetExtension(path));
                if (found.Format != DetectedFormat.Original || found.Profile is null || found.Kind is null) continue;
                var imported = GameFiles.Import(bytes, found.Profile, found.Kind, relative);
                Assert.Equal(3, ((JsonInteger)imported.Document["$version"]!).Value);
                var fromVersion1 = GameFiles.Read(GameFiles.Write(Version1Document(bytes, found.Profile, found.Kind, relative)), relative).Document;
                var fromVersion2 = GameFiles.Read(GameFiles.Write(Version2Document(bytes, found.Profile, found.Kind, relative)), relative).Document;
                // Import decodes with FormatSchema.Current. The version 2 step decodes the 39 pinned
                // properties through embedded codec specs, so the two documents are not one codec compared with itself.
                EqualJson(imported.Document, fromVersion1, relative + " version 1 migration");
                EqualJson(imported.Document, fromVersion2, relative + " version 2 migration");
                var root = (JsonObject)imported.Document["root"]!;
                var cls = found.Profile.RootHasClass
                    ? schema.Get(((JsonString)root["$class"]!).Value)
                    : schema.Get(found.Profile.RootFamily);
                Walk(root, cls, found.Profile.Engine, relative, schema, factory, counts);
            }
            var printed = new SortedSet<string>(StringComparer.Ordinal);
            foreach (var pair in counts)
                if (pair.Value > 0) printed.Add(pair.Key);
            foreach (var name in Watched) printed.Add(name);
            foreach (var name in printed)
            {
                counts.TryGetValue(name, out int count);
                blocked.TryGetValue(name, out var held);
                string line = held.Count == 0
                    ? name + " " + count
                    : name + " " + (count - held.Count) + " blocked " + held.Count + " " + held.Reason;
                output.WriteLine(line);
                Console.WriteLine(line);
            }
            foreach (var name in Required)
            {
                counts.TryGetValue(name, out int count);
                blocked.TryGetValue(name, out var held);
                Assert.True(count - held.Count > 0, name + " has no proved instance in the corpus. " + (held.Reason ?? ""));
            }
        }
        finally
        {
            reaper = null;
            AoWEUnit.AoWHSMap = previous;
            map.Destroy();
        }
    }

    private static void Walk(JsonObject obj, SchemaClass cls, string engine, string file, FormatSchema schema, EnginePersistableFactory factory, Dictionary<string, int> counts)
    {
        if (EngineTypes.TryGetValue(cls.Name, out var type) && TEObject.TreePersistenceRefusal(type) == null)
        {
            counts.TryGetValue(cls.Name, out int seen);
            counts[cls.Name] = seen + 1;
            Prove(obj, cls, engine, file, schema, factory);
        }
        foreach (var prop in cls.Properties)
        {
            var member = obj.Find(prop.Name);
            if (member is null) continue;
            if (prop.IsDynamic)
            {
                if (member.Value is JsonArray array)
                    foreach (var item in array.Items) Visit(cls, prop, item, engine, file, schema, factory, counts);
                continue;
            }
            Visit(cls, prop, member.Value, engine, file, schema, factory, counts);
        }
    }

    private static void Visit(SchemaClass cls, SchemaProperty prop, JsonValue value, string engine, string file, FormatSchema schema, EnginePersistableFactory factory, Dictionary<string, int> counts)
    {
        if (prop.Codec is FixedObjectCodec fixedObject && value is JsonObject nested)
            Walk(nested, schema.Get(fixedObject.ClassName), engine, file, schema, factory, counts);
        else if (prop.Codec is CreatedObjectCodec && value is JsonObject created && created["$class"] is JsonString name)
            Walk(created, schema.Get(name.Value), engine, file, schema, factory, counts);
        else if (prop.Codec is ChildrenCodec && value is JsonArray children)
        {
            foreach (var item in children.Items)
                if (item is JsonObject child && child["$class"] is JsonString childName)
                    Walk(child, schema.Get(childName.Value), engine, file, schema, factory, counts);
        }
        else if (prop.Codec is MapFieldsCodec && value is JsonArray rows)
        {
            var field = schema.Get(cls.FieldClass ?? "TMapField");
            foreach (var row in rows.Items)
                if (row is JsonArray cells)
                    foreach (var cell in cells.Items)
                        if (cell is JsonObject fieldObject)
                            Walk(fieldObject, field, engine, file, schema, factory, counts);
        }
        else if (prop.Codec is CampaignMapCodec && value is JsonObject map && map["root"] is JsonObject campaignRoot)
        {
            var campaignClass = campaignRoot["$class"] is JsonString nestedName ? schema.Get(nestedName.Value) : schema.Get("TAoWHSMap");
            Walk(campaignRoot, campaignClass, engine, file, schema, factory, counts);
        }
    }

    private static void Prove(JsonObject obj, SchemaClass cls, string engine, string file, FormatSchema schema, EnginePersistableFactory factory)
    {
        try
        {
            var image = PilotImage.Encode(obj, cls.Name, engine);
            var loaded = Blank(cls.Name);
            try
            {
                Load(loaded, image);
                var rewritten = Image(loaded);
                var expected = PilotImage.Decode(rewritten, cls.Name, engine);
                var tree = TreeWriter.Write(loaded, factory);
                var actual = DocumentBackend.Write(tree, cls, schema, engine);
                EqualJson(expected, actual, file + " " + cls.Name + " write");
                byte[] direct;
                try
                {
                    direct = OriginalBackend.Write(tree, cls, schema, engine);
                }
                catch (ConversionException error) when (IsDirectRefusal(error))
                {
                    Block(cls.Name, error);
                    return;
                }
                EqualBytes(rewritten, direct, file + " " + cls.Name + " original");
                var again = Blank(cls.Name);
                try
                {
                    TreeReader.Read(DocumentBackend.Read(expected, cls, schema, engine), again, factory);
                    EqualBytes(PilotImage.Encode(expected, cls.Name, engine), Image(again), file + " " + cls.Name + " read");
                }
                finally
                {
                    Release(again);
                }
                var fromImage = Blank(cls.Name);
                try
                {
                    ObjectNode read;
                    try
                    {
                        read = OriginalBackend.Read(image, cls, schema, engine);
                    }
                    catch (ConversionException error) when (IsDirectRefusal(error))
                    {
                        Block(cls.Name, error);
                        return;
                    }
                    TreeReader.Read(read, fromImage, factory);
                    EqualBytes(Image(loaded), Image(fromImage), file + " " + cls.Name + " readwrite");
                }
                finally
                {
                    Release(fromImage);
                }
            }
            finally
            {
                Release(loaded);
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

    // An image that still holds a run list, a map blob, a dynamic member or a case mapping.
    // The direct path refuses those shapes. The instance stays in the count and is not a proved round trip.
    private static readonly Dictionary<string, (int Count, string Reason)> blocked = new(StringComparer.Ordinal);

    private static void Block(string className, ConversionException error)
    {
        string reason = error.Diagnostic.Message;
        if (blocked.TryGetValue(className, out var seen)) blocked[className] = (seen.Count + 1, seen.Reason);
        else blocked[className] = (1, reason);
    }

    private static bool IsDirectRefusal(ConversionException error)
    {
        string message = error.Diagnostic.Message;
        return message.StartsWith("The direct path does not store", StringComparison.Ordinal)
            || message.StartsWith("PE-2c still needs", StringComparison.Ordinal)
            || message.StartsWith("PE-3 still needs", StringComparison.Ordinal)
            || message.StartsWith("A record has no image.", StringComparison.Ordinal)
            || message.Contains("uses a case mapping.", StringComparison.Ordinal);
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

    private static void Release(TEObject obj)
    {
        var previous = AoWEUnit.AoWHSMap;
        AoWEUnit.AoWHSMap = reaper;
        try
        {
            obj.Destroy();
        }
        finally
        {
            AoWEUnit.AoWHSMap = previous;
        }
    }

    private static TEObject Blank(string className)
    {
        foreach (var eclass in EClass.All)
            if (eclass.ClassName == className) return eclass.Create(null!);
        throw new InvalidOperationException("The engine has no class " + className + ".");
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

    private static JsonObject Version2Document(byte[] file, FileProfile profile, string kind, string relative) =>
        VersionDocument(file, profile, kind, relative, RecordMigration.Version2, 2);

    private static JsonObject Version1Document(byte[] file, FileProfile profile, string kind, string relative) =>
        VersionDocument(file, profile, kind, relative, SymbolicMigration.Version1, 1);

    private static JsonObject VersionDocument(byte[] file, FileProfile profile, string kind, string relative, FormatSchema schema, int version)
    {
        var session = new ImportSession
        {
            Schema = schema,
            Limits = FormatLimits.Default,
            Counts = new LayoutCounts(),
            File = relative,
        };
        var (container, root) = profile.Import(file, session);
        return new JsonObject()
            .Add("$format", new JsonString(OwnFormat.FormatName))
            .Add("$version", new JsonInteger(version))
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
