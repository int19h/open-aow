#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>
/// What a rule sees: the validation context and the identity domains of the document, each
/// collected once.
/// </summary>
public sealed class ContentScope(ValidationContext context, JsonObject root, DocumentNodePath rootPath)
{
    private readonly Dictionary<ReferenceDomain, IReadOnlySet<long>> _domains = [];

    public ValidationContext Context { get; } = context;

    /// <summary>
    /// The root of the map document the checked values belong to: the document's own root, or
    /// the root of a nested map such as a campaign save's Map, whose references never reach the
    /// outer document.
    /// </summary>
    public JsonObject Root { get; } = root;

    public DocumentNodePath RootPath { get; } = rootPath;

    public IReadOnlySet<long> Identities(ReferenceDomain domain)
    {
        if (!_domains.TryGetValue(domain, out var identities)) _domains[domain] = identities = domain.Collect(Root);
        return identities;
    }
}
