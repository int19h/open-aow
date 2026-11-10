#nullable enable
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Schema;
using Aow.Game.Session;
using Aow.Game.World.Structures;

namespace Aow.Game.Saves;

/// <summary>
/// The unit side of the own formats (own-formats.md 2.7 and 3.1). A mapping names a persisted
/// class, never a C# type, and carries the data-only rules that need knowledge of the rules
/// code: which values refer to which identities and which children the game requires.
/// ClassRegistry registers one mapping per class in its third block. Rules read the document
/// and nothing else, create no game object and never repair anything.
/// </summary>
public static class FormatMapping
{
    private static readonly Dictionary<string, ContentRule[]> mappings = [];

    /// <summary>Registers the rules of one persisted class. A class registers once.</summary>
    public static void Register(string persistedName, params ContentRule[] rules)
    {
        if (!mappings.TryAdd(persistedName, rules))
            throw new InvalidOperationException("The persisted class " + persistedName + " has two format mappings.");
        foreach (var rule in rules) rule.Attach(persistedName);
    }

    /// <summary>The registered mappings by persisted class name.</summary>
    public static IReadOnlyDictionary<string, ContentRule[]> All => mappings;

    /// <summary>The game validators the file boundary runs on every load and save.</summary>
    public static IReadOnlyList<IDocumentValidator> Validators { get; } = [new GameContentValidator()];
}

/// <summary>One data-only rule of a mapped class. Rules form an open family.</summary>
public abstract class ContentRule
{
    /// <summary>The members the rule reads, for the registry's completeness test.</summary>
    public abstract IEnumerable<string> Members { get; }

    public abstract IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocPath path);

    /// <summary>Called once when the rule is registered for its persisted class.</summary>
    public virtual void Attach(string persistedName)
    {
    }
}

/// <summary>
/// The case mapping of a kind family (own-formats.md 2.3 and 3.1). The schema's case table names
/// the cases by their registration keys. This rule gives the converter what the game's kind of a
/// key writes back, through the family's FromFields and ToFields, so a document never keeps a
/// case field the kind drops. It checks nothing in a document beyond what the codec checks.
/// </summary>
public abstract class KindCases : ContentRule, ICaseUnit
{
    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocPath path) => [];

    public override void Attach(string persistedName) => CaseMappings.Register(persistedName, this);

    public abstract IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored);
}

/// <summary>TProductionItem: the 9.6-6 production orders, keyed by ProductionID.</summary>
public sealed class ProductionOrderCases : KindCases
{
    public override IEnumerable<string> Members => ["ProductionID", "Parameter", "GroupID"];

    public override IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored)
    {
        var order = Aow.Game.World.Structures.ProductionOrder.FromFields((int)key, (int)stored.GetValueOrDefault("Parameter"), (int)stored.GetValueOrDefault("GroupID"));
        var (id, parameter, groupId) = order.ToFields();
        return new Dictionary<string, long> { ["ProductionID"] = id, ["Parameter"] = parameter, ["GroupID"] = groupId };
    }
}

/// <summary>TDiplomaticAction: the 9.6-7 diplomatic proposals, keyed by the one-byte ActionType.</summary>
public sealed class DiplomaticProposalCases : KindCases
{
    public override IEnumerable<string> Members => ["ActionType", "Gold", "Mana"];

    public override IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored)
    {
        var proposal = Aow.Game.Session.DiplomaticProposal.FromFields((byte)key, (int)stored.GetValueOrDefault("Gold"), (int)stored.GetValueOrDefault("Mana"));
        var (kind, gold, mana) = proposal.ToFields();
        return new Dictionary<string, long> { ["ActionType"] = kind, ["Gold"] = gold, ["Mana"] = mana };
    }
}

/// <summary>
/// What a rule sees: the validation context and the identity domains of the document, each
/// collected once.
/// </summary>
public sealed class ContentScope(ValidationContext context, JsonObject root, DocPath rootPath)
{
    private readonly Dictionary<ReferenceDomain, IReadOnlySet<long>> domains = [];

    public ValidationContext Context { get; } = context;

    /// <summary>
    /// The root of the map document the checked values belong to: the document's own root, or
    /// the root of a nested map such as a campaign save's Map, whose references never reach the
    /// outer document.
    /// </summary>
    public JsonObject Root { get; } = root;

    public DocPath RootPath { get; } = rootPath;

    public IReadOnlySet<long> Identities(ReferenceDomain domain)
    {
        if (!domains.TryGetValue(domain, out var identities)) domains[domain] = identities = domain.Collect(Root);
        return identities;
    }
}

/// <summary>A domain of identities a stored value refers to (own-formats.md 2.4).</summary>
public abstract class ReferenceDomain
{
    public abstract string Name { get; }

    /// <summary>Where the identities live below the document root, for the diagnostic.</summary>
    public abstract string Owner { get; }

    public abstract IReadOnlySet<long> Collect(JsonObject root);
}

/// <summary>The players of a world map: the stored slots of root.Players.Children.</summary>
public sealed class PlayerSlots : ReferenceDomain
{
    public static PlayerSlots Instance { get; } = new();
    public override string Name => "player-slot";
    public override string Owner => "Players.Children";

