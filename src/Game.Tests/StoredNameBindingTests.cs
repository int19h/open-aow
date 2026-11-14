using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Rtl;
using Xunit.Abstractions;

namespace Aow.Game.Tests;

/// <summary>
/// Issue 230: every schema member name is the name of the C# member its storage call reads and
/// writes, and every record field and enum member stores its C# name. A member that no single C#
/// member binds, such as an array element, a local the storage method copies, a second copy of one
/// member or a stored value nothing reads, is listed in <see cref="s_unboundNames"/> with what it
/// holds. A private field <c>_name</c> binds the name <c>Name</c>.
/// </summary>
[Collection("Engine")]
[EngineState]
public sealed class StoredNameBindingTests(ITestOutputHelper output)
{
    private const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

    // The stored names that bind no C# member of that name, by schema class, with what each holds.
    private static readonly Dictionary<(string Class, string Name), string> s_unboundNames = new()
    {
        [("AbilityOwner", "AbilityDataIds")] = "the ability ids of the AbilityData chain, a local list of ReadWrite",
        [("ArmyId", "Location")] = "a local that packs X, Y, MapLevel and UnknownBytes",
        [("Battle", "AttackerSide")] = "Sides[CombatSideIndex.Attacker]",
        [("Battle", "DefenderSide")] = "Sides[CombatSideIndex.Defender]",
        [("Campaign", "Map")] = "the map stream of the campaign, written through a local",
        [("DiplomaticAction", "Gold")] = "Proposal.ToFields().Gold",
        [("DiplomaticAction", "Mana")] = "Proposal.ToFields().Mana",
        [("DiplomaticAction", "Kind")] = "Proposal.ToFields().Kind",
        [("DiplomaticAction", "Message")] = "Proposal.Message",
        [("Hero", "OldExperience")] = "a local, the experience of an old document, read only",
        [("MapObjectResource", "Complex")] = "a local, whether ImageSequenceList exists",
        [("ProductionItem", "Kind")] = "Order.ToFields().Kind",
        [("ProductionItem", "Parameter")] = "Order.ToFields().Parameter",
        [("ProductionItem", "GroupId")] = "Order.ToFields().GroupId",
        [("StrategicMap", "HeroControlCopy")] = "HeroControl again, the original stores it twice",
        [("StrategicMap", "UnknownProperty57")] = "a Boolean that Tutorial stores and no 1.36 code reads",
        [("Troop", "LegacyGroup")] = "a local, the legacy group control whose behavior becomes LegacyGroupBehavior",
        [("Unit", "UnitResourceIndex")] = "a local, the index of UnitResource",
        [("UnitGraphicsResource", "SelectSfxLibrary")] = "SfxLibraries[0], played when the unit is selected",
        [("UnitGraphicsResource", "MoveSfxLibrary")] = "SfxLibraries[1], played on move frames",
        [("UnitGraphicsResource", "MeleeSfxLibrary")] = "SfxLibraries[2], played on a melee strike",
        [("UnitGraphicsResource", "HitSfxLibrary")] = "SfxLibraries[3], played when the unit is hit",
        [("UnitGraphicsResource", "DeathSfxLibrary")] = "SfxLibraries[4], played when the unit is killed",
        [("UnitResource", "NoRankAbilities")] = "Abilities[Unit.Rank.None]",
        [("UnitResource", "SilverRankAbilities")] = "Abilities[Unit.Rank.Silver]",
        [("UnitResource", "GoldRankAbilities")] = "Abilities[Unit.Rank.Gold]",
    };

    [Fact]
    public void EverySchemaMemberNamesItsCSharpMember()
    {
        var schema = FormatSchema.Current;
        var failures = new List<string>();
        var used = new HashSet<(string, string)>();
        int checkedNames = 0;
        foreach (var classValue in schema.Classes)
        {
            if (classValue.Own.Count == 0) continue;
            var type = TypeOf(classValue.Name);
            if (type is null)
            {
                failures.Add(classValue.Name + " names no C# type");
                continue;
            }
            foreach (var property in classValue.Own)
            {
                checkedNames++;
                bool bound = Member(type, property.Name) is not null;
                bool listed = s_unboundNames.ContainsKey((classValue.Name, property.Name));
                if (listed) used.Add((classValue.Name, property.Name));
                if (bound && listed) failures.Add(classValue.Name + "." + property.Name + " is listed as unbound and binds " + type.Name + "." + property.Name);
                if (!bound && !listed) failures.Add(classValue.Name + "." + property.Name + " names no member of " + type.FullName);
            }
        }
        foreach (var key in s_unboundNames.Keys)
            if (!used.Contains(key)) failures.Add(key.Class + "." + key.Name + " is listed as unbound and is no schema member");
        output.WriteLine(checkedNames + " schema members checked, " + s_unboundNames.Count + " bind no member of their name");
        Assert.True(failures.Count == 0, string.Join("\n", failures));
    }

