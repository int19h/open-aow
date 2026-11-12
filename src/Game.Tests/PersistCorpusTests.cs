using System.Globalization;
using System.Text;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Migrations;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

using Aow.Game.World.Map;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Aow.Game.Tests;

/// <summary>Every tree-persistable instance in the installed corpus matches the converter after an engine rewrite.</summary>
[Collection("Engine")]
[EngineState]
public sealed class PersistCorpusTests
{
    private static readonly string[] s_patterns =
        ["Scenario/Custom/*.hsm", "Scenario/Campaign/*.csm", "Scenario/Campaign/*.cam", "Release/*.ahl", "Release/*.hss", "TCMaps/*.hsm", "Save/*.asg"];

    // These subclasses are in the installed scenarios and saves. TRaidAG, TScoutAG and the other
    // behavior groups are absent from those files. TCustomAbilityList.PersistFields has no corpus
    // coverage. Every class that stores the bit array descends from TAbilityOwner, which overrides
    // ReadWrite and has no PersistFields, so the closure is refused before that method runs.
    // Class id 80 is shared with TEObject, so a created object is not named TAbilityList. The
    // constructed tests cover the bit array. TSetupSettings is not a member of a map, campaign,
    // hero list or save. Storm and animation sprites are absent from these files.
    private static readonly string[] s_required =
    [
        "GuardAIGroup", "PassiveAIGroup", "FortifyAIGroup", "NormalAIGroup", "SkirmishAIGroup",
        "AIBudgetManager", "ResourceEditSettings", "GlobalMagicControl",
    ];

    private static readonly string[] s_watched =
    [
"GuardAIGroup", "PassiveAIGroup", "FortifyAIGroup", "RaidAIGroup", "ScoutAIGroup", "NormalAIGroup", "SkirmishAIGroup",
        "GuardAreaAIGroup", "PatrolAIGroup", "RefugeAIGroup", "DefenderAIGroup", "AggressorAIGroup", "ExpanderAIGroup", "ScorcherAIGroup",
        "SuicidalAIGroup", "TransportAIGroup", "BehaviorAIGroup",
        "Storm", "IceStorm", "DeathStorm", "DivineStorm", "FireStorm", "LightningStorm", "PestilenceStorm", "BlastStorm",
        "Animation", "BloodShrineAnimation", "ImageSequenceAnimation", "SpringOfLifeAnimation", "TwinkleAnimation",
        "DamageCombatAction", "SelfDestructCombatAction", "TurnUndeadCombatAction", "RemotePlayer", "CustomAbilityList", "SetupSettings", "AIGroup",
        "AIBudgetManager", "ResourceEditSettings", "GlobalMagicControl",
    ];

    private readonly ITestOutputHelper _output;

    // TArmy.Destroy notifies AoWHSMap unless that map is being destroyed. A blank instance has no map.
    private static StrategicMap? s_reaper;

    public PersistCorpusTests(ITestOutputHelper output) => this._output = output;

