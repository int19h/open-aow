using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Convert;

/// <summary>
/// The case mappings of TProductionItem and TDiplomaticAction (own-formats.md 2.3, 9.7-3): the
/// production kinds the census finds (1, 2, 11, 12, 13), the diplomatic kinds it finds (0, 1),
/// an unknown kind of each, a residue, and the refusals of a wrong case, a field the case does
/// not keep and a known key under the unknown case.
/// </summary>
public static partial class Fixtures
{
    private const string Save = "Save/After the Fall.asg";

    internal static IEnumerable<Case> KindCases()
    {
        yield return new("cases", "production orders 1, 2, 11, 12, 13 and an unknown kind keep their cases", () =>
            OwnRoundTrip(WithKinds(ProductionItems, DiplomaticActions), (normal, _) => CasesOf(normal, "ProductionItem") is var cases && cases == ProductionCases
                ? null : "production cases read back as " + cases));
        yield return new("cases", "diplomatic proposals 0, 1, a residue and an unknown kind keep their cases", () =>
            OwnRoundTrip(WithKinds(ProductionItems, DiplomaticActions), (normal, _) => CasesOf(normal, "DiplomaticAction") is var cases && cases == DiplomacyCases
                ? null : "diplomatic cases read back as " + cases));
        yield return new("cases", "an unknown case name is refused", () => Expect(Codes.Case,
            () => GameFiles.Export(Copy(WithKinds([Item(0, "Build", [])], []))), Located));
        yield return new("cases", "a field the case does not keep is refused", () => Expect(Codes.UnknownMember,
            () => GameFiles.Export(Copy(WithKinds([], [Action(0, "DeclareWar", [("Gold", new JsonInteger(5))])]))), Located));
        yield return new("cases", "a registered key under the unknown case is refused", () => Expect(Codes.Case,
            () => GameFiles.Export(Copy(WithKinds([Item(0, "Unknown", [("Kind", new JsonInteger(12))])], []))), Located));
    }

    private const string ProductionCases = "ProduceUnit{Index=3,GroupId=5} ResearchSpell{Index=2} Upgrade{} Fortify{} Migrate{Race=4} Unknown{Kind=99,Parameter=7,GroupId=1}";
    private const string DiplomacyCases = "DeclareWar{} OfferPeace{Gold=100,Mana=20} DeclareWar{$residue} Unknown{Kind=\"0c\",Gold=5}";

    private static JsonObject[] ProductionItems =>
    [
        Item(0, "ProduceUnit", [("Index", new JsonInteger(3)), ("GroupId", new JsonInteger(5))]),
        Item(1, "ResearchSpell", [("Index", new JsonInteger(2))]),
        Item(2, "Upgrade", []),
        Item(3, "Fortify", []),
        Item(4, "Migrate", [("Race", new JsonInteger(4))]),
        Item(5, "Unknown", [("Kind", new JsonInteger(99)), ("Parameter", new JsonInteger(7)), ("GroupId", new JsonInteger(1))]),
    ];

    private static JsonObject[] DiplomaticActions =>
    [
        Action(0, "DeclareWar", []),
        Action(1, "OfferPeace", [("Gold", new JsonInteger(100)), ("Mana", new JsonInteger(20))]),
        Action(2, "DeclareWar", [], new JsonObject().Add("Gold", new JsonInteger(5))),
        Action(3, "Unknown", [("Kind", new JsonString("0c")), ("Gold", new JsonInteger(5))]),
    ];

    private static JsonObject Item(int slot, string kind, (string, JsonValue)[] fields) => Mapped("ProductionItem", slot, kind, fields, null);

    private static JsonObject Action(int slot, string kind, (string, JsonValue)[] fields, JsonObject? residue = null)
    {
        var action = Mapped("DiplomaticAction", slot, kind, fields, residue);
        action.Add("Source", new JsonInteger(1)).Add("Target", new JsonInteger(2)).Add("OtherPlayer", new JsonInteger(255));
        return action;
    }

    private static JsonObject Mapped(string classValue, int slot, string kind, (string, JsonValue)[] fields, JsonObject? residue)
    {
        var objectValue = new JsonObject().Add("$class", new JsonString(classValue)).Add("$case", new JsonString(kind)).Add("$slot", new JsonInteger(slot));
        if (residue is not null) objectValue.Add("$residue", residue);
        foreach (var (name, value) in fields) objectValue.Add(name, value);
        return objectValue;
    }

    /// <summary>The shipped save with the items in its first empty production queue and the actions in player 1's list.</summary>
    private static JsonObject WithKinds(JsonObject[] items, JsonObject[] actions)
    {
        var document = Import(Game(Save), "world-hsm", "save");
        if (items.Length > 0)
        {
            var queue = FirstQueue(Root(document)) ?? throw new InvalidOperationException("The save has no production queue.");
            foreach (var item in items) Arr(queue["Children"]).Add(item);
        }
        if (actions.Length > 0)
        {
            var player = Object(Arr(Object(Root(document)["Players"])["Children"]).Items[1]);
            foreach (var action in actions) Arr(Object(player["DiplomaticActions"])["Children"]).Add(action);
        }
        return document;
    }

    private static JsonObject? FirstQueue(JsonValue value)
    {
        switch (value)
        {
            case JsonObject objectValue:
                if (objectValue["Queue"] is JsonObject queue && queue["Children"] is JsonArray { Items.Count: 0 }) return queue;
                foreach (var m in objectValue.Members)
                    if (FirstQueue(m.Value) is { } found) return found;
                break;
            case JsonArray array:
                foreach (var item in array.Items)
                    if (FirstQueue(item) is { } found) return found;
                break;
        }
        return null;
    }

    /// <summary>The cases of every object of a class, in document order, with their fields.</summary>
    private static string CasesOf(JsonValue value, string classValue)
    {
        var found = new List<string>();
        void Walk(JsonValue v)
        {
            switch (v)
            {
                case JsonObject objectValue:
                    if (objectValue["$class"] is JsonString { } name && name.Value == classValue && objectValue["$case"] is JsonString kind)
                    {
                        var fields = objectValue.Members.Where(m => !m.Name.StartsWith('$') && m.Name is not ("Source" or "Target" or "OtherPlayer"))
                            .Select(m => m.Name + "=" + (m.Value is JsonInteger i ? i.Value.ToString(System.Globalization.CultureInfo.InvariantCulture) : "\"" + ((JsonString)m.Value).Value + "\""));
                        found.Add(kind.Value + "{" + (objectValue["$residue"] is null ? string.Join(",", fields) : "$residue") + "}");
                    }
                    foreach (var m in objectValue.Members) Walk(m.Value);
                    break;
                case JsonArray array:
                    foreach (var item in array.Items) Walk(item);
                    break;
            }
        }
        Walk(value);
        return string.Join(" ", found);
    }
}