    // The original image of an object whose integer members hold distinct values decodes, under each
    // stored name, the value of the C# member of that name. A storage call that writes another
    // member under the name fails here.
    [Fact]
    public void EveryStoredIntegerComesFromTheMemberOfItsName()
    {
        var schema = FormatSchema.Current;
        var failures = new List<string>();
        var skipped = new List<string>();
        int objects = 0, values = 0;
        var combatMap = CombatMap.Current;
        foreach (var engineClass in EngineClass.All)
        {
            var classValue = schema.Find(engineClass.StorageKey);
            if (classValue is null || classValue.Properties.Count == 0) continue;
            EngineObject? objectValue = null;
            try
            {
                objectValue = engineClass.CreateInstance(null!, holder: EngineFixture.Engine);
                var filled = Fill(objectValue, classValue);
                if (filled.Count == 0) continue;
                byte[] image = Image(objectValue);
                var document = PilotImage.Decode(image, classValue.Name);
                objects++;
                foreach (var (name, expected) in filled)
                {
                    if (document.Find(name)?.Value is not JsonInteger stored) continue;
                    values++;
                    if (stored.Value != expected)
                        failures.Add(classValue.Name + "." + name + " stores " + stored.Value + ", the member holds " + expected);
                }
            }
            catch (Exception error) when (error is not Xunit.Sdk.XunitException)
            {
                skipped.Add(classValue.Name + ": " + error.GetType().Name + " " + error.Message);
            }
            finally
            {
                Release(objectValue);
                CombatMap.Current = combatMap;
            }
        }
        output.WriteLine(objects + " objects, " + values + " stored integers checked, " + skipped.Count + " classes skipped");
        foreach (var line in skipped) output.WriteLine("skipped " + line);
        Assert.True(failures.Count == 0, string.Join("\n", failures));
        Assert.True(objects >= 300, "only " + objects + " objects were checked");
    }

    [Fact]
    public void RecordFieldsAndEnumMembersStoreTheirCSharpNames()
    {
        var records = new SortedSet<string>(StringComparer.Ordinal);
        var enums = new SortedSet<string>(StringComparer.Ordinal);
        Collect(SchemaSource(), records, enums);
        Assert.NotEmpty(records);
        Assert.NotEmpty(enums);
        var failures = new List<string>();
        foreach (string name in records)
        {
            // A record is the image its [StoredRecord] binding names.
            var info = RecordCatalog.Find(name) ?? throw new InvalidOperationException("The record " + name + " is not in the catalog.");
            var type = info.Image;
            if (RecordTypeInfo.StoredName(type) != name) failures.Add("record " + name + " is bound to " + type.FullName + ", which stores " + RecordTypeInfo.StoredName(type));
            foreach (var field in info.Fields)
                if (Member(type, field.Name) is null) failures.Add("record " + name + " stores " + field.Name + ", which is no member of " + type.FullName);
        }
        foreach (string name in enums)
        {
            var type = NameTable.Find(name);
            var stored = EnumTypeInfo.Describe(type).ByName.Keys.Order(StringComparer.Ordinal).ToArray();
            Assert.Equal(Enum.GetNames(type).Order(StringComparer.Ordinal).ToArray(), stored);
        }
        output.WriteLine(records.Count + " records and " + enums.Count + " enums checked");
        Assert.True(failures.Count == 0, string.Join("\n", failures));
    }

    private static Type? TypeOf(string className)
    {
        try
        {
            return NameTable.Find(className);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    // The field or property of that name, or the private field _name, in the type or its bases.
    private static MemberInfo? Member(Type type, string name)
    {
        if (MemberNames.Field(type, name) is { } field) return field;
        if (MemberNames.Field(type, "_" + char.ToLowerInvariant(name[0]) + name[1..]) is { } privateField) return privateField;
        for (var current = type; current is not null; current = current.BaseType)
            if (current.GetProperty(name, Instance | BindingFlags.DeclaredOnly) is { } property) return property;
        return null;
    }

    // Gives each integer field that a schema member names a value no other one has, from first on,
    // and returns the values by stored name. The members of skip keep their values.
    internal static List<(string Name, long Value)> Fill(EngineObject objectValue, SchemaClass classValue, int first = 11, IReadOnlySet<string>? skip = null)
    {
        var filled = new List<(string, long)>();
        int next = first;
        foreach (var property in classValue.Properties)
        {
            if (skip?.Contains(property.Name) == true) continue;
            if (Member(objectValue.GetType(), property.Name) is not FieldInfo field || field.IsInitOnly) continue;
            object? value = Type.GetTypeCode(field.FieldType) switch
            {
                TypeCode.Int32 => next,
                TypeCode.UInt16 => (ushort)next,
                TypeCode.Int16 => (short)next,
                TypeCode.Byte => (byte)next,
                TypeCode.SByte => (sbyte)next,
                _ => null,
            };
            if (value is null || field.FieldType.IsEnum) continue;
            field.SetValue(objectValue, value);
            filled.Add((property.Name, next));
            next++;
            if (next > first + 109) break;
        }
        return filled;
    }

    // A blank object of some classes cannot be destroyed outside a map. The test only drops it.
    private static void Release(EngineObject? objectValue)
    {
        try
        {
            objectValue?.Dispose();
        }
        catch (Exception error) when (error is NullReferenceException or AbstractErrorException or InvalidCastException)
        {
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

    private static JsonObject SchemaSource()
    {
        using var stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.schema.json")!;
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return (JsonObject)JsonReader.Parse(memory.ToArray(), "schema.json");
    }

    private static void Collect(JsonValue value, SortedSet<string> records, SortedSet<string> enums)
    {
        switch (value)
        {
            case JsonObject objectValue:
                foreach (var member in objectValue.Members)
                {
                    if (member.Value is JsonString { Value: var name })
                    {
                        if (member.Name == "record") records.Add(name);
                        else if (member.Name is "enum" or "set") enums.Add(name);
                    }
                    Collect(member.Value, records, enums);
                }
                break;
            case JsonArray array:
                foreach (var item in array.Items) Collect(item, records, enums);
                break;
        }
    }
}
