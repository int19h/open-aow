using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;

using Aow.Game.Units;
using Aow.Game.World;
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
    // behavior groups are absent from those files. Items prove the bit array of
    // TCustomAbilityList, the ability-id list and the keyed ability data, item controls the keyed
    // items, the relation and tutorial lists the byte and integer lists with a stored default, and
    // race resources the unit index lists, diplomatic action lists the reference children of
    // EReferenceNode.
    // Class id 80 is shared with TEObject, so a created object is not named TAbilityList.
    // TSetupSettings is not a member of a map, campaign, hero list or save. Storm and animation
    // sprites are absent from these files. Diplomatic actions prove the proposal's case mapping,
    // the campaign files the world map campaign, its scenarios and pages and the transfer settings,
    // and the sound libraries and unit graphics the sound sequences.
    // The game set files hold the resources of units, heroes, items and abilities, which no map,
    // campaign, hero list or save stores. Each is a written object between two CRC words, read as
    // GameSetTests reads it.
    private static readonly (string Relative, string RootClass, string Engine)[] s_gameSetFiles =
    [
        ("Release/Ability.pfs", "AbilityControl", "game"),
        ("Release/Spells.pfs", "SpellControl", "game"),
        ("Release/Unitgfx.pfs", "UnitGFXResourceList", "game"),
        ("Release/Unitres.pfs", "UnitResourceList", "game"),
        ("Release/HERORES.PFS", "HeroResourceList", "game"),
        ("Release/ITEMGFX.PFS", "ItemGFXList", "game"),
        ("Release/FACERES.PFS", "FaceResourceList", "game"),
        ("Release/TEXT.pfs", "TextControl", "game"),
        ("Release/comb_res.pfs", "MapObjectResourceList", "tactical"),
    ];

    private static readonly string[] s_required =
    [
        "GuardAIGroup", "PassiveAIGroup", "FortifyAIGroup", "NormalAIGroup", "SkirmishAIGroup",
        "AIBudgetManager", "ResourceEditSettings", "GlobalMagicControl",
        "Item", "ItemControl", "PlayerDiplomaticRelations", "TutorialControl", "RaceResource",
        "DiplomaticActionList", "Army", "DefendersArmy", "PrisonersArmy", "Unit", "Hero", "Leader",
        "ProductionItem", "ProductionQueue", "UnitProduction",
        "Player", "PlayerList", "DiplomaticAction", "WorldMapCampaign", "WorldMapCampaignScenario", "CampaignScenarioPage",
        "CampaignTransferSettings", "EventLogbook", "EventLogList", "AIGroupManager", "GuardAreaAIGroup", "PatrolAIGroup",
        "SfxSequence", "SfxLibrary", "UnitGraphicsResource",
        "CombatParty", "CombatPlayer", "CombatUnit", "FastBattleUnit", "CombatLogbook",
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
        "Item", "ItemControl", "PlayerDiplomaticRelations", "TutorialControl",
        "CommandAbilityData", "CommandedAbilityData", "PossessedAbilityData", "ItemExplorationSiteEventLog",
        "RaceResource", "AIPlayerControl", "AbilityControl", "SpellControl",
        "DiplomaticActionList", "PlayerList", "EResourceList",
    ];

    private readonly ITestOutputHelper _output;

    // TArmy.Destroy notifies AoWHSMap unless that map is being destroyed. A blank instance has no map.
    private static StrategicMap? s_reaper;

    public PersistCorpusTests(ITestOutputHelper output) => this._output = output;

    [CorpusFact]
    public void CorpusInstancesMatchTheConverter()
    {
        var previous = StrategicMap.Current;
        var map = new StrategicMap(null!, holder: EngineFixture.Engine);
        map.Status = Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Destroying);
        s_reaper = map;
        // A player's write reads the current map (its seat, its connection type and the view
        // center), so the walk runs with the destroying map current.
        StrategicMap.Current = map;
        s_blocked.Clear();
        s_proved.Clear();
        s_refusers.Clear();
        s_levels.Clear();
        s_levelBlocks.Clear();
        // A unit's load looks its resource up in the game set, so the walk reads the installed
        // unit resources into the fixture's game set and puts the empty list back after.
        int previousSeed = Runtime.RandSeed;
        var gameSet = GamePack.Current;
        var emptyResources = gameSet.UnitResources;
        var unitResources = UnitResources();
        gameSet.UnitResources = unitResources;
        var enlisted = Enlist();
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
                Assert.Equal(OwnFormat.CurrentVersion, ((JsonInteger)imported.Document["$version"]!).Value);
                var root = (JsonObject)imported.Document["root"]!;
                var classValue = found.Profile.RootHasClass
                    ? schema.Get(((JsonString)root["$class"]!).Value)
                    : schema.Get(found.Profile.RootFamily);
                Walk(root, classValue, found.Profile.Engine, relative, schema, factory, counts);
            }
            foreach (var (relative, rootClass, engine) in s_gameSetFiles)
            {
                var path = Path.Combine(Root, relative);
                if (!File.Exists(path)) continue;
                var bytes = File.ReadAllBytes(path);
                var profile = new WrittenObjectProfile("pfs", ["pfs"], rootClass, engine);
                var imported = GameFiles.Import(bytes[4..^4], profile, "pfs", relative);
                Walk((JsonObject)imported.Document["root"]!, schema.Get(rootClass), engine, relative, schema, factory, counts);
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
            PrintLevels();
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
            Runtime.RandSeed = previousSeed;
            gameSet.UnitResources = emptyResources;
            unitResources.Dispose();
            s_reaper = null;
            StrategicMap.Current = previous;
            map.Dispose();
        }
    }

    // The fixture engine enlists the classes the game registers when the engine starts. The game
    // enlists the event logs, the campaign pages and scenarios and the AI groups and budgets the saves
    // hold when its screens and the AI start, and the combat spell actions with the combat spells, so
    // the walk enlists every class of the session, event, AI, magic, interface, audio and combat
    // namespaces that the engine does not hold yet, and takes them out after. Each class id is
    // enlisted once.
    private static readonly string[] s_enlistedNamespaces =
        ["Aow.Game.Session", "Aow.Game.Events", "Aow.Game.AI", "Aow.Game.Magic", "Aow.Game.UI", "Aow.Game.Audio", "Aow.Game.Combat"];

    private static EngineClass[] Enlist()
    {
        var table = EngineFixture.Engine.ClassTable;
        var added = new Dictionary<int, EngineClass>();
        foreach (var engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != typeof(GamePack).Assembly || engineClass.Type.Namespace is not { } space) continue;
            if (!s_enlistedNamespaces.Any(prefix => space == prefix || space.StartsWith(prefix + ".", comparisonType: StringComparison.Ordinal))) continue;
            if (table.ContainsKey(engineClass.ClassId) || added.ContainsKey(engineClass.ClassId)) continue;
            added[engineClass.ClassId] = engineClass;
        }
        var classes = added.Values.ToArray();
        EngineFixture.Engine.RegisterEngineClasses(classes);
        return classes;
    }

    // Unitres.pfs holds the stored object between two CRC words, and GameEngine.ReadFromFileCrc
    // reads the image between them.
    private static UnitResourceList UnitResources()
    {
        var list = new UnitResourceList(null!) { Holder = EngineFixture.Engine };
        var bytes = File.ReadAllBytes(Path.Combine(Root, "Release", "Unitres.pfs"));
        EngineFixture.Engine.ReadObjectImage(bytes[4..^4], objectValue: list, progress: null, classTable: "game");
        return list;
    }

    private static void Walk(JsonObject objectValue, SchemaClass classValue, string engine, string file, FormatSchema schema, EnginePersistableFactory factory, Dictionary<string, int> counts)
    {
        if (IsMapLevel(classValue)) ProveLevel(objectValue, classValue, engine, file, schema);
        else if (classValue.Name != "MapField" && s_engineTypes.TryGetValue(classValue.Name, out var type) && EngineObject.TreePersistenceRefusal(type) == null)
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
        // MapContainer.ReadWrite makes its owner's map hold it, so a blank container with no map
        // cannot load. Its levels are proved on the direct path, and a constructed test proves it.
        if (classValue.Name == "MapContainer")
        {
            Block(classValue.Name, "fixture: a map container loads only into its map");
            return false;
        }

        try
        {
            var image = PilotImage.Encode(objectValue, classValue.Name, engine);
            // An image sequence draws Random once per frame-table layer when it loads, so every
            // load of the instance starts from the same seed and draws the same image indices.
            int seed = Runtime.RandSeed;
            var loaded = Blank(classValue.Name);
            try
            {
                try
                {
                    Runtime.RandSeed = seed;
                    Load(loaded, image);
                }
                catch (Exception error) when (PortedExceptions.Is(error))
                {
                    // The fixture loads no game resources, so a part that needs one (a unit's
                    // resource) cannot load. Such an instance proves nothing either way.
                    Block(classValue.Name, "fixture: " + FixtureLabel(error));
                    return false;
                }
                catch (NullReferenceException error)
                {
                    // A hexagon sprite whose resource does not link reads that resource anyway
                    // (HexBridge.LinkToResource), and the fixture has no map for a sprite to
                    // place itself on. The stream load fails before the tree is involved.
                    Block(classValue.Name, "fixture: null reference in " + error.TargetSite?.DeclaringType?.Name + "." + error.TargetSite?.Name);
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
                // A class whose ReadWrite is retired (PE-3-i) loads and writes through the tree on
                // the stream too, so its stream image is this tree's image, and the comparisons with
                // the stream's write and load prove nothing for it. The converter's write and read
                // comparisons stay.
                bool retired = EngineObject.StreamRetired(loaded.GetType());
                if (!retired)
                    EqualBytes(rewritten, direct, file + " " + classValue.Name + " original");
                var again = Blank(classValue.Name);
                try
                {
                    Runtime.RandSeed = seed;
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
                    Runtime.RandSeed = seed;
                    TreeReader.Read(read, fromImage, factory, Stop);
                    if (!retired)
                        EqualBytes(Image(loaded), actual: Image(fromImage), label: file + " " + classValue.Name + " readwrite");
                }
                finally
                {
                    Release(fromImage);
                }
                // PE-3-h: a copy goes through the tree and loads what the stream copy of the
                // instance loaded (WriteEObject, then ReadEObject into a new object).
                Runtime.RandSeed = seed;
                var copied = loaded.Copy(EngineFixture.Engine);
                var streamCopied = Blank(classValue.Name);
                try
                {
                    Runtime.RandSeed = seed;
                    Load(streamCopied, Image(loaded));
                    EqualBytes(Image(streamCopied), actual: Image(copied), label: file + " " + classValue.Name + " copy");
                }
                finally
                {
                    Release(streamCopied);
                    Release(copied);
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

    // A map level and its fields have no engine proof here: the fixture loads no resources, so
    // every hexagon sprite fails its resource link, and a level needs a map to report progress
    // to. The constructed level of PersistEquivalenceTests proves them against MainReadWrite. The
    // direct path proves every level of the corpus: the converter's image of the level, read into
    // a tree by the original backend, writes back the canonical image, the document backend writes
    // the converter's document of that image, and that document read back writes the same image.
    // The read keeps every value: the converter's document of the stored image less its $layout
    // parts equals the document of the canonical image.
    private static readonly Dictionary<string, (int Levels, long Fields, long Sprites)> s_levels = new(StringComparer.Ordinal);
    private static readonly Dictionary<string, int> s_levelBlocks = new(StringComparer.Ordinal);

    private static bool IsMapLevel(SchemaClass classValue) => classValue.Properties.Any(prop => prop.Codec is MapFieldsCodec);

    private static void ProveLevel(JsonObject level, SchemaClass classValue, string engine, string file, FormatSchema schema)
    {
        string label = file + " " + classValue.Name;
        var image = PilotImage.Encode(level, classValue.Name, engine);
        ObjectNode tree;
        byte[] direct;
        try
        {
            tree = OriginalBackend.Read(image, classValue, schema, engine);
            direct = OriginalBackend.Write(tree, classValue, schema, engine);
        }
        catch (ConversionException error) when (IsDirectRefusal(error))
        {
            s_levelBlocks.TryGetValue(error.Diagnostic.Message, out int seen);
            s_levelBlocks[error.Diagnostic.Message] = seen + 1;
            return;
        }
        var expected = PilotImage.Decode(direct, classValue.Name, engine);
        EqualJson(WithoutLayout(PilotImage.Decode(image, classValue.Name, engine)), expected, label + " direct read");
        EqualBytes(direct, PilotImage.Encode(expected, classValue.Name, engine), label + " direct canonical");
        EqualJson(expected, DocumentBackend.Write(tree, classValue, schema, engine), label + " direct document");
        EqualBytes(direct, OriginalBackend.Write(DocumentBackend.Read(expected, classValue, schema, engine), classValue, schema, engine), label + " direct document read");
        var grid = (GridNode)tree.Find("Fields")!;
        long sprites = 0;
        foreach (var field in grid.Fields)
            if (field.Find("HexSpriteList") is SequenceNode sequence) sprites += sequence.Items.Count;
        s_levels.TryGetValue(classValue.Name, out var total);
        s_levels[classValue.Name] = (total.Levels + 1, total.Fields + grid.Fields.Count, total.Sprites + sprites);
    }

    private static JsonObject WithoutLayout(JsonObject objectValue)
    {
        objectValue.Members.RemoveAll(member => member.Name == "$layout");
        foreach (var member in objectValue.Members) WithoutLayout(member.Value);
        return objectValue;
    }

    private static void WithoutLayout(JsonValue value)
    {
        if (value is JsonObject nested) WithoutLayout(nested);
        else if (value is JsonArray array)
            foreach (var item in array.Items) WithoutLayout(item);
    }

    private void PrintLevels()
    {
        int levels = 0;
        foreach (var (name, (count, fields, sprites)) in s_levels.OrderBy(pair => pair.Key, StringComparer.Ordinal))
        {
            levels += count;
            Print("direct " + name + " " + count + " levels, " + fields + " fields, " + sprites + " sprites");
        }
        foreach (var (message, count) in s_levelBlocks.OrderByDescending(pair => pair.Value).ThenBy(pair => pair.Key, StringComparer.Ordinal))
        {
            levels += count;
            Print("direct blocked " + count + " " + message);
        }
        Print("direct levels " + levels);
        foreach (var message in s_levelBlocks.Keys)
            Assert.False(message.Contains("map fields", StringComparison.Ordinal) || message.Contains("dynamic member", StringComparison.Ordinal),
                "A map level is refused for its fields or sprites: " + message);
    }

    private void Print(string line)
    {
        _output.WriteLine(line);
        Console.WriteLine(line);
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

    // A missing resource names its id, and a missing hexagon sprite set its path under the
    // fixture's temporary root (the stream's error text). The label keeps the class and the file
    // name, so one cause sums under one refuser line.
    private static string FixtureLabel(Exception error) => error switch
    {
        MapObjectResourceMissingException => Regex.Replace(error.Message, " -?[0-9]+$", ""),
        _ when error.Message.StartsWith(EngineFixture.Root, StringComparison.OrdinalIgnoreCase) =>
            "no hexagon sprite set " + Path.GetFileName(error.Message.Split(' ')[0]).ToUpperInvariant(),
        _ => error.Message,
    };

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
            if (engineClass.StorageKey == className) return engineClass.CreateInstance(null!, holder: EngineFixture.Engine);
        throw new InvalidOperationException("The engine has no class " + className + ".");
    }

    // Both loads stop on a nested error, so a dropped part fails the comparison instead of
    // vanishing from both sides.
    private static void Stop(string message, out bool continueLoad) => continueLoad = false;

    private static void Load(EngineObject objectValue, byte[] image)
    {
        using var memory = new MemoryStream();
        memory.Write(image);
        memory.Position = 0;
        var reader = new StorageReader(memory, EngineFixture.Engine, false);
        reader.OnError += static (StorageStream _, string _, ref bool cont) => cont = false;
        try
        {
            reader.PersistObject(objectValue);
        }
        finally
        {
            reader.Dispose();
        }
    }

    private static byte[] Image(EngineObject objectValue)
    {
        using var memory = new MemoryStream();
        using (var writer = new StorageWriter(memory, EngineFixture.Engine, false))
        {
            writer.PersistObject(objectValue);
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
