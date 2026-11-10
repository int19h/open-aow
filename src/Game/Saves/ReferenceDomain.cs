#nullable enable
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>A domain of identities a stored value refers to (own-formats.md 2.4).</summary>
public abstract class ReferenceDomain
{
    public abstract string Name { get; }

    /// <summary>Where the identities live below the document root, for the diagnostic.</summary>
    public abstract string Owner { get; }

    public abstract IReadOnlySet<long> Collect(JsonObject root);
}
