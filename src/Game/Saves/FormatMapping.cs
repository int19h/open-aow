#nullable enable
using Aow.Formats;

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
