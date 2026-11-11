#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>A child of a family the game requires, at least one stored child of that family.</summary>
public sealed class RequiredChild(string family) : ContentRule
{
    public override IEnumerable<string> Members => ["Children"];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocumentNodePath path)
    {
        if (ContentChildren.Count(scope, value, family) == 0)
            yield return scope.Context.At(Codes.Required, $"The game requires a {family} child here.", value, path, null, "one " + family + " child");
    }
}
