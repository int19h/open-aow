#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>Exactly as many children of a family as an integer member counts.</summary>
public sealed class CountedChildren(string family, string countMember) : ContentRule
{
    public override IEnumerable<string> Members => ["Children", countMember];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocumentNodePath path)
    {
        long expected = value[countMember] is JsonInteger count ? count.Value : 0;
        int actual = ContentChildren.Count(scope, value, family);
        if (actual != expected)
            yield return scope.Context.At(Codes.Required, $"{countMember} counts {expected} {family} children, the document holds {actual}.",
                value[countMember] ?? value, path.Member(countMember), actual.ToString(System.Globalization.CultureInfo.InvariantCulture), $"{expected} {family} children");
    }
}
