using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Convert;

/// <summary>
/// The case mappings of TProductionItem and TDiplomaticAction (own-formats.md 2.3, 9.7-3): the
/// production kinds the census finds (1, 2, 11, 12, 13), the diplomatic kinds it finds (0, 1),
/// an unknown kind of each, the refusals of a wrong case, a field the case does not keep,
/// $residue and a known key under the unknown case, plus the constant-0 load rules for unused
/// stored members.
/// </summary>
public static partial class Fixtures
{
    private const string Save = "Save/After the Fall.asg";

    internal static IEnumerable<Case> KindCases()
    {
        yield return new("cases", Name: "production orders 1, 2, 11, 12, 13 and an unknown kind keep their cases", () =>
            OwnRoundTrip(WithKinds(ProductionItems, actions: DiplomaticActions), (normal, _) => CasesOf(normal, classValue: "ProductionItem") is var cases && cases == ProductionCases
                ? null : $"production cases read back as {cases}"));
        yield return new("cases", Name: "diplomatic proposals 0, 1 and an unknown kind keep their cases", () =>
            OwnRoundTrip(WithKinds(ProductionItems, actions: DiplomaticActions), (normal, _) => CasesOf(normal, classValue: "DiplomaticAction") is var cases && cases == DiplomacyCases
                ? null : $"diplomatic cases read back as {cases}"));
        yield return new("cases", Name: "an unknown case name is refused", () => Expect(Codes.Case,
            action: () => GameFiles.Export(Copy(WithKinds([Item(0, kind: "Build", fields: [])], actions: []))), inspect: Located));
        yield return new("cases", Name: "a field the case does not keep is refused", () => Expect(Codes.UnknownMember,
            action: () => GameFiles.Export(Copy(WithKinds([], actions: [Action(0, kind: "DeclareWar", fields: [("Gold", new JsonInteger(5))])]))), inspect: Located));
        yield return new("cases", Name: "a $residue member on a mapped case is refused", () => Expect(Codes.ReservedMember,
            action: () => GameFiles.Export(Copy(WithKinds([], actions: [Action(0, kind: "DeclareWar", fields: [], residue: new JsonObject().Add("Gold", new JsonInteger(5)))]))), inspect: Located));
        yield return new("cases", Name: "a registered key under the unknown case is refused", () => Expect(Codes.Case,
            action: () => GameFiles.Export(Copy(WithKinds([Item(0, kind: "Unknown", fields: [("Kind", new JsonInteger(12))])], actions: []))), inspect: Located));
        yield return new("cases", Name: "nonzero unused Gold on DeclareWar is refused on import", () => Expect(Codes.Range,
            action: () => Import(ExportWithRawMember(Action(0, kind: "Unknown", fields: [("Kind", new JsonString("0c")), ("Gold", new JsonInteger(0x12345678))]), fromByte: 0x0C, toByte: 0x00), profile: "world-hsm", kind: "save"), inspect: Located));
        yield return new("cases", Name: "nonzero unused GroupId on ResearchSpell is refused on import", () => Expect(Codes.Range,
            action: () => Import(ExportWithRawProductionKind(Item(0, kind: "Unknown", fields: [("Kind", new JsonInteger(0x12345678)), ("Parameter", new JsonInteger(2)), ("GroupId", new JsonInteger(5))]), newKind: 2), profile: "world-hsm", kind: "save"), inspect: Located));
        yield return new("cases", Name: "nonzero unused Parameter on Upgrade is ignored on import", () =>
        {
            byte[] image = ExportWithRawProductionKind(Item(0, kind: "Unknown", fields: [("Kind", new JsonInteger(0x12345678)), ("Parameter", new JsonInteger(7))]), newKind: 11);
            JsonObject imported = Import(image, profile: "world-hsm", kind: "save");
            string cases = CasesOf(imported, classValue: "ProductionItem");
            return cases == "Upgrade{}" ? Pass("ignored and omitted") : Fail($"read back as {cases}");
        });
    }

    private const string ProductionCases = "ProduceUnit{Index=3,GroupId=5} ResearchSpell{Index=2} Upgrade{} Fortify{} Migrate{Race=4} Unknown{Kind=99,Parameter=7,GroupId=1}";
    private const string DiplomacyCases = "DeclareWar{} OfferPeace{Gold=100,Mana=20} Unknown{Kind=\"0c\",Gold=5}";

    private static JsonObject[] ProductionItems
    => [
        Item(0, kind: "ProduceUnit", fields: [("Index", new JsonInteger(3)), ("GroupId", new JsonInteger(5))]),
        Item(1, kind: "ResearchSpell", fields: [("Index", new JsonInteger(2))]),
        Item(2, kind: "Upgrade", fields: []),
        Item(3, kind: "Fortify", fields: []),
        Item(4, kind: "Migrate", fields: [("Race", new JsonInteger(4))]),
        Item(5, kind: "Unknown", fields: [("Kind", new JsonInteger(99)), ("Parameter", new JsonInteger(7)), ("GroupId", new JsonInteger(1))]),
    ];

