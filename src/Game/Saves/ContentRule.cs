#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

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