    [CorpusFact]
    public void CorpusInstancesMatchTheConverter()
    {
        var previous = StrategicMap.Current;
        var map = new StrategicMap(null!);
        map.Status = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        s_reaper = map;
        s_blocked.Clear();
        s_proved.Clear();
        s_refusers.Clear();
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
                var classValue = found.Profile.RootHasClass
                    ? schema.Get(((JsonString)root["$class"]!).Value)
                    : schema.Get(found.Profile.RootFamily);
                Walk(root, classValue, found.Profile.Engine, relative, schema, factory, counts);
            }
            var printed = new SortedSet<string>(StringComparer.Ordinal);
            foreach (var pair in counts)
                if (pair.Value > 0) printed.Add(pair.Key);
            foreach (var name in s_watched) printed.Add(name);
            foreach (var name in printed)
            {
                s_proved.TryGetValue(name, out int ok);
                s_blocked.TryGetValue(name, out var held);
                string line = held.Count == 0
                    ? name + " " + ok
                    : name + " " + ok + " blocked " + held.Count + " " + held.Reason;
                _output.WriteLine(line);
                Console.WriteLine(line);
            }
            foreach (var pair in s_refusers.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key, StringComparer.Ordinal))
            {
                string line = "refused " + pair.Value + " " + pair.Key;
                _output.WriteLine(line);
                Console.WriteLine(line);
            }
            foreach (var name in s_required)
            {
                s_proved.TryGetValue(name, out int ok);
                s_blocked.TryGetValue(name, out var held);
                Assert.True(ok > 0, name + " has no proved instance in the corpus. " + (held.Reason ?? ""));
            }
        }
        finally
        {
            s_reaper = null;
            StrategicMap.Current = previous;
            map.Dispose();
        }
    }

    private static void Walk(JsonObject objectValue, SchemaClass classValue, string engine, string file, FormatSchema schema, EnginePersistableFactory factory, Dictionary<string, int> counts)
    {
        if (s_engineTypes.TryGetValue(classValue.Name, out var type) && EngineObject.TreePersistenceRefusal(type) == null)
        {
            counts.TryGetValue(classValue.Name, out int seen);
            counts[classValue.Name] = seen + 1;
            if (Prove(objectValue, classValue, engine, file, schema, factory))
            {
                s_proved.TryGetValue(classValue.Name, out int ok);
                s_proved[classValue.Name] = ok + 1;
            }
        }
        foreach (var prop in classValue.Properties)
        {
            var member = objectValue.Find(prop.Name);
            if (member is null) continue;
            if (prop.IsDynamic)
            {
                if (member.Value is JsonArray array)
                    foreach (var item in array.Items) Visit(classValue, prop, item, engine, file, schema, factory, counts);
                continue;
            }
            Visit(classValue, prop, member.Value, engine, file, schema, factory, counts);
        }
    }

    private static void Visit(SchemaClass classValue, SchemaProperty prop, JsonValue value, string engine, string file, FormatSchema schema, EnginePersistableFactory factory, Dictionary<string, int> counts)
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
            var field = schema.Get(classValue.FieldClass ?? "MapField");
            foreach (var row in rows.Items)
                if (row is JsonArray cells)
                    foreach (var cell in cells.Items)
                        if (cell is JsonObject fieldObject)
                            Walk(fieldObject, field, engine, file, schema, factory, counts);
        }
        else if (prop.Codec is CampaignMapCodec && value is JsonObject map && map["root"] is JsonObject campaignRoot)
        {
            var campaignClass = campaignRoot["$class"] is JsonString nestedName ? schema.Get(nestedName.Value) : schema.Get("StrategicMap");
            Walk(campaignRoot, campaignClass, engine, file, schema, factory, counts);
        }
    }

    private static bool Prove(JsonObject objectValue, SchemaClass classValue, string engine, string file, FormatSchema schema, EnginePersistableFactory factory)
    {
        try
        {
            var image = PilotImage.Encode(objectValue, classValue.Name, engine);
            var loaded = Blank(classValue.Name);
            try
            {
                try
                {
                    Load(loaded, image);
                }
                catch (Exception error) when (PortedExceptions.Is(error))
                {
                    // The fixture loads no game resources, so a part that needs one (a unit's
                    // resource) cannot load. Such an instance proves nothing either way.
                    Block(classValue.Name, "fixture: " + error.Message);
                    return false;
                }
                var rewritten = Image(loaded);
                var expected = PilotImage.Decode(rewritten, classValue.Name, engine);
                var tree = TreeWriter.Write(loaded, factory);
                var actual = DocumentBackend.Write(tree, classValue, schema, engine);
                EqualJson(expected, actual, file + " " + classValue.Name + " write");
                byte[] direct;
                try
                {
                    direct = OriginalBackend.Write(tree, classValue, schema, engine);
                }
                catch (ConversionException error) when (IsDirectRefusal(error))
                {
                    Block(classValue.Name, error.Diagnostic.Message);
                    return false;
                }
                EqualBytes(rewritten, direct, file + " " + classValue.Name + " original");
                var again = Blank(classValue.Name);
                try
                {
                    TreeReader.Read(DocumentBackend.Read(expected, classValue, schema, engine), again, factory, Stop);
                    EqualBytes(PilotImage.Encode(expected, classValue.Name, engine), Image(again), file + " " + classValue.Name + " read");
                }
                finally
                {
                    Release(again);
                }
                var fromImage = Blank(classValue.Name);
                try
                {
                    ObjectNode read;
                    try
                    {
                        read = OriginalBackend.Read(image, classValue, schema, engine);
                    }
                    catch (ConversionException error) when (IsDirectRefusal(error))
                    {
                        Block(classValue.Name, error.Diagnostic.Message);
                        return false;
                    }
                    TreeReader.Read(read, fromImage, factory, Stop);
                    EqualBytes(Image(loaded), Image(fromImage), file + " " + classValue.Name + " readwrite");
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
            return true;
        }
        catch (InvalidOperationException error) when (IsTreeRefusal(error))
        {
            Block(classValue.Name, RefusalName(error));
            return false;
        }
        catch (Exception error) when (error is not XunitException)
        {
            throw new InvalidOperationException(file + " " + classValue.Name + " failed. " + error.Message, error);
        }
    }

    // An instance is proved only when the write and both reads finish. A refusal anywhere in the
    // closure blocks it. The label is the refusing class, or the direct-path diagnostic when no class is named.
    private static readonly Dictionary<string, int> s_proved = new(StringComparer.Ordinal);
    private static readonly Dictionary<string, (int Count, string Reason)> s_blocked = new(StringComparer.Ordinal);
    private static readonly Dictionary<string, int> s_refusers = new(StringComparer.Ordinal);

    private static void Block(string className, string label)
    {
        if (s_blocked.TryGetValue(className, out var seen)) s_blocked[className] = (seen.Count + 1, seen.Reason);
        else s_blocked[className] = (1, label);
        s_refusers.TryGetValue(label, out int count);
        s_refusers[label] = count + 1;
    }

    private static string RefusalName(InvalidOperationException error)
    {
        const string Prefix = "Class ";
        int nameEnd = error.Message.IndexOf(' ', Prefix.Length);
        return error.Message.Substring(Prefix.Length, nameEnd - Prefix.Length);
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

    private static readonly Dictionary<string, Type> s_engineTypes = BuildEngineTypes();

    private static Dictionary<string, Type> BuildEngineTypes()
    {
        var types = new Dictionary<string, Type>();
        foreach (var engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(EngineObject).Assembly) continue;
            types.TryAdd(engineClass.Type.Name, engineClass.Type);
        }
        return types;
    }

    private static bool IsTreeRefusal(InvalidOperationException error)
    {
        string message = error.Message;
        const string Prefix = "Class ";
        if (!message.StartsWith(Prefix, StringComparison.Ordinal)) return false;
        int nameEnd = message.IndexOf(' ', Prefix.Length);
        if (nameEnd < 0) return false;
        string name = message.Substring(Prefix.Length, nameEnd - Prefix.Length);
        return s_engineTypes.TryGetValue(name, out var type) && EngineObject.TreePersistenceRefusal(type) == message;
    }

    private static void Release(EngineObject objectValue)
    {
        var previous = StrategicMap.Current;
        StrategicMap.Current = s_reaper;
        try
        {
            objectValue.Dispose();
        }
        finally
        {
            StrategicMap.Current = previous;
        }
    }

    private static EngineObject Blank(string className)
    {
        foreach (var engineClass in EngineClass.All)
            if (engineClass.StorageKey == className) return engineClass.CreateInstance(null!);
        throw new InvalidOperationException("The engine has no class " + className + ".");
    }

    // Both loads stop on a nested error, so a dropped part fails the comparison instead of
    // vanishing from both sides.
    private static void Stop(string message, ref bool continueLoad) => continueLoad = false;

    private static void Load(EngineObject objectValue, byte[] image)
    {
        using var memory = new MemoryStream();
        memory.Write(image);
        memory.Position = 0;
        var reader = new EReadStorageStream(memory, EngineFixture.Engine, false);
        reader.OnError = static (EStorageStream _, string _, ref bool cont) => cont = false;
        try
        {
            objectValue.MainReadWrite(reader);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private static byte[] Image(EngineObject objectValue)
    {
        using var memory = new MemoryStream();
        using (var writer = new EWriteStorageStream(memory, EngineFixture.Engine, false))
        {
            objectValue.MainReadWrite(writer);
        }
        return memory.ToArray();
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
                    return path + " member " + i.ToString(CultureInfo.InvariantCulture) + " " + one.Name + " / " + other.Name;
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
                string at = path + "[" + i.ToString(CultureInfo.InvariantCulture) + "]";
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
        foreach (var pattern in s_patterns)
        {
            var directory = Path.Combine(Root, Path.GetDirectoryName(pattern)!);
            if (!Directory.Exists(directory)) continue;
            foreach (var file in Directory.EnumerateFiles(directory, Path.GetFileName(pattern), new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive }))
                yield return Path.GetRelativePath(Root, file);
        }
    }
}
