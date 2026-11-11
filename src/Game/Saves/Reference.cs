#nullable enable
using Aow.Formats;
using Aow.Formats.Json;

namespace Aow.Game.Saves;

/// <summary>An integer member that names an identity of a domain. Absence is the reader default and refers to nothing.</summary>
public sealed class Reference(string member, ReferenceDomain domain) : ContentRule
{
    public override IEnumerable<string> Members => [member];

    public override IEnumerable<FormatDiagnostic> Check(ContentScope scope, JsonObject value, DocumentNodePath path)
    {
        if (value[member] is not JsonInteger target) yield break;
        if (!scope.Identities(domain).Contains(target.Value))
            yield return Dangling(scope, target, path.Member(member), domain);
    }

    internal static FormatDiagnostic Dangling(ContentScope scope, JsonInteger target, DocumentNodePath at, ReferenceDomain domain) =>
        scope.Context.At(Codes.Reference, $"The {domain.Name} {target.Value} is absent from {scope.RootPath}.{domain.Owner}. Use an existing one.", target, at,
            target.Value.ToString(System.Globalization.CultureInfo.InvariantCulture), $"{domain.Name} in {scope.RootPath}.{domain.Owner}");
}
