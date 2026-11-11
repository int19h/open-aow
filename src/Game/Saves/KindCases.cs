#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>
/// The case mapping of a kind family (own-formats.md 2.3 and 3.1). The schema's case table names
/// the cases by their registration keys. This rule gives the converter what the game's kind of a
/// key writes back, through the family's FromFields and ToFields, so a document never keeps a
/// case field the kind drops. It checks nothing in a document beyond what the codec checks.
/// </summary>
public abstract class KindCases : ContentRule, ICaseUnit
{
    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocumentNodePath path) => [];

    public override void Attach(string persistedName) => CaseMappings.Register(persistedName, this);

    public abstract IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored);
}