    private static JsonObject[] DiplomaticActions
    => [
        Action(0, kind: "DeclareWar", fields: []),
        Action(1, kind: "OfferPeace", fields: [("Gold", new JsonInteger(100)), ("Mana", new JsonInteger(20))]),
        Action(2, kind: "Unknown", fields: [("Kind", new JsonString("0c")), ("Gold", new JsonInteger(5))]),
    ];

    private static byte[] ExportWithRawMember(JsonObject action, byte fromByte, byte toByte)
    {
        byte[] raw = Payload(GameFiles.Export(WithKinds([], actions: [action])).Bytes);
        // Locate the DiplomaticAction entry: property 25 (Kind) with byte fromByte after Gold = 0x12345678.
        byte[] pattern = [0x78, 0x56, 0x34, 0x12, fromByte];
        int at = raw.AsSpan().IndexOf(pattern);
        if (at < 0 || raw.AsSpan().LastIndexOf(pattern) != at)
        {
            throw new InvalidOperationException("The raw diplomatic action bytes were not uniquely found.");
        }

        raw[at + 4] = toByte;
        return raw;
    }

    private static byte[] ExportWithRawProductionKind(JsonObject item, int newKind)
    {
        byte[] raw = Payload(GameFiles.Export(WithKinds([item], actions: [])).Bytes);
        byte[] pattern = [0x78, 0x56, 0x34, 0x12];
        int at = raw.AsSpan().IndexOf(pattern);
        if (at < 0 || raw.AsSpan().LastIndexOf(pattern) != at)
        {
            throw new InvalidOperationException("The raw production item bytes were not uniquely found.");
        }

        System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(raw.AsSpan(at, length: 4), value: newKind);
        return raw;
    }

    private static JsonObject Item(int slot, string kind, (string, JsonValue)[] fields) => Mapped("ProductionItem", slot, kind, fields, residue: null);

    private static JsonObject Action(int slot, string kind, (string, JsonValue)[] fields, JsonObject? residue = null)
    {
        JsonObject action = Mapped("DiplomaticAction", slot, kind, fields, residue);
        _ = action.Add("Source", new JsonInteger(1)).Add("Target", new JsonInteger(2)).Add("OtherPlayer", new JsonInteger(255));
        return action;
    }

    private static JsonObject Mapped(string classValue, int slot, string kind, (string, JsonValue)[] fields, JsonObject? residue)
    {
        JsonObject objectValue = new JsonObject().Add("$class", new JsonString(classValue)).Add("$case", new JsonString(kind)).Add("$slot", new JsonInteger(slot));
        if (residue is not null)
        {
            _ = objectValue.Add("$residue", residue);
        }

        foreach ((string? name, JsonValue? value) in fields)
        {
            _ = objectValue.Add(name, value);
        }

        return objectValue;
    }

    /// <summary>The shipped save with the items in its first empty production queue and the actions in player 1's list.</summary>
    private static JsonObject WithKinds(JsonObject[] items, JsonObject[] actions)
    {
        JsonObject document = Import(Game(Save), profile: "world-hsm", kind: "save");
        if (items.Length > 0)
        {
            JsonObject queue = FirstQueue(Root(document)) ?? throw new InvalidOperationException("The save has no production queue.");
            Arr(queue["Children"]).Items.AddRange(items);
        }
        if (actions.Length > 0)
        {
            JsonObject player = Object(Arr(Object(Root(document)["Players"])["Children"]).Items[1]);
            Arr(Object(player["DiplomaticActions"])["Children"]).Items.AddRange(actions);
        }
        return document;
    }

    private static JsonObject? FirstQueue(JsonValue value)
    {
        switch (value)
        {
            case JsonObject objectValue:
                if (objectValue["Queue"] is JsonObject queue && queue["Children"] is JsonArray { Items.Count: 0 })
                {
                    return queue;
                }

                return objectValue.Members.Select(static member => FirstQueue(member.Value)).FirstOrDefault(static found => found is not null);
            case JsonArray array:
                return array.Items.Select(FirstQueue).FirstOrDefault(static found => found is not null);
            default:
                break;
        }
        return null;
    }

    /// <summary>The cases of every object of a class, in document order, with their fields.</summary>
    private static string CasesOf(JsonValue value, string classValue)
    {
        List<string> found = [];
        void Walk(JsonValue v)
        {
            switch (v)
            {
                case JsonObject objectValue:
                    if (objectValue["$class"] is JsonString { } name && name.Value == classValue && objectValue["$case"] is JsonString kind)
                    {
                        IEnumerable<string> fields = objectValue.Members.Where(m => !m.Name.StartsWith('$') && m.Name is not ("Source" or "Target" or "OtherPlayer"))
                            .Select(m => $"{m.Name}={(m.Value is JsonInteger i ? i.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : $"\"{((JsonString)m.Value).Value}\"")}");
                        found.Add($"{kind.Value}{{{(objectValue["$residue"] is null ? string.Join(",", values: fields) : "$residue")}}}");
                    }
                    foreach (JsonMember m in objectValue.Members)
                    {
                        Walk(m.Value);
                    }

                    break;
                case JsonArray array:
                    foreach (JsonValue item in array.Items)
                    {
                        Walk(item);
                    }

                    break;
                default:
                    break;
            }
        }
        Walk(value);
        return string.Join(" ", values: found);
    }
}
