using System.Reflection;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Game.Combat;
using Aow.Game.Session;
using Xunit.Abstractions;
using EngineReferenceNode = Aow.Engine.Nodes.ReferenceNode;

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
        FormatSchema schema = FormatSchema.Current;
        List<string> failures = [];
        HashSet<(string, string)> used = [];
        int checkedNames = 0;
        foreach (SchemaClass classValue in schema.Classes)
        {
            if (classValue.Own.Count == 0)
            {
                continue;
            }

            Type? type = TypeOf(classValue.Name);
            if (type is null)
            {
                failures.Add(classValue.Name + " names no C# type");
                continue;
            }
            foreach (SchemaProperty property in classValue.Own)
            {
                checkedNames++;
                bool bound = Member(type, property.Name) is not null;
                bool listed = s_unboundNames.ContainsKey((classValue.Name, property.Name));
                if (listed)
                {
                    _ = used.Add((classValue.Name, property.Name));
                }

                if (bound && listed)
                {
                    failures.Add(classValue.Name + "." + property.Name + " is listed as unbound and binds " + type.Name + "." + property.Name);
                }

                if (!bound && !listed)
                {
                    failures.Add(classValue.Name + "." + property.Name + " names no member of " + type.FullName);
                }
            }
        }
        foreach ((string Class, string Name) key in s_unboundNames.Keys)
        {
            if (!used.Contains(key))
            {
                failures.Add(key.Class + "." + key.Name + " is listed as unbound and is no schema member");
            }
        }

        output.WriteLine(checkedNames + " schema members checked, " + s_unboundNames.Count + " bind no member of their name");
        Assert.True(failures.Count == 0, string.Join("\n", failures));
    }

    // The original image of an object whose integer members hold distinct values decodes, under each
    // stored name, the value of the C# member of that name. A storage call that writes another
    // member under the name fails here.
    [Fact]
    public void EveryStoredIntegerComesFromTheMemberOfItsName()
    {
        FormatSchema schema = FormatSchema.Current;
        List<string> failures = [];
        List<string> skipped = [];
        int objects = 0, values = 0;
        CombatMap combatMap = CombatMap.Current;
        foreach (EngineClass? engineClass in EngineClass.All)
        {
            SchemaClass? classValue = schema.Find(engineClass.StorageKey);
            if (classValue is null || classValue.Properties.Count == 0)
            {
                continue;
            }

            EngineObject? objectValue = null;
            try
            {
                objectValue = engineClass.CreateInstance(null, holder: EngineFixture.Engine);
                List<(string Name, long Value)> filled = Fill(objectValue, classValue);
                if (filled.Count == 0)
                {
                    continue;
                }

                JsonObject document = PilotImage.Decode(Image(objectValue), className: classValue.Name);
                objects++;
                foreach ((string? name, long expected) in filled)
                {
                    if (document.Find(name)?.Value is not JsonInteger stored)
                    {
                        continue;
                    }

                    values++;
                    if (stored.Value != expected)
                    {
                        failures.Add(classValue.Name + "." + name + " stores " + stored.Value + ", the member holds " + expected);
                    }
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
        foreach (string line in skipped)
        {
            output.WriteLine("skipped " + line);
        }

        Assert.True(failures.Count == 0, string.Join("\n", failures));
        Assert.True(objects >= 300, "only " + objects + " objects were checked");
    }

    [Fact]
    public void RecordFieldsAndEnumMembersStoreTheirCSharpNames()
    {
        SortedSet<string> records = [with(StringComparer.Ordinal)];
        SortedSet<string> enums = [with(StringComparer.Ordinal)];
        Collect(SchemaSource(), records, enums);
        Assert.NotEmpty(records);
        Assert.NotEmpty(enums);
        List<string> failures = [];
        foreach (string name in records)
        {
            // A record is the image its [StoredRecord] binding names.
            RecordTypeInfo info = RecordCatalog.Find(name) ?? throw new InvalidOperationException("The record " + name + " is not in the catalog.");
            Type type = info.Image;
            if (RecordTypeInfo.StoredName(type) != name)
            {
                failures.Add("record " + name + " is bound to " + type.FullName + ", which stores " + RecordTypeInfo.StoredName(type));
            }

            foreach (RecordFieldInfo field in info.Fields)
            {
                if (Member(type, field.Name) is null)
                {
                    failures.Add("record " + name + " stores " + field.Name + ", which is no member of " + type.FullName);
                }
            }
        }
        foreach (string name in enums)
        {
            Type type = NameTable.Find(name);
            Assert.Equal(actual: EnumTypeInfo.Describe(type).ByName.Keys.Order(StringComparer.Ordinal).ToArray(), expected: [.. Enum.GetNames(type).Order(StringComparer.Ordinal)]);
        }
        output.WriteLine(records.Count + " records and " + enums.Count + " enums checked");
        Assert.True(failures.Count == 0, string.Join("\n", failures));
    }

    // Issue 158 part 9. A renamed node keeps the stored name the schema already uses.
    [Fact]
    public void ARenamedNodeKeepsItsStoredName()
    {
        (Type Type, string Stored)[] pinned =
        [
            (typeof(CustomNode), "ECustomNode"),
            (typeof(OwningNode), "ENode"),
            (typeof(EngineReferenceNode), "EReferenceNode"),
            (typeof(ObservableNode), "EChangeNotifyNode"),
            (typeof(ObservableEventList), "EChangeNotifyEventList"),
            (typeof(SortedNode), "ESortNode"),
            (typeof(InverseSortedNode), "EInverseSortNode"),
            (typeof(EngineNode), "ECustomOwner"),
            (typeof(ControlStyleNode), "EControlStyleNode"),
            (typeof(ControlStyleInverseSortedNode), "EControlStyleInverseSortNode"),
            (typeof(ResourceCollection), "EResourceList"),
            (typeof(PlayerCollection), "PlayerList"),
        ];
        foreach ((Type? type, string? stored) in pinned)
        {
            Assert.Equal(stored, NameTable.Stored(type));
            Assert.NotEqual(type.Name, stored);
            Assert.Equal(stored, NameTable.ResolveStored(NameTable.Delphi(type)));
            Assert.Equal(stored, NameTable.ResolveStored(stored));
            Assert.NotNull(FormatSchema.Current.Find(stored));
        }
        Assert.Equal("ECustomNode.ChildrenAccessor", NameTable.Stored(typeof(CustomNode.ChildrenAccessor)));
        Assert.Equal("ESortNode.SortItem", NameTable.Stored(typeof(SortedNode.SortItem)));
        Assert.Equal("ReferenceNode", NameTable.Stored(typeof(Formats.Tree.ReferenceNode)));
        Assert.Equal(typeof(EngineReferenceNode), NameTable.Find("EReferenceNode"));
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
        if (MemberNames.Field(type, name) is { } field)
        {
            return field;
        }

        if (MemberNames.Field(type, "_" + char.ToLowerInvariant(name[0]) + name[1..]) is { } privateField)
        {
            return privateField;
        }

        for (Type? current = type; current is not null; current = current.BaseType)
        {
            if (current.GetProperty(name, Instance | BindingFlags.DeclaredOnly) is { } property)
            {
                return property;
            }
        }

        return null;
    }

    // Gives each integer field that a schema member names a value no other one has, from first on,
    // and returns the values by stored name. The members of skip keep their values.
    internal static List<(string Name, long Value)> Fill(EngineObject objectValue, SchemaClass classValue, int first = 11, IReadOnlySet<string>? skip = null)
    {
        List<(string, long)> filled = [];
        int next = first;
        foreach (SchemaProperty property in classValue.Properties)
        {
            if (skip?.Contains(property.Name) == true)
            {
                continue;
            }

            if (Member(objectValue.GetType(), property.Name) is not FieldInfo field || field.IsInitOnly)
            {
                continue;
            }

            object? value = Type.GetTypeCode(field.FieldType) switch
            {
                TypeCode.Int32 => next,
                TypeCode.UInt16 => (ushort)next,
                TypeCode.Int16 => (short)next,
                TypeCode.Byte => (byte)next,
                TypeCode.SByte => (sbyte)next,
                TypeCode.Empty or TypeCode.Object or TypeCode.DBNull or TypeCode.Boolean or TypeCode.Char
                    or TypeCode.UInt32 or TypeCode.Int64 or TypeCode.UInt64 or TypeCode.Single
                    or TypeCode.Double or TypeCode.Decimal or TypeCode.DateTime or TypeCode.String or _ => null,
            };
            if (value is null || field.FieldType.IsEnum)
            {
                continue;
            }

            field.SetValue(objectValue, value);
            filled.Add((property.Name, next));
            next++;
            if (next > first + 109)
            {
                break;
            }
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
        EnginePersistableFactory factory = new(EngineFixture.Engine);
        ObjectNode root = TreeWriter.Write(objectValue, factory: factory);
        FormatSchema schema = FormatSchema.Current;
        SchemaClass classValue = schema.Get(factory.ClassName(objectValue));
        return OriginalBackend.Write(root, classValue: classValue, schema: schema, engine: GameBoundary.PacketClassTable(classValue));
    }

    private static JsonObject SchemaSource()
    {
        using Stream stream = typeof(FormatSchema).Assembly.GetManifestResourceStream("Aow.Formats.schema.json")!;
        using MemoryStream memory = new();
        stream.CopyTo(memory);
        return (JsonObject)JsonReader.Parse(memory.ToArray(), "schema.json");
    }

    private static void Collect(JsonValue value, SortedSet<string> records, SortedSet<string> enums)
    {
        switch (value)
        {
            case JsonObject objectValue:
                foreach (JsonMember member in objectValue.Members)
                {
                    if (member.Value is JsonString { Value: var name })
                    {
                        if (member.Name == "record")
                        {
                            _ = records.Add(name);
                        }
                        else if (member.Name is "enum" or "set")
                        {
                            _ = enums.Add(name);
                        }
                    }
                    Collect(member.Value, records, enums);
                }
                break;
            case JsonArray array:
                foreach (JsonValue item in array.Items)
                {
                    Collect(item, records, enums);
                }

                break;
            default:
                break;
        }
    }
}
