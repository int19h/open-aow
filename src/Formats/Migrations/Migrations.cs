using Aow.Formats.Json;
using Aow.Rtl;

namespace Aow.Formats.Migrations;

/// <summary>
/// One pure tree transform from version From to From + 1 (own-formats.md 2.6). A migration
/// creates no game object, executes no rule and reads no clock, file or mod assembly. Values it
/// synthesizes carry its name as their origin, so diagnostics can name it.
/// </summary>
public sealed class Migration(int from, string name, Func<JsonObject, JsonObject> transform)
{
    public int From { get; } = from;
    public string Name { get; } = name;

    public JsonObject Apply(JsonObject document) => transform(document);
}

/// <summary>The historical decoder contracts: the current version and every step up to it.</summary>
public sealed class MigrationChain
{
    private readonly Dictionary<int, Migration> _steps = [];

    public MigrationChain(int current, IEnumerable<Migration> migrations)
    {
        Current = current;
        foreach (var m in migrations)
        {
            if (m.From < 1 || m.From >= current) throw new ArgumentException($"Migration {m.Name} starts outside 1 to {current - 1}.");
            if (!_steps.TryAdd(m.From, m)) throw new ArgumentException($"Two migrations start at version {m.From}.");
        }
        for (int v = 1; v < current; v++)
            if (!_steps.ContainsKey(v)) throw new ArgumentException($"No migration starts at version {v}.");
    }

    public int Current { get; }

    /// <summary>
    /// Version 5 is the current format. Version 1 reaches it through the symbolic step, the record
    /// step, the list step and the resource step.
    /// </summary>
    public static MigrationChain Default { get; } = new(OwnFormat.CurrentVersion, [SymbolicMigration.Step, RecordMigration.Step, ListMigration.Step, ResourceMigration.Step]);

    /// <summary>Migrates a parsed document from its declared version to the current one.</summary>
    public JsonObject Migrate(JsonObject document, int version, List<string> applied)
    {
        for (int v = version; v < Current; v++)
        {
            var step = _steps[v];
            document = step.Apply(document);
            Mark(document, step.Name);
            var versionMember = document.Find("$version")!;
            versionMember.Value = new JsonInteger(v + 1) { Span = versionMember.Value.Span, Origin = step.Name };
            applied.Add(step.Name);
        }
        if (version < Current)
        {
            MemberNameMigration.Apply(document);
            StoreCurrentClassNames(document);
            RecordMigration.RefreshFingerprints(document);
        }
        else
        {
            // A current document from before a property rename keeps a valid fingerprint valid.
            RecordMigration.KeepFingerprints(document, () => MemberNameMigration.Apply(document));
        }
        return document;
    }

    // A version 1 or 2 document stores the Delphi class name. The current document stores the current name.
    private static void StoreCurrentClassNames(JsonValue value)
    {
        switch (value)
        {
            case JsonObject objectValue:
                if (objectValue.Find("$class") is { Value: JsonString name } member)
                {
                    string? stored = StoredClass(name.Value);
                    if (stored is not null && stored != name.Value)
                        member.Value = new JsonString(stored) { Span = name.Span, Origin = name.Origin };
                }
                foreach (var child in objectValue.Members) StoreCurrentClassNames(child.Value);
                break;
            case JsonArray array:
                foreach (var item in array.Items) StoreCurrentClassNames(item);
                break;
        }
    }

    private static string? StoredClass(string name)
    {
        try { return NameTable.ResolveStored(name); }
        catch (InvalidOperationException) { return null; }
    }

    // Values without a source span were synthesized by the step.
    private static void Mark(JsonValue value, string migration)
    {
        if (!value.Span.IsKnown && value.Origin is null) value.Origin = migration;
        switch (value)
        {
            case JsonObject objectValue:
                foreach (var m in objectValue.Members) Mark(m.Value, migration);
                break;
            case JsonArray array:
                foreach (var item in array.Items) Mark(item, migration);
                break;
        }
    }
}
