namespace Aow.Formats;

/// <summary>
/// A document kind (own-formats.md 1.1 and 2.9): the $kind name, the extension of its original
/// files and the extension of its own documents, an "o" before the original one. A profile holds
/// several kinds with different extensions (world-hsm holds saves, maps and campaign maps), so
/// both extensions belong to the kind. An extension only selects candidates, the content decides
/// what a file holds.
/// </summary>
public sealed class DocumentKind
{
    private DocumentKind(string name, string originalExtension)
    {
        Name = name;
        OriginalExtension = originalExtension;
        OwnExtension = ".o" + originalExtension[1..];
    }

    /// <summary>The $kind name.</summary>
    public string Name { get; }

    /// <summary>The extension of the original files, with its dot, in lower case.</summary>
    public string OriginalExtension { get; }

    /// <summary>The extension the port gives its own documents of this kind.</summary>
    public string OwnExtension { get; }

    public static DocumentKind Save { get; } = new("save", ".asg");
    public static DocumentKind Map { get; } = new("map", ".hsm");
    public static DocumentKind CampaignMap { get; } = new("campaign-map", ".csm");
    public static DocumentKind Campaign { get; } = new("campaign", ".cam");
    public static DocumentKind CampaignSave { get; } = new("campaign-save", ".acg");
    public static DocumentKind ScenarioSet { get; } = new("scenario-set", ".hss");
    public static DocumentKind HeroList { get; } = new("hero-list", ".ahl");
    public static DocumentKind ItemList { get; } = new("item-list", ".ail");

    public static IReadOnlyList<DocumentKind> All { get; } = [Save, Map, CampaignMap, Campaign, CampaignSave, ScenarioSet, HeroList, ItemList];

    public static DocumentKind? Find(string name) => All.FirstOrDefault(k => k.Name == name);

    /// <summary>The kind whose original or own extension this is, without regard to case.</summary>
    public static DocumentKind? ForExtension(string? extension) =>
        All.FirstOrDefault(k => k.HasExtension(extension));

    /// <summary>The kind whose original extension this is, without regard to case.</summary>
    public static DocumentKind? ForOriginalExtension(string? extension) =>
        All.FirstOrDefault(k => string.Equals(extension, k.OriginalExtension, StringComparison.OrdinalIgnoreCase));

    /// <summary>Whether the extension is this kind's original or own extension.</summary>
    public bool HasExtension(string? extension, StringComparison comparison = StringComparison.OrdinalIgnoreCase) =>
        string.Equals(extension, OriginalExtension, comparison) || string.Equals(extension, OwnExtension, comparison);

    /// <summary>Whether the extension is this kind's own extension, without regard to case.</summary>
    public bool IsOwnExtension(string? extension) => string.Equals(extension, OwnExtension, StringComparison.OrdinalIgnoreCase);

    /// <summary>The name with this kind's own extension in place of its extension.</summary>
    public string OwnName(string name) => ChangeExtension(name, OwnExtension);

    /// <summary>The name with this kind's original extension in place of its extension.</summary>
    public string OriginalName(string name) => ChangeExtension(name, OriginalExtension);

    /// <summary>Replaces the extension after the last separator, or appends one to a name without it.</summary>
    private static string ChangeExtension(string name, string extension)
    {
        int separator = name.LastIndexOfAny(['\\', '/', ':']);
        int dot = name.LastIndexOf('.');
        return (dot > separator ? name[..dot] : name) + extension;
    }

    public override string ToString() => Name;
}