    public override IReadOnlySet<long> Collect(JsonObject root)
    {
        var slots = new HashSet<long>();
        if (root["Players"] is JsonObject players && players["Children"] is JsonArray children)
            foreach (var child in children.Items)
                if (child is JsonObject { } player && player["$slot"] is JsonInteger slot) slots.Add(slot.Value);
        return slots;
    }
}

/// <summary>An integer member that names an identity of a domain. Absence is the reader default and refers to nothing.</summary>
public sealed class Reference(string member, ReferenceDomain domain) : ContentRule
{
    public override IEnumerable<string> Members => [member];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocPath path)
    {
        if (value[member] is not JsonInteger target) yield break;
        if (!scope.Identities(domain).Contains(target.Value))
            yield return Dangling(scope, target, path.Member(member), domain);
    }

    internal static FormatDiagnostic Dangling(ContentScope scope, JsonInteger target, DocPath at, ReferenceDomain domain) =>
        scope.Context.At(Codes.Reference, $"The {domain.Name} {target.Value} is absent from {scope.RootPath}.{domain.Owner}. Use an existing one.", target, at,
            target.Value.ToString(System.Globalization.CultureInfo.InvariantCulture), $"{domain.Name} in {scope.RootPath}.{domain.Owner}");
}

/// <summary>A list member ({ "default", "items" }) whose items name identities of a domain.</summary>
public sealed class ReferenceList(string member, ReferenceDomain domain) : ContentRule
{
    public override IEnumerable<string> Members => [member];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocPath path)
    {
        if (value[member] is not JsonObject list || list["items"] is not JsonArray items) yield break;
        for (int i = 0; i < items.Items.Count; i++)
            if (items.Items[i] is JsonInteger target && !scope.Identities(domain).Contains(target.Value))
                yield return Reference.Dangling(scope, target, path.Member(member).Member("items").Item(i), domain);
    }
}

/// <summary>A child of a family the game requires, at least one stored child of that family.</summary>
public sealed class RequiredChild(string family) : ContentRule
{
    public override IEnumerable<string> Members => ["Children"];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocPath path)
    {
        if (ContentChildren.Count(scope, value, family) == 0)
            yield return scope.Context.At(Codes.Required, $"The game requires a {family} child here.", value, path, null, "one " + family + " child");
    }
}

/// <summary>Exactly as many children of a family as an integer member counts.</summary>
public sealed class CountedChildren(string family, string countMember) : ContentRule
{
    public override IEnumerable<string> Members => ["Children", countMember];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocPath path)
    {
        long expected = value[countMember] is JsonInteger count ? count.Value : 0;
        int actual = ContentChildren.Count(scope, value, family);
        if (actual != expected)
            yield return scope.Context.At(Codes.Required, $"{countMember} counts {expected} {family} children, the document holds {actual}.",
                value[countMember] ?? value, path.Member(countMember), actual.ToString(System.Globalization.CultureInfo.InvariantCulture), $"{expected} {family} children");
    }
}

internal static class ContentChildren
{
    public static int Count(ContentScope scope, JsonObject value, string family)
    {
        if (value["Children"] is not JsonArray children) return 0;
        int count = 0;
        foreach (var child in children.Items)
            if (child is JsonObject c && c["$class"] is JsonString name && GameFiles.Schema.Find(name.Value) is { } cls && cls.InheritsFrom(family)) count++;
        return count;
    }
}

/// <summary>
/// Walks a document and applies the rules registered for each object's class and its
/// ancestors. Objects without $class have their class fixed by schema context and carry no
/// mapped rules in version 1, apart from a root whose profile fixes its class.
/// </summary>
internal sealed class GameContentValidator : IDocumentValidator
{
    public string Name => "unit-content";

    public IEnumerable<FormatDiagnostic> Validate(ValidationContext context)
    {
        var root = context.Root;
        var scope = new ContentScope(context, root, DocPath.Root);
        var findings = new List<FormatDiagnostic>();
        var rootClass = root["$class"] is JsonString name ? GameFiles.Schema.Find(name.Value) : GameFiles.Schema.Find(context.Profile.RootFamily);
        Visit(scope, root, rootClass, DocPath.Root, findings);
        return findings;
    }

    private static void Visit(ContentScope scope, JsonValue value, SchemaClass? cls, DocPath path, List<FormatDiagnostic> findings)
    {
        switch (value)
        {
            case JsonObject obj:
                // A nested document (a campaign's Map) is a map of its own: its references
                // resolve inside its root.
                if (obj["$container"] is JsonObject && obj["root"] is JsonObject nested && obj["$class"] is null)
                {
                    Visit(new ContentScope(scope.Context, nested, path.Member("root")), nested, null, path.Member("root"), findings);
                    break;
                }
                if (obj["$class"] is JsonString name) cls = GameFiles.Schema.Find(name.Value);
                for (var c = cls; c is not null; c = c.Parent)
                    if (FormatMapping.All.TryGetValue(c.Name, out var rules))
                        foreach (var rule in rules) findings.AddRange(rule.Check(scope, obj, path));
                foreach (var member in obj.Members)
                    if (member.Value is JsonObject or JsonArray && member.Name != "Fields")
                        Visit(scope, member.Value, null, path.Member(member.Name), findings);
                break;
            case JsonArray array:
                for (int i = 0; i < array.Items.Count; i++)
                    if (array.Items[i] is JsonObject or JsonArray) Visit(scope, array.Items[i], null, path.Item(i), findings);
                break;
        }
    }
}
